namespace CdiskClean.Forms
{
    /// <summary>
    /// 高级添加监测目录：以基础目录为根，树状展示子路径，勾选后作为监测目录返回。
    /// </summary>
    public partial class BetterDirAddForm : Form
    {
        /// <summary>确认后返回的勾选路径列表（含基础目录本身）</summary>
        public List<string> SelectedPaths { get; private set; } = new();

        public BetterDirAddForm()
        {
            InitializeComponent();
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
                MessageBox.Show("请至少勾选一个目录路径。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
