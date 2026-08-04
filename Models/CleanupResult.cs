namespace CdiskClean.Models;

/// <summary>
/// 一次清理执行的汇总结果
/// </summary>
public class CleanupResult
{
    public int Total { get; set; }

    public int Success { get; set; }

    public int Fail { get; set; }

    /// <summary>估算释放的空间（永久删除=文件大小；压缩=原大小-压缩包大小）</summary>
    public long FreedBytes { get; set; }
}
