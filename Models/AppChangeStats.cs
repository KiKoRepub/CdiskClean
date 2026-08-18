namespace CdiskClean.Models;

public class AppChangeStats
{
    public string AppName { get; set; } = string.Empty;

    public int ChangeCount { get; set; }

    public DateTime LastChangeTime { get; set; }

    public DateTime FirstChangeTime { get; set; }

    /// <summary>按来源进程聚合统计（未知进程统一归并为"未知进程"）</summary>
    public static List<AppChangeStats> BuildFrom(IEnumerable<FileChangeRecord> records)
    {
        return records
            .GroupBy(r => string.IsNullOrWhiteSpace(r.SourceProcess) ? "未知进程" : r.SourceProcess,
                StringComparer.OrdinalIgnoreCase)
            .Select(g => new AppChangeStats
            {
                AppName = g.Key,
                ChangeCount = g.Count(),
                FirstChangeTime = g.Min(r => r.Timestamp),
                LastChangeTime = g.Max(r => r.Timestamp)
            })
            .OrderByDescending(s => s.ChangeCount)
            .ThenByDescending(s => s.LastChangeTime)
            .ToList();
    }
}
