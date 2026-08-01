using CdiskClean.Models;

namespace CdiskClean.Services;

/// <summary>
/// 按进程跟踪文件操作窗口：进程首次操作开始计时，空闲超过 IdleDelaySeconds 后触发一次提醒。
/// 与 StatisticsService（统计面板）完全独立。
/// </summary>
public class NotificationService : IDisposable
{
    private readonly object _lock = new();
    private readonly Dictionary<string, ActivityWindow> _windows = new(StringComparer.OrdinalIgnoreCase);
    private readonly System.Threading.Timer _timer;

    private const int TickIntervalMs = 500;

    /// <summary>进程空闲多少秒后触发提醒（可调变量）</summary>
    public int IdleDelaySeconds { get; set; } = 2;

    public event Action<ProcessNotificationRecord>? NotificationTriggered;

    private sealed class ActivityWindow
    {
        public DateTime FirstOpTime;
        public DateTime LastOpTime;
        public int Count;
    }

    public NotificationService()
    {
        _timer = new System.Threading.Timer(OnTick, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void RecordChange(FileChangeRecord record)
    {
        var appName = string.IsNullOrWhiteSpace(record.SourceProcess)
            ? "未知进程"
            : record.SourceProcess;

        var now = DateTime.Now;
        lock (_lock)
        {
            if (!_windows.TryGetValue(appName, out var window))
            {
                window = new ActivityWindow { FirstOpTime = now, LastOpTime = now };
                _windows[appName] = window;
            }

            window.LastOpTime = now;
            window.Count++;
        }

        _timer.Change(TickIntervalMs, TickIntervalMs);
    }

    private void OnTick(object? state)
    {
        var now = DateTime.Now;
        var threshold = TimeSpan.FromSeconds(Math.Max(IdleDelaySeconds, 1));
        List<ProcessNotificationRecord>? ready = null;

        lock (_lock)
        {
            foreach (var pair in _windows.ToList())
            {
                if (now - pair.Value.LastOpTime < threshold) continue;

                _windows.Remove(pair.Key);

                var duration = (int)Math.Max(1, Math.Round((now - pair.Value.FirstOpTime).TotalSeconds));
                ready ??= new List<ProcessNotificationRecord>();
                ready.Add(new ProcessNotificationRecord
                {
                    ProcessName = pair.Key,
                    OperationCount = pair.Value.Count,
                    DurationSeconds = duration,
                    TriggerTime = now
                });
            }

            if (_windows.Count == 0)
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        if (ready != null)
        {
            foreach (var record in ready)
                NotificationTriggered?.Invoke(record);
        }
    }

    public void Start() => _timer.Change(TickIntervalMs, TickIntervalMs);

    public void Stop()
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        lock (_lock)
        {
            _windows.Clear();
        }
    }

    public void Dispose() => _timer.Dispose();
}
