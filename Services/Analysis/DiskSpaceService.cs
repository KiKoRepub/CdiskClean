using CdiskClean.Models;

namespace CdiskClean.Services;

public class DiskSpaceService
{
    public DriveInfoModel GetDriveInfo(string driveName = "C:")
    {
        var drive = new DriveInfo(driveName);
        if (!drive.IsReady)
            throw new InvalidOperationException($"驱动器 {driveName} 未就绪。");

        return new DriveInfoModel
        {
            TotalSizeBytes = drive.TotalSize,
            FreeSpaceBytes = drive.AvailableFreeSpace
        };
    }
}
