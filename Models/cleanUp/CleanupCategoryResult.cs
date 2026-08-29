namespace CdiskClean.Models.cleanUp;

public sealed class CleanupCategoryResult
{
    public CleanupCategory Category { get; init; }
    public int Success { get; set; }
    public int Fail { get; set; }
}
