using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>工作区「记录中心」页逻辑：四视图切换与数据刷新</summary>
public partial class Form1
{
    private void ShowRecordView(string id)
    {
        notificationRecordsTable.Visible = id == "notifications";
        processStatsTable.Visible = id == "stats";
        detailRecordsTable.Visible = id == "details";
        cleanupRecordView.Visible = id == "cleanup";
        if (id == "notifications") notificationRecordsTable.BringToFront();
        else if (id == "stats") processStatsTable.BringToFront();
        else if (id == "details") detailRecordsTable.BringToFront();
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
        BindRecordsCenter( new (notifications), new (stats), new (records));
    }

    private void BindRecordsCenter(
        BindingList<ProcessNotificationRecord> notifications,
        BindingList<AppChangeStats> stats,
        BindingList<FileChangeRecord> records)
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

        // 列配置见 ConfigureTableColumns()；时间格式化由列 DisplayFormat 完成
        notificationRecordsTable.DataSource = notifications;
        processStatsTable.DataSource = stats;
        detailRecordsTable.DataSource = new BindingList<FileChangeRecord>(records.OrderByDescending(r => r.Timestamp).ToList());
        RefreshCleanHistory();
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void recordsNotificationTab_Click(object? sender, EventArgs e) => ShowRecordView("notifications");
    private void recordsStatsTab_Click(object? sender, EventArgs e) => ShowRecordView("stats");
    private void recordsDetailsTab_Click(object? sender, EventArgs e) => ShowRecordView("details");
    private void recordsCleanupTab_Click(object? sender, EventArgs e) => ShowRecordView("cleanup");
    private void recordsRefreshButton_Click(object? sender, EventArgs e) => RefreshRecordsCenter();
}
