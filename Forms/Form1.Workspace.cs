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

    // ==================== 页面切换 ====================

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
        brandLabel.Visible = !_workspaceMenuCollapsed;
        workspaceCollapseButton.Text = _workspaceMenuCollapsed ? string.Empty : "折叠菜单";
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

    // ==================== 事件包装方法（设计器绑定） ====================

    private void workspaceMenu_ItemClick(object? sender, AntdUI.MenuItemEventArgs e)
    {
        if (e.Item != null && !string.IsNullOrWhiteSpace(e.Item.ID))
            ShowWorkspacePage(e.Item.ID);
    }

    private void workspaceCollapseButton_Click(object? sender, EventArgs e) => ToggleWorkspaceMenu();
    private void activityRecordCenterButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(RecordsPageId);
    private void recordSearchBox_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private sealed class DashboardActivityRow
    {
        public DateTime Timestamp { get; init; }
        public string TypeText { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string SourceProcess { get; init; } = string.Empty;
        public string Directory { get; init; } = string.Empty;
    }
}
