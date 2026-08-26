using AntdUI;
using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>
/// 工作区壳：侧边导航 + 页面承载 + 状态栏。控件树已在 Form1.Designer.cs 中由设计器维护，
/// 本文件只保留页面切换、折叠、状态刷新等运行时逻辑。
/// </summary>
public partial class Form1
{
    private const string DashboardPageId = "dashboard";
    private const string ActivityPageId = "activity";
    private const string RulesPageId = "rules";
    private const string AnalyzerPageId = "analyzer";
    private const string CleanupPageId = "cleanup";
    private const string RecordsPageId = "records";

    private bool _workspaceMenuCollapsed;
    private bool _isExiting;

    #region 页面切换 

    internal void ShowWorkspacePage(int legacyIndex)
    {
        ShowWorkspacePage(legacyIndex switch
        {
            1 => ActivityPageId,
            2 => AnalyzerPageId,
            3 => CleanupPageId,
            4 => RulesPageId,
            5 => RecordsPageId,
            _ => DashboardPageId
        });
    }

    private void ShowWorkspacePage(string id)
    {
        // 隐藏所有页面面板
        dashboardPanel.Visible = false;
        activityPanel.Visible = false;
        rulesPanel.Visible = false;
        analyzerPanel.Visible = false;
        cleanupPanel.Visible = false;
        recordsPanel.Visible = false;

        // 显示目标页面面板
        var targetPanel = id switch
        {
            DashboardPageId => dashboardPanel,
            ActivityPageId => activityPanel,
            RulesPageId => rulesPanel,
            AnalyzerPageId => analyzerPanel,
            CleanupPageId => cleanupPanel,
            RecordsPageId => recordsPanel,
            _ => dashboardPanel
        };
        targetPanel.Visible = true;

        SetNavigationSelection(id);

        var (title, subtitle) = id switch
        {
            DashboardPageId => ("工作台", "查看磁盘空间、监控健康状态与最近文件活动"),
            ActivityPageId => ("实时活动", "观察并筛选监控目录中的文件变化"),
            RulesPageId => ("监控规则", "集中管理监控目录与忽略进程"),
            AnalyzerPageId => ("空间分析", "扫描目录占用并将结果直接带入清理流程"),
            CleanupPageId => ("清理中心", "选择文件、确认清理方式并执行安全清理"),
            RecordsPageId => ("记录中心", "统一查看提醒、进程统计、变更明细和清理历史"),
            _ => ("CdiskClean", string.Empty)
        };
        workspacePageTitle.Text = title;
        workspacePageSubtitle.Text = subtitle;

        if (id == DashboardPageId)
            RefreshDashboardMetrics();
        else if (id == RecordsPageId)
            RefreshRecordsCenter();
    }

    /// <summary>侧栏导航选中态：由 AntdUI.Menu 自身维护高亮</summary>
    private void SetNavigationSelection(string id)
    {
        if (workspaceMenu.FindID(id) is AntdUI.MenuItem menuItem && !menuItem.Select)
            workspaceMenu.Select(menuItem, true);
    }

    private void ToggleWorkspaceMenu()
    {
        _workspaceMenuCollapsed = !_workspaceMenuCollapsed;
        workspaceMenu.Collapsed = _workspaceMenuCollapsed;
        workspaceBodyLayout.ColumnStyles[0].Width = _workspaceMenuCollapsed ? 64F : 208F;
        //brandLabel.Visible = !_workspaceMenuCollapsed;
        workspaceCollapseButton.Text = _workspaceMenuCollapsed ? string.Empty : "折叠菜单";
        //brandLabel.Text = _workspaceMenuCollapsed ? "C" : "CdiskClean";
    }

    private void UpdateCleanupSelectionSummary()
    {
        var selected = GetCheckedEntries();
        cleanupSelectionLabel.Text = $"已选择 {selected.Count:N0} 项 / {FormatHelper.FormatBytes(selected.Sum(e => e.SizeBytes))}";
    }

