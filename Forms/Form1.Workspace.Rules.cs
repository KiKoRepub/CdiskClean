using CdiskClean.Helpers;

namespace CdiskClean;

/// <summary>工作区「监控规则」页：监测目录 / 忽略进程两个子视图</summary>
public partial class Form1
{
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
}
