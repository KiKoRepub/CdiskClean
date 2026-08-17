using CdiskClean.Helpers;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

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

    private AntdUI.Progress? _dashboardDiskProgress;
    private Label? _dashboardUsageLabel;
    private Label? _dashboardCapacityLabel;
    private Label? _dashboardMonitorMetric;
    private Label? _dashboardRecordMetric;
    private Label? _dashboardRuleMetric;
    private DataGridView? _dashboardRecentGrid;

    private AntdUI.Button? _workspaceMonitorToggleButton;
    private AntdUI.Input? _recordSearchInput;

    private Panel? _rulesDirectoryView;
    private Panel? _rulesProcessView;
    private AntdUI.Button? _rulesDirectoryTab;
    private AntdUI.Button? _rulesProcessTab;
    private AntdUI.Input? _manualProcessInput;

    private Label? _analyzerPathValue;
    private Label? _analyzerSizeValue;
    private Label? _analyzerFilesValue;
    private Label? _analyzerFoldersValue;

    private Label? _cleanupSelectionLabel;

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

        panelTitle.Visible = false;
        statusStrip1.Visible = false;
        TabPageControl1.Visible = false;
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

    private Control BuildActivityPage()
    {
        var page = CreatePageLayout(2);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var toolbar = CreateCommandBar();
        _workspaceMonitorToggleButton = CreateAntButton("开始监测", "PlayCircleOutlined", pauseBtn_Click,
            AntdUI.TTypeMini.Primary);
        typeFilterCombo.Width = 126;
        typeFilterCombo.Height = 36;
        typeFilterCombo.Margin = new Padding(8, 6, 0, 0);
        _recordSearchInput = new AntdUI.Input
        {
            Width = 260,
            Height = 36,
            Margin = new Padding(10, 4, 0, 0),
            PlaceholderText = "搜索文件、路径或来源进程",
            PrefixSvg = "SearchOutlined",
            Radius = 5
        };
        _recordSearchInput.TextChanged += (_, _) => ApplyFilter();

        toolbar.Controls.Add(_workspaceMonitorToggleButton);
        toolbar.Controls.Add(typeFilterCombo);
        toolbar.Controls.Add(_recordSearchInput);
        toolbar.Controls.Add(CreateAntButton("导出", "ExportOutlined", exportBtn_Click));
        toolbar.Controls.Add(CreateAntButton("清空", "ClearOutlined", clearBtn_Click, AntdUI.TTypeMini.Error));
        toolbar.Controls.Add(CreateAntButton("记录中心", "HistoryOutlined", (_, _) => ShowWorkspacePage(RecordsPageId)));
        page.Controls.Add(toolbar, 0, 0);

        var surface = CreateSurface(new Padding(12));
        changesDataGrid.Dock = DockStyle.Fill;
        changesDataGrid.Margin = Padding.Empty;
        surface.Controls.Add(changesDataGrid);
        page.Controls.Add(surface, 0, 1);
        return page;
    }

    private Control BuildRulesPage()
    {
        var page = CreatePageLayout(2);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var switcher = CreateCommandBar();
        _rulesDirectoryTab = CreateAntButton("监控目录", "FolderOpenOutlined", (_, _) => ShowRulesView(true),
            AntdUI.TTypeMini.Primary);
        _rulesProcessTab = CreateAntButton("忽略进程", "ControlOutlined", (_, _) => ShowRulesView(false));
        switcher.Controls.Add(_rulesDirectoryTab);
        switcher.Controls.Add(_rulesProcessTab);
        page.Controls.Add(switcher, 0, 0);

        var host = CreateSurface(new Padding(14));
        _rulesDirectoryView = BuildDirectoryRulesView();
        _rulesProcessView = BuildProcessRulesView();
        host.Controls.Add(_rulesProcessView);
        host.Controls.Add(_rulesDirectoryView);
        page.Controls.Add(host, 0, 1);
        ShowRulesView(true);
        return page;
    }

    private Panel BuildDirectoryRulesView()
    {
        var view = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        var toolbar = CreateCommandBar();
        toolbar.Dock = DockStyle.Top;
        toolbar.Height = 48;
        toolbar.Padding = new Padding(0, 2, 0, 7);
        toolbar.Controls.Add(CreateAntButton("添加目录", "PlusOutlined", dirAddButton_Click,
            AntdUI.TTypeMini.Primary));
        toolbar.Controls.Add(CreateAntButton("批量选择", "FolderOpenOutlined", betterDirAddButton_Click));

        watcherDirListView.Dock = DockStyle.Fill;
        watcherDirListView.Margin = Padding.Empty;
        view.Controls.Add(watcherDirListView);
        view.Controls.Add(toolbar);
        return view;
    }

    private Panel BuildProcessRulesView()
    {
        var view = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        var toolbar = CreateCommandBar();
        toolbar.Dock = DockStyle.Top;
        toolbar.Height = 48;
        toolbar.Padding = new Padding(0, 2, 0, 7);
        _manualProcessInput = new AntdUI.Input
        {
            Width = 240,
            Height = 36,
            PlaceholderText = "输入进程名",
            PrefixSvg = "SearchOutlined",
            Radius = 5,
            Margin = new Padding(0, 4, 0, 0)
        };
        toolbar.Controls.Add(_manualProcessInput);
        toolbar.Controls.Add(CreateAntButton("添加", "PlusOutlined", (_, _) => AddManualIgnoreProcess(),
            AntdUI.TTypeMini.Primary));
        toolbar.Controls.Add(CreateAntButton("选择运行进程", "ControlOutlined", betterProcessAddButton_Click));

        ignoreProcessView.Dock = DockStyle.Fill;
        ignoreProcessView.Margin = Padding.Empty;
        view.Controls.Add(ignoreProcessView);
        view.Controls.Add(toolbar);
        return view;
    }

    private Control BuildAnalyzerPage()
    {
        var page = CreatePageLayout(3);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 4,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
        toolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        selectedPathTextBox.Dock = DockStyle.Fill;
        selectedPathTextBox.Margin = new Padding(0, 7, 8, 7);
        PrepareLegacyToolbarButton(selectDirBtn, "浏览目录");
        PrepareLegacyToolbarButton(scanBtn, "开始扫描");
        PrepareLegacyToolbarButton(stopBtn, "停止");
        toolbar.Controls.Add(selectedPathTextBox, 0, 0);
        toolbar.Controls.Add(selectDirBtn, 1, 0);
        toolbar.Controls.Add(scanBtn, 2, 0);
        toolbar.Controls.Add(stopBtn, 3, 0);
        page.Controls.Add(toolbar, 0, 0);

        scanProgressBar.Dock = DockStyle.Fill;
        scanProgressBar.Margin = new Padding(0, 5, 0, 5);
        page.Controls.Add(scanProgressBar, 0, 1);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var treeSurface = CreateSurface(new Padding(12));
        treeSurface.Margin = new Padding(0, 10, 8, 0);
        folderTreeView.Dock = DockStyle.Fill;
        folderTreeView.AfterSelect += FolderTreeViewAfterSelect;
        treeSurface.Controls.Add(folderTreeView);
        content.Controls.Add(treeSurface, 0, 0);

        var details = CreateSurface(new Padding(18));
        details.Margin = new Padding(8, 10, 0, 0);
        BuildAnalyzerDetails(details);
        content.Controls.Add(details, 1, 0);
        page.Controls.Add(content, 0, 2);
        return page;
    }

    private void BuildAnalyzerDetails(Control details)
    {
        var title = new Label
        {
            Text = "选中目录",
            Location = new Point(18, 18),
            Size = new Size(260, 28),
            Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent
        };
        _analyzerPathValue = CreateDetailValue("请先扫描并选择目录", 18, 58, 260, 64);
        _analyzerSizeValue = CreateDetailValue("大小：-", 18, 136, 260, 28);
        _analyzerFilesValue = CreateDetailValue("文件：-", 18, 174, 260, 28);
        _analyzerFoldersValue = CreateDetailValue("子目录：-", 18, 212, 260, 28);
        var useForCleanup = CreateAntButton("作为清理来源", "DeleteOutlined", (_, _) => UseAnalyzerPathForCleanup(),
            AntdUI.TTypeMini.Primary);
        useForCleanup.Location = new Point(18, 262);
        useForCleanup.Size = new Size(160, 38);

        details.Controls.Add(title);
        details.Controls.Add(_analyzerPathValue);
        details.Controls.Add(_analyzerSizeValue);
        details.Controls.Add(_analyzerFilesValue);
        details.Controls.Add(_analyzerFoldersValue);
        details.Controls.Add(useForCleanup);
        details.Resize += (_, _) =>
        {
            var width = Math.Max(160, details.ClientSize.Width - 36);
            if (_analyzerPathValue != null) _analyzerPathValue.Width = width;
            if (_analyzerSizeValue != null) _analyzerSizeValue.Width = width;
            if (_analyzerFilesValue != null) _analyzerFilesValue.Width = width;
            if (_analyzerFoldersValue != null) _analyzerFoldersValue.Width = width;
        };
    }

    private static Label CreateDetailValue(string text, int x, int y, int width, int height)
    {
        return new Label
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, height),
            Font = new Font("Microsoft YaHei UI", 9.5F),
            ForeColor = UiTheme.TextSecondary,
            BackColor = Color.Transparent,
            AutoEllipsis = true
        };
    }

    private Control BuildCleanupPage()
    {
        var page = CreatePageLayout(3);
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 3,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        toolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        cleanPathTextBox.Dock = DockStyle.Fill;
        cleanPathTextBox.Margin = new Padding(0, 7, 8, 7);
        PrepareLegacyToolbarButton(cleanSelectDirBtn, "浏览目录");
        PrepareLegacyToolbarButton(cleanScanBtn, "开始扫描");
        toolbar.Controls.Add(cleanPathTextBox, 0, 0);
        toolbar.Controls.Add(cleanSelectDirBtn, 1, 0);
        toolbar.Controls.Add(cleanScanBtn, 2, 0);
        page.Controls.Add(toolbar, 0, 0);

        cleanScanProgressBar.Dock = DockStyle.Fill;
        cleanScanProgressBar.Margin = new Padding(0, 5, 0, 5);
        page.Controls.Add(cleanScanProgressBar, 0, 1);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 2,
            RowCount = 1,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 344F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        content.Controls.Add(BuildCleanupTreeSurface(), 0, 0);
        content.Controls.Add(BuildCleanupActionSurface(), 1, 0);
        page.Controls.Add(content, 0, 2);
        return page;
    }

    private Control BuildCleanupTreeSurface()
    {
        var surface = CreateSurface(new Padding(12));
        surface.Margin = new Padding(0, 10, 8, 0);
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
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

        var selectionBar = CreateCommandBar();
        selectionBar.Padding = Padding.Empty;
        PrepareLegacyToolbarButton(cleanSelectAllBtn, "全选");
        PrepareLegacyToolbarButton(cleanSelectNoneBtn, "全不选");
        cleanSelectAllBtn.Width = 72;
        cleanSelectNoneBtn.Width = 84;
        _cleanupSelectionLabel = new Label
        {
            Text = "已选择 0 项 / 0 B",
            Width = 310,
            Height = 34,
            Margin = new Padding(12, 4, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.Transparent,
            AutoEllipsis = true
        };
        selectionBar.Controls.Add(cleanSelectAllBtn);
        selectionBar.Controls.Add(cleanSelectNoneBtn);
        selectionBar.Controls.Add(_cleanupSelectionLabel);
        layout.Controls.Add(selectionBar, 0, 0);

        cleanTreeView.Dock = DockStyle.Fill;
        cleanTreeView.Margin = Padding.Empty;
        layout.Controls.Add(cleanTreeView, 0, 1);
        cleanStatusLabel.Dock = DockStyle.Fill;
        cleanStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        cleanStatusLabel.AutoEllipsis = true;
        layout.Controls.Add(cleanStatusLabel, 0, 2);
        surface.Controls.Add(layout);
        return surface;
    }

    private Control BuildCleanupActionSurface()
    {
        var surface = CreateSurface(new Padding(12));
        surface.Margin = new Padding(8, 10, 0, 0);
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 2,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 216F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var frequentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        var frequentTitle = new Label
        {
            Text = "高频修改路径",
            Dock = DockStyle.Top,
            Height = 34,
            Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft
        };
        var refresh = CreateAntButton(string.Empty, "ReloadOutlined", cleanRefreshFrequentBtn_Click);
        refresh.Size = new Size(34, 32);
        refresh.Location = new Point(272, 0);
        refresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        frequentPathListView.Dock = DockStyle.Fill;
        frequentPathListView.Margin = Padding.Empty;
        frequentHintLabel.Dock = DockStyle.Bottom;
        frequentHintLabel.Height = 30;
        frequentHintLabel.AutoEllipsis = true;
        frequentPanel.Controls.Add(frequentPathListView);
        frequentPanel.Controls.Add(frequentHintLabel);
        frequentPanel.Controls.Add(refresh);
        frequentPanel.Controls.Add(frequentTitle);
        layout.Controls.Add(frequentPanel, 0, 0);

        var methodPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
        var methodTitle = new Label
        {
            Text = "清理方式",
            Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
            ForeColor = UiTheme.TextPrimary,
            BackColor = Color.White,
            Location = new Point(4, 12),
            Size = new Size(260, 28)
        };
        methodPanel.Controls.Add(methodTitle);
        methodPanel.Controls.Add(cleanRecycleRadio);
        methodPanel.Controls.Add(cleanPermanentRadio);
        methodPanel.Controls.Add(cleanMoveRadio);
        methodPanel.Controls.Add(cleanCompressRadio);
        methodPanel.Controls.Add(cleanMklinkRadio);
        methodPanel.Controls.Add(cleanTargetLabel);
        methodPanel.Controls.Add(cleanTargetTextBox);
        methodPanel.Controls.Add(cleanTargetSelectBtn);
        methodPanel.Controls.Add(cleanBtn);
        methodPanel.Resize += (_, _) => LayoutCleanupMethodPanel(methodPanel);
        LayoutCleanupMethodPanel(methodPanel);
        layout.Controls.Add(methodPanel, 0, 1);

        surface.Controls.Add(layout);
        return surface;
    }

    private void LayoutCleanupMethodPanel(Control panel)
    {
        var width = Math.Max(250, panel.ClientSize.Width);
        var y = 46;
        foreach (var radio in new[]
                 {
                     cleanRecycleRadio, cleanPermanentRadio, cleanMoveRadio,
                     cleanCompressRadio, cleanMklinkRadio
                 })
        {
            radio.SetBounds(4, y, width - 8, 27);
            y += 29;
        }

        cleanTargetLabel.SetBounds(4, y + 3, width - 8, 24);
        y += 29;
        cleanTargetTextBox.SetBounds(4, y, Math.Max(120, width - 92), 32);
        cleanTargetSelectBtn.SetBounds(width - 80, y - 1, 76, 34);
        cleanBtn.SetBounds(4, Math.Max(y + 46, panel.ClientSize.Height - 48), width - 8, 42);
    }

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

    private static void PrepareLegacyToolbarButton(Button button, string text)
    {
        button.Text = text;
        button.Dock = DockStyle.Fill;
        button.Margin = new Padding(4, 6, 4, 6);
        button.MinimumSize = new Size(64, 34);
    }

    private void WorkspaceMenuItemClick(object sender, AntdUI.MenuItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.ID))
            ShowWorkspacePage(item.ID);
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

    private void ShowRulesView(bool directories)
    {
        if (_rulesDirectoryView == null || _rulesProcessView == null) return;
        _rulesDirectoryView.Visible = directories;
        _rulesProcessView.Visible = !directories;
        if (directories) _rulesDirectoryView.BringToFront();
        else _rulesProcessView.BringToFront();

        if (_rulesDirectoryTab != null)
            _rulesDirectoryTab.Type = directories ? AntdUI.TTypeMini.Primary : AntdUI.TTypeMini.Default;
        if (_rulesProcessTab != null)
            _rulesProcessTab.Type = directories ? AntdUI.TTypeMini.Default : AntdUI.TTypeMini.Primary;
    }

    private void AddManualIgnoreProcess()
    {
        var processName = _manualProcessInput?.Text.Trim();
        if (string.IsNullOrWhiteSpace(processName))
        {
            AntdUI.Message.warn(this, "请输入进程名");
            return;
        }

        AddIgnoreProcessInternal(processName);
        if (_manualProcessInput != null) _manualProcessInput.Text = string.Empty;
        RefreshDashboardMetrics();
    }

    private void FolderTreeViewAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not FolderSizeInfo info) return;
        if (_analyzerPathValue != null) _analyzerPathValue.Text = info.Path;
        if (_analyzerSizeValue != null) _analyzerSizeValue.Text = $"大小：{FormatBytes(info.SizeBytes)}";
        if (_analyzerFilesValue != null) _analyzerFilesValue.Text = $"文件：{info.FileCount:N0}";
        if (_analyzerFoldersValue != null) _analyzerFoldersValue.Text = $"子目录：{info.SubFolders.Count:N0}";
    }

    private void UseAnalyzerPathForCleanup()
    {
        var path = _analyzerPathValue?.Text;
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            AntdUI.Message.warn(this, "请先在分析结果中选择有效目录");
            return;
        }

        cleanPathTextBox.Text = path;
        ShowWorkspacePage(CleanupPageId);
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

    private void RefreshRecordsCenter()
    {
        if (_notificationRecordsGrid == null || _processStatsGrid == null || _detailRecordsGrid == null)
            return;

        List<FileChangeRecord> records;
        lock (_recordsLock)
            records = _records.ToList();

        if (records.Count == 0)
        {
            try { records = _databaseService.GetChangeRecords(1000); }
            catch { records = new List<FileChangeRecord>(); }
        }

        List<ProcessNotificationRecord> notifications;
        try { notifications = _databaseService.GetProcessNotifications(500); }
        catch { notifications = new List<ProcessNotificationRecord>(); }

        var stats = records
            .GroupBy(r => string.IsNullOrWhiteSpace(r.SourceProcess) ? "未知进程" : r.SourceProcess,
                StringComparer.OrdinalIgnoreCase)
            .Select(g => new AppChangeStats
            {
                AppName = g.Key,
                ChangeCount = g.Count(),
                FirstChangeTime = g.Min(r => r.Timestamp),
                LastChangeTime = g.Max(r => r.Timestamp)
            })
            .OrderByDescending(s => s.ChangeCount)
            .ThenByDescending(s => s.LastChangeTime)
            .ToList();

        _notificationRecordsGrid.DataSource = new BindingList<ProcessNotificationRecord>(notifications);
        _processStatsGrid.DataSource = new BindingList<AppChangeStats>(stats);
        _detailRecordsGrid.DataSource = new BindingList<FileChangeRecord>(
            records.OrderByDescending(r => r.Timestamp).ToList());
        RefreshCleanHistory();
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
            _dashboardUsageLabel.Text = $"{info.UsagePercent:0.#}% 已使用  ·  剩余 {FormatBytes(info.FreeSpaceBytes)}";
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
                $"总容量 {FormatBytes(info.TotalSizeBytes)}    已用 {FormatBytes(info.UsedSpaceBytes)}    剩余 {FormatBytes(info.FreeSpaceBytes)}";
        }
        if (_workspaceDiskStatus != null)
        {
            _workspaceDiskStatus.Text = $"C: 剩余 {FormatBytes(info.FreeSpaceBytes)} / {FormatBytes(info.TotalSizeBytes)}";
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

    private void UpdateCleanupSelectionSummary()
    {
        if (_cleanupSelectionLabel == null) return;
        var selected = GetCheckedEntries();
        _cleanupSelectionLabel.Text = $"已选择 {selected.Count:N0} 项 / {FormatBytes(selected.Sum(e => e.SizeBytes))}";
    }

    private void WorkspaceFormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isExiting || e.CloseReason != CloseReason.UserClosing) return;
        e.Cancel = true;
        Hide();
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
