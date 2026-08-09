using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using CdiskClean.Helpers;
using CdiskClean.Models;

namespace CdiskClean.Services;

/// <summary>
/// 清理方式
/// </summary>
public enum CleanupMethod
{
    RecycleBin,
    PermanentDelete,
    Move,
    Compress,
    Mklink
}

/// <summary>
/// 文件清理服务：目录扫描、安全校验、清理执行（回收站/永久删除/移动/压缩/mkLink）、
/// 清理日志入库，以及清理期间的自身动作过滤（供监控服务查询）。
/// </summary>
public class CleanupService
{
    private readonly IDatabaseService _databaseService;

    // 清理期间被操作的原路径快照 + 目标目录，用于让 FSW/ETW 监控忽略本次清理产生的事件
    private volatile string[] _activePathSnapshot = Array.Empty<string>();
    private volatile string? _activeTargetDir;

    public bool IsCleaning => _activePathSnapshot.Length > 0;

    public CleanupService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    #region 监控联动（清理期间过滤自身事件）

    /// <summary>清理开始前调用：登记被清理的原路径与目标目录</summary>
    public void BeginCleanup(IEnumerable<CleanupFileEntry> entries, string? targetDir)
    {
        _activePathSnapshot = entries
            .Select(e => Path.GetFullPath(e.FullPath))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        _activeTargetDir = string.IsNullOrWhiteSpace(targetDir)
            ? null
            : Path.GetFullPath(targetDir);
    }

    public void EndCleanup()
    {
        _activePathSnapshot = Array.Empty<string>();
        _activeTargetDir = null;
    }

    /// <summary>
    /// 监控服务在产生记录前调用：路径位于本次清理的原路径（或其子项）内、
    /// 或位于目标目录内，都应忽略，避免"自己监听到自己"污染统计与提醒。
    /// </summary>
    public bool ShouldIgnoreEvent(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;

        var target = _activeTargetDir;
        if (target != null && IsPathInside(path, target)) return true;

        foreach (var p in _activePathSnapshot)
        {
            if (IsPathInside(path, p)) return true;
        }
        return false;
    }

    private static bool IsPathInside(string path, string parent)
    {
        var full = path.TrimEnd('\\');
        var root = parent.TrimEnd('\\');
        if (full.Equals(root, StringComparison.OrdinalIgnoreCase)) return true;
        return full.StartsWith(root + "\\", StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region 扫描

    /// <summary>
    /// 递归扫描目录，返回全部文件与目录条目（目录条目在文件之前，父目录先于子目录）。
    /// 跳过重解析点（符号链接/挂载点），无权限目录静默跳过。
    /// </summary>
    public async Task<List<CleanupFileEntry>> ScanDirectoryAsync(
        string rootPath,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var root = new DirectoryInfo(rootPath);
        if (!root.Exists)
            throw new DirectoryNotFoundException($"目录不存在: {rootPath}");

        var entries = new List<CleanupFileEntry>();
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        await Task.Run(() => ScanRecursive(root, entries, progress, cts.Token), cts.Token);
        return entries;
    }
    /// <summary>
    /// 递归扫描目录，返回全部文件与目录条目（目录条目在文件之前，父目录先于子目录）。
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="entries"></param>
    /// <param name="progress"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private static CleanupFileEntry ScanRecursive(
        DirectoryInfo dir,
        List<CleanupFileEntry> entries,
        IProgress<int>? progress,
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
                    dirEntry.SizeBytes += ScanRecursive(subDir, entries, progress, ct).SizeBytes;
                }
                catch { continue; }
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }

