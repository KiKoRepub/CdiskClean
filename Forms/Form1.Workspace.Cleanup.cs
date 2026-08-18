using CdiskClean.Helpers;

namespace CdiskClean;

/// <summary>工作区「清理中心」页：扫描结果树、高频路径参考与清理方式面板</summary>
public partial class Form1
{
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
        if (_cleanupMethodRadios.Length == 0) return;
        var width = Math.Max(250, panel.ClientSize.Width);
        var y = 46;
        foreach (var (radio, _) in _cleanupMethodRadios)
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
}
