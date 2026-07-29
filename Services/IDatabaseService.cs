using CdiskClean.Models;

namespace CdiskClean.Services;

public interface IDatabaseService
{
    void Initialize();
    List<WatchingDirectory> GetWatchDirectories();
    void SaveWatchDirectory(WatchingDirectory dir);
    void DeleteWatchDirectory(string path);
    void SaveChangeRecord(FileChangeRecord record);
    List<FileChangeRecord> GetChangeRecords(int limit = 1000);
    void ClearChangeRecords();
}
