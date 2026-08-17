using CdiskClean.Helpers;

namespace CdiskClean;

public partial class StatisticForm
{
    private void BuildDialogShell()
    {
        SuspendLayout();
        Controls.Clear();
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1120, 700);
        MinimumSize = new Size(900, 600);
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
            Text = "统计与提醒记录",
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
            Padding = new Padding(12)
        };
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Margin = Padding.Empty;
        notificationGrid.Dock = DockStyle.Fill;
        statsGrid.Dock = DockStyle.Fill;
        detailDataGrid.Dock = DockStyle.Fill;
        surface.Controls.Add(mainTabControl);
        root.Controls.Add(surface, 0, 1);

        var footer = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            BackColor = UiTheme.Canvas,
            Padding = new Padding(18, 10, 18, 8)
        };
        closeBtn.IconSvg = "CloseOutlined";
        closeBtn.Radius = 5;
        closeBtn.Size = new Size(96, 36);
        closeBtn.Margin = Padding.Empty;
        closeBtn.Font = new Font("Microsoft YaHei UI", 9.5F);
        footer.Controls.Add(closeBtn);
        root.Controls.Add(footer, 0, 2);
        Controls.Add(root);
        ResumeLayout(true);
    }
}
