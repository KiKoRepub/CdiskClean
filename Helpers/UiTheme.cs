using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace CdiskClean.Helpers
{
    internal static class UiTheme
    {
        internal static readonly Color Canvas = Color.FromArgb(245, 247, 250);
        internal static readonly Color Surface = Color.White;
        internal static readonly Color SurfaceMuted = Color.FromArgb(248, 250, 252);
        internal static readonly Color Border = Color.FromArgb(218, 223, 230);
        internal static readonly Color TextPrimary = Color.FromArgb(31, 41, 55);
        internal static readonly Color TextSecondary = Color.FromArgb(102, 112, 133);
        internal static readonly Color Primary = Color.FromArgb(22, 119, 255);
        internal static readonly Color PrimaryHover = Color.FromArgb(64, 150, 255);
        internal static readonly Color PrimarySoft = Color.FromArgb(230, 244, 255);
        internal static readonly Color Danger = Color.FromArgb(217, 45, 32);
        internal static readonly Color Success = Color.FromArgb(22, 163, 74);
        internal static readonly Color TitleBar = Color.FromArgb(35, 39, 47);

        private static readonly Font BodyFont = new("Microsoft YaHei UI", 9.5F);
        private static readonly Font StrongFont = new("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
        private static readonly Font HeadingFont = new("Microsoft YaHei UI", 12F, FontStyle.Bold);
        private static readonly ToolTip WindowToolTip = new() { ShowAlways = true };

        public static void Apply(Form form)
        {
            form.SuspendLayout();
            form.BackColor = Canvas;
            StyleControlTree(form);

            if (form.Name != "Form1")
                ConfigureDialog(form);

            form.ResumeLayout(true);
        }

        private static void StyleControlTree(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                StyleControl(control);
                StyleControlTree(control);
            }
        }

        private static void StyleControl(Control control)
        {
            control.Font = BodyFont;
            switch (control)
            {
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case ListView list:
                    StyleList(list);
                    break;
                case TreeView tree:
                    StyleTree(tree);
                    break;
                case Button button:
                    StyleButton(button);
                    break;
                case TextBox textBox:
                    StyleTextBox(textBox);
                    break;
                case ComboBox combo:
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = Surface;
                    combo.ForeColor = TextPrimary;
                    break;
                case Label label:
                    StyleLabel(label);
                    break;
                default:
                    control.ForeColor = TextPrimary;
                    break;
            }
        }

        private static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Border;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 36;
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = PrimarySoft;
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(9, 88, 217);
            grid.DefaultCellStyle.Padding = new Padding(6, 2, 6, 2);
            grid.AlternatingRowsDefaultCellStyle.BackColor = SurfaceMuted;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = PrimarySoft;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 247);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = StrongFont;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        }

        private static void StyleList(ListView list)
        {
            list.BackColor = Surface;
            list.ForeColor = TextPrimary;
            list.BorderStyle = BorderStyle.FixedSingle;
            list.FullRowSelect = true;
            list.HideSelection = false;
            list.GridLines = false;

            if (list.Name is "procListView" or "watcherDirListView" or
                "ignoreProcessView" or "frequentPathListView")
            {
                list.Resize -= ResizeListColumns;
                list.Resize += ResizeListColumns;
                ResizeListColumns(list, EventArgs.Empty);
            }
        }

        private static void ResizeListColumns(object? sender, EventArgs e)
        {
            if (sender is not ListView list || list.Columns.Count == 0)
                return;

            int width = Math.Max(300, list.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            switch (list.Name)
            {
                case "procListView" when list.Columns.Count == 3:
                    list.Columns[0].Width = (int)(width * 0.28);
                    list.Columns[1].Width = (int)(width * 0.14);
                    list.Columns[2].Width = width - list.Columns[0].Width - list.Columns[1].Width;
                    break;
                case "watcherDirListView" when list.Columns.Count == 3:
                    list.Columns[0].Width = (int)(width * 0.70);
                    list.Columns[1].Width = (int)(width * 0.15);
                    list.Columns[2].Width = width - list.Columns[0].Width - list.Columns[1].Width;
                    break;
                case "ignoreProcessView" when list.Columns.Count == 2:
                    list.Columns[0].Width = (int)(width * 0.80);
                    list.Columns[1].Width = width - list.Columns[0].Width;
                    break;
                case "frequentPathListView" when list.Columns.Count == 2:
                    list.Columns[1].Width = Math.Min(92, width / 3);
                    list.Columns[0].Width = width - list.Columns[1].Width;
                    break;
            }
        }

        private static void StyleTree(TreeView tree)
        {
            tree.BackColor = Surface;
            tree.ForeColor = TextPrimary;
            tree.BorderStyle = BorderStyle.FixedSingle;
            tree.HideSelection = false;
            tree.FullRowSelect = true;
            tree.ItemHeight = Math.Max(tree.ItemHeight, 28);
            tree.LineColor = Border;
        }

        private static void StyleButton(Button button)
        {
            bool primary = IsPrimaryButton(button);
            bool danger = IsDangerButton(button);
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            button.Font = StrongFont;
            button.FlatAppearance.BorderSize = primary || danger ? 0 : 1;
            button.FlatAppearance.BorderColor = Border;

            if (danger)
            {
                button.BackColor = Danger;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 47, 47);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 35, 24);
            }
            else if (primary)
            {
                button.BackColor = Primary;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = PrimaryHover;
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(9, 88, 217);
            }
            else
            {
                button.BackColor = Surface;
                button.ForeColor = TextPrimary;
                button.FlatAppearance.MouseOverBackColor = SurfaceMuted;
                button.FlatAppearance.MouseDownBackColor = PrimarySoft;
            }

            UpdateButtonRegion(button);
            button.Resize -= ButtonResize;
            button.Resize += ButtonResize;
        }

        private static bool IsPrimaryButton(Button button) =>
            button.Name is "pauseBtn" or "scanBtn" or "cleanScanBtn" or "cleanBtn" or "dirAddButton";

        private static bool IsDangerButton(Button button) =>
            button.Name is "stopBtn" or "clearBtn";

        private static void ButtonResize(object? sender, EventArgs e)
        {
            if (sender is Button button)
                UpdateButtonRegion(button);
        }

        private static void UpdateButtonRegion(Button button)
        {
            if (button.Width <= 0 || button.Height <= 0)
                return;

            using GraphicsPath path = CreateRoundedRectangle(
                new Rectangle(0, 0, button.Width, button.Height), 6);
            button.Region?.Dispose();
            button.Region = new Region(path);
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.ForeColor = TextPrimary;
            textBox.BackColor = textBox.ReadOnly ? SurfaceMuted : Surface;

            string? placeholder = textBox.Name switch
            {
                "searchTextBox" => "输入进程名或窗口标题",
                "cleanPathTextBox" => "请选择需要扫描的目录",
                "selectedPathTextBox" => "请选择需要分析的目录",
                "cleanTargetTextBox" => "请选择目标目录",
                _ => null
            };

            if (placeholder != null)
                SetPlaceholder(textBox, placeholder);
        }

        private static void SetPlaceholder(TextBox textBox, string placeholder)
        {
            void ApplyPlaceholder(object? sender, EventArgs e)
            {
                if (sender is TextBox box && box.IsHandleCreated)
                    SendMessage(box.Handle, 0x1501, (IntPtr)1, placeholder);
            }

            if (textBox.IsHandleCreated)
                SendMessage(textBox.Handle, 0x1501, (IntPtr)1, placeholder);
            else
                textBox.HandleCreated += ApplyPlaceholder;
        }

        private static void StyleLabel(Label label)
        {
            label.ForeColor = TextPrimary;
            if (label.Name.Contains("hint", StringComparison.OrdinalIgnoreCase))
                label.ForeColor = TextSecondary;
        }

        private static void ConfigureDialog(Form form)
        {
            form.BackColor = Canvas;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimumSize = form.Size;

            if (FindControl<Label>(form, "hintLabel") is Label hint)
            {
                hint.AutoSize = false;
                hint.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                hint.ForeColor = TextSecondary;
                LayoutDialogHint(form, hint);
                form.Resize += (_, _) => LayoutDialogHint(form, hint);
            }
        }

        private static void LayoutDialogHint(Form form, Label hint)
        {
            hint.SetBounds(14, form.ClientSize.Height - 31,
                Math.Max(100, form.ClientSize.Width - 28), 24);
        }

        private static T? FindControl<T>(Control root, string name) where T : Control
        {
            if (root is T typedRoot && root.Name == name)
                return typedRoot;

            foreach (Control child in root.Controls)
            {
                if (child is T typedChild && child.Name == name)
                    return typedChild;

                T? nested = FindControl<T>(child, name);
                if (nested != null)
                    return nested;
            }

            return null;
        }

        private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int message, IntPtr wParam, string lParam);
    }
}
