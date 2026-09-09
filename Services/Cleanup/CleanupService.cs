using CdiskClean.Services.database;
using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.cleanUp;
using System.Diagnostics;

namespace CdiskClean.Services;

/// <summary>
/// 协调清理批次、分类统计和日志入库，维护清理期间的监控过滤状态。
/// </summary>
public class CleanupService
{
    private readonly HistoryStore _databaseService;
    private readonly CleanupClassifier _classifier;

    // 清理期间被操作的原路径快照 + 目标目录，用于让 FSW/ETW 监控忽略本次清理产生的事件
    private volatile string[] _activePathSnapshot = Array.Empty<string>();
    private volatile string? _activeTargetDir;

    public CleanupService(HistoryStore databaseService, CleanupClassifier? classifier = null)
    {
        _databaseService = databaseService;
        _classifier = classifier ?? new CleanupClassifier();
    }

    public CleanupCandidate Classify(CleanupFileEntry entry) => _classifier.Classify(entry);

    public IReadOnlyList<CleanupCandidate> Classify(IEnumerable<CleanupFileEntry> entries) =>
        _classifier.Classify(entries);

    #region 监控联动（清理期间过滤自身事件）

    /// <summary>清理开始前调用：登记被清理的原路径与目标目录</summary>
    public void BeginCleanup(IEnumerable<CleanupFileEntry> entries, string? targetDir)
    {
        _activePathSnapshot = entries
            .Select(e => Path.GetFullPath(e.FullPath))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        _activeTargetDir = string.IsNullOrWhiteSpace(targetDir)
            ? null
            : Path.GetFullPath(targetDir);
    }

    public void EndCleanup()
    {
        _activePathSnapshot = Array.Empty<string>();
        _activeTargetDir = null;
    }

    /// <summary>
    /// 监控服务在产生记录前调用：路径位于本次清理的原路径（或其子项）内、
    /// 或位于目标目录内，都应忽略，避免"自己监听到自己"污染统计与提醒。
    /// </summary>
    public bool ShouldIgnoreEvent(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;

        var target = _activeTargetDir;
        if (target != null && PathHelper.IsPathInside(path, target)) return true;

        foreach (var p in _activePathSnapshot)
        {
            if (PathHelper.IsPathInside(path, p)) return true;
        }
        return false;
    }

    #endregion

    #region 高频修改路径参考

    /// <summary>按目录分组统计变更记录，返回变更最频繁的目录</summary>
    public static List<FrequentPathInfo> GetFrequentPaths(
        IEnumerable<FileChangeRecord> records,
        int topN = 30)
    {
        return records
            .Where(r => !string.IsNullOrEmpty(r.Directory))
            .GroupBy(r => r.Directory)
            .Select(g => new FrequentPathInfo
            {
                Path = g.Key,
                ChangeCount = g.Count(),
                LastChangeTime = g.Max(r => r.Timestamp)
            })
            .OrderByDescending(f => f.ChangeCount)
            .Take(topN)
            .ToList();
    }

    #endregion
    
    #region 清理执行

    public static string GetMethodDisplayName(CleanupMethod method) => method switch
    {
        CleanupMethod.RecycleBin => "回收站",
        CleanupMethod.PermanentDelete => "永久删除",
        CleanupMethod.Move => "移动",
        CleanupMethod.Compress => "压缩",
        CleanupMethod.Mklink => "mkLink",
        _ => "未知"
    };

    /// <summary>清理方式是否需要目标目录</summary>
    public static bool RequiresTarget(CleanupMethod method) =>
        method is CleanupMethod.Move or CleanupMethod.Compress or CleanupMethod.Mklink;

    /// <summary>
    /// 逐项执行清理：安全校验 → 执行 → 日志入库。整个过程在后台线程运行。
    /// </summary>
    public async Task<CleanupResult> ExecuteAsync(
        IReadOnlyList<CleanupFileEntry> entries,
        CleanupMethod method,
        string? targetDir,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (entries == null || entries.Count == 0)
            return new CleanupResult();

        BeginCleanup(entries, targetDir);
        try
        {
            return await Task.Run(() =>
            {
                var result = new CleanupResult { Total = entries.Count };
                for (int i = 0; i < entries.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var entry = entries[i];
                    var candidate = _classifier.Classify(entry);
                    progress?.Report($"正在清理 ({i + 1}/{entries.Count}): {entry.FullPath}");

                    var ok = CleanupFileOperations.Execute(entry, method, targetDir, out var error, out var freed);
                    if (ok)
                    {
                        result.Success++;
                        result.FreedBytes += freed;
                    }
                    else
                    {
                        result.Fail++;
                        progress?.Report($"清理失败: {entry.FullPath} - {error}");
                    }

                    if (!result.CategoryResults.TryGetValue(candidate.Category, out var categoryResult))
                    {
                        categoryResult = new CleanupCategoryResult { Category = candidate.Category };
                        result.CategoryResults[candidate.Category] = categoryResult;
                    }
                    if (ok) categoryResult.Success++;
                    else categoryResult.Fail++;

                    SaveRecord(entry, method, ok, error);
                }
                return result;
            }, cancellationToken);
        }
        finally
        {
            EndCleanup();
        }
    }

    private void SaveRecord(CleanupFileEntry entry, CleanupMethod method, bool success, string? message)
    {
        try
        {
            var candidate = _classifier.Classify(entry);
            _databaseService.SaveCleanupRecord(new CleanupRecord
            {
                CleanupTime = DateTime.Now,
                FullPath = entry.FullPath,
                FileName = entry.Name,
                SizeBytes = entry.IsDirectory ? (long?)null : entry.SizeBytes,
                Method = GetMethodDisplayName(method),
                Category = candidate.CategoryText,
                Success = success,
                Message = message
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"保存清理记录失败: {ex.Message}");
        }
    }

    #endregion
}
