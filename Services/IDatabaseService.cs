using CdiskClean.Models;
using CdiskClean.Models.cleanUp;
using CdiskClean.Models.rules;

namespace CdiskClean.Services;

public interface IDatabaseService
{
    void Initialize();
    #region WatchingDirectory
    List<WatchingDirectory> GetWatchDirectories();
    void SaveWatchDirectory(WatchingDirectory dir);
    void DeleteWatchDirectory(string path);
    #endregion
    #region FileChangeRecord
    void SaveChangeRecord(FileChangeRecord record);
    List<FileChangeRecord> GetChangeRecords(int limit = 1000);
    List<FileChangeRecord> GetChangeRecordsUnderPath(string path, int limit = 100);
    #endregion
    #region ProcessNotificationRecord
    void SaveProcessNotification(ProcessNotificationRecord record);
    List<ProcessNotificationRecord> GetProcessNotifications(int limit = 200);
    #endregion
    #region IgnoreProcessRecord
    void SaveIgnoreProcessRecord(IgnoreProcessRecord record);
    List<IgnoreProcessRecord> GetIgnoreProcessRecords(int limit = 200);
    void DeleteIgnoreProcessRecord(string processName);
    #endregion
    #region WatchingExeInfo
    List<WatchingExeInfo> GetWatchingApplications();
    void SaveWatchingApplication(WatchingExeInfo application);
    void DeleteWatchingApplication(string fullPath);
    void UpdateWatchingApplicationActivity(string exeName, DateTime activityTime);
    #endregion
    #region CleanupRecord
    void SaveCleanupRecord(CleanupRecord record);
    List<CleanupRecord> GetCleanupRecords(int limit = 200);
    #endregion
}
