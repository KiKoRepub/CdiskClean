namespace CdiskClean.Models.cleanUp;

/// <summary>
/// 扫描得到的可清理文件/目录条目
/// </summary>
public class CleanupFileEntry
{
    public string FullPath { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    /// <summary>文件大小；目录为其下所有文件大小总和</summary>
    public long SizeBytes { get; set; }

    public DateTime? LastWriteTime { get; set; }

    public bool IsDirectory { get; set; }
}
