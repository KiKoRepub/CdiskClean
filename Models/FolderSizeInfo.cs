namespace CdiskClean.Models;

public class FolderSizeInfo
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int FileCount { get; set; }
    public List<FolderSizeInfo> SubFolders { get; set; } = new();
    public FolderAccessStatus AccessStatus { get; set; } = FolderAccessStatus.Accessible;
    public string? ErrorMessage { get; set; }
    public DateTime LastScannedAt { get; set; }
    public int InaccessibleCount { get; set; }
    public Dictionary<string, long> ExtensionSizes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public enum FolderAccessStatus
{
    Accessible,
    Partial,
    Denied,
    Missing,
    Error
}
