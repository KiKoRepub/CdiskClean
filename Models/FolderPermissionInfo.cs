namespace CdiskClean.Models;

public class FolderPermissionInfo
{
    public string Path { get; init; } = string.Empty;
    public string Status { get; init; } = "未知";
    public string? Owner { get; init; }
    public int RuleCount { get; init; }
    public bool CanRead { get; init; }
    public string? ErrorMessage { get; init; }
}
