using CdiskClean.Helpers;

namespace CdiskClean.Forms;

public partial class ProcessPickForm
{
    private void BuildDialogShell()
    {
        var root = DialogShellBuilder.Build(this, "选择忽略进程",
            new Size(760, 620), new Size(680, 540),
            out var surface, out var footer);

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
        DialogShellBuilder.ConfigureButton(refreshBtn, "ReloadOutlined");
        searchBar.Controls.Add(searchLabel, 0, 0);
        searchBar.Controls.Add(searchTextBox, 1, 0);
        searchBar.Controls.Add(refreshBtn, 2, 0);
        content.Controls.Add(searchBar, 0, 0);

        procListView.Dock = DockStyle.Fill;
        content.Controls.Add(procListView, 0, 1);
        surface.Controls.Add(content);

        DialogShellBuilder.ConfigureButton(cancelBtn, "CloseOutlined");
        DialogShellBuilder.ConfigureButton(okBtn, "CheckOutlined", AntdUI.TTypeMini.Primary);
        footer.Controls.Add(cancelBtn);
        footer.Controls.Add(okBtn);
    }
}
