using CdiskClean.Helpers;

namespace CdiskClean.Models.cleanUp;

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

    public string Category { get; set; } = CleanupCategory.Other.GetDisplayName();

    public bool Success { get; set; }

    public string? Message { get; set; }

    public string SizeText => SizeBytes.HasValue ? FormatHelper.FormatBytes(SizeBytes.Value) : "-";

    public string ResultText => Success ? "成功" : "失败";

}
