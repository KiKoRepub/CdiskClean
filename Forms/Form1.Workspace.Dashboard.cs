using CdiskClean.Helpers;
using CdiskClean.Models;

namespace CdiskClean;

/// <summary>工作区「工作台」页逻辑：磁盘容量、三指标与最近活动表格刷新</summary>
public partial class Form1
{
    private void RefreshDashboardMetrics()
    {
        dashboardMonitorMetric.Text = _monitorService.IsRunning ? "运行中" : "已暂停";
        dashboardMonitorMetric.ForeColor = _monitorService.IsRunning ? UiTheme.Success : UiTheme.TextSecondary;
        dashboardRecordMetric.Text = $"{_records.Count:N0} 条";

        var directories = _monitorService.WatchDirectories.Count(d => d.Status == RecordStatusEnum.USING);
        var ignored = _monitorService.IgnoreProcessRecords.Count(p => p.Status == RecordStatusEnum.USING);
        dashboardRuleMetric.Text = $"{directories} 目录 / {ignored} 进程";

        List<DashboardActivityRow> rows;
        lock (_recordsLock)
        {
            rows = _records.Take(8).Select(r => new DashboardActivityRow
            {
                Timestamp = r.Timestamp,
                TypeText = EnumHelper.FormatChangeType(r.ChangeType),
                FileName = r.FileName,
                SourceProcess = r.SourceProcess ?? "未知进程",
                Directory = r.Directory
            }).ToList();
        }
        dashboardRecentGrid.DataSource = rows;

        RefreshWorkspaceStatus();
    }

    private void UpdateWorkspaceDiskStatus(DriveInfoModel info)
    {
        dashboardDiskProgress.Value = (int)Math.Round(Math.Min(info.UsagePercent, 100D));
        dashboardUsageLabel.Text = $"{info.UsagePercent:0.#}% 已使用  ·  剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)}";
        dashboardUsageLabel.ForeColor = info.UsagePercent switch
        {
            > 90 => UiTheme.Danger,
            > 70 => Color.FromArgb(217, 119, 6),
            _ => UiTheme.TextPrimary
        };
        // 更新进度条

        dashboardDiskProgress.Value = (float)(info.UsagePercent / 100);
        
        dashboardDiskProgress.ForeColor = info.UsagePercent switch
        {
            > 90 => UiTheme.Danger,
            > 70 => Color.FromArgb(217, 119, 6),
            _ => UiTheme.Primary
        };
        //LogHelper.showDefaultToDoMessage("进度条颜色渐变还没做呢，等一等");
        //dashboardDiskProgress

        dashboardCapacityLabel.Text =
            $"总容量 {FormatHelper.FormatBytes(info.TotalSizeBytes)}    已用 {FormatHelper.FormatBytes(info.UsedSpaceBytes)}    剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)}";
        workspaceDiskStatus.Text = $"C: 剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)} / {FormatHelper.FormatBytes(info.TotalSizeBytes)}";
        workspaceDiskStatus.ForeColor = info.IsLowSpace ? UiTheme.Danger : UiTheme.TextSecondary;
    }

    private void RefreshWorkspaceStatus()
    {
        var directoryCount = _monitorService.WatchDirectories.Count(d => d.Status == RecordStatusEnum.USING);
        workspaceMonitorStatus.Text = _monitorService.IsRunning
            ? $"监控运行中 · {directoryCount} 个目录"
            : $"监控已暂停 · {directoryCount} 个目录";
        workspaceMonitorStatus.ForeColor = _monitorService.IsRunning
            ? UiTheme.Success
            : UiTheme.TextSecondary;
        workspaceRecordStatus.Text = $"当前记录 {_records.Count:N0} 条";
        workspaceClockStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
