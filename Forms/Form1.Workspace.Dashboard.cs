using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.rules;
using System.ComponentModel;
using System.Diagnostics;

namespace CdiskClean;

/// <summary>工作区「工作台」页逻辑：磁盘容量、三指标与最近活动表格刷新</summary>
public partial class Form1
{
    private AntdUI.Panel? _dashboardNotificationSurface;
    private Label? _dashboardNotificationTitle;
    private Label? _dashboardNotificationMetric;
    private Label? _dashboardHealthLabel;
    private AntdUI.Button? _dashboardQuickCleanupButton;
    private ToolTip? _dashboardToolTip;
    private string? _dashboardQuickCleanupPath;
    private CancellationTokenSource? _dashboardQuickCleanupCts;
    private int _dashboardSnapshotVersion;

    private void SetupDashboardEnhancements()
    {
        dashboardMetrics.ColumnCount = 4;
        dashboardMetrics.ColumnStyles.Clear();
        for (var index = 0; index < 4; index++)
            dashboardMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        dashboardMonitorSurface.Margin = new Padding(0, 0, 6, 0);
        dashboardRecordSurface.Margin = new Padding(6, 0, 6, 0);
        dashboardRuleSurface.Margin = new Padding(6, 0, 6, 0);

        _dashboardNotificationSurface = new AntdUI.Panel
        {
            BackColor = Color.White,
            Dock = DockStyle.Fill,
            Margin = new Padding(6, 0, 0, 0),
            Cursor = Cursors.Hand
        };
        _dashboardNotificationTitle = new Label
        {
            BackColor = Color.Transparent,
            Dock = DockStyle.Top,
            Height = 25,
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = UiTheme.TextSecondary,
            Text = "今日通知",
            TextAlign = ContentAlignment.MiddleLeft,
            Cursor = Cursors.Hand
        };
        _dashboardNotificationMetric = new Label
        {
            AutoEllipsis = true,
            BackColor = Color.Transparent,
            Dock = DockStyle.Fill,
            Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            Text = "读取中...",
            TextAlign = ContentAlignment.MiddleLeft,
            Cursor = Cursors.Hand
        };
        _dashboardNotificationSurface.Controls.Add(_dashboardNotificationMetric);
        _dashboardNotificationSurface.Controls.Add(_dashboardNotificationTitle);
        _dashboardNotificationSurface.Click += dashboardNotification_Click;
        _dashboardNotificationTitle.Click += dashboardNotification_Click;
        _dashboardNotificationMetric.Click += dashboardNotification_Click;
        dashboardMetrics.Controls.Add(_dashboardNotificationSurface, 3, 0);

        _dashboardQuickCleanupButton = new AntdUI.Button
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(Math.Max(24, dashboardCapacitySurface.Width - 210), 16),
            Size = new Size(186, 38),
            Radius = 4,
            Text = "快捷清理",
            Type = AntdUI.TTypeMini.Primary
        };
        _dashboardQuickCleanupButton.Click += dashboardQuickCleanupButton_Click;
        dashboardCapacitySurface.Controls.Add(_dashboardQuickCleanupButton);
        _dashboardQuickCleanupButton.BringToFront();

