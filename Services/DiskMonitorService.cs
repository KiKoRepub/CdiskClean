using System.Collections.Concurrent;
using CdiskClean.Models;

namespace CdiskClean.Services;

public class DiskMonitorService : IDisposable
{
    private readonly List<FileSystemWatcher> _watchers = new();
    private readonly EtwMonitorService _etwService;
    private readonly CleanupService? _cleanupService;
    private readonly object _lock = new();
    private volatile bool _paused;

    public event Action<FileChangeRecord>? FileChanged;
    public event Action<FileChangeRecord>? FileRecordUpdated;
    public event Action<string>? MonitorError;

    public List<WatchingDirectory> WatchDirectories { get; } = new();

    public List<IgnoreProcessRecord> IgnoreProcessRecords { get; } = new();

    private readonly ConcurrentDictionary<string, FileSystemWatcher> _watcherMap = new();

    // 延迟查询机制：FSW 先触发，ETW 可能还没捕捉到进程信息
    private readonly ConcurrentQueue<PendingQuery> _pendingQueries = new();
    private CancellationTokenSource? _deferredCts; // 用于取消延迟查询任务
    private Task? _deferredTask;

    private record struct PendingQuery(FileChangeRecord Record, string FilePath);

    public bool IsRunning => _watchers.Any(w => w.EnableRaisingEvents);

    public DiskMonitorService(EtwMonitorService etwService, CleanupService? cleanupService = null)
    {
        _etwService = etwService;
        _cleanupService = cleanupService;
    }

    public bool HasDirectory(string path) =>
        _watcherMap.ContainsKey(path);

    /// <summary>用外部目录列表（如数据库）初始化，替代默认列表</summary>
    public void LoadDirectories(List<WatchingDirectory> dirs)
    {
        WatchDirectories.Clear();
        // 先去重 再筛选
        var usingFolderList = dirs
            .Where(d => d.Status != RecordStatusEnum.DELETED);

        // 存进 监视列表，ETW 白名单
        WatchDirectories.AddRange(usingFolderList);
        // 原子操作
        _etwService.OperateWriteFolderArr(usingFolderList.Select(f => f.Path).ToArray());
    }

    /// <summary>初始化默认监视目录（数据库为空时使用）</summary>
    public void LoadDefaults()
    {
        WatchDirectories.Clear();
        WatchDirectories.AddRange(WatchingDirectory.getDefaultDirectories());
    }

    public void Start()
    {
        lock (_lock)
        {
            foreach (var item in WatchDirectories)
            {
                if (item.Status != RecordStatusEnum.USING) continue;
                StartWatchingInternal(item.Path, item.IncludeSubdirs);
            }
        }

        // 启动延迟查询后台处理器
        _deferredCts = new CancellationTokenSource();
        _deferredTask = Task.Run(() => ProcessPendingQueriesAsync(_deferredCts.Token));
    }

    public void Stop()
    {
        _deferredCts?.Cancel();
        _deferredTask = null;

        lock (_lock)
        {
            foreach (var w in _watchers)
            {
                w.EnableRaisingEvents = false;
                w.Dispose();
            }
            _watchers.Clear();
            _watcherMap.Clear();
        }
    }

    public void Pause()
    {
        _paused = true;
        lock (_lock)
        {
            foreach (var w in _watchers)
                w.EnableRaisingEvents = false;
        }
    }

    public void Resume()
    {
        _paused = false;
        lock (_lock)
        {
            foreach (var w in _watchers)
            {
                var dir = w.Path;
                var item = WatchDirectories.FirstOrDefault(d => d.Path == dir);
                if (item?.Status == RecordStatusEnum.USING)
                    w.EnableRaisingEvents = true;
            }
        }
    }

    /// <summary>对单个目录启动监视</summary>
    public void StartDirectory(string path, bool includeSubdirs)
    {
        lock (_lock)
        {
            if (_watcherMap.ContainsKey(path)) return;
            StartWatchingInternal(path, includeSubdirs);
        }
    }

