namespace CdiskClean.Models;

public class ProcessNotificationRecord
{
    public string ProcessName { get; set; } = string.Empty;

    public int OperationCount { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime TriggerTime { get; set; }

    public static string getCreateSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS ProcessNotifications (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProcessName TEXT NOT NULL,
                OperationCount INTEGER NOT NULL,
                DurationSeconds INTEGER NOT NULL,
                TriggerTime TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
            );";
    }
}
