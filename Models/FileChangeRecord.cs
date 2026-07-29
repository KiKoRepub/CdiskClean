namespace CdiskClean.Models;

public enum ChangeType
{
    Created,
    Changed,
    Deleted,
    Renamed
}

public class FileChangeRecord
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public ChangeType ChangeType { get; set; }
    public string FullPath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;
    public long? SizeBytes { get; set; }
}