    /// <summary>停止并移除单个目录的监视</summary>
    public void StopDirectory(string path)
    {
        lock (_lock)
        {
            if (_watcherMap.TryRemove(path, out var watcher))
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                _watchers.Remove(watcher);
            }
        }
    }

    /// <summary>更新单个目录的状态</summary>
    public void SetDirectoryStatus(string path, RecordStatusEnum status)
    {
        var item = WatchDirectories.FirstOrDefault(d => d.Path == path);
        if (item != null)
            item.Status = status;

        if (status == RecordStatusEnum.USING)
            StartDirectory(path, item?.IncludeSubdirs ?? true);
        else
            StopDirectory(path);
    }

    /// <summary>添加新目录到监视列表</summary>
    public WatchingDirectory AddDirectoryToEtwArr(string path, bool includeSubdirs)
    {

        var dir = new WatchingDirectory(path, includeSubdirs);

        // 如果已经存在，则不重复添加
        if (WatchDirectories.Any(d => d.Path == path)) return dir;
        
        WatchDirectories.Add(dir);
        _etwService.OperateWriteFolderArr(dir.Path, 1);

        if (_watchers.Any(w => w.EnableRaisingEvents))
            StartDirectory(path, includeSubdirs);

        return dir;
    }

    #region 忽略进程列表 
    /// <summary>用外部忽略进程列表（如数据库）初始化，并同步 ETW 黑名单</summary>
    public void LoadIgnoreProcesses(List<IgnoreProcessRecord> records)
    {
        IgnoreProcessRecords.Clear();
        IgnoreProcessRecords.AddRange(records);

        // 只有 USING 状态的进程才进 ETW 黑名单
        _etwService.OperateIgnoreProcessArr(
            records.Where(r => r.Status == RecordStatusEnum.USING)
                   .Select(r => r.ProcessName).ToArray());
    }

    /// <summary>添加忽略进程并同步 ETW 黑名单（已存在则返回现有记录）</summary>
    public IgnoreProcessRecord AddIgnoreProcess(string processName)
    {
        var existing = IgnoreProcessRecords.FirstOrDefault(r =>
            string.Equals(r.ProcessName, processName, StringComparison.OrdinalIgnoreCase));
        if (existing != null) return existing;

        var record = new IgnoreProcessRecord(processName);
        IgnoreProcessRecords.Add(record);
        _etwService.OperateIgnoreProcessArr(record.ProcessName, 1);
        return record;
    }

    /// <summary>更新忽略进程状态：USING 加入 ETW 黑名单，其他状态移出黑名单</summary>
    public void SetIgnoreProcessStatus(string processName, RecordStatusEnum status)
    {
        var item = IgnoreProcessRecords.FirstOrDefault(r =>
            string.Equals(r.ProcessName, processName, StringComparison.OrdinalIgnoreCase));
        if (item != null)
            item.Status = status;

        _etwService.OperateIgnoreProcessArr(processName,
            status == RecordStatusEnum.USING ? 1 : 2);
    }
    #endregion

    private void StartWatchingInternal(string dir, bool includeSubdirs)
    {
        if (!System.IO.Directory.Exists(dir)) return;

        try
        {
            var watcher = new FileSystemWatcher(dir)
            {
                NotifyFilter = NotifyFilters.FileName
                               | NotifyFilters.Size
                               | NotifyFilters.LastWrite,
                IncludeSubdirectories = includeSubdirs,
                InternalBufferSize = 64 * 1024,
                EnableRaisingEvents = true
            };

            watcher.Created += OnFileEvent;
            watcher.Changed += OnFileEvent;
            watcher.Deleted += OnFileEvent;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            _watchers.Add(watcher);
            _watcherMap[dir] = watcher;
        }
        catch (Exception ex)
        {
            MonitorError?.Invoke($"无法监视目录 {dir}: {ex.Message}");
        }
    }

    private void OnFileEvent(object sender, FileSystemEventArgs e)
    {
        if (_paused) return;

        // 清理功能产生的文件操作不应被记录（避免自己监听到自己）
        if (_cleanupService?.ShouldIgnoreEvent(e.FullPath) == true) return;

        var changeType = e.ChangeType switch
        {
            WatcherChangeTypes.Created => ChangeType.Created,
            WatcherChangeTypes.Changed => ChangeType.Changed,
            WatcherChangeTypes.Deleted => ChangeType.Deleted,
            _ => ChangeType.Changed
        };

        // 先用快速非阻塞方式查询 ETW 缓冲区
        var processName = _etwService.TryGetProcessOnce(e.FullPath);

        var record = new FileChangeRecord
        {
            Timestamp = DateTime.Now,
            ChangeType = changeType,
            FullPath = e.FullPath,
            FileName = Path.GetFileName(e.FullPath),
            Directory = Path.GetDirectoryName(e.FullPath) ?? "",
            SizeBytes = GetFileSizeSafe(e.FullPath),
            SourceProcess = processName
        };

        FileChanged?.Invoke(record);

        // 如果未命中 ETW 缓冲，放入延迟查询队列
        if (processName == null)
            _pendingQueries.Enqueue(new PendingQuery(record, e.FullPath));
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (_paused) return;

        // 移动/软链接操作会产生 Renamed 事件：新路径可能落在目标目录，
        // 旧路径一定位于清理的原路径集合内，两者都需过滤
        if (_cleanupService != null &&
            (_cleanupService.ShouldIgnoreEvent(e.FullPath) ||
             _cleanupService.ShouldIgnoreEvent(e.OldFullPath)))
            return;

        var processName = _etwService.TryGetProcessOnce(e.FullPath);

        var record = new FileChangeRecord
        {
            Timestamp = DateTime.Now,
            ChangeType = ChangeType.Renamed,
            FullPath = e.FullPath,
            FileName = Path.GetFileName(e.FullPath),
            Directory = Path.GetDirectoryName(e.FullPath) ?? "",
            SizeBytes = GetFileSizeSafe(e.FullPath),
            SourceProcess = processName
        };

        FileChanged?.Invoke(record);

        if (processName == null)
            _pendingQueries.Enqueue(new PendingQuery(record, e.FullPath));
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        var ex = e.GetException();
        MonitorError?.Invoke($"FileSystemWatcher 错误: {ex.Message}");
    }

    private static long? GetFileSizeSafe(string path)
    {
        try
        {
            var fi = new FileInfo(path);
            return fi.Exists ? fi.Length : null;
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 后台处理延迟查询队列。FSW 比 ETW 先触发时，在此异步重试获取进程名。
    /// </summary>
    private async Task ProcessPendingQueriesAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            while (_pendingQueries.TryDequeue(out var query))
            {
                try
                {
                    var processName = await _etwService.TryGetProcessAsync(query.FilePath, 1500, ct);
                    if (processName != null)
                    {
                        query.Record.SourceProcess = processName;
                        FileRecordUpdated?.Invoke(query.Record);
                    }
                }
                catch
                {
                    // 单条查询失败不影响后续处理
                }
            }

            try
            {
                await Task.Delay(100, ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    public void Dispose()
    {
        Stop();
        _deferredCts?.Dispose(); 
    }
}
