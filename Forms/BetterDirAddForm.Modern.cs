using CdiskClean.Helpers;

namespace CdiskClean.Forms;

public partial class BetterDirAddForm
{
    private void BuildDialogShell()
    {
        SuspendLayout();
        Controls.Clear();
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(760, 650);
        MinimumSize = new Size(680, 560);
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
            Text = "添加监控目录",
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
            RowCount = 3
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var pathBar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 3,
            RowCount = 1
        };
        pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82F));
        pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
        basePathLabel.Text = "基础目录";
        basePathLabel.Dock = DockStyle.Fill;
        basePathLabel.TextAlign = ContentAlignment.MiddleLeft;
        basePathTextBox.Dock = DockStyle.Fill;
        basePathTextBox.Margin = new Padding(0, 4, 10, 6);
        basePathTextBox.Radius = 5;
        browseBtn.Dock = DockStyle.Fill;
        ConfigureDialogButton(browseBtn, "FolderOpenOutlined");
        pathBar.Controls.Add(basePathLabel, 0, 0);
        pathBar.Controls.Add(basePathTextBox, 1, 0);
        pathBar.Controls.Add(browseBtn, 2, 0);
        content.Controls.Add(pathBar, 0, 0);

        var selectionBar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Color.White
        };
        ConfigureDialogButton(selectAllBtn, "CheckSquareOutlined");
        ConfigureDialogButton(selectNoneBtn, "BorderOutlined");
        selectionBar.Controls.Add(selectAllBtn);
        selectionBar.Controls.Add(selectNoneBtn);
        content.Controls.Add(selectionBar, 0, 1);

        dirTreeView.Dock = DockStyle.Fill;
        content.Controls.Add(dirTreeView, 0, 2);
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
