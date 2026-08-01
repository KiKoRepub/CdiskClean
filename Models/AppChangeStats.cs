namespace CdiskClean.Models;

public class AppChangeStats
{
    public string AppName { get; set; } = string.Empty;

    public int ChangeCount { get; set; }

    public DateTime LastChangeTime { get; set; }

    public DateTime FirstChangeTime { get; set; }
}
