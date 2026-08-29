namespace CdiskClean.Models.cleanUp;

public enum CleanupCategory
{
    TemporaryFiles,
    Cache,
    Logs,
    CrashDumps,
    Installers,
    Other
}

public static class CleanupCategoryExtensions
{
    public static string GetDisplayName(this CleanupCategory category) => category switch
    {
        CleanupCategory.TemporaryFiles => "临时文件",
        CleanupCategory.Cache => "缓存",
        CleanupCategory.Logs => "日志",
        CleanupCategory.CrashDumps => "崩溃转储",
        CleanupCategory.Installers => "安装包/更新残留",
        _ => "其他"
    };
}
