using CdiskClean.Helpers;
using CdiskClean.Models.cleanUp;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace CdiskClean.Services;

/// <summary>单项文件操作与安全校验；失败回滚和重试均在此维护。</summary>
internal static class CleanupFileOperations
{
    /// <summary>执行单项清理，返回是否成功、错误信息与估算释放空间</summary>
    internal static bool Execute(
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

        if (!ValidateSnapshot(entry, full, out error))
            return false;

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

    private static bool ValidateSnapshot(CleanupFileEntry entry, string fullPath, out string? error)
    {
        error = null;
        try
        {
            var attributes = File.GetAttributes(fullPath);
            if (attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                error = "重解析点不会被自动清理";
                return false;
            }

            if (entry.IsDirectory) return true;

            var info = new FileInfo(fullPath);
            if (info.Length != entry.SizeBytes)
            {
                error = "扫描后文件大小已变化，请重新扫描";
                return false;
            }

            if (entry.LastWriteTime.HasValue &&
                Math.Abs((info.LastWriteTime - entry.LastWriteTime.Value).TotalSeconds) > 2)
            {
                error = "扫描后文件内容已变化，请重新扫描";
                return false;
            }

            return true;
        }
        catch (UnauthorizedAccessException ex)
        {
            error = $"无权限复核: {ex.Message}";
            return false;
        }
        catch (IOException ex)
        {
            error = $"无法复核文件状态: {ex.Message}";
            return false;
        }
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
                {
                    // 检查是否跨卷
                    bool sameVolume = string.Equals(
                        Path.GetPathRoot(source),
                        Path.GetPathRoot(destination),
                        StringComparison.OrdinalIgnoreCase);

                    if (sameVolume)
                    {
                        Directory.Move(source, destination);
                    }
                    else
                    {
                        // 跨卷：复制整个目录树，然后删除源
                        CopyDirectory(source, destination);
                        // 删除 源
                        DeleteDirectoryWithRetry(source);
                    }
                }
                else
                {
                    File.Move(source, destination);
                }
                return;
            }
            catch (IOException ex) when (attempt < maxAttempts)
            {
                lastError = ex;
                ReleaseFileLocks();
                Thread.Sleep(500 * attempt);
            }
            catch (IOException ex)
            {
                lastError = ex;
            }
        }

        throw new IOException(
            $"文件或目录被占用或移动失败，重试 {maxAttempts} 次（约 14 秒）后仍未成功。" +
            $"可能被编辑器/杀毒软件/搜索索引等占用，请关闭后重试。{lastError?.Message}",
            lastError);
    }


    private static void CopyDirectory(string sourceDir, string destDir)
    {
        // 创建目标目录（包括所有子目录结构）
        Directory.CreateDirectory(destDir);

        // 复制所有文件
        foreach (string filePath in Directory.GetFiles(sourceDir, "*", SearchOption.TopDirectoryOnly))
        {
            string fileName = Path.GetFileName(filePath);
            string destFilePath = Path.Combine(destDir, fileName);
            File.Copy(filePath, destFilePath, overwrite: true);
        }

        // 递归复制子目录
        foreach (string subDir in Directory.GetDirectories(sourceDir, "*", SearchOption.TopDirectoryOnly))
        {
            string dirName = Path.GetFileName(subDir);
            string destSubDir = Path.Combine(destDir, dirName);
            CopyDirectory(subDir, destSubDir);
        }
    }

    private static void DeleteDirectoryWithRetry(string path, int maxAttempts = 8)
    {
        if (!Directory.Exists(path))
            return;

        IOException? lastError = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                // 清空只读、系统等属性，并递归删除
                ForceDeleteDirectory(path);
                return;
            }
            catch (IOException ex) when (attempt < maxAttempts)
            {
                lastError = ex;
                // 释放可能占用的句柄（与之前相同）
                ReleaseFileLocks();
                Thread.Sleep(500 * attempt);
            }
            catch (IOException ex)
            {
                lastError = ex;
            }
            catch (UnauthorizedAccessException ex) when (attempt < maxAttempts)
            {
                // 权限问题也可能在删除时触发，重试
                lastError = new IOException("权限不足", ex);
                Thread.Sleep(500 * attempt);
            }
            catch (UnauthorizedAccessException ex)
            {
                lastError = new IOException("权限不足", ex);
            }
        }

        throw new IOException(
            $"删除源目录失败，重试 {maxAttempts} 次后仍未成功。请手动清理残留目录: {path}。{lastError?.Message}",
            lastError);
    }

    /// <summary>强制删除目录（会先清除只读/系统属性）</summary>
    private static void ForceDeleteDirectory(string path)
    {
        // 删除所有子目录（先递归，确保子目录内容先清空）
        foreach (string subDir in Directory.GetDirectories(path))
        {
            ForceDeleteDirectory(subDir);
        }

        // 删除所有文件（清除属性后再删除）
        foreach (string file in Directory.GetFiles(path))
        {
            // 移除只读、系统、隐藏等属性
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        // 最后删除空目录
        Directory.Delete(path, false);
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
            // 启动命令行进程
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
}