        progress?.Report(0);
        return dirEntry;
    }

    #endregion

    #region 高频修改路径参考

    /// <summary>按目录分组统计变更记录，返回变更最频繁的目录</summary>
    public static List<FrequentPathInfo> GetFrequentPaths(
        IEnumerable<FileChangeRecord> records,
        int topN = 30)
    {
        return records
            .Where(r => !string.IsNullOrEmpty(r.Directory))
            .GroupBy(r => r.Directory)
            .Select(g => new FrequentPathInfo
            {
                Path = g.Key,
                ChangeCount = g.Count(),
                LastChangeTime = g.Max(r => r.Timestamp)
            })
            .OrderByDescending(f => f.ChangeCount)
            .Take(topN)
            .ToList();
    }

    #endregion
    
    #region 清理执行

    public static string GetMethodDisplayName(CleanupMethod method) => method switch
    {
        CleanupMethod.RecycleBin => "回收站",
        CleanupMethod.PermanentDelete => "永久删除",
        CleanupMethod.Move => "移动",
        CleanupMethod.Compress => "压缩",
        CleanupMethod.Mklink => "mkLink",
        _ => "未知"
    };

    /// <summary>清理方式是否需要目标目录</summary>
    public static bool RequiresTarget(CleanupMethod method) =>
        method is CleanupMethod.Move or CleanupMethod.Compress or CleanupMethod.Mklink;

    /// <summary>
    /// 逐项执行清理：安全校验 → 执行 → 日志入库。整个过程在后台线程运行。
    /// </summary>
    public async Task<CleanupResult> ExecuteAsync(
        IReadOnlyList<CleanupFileEntry> entries,
        CleanupMethod method,
        string? targetDir,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (entries == null || entries.Count == 0)
            return new CleanupResult();

        BeginCleanup(entries, targetDir);
        try
        {
            return await Task.Run(() =>
            {
                var result = new CleanupResult { Total = entries.Count };
                for (int i = 0; i < entries.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var entry = entries[i];
                    progress?.Report($"正在清理 ({i + 1}/{entries.Count}): {entry.FullPath}");

                    var ok = ExecuteSingle(entry, method, targetDir, out var error, out var freed);
                    if (ok)
                    {
                        result.Success++;
                        result.FreedBytes += freed;
                    }
                    else
                    {
                        result.Fail++;
                        progress?.Report($"清理失败: {entry.FullPath} - {error}");
                    }

                    SaveRecord(entry, method, ok, error);
                }
                return result;
            }, cancellationToken);
        }
        finally
        {
            EndCleanup();
        }
    }

    /// <summary>执行单项清理，返回是否成功、错误信息与估算释放空间</summary>
    private static bool ExecuteSingle(
        CleanupFileEntry entry,
        CleanupMethod method,
        string? targetDir,
        out string? error,
        out long freed)
    {
        error = null;
        freed = 0;

        var full = Path.GetFullPath(entry.FullPath);

        // 安全校验：受保护路径拒绝清理
        if (IsProtectedPath(full, out var reason))
        {
            error = $"受保护路径({reason})，已拒绝清理";
            return false;
        }

        // 文件/目录已不存在视为成功（例如整目录删除后子项已被移除）
        if (!File.Exists(full) && !Directory.Exists(full))
        {
            error = "文件已不存在";
            return true;
        }

        try
        {
            switch (method)
            {
                case CleanupMethod.RecycleBin:
                    if (!FileRecycleHelper.SendToRecycleBin(full, showConfirmDialog: false))
                    {
                        error = "移入回收站失败";
                        return false;
                    }
                    break;

                case CleanupMethod.PermanentDelete:
                    if (entry.IsDirectory)
                        Directory.Delete(full, true);
                    else
                        File.Delete(full);
                    freed = entry.SizeBytes;
                    break;

                case CleanupMethod.Move:
                    MoveToTarget(full, entry, targetDir);
                    break;

                case CleanupMethod.Compress:
                    freed = CompressToTarget(full, entry, targetDir);
                    // 压缩成功后删除原文件，达到释放空间的目的
                    if (entry.IsDirectory)
                        Directory.Delete(full, true);
                    else
                        File.Delete(full);
                    break;

                case CleanupMethod.Mklink:
                    MklinkToTarget(full, entry, targetDir);
                    break;

                default:
                    error = "未知的清理方式";
                    return false;
            }
        }
        catch (IOException ex)
        {
            error = $"文件被占用或操作失败: {ex.Message}";
            return false;
        }
        catch (UnauthorizedAccessException ex)
        {
            error = $"无权限访问: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }

        return true;
    }

    /// <summary>把文件/目录移动到目标目录（重名时自动加时间戳后缀）</summary>
    private static void MoveToTarget(string full, CleanupFileEntry entry, string? targetDir)
    {
        if (string.IsNullOrWhiteSpace(targetDir)) throw new ArgumentException("未指定目标目录");
        Directory.CreateDirectory(targetDir);
        var dest = GetUniquePath(targetDir, entry.Name);
        MoveWithRetry(full, dest, entry.IsDirectory);
    }

    /// <summary>把文件/目录压缩到目标目录，返回估算释放空间（原大小-压缩包大小）</summary>
    private static long CompressToTarget(string full, CleanupFileEntry entry, string? targetDir)
    {
        if (string.IsNullOrWhiteSpace(targetDir)) throw new ArgumentException("未指定目标目录");
        Directory.CreateDirectory(targetDir);
        var zipPath = GetUniquePath(targetDir, entry.Name + ".zip");

        if (entry.IsDirectory)
        {
            ZipFile.CreateFromDirectory(full, zipPath, CompressionLevel.Optimal, false);
        }
        else
        {
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(full, entry.Name);
            }
        }

        var compressedSize = new FileInfo(zipPath).Length;
        return Math.Max(0, entry.SizeBytes - compressedSize);
    }

    /// <summary>把文件/目录迁移到目标目录，并在原位置创建软链接</summary>
    private static void MklinkToTarget(string full, CleanupFileEntry entry, string? targetDir)
    {
        if (string.IsNullOrWhiteSpace(targetDir)) throw new ArgumentException("未指定目标目录");
        Directory.CreateDirectory(targetDir);
        var dest = GetUniquePath(targetDir, entry.Name);

        if (entry.IsDirectory)
        {
            // 目录使用联接（junction /J），无需管理员权限且支持跨卷
            MoveWithRetry(full, dest, true);
            if (!RunCmd($"mklink /J \"{full}\" \"{dest}\""))
                throw RollbackAfterLinkFailure(dest, full, true, "创建目录联接失败");
        }
        else
        {
            if (!string.Equals(Path.GetPathRoot(full), Path.GetPathRoot(targetDir), StringComparison.OrdinalIgnoreCase))
                throw new IOException("硬链接要求源与目标位于同一磁盘卷，请改用其他清理方式");

            MoveWithRetry(full, dest, false);
            // 文件使用 Win32 原生硬链接 API（替代 cmd.exe mklink /H），
            // 无需管理员权限，原路径即刻可继续使用
            if (!CreateHardLinkW(full, dest, IntPtr.Zero))
            {
                var win32Error = Marshal.GetLastWin32Error();
                throw RollbackAfterLinkFailure(dest, full, false,
                    $"创建文件硬链接失败(Win32 错误码 {win32Error})");
            }
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateHardLinkW(
        string lpFileName,
        string lpExistingFileName,
        IntPtr lpSecurityAttributes);

    private static void MoveWithRetry(string source, string destination, bool isDirectory)
    {
        IOException? lastError = null;
        const int maxAttempts = 8;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                if (isDirectory)
                    Directory.Move(source, destination);
                else
                    File.Move(source, destination);
                return;
            }
            catch (IOException ex) when (attempt < maxAttempts)
            {
                lastError = ex;
                // 移动前先回收本程序可能滞留的句柄（如压缩/扫描遗留流），
                // 再等待瞬时占用（杀毒扫描、搜索索引等）自动释放后重试
                ReleaseFileLocks();
                Thread.Sleep(500 * attempt);
            }
            catch (IOException ex)
            {
                lastError = ex;
            }
        }

        throw new IOException(
            $"文件被占用或移动失败，重试 {maxAttempts} 次（约 14 秒）后仍未成功。" +
            $"文件可能正被编辑器/播放器/下载器/杀毒软件/搜索索引等程序使用，请关闭后重试。{lastError?.Message}",
            lastError);
    }

    /// <summary>回收本程序可能滞留的文件句柄（垃圾回收并等待终结器）</summary>
    private static void ReleaseFileLocks()
    {
        try
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        catch
        {
            // 回收失败不影响主流程，仅无法释放滞留句柄
        }
    }

    private static IOException RollbackAfterLinkFailure(
        string movedPath,
        string originalPath,
        bool isDirectory,
        string message)
    {
        try
        {
            if (!File.Exists(originalPath) && !Directory.Exists(originalPath))
            {
                MoveWithRetry(movedPath, originalPath, isDirectory);
                return new IOException($"{message}: {originalPath}；已恢复原位置");
            }
        }
        catch
        {
            // 返回的错误会明确告知数据保留位置。
        }

        return new IOException($"{message}: {originalPath}；数据保留在目标位置: {movedPath}");
    }

    private static bool RunCmd(string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c " + arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            using var proc = Process.Start(psi);
            if (proc == null) return false;
            if (!proc.WaitForExit(15000))
            {
                try { proc.Kill(true); } catch { }
                return false;
            }
            return proc.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>目标目录下生成不重名的路径（重名时追加时间戳+序号）</summary>
    private static string GetUniquePath(string dir, string name)
    {
        var candidate = Path.Combine(dir, name);
        if (!File.Exists(candidate) && !Directory.Exists(candidate))
            return candidate;

        var ext = Path.GetExtension(name);
        var stem = Path.GetFileNameWithoutExtension(name);
        for (int i = 1; i < 1000; i++)
        {
            candidate = Path.Combine(dir, $"{stem}_{DateTime.Now:yyyyMMddHHmmss}_{i}{ext}");
            if (!File.Exists(candidate) && !Directory.Exists(candidate))
                return candidate;
        }
        return Path.Combine(dir, $"{stem}_{Guid.NewGuid():N}{ext}");
    }

    /// <summary>受保护路径检查：程序自身目录、驱动器根目录、Windows 系统目录</summary>
    private static bool IsProtectedPath(string path, out string reason)
    {
        var full = Path.GetFullPath(path).TrimEnd('\\');
        var appDir = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory).TrimEnd('\\');

        if (full.Equals(appDir, StringComparison.OrdinalIgnoreCase) ||
            full.StartsWith(appDir + "\\", StringComparison.OrdinalIgnoreCase))
        {
            reason = "程序自身目录";
            return true;
        }

        // 驱动器根目录（如 C:\）
        if (full.Length <= 3 && full.EndsWith(":"))
        {
            reason = "驱动器根目录";
            return true;
        }

        string[] systemRoots =
        {
            @"C:\Windows", @"C:\Program Files", @"C:\Program Files (x86)", @"C:\ProgramData"
        };
        foreach (var root in systemRoots)
        {
            if (full.Equals(root, StringComparison.OrdinalIgnoreCase))
            {
                reason = "系统目录";
                return true;
            }
        }

        reason = "";
        return false;
    }

    private void SaveRecord(CleanupFileEntry entry, CleanupMethod method, bool success, string? message)
    {
        try
        {
            _databaseService.SaveCleanupRecord(new CleanupRecord
            {
                CleanupTime = DateTime.Now,
                FullPath = entry.FullPath,
                FileName = entry.Name,
                SizeBytes = entry.IsDirectory ? (long?)null : entry.SizeBytes,
                Method = GetMethodDisplayName(method),
                Success = success,
                Message = message
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"保存清理记录失败: {ex.Message}");
        }
    }

    #endregion
}
