namespace CdiskClean.Helpers;

/// <summary>
/// AntdUI 对话框壳构建工具：多个对话框复用同一套"页头 + 内容面 + 页脚按钮"布局。
/// </summary>
internal static class DialogShellBuilder
{
    public static TableLayoutPanel Build(
        Form form,
        string title,
        Size clientSize,
        Size minimumSize,
        out AntdUI.Panel surface,
        out FlowLayoutPanel footer)
    {
        form.SuspendLayout();
        form.Controls.Clear();
        form.FormBorderStyle = FormBorderStyle.None;
        form.ClientSize = clientSize;
        form.MinimumSize = minimumSize;
        form.BackColor = UiTheme.Canvas;

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
            Text = title,
            ShowButton = true,
            ShowIcon = false,
            BackColor = Color.White,
            ForeColor = UiTheme.TextPrimary,
            Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold)
        }, 0, 0);

        surface = new AntdUI.Panel
        {
            Dock = DockStyle.Fill,
            Radius = 6,
            BackColor = Color.White,
            Margin = new Padding(18, 16, 18, 0),
            Padding = new Padding(16)
        };
        root.Controls.Add(surface, 0, 1);

        footer = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = UiTheme.Canvas,
            Padding = new Padding(18, 10, 18, 8)
        };
        root.Controls.Add(footer, 0, 2);

        form.Controls.Add(root);
        form.ResumeLayout(true);
        return root;
    }

    public static void ConfigureButton(
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
