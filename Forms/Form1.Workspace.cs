using Sunny.UI;
using System.Drawing.Drawing2D;

namespace CdiskClean;

public partial class Form1
{
    private bool _workspaceReady;
    private UIPanel? _workspaceRoot;
    private UIPanel? _workspaceHeader;
    private UIPanel? _workspaceSidebar;
    private Panel? _workspaceContent;
    private Label? _workspacePageTitle;
    private Label? _workspacePageHint;
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
        if (!_workspaceReady && !DesignMode)
        {
            BuildWorkspaceShell();
        }
    }

    private void BuildWorkspaceShell()
    {
        _workspaceReady = true;
        ShowTitle = false;
        ControlBox = false;
        Padding = new Padding(0);
        panelTitle.Visible = false;
        SuspendLayout();

        _workspaceRoot = new UIPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
            FillColor = Color.FromArgb(245, 247, 250),
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty
        };

        _workspaceHeader = new UIPanel
        {
            Dock = DockStyle.Top,
            Height = 58,
            FillColor = Color.FromArgb(35, 43, 54),
            RectColor = Color.FromArgb(35, 43, 54),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty,
            Cursor = Cursors.SizeAll
        };
        _workspaceHeader.MouseDown += WorkspaceHeader_MouseDown;
        _workspaceHeader.MouseMove += WorkspaceHeader_MouseMove;

        var brand = new Label
        {
            AutoSize = true,
            Text = "CdiskClean",
            ForeColor = Color.White,
            Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold),
            Location = new Point(22, 10),
            BackColor = Color.Transparent
        };
        var subtitle = new Label
        {
            AutoSize = true,
            Text = "磁盘监控与清理控制台",
            ForeColor = Color.FromArgb(190, 199, 210),
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(24, 34),
            BackColor = Color.Transparent
        };
        _workspaceHeader.Controls.Add(brand);
        _workspaceHeader.Controls.Add(subtitle);

        var role = new Label
        {
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Text = label1.Text.Replace("C盘监测工具", string.Empty).Trim(' ', '—'),
            ForeColor = Color.FromArgb(214, 220, 228),
            Font = new Font("Microsoft YaHei UI", 9F),
            Location = new Point(Width - 170, 20),
            BackColor = Color.Transparent
        };
        _workspaceHeader.Controls.Add(role);

        var minimize = CreateHeaderButton("—", "最小化", (_, _) => WindowState = FormWindowState.Minimized);
        minimize.Location = new Point(Width - 118, 11);
        var maximize = CreateHeaderButton("□", "最大化/还原", (_, _) =>
        {
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        });
        maximize.Location = new Point(Width - 80, 11);
        var close = CreateHeaderButton("×", "隐藏到托盘", (_, _) => Hide());
        close.Location = new Point(Width - 42, 11);
        _workspaceHeader.Controls.Add(minimize);
        _workspaceHeader.Controls.Add(maximize);
        _workspaceHeader.Controls.Add(close);
        _workspaceHeader.Resize += (_, _) =>
        {
            role.Left = Math.Max(240, _workspaceHeader.Width - 170);
            minimize.Left = Math.Max(280, _workspaceHeader.Width - 118);
            maximize.Left = Math.Max(318, _workspaceHeader.Width - 80);
            close.Left = Math.Max(356, _workspaceHeader.Width - 42);
        };

        _workspaceSidebar = new UIPanel
        {
            Dock = DockStyle.Left,
            Width = 214,
            FillColor = Color.White,
            RectColor = Color.FromArgb(218, 223, 230),
            StyleCustomMode = true,
            Radius = 0,
            Text = string.Empty,
            Padding = new Padding(12, 16, 12, 12)
        };
        var navTitle = new Label
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

        _workspaceContent = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 247, 250),
            Padding = new Padding(20, 16, 20, 20)
        };
        var pageBar = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.Transparent };
        _workspacePageTitle = new Label
        {
            AutoSize = true,
            Text = "工作台",
            Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            Location = new Point(0, 1),
            BackColor = Color.Transparent
        };
        _workspacePageHint = new Label
        {
            AutoSize = true,
            Text = "系统运行状态和磁盘空间总览",
            Font = new Font("Microsoft YaHei UI", 9F),
            ForeColor = Color.FromArgb(102, 112, 133),
            Location = new Point(2, 28),
            BackColor = Color.Transparent
        };
        pageBar.Controls.Add(_workspacePageTitle);
        pageBar.Controls.Add(_workspacePageHint);
        _workspaceContent.Controls.Add(pageBar);

        TabPageControl1.Appearance = TabAppearance.FlatButtons;
        TabPageControl1.ItemSize = new Size(1, 1);
        TabPageControl1.SizeMode = TabSizeMode.Fixed;
        TabPageControl1.Dock = DockStyle.Fill;
        TabPageControl1.Padding = new Point(0, 0);
        TabPageControl1.BackColor = Color.FromArgb(245, 247, 250);
        TabPageControl1.SelectedIndexChanged += (_, _) => UpdateWorkspacePage(TabPageControl1.SelectedIndex);
        _workspaceContent.Controls.Add(TabPageControl1);

        _workspaceRoot.Controls.Add(_workspaceContent);
        _workspaceRoot.Controls.Add(_workspaceSidebar);
        _workspaceRoot.Controls.Add(_workspaceHeader);
        Controls.Add(_workspaceRoot);
        _workspaceRoot.BringToFront();

        ApplyWorkspaceControlStyle();
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
    private void ApplyWorkspaceControlStyle()
    {
        foreach (var button in new[] { pauseBtn, clearBtn, exportBtn, statisticButton, dirAddButton, betterDirAddButton, processAddButton, betterProcessAddButton, selectDirBtn, scanBtn, stopBtn, cleanSelectDirBtn, cleanScanBtn, cleanSelectAllBtn, cleanSelectNoneBtn, cleanTargetSelectBtn, cleanBtn, cleanRefreshFrequentBtn })
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(218, 223, 230);
            button.BackColor = Color.White;
            button.ForeColor = Color.FromArgb(31, 41, 55);
            button.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            button.Height = Math.Max(button.Height, 34);
        }
        foreach (var grid in new[] { changesDataGrid, cleanHistoryGrid })
        {
            grid.BackgroundColor = Color.White;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(102, 112, 133);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 242, 255);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 41, 55);
            grid.RowTemplate.Height = 36;
        }
        foreach (var textBox in new[] { selectedPathTextBox, cleanPathTextBox, cleanTargetTextBox, dirSelectedTextBox, procSelectedTextBox })
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.White;
            textBox.Font = new Font("Microsoft YaHei UI", 9F);
        }
        foreach (var tree in new[] { folderTreeView, cleanTreeView })
        {
            tree.BackColor = Color.White;
            tree.BorderStyle = BorderStyle.FixedSingle;
            tree.Font = new Font("Microsoft YaHei UI", 9F);
            tree.ItemHeight = 28;
        }
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
            ForeHoverColor = Color.FromArgb(22, 119, 255)
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
            FillColor = Color.Transparent,
            RectColor = Color.Transparent,
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
