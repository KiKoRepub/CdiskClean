using System.Collections.Concurrent;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace CdiskClean.Services;

public class EtwMonitorService : IDisposable
{
    private TraceEventSession? _session;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<string, (string ProcessName, DateTime Timestamp)> _eventBuffer = new(StringComparer.OrdinalIgnoreCase);
    private readonly TimeSpan _bufferTtl = TimeSpan.FromSeconds(3);

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

    private void BufferEvent(string? fileName, string processName)
    {
        try
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var normalized = NormalizePath(fileName);
            _eventBuffer[normalized] = (processName, DateTime.Now);
        }
        catch
        {
            // 忽略单条事件异常
        }
    }

    public string? TryGetProcess(string filePath)
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
