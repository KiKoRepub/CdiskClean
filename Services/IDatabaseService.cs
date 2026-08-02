using CdiskClean.Models;

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
    void ClearChangeRecords();
    #endregion
    #region ProcessNotificationRecord
    void SaveProcessNotification(ProcessNotificationRecord record);
    List<ProcessNotificationRecord> GetProcessNotifications(int limit = 200);
    #endregion
    #region IgnoreProcessRecord
     void SaveIgnoreProcessRecord(IgnoreProcessRecord record);
    List<IgnoreProcessRecord> GetIgnoreProcessRecords(int limit = 200);
    #endregion
}
