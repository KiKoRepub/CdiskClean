using System.Configuration;

namespace CdiskClean.Models;

public class DriveInfoModel
{
    public long TotalSizeBytes { get; set; }
    public long FreeSpaceBytes { get; set; }
    public long UsedSpaceBytes => TotalSizeBytes - FreeSpaceBytes;

    public double UsagePercent =>
        TotalSizeBytes > 0 ? (double)UsedSpaceBytes / TotalSizeBytes * 100 : 0;

    public bool IsLowSpace =>
        FreeSpaceBytes < 10L * 1024 * 1024 * 1024; // < 10 GB
}
