using CdiskClean.Models;

namespace CdiskClean.Services;

public sealed class DashboardQueryService
{
    private readonly IDatabaseService _databaseService;

    public DashboardQueryService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public DashboardSnapshot GetSnapshot()
    {
        var notifications = _databaseService.GetProcessNotifications(500);
        var cleanups = _databaseService.GetCleanupRecords(5000);
        var changes = _databaseService.GetChangeRecords(5000);
        var today = DateTime.Today;
        var todayNotifications = notifications
            .Where(record => record.TriggerTime >= today)
            .ToList();
        var quickPath = CleanupService.GetFrequentPaths(changes, 1).FirstOrDefault();

        return new DashboardSnapshot
        {
            TodayNotificationCount = todayNotifications.Count,
            NotificationProcessCount = todayNotifications
                .Select(record => record.ProcessName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count(),
            LastNotificationAt = notifications.FirstOrDefault()?.TriggerTime,
            RecentChangeCount = changes.Count,
            RecentCleanupCount = cleanups.Count,
            LastCleanupAt = cleanups.FirstOrDefault()?.CleanupTime,
            QuickCleanupPath = quickPath?.Path,
            QuickCleanupChangeCount = quickPath?.ChangeCount ?? 0
        };
    }
}
