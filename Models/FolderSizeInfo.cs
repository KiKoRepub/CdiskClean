namespace CdiskClean.Models;

public class FolderSizeInfo
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int FileCount { get; set; }
    public List<FolderSizeInfo> SubFolders { get; set; } = new();
}
