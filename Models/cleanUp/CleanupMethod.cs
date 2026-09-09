namespace CdiskClean.Models.cleanUp;

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

