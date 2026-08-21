using Sunny.UI;

namespace CdiskClean.Forms
{
    /// <summary>
    /// 高级添加监测目录：以基础目录为根，树状展示子路径，勾选后作为监测目录返回。
    /// </summary>
    public partial class BetterDirAddForm : UIForm
    {
        /// <summary>确认后返回的勾选路径列表（含基础目录本身）</summary>
        public List<string> SelectedPaths { get; private set; } = new();

        public BetterDirAddForm()
        {
            InitializeComponent();
#if DEBUG
            ApplyDebugLayout();
#endif
        }

        private void ApplyDebugLayout()
        {
            ShowTitle = true;
            ControlBox = true;
            TitleHeight = 42;
            Text = "添加监控目录";
            ClientSize = new Size(760, 620);
            MinimumSize = new Size(640, 520);
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

            var pathBar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 0, 10)
            };
            pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 82));
            pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pathBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104));
            basePathLabel.Text = "基础目录";
            basePathLabel.Dock = DockStyle.Fill;
            basePathLabel.TextAlign = ContentAlignment.MiddleLeft;
            basePathLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            basePathTextBox.Dock = DockStyle.Fill;
            basePathTextBox.Margin = new Padding(0, 0, 8, 0);
            browseBtn.Dock = DockStyle.Fill;
            ConfigureDialogButton(browseBtn);
            pathBar.Controls.Add(basePathLabel, 0, 0);
            pathBar.Controls.Add(basePathTextBox, 1, 0);
            pathBar.Controls.Add(browseBtn, 2, 0);

            dirTreeView.Dock = DockStyle.Fill;
            dirTreeView.Margin = new Padding(0);
            dirTreeView.BorderStyle = BorderStyle.None;
            dirTreeView.BackColor = Color.White;
            dirTreeView.Font = new Font("Microsoft YaHei UI", 9F);
            dirTreeView.ItemHeight = 27;

            var actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 5,
                RowCount = 1,
                Padding = new Padding(0, 10, 0, 0)
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
            foreach (var button in new[] { selectAllBtn, selectNoneBtn, okBtn, cancelBtn })
            {
                ConfigureDialogButton(button);
                button.Dock = DockStyle.Fill;
                button.Margin = new Padding(0, 0, 8, 0);
            }
            ConfigurePrimaryDialogButton(okBtn);
            hintLabel.Visible = false;
            actions.Controls.Add(selectAllBtn, 0, 0);
            actions.Controls.Add(selectNoneBtn, 1, 0);
            actions.Controls.Add(okBtn, 3, 0);
            actions.Controls.Add(cancelBtn, 4, 0);

            root.Controls.Add(pathBar, 0, 0);
            root.Controls.Add(dirTreeView, 0, 1);
            root.Controls.Add(actions, 0, 2);
            Controls.Add(root);
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

        private void browseBtn_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "选择作为基础的目录（其子路径将展示为树状勾选列表）",
                ShowNewFolderButton = false
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            basePathTextBox.Text = dialog.SelectedPath;
            LoadRoot(dialog.SelectedPath);
        }

        /// <summary>以基础目录为根构建树并展开第一层，基础目录本身默认勾选</summary>
        private void LoadRoot(string basePath)
        {
            dirTreeView.BeginUpdate();
            dirTreeView.Nodes.Clear();
            var root = CreateDirNode(basePath, showFullName: true);
            root.Checked = true;
            dirTreeView.Nodes.Add(root);
            root.Expand();
            dirTreeView.EndUpdate();
        }

        private static TreeNode CreateDirNode(string dirPath, bool showFullName = false)
        {
            var node = new TreeNode(showFullName ? dirPath : Path.GetFileName(dirPath))
            {
                Tag = dirPath
            };
            // 占位子节点（空文本、无 Tag），保证目录有展开箭头；展开时替换为实际子目录
            node.Nodes.Add(new TreeNode());
            return node;
        }

        private void dirTreeView_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is null || e.Node.Tag is not string path) return;
            var placeholder = e.Node.Nodes.Count == 1 ? e.Node.Nodes[0] : null;
            if (placeholder != null &&
                string.IsNullOrEmpty(placeholder.Text) &&
                placeholder.Tag == null)
            {
                LoadChildNodes(e.Node, path);
            }
        }

        private static void LoadChildNodes(TreeNode node, string path)
        {
            node.Nodes.Clear();
            try
            {
                foreach (var sub in Directory.EnumerateDirectories(path))
                {
                    if (IsReparsePoint(sub)) continue;
                    node.Nodes.Add(CreateDirNode(sub));
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
        }

        private static bool IsReparsePoint(string path)
        {
            try { return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint); }
            catch { return true; }
        }

        private void selectAllBtn_Click(object? sender, EventArgs e)
        {
            SetAllChecked(true);
        }

        private void selectNoneBtn_Click(object? sender, EventArgs e)
        {
            SetAllChecked(false);
        }

        private void SetAllChecked(bool check)
        {
            dirTreeView.BeginUpdate();
            foreach (TreeNode node in dirTreeView.Nodes)
                SetNodeChecked(node, check);
            dirTreeView.EndUpdate();
        }

        private static void SetNodeChecked(TreeNode node, bool check)
        {
            node.Checked = check;
            foreach (TreeNode child in node.Nodes)
                SetNodeChecked(child, check);
        }

        private void okBtn_Click(object? sender, EventArgs e)
        {
            var paths = new List<string>();
            CollectCheckedPaths(dirTreeView.Nodes, paths);
            if (paths.Count == 0)
            {
                UIMessageBox.ShowInfo("请至少勾选一个目录路径。");
                return;
            }

            SelectedPaths = paths;
            DialogResult = DialogResult.OK;
            Close();
        }

        private static void CollectCheckedPaths(TreeNodeCollection nodes, List<string> paths)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && node.Tag is string path)
                    paths.Add(path);
                CollectCheckedPaths(node.Nodes, paths);
            }
        }

        private void cancelBtn_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
