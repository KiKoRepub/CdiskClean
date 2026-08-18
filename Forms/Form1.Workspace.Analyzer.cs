using CdiskClean.Helpers;
using CdiskClean.Models;

namespace CdiskClean;

/// <summary>工作区「空间分析」页：目录扫描树与选中目录详情面板</summary>
public partial class Form1
{
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

    private void FolderTreeViewAfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not FolderSizeInfo info) return;
        if (_analyzerPathValue != null) _analyzerPathValue.Text = info.Path;
        if (_analyzerSizeValue != null) _analyzerSizeValue.Text = $"大小：{FormatHelper.FormatBytes(info.SizeBytes)}";
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
}
