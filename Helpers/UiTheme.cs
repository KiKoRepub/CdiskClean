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

            if (form.Name == "Form1")
                ConfigureMainWindow(form);
            else
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
                case TabControl tabs:
                    StyleTabs(tabs);
                    break;
                case TabPage page:
                    page.BackColor = Canvas;
                    page.ForeColor = TextPrimary;
                    break;
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case ListView list:
                    StyleList(list);
                    break;
                case TreeView tree:
                    StyleTree(tree);
                    break;
                case GroupBox group:
                    StyleGroup(group);
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
                case StatusStrip status:
                    StyleToolStrip(status);
                    status.SizingGrip = false;
                    break;
                case ToolStrip strip:
                    StyleToolStrip(strip);
                    break;
                case SplitContainer split:
                    split.BackColor = Canvas;
                    split.Panel1.BackColor = Canvas;
                    split.Panel2.BackColor = Canvas;
                    break;
                default:
                    control.ForeColor = TextPrimary;
                    break;
            }
        }

        private static void StyleTabs(TabControl tabs)
        {
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.ItemSize = new Size(tabs.TabCount > 3 ? 138 : 150, 40);
            tabs.Padding = new Point(16, 5);
            tabs.TabStop = false;
            tabs.DrawItem -= DrawTabItem;
            tabs.DrawItem += DrawTabItem;
        }

        private static void DrawTabItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabs || e.Index < 0 || e.Index >= tabs.TabPages.Count)
                return;

            bool selected = e.Index == tabs.SelectedIndex;
            Rectangle bounds = e.Bounds;
            using var background = new SolidBrush(selected ? Surface : Canvas);
            e.Graphics.FillRectangle(background, bounds);

            if (selected)
            {
                using var accent = new SolidBrush(Primary);
                e.Graphics.FillRectangle(accent, bounds.Left + 14, bounds.Bottom - 3, bounds.Width - 28, 3);
            }

            TextRenderer.DrawText(e.Graphics, tabs.TabPages[e.Index].Text,
                selected ? StrongFont : BodyFont, bounds,
                selected ? Primary : TextSecondary,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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

        private static void StyleGroup(GroupBox group)
        {
            group.BackColor = Surface;
            group.ForeColor = TextPrimary;
            group.Font = StrongFont;
            group.Padding = new Padding(12, 10, 12, 12);
            group.Paint -= PaintGroupBox;
            group.Paint += PaintGroupBox;
        }

        private static void PaintGroupBox(object? sender, PaintEventArgs e)
        {
            if (sender is not GroupBox group || group.Width < 2 || group.Height < 12)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(group.BackColor);
            Rectangle bounds = new(0, 9, group.Width - 1, group.Height - 10);
            using GraphicsPath path = CreateRoundedRectangle(bounds, 7);
            using var borderPen = new Pen(Border);
            e.Graphics.DrawPath(borderPen, path);

            Size textSize = TextRenderer.MeasureText(group.Text, StrongFont);
            using var background = new SolidBrush(group.BackColor);
            e.Graphics.FillRectangle(background, 13, 0, textSize.Width + 12, textSize.Height);
            TextRenderer.DrawText(e.Graphics, group.Text, StrongFont,
                new Point(18, 0), TextPrimary, TextFormatFlags.NoPadding);
        }

        private static void StyleButton(Button button)
        {
            if (IsWindowButton(button))
                return;

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
            button.Name is "pauseBtn" or "scanBtn" or "cleanScanBtn" or "cleanBtn" or
                "okBtn" or "processAddButton" or "dirAddButton";

        private static bool IsDangerButton(Button button) =>
            button.Name is "stopBtn" or "clearBtn";

        private static bool IsWindowButton(Button button) =>
            button.Name is "closeButton" or "BiggerButton" or "button1";

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
                "dirSelectedTextBox" => "未选择监测目录",
                "procSelectedTextBox" => "未选择忽略进程",
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
            if (label.Name is "dashboardTitleLabel" or "titleLabel")
                label.Font = HeadingFont;
            else if (label.Name.Contains("hint", StringComparison.OrdinalIgnoreCase))
                label.ForeColor = TextSecondary;
        }

        private static void StyleToolStrip(ToolStrip strip)
        {
            strip.BackColor = Surface;
            strip.ForeColor = TextSecondary;
            strip.RenderMode = ToolStripRenderMode.System;
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

        private static void ConfigureMainWindow(Form form)
        {
            if (FindControl<Control>(form, "workspaceRoot") != null)
                return;

            if (FindControl<SplitContainer>(form, "panelTitle") is SplitContainer title)
            {
                title.BackColor = TitleBar;
                title.Panel1.BackColor = TitleBar;
                title.Panel2.BackColor = TitleBar;
                title.SplitterWidth = 1;
            }

            if (FindControl<Label>(form, "label1") is Label titleLabel)
            {
                titleLabel.ForeColor = Color.White;
                titleLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            }

            ConfigureWindowButton(form, "button1", "\u2014", "最小化", false);
            ConfigureWindowButton(form, "BiggerButton", "\u25A1", "最大化或还原", false);
            ConfigureWindowButton(form, "closeButton", "\u00D7", "关闭", true);
            BuildOverviewCard(form);
            LayoutMainPages(form);
        }

        private static void ConfigureWindowButton(Form form, string name, string text, string tooltip, bool close)
        {
            if (FindControl<Button>(form, name) is not Button button)
                return;

            button.Text = text;
            button.Font = new Font("Segoe UI Symbol", 11F);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = close ? Danger : Color.FromArgb(58, 63, 73);
            button.FlatAppearance.MouseDownBackColor = close ? Color.FromArgb(180, 35, 24) : Color.FromArgb(75, 81, 92);
            button.BackColor = TitleBar;
            button.ForeColor = Color.FromArgb(234, 236, 240);
            button.UseVisualStyleBackColor = false;
            button.Cursor = Cursors.Hand;
            WindowToolTip.SetToolTip(button, tooltip);
        }

        private static void BuildOverviewCard(Form form)
        {
            if (FindControl<TabPage>(form, "totalReviewPage") is not TabPage page ||
                page.Controls.ContainsKey("diskOverviewCard"))
                return;

            var card = new Panel
            {
                Name = "diskOverviewCard",
                BackColor = Surface,
                Location = new Point(28, 28),
                Size = new Size(Math.Max(720, page.ClientSize.Width - 56), 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Paint += PaintOverviewCard;

            MoveToCard(page, card, "dashboardTitleLabel", new Point(28, 24));
            MoveToCard(page, card, "usageProgressBar", new Point(28, 74));
            MoveToCard(page, card, "totalSpaceLabel", new Point(28, 124));
            MoveToCard(page, card, "usedSpaceLabel", new Point(270, 124));
            MoveToCard(page, card, "freeSpaceLabel", new Point(500, 124));
            MoveToCard(page, card, "warningLabel", new Point(28, 178));

            if (FindControl<ProgressBar>(page, "progressBar1") is ProgressBar unused)
                unused.Visible = false;

            if (FindControl<ProgressBar>(card, "usageProgressBar") is ProgressBar usage)
            {
                usage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                usage.Size = new Size(card.ClientSize.Width - 56, 18);
                usage.ForeColor = Primary;
            }

            page.Controls.Add(card);
            card.BringToFront();
        }

        private static void PaintOverviewCard(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel)
                return;

            using var borderPen = new Pen(Border);
            e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
            using var accent = new SolidBrush(Primary);
            e.Graphics.FillRectangle(accent, 0, 0, panel.Width, 4);
        }

        private static void MoveToCard(TabPage page, Panel card, string name, Point location)
        {
            if (FindControl<Control>(page, name) is not Control control)
                return;

            page.Controls.Remove(control);
            control.Location = location;
            card.Controls.Add(control);
        }

        private static void LayoutMainPages(Form form)
        {
            RegisterPageLayout(form, "watcherPage", LayoutWatcherPage);
            RegisterPageLayout(form, "folderAnalyzerPage", LayoutAnalyzerPage);
            RegisterPageLayout(form, "diskCleanPage", LayoutCleanPage);
        }

        private static void RegisterPageLayout(Form form, string name, Action<TabPage> layout)
        {
            if (FindControl<TabPage>(form, name) is not TabPage page)
                return;

            layout(page);
            page.Resize += (_, _) => layout(page);
        }

        private static void LayoutWatcherPage(TabPage page)
        {
            int rightWidth = Math.Min(560, Math.Max(430, page.ClientSize.Width / 2 - 40));
            int rightX = page.ClientSize.Width - rightWidth - 16;
            int leftWidth = Math.Max(420, rightX - 36);
            SetBounds(page, "changesDataGrid", 16, 58, leftWidth, Math.Max(250, page.ClientSize.Height - 132));
            SetBounds(page, "statisticButton", 16, page.ClientSize.Height - 60, 148, 40);
            SetBounds(page, "WatcherDirectoryBox", rightX, 16, rightWidth, 280);
            SetBounds(page, "ignoreProcessBox", rightX, 312, rightWidth, Math.Max(220, page.ClientSize.Height - 328));

            if (FindControl<GroupBox>(page, "ignoreProcessBox") is GroupBox ignore)
                ignore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private static void LayoutAnalyzerPage(TabPage page)
        {
            const int margin = 16;
            const int buttonWidth = 100;
            const int gap = 8;
            int stopX = page.ClientSize.Width - margin - 82;
            int scanX = stopX - gap - buttonWidth;
            int selectX = scanX - gap - buttonWidth;
            SetBounds(page, "selectedPathTextBox", margin, 16, Math.Max(220, selectX - margin - 10), 30);
            SetBounds(page, "selectDirBtn", selectX, 14, buttonWidth, 34);
            SetBounds(page, "scanBtn", scanX, 14, buttonWidth, 34);
            SetBounds(page, "stopBtn", stopX, 14, 82, 34);
            SetBounds(page, "scanProgressBar", margin, 60, page.ClientSize.Width - margin * 2, 18);
            SetBounds(page, "folderTreeView", margin, 92,
                page.ClientSize.Width - margin * 2, Math.Max(180, page.ClientSize.Height - 108));
        }

        private static void LayoutCleanPage(TabPage page)
        {
            const int margin = 16;
            const int refreshWidth = 150;
            int refreshX = page.ClientSize.Width - margin - refreshWidth;
            SetBounds(page, "cleanPathTextBox", margin, 14, 350, 30);
            SetBounds(page, "cleanSelectDirBtn", 374, 12, 100, 34);
            SetBounds(page, "cleanScanBtn", 482, 12, 100, 34);
            SetBounds(page, "cleanRefreshFrequentBtn", refreshX, 12, refreshWidth, 34);
            SetBounds(page, "cleanScanProgressBar", 594, 20, Math.Max(120, refreshX - 606), 18);
            SetBounds(page, "frequentPathBox", margin, 62, 344, 314);
            SetBounds(page, "cleanTreeBox", 376, 62, Math.Max(420, page.ClientSize.Width - 392), 314);
            SetBounds(page, "cleanMethodBox", margin, 390, page.ClientSize.Width - margin * 2, 94);
            SetBounds(page, "cleanHistoryBox", margin, 498,
                page.ClientSize.Width - margin * 2, Math.Max(120, page.ClientSize.Height - 514));

            if (FindControl<GroupBox>(page, "cleanTreeBox") is GroupBox treeGroup)
            {
                SetBounds(treeGroup, "cleanTreeView", 14, 28,
                    treeGroup.ClientSize.Width - 28, treeGroup.ClientSize.Height - 80);
                SetBounds(treeGroup, "cleanSelectAllBtn", 14, treeGroup.ClientSize.Height - 42, 72, 30);
                SetBounds(treeGroup, "cleanSelectNoneBtn", 94, treeGroup.ClientSize.Height - 42, 86, 30);
                SetBounds(treeGroup, "cleanStatusLabel", 192, treeGroup.ClientSize.Height - 40,
                    treeGroup.ClientSize.Width - 206, 26);
            }

            if (FindControl<GroupBox>(page, "cleanMethodBox") is GroupBox methodGroup)
                SetBounds(methodGroup, "cleanBtn", methodGroup.ClientSize.Width - 164, 24, 150, 58);

            if (FindControl<GroupBox>(page, "cleanHistoryBox") is GroupBox history &&
                FindControl<DataGridView>(history, "cleanHistoryGrid") is DataGridView grid)
            {
                grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                grid.SetBounds(12, 28, history.ClientSize.Width - 24, history.ClientSize.Height - 40);
            }
        }

        private static void SetBounds(Control parent, string name, int x, int y, int width, int height)
        {
            if (FindControl<Control>(parent, name) is Control control)
                control.SetBounds(x, y, Math.Max(1, width), Math.Max(1, height));
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

    internal sealed class ThemedProgressBar : ProgressBar
    {
        public ThemedProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle track = new(0, 0, Width - 1, Height - 1);
            using GraphicsPath trackPath = CreatePath(track, Math.Max(2, Height / 2));
            using var trackBrush = new SolidBrush(Color.FromArgb(234, 236, 240));
            e.Graphics.FillPath(trackBrush, trackPath);

            if (Maximum <= Minimum || Value <= Minimum)
                return;

            float ratio = (float)(Value - Minimum) / (Maximum - Minimum);
            int fillWidth = Math.Max(Height, (int)(Width * ratio));
            Rectangle fill = new(0, 0, Math.Min(fillWidth, Width - 1), Height - 1);
            using GraphicsPath fillPath = CreatePath(fill, Math.Max(2, Height / 2));
            using var fillBrush = new SolidBrush(ForeColor.IsEmpty ? UiTheme.Primary : ForeColor);
            e.Graphics.FillPath(fillBrush, fillPath);
        }

        private static GraphicsPath CreatePath(Rectangle bounds, int radius)
        {
            int diameter = Math.Min(Math.Min(radius * 2, bounds.Width), bounds.Height);
            var path = new GraphicsPath();
            if (diameter <= 1)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
