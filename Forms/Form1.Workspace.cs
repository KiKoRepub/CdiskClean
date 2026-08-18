using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>
/// 工作区壳：侧边导航 + 页面承载 + 状态栏，页面构建按功能拆分在
/// Form1.Workspace.{Dashboard,Activity,Rules,Analyzer,Cleanup,Records}.cs 各分页文件中。
/// </summary>
public partial class Form1
{
    private const string DashboardPageId = "dashboard";
    private const string ActivityPageId = "activity";
    private const string RulesPageId = "rules";
    private const string AnalyzerPageId = "analyzer";
    private const string CleanupPageId = "cleanup";
    private const string RecordsPageId = "records";

    private readonly Dictionary<string, Control> _workspacePages = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Control> _recordViews = new(StringComparer.Ordinal);
    private TableLayoutPanel? _workspaceBodyLayout;
    private AntdUI.Menu? _workspaceMenu;
    private Panel? _workspacePageHost;
    private Label? _workspacePageTitle;
    private Label? _workspacePageSubtitle;
    private Label? _workspaceDiskStatus;
    private Label? _workspaceMonitorStatus;
    private Label? _workspaceRecordStatus;
    private Label? _workspaceClockStatus;
    private bool _workspaceMenuCollapsed;
    private bool _isExiting;

    // 工作台页
    private AntdUI.Progress? _dashboardDiskProgress;
    private Label? _dashboardUsageLabel;
    private Label? _dashboardCapacityLabel;
    private Label? _dashboardMonitorMetric;
    private Label? _dashboardRecordMetric;
    private Label? _dashboardRuleMetric;
    private DataGridView? _dashboardRecentGrid;

    // 实时活动页
    private AntdUI.Button? _workspaceMonitorToggleButton;
    private AntdUI.Input? _recordSearchInput;

    // 监控规则页
    private Panel? _rulesDirectoryView;
    private Panel? _rulesProcessView;
    private AntdUI.Button? _rulesDirectoryTab;
    private AntdUI.Button? _rulesProcessTab;
    private AntdUI.Input? _manualProcessInput;

    // 空间分析页
    private Label? _analyzerPathValue;
    private Label? _analyzerSizeValue;
    private Label? _analyzerFilesValue;
    private Label? _analyzerFoldersValue;

    // 清理中心页
    private Label? _cleanupSelectionLabel;

    // 记录中心页
    private Panel? _recordViewHost;
    private DataGridView? _notificationRecordsGrid;
    private DataGridView? _processStatsGrid;
    private DataGridView? _detailRecordsGrid;
    private AntdUI.Button? _notificationTabButton;
    private AntdUI.Button? _processStatsTabButton;
    private AntdUI.Button? _detailRecordsTabButton;
    private AntdUI.Button? _cleanupHistoryTabButton;

