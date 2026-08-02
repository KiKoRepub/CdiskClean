using System.Collections.Concurrent;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace CdiskClean.Services;

/*
 public enum EtwWatchType
{
    None = 0,
    WATCH_DIRECTORY = 1,
    IGNORE_PROCESS = 2

}
*/
public class EtwMonitorService : IDisposable
{
    private TraceEventSession? _session;
    private CancellationTokenSource? _cts;
    /// <summary>
    /// 缓存起来的 所有进程的事件字典，监控触发的时候 会来这里面查询对应的进程名
    /// </summary>
    private readonly ConcurrentDictionary<string, (string ProcessName, DateTime Timestamp)> _eventBuffer = new(StringComparer.OrdinalIgnoreCase);
    private readonly TimeSpan _bufferTtl = TimeSpan.FromSeconds(3);
    // 定义一个原子引用（因为引用赋值是原子的，不需要锁）
    private volatile string[] _watchDirectoryArray = new string[0]; // 白名单
    private volatile string[] _ignoreProcessArray = new string[0]; // 黑名单

    
    public bool IsRunning { get; private set; }

    public void Start()
    {
        if (IsRunning) return;

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        Task.Run(() =>
        {
            try
            {
                var sessionName = "CdiskCleanEtwSession";
                try { TraceEventSession.GetActiveSession(sessionName)?.Dispose(); } catch { }

                using (_session = new TraceEventSession(sessionName))
                {
                    _session.EnableKernelProvider(KernelTraceEventParser.Keywords.FileIOInit);

                    var source = _session.Source;
                    source.Kernel.FileIOCreate += d => BufferEvent(d.FileName, d.ProcessName);
                    source.Kernel.FileIORead += d => BufferEvent(d.FileName, d.ProcessName);
                    source.Kernel.FileIOWrite += d => BufferEvent(d.FileName, d.ProcessName);
                    source.Kernel.FileIODelete += d => BufferEvent(d.FileName, d.ProcessName);

                    IsRunning = true;

                    using var cleanupTimer = new System.Timers.Timer(10000);
                    cleanupTimer.Elapsed += (_, _) => CleanupBuffer();
                    cleanupTimer.Start();

                    try
                    {
                        source.Process();
                    }
                    catch (OperationCanceledException) { }
                }
            }
            catch (Exception)
            {
                IsRunning = false;
            }
        }, token);
    }

    #region WatchDirectoryArray 相关操作
    public void OperateWriteFolderArr(string path, int type)
    {
        // TODO 用户添加目录时(需要添加订阅,目前直接在目标位置进行调用)
        // 生成新数组，替换旧数组
        var list = _watchDirectoryArray.ToList();

        if (type == 1)
            list.Add(path);
        if (type == 2)
            list.Remove(path);

        _watchDirectoryArray = list.Distinct().ToArray();
    }
    public void OperateWriteFolderArr(string[] paths)
    {
        _watchDirectoryArray = paths.ToArray();
    }
    #endregion

    #region IgnoreProcessArray 相关操作
    public void OperateIgnoreProcessArr(string processName, int type)
    {
        var list = _ignoreProcessArray.ToList();
        if (type == 1)
            list.Add(processName);
        if (type == 2)
            list.Remove(processName);
        _ignoreProcessArray = list.Distinct().ToArray();
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
            // 对于 白名单中的 目录路径进行选取

            var dirs = _watchDirectoryArray;

            // 对于 黑名单中的 进程名进行过滤（大小写不敏感）
            var ignoreProcesses = _ignoreProcessArray;

            if (ignoreProcesses.Any(p => string.Equals(p, processName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }


            foreach (var dir in dirs)
            {
                if (fileName.StartsWith(dir))
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
    public string? TryGetProcessOnce(string filePath)
    {
        if (!IsRunning || string.IsNullOrEmpty(filePath)) return null;

        var normalized = NormalizePath(filePath);
        if (_eventBuffer.TryGetValue(normalized, out var entry))
        {
            if (DateTime.Now - entry.Timestamp <= _bufferTtl)
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
    public async Task<string?> TryGetProcessAsync(string filePath, int maxDelayMs = 1500, CancellationToken ct = default)
    {
        if (!IsRunning || string.IsNullOrEmpty(filePath)) return null;

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
                if (DateTime.Now - entry.Timestamp <= _bufferTtl)
                {
                    _eventBuffer.TryRemove(normalized, out _);
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

    /// <summary>
    /// 同步重试查询进程名（保留原方法签名，内部委托给异步版本）。
    /// </summary>
    public string? TryGetProcess(string filePath)
    {
        if (!IsRunning || string.IsNullOrEmpty(filePath)) return null;

        var normalized = NormalizePath(filePath);

        for (int i = 0; i < 10; i++)
        {
            if (_eventBuffer.TryGetValue(normalized, out var entry))
            {
                if (DateTime.Now - entry.Timestamp <= _bufferTtl)
                {
                    _eventBuffer.TryRemove(normalized, out _);
                    return entry.ProcessName;
                }
            }

            Thread.Sleep(20);
        }

        return "未知进程";
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
    }

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }
}
