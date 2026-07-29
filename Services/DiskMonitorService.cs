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

    private static readonly (string Path, bool IncludeSubdirs)[] DefaultWatchDirs =
    {
        (Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true),
        (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), true),
        (GetDownloadsPath(), true),
        (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp"), true),
        (@"C:\Windows\Temp", true),
        (@"C:\Program Files", false),
        (@"C:\Program Files (x86)", false)
    };

    public bool IsRunning => _watchers.Any(w => w.EnableRaisingEvents);

    public void Start()
    {
        lock (_lock)
        {
            foreach (var (dir, includeSubdirs) in DefaultWatchDirs)
            {
                if (!System.IO.Directory.Exists(dir))
                    continue;

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
                }
                catch (Exception ex)
                {
                    MonitorError?.Invoke($"无法监视目录 {dir}: {ex.Message}");
                }
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
                w.EnableRaisingEvents = true;
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
        // 获取 Downloads 文件夹路径（跨文化兼容方式）
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
