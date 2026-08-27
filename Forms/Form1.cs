using CdiskClean.Forms;
using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CdiskClean
{
    public partial class Form1 : AntdUI.Window
    {
        private readonly IDatabaseService _databaseService;
        private readonly EtwMonitorService _etwService;
        private readonly DiskMonitorService _monitorService;
        private readonly DiskSpaceService _diskSpaceService;
        private readonly FolderSizeAnalyzer _folderAnalyzer;
        private readonly NotificationService _notificationService;
        private readonly CleanupService _cleanupService;
        private readonly BindingList<FileChangeRecord> _exeChangeRecords;
        private readonly BindingList<FileChangeRecord> _records;

        private readonly object _recordsLock = new();
        private const int MaxRecords = 5000;

        /// <summary>等待批量刷入网格的变更记录（UI 线程队列，由 _recordFlushTimer 触发合并）</summary>
        private readonly List<FileChangeRecord> _pendingRecords = new();
        private System.Windows.Forms.Timer _recordFlushTimer;

        /// <summary>变更网格当前是否直接绑定 _records（false=过滤快照）</summary>
        private bool _gridBoundToRecords;

        public Form1()
        {
            InitializeComponent();

            // 记录批量刷新定时器：合并 150ms 内到达的变更记录，避免高频事件逐条刷新网格
            _recordFlushTimer = new System.Windows.Forms.Timer { Interval = 150 };
            _recordFlushTimer.Tick += (_, _) => FlushPendingRecords();

            // 初始化 ETW 监控
            _etwService = new EtwMonitorService();

            // 初始化数据库
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CdiskClean.db");
            _databaseService = new SqliteDatabaseService(dbPath);
            _databaseService.Initialize();

            // 清理服务（清理期间监控联动过滤自身动作）
            _cleanupService = new CleanupService(_databaseService);

            // 从数据库加载监视目录（空则用默认列表）
            var savedDirs = _databaseService.GetWatchDirectories();
            _monitorService = new DiskMonitorService(_etwService, _cleanupService, _databaseService);
            if (savedDirs.Count > 0)
                _monitorService.LoadDirectories(savedDirs);
            else
            {
                _monitorService.LoadDefaults();
                foreach (var dir in _monitorService.WatchDirectories)
                    _databaseService.SaveWatchDirectory(dir);
            }

            // 从数据库加载忽略进程（空则用默认列表），并同步进 ETW 黑名单
            var savedProcs = _databaseService.GetIgnoreProcessRecords();
            if (savedProcs.Count > 0)
                _monitorService.LoadIgnoreProcesses(savedProcs);
            else
            {
                var defaultProcesses = IgnoreProcessRecord.GetDefaultRecords();
                _monitorService.LoadIgnoreProcesses(defaultProcesses);
                foreach (var process in defaultProcesses)
                    _databaseService.SaveIgnoreProcessRecord(process);
            }

            // 初始化磁盘空间服务和文件夹分析器
            _diskSpaceService = new DiskSpaceService();
            _folderAnalyzer = new FolderSizeAnalyzer();

            // 初始化右下角提醒服务（与统计按钮相互独立）
            _notificationService = new NotificationService();
            _notificationService.NotificationTriggered += OnNotificationTriggered;

            // 设置数据绑定（关闭自动生成列，使用设计器定义的手动列）
            _records = new BindingList<FileChangeRecord>();
            //changesDataGrid.AutoGenerateColumns = false;
            //changesDataGrid.DataSource = _records;
            _gridBoundToRecords = true;
            //changesDataGrid.CellFormatting += changesDataGrid_CellFormatting;

            typeFilterCombo.SelectedIndex = 0;

            // 订阅监视服务事件
            _monitorService.FileChanged += OnFileChanged;
            _monitorService.MonitorError += OnMonitorError;

            // 初始化监视目录列表视图
            SetupDirListView();
            PopulateDirListView();
            SetupDirContextMenu();

            // 初始化忽略进程列表视图
            SetupProcessListView();
            PopulateProcessListView();
            SetupProcessContextMenu();

            // 初始化磁盘清理页
            SetupCleanPage();

            // 统一外观：设计器布局 + 原生控件样式（AntdUI 控件样式由自身 Type 管理）
            UiTheme.Apply(this);

            // 运行时工作区初始化：设置初始页面与子视图（须在 Apply 之后，避免选中态颜色被覆盖）
            ShowWorkspacePage(DashboardPageId);
            ShowRulesView(true);
            ShowRecordView("notifications");
            RefreshWorkspaceStatus();

            ConfigureTableColumns();
        }



        // ==================== 窗体加载 ====================

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
            timer1.Start();
            diskRefreshTimer.Start();
            RefreshDiskInfo();

            BindActivityCenter(_records);
        }

        // ==================== 监视目录列表 ====================

        private void SetupDirListView()
        {
            int totalWidth = watcherDirListView.Width;

            watcherDirListView.View = View.Details;
            watcherDirListView.FullRowSelect = true;
            watcherDirListView.MultiSelect = false;
            watcherDirListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            // 设置内部列宽度为总宽度的比例
            watcherDirListView.Columns.Add("目录路径", (int)(totalWidth * 0.70));
            watcherDirListView.Columns.Add("状态", (int)(totalWidth * 0.15));
            watcherDirListView.Columns.Add("子目录", (int)(totalWidth * 0.15));

            // 开启双缓冲，减少闪烁
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, watcherDirListView, new object[] { true });
        }

        #region 监视目录列表 相关操作
        private void dirAddButton_Click(object? sender, EventArgs e)
        {

            DialogResult result = ImportFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedPath = ImportFolderDialog.SelectedPath;
                var newDir = new WatchingDirectory(selectedPath, true);
                try
                {
                    _databaseService.SaveWatchDirectory(newDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存监测目录失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dir = _monitorService.AddDirectoryToEtwArr(selectedPath, true);
                _monitorService.SetDirectoryStatus(dir.Path, RecordStatusEnum.USING);
                PopulateDirListView();
            }
        }

        private void PopulateDirListView()
        {
            watcherDirListView.Items.Clear();

            _monitorService.WatchDirectories.ForEach(addWatchingToListView);
            RefreshDashboardMetrics();
        }
        private void addWatchingToListView(WatchingDirectory dir)
        {
            // 根据路径 判重
            if (watcherDirListView.Items.Cast<ListViewItem>()
                .Any(item => item.Text == dir.Path))
                return;

            var item = new ListViewItem(dir.Path);
            item.SubItems.Add(EnumHelper.FormatStatus(dir.Status));
            item.SubItems.Add(dir.IncludeSubdirs ? "是" : "否");
            item.Tag = dir;


            StyleHelper.ApplyRecordStatusStyle(item, dir.Status);
            watcherDirListView.Items.Add(item);
        }

        private void watcherDirListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // 仅保留选中状态（原"选中目录为"提示框已随旧界面移除）
        }

        private void watcherDirListView_Resize(object sender, EventArgs e)
        {
            int totalWidth = watcherDirListView.Width;
            watcherDirListView.Columns[0].Width = (int)(totalWidth * 0.70);
            watcherDirListView.Columns[1].Width = (int)(totalWidth * 0.15);
            watcherDirListView.Columns[2].Width = (int)(totalWidth * 0.15);
        }


        private void SetupDirContextMenu()
        {
            watcherDirListView.MouseClick += watcherDirListView_MouseClick;
        }
        /// <summary>
        /// 点击之后 根据目标显示不同的菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watcherDirListView_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var item = watcherDirListView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not WatchingDirectory dir) return;
            if (dir.Status == RecordStatusEnum.DELETED) return;

            BuildStatusContextMenu(dir.Status, status => ChangeDirStatus(dir, status))
                .Show(watcherDirListView, e.Location);
        }

        private void ChangeDirStatus(WatchingDirectory dir, RecordStatusEnum newStatus)
        {
            try
            {
                if (newStatus == RecordStatusEnum.DELETED)
                {
                    _databaseService.DeleteWatchDirectory(dir.Path);
                }
                else
                {
                    _databaseService.SaveWatchDirectory(new WatchingDirectory(dir.Path, dir.IncludeSubdirs)
                    {
                        Status = newStatus
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新监测目录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _monitorService.SetDirectoryStatus(dir.Path, newStatus);

            PopulateDirListView();
        }



        private void betterDirAddButton_Click(object? sender, EventArgs e)
        {
            using var form = new BetterDirAddForm();
            if (form.ShowDialog() != DialogResult.OK) return;

            var addedCount = 0;
            foreach (var path in form.SelectedPaths)
            {
                // 已在监视列表中（非删除状态）的路径跳过
                if (_monitorService.WatchDirectories.Any(d =>
                        string.Equals(d.Path, path, StringComparison.OrdinalIgnoreCase) &&
                        d.Status != RecordStatusEnum.DELETED))
                    continue;

                try
                {
                    _databaseService.SaveWatchDirectory(new WatchingDirectory(path, true));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存监测目录失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                var dir = _monitorService.AddDirectoryToEtwArr(path, true);
                _monitorService.SetDirectoryStatus(dir.Path, RecordStatusEnum.USING);
                addedCount++;
            }

            PopulateDirListView();
            if (addedCount == 0)
            {
                MessageBox.Show("所选路径已在监测列表中。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void betterProcessAddButton_Click(object? sender, EventArgs e)
        {
            using var form = new ProcessPickForm();
            if (form.ShowDialog() != DialogResult.OK) return;

            foreach (var name in form.SelectedProcessNames)
                AddIgnoreProcessInternal(name);
        }

        #endregion


        #region 忽略进程列表 相关操作
        private void SetupProcessListView()
        {
            int totalWidth = ignoreProcessListView.Width;

            ignoreProcessListView.View = View.Details;
            ignoreProcessListView.FullRowSelect = true;
            ignoreProcessListView.MultiSelect = false;
            ignoreProcessListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            // 设置内部列宽度为总宽度的比例
            ignoreProcessListView.Columns.Add("进程名称", (int)(totalWidth * 0.80));
            ignoreProcessListView.Columns.Add("状态", (int)(totalWidth * 0.20));


            // 开启双缓冲，减少闪烁
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, ignoreProcessListView, new object[] { true });
        }

        private void PopulateProcessListView()
        {
            ignoreProcessListView.Items.Clear();
            foreach (var proc in _monitorService.IgnoreProcessRecords)
            {
                var item = new ListViewItem(proc.ProcessName);
                item.SubItems.Add(EnumHelper.FormatStatus(proc.Status));
                item.Tag = proc;

                StyleHelper.ApplyRecordStatusStyle(item, proc.Status);
                ignoreProcessListView.Items.Add(item);
            }
            RefreshDashboardMetrics();
        }

        private void ignoreProcessView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // 仅保留选中状态（原"选中进程为"提示框已随旧界面移除）
        }

        private void ignoreProcessView_Resize(object sender, EventArgs e)
        {
            int totalWidth = ignoreProcessListView.Width;
            ignoreProcessListView.Columns[0].Width = (int)(totalWidth * 0.80);
            ignoreProcessListView.Columns[1].Width = (int)(totalWidth * 0.20);
        }


        // ==================== 忽略进程右键菜单 ====================

        private void SetupProcessContextMenu()
        {
            ignoreProcessListView.MouseClick += ignoreProcessView_MouseClick;
        }

        private void ignoreProcessView_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var item = ignoreProcessListView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not IgnoreProcessRecord proc) return;
            if (proc.Status == RecordStatusEnum.DELETED) return;

            BuildStatusContextMenu(proc.Status, status => ChangeProcessStatus(proc, status))
                .Show(ignoreProcessListView, e.Location);
        }

        /// <summary>按记录状态构建统一的"禁用/启用/删除"右键菜单</summary>
        private static ContextMenuStrip BuildStatusContextMenu(
            RecordStatusEnum status,
            Action<RecordStatusEnum> changeStatus)
        {
            var menu = new ContextMenuStrip();

            if (status == RecordStatusEnum.USING)
                menu.Items.Add("禁用监测").Click += (_, _) => changeStatus(RecordStatusEnum.FORBIDDEN);
            else if (status == RecordStatusEnum.FORBIDDEN)
                menu.Items.Add("启用监测").Click += (_, _) => changeStatus(RecordStatusEnum.USING);

            menu.Items.Add("从列表删除").Click += (_, _) => changeStatus(RecordStatusEnum.DELETED);
            return menu;
        }

        private void ChangeProcessStatus(IgnoreProcessRecord proc, RecordStatusEnum newStatus)
        {
            try
            {
                if (newStatus == RecordStatusEnum.DELETED)
                {
                    _databaseService.DeleteIgnoreProcessRecord(proc.ProcessName);
                }
                else
                {
                    _databaseService.SaveIgnoreProcessRecord(new IgnoreProcessRecord(proc.ProcessName)
                    {
                        Status = newStatus
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新忽略进程失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 数据库成功后再同步内存与 ETW 黑名单，避免两处状态不一致。
            _monitorService.SetIgnoreProcessStatus(proc.ProcessName, newStatus);

            PopulateProcessListView();
        }

        private void AddIgnoreProcessInternal(string processName)
        {
            processName = Path.GetFileNameWithoutExtension(processName.Trim());
            if (string.IsNullOrWhiteSpace(processName)) return;

            var existing = _monitorService.IgnoreProcessRecords.FirstOrDefault(r =>
                string.Equals(r.ProcessName, processName, StringComparison.OrdinalIgnoreCase));

            if (existing != null && existing.Status != RecordStatusEnum.DELETED)
            {
                MessageBox.Show($"进程「{processName}」已在忽略列表中。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SelectProcessInListView(processName);
                return;
            }

            var record = new IgnoreProcessRecord(processName);
            try
            {
                _databaseService.SaveIgnoreProcessRecord(record);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存忽略进程失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (existing != null)
            {
                _monitorService.SetIgnoreProcessStatus(processName, RecordStatusEnum.USING);
            }
            else
            {
                _monitorService.AddIgnoreProcess(processName);
            }

            PopulateProcessListView();
            SelectProcessInListView(processName);
        }

        private void SelectProcessInListView(string processName)
        {
            foreach (ListViewItem item in ignoreProcessListView.Items)
            {
                if (string.Equals(item.Text, processName, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    ignoreProcessListView.EnsureVisible(item.Index);
                    break;
                }
            }
        }

        #endregion


        #region 磁盘概览

        private void RefreshDiskInfo()
        {
            try
            {
                var info = _diskSpaceService.GetDriveInfo("C:");
                UpdateDiskUI(info);
            }
            catch (Exception ex)
            {
                // 定时刷新失败仅记录日志，避免每 30 秒弹窗骚扰
                Debug.WriteLine($"获取磁盘信息失败: {ex.Message}");
            }
        }

        private void UpdateDiskUI(DriveInfoModel info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateDiskUI(info));
                return;
            }

            UpdateWorkspaceDiskStatus(info);
        }

        private void diskRefreshTimer_Tick(object? sender, EventArgs e)
        {
            RefreshDiskInfo();
        }

        #endregion



        #region 文件夹分析
        private void selectDirBtn_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "选择要分析的文件夹",
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedPathTextBox.Text = dialog.SelectedPath;
            }
        }

        private async void scanBtn_Click(object? sender, EventArgs e)
        {
            var path = selectedPathTextBox.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择要分析的目录。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Directory.Exists(path))
            {
                MessageBox.Show("所选目录不存在，请重新选择。", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            scanBtn.Enabled = false;
            selectDirBtn.Enabled = false;
            stopBtn.Enabled = true;
            folderTreeView.Nodes.Clear();
            scanProgressBar.Style = ProgressBarStyle.Marquee;

            try
            {
                var cts = new CancellationTokenSource();
                stopBtn.Tag = cts;

                var result = await _folderAnalyzer.ScanFolderAsync(path, cts.Token);

                PopulateTreeView(result);
            }
            catch (OperationCanceledException)
            {
                scanProgressBar.Style = ProgressBarStyle.Blocks;
            }
            catch (Exception ex)
            {
                BeginInvoke(() =>
                    MessageBox.Show($"扫描失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
            finally
            {
                scanProgressBar.Style = ProgressBarStyle.Blocks;
                scanBtn.Enabled = true;
                selectDirBtn.Enabled = true;
                stopBtn.Enabled = false;
                stopBtn.Tag = null;
            }
        }

        private void stopBtn_Click(object? sender, EventArgs e)
        {
            // 扫描协程持有的 CancellationTokenSource 与 FolderSizeAnalyzer 内部为同一链接 token，取消一次即可
            if (stopBtn.Tag is CancellationTokenSource cts)
            {
                cts.Cancel();
            }
        }

        private void PopulateTreeView(FolderSizeInfo info)
        {
            folderTreeView.Nodes.Clear();
            var rootNode = CreateTreeNode(info);
            folderTreeView.Nodes.Add(rootNode);
            rootNode.Expand();
        }

        private static TreeNode CreateTreeNode(FolderSizeInfo info)
        {
            var displayText = $"{info.Name}  [{FormatHelper.FormatBytes(info.SizeBytes)}, {info.FileCount} 个文件]";
            var node = new TreeNode(displayText) { Tag = info };

            foreach (var sub in info.SubFolders.OrderByDescending(s => s.SizeBytes))
            {
                node.Nodes.Add(CreateTreeNode(sub));
            }

            return node;
        }

        #endregion

        #region 磁盘清理
        // ==================== 磁盘清理 ====================

        private CancellationTokenSource? _cleanScanCts;
        private CancellationTokenSource? _cleanExecCts;
        private bool _treeUpdating;

        /// <summary>树节点数量上限，超过则仅显示目录节点（文件通过勾选目录整体清理）</summary>
        private const int MaxCleanTreeNodes = 50000;

        // 树勾选状态图标索引（StateImageList）：0=未选 1=全选 2=部分
        private const int StateUnchecked = 0;
        private const int StateChecked = 1;
        private const int StatePartial = 2;

        private void SetupCleanPage()
        {
            _cleanupMethodRadios = new[]
            {
                (cleanRecycleRadio, CleanupMethod.RecycleBin),
                (cleanPermanentRadio, CleanupMethod.PermanentDelete),
                (cleanMoveRadio, CleanupMethod.Move),
                (cleanCompressRadio, CleanupMethod.Compress),
                (cleanMklinkRadio, CleanupMethod.Mklink)
            };
            SetupFrequentListView();
            SetupTreeStateImages();
            LayoutCleanupMethodPanel(cleanupMethodPanel);
            UpdateTargetBoxState();
            RefreshFrequentPaths();
            RefreshCleanHistory();
        }

        /// <summary>设置树勾选三态图标（0=未选 1=全选 2=部分），替换默认复选框</summary>
        private void SetupTreeStateImages()
        {
            var images = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit,
                TransparentColor = Color.Transparent
            };
            images.Images.Add(CreateCheckStateImage(false, false));
            images.Images.Add(CreateCheckStateImage(true, false));
            images.Images.Add(CreateCheckStateImage(false, true));
            cleanTreeView.StateImageList = images;
        }

        private static Image CreateCheckStateImage(bool checkedState, bool indeterminate)
        {
            var bmp = new Bitmap(16, 16);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(1, 1, 13, 13);
            if (indeterminate)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(225, 238, 255)), rect);
                g.DrawRectangle(new Pen(Color.DodgerBlue), rect);
                g.FillRectangle(new SolidBrush(Color.DodgerBlue), new Rectangle(4, 7, 7, 2));
            }
            else
            {
                g.DrawRectangle(new Pen(Color.FromArgb(120, 120, 120)), rect);
                if (checkedState)
                {
                    using var pen = new Pen(Color.DodgerBlue, 2f)
                    {
                        StartCap = LineCap.Round,
                        EndCap = LineCap.Round
                    };
                    g.DrawLines(pen, new[]
                    {
                        new PointF(3, 8), new PointF(6.5f, 11f), new PointF(12.5f, 4f)
                    });
                }
            }
            return bmp;
        }

        private void SetupFrequentListView()
        {
            int totalWidth = frequentPathListView.Width;

            frequentPathListView.View = View.Details;
            frequentPathListView.FullRowSelect = true;
            frequentPathListView.MultiSelect = false;
            frequentPathListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            frequentPathListView.Columns.Add("目录路径", (int)(totalWidth * 0.70));
            frequentPathListView.Columns.Add("变更次数", (int)(totalWidth * 0.30));

            // 开启双缓冲，减少闪烁
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, frequentPathListView, new object[] { true });
        }

        /// <summary>从变更记录中统计高频修改目录，展示在左侧参考列表（历史记录读取在后台执行，避免卡 UI）</summary>
        private async void RefreshFrequentPaths()
        {
            List<FileChangeRecord> snapshot;
            lock (_recordsLock)
            {
                snapshot = _records.ToList();
            }

            List<FileChangeRecord> dbRecords;
            try
            {
                dbRecords = await Task.Run(() => _databaseService.GetChangeRecords(5000));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取历史变更记录失败: {ex.Message}");
                dbRecords = new List<FileChangeRecord>();
            }

            var seen = new HashSet<string>(
                snapshot.Select(GetChangeRecordKey),
                StringComparer.OrdinalIgnoreCase);
            foreach (var record in dbRecords)
            {
                if (seen.Add(GetChangeRecordKey(record)))
                    snapshot.Add(record);
            }

            var paths = CleanupService.GetFrequentPaths(snapshot, 30);
            ApplyFrequentPaths(paths);
        }

        private void ApplyFrequentPaths(List<FrequentPathInfo> paths)
        {
            if (IsDisposed) return;
            // 句柄尚未创建（构造期异步完成时）挂到 Load 后执行；非 UI 线程则回传 UI 线程
            if (!IsHandleCreated)
            {
                Load += (_, _) => ApplyFrequentPaths(paths);
                return;
            }
            if (InvokeRequired)
            {
                BeginInvoke(() => ApplyFrequentPaths(paths));
                return;
            }

            frequentPathListView.BeginUpdate();
            frequentPathListView.Items.Clear();
            if (paths.Count == 0)
            {
                frequentPathListView.Items.Add(new ListViewItem("暂无变更记录"));
            }
            else
            {
                foreach (var p in paths)
                {
                    var item = new ListViewItem(p.Path);
                    item.SubItems.Add($"{p.ChangeCount}次");
                    item.Tag = p;
                    frequentPathListView.Items.Add(item);
                }
            }
            frequentPathListView.EndUpdate();
        }

        private void cleanRefreshFrequentBtn_Click(object? sender, EventArgs e)
        {
            RefreshFrequentPaths();
        }

        private void frequentPathListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item?.Tag is FrequentPathInfo info)
                cleanPathTextBox.Text = info.Path;
        }

        private void frequentPathListView_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            var item = frequentPathListView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not FrequentPathInfo info) return;

            cleanPathTextBox.Text = info.Path;
            _ = TryScanCurrentPathAsync();
        }

        private void cleanSelectDirBtn_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "选择要清理的目录",
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                cleanPathTextBox.Text = dialog.SelectedPath;
        }

        private async void cleanScanBtn_Click(object? sender, EventArgs e)
        {
            // 扫描进行中再次点击 = 停止扫描
            if (_cleanScanCts != null)
            {
                _cleanScanCts.Cancel();
                return;
            }
            await TryScanCurrentPathAsync();
        }

        private async Task TryScanCurrentPathAsync()
        {
            var path = cleanPathTextBox.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择要清理的目录。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Directory.Exists(path))
            {
                MessageBox.Show("所选目录不存在，请重新选择。", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _cleanScanCts = new CancellationTokenSource();
            var cts = _cleanScanCts;

            cleanScanBtn.Text = "停止扫描";
            cleanScanProgressBar.Style = ProgressBarStyle.Marquee;
            cleanStatusLabel.Text = "正在扫描...";
            cleanTreeView.Nodes.Clear();

            try
            {
                var entries = await _cleanupService.ScanDirectoryAsync(path, cts.Token);
                if (cts.IsCancellationRequested) return;

                // 根目录条目（首个目录）已递归汇总全部子项大小，直接取它避免重复累加
                var totalSize = entries.FirstOrDefault(e => e.IsDirectory)?.SizeBytes ?? 0;
                var fileCount = entries.Count(e => !e.IsDirectory);

                // 节点过多时仅构建目录树，避免数十万节点卡死 UI
                var dirOnly = entries.Count > MaxCleanTreeNodes;
                cleanStatusLabel.Text = dirOnly
                    ? $"正在构建目录树（文件过多：{fileCount} 个文件，仅显示目录以保持流畅）..."
                    : "正在构建目录树...";

                await BuildCleanTreeAsync(entries, path, totalSize, dirOnly, cts.Token);

                cleanStatusLabel.Text = dirOnly
                    ? $"扫描完成：{fileCount} 个文件，共 {FormatHelper.FormatBytes(totalSize)}（文件过多仅显示目录，可勾选目录整体清理）"
                    : $"扫描完成：{fileCount} 个文件，共 {FormatHelper.FormatBytes(totalSize)}";
            }
            catch (OperationCanceledException)
            {
                cleanStatusLabel.Text = "扫描已取消";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"扫描失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cleanStatusLabel.Text = "扫描失败";
            }
            finally
            {
                if (_cleanScanCts == cts) _cleanScanCts = null;
                cleanScanBtn.Text = "开始扫描";
                cleanScanProgressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        /// <summary>后台构建节点树后一次性挂载，避免大量节点在 UI 线程创建导致卡死</summary>
        private async Task BuildCleanTreeAsync(
            List<CleanupFileEntry> entries,
            string rootPath,
            long totalSize,
            bool dirOnly,
            CancellationToken ct)
        {
            var rootFull = rootPath.TrimEnd('\\');
            var rootNode = await Task.Run(() =>
                BuildCleanTreeNodes(entries, rootFull, totalSize, dirOnly), ct);

            cleanTreeView.BeginUpdate();
            cleanTreeView.Nodes.Clear();
            cleanTreeView.Nodes.Add(rootNode);
            rootNode.Expand();
            cleanTreeView.EndUpdate();
            UpdateCleanupSelectionSummary();
        }

        /// <summary>
        /// 在后台线程构建节点树（TreeNode 无句柄，可跨线程构建父子关系，挂载后再由 UI 线程渲染）。
        /// 跳过扫描根目录自身（entries 首项即根），避免树中出现重复根节点。
        /// </summary>
        private static TreeNode BuildCleanTreeNodes(
            List<CleanupFileEntry> entries,
            string rootFull,
            long totalSize,
            bool dirOnly)
        {
            var rootName = Path.GetFileName(rootFull);
            var rootNode = new TreeNode($"{rootName}  [{FormatHelper.FormatBytes(totalSize)}]")
            {
                Tag = null,
                StateImageIndex = -1
            };

            // 目录节点（扫描顺序保证父目录先于子目录）
            var dirNodes = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase)
            {
                [rootFull] = rootNode
            };

            foreach (var entry in entries.Where(e => e.IsDirectory))
            {
                var entryFull = entry.FullPath.TrimEnd('\\');
                if (string.Equals(entryFull, rootFull, StringComparison.OrdinalIgnoreCase))
                    continue;

                var node = new TreeNode($"{entry.Name}  [{FormatHelper.FormatBytes(entry.SizeBytes)}]")
                {
                    Tag = entry,
                    StateImageIndex = 0
                };
                dirNodes[entryFull] = node;

                var parentDir = Path.GetDirectoryName(entryFull) ?? "";
                if (dirNodes.TryGetValue(parentDir.TrimEnd('\\'), out var parent))
                    parent.Nodes.Add(node);
                else
                    rootNode.Nodes.Add(node);
            }

            if (!dirOnly)
            {
                // 文件节点
                foreach (var entry in entries.Where(e => !e.IsDirectory))
                {
                    var node = new TreeNode($"{entry.Name}  [{FormatHelper.FormatBytes(entry.SizeBytes)}, {entry.LastWriteTime:yyyy-MM-dd HH:mm}]")
                    {
                        Tag = entry,
                        StateImageIndex = 0
                    };

                    var parentDir = Path.GetDirectoryName(entry.FullPath) ?? "";
                    if (dirNodes.TryGetValue(parentDir.TrimEnd('\\'), out var parent))
                        parent.Nodes.Add(node);
                    else
                        rootNode.Nodes.Add(node);
                }
            }

            SortCleanTreeNodes(rootNode.Nodes);
            return rootNode;
        }

        /// <summary>目录在前，其余按大小降序排列</summary>
        private static void SortCleanTreeNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
                SortCleanTreeNodes(node.Nodes);

            if (nodes.Count < 2) return;

            var list = nodes.Cast<TreeNode>().ToList();
            // 目录在前，其余按大小降序排列
            list.Sort((a, b) =>
            {
                // 目录在前
                bool aDir = a.Tag is CleanupFileEntry ae && ae.IsDirectory;
                bool bDir = b.Tag is CleanupFileEntry be && be.IsDirectory;
                if (aDir != bDir) return aDir ? -1 : 1;

                // 其余按大小降序排列
                long aSize = a.Tag is CleanupFileEntry ae2 ? ae2.SizeBytes : 0;
                long bSize = b.Tag is CleanupFileEntry be2 ? be2.SizeBytes : 0;
                return bSize.CompareTo(aSize);
            });

            for (int i = 0; i < list.Count; i++)
            {
                if (nodes[i] == list[i]) continue;
                nodes.Remove(list[i]);
                nodes.Insert(i, list[i]);
            }
        }

        /// <summary>根节点（Tag=null）不可勾选，防止误删基础路径本身</summary>
        private void cleanTreeView_BeforeCheck(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node?.Tag == null)
                e.Cancel = true;
        }

        /// <summary>勾选/取消勾选时级联应用到子节点</summary>
        private void cleanTreeView_AfterCheck(object? sender, TreeViewEventArgs e)
        {
            if (_treeUpdating) return;
            if (e.Node == null) return;

            _treeUpdating = true;
            try
            {
                SetNodeCheckedRecursive(e.Node, e.Node.Checked);
                UpdateParentState(e.Node);
            }
            finally
            {
                _treeUpdating = false;
            }
            UpdateCleanupSelectionSummary();
        }

        private static void SetNodeCheckedRecursive(TreeNode node, bool check)
        {
            node.Checked = check;
            node.StateImageIndex = check ? StateChecked : StateUnchecked;
            foreach (TreeNode child in node.Nodes)
                SetNodeCheckedRecursive(child, check);
        }

        /// <summary>按子节点勾选情况刷新父链图标：全选=1、全不选=0、混合=2</summary>
        private static void UpdateParentState(TreeNode node)
        {
            var parent = node.Parent;
            while (parent?.Parent != null)
            {
                bool hasChecked = false;
                bool hasUnchecked = false;
                foreach (TreeNode child in parent.Nodes)
                {
                    if (child.StateImageIndex == StatePartial)
                    {
                        hasChecked = true;
                        hasUnchecked = true;
                    }
                    else if (child.Checked)
                    {
                        hasChecked = true;
                    }
                    else
                    {
                        hasUnchecked = true;
                    }
                }

                var state = hasChecked && hasUnchecked
                    ? StatePartial
                    : hasChecked ? StateChecked : StateUnchecked;
                parent.Checked = state != StateUnchecked;
                parent.StateImageIndex = state;
                parent = parent.Parent;
            }
        }

        private void cleanSelectAllBtn_Click(object? sender, EventArgs e)
        {
            SetAllCleanNodesChecked(true);
        }

        private void cleanSelectNoneBtn_Click(object? sender, EventArgs e)
        {
            SetAllCleanNodesChecked(false);
        }

        private void SetAllCleanNodesChecked(bool check)
        {
            if (cleanTreeView.Nodes.Count == 0) return;

            _treeUpdating = true;
            try
            {
                foreach (TreeNode child in cleanTreeView.Nodes[0].Nodes)
                    SetNodeCheckedRecursive(child, check);
            }
            finally
            {
                _treeUpdating = false;
            }
            UpdateCleanupSelectionSummary();
        }

        private void cleanTargetSelectBtn_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "选择清理操作的目标目录",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
                cleanTargetTextBox.Text = dialog.SelectedPath;
        }

        private void cleanMethodRadio_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateTargetBoxState();
        }

        /// <summary>仅需要目标目录的清理方式才启用目标目录输入</summary>
        private void UpdateTargetBoxState()
        {
            var needTarget = CleanupService.RequiresTarget(GetSelectedMethod());
            cleanTargetLabel.Enabled = needTarget;
            cleanTargetTextBox.Enabled = needTarget;
            cleanTargetSelectBtn.Enabled = needTarget;
        }

        private CleanupMethod GetSelectedMethod()
        {
            foreach (var (radio, method) in _cleanupMethodRadios)
            {
                if (radio.Checked) return method;
            }
            return CleanupMethod.RecycleBin;
        }

        private List<CleanupFileEntry> GetCheckedEntries()
        {
            var list = new List<CleanupFileEntry>();
            if (cleanTreeView.Nodes.Count > 0)
                CollectCheckedCleanNodes(cleanTreeView.Nodes[0], list);
            return list;
        }

        private static void CollectCheckedCleanNodes(TreeNode node, List<CleanupFileEntry> list)
        {
            // 勾选的目录仅在"全选"态下整体清理；部分勾选（中间态）则继续下钻子项
            if (node.Checked && node.StateImageIndex != StatePartial && node.Tag is CleanupFileEntry entry)
            {
                list.Add(entry);
                return;
            }

            foreach (TreeNode child in node.Nodes)
                CollectCheckedCleanNodes(child, list);
        }

        private async void cleanBtn_Click(object? sender, EventArgs e)
        {
            // 清理进行中再次点击 = 取消剩余清理项
            if (_cleanExecCts != null)
            {
                _cleanExecCts.Cancel();
                return;
            }

            var entries = GetCheckedEntries();
            if (entries.Count == 0)
            {
                MessageBox.Show("请先勾选要清理的文件。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var method = GetSelectedMethod();
            string? targetDir = null;

            if (CleanupService.RequiresTarget(method))
            {
                targetDir = cleanTargetTextBox.Text.Trim();
                if (string.IsNullOrEmpty(targetDir) || !Directory.Exists(targetDir))
                {
                    MessageBox.Show("请先选择有效的目标目录。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var basePath = cleanPathTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(basePath) && PathHelper.IsPathInside(targetDir, basePath))
                {
                    MessageBox.Show("目标目录不能位于待清理的目录内部。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var totalSize = entries.Sum(e => e.SizeBytes);
            var methodName = CleanupService.GetMethodDisplayName(method);
            var confirmText = method switch
            {
                CleanupMethod.RecycleBin =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）移入回收站吗？",
                CleanupMethod.PermanentDelete =>
                    $"确定要永久删除选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）吗？\n\n此操作不可恢复！",
                CleanupMethod.Move =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）移动到：\n{targetDir}\n\n吗？",
                CleanupMethod.Compress =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）压缩到：\n{targetDir}\n\n并删除原文件吗？",
                CleanupMethod.Mklink =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）迁移到：\n{targetDir}\n\n并在原位置创建软链接吗？",
                _ => $"确定要清理选中的 {entries.Count} 项吗？"
            };

            if (MessageBox.Show(confirmText, $"确认清理（{methodName}）",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            _cleanExecCts = new CancellationTokenSource();
            var cts = _cleanExecCts;

            cleanButton.Enabled = false;
            cleanButton.Text = "取消清理";
            cleanScanProgressBar.Style = ProgressBarStyle.Marquee;
            var progress = new Progress<string>(s => cleanStatusLabel.Text = s);

            // 按待清理文件所在盘统计释放空间（清理对象可能不在 C 盘）
            var freedDriveRoot = entries[0].FullPath.Length >= 2
                ? Path.GetPathRoot(entries[0].FullPath)
                : null;
            long freeBefore = GetFreeSpaceSafe(freedDriveRoot);
            try
            {
                var result = await _cleanupService.ExecuteAsync(entries, method, targetDir, progress, cts.Token);
                long freedDelta = Math.Max(0, GetFreeSpaceSafe(freedDriveRoot) - freeBefore);

                var summary = $"清理完成：成功 {result.Success} 项，失败 {result.Fail} 项";
                if (method is CleanupMethod.PermanentDelete or CleanupMethod.Compress)
                    summary += $"\n释放空间约 {FormatHelper.FormatBytes(freedDelta)}";
                MessageBox.Show(summary, "清理完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ShowCleanupBalloon(method, result, freedDelta, totalSize);
                RefreshDiskInfo();
                RefreshCleanHistory();
                RefreshFrequentPaths();

                // 清理后自动重新扫描，刷新剩余文件
                if (Directory.Exists(cleanPathTextBox.Text.Trim()))
                    await TryScanCurrentPathAsync();
            }
            catch (OperationCanceledException)
            {
                cleanStatusLabel.Text = "已取消，剩余清理项未执行";
                RefreshDiskInfo();
                RefreshCleanHistory();
                RefreshFrequentPaths();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_cleanExecCts == cts) _cleanExecCts = null;
                cleanButton.Enabled = true;
                cleanButton.Text = "清理选中文件";
                cleanScanProgressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private void ShowCleanupBalloon(
            CleanupMethod method,
            CleanupResult result,
            long freedDelta,
            long selectedTotalSize)
        {
            var msg = method switch
            {
                CleanupMethod.RecycleBin => $"已将 {result.Success} 项移入回收站（可在回收站恢复）",
                CleanupMethod.PermanentDelete => $"已永久删除 {result.Success} 项，释放约 {FormatHelper.FormatBytes(freedDelta)}",
                CleanupMethod.Move => $"已将 {result.Success} 项移动到目标目录",
                CleanupMethod.Compress =>
                    $"已压缩 {result.Success} 项到目标目录，选中文件总大小 {FormatHelper.FormatBytes(selectedTotalSize)}",
                CleanupMethod.Mklink => $"已将 {result.Success} 项迁移至目标目录并创建软链接",
                _ => $"清理完成，成功 {result.Success} 项"
            };
            if (result.Fail > 0) msg += $"，失败 {result.Fail} 项";

            notifyIcon1.ShowBalloonTip(3000, "清理完成", msg, ToolTipIcon.Info);
        }

        private long GetFreeSpaceSafe(string? driveRoot = null)
        {
            try
            {
                var drive = string.IsNullOrEmpty(driveRoot) ? "C:" : driveRoot;
                return _diskSpaceService.GetDriveInfo(drive).FreeSpaceBytes;
            }
            catch { return 0; }
        }

        private void RefreshCleanHistory()
        {
            var records = _databaseService.GetCleanupRecords(200);
            cleanHistoryTable.DataSource = records;
            cleanHistoryEmptyLabel.Visible = records.Count == 0;

            // 列配置统一在 ConfigureTableColumns() 中完成，此处只需绑定数据
        }

        /// <summary>
        /// 清理历史表格右键菜单：选中行后弹出（回收站记录可定位回收站 / 复制路径）。
        /// AntdUI.Table 无 CellContextMenuStripNeeded，改用 CellClick 监听右键。
        /// </summary>
        private void cleanHistoryGrid_CellClick(object? sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.Record is not CleanupRecord record)
                return;

            cleanHistoryTable.SetSelected(record);

            var menu = new ContextMenuStrip();

            // 回收站方式且清理成功的记录可定位到回收站
            if (record.Success && string.Equals(record.Method, "回收站", StringComparison.Ordinal))
            {
                var openRecycle = new ToolStripMenuItem("打开回收站定位");
                openRecycle.Click += (_, _) => OpenRecycleBin();
                menu.Items.Add(openRecycle);
            }

            var copyPath = new ToolStripMenuItem("复制路径");
            copyPath.Click += (_, _) =>
            {
                try { Clipboard.SetText(record.FullPath); }
                catch (Exception ex) { Debug.WriteLine($"复制路径失败: {ex.Message}"); }
            };
            menu.Items.Add(copyPath);

            menu.Show(Cursor.Position);
        }

        private static void OpenRecycleBin()
        {
            try
            {
                Process.Start(new ProcessStartInfo("explorer.exe")
                {
                    Arguments = "shell:RecycleBinFolder",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开回收站失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        // ==================== 时钟 ====================

        private void timer1_Tick(object? sender, EventArgs e)
        {
            workspaceClockStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region 工具方法
        private static string GetChangeRecordKey(FileChangeRecord record) =>
            $"{record.Timestamp.Ticks}|{record.ChangeType}|{record.FullPath}";

        private static string EscapeCsv(string? value)
        {
            const char quote = (char)34;
            return quote + (value ?? string.Empty).Replace(
                quote.ToString(),
                new string(quote, 2)) + quote;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        public void closeApplication()
        {
            _isExiting = true;
            _monitorService.Dispose();
            _etwService.Dispose();
            _notificationService.Dispose();
            diskRefreshTimer.Stop();
            timer1.Stop();
            Application.Exit();
        }

        #endregion




        #region 记录拖拽 [遗留项]
        // ==================== 拖拽：监视记录 → 忽略进程列表 ====================

        private bool _isGridDragging;
        private Point _gridMouseStartPos;

        // 鼠标按下：记录起点
        private void changesDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _gridMouseStartPos = e.Location;
                _isGridDragging = false;
            }
        }

        // 鼠标移动：达到拖拽阈值后启动拖拽，携带整条变更记录
        /*private void changesDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _isGridDragging) return;

            int moveRange = SystemInformation.DragSize.Width;
            var offset = Math.Abs(e.X - _gridMouseStartPos.X) + Math.Abs(e.Y - _gridMouseStartPos.Y);
            if (offset < moveRange) return;

            var hit = changesDataGrid.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;

            if (changesDataGrid.Rows[hit.RowIndex].DataBoundItem is not FileChangeRecord record) return;
            // 未解析出进程名的记录无法用于忽略进程
            if (string.IsNullOrEmpty(record.SourceProcess) || record.SourceProcess == "未知进程") return;

            _isGridDragging = true;
            changesDataGrid.DoDragDrop(record, DragDropEffects.Copy);
        }
*/
        // 鼠标松开：重置标记
        private void changesDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _isGridDragging = false;
        }

        private void ignoreProcessView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(typeof(FileChangeRecord)) == true &&
                !string.IsNullOrWhiteSpace(
                    (e.Data.GetData(typeof(FileChangeRecord)) as FileChangeRecord)?.SourceProcess))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ignoreProcessView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(typeof(FileChangeRecord)) is not FileChangeRecord record ||
                string.IsNullOrEmpty(record.SourceProcess))
                return;

            // 监视进行中不允许接收拖拽记录，给出提示
            if (_monitorService.IsRunning)
            {
                MessageBox.Show("监视进行中不能接收拖拽记录，请先点击「暂停」后再拖拽。",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddIgnoreProcessInternal(record.SourceProcess);
        }
        #endregion


        /// <summary>
        /// 添加忽略进程：去重 → 同步 ETW 黑名单 → 入库 → 刷新列表
        /// </summary>

        private void recordSearchBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { }
            //ApplyFilter();
        }

        #region 托盘
        int cycleCount = 0;
        private void notifyRotateTimer_Tick(object sender, EventArgs e)
        {
            cycleCount++;
            if (cycleCount == 1)
                notifyIcon1.Icon = Properties.Resources.leftRotate_1;
            else if (cycleCount == 2)
                notifyIcon1.Icon = Properties.Resources.leftRotate_2;
            else if (cycleCount == 3)
                notifyIcon1.Icon = Properties.Resources.leftRotate_3;
            else if (cycleCount == 4)
            {
                notifyIcon1.Icon = Properties.Resources.leftRotate_4;
                cycleCount = 0;
            }

        }

        private void startMonitorNotifyItem_Click(object sender, EventArgs e)
        {
            // 通过托盘图标启动监视时，确保主窗口显示
            pauseBtn_Click(sender, e);
        }

        private void defaultModeRadio_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (defaultModeRadio.Checked)
            {
                if (_monitorService.IsRunning)
                {
                    MessageBox.Show("需要暂停监测才能选择模式！！！");
                    defaultModeRadio.Checked = false;
                    return;
                }
                // 切换到默认模式，启用相关资源。
                _monitorService.EnableDefaultMode();

            }
            else
            {
                // 离开默认模式，关闭默认模式相关资源。
                _monitorService.DisableDefaultMode();
            }
        }

        private void exeModeRadio_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            /*
             需要 一个组件来向 ETW 获取 记录。
                FSW 不能满足


            BindingList
             */
            LogHelper.showDefaultToDoMessage("exeModeRadio_CheckedChanged");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }
        #endregion



        private void cleanTargetSelectBtn_Click(object sender, EventArgs e)
        {


        private void cleanTargetSelectBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
