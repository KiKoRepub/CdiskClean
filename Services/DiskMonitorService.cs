using System.Collections.Concurrent;
using CdiskClean.Models;

namespace CdiskClean.Services;

public class DiskMonitorService : IDisposable
{
    private readonly List<FileSystemWatcher> _watchers = new();
    private readonly object _lock = new();
    private volatile bool _paused;

    public event Action<FileChangeRecord>? FileChanged;
    public event Action<string>? MonitorError;

    public List<WatchingDirectory> WatchDirectories { get; } = new();

    private readonly ConcurrentDictionary<string, FileSystemWatcher> _watcherMap = new();

    public bool IsRunning => _watchers.Any(w => w.EnableRaisingEvents);

    public bool HasDirectory(string path) =>
        _watcherMap.ContainsKey(path);

    /// <summary>用外部目录列表（如数据库）初始化，替代默认列表</summary>
    public void LoadDirectories(List<WatchingDirectory> dirs)
    {
        WatchDirectories.Clear();
        WatchDirectories.AddRange(dirs.Where(d => d.Status != DirectoryStatusEnum.DELETED));
    }

    /// <summary>初始化默认监视目录（数据库为空时使用）</summary>
    public void LoadDefaults()
    {
        WatchDirectories.Clear();
        WatchDirectories.AddRange(new List<WatchingDirectory>
        {
            new(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true),
            new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), true),
            new(GetDownloadsPath(), true),
            new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp"), true),
            new(@"C:\Windows\Temp", true),
            new(@"C:\Program Files", false),
            new(@"C:\Program Files (x86)", false)
        });
    }

    public void Start()
    {
        lock (_lock)
        {
            foreach (var item in WatchDirectories)
            {
                if (item.Status != DirectoryStatusEnum.USING) continue;
                StartWatchingInternal(item.Path, item.IncludeSubdirs);
            }
        }
    }

    public void Stop()
    {
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
                if (item?.Status == DirectoryStatusEnum.USING)
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
    public void SetDirectoryStatus(string path, DirectoryStatusEnum status)
    {
        var item = WatchDirectories.FirstOrDefault(d => d.Path == path);
        if (item != null)
            item.Status = status;

        if (status == DirectoryStatusEnum.USING)
            StartDirectory(path, item?.IncludeSubdirs ?? true);
        else
            StopDirectory(path);
    }

    /// <summary>添加新目录到监视列表</summary>
    public void AddDirectory(string path, bool includeSubdirs)
    {
        if (WatchDirectories.Any(d => d.Path == path)) return;

        var dir = new WatchingDirectory(path, includeSubdirs);
        WatchDirectories.Add(dir);

        if (_watchers.Any(w => w.EnableRaisingEvents))
            StartDirectory(path, includeSubdirs);
    }

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

        var changeType = e.ChangeType switch
        {
            WatcherChangeTypes.Created => ChangeType.Created,
            WatcherChangeTypes.Changed => ChangeType.Changed,
            WatcherChangeTypes.Deleted => ChangeType.Deleted,
            _ => ChangeType.Changed
        };

        var record = new FileChangeRecord
        {
            Timestamp = DateTime.Now,
            ChangeType = changeType,
            FullPath = e.FullPath,
            FileName = Path.GetFileName(e.FullPath),
            Directory = Path.GetDirectoryName(e.FullPath) ?? "",
            SizeBytes = GetFileSizeSafe(e.FullPath)
        };

        FileChanged?.Invoke(record);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (_paused) return;

        var record = new FileChangeRecord
        {
            Timestamp = DateTime.Now,
            ChangeType = ChangeType.Renamed,
            FullPath = e.FullPath,
            FileName = Path.GetFileName(e.FullPath),
            Directory = Path.GetDirectoryName(e.FullPath) ?? "",
            SizeBytes = GetFileSizeSafe(e.FullPath)
        };

        FileChanged?.Invoke(record);
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

    private static string GetDownloadsPath()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var downloads = Path.Combine(userProfile, "Downloads");
        return System.IO.Directory.Exists(downloads)
            ? downloads
            : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }

    public void Dispose()
    {
        Stop();
    }
}
