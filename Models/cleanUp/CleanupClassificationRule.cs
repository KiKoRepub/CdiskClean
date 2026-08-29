using CdiskClean.Helpers;

namespace CdiskClean.Models.cleanUp;

public sealed class CleanupClassificationRule
{
    public required CleanupCategory Category { get; init; }
    public required RiskLevel RiskLevel { get; init; }
    public required string Recommendation { get; init; }
    public string[] Extensions { get; init; } = Array.Empty<string>();
    public string[] PathSegments { get; init; } = Array.Empty<string>();

    public bool IsMatch(CleanupFileEntry entry)
    {
        var extension = entry.IsDirectory ? string.Empty : Path.GetExtension(entry.FullPath);
        if (Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            return true;

        var segments = entry.FullPath
            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Where(segment => !string.IsNullOrWhiteSpace(segment));
        return segments.Any(segment =>
            PathSegments.Contains(segment, StringComparer.OrdinalIgnoreCase));
    }
}
