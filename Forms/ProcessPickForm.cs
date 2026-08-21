using System.Diagnostics;

using Sunny.UI;

namespace CdiskClean.Forms
{
    /// <summary>
    /// 高级添加忽略进程：列出当前系统运行的进程，支持搜索与多选。
    /// </summary>
    public partial class ProcessPickForm : Sunny.UI.UIForm
    {
        private List<(string Name, int Pid, string Title)> _allProcesses = new();

        /// <summary>确认后返回的进程名列表</summary>
        public List<string> SelectedProcessNames { get; private set; } = new();

        public ProcessPickForm()
        {
            InitializeComponent();
#if DEBUG
            ApplyDebugLayout();
#endif
            LoadProcesses();
        }

        private void ApplyDebugLayout()
        {
            ShowTitle = true;
            ControlBox = true;
            TitleHeight = 42;
            Text = "选择忽略进程";
            ClientSize = new Size(760, 600);
            MinimumSize = new Size(660, 500);
            BackColor = Color.FromArgb(244, 246, 249);

            Controls.Clear();
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(244, 246, 249),
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            var searchBar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 0, 10)
            };
            searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 58));
            searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            searchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92));
            searchLabel.Text = "搜索";
            searchLabel.Dock = DockStyle.Fill;
            searchLabel.TextAlign = ContentAlignment.MiddleLeft;
            searchLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            searchTextBox.Dock = DockStyle.Fill;
            searchTextBox.Margin = new Padding(0, 0, 8, 0);
            refreshBtn.Dock = DockStyle.Fill;
            ConfigureDialogButton(refreshBtn);
            searchBar.Controls.Add(searchLabel, 0, 0);
            searchBar.Controls.Add(searchTextBox, 1, 0);
            searchBar.Controls.Add(refreshBtn, 2, 0);

            ConfigureDialogGrid(procListView);
            procListView.Dock = DockStyle.Fill;
            procListView.Margin = new Padding(0);
            procListView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            nameColumn.FillWeight = 30;
            pidColumn.FillWeight = 15;
            titleColumn.FillWeight = 55;

            var actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0, 10, 0, 0)
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
            foreach (var button in new[] { okBtn, cancelBtn })
            {
                ConfigureDialogButton(button);
                button.Dock = DockStyle.Fill;
                button.Margin = new Padding(0, 0, 8, 0);
            }
            ConfigurePrimaryDialogButton(okBtn);
            hintLabel.Visible = false;
            actions.Controls.Add(okBtn, 1, 0);
            actions.Controls.Add(cancelBtn, 2, 0);

            root.Controls.Add(searchBar, 0, 0);
            root.Controls.Add(procListView, 0, 1);
            root.Controls.Add(actions, 0, 2);
            Controls.Add(root);
        }

        private static void ConfigureDialogGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(232, 236, 241);
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 84, 103);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 238, 252);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(24, 77, 120);
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);
            grid.RowTemplate.Height = 34;
        }

        private static void ConfigureDialogButton(UIButton button)
        {
            button.Style = UIStyle.Custom;
            button.StyleCustomMode = true;
            button.Radius = 4;
            button.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            button.FillColor = Color.White;
            button.RectColor = Color.FromArgb(199, 207, 217);
            button.ForeColor = Color.FromArgb(49, 61, 76);
            button.FillHoverColor = Color.FromArgb(239, 246, 252);
            button.ForeHoverColor = Color.FromArgb(27, 111, 181);
        }

        private static void ConfigurePrimaryDialogButton(UIButton button)
        {
            var blue = Color.FromArgb(27, 111, 181);
            button.FillColor = blue;
            button.RectColor = blue;
            button.ForeColor = Color.White;
            button.FillHoverColor = ControlPaint.Light(blue, 0.12F);
            button.ForeHoverColor = Color.White;
        }

        private void LoadProcesses()
        {
            var self = Process.GetCurrentProcess().ProcessName;
            var items = new List<(string Name, int Pid, string Title)>();
            try
            {
                foreach (var proc in Process.GetProcesses())
                {
                    try
                    {
                        var name = proc.ProcessName;
                        if (string.IsNullOrEmpty(name) ||
                            string.Equals(name, self, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string title;
                        try { title = proc.MainWindowTitle ?? ""; }
                        catch { title = ""; }

                        items.Add((name, proc.Id, title));
                    }
                    catch
                    {
                        // 无权访问的进程（如部分系统进程）跳过
                    }
                }
            }
            catch { }

            _allProcesses = items
                .OrderBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var keyword = searchTextBox.Text.Trim();
            procListView.Rows.Clear();
            foreach (var p in _allProcesses)
            {
                if (keyword.Length > 0 &&
                    !p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) &&
                    !p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    continue;

                var idx = procListView.Rows.Add(p.Name, p.Pid.ToString(), p.Title);
                procListView.Rows[idx].Tag = p.Name;
            }
        }

        private void searchTextBox_TextChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void refreshBtn_Click(object? sender, EventArgs e)
        {
            LoadProcesses();
        }

        private void okBtn_Click(object? sender, EventArgs e)
        {
            var names = procListView.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(i => i.Tag as string)
                .Where(n => !string.IsNullOrEmpty(n))
                .Select(n => n!)
                .ToList();

            if (names.Count == 0)
            {
                UIMessageBox.ShowInfo("请先选择要添加的进程。");
                return;
            }

            SelectedProcessNames = names;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelBtn_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