    private void WorkspaceFormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isExiting || e.CloseReason != CloseReason.UserClosing) return;
        e.Cancel = true;
        Hide();
    }

    #endregion


    # region 事件包装方法（设计器绑定）


    private void workspaceMenu_ItemClick(object? sender, AntdUI.MenuItemEventArgs e)
    {
        if (e.Item != null && !string.IsNullOrWhiteSpace(e.Item.ID))
            ShowWorkspacePage(e.Item.ID);
    }

    private void workspaceCollapseButton_Click(object? sender, EventArgs e) => ToggleWorkspaceMenu();
    private void activityRecordCenterButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(RecordsPageId);
    private void recordSearchBox_TextChanged(object? sender, EventArgs e) => ApplyFilter();
    #endregion


    #region AntdUI Table 

    /// <summary>
    /// 统一表格列创建入口：项目中所有记录表格（工作台/记录中心共 5 张表）的列
    /// 都经由此方法创建。如需统一调整列样式（对齐、宽度、省略显示、表头样式等），
    /// 只需修改此方法即可全局生效。
    /// </summary>
    private static AntdUI.Column MakeColumn(string key, string title,
        string width = "auto", AntdUI.ColumnAlign align = AntdUI.ColumnAlign.Center)
    {
        return new AntdUI.Column(key, title, align)
            .SetWidth(width);
    }

    /// <summary>配置工作台与记录中心的 5 个 AntdUI.Table 列（构造期执行一次）</summary>
    private void ConfigureTableColumns()
    {
        // 工作台-最近活动
        dashboardRecentTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("Timestamp", "时间", "16%"),
                MakeColumn("TypeText", "类型", "10%"),
                MakeColumn("FileName", "文件名", "28%"),
                MakeColumn("SourceProcess", "来源进程", "16%"),
                MakeColumn("Directory", "目录", "30%", AntdUI.ColumnAlign.Left)
            };
        dashboardRecentTable.Columns["Timestamp"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");
        dashboardRecentTable.Columns["TypeText"]?.SetRender(
            (value, _, _) => value is ChangeType type ? EnumHelper.FormatChangeType(type) : value);

        // 记录中心-提醒记录
        notificationRecordsTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("ProcessName", "进程", "24%"),
                MakeColumn("OperationCount", "操作次数", "16%"),
                MakeColumn("DurationSeconds", "持续时长(秒)", "18%"),
                MakeColumn("TriggerTime", "触发时间", "30%")
            };
        notificationRecordsTable.Columns["TriggerTime"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");

        // 记录中心-进程统计
        processStatsTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("AppName", "应用", "26%"),
                MakeColumn("ChangeCount", "变更次数", "14%"),
                MakeColumn("FirstChangeTime", "首次变更", "24%"),
                MakeColumn("LastChangeTime", "最近变更", "24%")
            };
        processStatsTable.Columns["FirstChangeTime"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");
        processStatsTable.Columns["LastChangeTime"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");

        // 记录中心-变更明细（ChangeType 枚举经 Render 转中文显示）
        detailRecordsTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("Timestamp", "时间", "14%"),
                MakeColumn("ChangeType", "类型", "8%"),
                MakeColumn("FileName", "文件名", "14%"),
                MakeColumn("FullPath", "完整路径", "22%", AntdUI.ColumnAlign.Left),
                MakeColumn("Directory", "目录", "20%", AntdUI.ColumnAlign.Left),
                MakeColumn("SizeBytes", "大小", "10%"),
                MakeColumn("SourceProcess", "来源进程", "12%")
            };
        detailRecordsTable.Columns["Timestamp"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");
        detailRecordsTable.Columns["ChangeType"]?.SetRender(
            (value, _, _) => value is ChangeType type ? EnumHelper.FormatChangeType(type) : value);

        // 记录中心-清理历史
        cleanHistoryTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("CleanupTime", "清理时间", "16%"),
                MakeColumn("FullPath", "原始路径", "28%", AntdUI.ColumnAlign.Left),
                MakeColumn("FileName", "文件名", "16%"),
                MakeColumn("Method", "清理方式", "10%"),
                MakeColumn("Message", "处理结果", "14%", AntdUI.ColumnAlign.Left),
                MakeColumn("SizeText", "文件大小", "10%"),
                MakeColumn("ResultText", "状态", "6%")
            };
        cleanHistoryTable.Columns["CleanupTime"]?.SetDisplayFormat("yyyy-MM-dd HH:mm:ss");
    }
    #endregion

}
