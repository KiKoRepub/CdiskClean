using CdiskClean.Models;

namespace CdiskClean.Services;

public class FolderSizeAnalyzer
{
    public async Task<FolderSizeInfo> ScanFolderAsync(
        string rootPath,
        CancellationToken cancellationToken = default)
    {
        var root = new DirectoryInfo(rootPath);
        if (!root.Exists)
            throw new DirectoryNotFoundException($"目录不存在: {rootPath}");

        var result = new FolderSizeInfo
        {
            Path = rootPath,
            Name = root.Name
        };

        await Task.Run(() => ScanRecursive(root, result, cancellationToken), cancellationToken);

        return result;
    }

    private static void ScanRecursive(
        DirectoryInfo dir,
        FolderSizeInfo info,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested) return;

        // 扫描文件
        try
        {
            foreach (var file in dir.EnumerateFiles())
            {
                if (ct.IsCancellationRequested) return;
                try
                {
                    info.SizeBytes += file.Length;
                    info.FileCount++;
                }
                catch
                {
                    // 跳过无权限的文件
                }
            }
        }
        catch (UnauthorizedAccessException) { return; }
        catch (DirectoryNotFoundException) { return; }

        // 扫描子文件夹
        try
        {
            foreach (var subDir in dir.EnumerateDirectories())
            {
                if (ct.IsCancellationRequested) return;

                // 跳过重解析点（符号链接/挂载点）
                try
                {
                    if (subDir.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        continue;
                }
                catch { continue; }

                var subInfo = new FolderSizeInfo
                {
                    Path = subDir.FullName,
                    Name = subDir.Name
                };

                try
                {
                    ScanRecursive(subDir, subInfo, ct);
                }
                catch { continue; }

                info.SubFolders.Add(subInfo);
                info.SizeBytes += subInfo.SizeBytes;
                info.FileCount += subInfo.FileCount;
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }
    }
}
