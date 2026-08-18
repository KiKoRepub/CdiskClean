using CdiskClean.Helpers;

namespace CdiskClean.Forms;

public partial class BetterDirAddForm
{
    private void BuildDialogShell()
    {
        var root = DialogShellBuilder.Build(this, "添加监控目录",
            new Size(760, 650), new Size(680, 560),
            out var surface, out var footer);

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
        DialogShellBuilder.ConfigureButton(browseBtn, "FolderOpenOutlined");
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
        DialogShellBuilder.ConfigureButton(selectAllBtn, "CheckSquareOutlined");
        DialogShellBuilder.ConfigureButton(selectNoneBtn, "BorderOutlined");
        selectionBar.Controls.Add(selectAllBtn);
        selectionBar.Controls.Add(selectNoneBtn);
        content.Controls.Add(selectionBar, 0, 1);

        dirTreeView.Dock = DockStyle.Fill;
        content.Controls.Add(dirTreeView, 0, 2);
        surface.Controls.Add(content);

        DialogShellBuilder.ConfigureButton(cancelBtn, "CloseOutlined");
        DialogShellBuilder.ConfigureButton(okBtn, "CheckOutlined", AntdUI.TTypeMini.Primary);
        footer.Controls.Add(cancelBtn);
        footer.Controls.Add(okBtn);
    }
}
