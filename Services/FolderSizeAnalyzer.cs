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
            Name = root.Name,
            LastScannedAt = DateTime.Now
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
                    var length = file.Length;
                    info.SizeBytes += length;
                    info.FileCount++;
                    var extension = string.IsNullOrWhiteSpace(file.Extension) ? "(无扩展名)" : file.Extension.ToLowerInvariant();
                    info.ExtensionSizes[extension] = info.ExtensionSizes.GetValueOrDefault(extension) + length;
                }
                catch
                {
                    info.InaccessibleCount++;
                    info.AccessStatus = FolderAccessStatus.Partial;
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            info.AccessStatus = FolderAccessStatus.Denied;
            info.ErrorMessage = ex.Message;
            info.InaccessibleCount++;
            return;
        }
        catch (DirectoryNotFoundException ex)
        {
            info.AccessStatus = FolderAccessStatus.Missing;
            info.ErrorMessage = ex.Message;
            return;
        }

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
                    Name = subDir.Name,
                    LastScannedAt = DateTime.Now
                };

                try
                {
                    ScanRecursive(subDir, subInfo, ct);
                }
                catch { continue; }

                info.SubFolders.Add(subInfo);
                info.SizeBytes += subInfo.SizeBytes;
                info.FileCount += subInfo.FileCount;
                info.InaccessibleCount += subInfo.InaccessibleCount;
                foreach (var pair in subInfo.ExtensionSizes)
                    info.ExtensionSizes[pair.Key] = info.ExtensionSizes.GetValueOrDefault(pair.Key) + pair.Value;
                if (subInfo.AccessStatus != FolderAccessStatus.Accessible)
                    info.AccessStatus = FolderAccessStatus.Partial;
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            info.AccessStatus = FolderAccessStatus.Partial;
            info.ErrorMessage ??= ex.Message;
            info.InaccessibleCount++;
        }
        catch (DirectoryNotFoundException ex)
        {
            info.AccessStatus = FolderAccessStatus.Missing;
            info.ErrorMessage ??= ex.Message;
        }
    }
}
