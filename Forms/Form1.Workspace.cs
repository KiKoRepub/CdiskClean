using Sunny.UI;
using System.Drawing.Drawing2D;

namespace CdiskClean;

public partial class Form1
{
#if DEBUG
    private bool _workspaceReady;
#endif
    private UIPanel? _workspaceRoot;
    private UIPanel? _workspaceHeader;
    private UIPanel? _workspaceSidebar;
    private UIPanel? _workspaceContent;
    private UILabel? _workspacePageTitle;
    private UILabel? _workspacePageHint;
    private UIPanel? _workspaceFooter;
    private UILedBulb? _watchLed;
    private UILabel? _watchStatusLabel;
    private UILabel? _recordStatusLabel;
    private UILabel? _timeStatusLabel;
    private readonly List<UIButton> _workspaceNavButtons = new();
    private readonly Dictionary<int, string> _workspaceTitles = new()
    {
        [0] = "工作台",
        [1] = "实时活动",
        [2] = "空间分析",
        [3] = "清理中心",
        [4] = "监控规则",
        [5] = "记录中心"
    };

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
#if DEBUG
        if (!_workspaceReady && !DesignMode)
        {
            BuildWorkspaceShell();
        }
#endif
    }

    private void BuildWorkspaceShell()
    {
#if DEBUG
        _workspaceReady = true;
#endif
        ShowTitle = false;
        ControlBox = false;
        Padding = new Padding(0);
        panelTitle.Visible = false;
        SuspendLayout();

        BuildWorkspacePageBoundaries();
        ApplyDebugPageLayouts();

        _workspaceRoot = new UIPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
            Style = UIStyle.Custom,
            FillColor = Color.FromArgb(245, 247, 250),
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty
        };

        _workspaceHeader = new UIPanel
        {
            Dock = DockStyle.Top,
            Height = 72,
            Style = UIStyle.Custom,
            FillColor = Color.FromArgb(35, 43, 54),
            RectColor = Color.FromArgb(35, 43, 54),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty,
            Cursor = Cursors.SizeAll
        };
        _workspaceHeader.MouseDown += WorkspaceHeader_MouseDown;
        _workspaceHeader.MouseMove += WorkspaceHeader_MouseMove;

        var brand = new UILabel
        {
            AutoSize = true,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            Text = "CdiskClean",
            ForeColor = Color.White,
            Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold),
            Location = new Point(22, 9),
            BackColor = Color.Transparent
        };
        var subtitle = new UILabel
        {
            AutoSize = true,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            Text = "磁盘监控与清理控制台",
            ForeColor = Color.FromArgb(190, 199, 210),
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(24, 42),
            BackColor = Color.Transparent
        };
        _workspaceHeader.Controls.Add(brand);
        _workspaceHeader.Controls.Add(subtitle);

        var roleText = label1.Text
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? string.Empty;
        var role = new UILabel
        {
            AutoSize = false,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            Text = label1.Text.Replace("C盘监测工具", string.Empty).Trim(' ', '—'),
            ForeColor = Color.FromArgb(214, 220, 228),
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(0, 20),
            Size = new Size(180, 32),
            TextAlign = ContentAlignment.MiddleRight,
            BackColor = Color.Transparent
        };
        role.Text = roleText;
        _workspaceHeader.Controls.Add(role);

        var minimize = CreateHeaderButton("—", "最小化", (_, _) => WindowState = FormWindowState.Minimized);
        minimize.Location = new Point(Width - 118, 19);
        var maximize = CreateHeaderButton("□", "最大化/还原", (_, _) =>
        {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        });
        maximize.Location = new Point(Width - 80, 19);
        var close = CreateHeaderButton("×", "隐藏到托盘", (_, _) => Hide());
        close.Location = new Point(Width - 42, 19);
        _workspaceHeader.Controls.Add(minimize);
        _workspaceHeader.Controls.Add(maximize);
        _workspaceHeader.Controls.Add(close);
        void LayoutHeaderActions()
        {
            minimize.Left = Math.Max(280, _workspaceHeader.Width - 118);
            maximize.Left = Math.Max(318, _workspaceHeader.Width - 80);
            close.Left = Math.Max(356, _workspaceHeader.Width - 42);
            role.Left = Math.Max(240, minimize.Left - role.Width - 16);
        }
        _workspaceHeader.Resize += (_, _) => LayoutHeaderActions();
        LayoutHeaderActions();

        _workspaceSidebar = new UIPanel
        {
            Dock = DockStyle.Left,
            Width = 214,
            Style = UIStyle.Custom,
            FillColor = Color.White,
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty,
            Padding = new Padding(12, 16, 12, 12)
        };
        var navTitle = new UILabel
        {
            AutoSize = true,
            Text = "功能导航",
            Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(102, 112, 133),
            Location = new Point(16, 16),
            BackColor = Color.Transparent
        };
        _workspaceSidebar.Controls.Add(navTitle);

        AddNavButton("工作台", 0, "查看磁盘、监控和最近活动");
        AddNavButton("实时活动", 1, "查看实时文件变更");
        AddNavButton("监控规则", 4, "管理监控目录和忽略进程");
        AddNavButton("空间分析", 2, "扫描目录空间占用");
        AddNavButton("清理中心", 3, "选择方式并执行清理");
        AddNavButton("记录中心", 5, "查看提醒、统计和历史记录");

        _workspaceContent = new UIPanel
        {
            Dock = DockStyle.Fill,
            Style = UIStyle.Custom,
            FillColor = Color.FromArgb(245, 247, 250),
            RectColor = Color.FromArgb(245, 247, 250),
            StyleCustomMode = true,
            Radius = 0,
            Padding = new Padding(20, 16, 20, 20)
        };
        var pageBar = new UIPanel
        {
            Dock = DockStyle.Fill,
            Style = UIStyle.Custom,
            FillColor = Color.FromArgb(245, 247, 250),
            RectColor = Color.FromArgb(245, 247, 250),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty
        };
        _workspacePageTitle = new UILabel
        {
            AutoSize = true,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            Text = "工作台",
            Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            Location = new Point(0, 4),
            BackColor = Color.Transparent
        };
        _workspacePageHint = new UILabel
        {
            AutoSize = true,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            Text = "系统运行状态和磁盘空间总览",
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = Color.FromArgb(102, 112, 133),
            Location = new Point(2, 40),
            BackColor = Color.Transparent
        };
        pageBar.Controls.Add(_workspacePageTitle);
        pageBar.Controls.Add(_workspacePageHint);

        TabPageControl1.Appearance = TabAppearance.FlatButtons;
        TabPageControl1.ItemSize = new Size(1, 1);
        TabPageControl1.SizeMode = TabSizeMode.Fixed;
        TabPageControl1.Dock = DockStyle.Fill;
        TabPageControl1.Padding = new Point(0, 0);
        TabPageControl1.BackColor = Color.FromArgb(245, 247, 250);
        TabPageControl1.SelectedIndexChanged += (_, _) => UpdateWorkspacePage(TabPageControl1.SelectedIndex);
        var contentLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(0),
            Padding = new Padding(0)
        };
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        contentLayout.Controls.Add(pageBar, 0, 0);
        contentLayout.Controls.Add(TabPageControl1, 0, 1);
        _workspaceContent.Controls.Add(contentLayout);

        // 底部状态栏脚印
        _workspaceFooter = new UIPanel
        {
            Dock = DockStyle.Bottom, Height = 34,
            FillColor = Color.FromArgb(35, 43, 54), RectColor = Color.FromArgb(35, 43, 54),
            Style = UIStyle.Custom, StyleCustomMode = true, Radius = 0, Text = string.Empty
        };
        _watchLed = new UILedBulb { Size = new Size(14, 14), Location = new Point(16, 10), On = false, Color = Color.Gray };
        _watchStatusLabel = new UILabel { AutoSize = true, Location = new Point(36, 8), ForeColor = Color.White, Font = new Font("Microsoft YaHei UI", 9F), Text = "未开始监测" };
        _recordStatusLabel = new UILabel { AutoSize = true, Location = new Point(150, 8), ForeColor = Color.FromArgb(214, 220, 228), Font = new Font("Microsoft YaHei UI", 9F), Text = "已记录 0 条" };
        _timeStatusLabel = new UILabel { AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(0, 8), ForeColor = Color.FromArgb(214, 220, 228), Font = new Font("Microsoft YaHei UI", 9F), Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
        _workspaceFooter.Controls.Add(_watchLed);
        _workspaceFooter.Controls.Add(_watchStatusLabel);
        _workspaceFooter.Controls.Add(_recordStatusLabel);
        _workspaceFooter.Controls.Add(_timeStatusLabel);
        foreach (var statusLabel in new[] { _watchStatusLabel, _recordStatusLabel, _timeStatusLabel })
        {
            statusLabel.Style = UIStyle.Custom;
            statusLabel.StyleCustomMode = true;
            statusLabel.BackColor = Color.Transparent;
        }
        _watchStatusLabel.Style = UIStyle.Custom;
        _watchStatusLabel.StyleCustomMode = true;
        _recordStatusLabel.Style = UIStyle.Custom;
        _recordStatusLabel.StyleCustomMode = true;
        _timeStatusLabel.Style = UIStyle.Custom;
        _timeStatusLabel.StyleCustomMode = true;
        _watchStatusLabel.Click += watchStatusLabel_Click;
        _recordStatusLabel.Click += WritedRecordStatusLabel_Click;
        _workspaceFooter.Resize += (_, _) => _timeStatusLabel.Left = Math.Max(20, _workspaceFooter.Width - _timeStatusLabel.Width - 16);
        _workspaceRoot.Controls.Add(_workspaceContent);
        _workspaceRoot.Controls.Add(_workspaceSidebar);
        _workspaceRoot.Controls.Add(_workspaceHeader);
        _workspaceRoot.Controls.Add(_workspaceFooter);
        Controls.Add(_workspaceRoot);
        _workspaceRoot.BringToFront();

        UpdateWorkspacePage(0);
        ResumeLayout(true);
    }

    private void BuildWorkspacePageBoundaries()
    {
        if (TabPageControl1.TabPages.Count > 4)
            return;

        var rulesPage = new TabPage("监控规则")
        {
            BackColor = Color.FromArgb(245, 247, 250),
            Padding = new Padding(12)
        };
        var rulesLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Padding = new Padding(0)
        };
        rulesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        rulesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        rulesPage.Controls.Add(rulesLayout);

        WatcherDirectoryBox.Parent = rulesLayout;
        WatcherDirectoryBox.Dock = DockStyle.Fill;
        WatcherDirectoryBox.Margin = new Padding(0, 0, 0, 8);
        WatcherDirectoryBox.Text = "监控目录规则";
        rulesLayout.Controls.Add(WatcherDirectoryBox, 0, 0);

        ignoreProcessBox.Parent = rulesLayout;
        ignoreProcessBox.Dock = DockStyle.Fill;
        ignoreProcessBox.Margin = new Padding(0, 8, 0, 0);
        ignoreProcessBox.Text = "忽略进程规则";
        rulesLayout.Controls.Add(ignoreProcessBox, 0, 1);

        TabPageControl1.TabPages.Add(rulesPage);
        watcherPage.BackColor = Color.FromArgb(245, 247, 250);
        watcherPage.Padding = new Padding(12);
        changesDataGrid.Dock = DockStyle.Fill;
        changesDataGrid.BringToFront();
        rulesPage.BringToFront();
    }

    private void ApplyDebugPageLayouts()
    {
        BackColor = Color.FromArgb(244, 246, 249);
        Font = new Font("Microsoft YaHei UI", 9F);

        foreach (var grid in new DataGridView[]
                 {
                     changesDataGrid, watcherDirListView, ignoreProcessView,
                     frequentPathListView, cleanHistoryGrid
                 })
        {
            ConfigureWorkspaceGrid(grid);
        }

        foreach (var button in new[]
                 {
                     pauseBtn, clearBtn, exportBtn, statisticButton,
                     dirAddButton, betterDirAddButton, processAddButton, betterProcessAddButton,
                     selectDirBtn, scanBtn, stopBtn, cleanSelectDirBtn, cleanScanBtn,
                     cleanRefreshFrequentBtn, cleanSelectAllBtn, cleanSelectNoneBtn,
                     cleanTargetSelectBtn, cleanBtn
                 })
        {
            ConfigureWorkspaceButton(button);
        }

        ConfigurePrimaryButton(pauseBtn, Color.FromArgb(27, 111, 181));
        ConfigurePrimaryButton(scanBtn, Color.FromArgb(27, 111, 181));
        ConfigurePrimaryButton(cleanScanBtn, Color.FromArgb(27, 111, 181));
        ConfigurePrimaryButton(cleanBtn, Color.FromArgb(194, 65, 55));

        ConfigureOverviewPage();
        ConfigureActivityPage();
        ConfigureRulesPage();
        ConfigureFolderAnalyzerPage();
        ConfigureCleanupPage();
    }

    private void ConfigureOverviewPage()
    {
        totalReviewPage.SuspendLayout();
        totalReviewPage.Controls.Clear();
        totalReviewPage.BackColor = Color.FromArgb(244, 246, 249);
        totalReviewPage.Padding = new Padding(0);

        progressBar1.Visible = false;
        dashboardTitleLabel.Text = "系统盘 C:";
        dashboardTitleLabel.Dock = DockStyle.Fill;
        dashboardTitleLabel.AutoSize = false;
        dashboardTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        dashboardTitleLabel.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
        dashboardTitleLabel.ForeColor = Color.FromArgb(35, 43, 54);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(0, 4, 0, 0)
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 142));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var metrics = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 3,
            RowCount = 1,
            Margin = new Padding(0, 8, 0, 10)
        };
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        metrics.Controls.Add(CreateMetricTile("总容量", totalSpaceLabel, Color.FromArgb(27, 111, 181)), 0, 0);
        metrics.Controls.Add(CreateMetricTile("已使用", usedSpaceLabel, Color.FromArgb(218, 142, 46)), 1, 0);
        metrics.Controls.Add(CreateMetricTile("可用空间", freeSpaceLabel, Color.FromArgb(46, 139, 87)), 2, 0);

        var usagePanel = new UIPanel
        {
            Dock = DockStyle.Fill,
            FillColor = Color.White,
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 4,
            Margin = new Padding(0, 0, 0, 8),
            Text = string.Empty
        };
        var usageTitle = new UILabel
        {
            AutoSize = true,
            Text = "磁盘使用率",
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 84, 103),
            Location = new Point(18, 16),
            BackColor = Color.Transparent
        };
        usageProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        usageProgressBar.Location = new Point(18, 48);
        usageProgressBar.Height = 18;
        warningLabel.AutoSize = false;
        warningLabel.Location = new Point(18, 72);
        warningLabel.Height = 28;
        warningLabel.TextAlign = ContentAlignment.MiddleLeft;
        warningLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
        usagePanel.Controls.Add(usageTitle);
        usagePanel.Controls.Add(usageProgressBar);
        usagePanel.Controls.Add(warningLabel);
        usagePanel.Resize += (_, _) =>
        {
            usageProgressBar.Width = Math.Max(120, usagePanel.ClientSize.Width - 36);
            warningLabel.Width = Math.Max(120, usagePanel.ClientSize.Width - 36);
        };

        root.Controls.Add(dashboardTitleLabel, 0, 0);
        root.Controls.Add(metrics, 0, 1);
        root.Controls.Add(usagePanel, 0, 2);
        totalReviewPage.Controls.Add(root);
        totalReviewPage.ResumeLayout(true);
    }

    private static UIPanel CreateMetricTile(string title, UILabel valueLabel, Color accent)
    {
        var tile = new UIPanel
        {
            Dock = DockStyle.Fill,
            FillColor = Color.White,
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 4,
            Margin = new Padding(0, 0, 10, 0),
            Text = string.Empty
        };
        var accentBar = new UIPanel
        {
            Dock = DockStyle.Left,
            Width = 4,
            FillColor = accent,
            RectColor = accent,
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty
        };
        var titleLabel = new UILabel
        {
            AutoSize = false,
            Text = title,
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = Color.FromArgb(102, 112, 133),
            Location = new Point(20, 20),
            Size = new Size(180, 24),
            BackColor = Color.Transparent
        };
        valueLabel.AutoSize = false;
        valueLabel.Location = new Point(20, 52);
        valueLabel.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
        valueLabel.ForeColor = Color.FromArgb(31, 41, 55);
        valueLabel.TextAlign = ContentAlignment.MiddleLeft;
        tile.Controls.Add(accentBar);
        tile.Controls.Add(titleLabel);
        tile.Controls.Add(valueLabel);
        tile.Resize += (_, _) => valueLabel.Size = new Size(Math.Max(80, tile.ClientSize.Width - 38), 52);
        return tile;
    }

    private void ConfigureActivityPage()
    {
        watcherPage.SuspendLayout();
        watcherPage.Controls.Clear();
        watcherPage.BackColor = Color.FromArgb(244, 246, 249);
        watcherPage.Padding = new Padding(0);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 2
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = Color.White,
            Padding = new Padding(8, 8, 8, 6),
            Margin = new Padding(0, 0, 0, 10)
        };
        pauseBtn.Size = new Size(104, 34);
        clearBtn.Size = new Size(80, 34);
        exportBtn.Size = new Size(80, 34);
        statisticButton.Size = new Size(104, 34);
        typeFilterLabel.AutoSize = false;
        typeFilterLabel.Size = new Size(96, 34);
        typeFilterLabel.Text = "变更类型";
        typeFilterLabel.TextAlign = ContentAlignment.MiddleRight;
        typeFilterLabel.Margin = new Padding(22, 0, 0, 0);
        typeFilterCombo.Size = new Size(138, 34);
        typeFilterCombo.Margin = new Padding(6, 0, 0, 0);
        toolbar.Controls.Add(pauseBtn);
        toolbar.Controls.Add(clearBtn);
        toolbar.Controls.Add(exportBtn);
        toolbar.Controls.Add(statisticButton);
        toolbar.Controls.Add(typeFilterLabel);
        toolbar.Controls.Add(typeFilterCombo);

        changesDataGrid.Dock = DockStyle.Fill;
        changesDataGrid.Margin = new Padding(0);
        root.Controls.Add(toolbar, 0, 0);
        root.Controls.Add(changesDataGrid, 0, 1);
        watcherPage.Controls.Add(root);
        watcherPage.ResumeLayout(true);
    }

    private void ConfigureRulesPage()
    {
        if (TabPageControl1.TabPages.Count <= 4) return;
        var rulesPage = TabPageControl1.TabPages[4];
        rulesPage.BackColor = Color.FromArgb(244, 246, 249);
        rulesPage.Padding = new Padding(0);

        if (rulesPage.Controls.OfType<TableLayoutPanel>().FirstOrDefault() is { } layout)
        {
            layout.Padding = new Padding(0);
            WatcherDirectoryBox.Margin = new Padding(0, 0, 0, 6);
            ignoreProcessBox.Margin = new Padding(0, 6, 0, 0);
        }

        ConfigureRuleGroup(
            WatcherDirectoryBox,
            watcherDirListView,
            label2,
            dirSelectedTextBox,
            dirAddButton,
            betterDirAddButton,
            "当前目录",
            "添加目录",
            "批量选择");
        ConfigureRuleGroup(
            ignoreProcessBox,
            ignoreProcessView,
            label3,
            procSelectedTextBox,
            processAddButton,
            betterProcessAddButton,
            "当前进程",
            "手动添加",
            "从进程选择");
    }

    private static void ConfigureRuleGroup(
        UIGroupBox group,
        UIDataGridView grid,
        UILabel selectedLabel,
        UITextBox selectedText,
        UIButton primaryButton,
        UIButton secondaryButton,
        string selectedCaption,
        string primaryCaption,
        string secondaryCaption)
    {
        StyleWorkspaceGroup(group);
        selectedLabel.Text = selectedCaption;
        selectedLabel.Font = new Font("Microsoft YaHei UI", 9F);
        selectedLabel.TextAlign = ContentAlignment.MiddleLeft;
        selectedLabel.AutoSize = false;
        primaryButton.Text = primaryCaption;
        secondaryButton.Text = secondaryCaption;
        primaryButton.Size = new Size(108, 34);
        secondaryButton.Size = new Size(116, 34);
        grid.ReadOnly = true;
        grid.Dock = DockStyle.None;
        selectedText.Multiline = false;

        void LayoutChildren()
        {
            var width = group.ClientSize.Width;
            var height = group.ClientSize.Height;
            secondaryButton.Location = new Point(Math.Max(16, width - 240), 30);
            primaryButton.Location = new Point(Math.Max(138, width - 116), 30);
            grid.Bounds = new Rectangle(16, 74, Math.Max(160, width - 32), Math.Max(72, height - 132));
            selectedLabel.Bounds = new Rectangle(16, Math.Max(78, height - 46), 82, 30);
            selectedText.Bounds = new Rectangle(102, Math.Max(78, height - 48), Math.Max(120, width - 118), 34);
        }

        group.Resize += (_, _) => LayoutChildren();
        LayoutChildren();
    }

    private void ConfigureFolderAnalyzerPage()
    {
        folderAnalyzerPage.SuspendLayout();
        folderAnalyzerPage.Controls.Clear();
        folderAnalyzerPage.BackColor = Color.FromArgb(244, 246, 249);
        folderAnalyzerPage.Padding = new Padding(0);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var toolbar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 4,
            RowCount = 1,
            Padding = new Padding(8, 7, 8, 7)
        };
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112));
        toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88));
        selectedPathTextBox.Dock = DockStyle.Fill;
        selectedPathTextBox.Margin = new Padding(0, 0, 8, 0);
        selectDirBtn.Dock = DockStyle.Fill;
        scanBtn.Dock = DockStyle.Fill;
        stopBtn.Dock = DockStyle.Fill;
        toolbar.Controls.Add(selectedPathTextBox, 0, 0);
        toolbar.Controls.Add(selectDirBtn, 1, 0);
        toolbar.Controls.Add(scanBtn, 2, 0);
        toolbar.Controls.Add(stopBtn, 3, 0);

        scanProgressBar.Dock = DockStyle.Fill;
        scanProgressBar.Margin = new Padding(0, 2, 0, 2);
        folderTreeView.Dock = DockStyle.Fill;
        folderTreeView.Margin = new Padding(0);
        folderTreeView.Font = new Font("Microsoft YaHei UI", 9F);
        root.Controls.Add(toolbar, 0, 0);
        root.Controls.Add(scanProgressBar, 0, 1);
        root.Controls.Add(folderTreeView, 0, 2);
        folderAnalyzerPage.Controls.Add(root);
        folderAnalyzerPage.ResumeLayout(true);
    }

    private void ConfigureCleanupPage()
    {
        diskCleanPage.SuspendLayout();
        diskCleanPage.Controls.Clear();
        diskCleanPage.BackColor = Color.FromArgb(244, 246, 249);
        diskCleanPage.Padding = new Padding(0);

        StyleWorkspaceGroup(frequentPathBox);
        StyleWorkspaceGroup(cleanTreeBox);
        StyleWorkspaceGroup(cleanMethodBox);
        StyleWorkspaceGroup(cleanHistoryBox);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 4
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 126));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 45));

        var toolbar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(8, 8, 8, 8),
            Margin = new Padding(0, 0, 0, 8)
        };
        cleanPathTextBox.Dock = DockStyle.None;
        cleanSelectDirBtn.Dock = DockStyle.None;
        cleanScanBtn.Dock = DockStyle.None;
        cleanScanProgressBar.Dock = DockStyle.None;
        cleanRefreshFrequentBtn.Dock = DockStyle.None;
        toolbar.Controls.Add(cleanPathTextBox);
        toolbar.Controls.Add(cleanSelectDirBtn);
        toolbar.Controls.Add(cleanScanBtn);
        toolbar.Controls.Add(cleanScanProgressBar);
        toolbar.Controls.Add(cleanRefreshFrequentBtn);

        void LayoutCleanupToolbar()
        {
            const int padding = 8;
            const int gap = 8;
            const int actionWidth = 112;
            const int refreshWidth = 130;
            var width = toolbar.ClientSize.Width;

            cleanScanBtn.Bounds = new Rectangle(Math.Max(padding, width - padding - actionWidth), 4, actionWidth, 34);
            cleanSelectDirBtn.Bounds = new Rectangle(Math.Max(padding, cleanScanBtn.Left - gap - actionWidth), 4, actionWidth, 34);
            cleanPathTextBox.Bounds = new Rectangle(
                padding,
                4,
                Math.Max(160, cleanSelectDirBtn.Left - gap - padding),
                34);
            cleanRefreshFrequentBtn.Bounds = new Rectangle(
                Math.Max(padding, width - padding - refreshWidth),
                42,
                refreshWidth,
                30);
            cleanScanProgressBar.Bounds = new Rectangle(
                padding,
                46,
                Math.Max(120, cleanRefreshFrequentBtn.Left - gap - padding),
                22);
        }

        toolbar.Resize += (_, _) => LayoutCleanupToolbar();
        LayoutCleanupToolbar();

        var scanArea = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0, 0, 0, 8)
        };
        scanArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31));
        scanArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 69));
        frequentPathBox.Dock = DockStyle.Fill;
        frequentPathBox.Margin = new Padding(0, 0, 6, 0);
        cleanTreeBox.Dock = DockStyle.Fill;
        cleanTreeBox.Margin = new Padding(6, 0, 0, 0);
        scanArea.Controls.Add(frequentPathBox, 0, 0);
        scanArea.Controls.Add(cleanTreeBox, 1, 0);

        cleanMethodBox.Dock = DockStyle.Fill;
        cleanMethodBox.Margin = new Padding(0, 0, 0, 8);
        cleanHistoryBox.Dock = DockStyle.Fill;
        cleanHistoryBox.Margin = new Padding(0);
        root.Controls.Add(toolbar, 0, 0);
        root.Controls.Add(scanArea, 0, 1);
        root.Controls.Add(cleanMethodBox, 0, 2);
        root.Controls.Add(cleanHistoryBox, 0, 3);
        diskCleanPage.Controls.Add(root);

        ConfigureFrequentPathGroup();
        ConfigureCleanupTreeGroup();
        ConfigureCleanupMethodGroup();
        ConfigureCleanupHistoryGroup();
        diskCleanPage.ResumeLayout(true);
    }

    private void ConfigureFrequentPathGroup()
    {
        frequentHintLabel.Font = new Font("Microsoft YaHei UI", 8.5F);
        frequentHintLabel.ForeColor = Color.FromArgb(102, 112, 133);
        frequentPathListView.Dock = DockStyle.None;

        void LayoutChildren()
        {
            var width = frequentPathBox.ClientSize.Width;
            var height = frequentPathBox.ClientSize.Height;
            frequentPathListView.Bounds = new Rectangle(14, 38, Math.Max(120, width - 28), Math.Max(64, height - 82));
            frequentHintLabel.Bounds = new Rectangle(14, Math.Max(82, height - 36), Math.Max(120, width - 28), 24);
        }

        frequentPathBox.Resize += (_, _) => LayoutChildren();
        LayoutChildren();
    }

    private void ConfigureCleanupTreeGroup()
    {
        cleanTreeView.BorderStyle = BorderStyle.None;
        cleanTreeView.BackColor = Color.White;
        cleanTreeView.Font = new Font("Microsoft YaHei UI", 9F);
        cleanTreeView.ItemHeight = 26;
        cleanStatusLabel.Font = new Font("Microsoft YaHei UI", 8.5F);
        cleanStatusLabel.ForeColor = Color.FromArgb(71, 84, 103);

        void LayoutChildren()
        {
            var width = cleanTreeBox.ClientSize.Width;
            var height = cleanTreeBox.ClientSize.Height;
            cleanTreeView.Bounds = new Rectangle(14, 38, Math.Max(160, width - 28), Math.Max(64, height - 88));
            cleanSelectAllBtn.Bounds = new Rectangle(14, Math.Max(82, height - 40), 72, 30);
            cleanSelectNoneBtn.Bounds = new Rectangle(92, Math.Max(82, height - 40), 80, 30);
            cleanStatusLabel.Bounds = new Rectangle(184, Math.Max(82, height - 40), Math.Max(100, width - 198), 30);
        }

        cleanTreeBox.Resize += (_, _) => LayoutChildren();
        LayoutChildren();
    }

    private void ConfigureCleanupMethodGroup()
    {
        var radios = new[]
        {
            cleanRecycleRadio, cleanPermanentRadio, cleanMoveRadio,
            cleanCompressRadio, cleanMklinkRadio
        };
        foreach (var radio in radios)
        {
            radio.Font = new Font("Microsoft YaHei UI", 9F);
            radio.Height = 28;
        }
        cleanTargetLabel.Text = "目标目录";
        cleanTargetLabel.AutoSize = false;
        cleanTargetLabel.TextAlign = ContentAlignment.MiddleLeft;
        cleanTargetLabel.Font = new Font("Microsoft YaHei UI", 9F);

        void LayoutChildren()
        {
            var width = cleanMethodBox.ClientSize.Width;
            cleanRecycleRadio.Bounds = new Rectangle(16, 32, 182, 28);
            cleanPermanentRadio.Bounds = new Rectangle(204, 32, 102, 28);
            cleanMoveRadio.Bounds = new Rectangle(312, 32, 88, 28);
            cleanCompressRadio.Bounds = new Rectangle(406, 32, 88, 28);
            cleanMklinkRadio.Bounds = new Rectangle(500, 32, 160, 28);

            cleanTargetLabel.Bounds = new Rectangle(16, 76, 78, 32);
            cleanTargetSelectBtn.Bounds = new Rectangle(Math.Max(420, width - 240), 75, 96, 34);
            cleanBtn.Bounds = new Rectangle(Math.Max(522, width - 138), 75, 122, 34);
            cleanTargetTextBox.Bounds = new Rectangle(98, 75, Math.Max(180, cleanTargetSelectBtn.Left - 108), 34);
        }

        cleanMethodBox.Resize += (_, _) => LayoutChildren();
        LayoutChildren();
    }

    private void ConfigureCleanupHistoryGroup()
    {
        cleanHistoryGrid.Dock = DockStyle.None;

        void LayoutChildren()
        {
            var width = cleanHistoryBox.ClientSize.Width;
            var height = cleanHistoryBox.ClientSize.Height;
            var bounds = new Rectangle(14, 38, Math.Max(180, width - 28), Math.Max(54, height - 50));
            cleanHistoryGrid.Bounds = bounds;
            if (_cleanHistoryEmptyLabel != null)
                _cleanHistoryEmptyLabel.Bounds = bounds;
        }

        cleanHistoryBox.Resize += (_, _) => LayoutChildren();
        LayoutChildren();
    }

    private static void ConfigureWorkspaceGrid(DataGridView grid)
    {
        grid.BackgroundColor = Color.White;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Color.FromArgb(232, 236, 241);
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 84, 103);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 238, 252);
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(24, 77, 120);
        grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);
        grid.RowTemplate.Height = 34;
        grid.RowHeadersVisible = false;
    }

    private static void ConfigureWorkspaceButton(UIButton button)
    {
        button.Style = UIStyle.Custom;
        button.StyleCustomMode = true;
        button.Radius = 4;
        button.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
        button.FillColor = Color.White;
        button.RectColor = Color.FromArgb(199, 207, 217);
        button.ForeColor = Color.FromArgb(49, 61, 76);
        button.FillHoverColor = Color.FromArgb(239, 246, 252);
        button.ForeHoverColor = Color.FromArgb(27, 111, 181);
    }

    private static void ConfigurePrimaryButton(UIButton button, Color color)
    {
        button.FillColor = color;
        button.RectColor = color;
        button.ForeColor = Color.White;
        button.FillHoverColor = ControlPaint.Light(color, 0.12F);
        button.ForeHoverColor = Color.White;
    }

    private static void StyleWorkspaceGroup(UIGroupBox group)
    {
        group.FillColor = Color.White;
        group.RectColor = Color.FromArgb(218, 223, 230);
        group.StyleCustomMode = true;
        group.Radius = 4;
        group.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
    }

    private void AddNavButton(string text, int pageIndex, string hint)
    {
        if (_workspaceSidebar == null) return;
        var button = new UIButton
        {
            Text = text,
            Tag = new WorkspaceNavTag(pageIndex, hint),
            Width = 188,
            Height = 40,
            Location = new Point(12, 48 + _workspaceNavButtons.Count * 44),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0),
            Radius = 4,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            FillColor = Color.White,
            RectColor = Color.Transparent,
            ForeColor = Color.FromArgb(71, 84, 103),
            FillHoverColor = Color.FromArgb(230, 242, 255),
            ForeHoverColor = Color.FromArgb(22, 119, 255),
            FillSelectedColor = Color.FromArgb(222, 238, 252),
            RectSelectedColor = Color.FromArgb(166, 207, 239),
            ForeSelectedColor = Color.FromArgb(24, 77, 120)
        };
        button.Click += (_, _) =>
        {
            if (button.Tag is WorkspaceNavTag nav)
            {
                if (nav.PageIndex == 5)
                {
                    statisticButton.PerformClick();
                    return;
                }
                TabPageControl1.SelectedIndex = nav.PageIndex;
                UpdateWorkspacePage(nav.PageIndex, nav.Hint);
            }
        };
        _workspaceNavButtons.Add(button);
        _workspaceSidebar.Controls.Add(button);
    }

    private void UpdateWorkspacePage(int index, string? hint = null)
    {
        if (_workspacePageTitle == null || _workspacePageHint == null) return;
        _workspacePageTitle.Text = _workspaceTitles.TryGetValue(index, out var title) ? title : "工作台";
        _workspacePageHint.Text = hint ?? index switch
        {
            1 => "实时文件变更、筛选与导出",
            2 => "扫描目录并查看空间占用",
            3 => "选择清理方式并执行任务",
            4 => "管理监控目录和忽略进程",
            5 => "查看提醒、统计和清理历史",
            _ => "系统运行状态和磁盘空间总览"
        };
        foreach (var button in _workspaceNavButtons)
        {
            if (button.Tag is WorkspaceNavTag nav)
                button.Selected = nav.PageIndex == index;
        }
    }

    private UIButton CreateHeaderButton(string text, string tip, EventHandler click)
    {
        var button = new UIButton
        {
            Text = text,
            Width = 32,
            Height = 32,
            Radius = 4,
            Style = UIStyle.Custom,
            StyleCustomMode = true,
            FillColor = Color.FromArgb(35, 43, 54),
            RectColor = Color.FromArgb(35, 43, 54),
            ForeColor = Color.White,
            ForeHoverColor = Color.White,
            FillHoverColor = Color.FromArgb(65, 76, 90),
            Font = new Font("Segoe UI", 14F),
            TabStop = false
        };
        button.Click += click;
        new ToolTip().SetToolTip(button, tip);
        return button;
    }

    private Point _workspaceDragOrigin;
    private void WorkspaceHeader_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            _workspaceDragOrigin = e.Location;
    }

    private void WorkspaceHeader_MouseMove(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            Location = new Point(Location.X + e.X - _workspaceDragOrigin.X, Location.Y + e.Y - _workspaceDragOrigin.Y);
    }

    private sealed record WorkspaceNavTag(int PageIndex, string Hint);
}
