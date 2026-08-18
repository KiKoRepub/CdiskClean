using System.Collections.Concurrent;
using System.Diagnostics;
using CdiskClean.Helpers;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace CdiskClean.Services;

/// <summary>ETW 白名单/黑名单的列表操作类型</summary>
public enum EtwListOperation
{
    Add = 1,
    Remove = 2
}

public class EtwMonitorService : IDisposable
{
    private TraceEventSession? _session;
    private CancellationTokenSource? _cts;
    /// <summary>
    /// 缓存起来的 所有进程的事件字典，监控触发的时候 会来这里面查询对应的进程名
    /// </summary>
    private readonly ConcurrentDictionary<string, (string ProcessName, DateTime Timestamp)> _eventBuffer = new(StringComparer.OrdinalIgnoreCase);
    private readonly TimeSpan _bufferTtl = TimeSpan.FromSeconds(3);
    private static readonly string CurrentProcessName = Process.GetCurrentProcess().ProcessName;
    // 定义一个原子引用（因为引用赋值是原子的，不需要锁）
    private volatile string[] _watchDirectoryArray = new string[0]; // 白名单
    private volatile string[] _ignoreProcessArray = new string[0]; // 黑名单

    
    public bool IsRunning { get; private set; }

    /// <summary>同步创建 ETW 会话，成功返回 true；事件处理在后台线程运行</summary>
    public bool Start()
    {
        if (IsRunning) return true;

        try
        {
            var sessionName = "CdiskCleanEtwSession";
            try { TraceEventSession.GetActiveSession(sessionName)?.Dispose(); } catch { }

            var session = new TraceEventSession(sessionName);
            session.EnableKernelProvider(KernelTraceEventParser.Keywords.FileIOInit);

            var source = session.Source;
            source.Kernel.FileIOCreate += d => BufferEvent(d.FileName, d.ProcessName);
            source.Kernel.FileIOWrite += d => BufferEvent(d.FileName, d.ProcessName);
            source.Kernel.FileIODelete += d => BufferEvent(d.FileName, d.ProcessName);

            _session = session;
            IsRunning = true;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Run(() =>
            {
                try
                {
                    using var cleanupTimer = new System.Timers.Timer(10000);
                    cleanupTimer.Elapsed += (_, _) => CleanupBuffer();
                    cleanupTimer.Start();

                    try
                    {
                        source.Process();
                    }
                    catch (OperationCanceledException) { }
                }
                catch (Exception) { }
                finally
                {
                    try { session.Dispose(); } catch { }
                    IsRunning = false;
                }
            }, token);

            return true;
        }
        catch (Exception)
        {
            IsRunning = false;
            return false;
        }
    }

    #region WatchDirectoryArray 相关操作
    public void OperateWriteFolderArr(string path, EtwListOperation operation)
    {
        // TODO 用户添加目录时(需要添加订阅,目前直接在目标位置进行调用)
        // 生成新数组，替换旧数组
        var list = _watchDirectoryArray.ToList();

        if (operation == EtwListOperation.Add)
            list.Add(path);
        if (operation == EtwListOperation.Remove)
            list.Remove(path);

        _watchDirectoryArray = list.Distinct().ToArray();
    }
    public void OperateWriteFolderArr(string[] paths)
    {
        _watchDirectoryArray = paths.ToArray();
    }
    #endregion

    #region IgnoreProcessArray 相关操作
    public void OperateIgnoreProcessArr(string processName, EtwListOperation operation)
    {
        var list = _ignoreProcessArray.ToList();
        if (operation == EtwListOperation.Add)
            list.Add(processName);
        if (operation == EtwListOperation.Remove)
            list.RemoveAll(p => string.Equals(p, processName, StringComparison.OrdinalIgnoreCase));
        _ignoreProcessArray = list.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }
    public void OperateIgnoreProcessArr(string[] processNames)
    {
        _ignoreProcessArray = processNames.ToArray();
    }
    #endregion

