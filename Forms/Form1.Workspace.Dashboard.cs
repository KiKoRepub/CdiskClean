using CdiskClean.Helpers;
using CdiskClean.Models;

namespace CdiskClean;

/// <summary>工作区「工作台」页：磁盘容量卡片、三指标卡片与最近活动表格</summary>
public partial class Form1
{
    private Control BuildDashboardPage()
    {
        var page = CreatePageLayout(3);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var capacity = CreateSurface(new Padding(24, 18, 24, 18));
        var title = new Label
        {
            Text = "C 盘空间",
            AutoSize = true,
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent,
            Location = new Point(24, 18)
        };
        _dashboardUsageLabel = new Label
        {
            Text = "正在读取磁盘信息...",
            AutoSize = false,
            Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent,
            Location = new Point(24, 48),
            Size = new Size(520, 42)
        };
        _dashboardCapacityLabel = new Label
        {
            AutoSize = false,
            Font = new Font("Microsoft YaHei UI", 9.5F),
            ForeColor = UiTheme.TextSecondary,
            BackColor = Color.Transparent,
            Location = new Point(24, 130),
            Size = new Size(720, 28)
        };
        _dashboardDiskProgress = new AntdUI.Progress
        {
            Radius = 4,
            Location = new Point(24, 98),
            Size = new Size(760, 18),
            Value = 0F
        };
        capacity.Controls.Add(title);
        capacity.Controls.Add(_dashboardUsageLabel);
        capacity.Controls.Add(_dashboardCapacityLabel);
        capacity.Controls.Add(_dashboardDiskProgress);
        capacity.Resize += (_, _) =>
        {
            if (_dashboardDiskProgress != null)
                _dashboardDiskProgress.Width = Math.Max(220, capacity.ClientSize.Width - 48);
            if (_dashboardCapacityLabel != null)
                _dashboardCapacityLabel.Width = Math.Max(220, capacity.ClientSize.Width - 48);
        };
        page.Controls.Add(capacity, 0, 0);

        var metrics = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 3,
            RowCount = 1,
            Margin = new Padding(0, 10, 0, 0),
            Padding = Padding.Empty
        };
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        metrics.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        metrics.Controls.Add(CreateMetricSurface("监控状态", out _dashboardMonitorMetric), 0, 0);
        metrics.Controls.Add(CreateMetricSurface("当前记录", out _dashboardRecordMetric), 1, 0);
        metrics.Controls.Add(CreateMetricSurface("生效规则", out _dashboardRuleMetric), 2, 0);
        page.Controls.Add(metrics, 0, 1);

        var recent = CreateSurface(new Padding(16, 12, 16, 14));
        var recentTitle = new Label
        {
            Text = "最近活动",
            Dock = DockStyle.Top,
            Height = 34,
            Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft
        };
        _dashboardRecentGrid = CreateReadOnlyGrid();
        _dashboardRecentGrid.AutoGenerateColumns = false;
        _dashboardRecentGrid.Columns.Add(CreateGridColumn("Timestamp", "时间", 20));
        _dashboardRecentGrid.Columns.Add(CreateGridColumn("TypeText", "类型", 12));
        _dashboardRecentGrid.Columns.Add(CreateGridColumn("FileName", "文件名", 28));
        _dashboardRecentGrid.Columns.Add(CreateGridColumn("SourceProcess", "来源进程", 18));
        _dashboardRecentGrid.Columns.Add(CreateGridColumn("Directory", "目录", 35));
        recent.Controls.Add(_dashboardRecentGrid);
        recent.Controls.Add(recentTitle);
        page.Controls.Add(recent, 0, 2);
        return page;
    }

    private Control CreateMetricSurface(string title, out Label valueLabel)
    {
        var surface = CreateSurface(new Padding(18, 10, 18, 10));
        surface.Margin = new Padding(title == "监控状态" ? 0 : 6, 0, title == "生效规则" ? 0 : 6, 0);
        var titleLabel = new Label
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 25,
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = UiTheme.TextSecondary,
            BackColor = Color.Transparent
        };
        valueLabel = new Label
        {
            Text = "-",
            Dock = DockStyle.Fill,
            Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };
        surface.Controls.Add(valueLabel);
        surface.Controls.Add(titleLabel);
        return surface;
    }

    private void RefreshDashboardMetrics()
    {
        if (_dashboardMonitorMetric != null)
        {
            _dashboardMonitorMetric.Text = _monitorService.IsRunning ? "运行中" : "已暂停";
            _dashboardMonitorMetric.ForeColor = _monitorService.IsRunning ? UiTheme.Success : UiTheme.TextSecondary;
        }
        if (_dashboardRecordMetric != null)
            _dashboardRecordMetric.Text = $"{_records.Count:N0} 条";
        if (_dashboardRuleMetric != null)
        {
            var directories = _monitorService.WatchDirectories.Count(d => d.Status == RecordStatusEnum.USING);
            var ignored = _monitorService.IgnoreProcessRecords.Count(p => p.Status == RecordStatusEnum.USING);
            _dashboardRuleMetric.Text = $"{directories} 目录 / {ignored} 进程";
        }

        if (_dashboardRecentGrid != null)
        {
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
            _dashboardRecentGrid.DataSource = rows;
        }
        RefreshWorkspaceStatus();
    }

    private void UpdateWorkspaceDiskStatus(DriveInfoModel info)
    {
        if (_dashboardDiskProgress != null)
            _dashboardDiskProgress.Value = (float)Math.Min(info.UsagePercent / 100D, 1D);
        if (_dashboardUsageLabel != null)
        {
            _dashboardUsageLabel.Text = $"{info.UsagePercent:0.#}% 已使用  ·  剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)}";
            _dashboardUsageLabel.ForeColor = info.UsagePercent switch
            {
                > 90 => UiTheme.Danger,
                > 70 => Color.FromArgb(217, 119, 6),
                _ => UiTheme.TextPrimary
            };
        }
        if (_dashboardCapacityLabel != null)
        {
            _dashboardCapacityLabel.Text =
                $"总容量 {FormatHelper.FormatBytes(info.TotalSizeBytes)}    已用 {FormatHelper.FormatBytes(info.UsedSpaceBytes)}    剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)}";
        }
        if (_workspaceDiskStatus != null)
        {
            _workspaceDiskStatus.Text = $"C: 剩余 {FormatHelper.FormatBytes(info.FreeSpaceBytes)} / {FormatHelper.FormatBytes(info.TotalSizeBytes)}";
            _workspaceDiskStatus.ForeColor = info.IsLowSpace ? UiTheme.Danger : UiTheme.TextSecondary;
        }
    }

    private void RefreshWorkspaceStatus()
    {
        if (_workspaceMonitorStatus != null)
        {
            var directoryCount = _monitorService.WatchDirectories.Count(d => d.Status == RecordStatusEnum.USING);
            _workspaceMonitorStatus.Text = _monitorService.IsRunning
                ? $"监控运行中 · {directoryCount} 个目录"
                : $"监控已暂停 · {directoryCount} 个目录";
            _workspaceMonitorStatus.ForeColor = _monitorService.IsRunning
                ? UiTheme.Success
                : UiTheme.TextSecondary;
        }
        if (_workspaceRecordStatus != null)
            _workspaceRecordStatus.Text = $"当前记录 {_records.Count:N0} 条";
        if (_workspaceClockStatus != null)
            _workspaceClockStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