    private void BuildWorkspaceShell()
    {
        SuspendLayout();

        // 旧设计器界面整体让位于工作区壳（被复用的控件在下方页面构建中重新挂载）
        Controls.Clear();

        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(1180, 720);
        ClientSize = new Size(1440, 860);
        BackColor = UiTheme.Canvas;

        var root = new TableLayoutPanel
        {
            Name = "workspaceRoot",
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 1,
            RowCount = 3,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        var header = new AntdUI.PageHeader
        {
            Name = "workspaceHeader",
            Dock = DockStyle.Fill,
            Text = "CdiskClean  C盘监测与清理",
            ShowButton = true,
            ShowIcon = false,
            BackColor = Color.White,
            ForeColor = UiTheme.TextPrimary,
            Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold)
        };
        root.Controls.Add(header, 0, 0);

        _workspaceBodyLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        _workspaceBodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 208F));
        _workspaceBodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _workspaceBodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var navigation = BuildWorkspaceNavigation();
        var mainArea = BuildWorkspaceMainArea();
        _workspaceBodyLayout.Controls.Add(navigation, 0, 0);
        _workspaceBodyLayout.Controls.Add(mainArea, 1, 0);
        root.Controls.Add(_workspaceBodyLayout, 0, 1);
        root.Controls.Add(BuildWorkspaceStatusBar(), 0, 2);

        Controls.Add(root);
        FormClosing += WorkspaceFormClosing;

        BuildWorkspacePages();
        ShowWorkspacePage(DashboardPageId);
        RefreshWorkspaceStatus();
        ResumeLayout(true);
    }

    private Control BuildWorkspaceNavigation()
    {
        var navigation = new AntdUI.Panel
        {
            Dock = DockStyle.Fill,
            Radius = 0,
            BackColor = Color.White,
            Padding = new Padding(8, 12, 8, 8)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 3,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

        var brand = new Label
        {
            Dock = DockStyle.Fill,
            Text = "  CDISK CLEAN",
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.White
        };
        layout.Controls.Add(brand, 0, 0);

        _workspaceMenu = new AntdUI.Menu
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Radius = 5,
            AutoCollapse = false
        };
        _workspaceMenu.Items.Add(CreateMenuItem(DashboardPageId, "工作台", "DashboardOutlined", true));
        _workspaceMenu.Items.Add(CreateMenuItem(ActivityPageId, "实时活动", "MonitorOutlined"));
        _workspaceMenu.Items.Add(CreateMenuItem(RulesPageId, "监控规则", "ControlOutlined"));
        _workspaceMenu.Items.Add(CreateMenuItem(AnalyzerPageId, "空间分析", "PieChartOutlined"));
        _workspaceMenu.Items.Add(CreateMenuItem(CleanupPageId, "清理中心", "DeleteOutlined"));
        _workspaceMenu.Items.Add(CreateMenuItem(RecordsPageId, "记录中心", "HistoryOutlined"));
        _workspaceMenu.ItemClick += (_, item) =>
        {
            if (item.Item != null && !string.IsNullOrWhiteSpace(item.Item.ID))
                ShowWorkspacePage(item.Item.ID);
        };
        layout.Controls.Add(_workspaceMenu, 0, 1);

        var collapse = CreateAntButton("折叠菜单", "ControlOutlined", (_, _) => ToggleWorkspaceMenu());
        collapse.Name = "workspaceCollapseButton";
        collapse.Dock = DockStyle.Fill;
        layout.Controls.Add(collapse, 0, 2);

        navigation.Controls.Add(layout);
        return navigation;
    }

    private static AntdUI.MenuItem CreateMenuItem(string id, string text, string icon, bool selected = false)
    {
        return new AntdUI.MenuItem
        {
            ID = id,
            Name = id,
            Text = text,
            IconSvg = icon,
            Select = selected
        };
    }

    private Control BuildWorkspaceMainArea()
    {
        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 1,
            RowCount = 2,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var pageHeader = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(20, 8, 20, 7)
        };
        _workspacePageTitle = new Label
        {
            AutoSize = false,
            Location = new Point(20, 8),
            Size = new Size(500, 27),
            Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent
        };
        _workspacePageSubtitle = new Label
        {
            AutoSize = false,
            Location = new Point(20, 37),
            Size = new Size(820, 22),
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = UiTheme.TextSecondary,
            BackColor = Color.Transparent
        };
        pageHeader.Controls.Add(_workspacePageTitle);
        pageHeader.Controls.Add(_workspacePageSubtitle);
        pageHeader.Resize += (_, _) =>
        {
            if (_workspacePageTitle != null)
                _workspacePageTitle.Width = Math.Max(200, pageHeader.ClientSize.Width - 40);
            if (_workspacePageSubtitle != null)
                _workspacePageSubtitle.Width = Math.Max(200, pageHeader.ClientSize.Width - 40);
        };
        main.Controls.Add(pageHeader, 0, 0);

        _workspacePageHost = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            Padding = new Padding(18)
        };
        main.Controls.Add(_workspacePageHost, 0, 1);
        return main;
    }

    private Control BuildWorkspaceStatusBar()
    {
        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 4,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = new Padding(10, 0, 10, 0)
        };
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        status.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _workspaceDiskStatus = CreateStatusLabel(ContentAlignment.MiddleLeft);
        _workspaceMonitorStatus = CreateStatusLabel(ContentAlignment.MiddleLeft);
        _workspaceRecordStatus = CreateStatusLabel(ContentAlignment.MiddleLeft);
        _workspaceClockStatus = CreateStatusLabel(ContentAlignment.MiddleRight);
        status.Controls.Add(_workspaceDiskStatus, 0, 0);
        status.Controls.Add(_workspaceMonitorStatus, 1, 0);
        status.Controls.Add(_workspaceRecordStatus, 2, 0);
        status.Controls.Add(_workspaceClockStatus, 3, 0);
        return status;
    }

    private static Label CreateStatusLabel(ContentAlignment alignment)
    {
        return new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = alignment,
            Font = new Font("Microsoft YaHei UI", 8.5F),
            ForeColor = UiTheme.TextSecondary,
            BackColor = Color.White,
            AutoEllipsis = true
        };
    }

    private void BuildWorkspacePages()
    {
        if (_workspacePageHost == null) return;

        AddWorkspacePage(DashboardPageId, BuildDashboardPage());
        AddWorkspacePage(ActivityPageId, BuildActivityPage());
        AddWorkspacePage(RulesPageId, BuildRulesPage());
        AddWorkspacePage(AnalyzerPageId, BuildAnalyzerPage());
        AddWorkspacePage(CleanupPageId, BuildCleanupPage());
        AddWorkspacePage(RecordsPageId, BuildRecordsPage());
    }

    private void AddWorkspacePage(string id, Control page)
    {
        if (_workspacePageHost == null) return;
        page.Name = $"workspacePage_{id}";
        page.Dock = DockStyle.Fill;
        page.Visible = false;
        _workspacePages[id] = page;
        _workspacePageHost.Controls.Add(page);
    }

    private void ToggleWorkspaceMenu()
    {
        if (_workspaceBodyLayout == null || _workspaceMenu == null) return;
        _workspaceMenuCollapsed = !_workspaceMenuCollapsed;
        _workspaceMenu.Collapsed = _workspaceMenuCollapsed;
        _workspaceBodyLayout.ColumnStyles[0].Width = _workspaceMenuCollapsed ? 64F : 208F;

        if (_workspaceBodyLayout.Controls[0] is Control navigation &&
            navigation.Controls.Find("workspaceCollapseButton", true).FirstOrDefault() is AntdUI.Button collapse)
        {
            collapse.Text = _workspaceMenuCollapsed ? string.Empty : "折叠菜单";
        }
    }

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
        if (!_workspacePages.TryGetValue(id, out var selected)) return;

        if (_workspaceMenu?.FindID(id) is AntdUI.MenuItem menuItem && !menuItem.Select)
            _workspaceMenu.Select(menuItem, true);

        foreach (var pair in _workspacePages)
            pair.Value.Visible = ReferenceEquals(pair.Value, selected);
        selected.BringToFront();

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
        if (_workspacePageTitle != null) _workspacePageTitle.Text = title;
        if (_workspacePageSubtitle != null) _workspacePageSubtitle.Text = subtitle;

        if (id == DashboardPageId)
            RefreshDashboardMetrics();
        else if (id == RecordsPageId)
            RefreshRecordsCenter();
    }

    private void UpdateCleanupSelectionSummary()
    {
        if (_cleanupSelectionLabel == null) return;
        var selected = GetCheckedEntries();
        _cleanupSelectionLabel.Text = $"已选择 {selected.Count:N0} 项 / {FormatHelper.FormatBytes(selected.Sum(e => e.SizeBytes))}";
    }

    private void WorkspaceFormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isExiting || e.CloseReason != CloseReason.UserClosing) return;
        e.Cancel = true;
        Hide();
    }

    // ==================== 共享构建工具 ====================

    private static TableLayoutPanel CreatePageLayout(int rowCount)
    {
        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 1,
            RowCount = rowCount,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        return page;
    }

    private static AntdUI.Panel CreateSurface(Padding padding)
    {
        return new AntdUI.Panel
        {
            Dock = DockStyle.Fill,
            Radius = 6,
            BackColor = Color.White,
            Padding = padding,
            Margin = Padding.Empty
        };
    }

    private static FlowLayoutPanel CreateCommandBar()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoScroll = true,
            BackColor = UiTheme.Canvas,
            Margin = Padding.Empty,
            Padding = new Padding(0, 4, 0, 8)
        };
    }

    private static AntdUI.Button CreateAntButton(
        string text,
        string icon,
        EventHandler click,
        AntdUI.TTypeMini? type = null)
    {
        var button = new AntdUI.Button
        {
            Text = text,
            IconSvg = icon,
            Radius = 5,
            Height = 36,
            Width = string.IsNullOrEmpty(text) ? 36 : Math.Max(84, text.Length * 18 + 42),
            Padding = string.IsNullOrEmpty(text) ? new Padding(8) : new Padding(12, 0, 12, 0),
            Margin = new Padding(0, 4, 8, 0),
            Font = new Font("Microsoft YaHei UI", 9.5F)
        };
        if (type.HasValue)
            button.Type = type.Value;
        button.Click += click;
        return button;
    }

    /// <summary>把设计器遗留的标准按钮纳入工作区工具栏布局（保留原有视觉样式）</summary>
    private static void PrepareLegacyToolbarButton(Button button, string text)
    {
        button.Text = text;
        button.Dock = DockStyle.Fill;
        button.Margin = new Padding(4, 6, 4, 6);
        button.MinimumSize = new Size(64, 34);
    }

    private static DataGridViewTextBoxColumn CreateGridColumn(string property, string header, float weight)
    {
        return new DataGridViewTextBoxColumn
        {
            DataPropertyName = property,
            Name = $"Workspace{property}Column",
            HeaderText = header,
            FillWeight = weight,
            ReadOnly = true
        };
    }

    private static DataGridView CreateReadOnlyGrid()
    {
        return new DataGridView
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackgroundColor = Color.White,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false
        };
    }

    private sealed class DashboardActivityRow
    {
        public DateTime Timestamp { get; init; }
        public string TypeText { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string SourceProcess { get; init; } = string.Empty;
        public string Directory { get; init; } = string.Empty;
    }
}
