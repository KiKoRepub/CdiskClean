using CdiskClean.Helpers;

namespace CdiskClean.Models.cleanUp;

public sealed class CleanupCandidate
{
    public required CleanupFileEntry Entry { get; init; }
    public required CleanupCategory Category { get; init; }
    public required RiskLevel RiskLevel { get; init; }
    public required string Recommendation { get; init; }

    public string CategoryText => Category.GetDisplayName();
    public string RiskText => RiskLevel switch
    {
        RiskLevel.High => "高风险",
        RiskLevel.Medium => "中风险",
        _ => "低风险"
    };
}
