namespace CdiskClean.Models;

public sealed class DashboardSnapshot
{
    public int TodayNotificationCount { get; init; }
    public int NotificationProcessCount { get; init; }
    public DateTime? LastNotificationAt { get; init; }
    public int RecentChangeCount { get; init; }
    public int RecentCleanupCount { get; init; }
    public DateTime? LastCleanupAt { get; init; }
    public string? QuickCleanupPath { get; init; }
    public int QuickCleanupChangeCount { get; init; }
}
