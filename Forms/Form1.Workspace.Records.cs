using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>工作区「记录中心」页：提醒记录 / 进程统计 / 变更明细 / 清理历史四个子视图</summary>
public partial class Form1
{
    private Control BuildRecordsPage()
    {
        var page = CreatePageLayout(2);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var toolbar = CreateCommandBar();
        _notificationTabButton = CreateAntButton("提醒记录", "MonitorOutlined",
            (_, _) => ShowRecordView("notifications"), AntdUI.TTypeMini.Primary);
        _processStatsTabButton = CreateAntButton("进程统计", "LineChartOutlined",
            (_, _) => ShowRecordView("stats"));
        _detailRecordsTabButton = CreateAntButton("变更明细", "DatabaseOutlined",
            (_, _) => ShowRecordView("details"));
        _cleanupHistoryTabButton = CreateAntButton("清理历史", "HistoryOutlined",
            (_, _) => ShowRecordView("cleanup"));
        toolbar.Controls.Add(_notificationTabButton);
        toolbar.Controls.Add(_processStatsTabButton);
        toolbar.Controls.Add(_detailRecordsTabButton);
        toolbar.Controls.Add(_cleanupHistoryTabButton);
        toolbar.Controls.Add(CreateAntButton("刷新", "ReloadOutlined", (_, _) => RefreshRecordsCenter()));
        page.Controls.Add(toolbar, 0, 0);

        var surface = CreateSurface(new Padding(12));
        _recordViewHost = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        surface.Controls.Add(_recordViewHost);
        page.Controls.Add(surface, 0, 1);

        _notificationRecordsGrid = CreateNotificationGrid();
        _processStatsGrid = CreateProcessStatsGrid();
        _detailRecordsGrid = CreateDetailRecordsGrid();
        RegisterRecordView("notifications", _notificationRecordsGrid);
        RegisterRecordView("stats", _processStatsGrid);
        RegisterRecordView("details", _detailRecordsGrid);

        var cleanupView = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        cleanHistoryGrid.Dock = DockStyle.Fill;
        cleanupView.Controls.Add(cleanHistoryGrid);
        if (_cleanHistoryEmptyLabel != null)
        {
            _cleanHistoryEmptyLabel.Dock = DockStyle.Fill;
            cleanupView.Controls.Add(_cleanHistoryEmptyLabel);
            _cleanHistoryEmptyLabel.BringToFront();
        }
        RegisterRecordView("cleanup", cleanupView);
        ShowRecordView("notifications");
        return page;
    }

    private void RegisterRecordView(string id, Control view)
    {
        if (_recordViewHost == null) return;
        view.Dock = DockStyle.Fill;
        view.Visible = false;
        _recordViews[id] = view;
        _recordViewHost.Controls.Add(view);
    }

    private DataGridView CreateNotificationGrid()
    {
        var grid = CreateReadOnlyGrid();
        grid.AutoGenerateColumns = false;
        grid.Columns.Add(CreateGridColumn("ProcessName", "进程名", 30));
        grid.Columns.Add(CreateGridColumn("OperationCount", "操作次数", 16));
        grid.Columns.Add(CreateGridColumn("DurationSeconds", "持续时间（秒）", 18));
        grid.Columns.Add(CreateGridColumn("TriggerTime", "提醒时间", 28));
        return grid;
    }

    private DataGridView CreateProcessStatsGrid()
    {
        var grid = CreateReadOnlyGrid();
        grid.AutoGenerateColumns = false;
        grid.Columns.Add(CreateGridColumn("AppName", "进程名", 28));
        grid.Columns.Add(CreateGridColumn("ChangeCount", "操作次数", 16));
        grid.Columns.Add(CreateGridColumn("FirstChangeTime", "首次时间", 28));
        grid.Columns.Add(CreateGridColumn("LastChangeTime", "最后时间", 28));
        return grid;
    }

    private DataGridView CreateDetailRecordsGrid()
    {
        var grid = CreateReadOnlyGrid();
        grid.AutoGenerateColumns = false;
        grid.Columns.Add(CreateGridColumn("Timestamp", "时间", 20));
        grid.Columns.Add(CreateGridColumn("SourceProcess", "来源进程", 18));
        grid.Columns.Add(CreateGridColumn("ChangeType", "类型", 12));
        grid.Columns.Add(CreateGridColumn("Directory", "目录", 28));
        grid.Columns.Add(CreateGridColumn("FileName", "文件名", 24));
        grid.CellFormatting += (_, e) =>
        {
            if (e.ColumnIndex >= 0 && grid.Columns[e.ColumnIndex].DataPropertyName == "ChangeType" &&
                e.Value is ChangeType type)
            {
                e.Value = EnumHelper.FormatChangeType(type);
                e.FormattingApplied = true;
            }
        };
        return grid;
    }

    private void ShowRecordView(string id)
    {
        if (!_recordViews.TryGetValue(id, out var selected)) return;
        foreach (var pair in _recordViews)
            pair.Value.Visible = ReferenceEquals(pair.Value, selected);
        selected.BringToFront();

        SetRecordTabState(_notificationTabButton, id == "notifications");
        SetRecordTabState(_processStatsTabButton, id == "stats");
        SetRecordTabState(_detailRecordsTabButton, id == "details");
        SetRecordTabState(_cleanupHistoryTabButton, id == "cleanup");
    }

    private static void SetRecordTabState(AntdUI.Button? button, bool active)
    {
        if (button != null)
            button.Type = active ? AntdUI.TTypeMini.Primary : AntdUI.TTypeMini.Default;
    }

    /// <summary>刷新记录中心四个视图；数据库读取在后台执行，避免卡 UI</summary>
    private async void RefreshRecordsCenter()
    {
        if (_notificationRecordsGrid == null || _processStatsGrid == null || _detailRecordsGrid == null)
            return;

        List<FileChangeRecord> records;
        lock (_recordsLock)
            records = _records.ToList();

        List<FileChangeRecord> dbRecords;
        List<ProcessNotificationRecord> notifications;
        try
        {
            var dbTask = Task.Run(() => _databaseService.GetChangeRecords(1000));
            var notifTask = Task.Run(() => _databaseService.GetProcessNotifications(500));
            await Task.WhenAll(dbTask, notifTask);
            dbRecords = dbTask.Result;
            notifications = notifTask.Result;
        }
        catch
        {
            dbRecords = new List<FileChangeRecord>();
            notifications = new List<ProcessNotificationRecord>();
        }

        if (records.Count == 0)
            records = dbRecords;

        var stats = AppChangeStats.BuildFrom(records);
        BindRecordsCenter(notifications, stats, records);
    }

    private void BindRecordsCenter(
        List<ProcessNotificationRecord> notifications,
        List<AppChangeStats> stats,
        List<FileChangeRecord> records)
    {
        if (IsDisposed || _notificationRecordsGrid == null ||
            _processStatsGrid == null || _detailRecordsGrid == null)
            return;
        // 句柄尚未创建（构造期异步完成时）挂到 Load 后执行；非 UI 线程则回传 UI 线程
        if (!IsHandleCreated)
        {
            Load += (_, _) => BindRecordsCenter(notifications, stats, records);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => BindRecordsCenter(notifications, stats, records));
            return;
        }

        _notificationRecordsGrid.DataSource = new BindingList<ProcessNotificationRecord>(notifications);
        _processStatsGrid.DataSource = new BindingList<AppChangeStats>(stats);
        _detailRecordsGrid.DataSource = new BindingList<FileChangeRecord>(
            records.OrderByDescending(r => r.Timestamp).ToList());
        RefreshCleanHistory();
    }
}
