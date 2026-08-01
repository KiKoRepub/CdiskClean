using CdiskClean.Models;

namespace CdiskClean.Services;

public class StatisticsService : IDisposable
{
    private readonly Dictionary<string, AppChangeStats> _stats = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();
    private readonly System.Timers.Timer _timer;
    private bool _isCountingDown;

    private const int IdleWindowSeconds = 120;

    public int CountdownRemaining { get; private set; }

    public bool HasStats => _stats.Count > 0;

    public event Action<int>? CountdownChanged;
    public event Action<List<AppChangeStats>>? StatsReady;

    public StatisticsService()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (_, _) => OnTimerTick();
        _timer.AutoReset = true;
    }

    public void RecordChange(FileChangeRecord record)
    {
        var appName = string.IsNullOrWhiteSpace(record.SourceProcess)
            ? "未知进程"
            : record.SourceProcess;

        lock (_lock)
        {
            if (_stats.TryGetValue(appName, out var existing))
            {
                existing.ChangeCount++;
                existing.LastChangeTime = record.Timestamp;
            }
            else
            {
                _stats[appName] = new AppChangeStats
                {
                    AppName = appName,
                    ChangeCount = 1,
                    FirstChangeTime = record.Timestamp,
                    LastChangeTime = record.Timestamp
                };
            }
        }

        ResetCountdown();
    }

    public void Start()
    {
        _isCountingDown = true;
        CountdownRemaining = IdleWindowSeconds;
        _timer.Start();
        CountdownChanged?.Invoke(CountdownRemaining);
    }

    public void Stop()
    {
        _timer.Stop();
        _isCountingDown = false;
    }

    public void Reset()
    {
        lock (_lock)
        {
            _stats.Clear();
        }
        Stop();
        CountdownRemaining = 0;
        CountdownChanged?.Invoke(CountdownRemaining);
    }

    public List<AppChangeStats> GetCurrentStats()
    {
        lock (_lock)
        {
            return _stats.Values
                .OrderByDescending(s => s.ChangeCount)
                .ThenByDescending(s => s.LastChangeTime)
                .ToList();
        }
    }

    private void ResetCountdown()
    {
        CountdownRemaining = IdleWindowSeconds;
        _isCountingDown = true;
        if (!_timer.Enabled)
            _timer.Start();
        CountdownChanged?.Invoke(CountdownRemaining);
    }

    private void OnTimerTick()
    {
        if (!_isCountingDown) return;

        CountdownRemaining--;

        if (CountdownRemaining <= 0)
        {
            CountdownRemaining = 0;
            _isCountingDown = false;
            _timer.Stop();

            CountdownChanged?.Invoke(CountdownRemaining);

            var stats = GetCurrentStats();
            if (stats.Count > 0)
                StatsReady?.Invoke(stats);
        }
        else
        {
            CountdownChanged?.Invoke(CountdownRemaining);
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
