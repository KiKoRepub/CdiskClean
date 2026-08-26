using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

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

        BindingList<DashboardActivityRow> rows;
        lock (_recordsLock)
        {
            rows = new (_records.Take(8).Select(r => new DashboardActivityRow
            {
                Timestamp = r.Timestamp,
                TypeText = EnumHelper.FormatChangeType(r.ChangeType),
                FileName = r.FileName,
                SourceProcess = r.SourceProcess ?? "未知进程",
                Directory = r.Directory
            }).ToList());
        }
        dashboardRecentTable.DataSource = rows;

        RefreshWorkspaceStatus();
    }

    private void UpdateWorkspaceDiskStatus(DriveInfoModel info)
    {
        dashboardDiskProgress.Value = (int)Math.Round(Math.Min(info.UsagePercent, 100D));
        
        dashboardUsageLabel.Text = $"{info.UsagePercent:0.#}% 已使用  ·  剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)}";
        Color diskUsageColor = info.UsagePercent switch
        {
            > 90 => UiTheme.Danger,
            > 70 => Color.FromArgb(217, 119, 6),
            _ => UiTheme.Primary
        };

        dashboardUsageLabel.ForeColor = diskUsageColor;

        // 更新进度条
        dashboardDiskProgress.Value = (float)(info.UsagePercent / 100);
        dashboardDiskProgress.ForeColor = diskUsageColor;


        dashboardDiskProgress.Fill = diskUsageColor;

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


    #region 实体
    private sealed class DashboardActivityRow
    {
        public DateTime Timestamp { get; init; }
        public string TypeText { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string SourceProcess { get; init; } = string.Empty;
        public string Directory { get; init; } = string.Empty;
    }
    #endregion 
}
