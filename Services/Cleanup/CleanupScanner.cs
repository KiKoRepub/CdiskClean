using CdiskClean.Models.cleanUp;

namespace CdiskClean.Services;

/// <summary>扫描清理候选项，不依赖数据库或窗体。</summary>
public static class CleanupScanner
{
    /// <summary>
    /// 递归扫描目录，返回全部文件与目录条目（目录条目在文件之前，父目录先于子目录）。
    /// 跳过重解析点（符号链接/挂载点），无权限目录静默跳过。
    /// </summary>
    public static async Task<List<CleanupFileEntry>> ScanDirectoryAsync(
        string rootPath,
        CancellationToken cancellationToken = default)
    {
        var root = new DirectoryInfo(rootPath);
        if (!root.Exists)
            throw new DirectoryNotFoundException($"目录不存在: {rootPath}");

        var entries = new List<CleanupFileEntry>();

        await Task.Run(() => ScanRecursive(root, entries, cancellationToken), cancellationToken);
        return entries;
    }

    /// <summary>
    /// 递归扫描目录，返回全部文件与目录条目（目录条目在文件之前，父目录先于子目录）。
    /// </summary>
    private static CleanupFileEntry ScanRecursive(
        DirectoryInfo dir,
        List<CleanupFileEntry> entries,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return new CleanupFileEntry { FullPath = dir.FullName, Name = dir.Name, IsDirectory = true };

        var dirEntry = new CleanupFileEntry
        {
            FullPath = dir.FullName,
            Name = dir.Name,
            IsDirectory = true
        };
        entries.Add(dirEntry);

        // 扫描文件
        try
        {
            foreach (var file in dir.EnumerateFiles())
            {
                if (ct.IsCancellationRequested) return dirEntry;
                try
                {
                    var fileEntry = new CleanupFileEntry
                    {
                        FullPath = file.FullName,
                        Name = file.Name,
                        SizeBytes = file.Length,
                        LastWriteTime = file.LastWriteTime
                    };
                    entries.Add(fileEntry);
                    dirEntry.SizeBytes += fileEntry.SizeBytes;
                }
                catch
                {
                    // 单个文件读取失败（权限/占用）跳过
                }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }

        // 扫描子文件夹
        try
        {
            foreach (var subDir in dir.EnumerateDirectories())
            {
                if (ct.IsCancellationRequested) return dirEntry;

                // 跳过重解析点（符号链接/挂载点），避免循环
                try
                {
                    if (subDir.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        continue;
                }
                catch { continue; }

                try
                {
                    dirEntry.SizeBytes += ScanRecursive(subDir, entries, ct).SizeBytes;
                }
                catch { continue; }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }

        return dirEntry;
    }
}
