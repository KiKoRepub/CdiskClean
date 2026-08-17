using CdiskClean.Helpers;

namespace CdiskClean.Forms;

public partial class ProcessPickForm
{
    private void BuildDialogShell()
    {
        SuspendLayout();
        Controls.Clear();
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(760, 620);
        MinimumSize = new Size(680, 540);
        BackColor = UiTheme.Canvas;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = UiTheme.Canvas,
            ColumnCount = 1,
            RowCount = 3
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
        root.Controls.Add(new AntdUI.PageHeader
        {
            Dock = DockStyle.Fill,
            Text = "选择忽略进程",
            ShowButton = true,
            ShowIcon = false,
            BackColor = Color.White,
            ForeColor = UiTheme.TextPrimary,
            Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold)
        }, 0, 0);

        var surface = new AntdUI.Panel
        {
            Dock = DockStyle.Fill,
            Radius = 6,
            BackColor = Color.White,
            Margin = new Padding(18, 16, 18, 0),
            Padding = new Padding(16)
        };
        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 2
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var searchBar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 3,
            RowCount = 1
        };
        searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
        searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        searchLabel.Text = "搜索";
        searchLabel.Dock = DockStyle.Fill;
        searchLabel.TextAlign = ContentAlignment.MiddleLeft;
        searchTextBox.Dock = DockStyle.Fill;
        searchTextBox.Margin = new Padding(0, 4, 10, 6);
        searchTextBox.PlaceholderText = "输入进程名或窗口标题";
        searchTextBox.PrefixSvg = "SearchOutlined";
        searchTextBox.Radius = 5;
        refreshBtn.Dock = DockStyle.Fill;
        ConfigureDialogButton(refreshBtn, "ReloadOutlined");
        searchBar.Controls.Add(searchLabel, 0, 0);
        searchBar.Controls.Add(searchTextBox, 1, 0);
        searchBar.Controls.Add(refreshBtn, 2, 0);
        content.Controls.Add(searchBar, 0, 0);

        procListView.Dock = DockStyle.Fill;
        content.Controls.Add(procListView, 0, 1);
        surface.Controls.Add(content);
        root.Controls.Add(surface, 0, 1);

        var footer = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = UiTheme.Canvas,
            Padding = new Padding(18, 10, 18, 8)
        };
        ConfigureDialogButton(cancelBtn, "CloseOutlined");
        ConfigureDialogButton(okBtn, "CheckOutlined", AntdUI.TTypeMini.Primary);
        footer.Controls.Add(cancelBtn);
        footer.Controls.Add(okBtn);
        root.Controls.Add(footer, 0, 2);
        Controls.Add(root);
        ResumeLayout(true);
    }

    private static void ConfigureDialogButton(
        AntdUI.Button button,
        string icon,
        AntdUI.TTypeMini type = AntdUI.TTypeMini.Default)
    {
        button.IconSvg = icon;
        button.Type = type;
        button.Radius = 5;
        button.Size = new Size(Math.Max(96, button.Width), 36);
        button.Margin = new Padding(0, 0, 8, 0);
        button.Font = new Font("Microsoft YaHei UI", 9.5F);
    }
}