    /// <summary>
    /// 缓冲区 用来往字典里添加记录到的事件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="processName"></param>
    // 
    private void BufferEvent(string? fileName, string processName)
    {
        try
        {
            // 过滤空文件
            if (string.IsNullOrEmpty(fileName)) return;
            if (string.IsNullOrWhiteSpace(processName) ||
                string.Equals(processName, CurrentProcessName, StringComparison.OrdinalIgnoreCase))
                return;
            // 对于 白名单中的 目录路径进行选取

            var dirs = _watchDirectoryArray;

            foreach (var dir in dirs)
            {
                if (PathHelper.IsPathInside(fileName, dir))
                {
                    // 匹配成功
                    var normalized = NormalizePath(fileName);
                    _eventBuffer[normalized] = (processName, DateTime.Now);
                    break;
                }
            }
        }
        catch
        {
            // 忽略单条事件异常
        }
    }
    /// <summary>
    /// 快速单次查找，不重试，不移除缓冲。用于 FSW 事件触发时的即时查询。
    /// </summary>
    public string? TryGetProcessOnce(string filePath, DateTime? notBefore = null)
    {
        if (string.IsNullOrEmpty(filePath)) return null;

        var normalized = NormalizePath(filePath);
        if (_eventBuffer.TryGetValue(normalized, out var entry))
        {
            if (DateTime.Now - entry.Timestamp <= _bufferTtl &&
                (!notBefore.HasValue || entry.Timestamp >= notBefore.Value))
                return entry.ProcessName;
        }
        return null;
    }

    /// <summary>
    /// 异步重试查询进程名。ETW 事件可能比 FSW 晚到，在后台延迟重试。
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="maxDelayMs">最大等待毫秒数（默认 1500ms）</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>进程名，超时则返回 null</returns>
    public async Task<string?> TryGetProcessAsync(
        string filePath,
        int maxDelayMs = 1500,
        CancellationToken ct = default,
        DateTime? notBefore = null)
    {
        if (string.IsNullOrEmpty(filePath)) return null;

        var normalized = NormalizePath(filePath);
        var sw = System.Diagnostics.Stopwatch.StartNew();
/*
        !ct.IsCancellationRequested 是必要的，它可以在程序结束后快速 结束循环，避免阻塞线程。
         程序停止后 会调用 Stop()，它会取消 _cts，从而触发 ct.IsCancellationRequested。
         如果没有这个检查，循环可能会继续等待，直到达到 maxDelayMs 才结束，导致程序退出延迟。
         另外，Task.Delay 本身也会抛出 OperationCanceledException，如果 ct 被取消，它会立即抛出异常，从而跳出循环。
         检查 ct.IsCancellationRequested 可以让我们在循环中及时响应取消请求，避免不必要的等待。
*/
        while (sw.ElapsedMilliseconds < maxDelayMs && !ct.IsCancellationRequested)
        {
            if (_eventBuffer.TryGetValue(normalized, out var entry))
            {
                // 如果事件在缓冲区中，并且没有过期，则返回进程名并移除缓冲区中的记录
                if (DateTime.Now - entry.Timestamp <= _bufferTtl &&
                    (!notBefore.HasValue || entry.Timestamp >= notBefore.Value))
                {
                    return entry.ProcessName;
                }
            }

            try
            {
                await Task.Delay(50, ct);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        return null;
    }

    private void CleanupBuffer()
    {
        var cutoff = DateTime.Now - _bufferTtl;
        foreach (var kv in _eventBuffer)
        {
            if (kv.Value.Timestamp < cutoff)
                _eventBuffer.TryRemove(kv.Key, out _);
        }
    }

    private static string NormalizePath(string path) =>
        path.Replace('/', '\\').TrimEnd('\\');

    public void Stop()
    {
        IsRunning = false;
        _cts?.Cancel();
        try { _session?.Dispose(); } catch { }
        _session = null;
        _eventBuffer.Clear();
    }

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}
