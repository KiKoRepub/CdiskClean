using System.Diagnostics;

namespace CdiskClean.Forms
{
    /// <summary>
    /// 高级添加忽略进程：列出当前系统运行的进程，支持搜索与多选。
    /// </summary>
    public partial class ProcessPickForm : Form
    {
        private List<(string Name, int Pid, string Title)> _allProcesses = new();

        /// <summary>确认后返回的进程名列表</summary>
        public List<string> SelectedProcessNames { get; private set; } = new();

        public ProcessPickForm()
        {
            InitializeComponent();
            LoadProcesses();
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
            procListView.BeginUpdate();
            procListView.Items.Clear();
            foreach (var p in _allProcesses)
            {
                if (keyword.Length > 0 &&
                    !p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) &&
                    !p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    continue;

                var item = new ListViewItem(p.Name);
                item.SubItems.Add(p.Pid.ToString());
                item.SubItems.Add(p.Title);
                item.Tag = p.Name;
                procListView.Items.Add(item);
            }
            procListView.EndUpdate();
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
            var names = procListView.SelectedItems
                .Cast<ListViewItem>()
                .Select(i => i.Tag as string)
                .Where(n => !string.IsNullOrEmpty(n))
                .Select(n => n!)
                .ToList();

            if (names.Count == 0)
            {
                MessageBox.Show("请先选择要添加的进程。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
