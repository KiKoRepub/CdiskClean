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
        var tab = id switch
        {
            DashboardPageId => dashboardPage,
            ActivityPageId => activityPage,
            RulesPageId => rulesPage,
            AnalyzerPageId => analyzerPage,
            CleanupPageId => cleanupPage,
            RecordsPageId => recordsPage,
            _ => dashboardPage
        };
        workspaceTabControl.SelectedTab = tab;
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

    /// <summary>侧栏导航选中态：当前页高亮为浅蓝底蓝字</summary>
    private void SetNavigationSelection(string id)
    {
        foreach (var (button, pageId) in new (Button, string)[]
        {
            (navDashboardButton, DashboardPageId),
            (navActivityButton, ActivityPageId),
            (navRulesButton, RulesPageId),
            (navAnalyzerButton, AnalyzerPageId),
            (navCleanupButton, CleanupPageId),
            (navRecordsButton, RecordsPageId)
        })
        {
            bool active = pageId == id;
            button.BackColor = active ? UiTheme.PrimarySoft : Color.White;
            button.ForeColor = active ? UiTheme.Primary : UiTheme.TextSecondary;
        }
    }

    private void ToggleWorkspaceMenu()
    {
        _workspaceMenuCollapsed = !_workspaceMenuCollapsed;
        workspaceBodyLayout.ColumnStyles[0].Width = _workspaceMenuCollapsed ? 64F : 208F;
        brandLabel.Visible = !_workspaceMenuCollapsed;
        workspaceCollapseButton.Text = _workspaceMenuCollapsed ? "☰" : "折叠菜单";

        foreach (var (button, text) in new (Button, string)[]
        {
            (navDashboardButton, "工作台"),
            (navActivityButton, "实时活动"),
            (navRulesButton, "监控规则"),
            (navAnalyzerButton, "空间分析"),
            (navCleanupButton, "清理中心"),
            (navRecordsButton, "记录中心")
        })
        {
            button.Text = _workspaceMenuCollapsed ? string.Empty : text;
        }
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

    private void navDashboardButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(DashboardPageId);
    private void navActivityButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(ActivityPageId);
    private void navRulesButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(RulesPageId);
    private void navAnalyzerButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(AnalyzerPageId);
    private void navCleanupButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(CleanupPageId);
    private void navRecordsButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(RecordsPageId);
    private void workspaceCollapseButton_Click(object? sender, EventArgs e) => ToggleWorkspaceMenu();
    private void activityRecordCenterButton_Click(object? sender, EventArgs e) => ShowWorkspacePage(RecordsPageId);
    private void recordSearchBox_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void minimizeButton_Click(object? sender, EventArgs e) => WindowState = FormWindowState.Minimized;

    private void maximizeButton_Click(object? sender, EventArgs e) =>
        WindowState = WindowState == FormWindowState.Maximized
            ? FormWindowState.Normal
            : FormWindowState.Maximized;

    private void closeButton_Click(object? sender, EventArgs e) => closeApplication();

    private sealed class DashboardActivityRow
    {
        public DateTime Timestamp { get; init; }
        public string TypeText { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string SourceProcess { get; init; } = string.Empty;
        public string Directory { get; init; } = string.Empty;
    }
}
