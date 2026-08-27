namespace CdiskClean.Helpers;

/// <summary>清理对象的风险等级（用于清理树节点背景色与图例展示）</summary>
public enum RiskLevel
{
    Low,
    Medium,
    High
}

/// <summary>
/// 风险等级配色与常见风险目录规则。
/// 清理树节点背景（CreateFileTreeItem → SetBack）与 cleanupSelectionBar 右侧图例共用此处的颜色，
/// 修改颜色时需同步 Form1.Designer.cs 中图例色块的字面值。
/// </summary>
public static class RiskLevelHelper
{
    /// <summary>高风险（红）</summary>
    public static readonly Color HighColor = Color.FromArgb(255, 230, 230);

    /// <summary>中风险（黄）</summary>
    public static readonly Color MediumColor = Color.FromArgb(255, 255, 220);

    /// <summary>低风险（绿）</summary>
    public static readonly Color LowColor = Color.FromArgb(230, 255, 230);

    /// <summary>高风险目录段名：路径中任意一段命中即视为高风险（系统关键目录）</summary>
    /// <summary>高风险目录段名：修改可能导致系统不稳定或无法启动</summary>
    private static readonly string[] HighDirSegments = new string[]
    {
    // 原有
    "Windows", "Program Files", "Program Files (x86)", "ProgramData",
    "$Recycle.Bin", "System Volume Information", "WindowsApps",
    // 补充
    "System32", "SysWOW64", "Boot", "Drivers", "INF", "Fonts", "winsxs",
    "servicing", "Installer", "CSC", "Registration", "Tasks",
    "Microsoft.NET", "PolicyDefinitions", "SystemResources", "AppCompat",
    "Config"
    };

    /// <summary>中风险目录段名：修改可能影响应用数据或用户配置，但系统基本稳定</summary>
    private static readonly string[] MediumDirSegments = new string[]
    {
    // 原有
    "Temp", "Cache", "Prefetch", "INetCache", "SoftwareDistribution",
    // 补充
    "AppData", "Local", "Roaming", "LocalLow", "Desktop", "Documents",
    "Downloads", "Pictures", "Music", "Videos", "OneDrive", "Public",
    "Favorites", "Start Menu", "Startup", "Recent", "SendTo", "PerfLogs"
    };

    /// <summary>高风险文件扩展名：可执行或系统关键文件</summary>
    private static readonly string[] HighFileExtensions = new string[]
    {
    // 原有
    ".sys", ".dll", ".exe", ".drv",
    // 补充
    ".ini", ".inf", ".msi", ".msp", ".com", ".scr", ".cpl", ".ocx",
    ".vxd", ".efi", ".bin"
    };

    /// <summary>中风险文件扩展名：可能含脚本或配置，修改有潜在风险</summary>
    private static readonly string[] MediumFileExtensions = new string[]
    {
    // 原有
    ".tmp", ".log", ".cache", ".bak", ".old",
    // 补充
    ".dat", ".xml", ".json", ".reg", ".vbs", ".ps1", ".js", ".cmd",
    ".bat", ".jar", ".swf"
    };

    public static Color GetColor(RiskLevel level) => level switch
    {
        RiskLevel.High => HighColor,
        RiskLevel.Medium => MediumColor,
        _ => LowColor
    };

    /// <summary>
    /// 按目录路径"段"匹配风险等级（高优先于中）。
    /// 使用段匹配而非 Contains("\\windows\\")，可正确命中 C:\Windows 本身及其子目录。
    /// </summary>
    public static RiskLevel GetDirectoryRisk(string fullPath)
    {
        var segs = fullPath.TrimEnd('\\').Split('\\', StringSplitOptions.RemoveEmptyEntries);
        if (segs.Any(seg => HighDirSegments.Contains(seg, StringComparer.OrdinalIgnoreCase)))
            return RiskLevel.High;
        if (segs.Any(seg => MediumDirSegments.Contains(seg, StringComparer.OrdinalIgnoreCase)))
            return RiskLevel.Medium;
        return RiskLevel.Low;
    }

    /// <summary>按文件扩展名匹配风险等级</summary>
    public static RiskLevel GetFileRisk(string fullPath)
    {
        var ext = Path.GetExtension(fullPath).ToLowerInvariant();
        if (HighFileExtensions.Contains(ext)) return RiskLevel.High;
        if (MediumFileExtensions.Contains(ext)) return RiskLevel.Medium;
        return RiskLevel.Low;
    }
}
