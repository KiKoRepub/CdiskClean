namespace CdiskClean.Models;

/// <summary>
/// 高频修改文件路径参考（按目录分组统计变更记录）
/// </summary>
public class FrequentPathInfo
{
    public string Path { get; set; } = string.Empty;

    public int ChangeCount { get; set; }

    public DateTime LastChangeTime { get; set; }
}
