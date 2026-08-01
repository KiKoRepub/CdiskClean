using System.Collections.Concurrent;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace CdiskClean.Services;

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
    private volatile string[] _watchDirectoryArray = new string[0];

    // TODO 用户添加目录时(需要添加订阅,目前直接在目标位置进行调用)
    public void OperateWriteFolderArr(string path, int type)
    {
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
    /// 尝试获取指定文件路径关联的进程名，如果存在且未过期。
    /// </summary>
    /// <param name="filePath">需要获取关联进程名的文件路径</param>
    /// <returns>The process name if found and valid; otherwise, null.</returns>
    public string? TryGetProcess(string filePath)
    {


        // 前置判断
        if (!IsRunning || string.IsNullOrEmpty(filePath)) return null;


        string processNameResult = "未知进程";

        // === 核心逻辑：重试机制 ===
        // 因为 ETW 可能比 FSW 慢几毫秒，我们尝试在短时间内查找几次
        int retryCount = 0;
        const int maxRetries = 10; // 最多尝试 10 次
        const int retryDelay = 20; // 每次等待 20 毫秒 (总共最多等待 200ms)
        while (retryCount < maxRetries)
        {
            // 正式 逻辑
            var normalized = NormalizePath(filePath);

            if (_eventBuffer.TryGetValue(normalized, out var entry))
            {

                if (DateTime.Now - entry.Timestamp <= _bufferTtl)
                    processNameResult = entry.ProcessName;

                // 获取到了记得移除，避免脏读（下次误判）
                _eventBuffer.TryRemove(filePath, out _);
                  
            }

            // 如果没找到，睡一小会儿再试
            Thread.Sleep(retryDelay);
            retryCount++;
        }

       

        return processNameResult;
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
