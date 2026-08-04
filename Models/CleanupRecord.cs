namespace CdiskClean.Models;

/// <summary>
/// 清理历史记录（CleanupRecords 表）
/// </summary>
public class CleanupRecord
{
    public long Id { get; set; }

    public DateTime CleanupTime { get; set; } = DateTime.Now;

    public string FullPath { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public long? SizeBytes { get; set; }

    /// <summary>清理方式显示名（回收站/永久删除/移动/压缩/mkLink）</summary>
    public string Method { get; set; } = string.Empty;

    public bool Success { get; set; }

    public string? Message { get; set; }

    public string SizeText => SizeBytes.HasValue ? FormatSize(SizeBytes.Value) : "-";

    public string ResultText => Success ? "成功" : "失败";

    private static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    public static string getCreateSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS CleanupRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CleanupTime TEXT NOT NULL,
                FullPath TEXT NOT NULL,
                FileName TEXT NOT NULL,
                SizeBytes INTEGER,
                Method TEXT NOT NULL,
                Success INTEGER NOT NULL,
                Message TEXT,
                CreatedAt TEXT NOT NULL
            );";
    }
}
