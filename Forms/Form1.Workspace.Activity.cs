using CdiskClean.Helpers;

namespace CdiskClean;

/// <summary>工作区「实时活动」页：监测开关、筛选、搜索与变更记录网格</summary>
public partial class Form1
{
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
}
