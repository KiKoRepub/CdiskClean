using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>工作区「记录中心」页逻辑：四视图切换与数据刷新</summary>
public partial class Form1
{
    private void ShowRecordView(string id)
    {
        notificationRecordsGrid.Visible = id == "notifications";
        processStatsGrid.Visible = id == "stats";
        detailRecordsGrid.Visible = id == "details";
        cleanupRecordView.Visible = id == "cleanup";
        if (id == "notifications") notificationRecordsGrid.BringToFront();
        else if (id == "stats") processStatsGrid.BringToFront();
        else if (id == "details") detailRecordsGrid.BringToFront();
        else cleanupRecordView.BringToFront();

        SetTabActive(recordsNotificationTab, id == "notifications");
        SetTabActive(recordsStatsTab, id == "stats");
        SetTabActive(recordsDetailsTab, id == "details");
        SetTabActive(recordsCleanupTab, id == "cleanup");
    }

    /// <summary>刷新记录中心四个视图；数据库读取在后台执行，避免卡 UI</summary>
    private async void RefreshRecordsCenter()
    {
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
        if (IsDisposed) return;
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

        notificationRecordsGrid.DataSource = new BindingList<ProcessNotificationRecord>(notifications);
        processStatsGrid.DataSource = new BindingList<AppChangeStats>(stats);
        detailRecordsGrid.DataSource = new BindingList<FileChangeRecord>(
            records.OrderByDescending(r => r.Timestamp).ToList());
        RefreshCleanHistory();
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void recordsNotificationTab_Click(object? sender, EventArgs e) => ShowRecordView("notifications");
    private void recordsStatsTab_Click(object? sender, EventArgs e) => ShowRecordView("stats");
    private void recordsDetailsTab_Click(object? sender, EventArgs e) => ShowRecordView("details");
    private void recordsCleanupTab_Click(object? sender, EventArgs e) => ShowRecordView("cleanup");
    private void recordsRefreshButton_Click(object? sender, EventArgs e) => RefreshRecordsCenter();

    /// <summary>变更明细网格：ChangeType 枚举转中文显示</summary>
    private void detailRecordsGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex >= 0 &&
            detailRecordsGrid.Columns[e.ColumnIndex].DataPropertyName == "ChangeType" &&
            e.Value is ChangeType type)
        {
            e.Value = EnumHelper.FormatChangeType(type);
            e.FormattingApplied = true;
        }
    }
}