        _dashboardHealthLabel = new Label
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            BackColor = Color.Transparent,
            ForeColor = UiTheme.TextSecondary,
            Font = new Font("Microsoft YaHei UI", 8.5F),
            Location = new Point(24, 158),
            Size = new Size(Math.Max(200, dashboardCapacitySurface.Width - 48), 24),
            Text = "健康状态：正在读取...",
            TextAlign = ContentAlignment.MiddleLeft
        };
        dashboardCapacitySurface.Controls.Add(_dashboardHealthLabel);
        _dashboardHealthLabel.BringToFront();
        _dashboardToolTip = new ToolTip();
    }

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

        if (_dashboardHealthLabel != null)
        {
            var pendingCleanup = cleanTreeView.Items.Count == 0 ? 0 : GetCheckedEntries().Count;
            _dashboardHealthLabel.Text =
                $"健康状态：监控{(_monitorService.IsRunning ? "运行中" : "已暂停")} · 活动 {_records.Count:N0} 条 · 待清理 {pendingCleanup:N0} 项 · 数据库正常";
        }

        RefreshWorkspaceStatus();
    }

    private async void RefreshDashboardInsightsAsync()
    {
        if (_dashboardNotificationMetric == null || _dashboardQuickCleanupButton == null) return;
        var version = Interlocked.Increment(ref _dashboardSnapshotVersion);
        try
        {
            var snapshot = await Task.Run(_dashboardQueryService.GetSnapshot).ConfigureAwait(false);
            ApplyDashboardSnapshot(snapshot, version);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"刷新工作台摘要失败: {ex.Message}");
            ApplyDashboardSnapshotError(version);
        }
    }

    private void ApplyDashboardSnapshot(DashboardSnapshot snapshot, int version)
    {
        if (IsDisposed || version != _dashboardSnapshotVersion) return;
        if (!IsHandleCreated)
        {
            Load += (_, _) => ApplyDashboardSnapshot(snapshot, version);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => ApplyDashboardSnapshot(snapshot, version));
            return;
        }

        _dashboardQuickCleanupPath = snapshot.QuickCleanupPath;
        if (string.IsNullOrWhiteSpace(_dashboardQuickCleanupPath))
        {
            _dashboardQuickCleanupPath = _monitorService.WatchDirectories
                .Where(directory => directory.Status == RecordStatusEnum.USING && Directory.Exists(directory.Path))
                .Select(directory => directory.Path)
                .FirstOrDefault();
        }

        _dashboardNotificationMetric!.Text =
            $"{snapshot.TodayNotificationCount:N0} 条 / {snapshot.NotificationProcessCount:N0} 进程";
        _dashboardNotificationMetric.ForeColor = snapshot.TodayNotificationCount > 0
            ? UiTheme.Primary
            : UiTheme.TextSecondary;
        _dashboardToolTip?.SetToolTip(_dashboardNotificationSurface!,
            snapshot.LastNotificationAt.HasValue
                ? $"最近触发：{snapshot.LastNotificationAt:yyyy-MM-dd HH:mm:ss}\n点击打开记录中心"
                : "暂无提醒记录，点击打开记录中心");

        _dashboardQuickCleanupButton!.Enabled = !string.IsNullOrWhiteSpace(_dashboardQuickCleanupPath);
        _dashboardQuickCleanupButton.Text = string.IsNullOrWhiteSpace(_dashboardQuickCleanupPath)
            ? "暂无快捷路径"
            : "快捷清理";
        _dashboardToolTip?.SetToolTip(_dashboardQuickCleanupButton,
            string.IsNullOrWhiteSpace(_dashboardQuickCleanupPath)
                ? "暂无监控目录或历史变更路径"
                : $"{_dashboardQuickCleanupPath}\n近期变更 {snapshot.QuickCleanupChangeCount:N0} 次");

        dashboardRecordMetric.Text = $"{Math.Max(_records.Count, snapshot.RecentChangeCount):N0} 条";
        if (_dashboardHealthLabel != null)
        {
            var pendingCleanup = cleanTreeView.Items.Count == 0 ? 0 : GetCheckedEntries().Count;
            _dashboardHealthLabel.ForeColor = UiTheme.TextSecondary;
            _dashboardHealthLabel.Text =
                $"健康状态：监控{(_monitorService.IsRunning ? "运行中" : "已暂停")} · 历史清理 {snapshot.RecentCleanupCount:N0} 条 · 待清理 {pendingCleanup:N0} 项 · 数据库正常";
        }
    }

    private void ApplyDashboardSnapshotError(int version)
    {
        if (IsDisposed || version != _dashboardSnapshotVersion) return;
        if (!IsHandleCreated)
        {
            Load += (_, _) => ApplyDashboardSnapshotError(version);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => ApplyDashboardSnapshotError(version));
            return;
        }

        _dashboardNotificationMetric!.Text = "数据读取异常";
        _dashboardNotificationMetric.ForeColor = UiTheme.Danger;
        if (_dashboardHealthLabel != null)
        {
            _dashboardHealthLabel.Text = "健康状态：数据库摘要读取失败，可前往记录中心重试";
            _dashboardHealthLabel.ForeColor = UiTheme.Danger;
        }
    }

    private void dashboardNotification_Click(object? sender, EventArgs e)
    {
        ShowWorkspacePage(RecordsPageId);
        ShowRecordView("notifications");
    }

    private async void dashboardQuickCleanupButton_Click(object? sender, EventArgs e)
    {
        var path = _dashboardQuickCleanupPath;
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            MessageBox.Show("快捷路径已不存在，请刷新监控目录或先完成一次空间分析。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshDashboardInsightsAsync();
            return;
        }

        _dashboardQuickCleanupCts?.Cancel();
        _dashboardQuickCleanupCts?.Dispose();
        _dashboardQuickCleanupCts = new CancellationTokenSource();
        var cts = _dashboardQuickCleanupCts;
        var originalText = _dashboardQuickCleanupButton!.Text;
        _dashboardQuickCleanupButton.Enabled = false;
        _dashboardQuickCleanupButton.Text = "正在估算...";
        try
        {
            var analysis = await _folderAnalyzer.ScanFolderAsync(path, cts.Token);
            if (cts.IsCancellationRequested) return;
            if (analysis.AccessStatus is FolderAccessStatus.Denied or FolderAccessStatus.Missing)
            {
                MessageBox.Show($"无法读取快捷路径：{analysis.ErrorMessage ?? "目录不可访问"}", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var inaccessible = analysis.InaccessibleCount > 0
                ? $"\n不可访问项：{analysis.InaccessibleCount:N0}"
                : string.Empty;
            var confirm = MessageBox.Show(
                $"快捷清理路径：\n{path}\n\n预计大小：{FormatHelper.FormatBytes(analysis.SizeBytes)}\n文件：{analysis.FileCount:N0}{inaccessible}\n\n进入清理页后仍需选择项目并再次确认，是否继续？",
                "快捷清理",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            if (confirm != DialogResult.Yes) return;

            cleanPathTextBox.Text = path;
            ShowWorkspacePage(CleanupPageId);
            await TryScanCurrentPathAsync();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            MessageBox.Show($"快捷清理估算失败：{ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (ReferenceEquals(_dashboardQuickCleanupCts, cts))
            {
                _dashboardQuickCleanupCts = null;
                cts.Dispose();
            }
            if (!IsDisposed)
            {
                _dashboardQuickCleanupButton.Enabled = true;
                _dashboardQuickCleanupButton.Text = originalText;
            }
        }
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
