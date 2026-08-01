namespace CdiskClean.Models;

public class ProcessNotificationRecord
{
    public string ProcessName { get; set; } = string.Empty;

    public int OperationCount { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime TriggerTime { get; set; }
}
