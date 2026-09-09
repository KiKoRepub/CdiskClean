using CdiskClean.Helpers;
using CdiskClean.Models.cleanUp;

namespace CdiskClean.Services;

public sealed class CleanupClassifier
{
    private readonly IReadOnlyList<CleanupClassificationRule> _rules;

    public CleanupClassifier()
    {
        _rules = new[]
        {
            new CleanupClassificationRule
            {
                Category = CleanupCategory.CrashDumps,
                RiskLevel = RiskLevel.Medium,
                Recommendation = "确认不再需要故障排查后，可优先移入回收站。",
                Extensions = new[] { ".dmp", ".mdmp", ".wer", ".hdmp" },
                PathSegments = new[] { "CrashDumps", "Minidump", "ReportArchive", "ReportQueue" }
            },
            new CleanupClassificationRule
            {
                Category = CleanupCategory.Installers,
                RiskLevel = RiskLevel.High,
                Recommendation = "可能影响应用修复、卸载或系统更新，默认不建议选择。",
                Extensions = new[] { ".msi", ".msp", ".msix", ".appx", ".cab", ".iso" },
                PathSegments = new[] { "Installer", "Package Cache", "SoftwareDistribution" }
            },
            new CleanupClassificationRule
            {
                Category = CleanupCategory.TemporaryFiles,
                RiskLevel = RiskLevel.Low,
                Recommendation = "通常可清理；正在使用的临时文件会在执行前被再次校验。",
                Extensions = new[] { ".tmp", ".temp", ".part", ".download", ".partial" },
                PathSegments = new[] { "Temp", "Tmp" }
            },
            new CleanupClassificationRule
            {
                Category = CleanupCategory.Cache,
                RiskLevel = RiskLevel.Low,
                Recommendation = "清理后应用可能需要重新生成缓存。",
                Extensions = new[] { ".cache", ".cached" },
                PathSegments = new[] { "Cache", "Caches", "INetCache", "Code Cache", "ShaderCache", "WebCache", "Prefetch" }
            },
            new CleanupClassificationRule
            {
                Category = CleanupCategory.Logs,
                RiskLevel = RiskLevel.Low,
                Recommendation = "确认不再需要审计或故障排查后可清理。",
                Extensions = new[] { ".log", ".etl", ".trace", ".evtx" },
                PathSegments = new[] { "Log", "Logs" }
            }
        };
    }

    public CleanupCandidate Classify(CleanupFileEntry entry)
    {
        var rule = _rules.FirstOrDefault(candidate => candidate.IsMatch(entry));
        var category = rule?.Category ?? CleanupCategory.Other;
        var recommendation = rule?.Recommendation ?? "未命中已知规则，请确认用途后再清理。";
        var ruleRisk = rule?.RiskLevel ?? RiskLevel.Medium;
        var pathRisk = entry.IsDirectory
            ? RiskLevelHelper.GetDirectoryRisk(entry.FullPath)
            : MaxRisk(
                RiskLevelHelper.GetDirectoryRisk(Path.GetDirectoryName(entry.FullPath) ?? entry.FullPath),
                RiskLevelHelper.GetFileRisk(entry.FullPath));

        return new CleanupCandidate
        {
            Entry = entry,
            Category = category,
            RiskLevel = MaxRisk(ruleRisk, pathRisk),
            Recommendation = recommendation
        };
    }

    public IReadOnlyList<CleanupCandidate> Classify(IEnumerable<CleanupFileEntry> entries)
    {
        return entries
            .Where(entry => !string.IsNullOrWhiteSpace(entry.FullPath))
            .DistinctBy(entry => Path.GetFullPath(entry.FullPath), StringComparer.OrdinalIgnoreCase)
            .Select(Classify)
            .ToList();
    }

    private static RiskLevel MaxRisk(RiskLevel left, RiskLevel right) =>
        (RiskLevel)Math.Max((int)left, (int)right);
}
