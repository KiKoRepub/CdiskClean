using AntdUI;
using CdiskClean.Forms;
using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.cleanUp;
using CdiskClean.Models.rules;
using CdiskClean.Services.database;
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
        private readonly FolderPermissionAnalyzer _folderPermissionAnalyzer;
        private readonly NotificationService _notificationService;
        private readonly CleanupService _cleanupService;
        private readonly BindingList<FileChangeRecord> _exeChangeRecords;
        private readonly BindingList<FileChangeRecord> _records;
        private CancellationTokenSource? _analyzerScanCts;
        private int _analyzerScanVersion;

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
            InitializeAnalyzerDetails();

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

            _monitorService.LoadWatchingApplications(_databaseService.GetWatchingApplications());

            // 初始化磁盘空间服务和文件夹分析器
            _diskSpaceService = new DiskSpaceService();
            _folderAnalyzer = new FolderSizeAnalyzer();
            _folderPermissionAnalyzer = new FolderPermissionAnalyzer();

            // 初始化右下角提醒服务（与统计按钮相互独立）
            _notificationService = new NotificationService();
            _notificationService.NotificationTriggered += OnNotificationTriggered;

            // 设置数据绑定（关闭自动生成列，使用设计器定义的手动列）
            _records = new BindingList<FileChangeRecord>();
            _exeChangeRecords = new BindingList<FileChangeRecord>();
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

            input1.TextChanged += rulesExeProcInput_TextChanged;
            rulesExeProcAddButton.Click += rulesExeProcAddButton_Click;
            button1.Click += rulesExeProcSelectButton_Click;
            rulesExeProcViewTable.CellClick += rulesExeProcViewTable_CellClick;


            ConfigureTableColumns();
            BindRulesTableCenter();
            // 初始化磁盘清理页
            SetupCleanPage();

            // 统一外观：设计器布局 + 原生控件样式（AntdUI 控件样式由自身 Type 管理）
            UiTheme.Apply(this);

            // 运行时工作区初始化：设置初始页面与子视图（须在 Apply 之后，避免选中态颜色被覆盖）
            ShowWorkspacePage(DashboardPageId);
            ShowRulesView(true);
            ShowRecordView("notifications");
            RefreshWorkspaceStatus();

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
        private static System.Windows.Forms.ContextMenuStrip BuildStatusContextMenu(
            RecordStatusEnum status,
            Action<RecordStatusEnum> changeStatus)
        {
            var menu = new System.Windows.Forms.ContextMenuStrip();

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
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
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

            _analyzerScanCts?.Cancel();
            var cts = new CancellationTokenSource();
            _analyzerScanCts = cts;
            var scanVersion = ++_analyzerScanVersion;

            scanBtn.Enabled = false;
            selectDirBtn.Enabled = false;
            stopBtn.Enabled = true;
            folderTreeView.Nodes.Clear();
            scanProgressBar.Style = ProgressBarStyle.Marquee;

            try
            {
                stopBtn.Tag = cts;

                var result = await _folderAnalyzer.ScanFolderAsync(path, cts.Token);
                cts.Token.ThrowIfCancellationRequested();
                var permission = await Task.Run(() => _folderPermissionAnalyzer.Analyze(path), cts.Token);
                cts.Token.ThrowIfCancellationRequested();
                if (scanVersion != _analyzerScanVersion) return;
                result.AccessStatus = permission.CanRead
                    ? result.AccessStatus
                    : FolderAccessStatus.Denied;
                result.ErrorMessage ??= permission.ErrorMessage;
                result.LastScannedAt = DateTime.Now;

                PopulateTreeView(result);
                UpdateAnalyzerPermission(permission);
            }
            catch (OperationCanceledException)
            {
                scanProgressBar.Style = ProgressBarStyle.Blocks;
                if (scanVersion == _analyzerScanVersion)
                    _analyzerAccessValue.Text = "访问状态：扫描已取消";
            }
            catch (Exception ex)
            {
                BeginInvoke(() =>
                    MessageBox.Show($"扫描失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
            finally
            {
                if (ReferenceEquals(_analyzerScanCts, cts))
                {
                    scanProgressBar.Style = ProgressBarStyle.Blocks;
                    scanBtn.Enabled = true;
                    selectDirBtn.Enabled = true;
                    stopBtn.Enabled = false;
                    _analyzerScanCts = null;
                    stopBtn.Tag = null;
                }
                cts.Dispose();
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
            var accessSuffix = info.InaccessibleCount > 0 ? $"，不可访问 {info.InaccessibleCount} 项" : string.Empty;
            var displayText = $"{info.Name}  [{FormatHelper.FormatBytes(info.SizeBytes)}, {info.FileCount} 个文件{accessSuffix}]";
            var node = new TreeNode(displayText) { Tag = info };

            foreach (var sub in info.SubFolders.OrderByDescending(s => s.SizeBytes))
            {
                node.Nodes.Add(CreateTreeNode(sub));
            }

            return node;
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
            _analyzerScanCts?.Cancel();
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

        private void rulesExeProcInput_TextChanged(object? sender, EventArgs e) => BindRulesTableCenter();

        private void rulesExeProcAddButton_Click(object? sender, EventArgs e)
        {
            var value = input1.Text.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("请输入应用程序名或可执行文件路径。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var application = _monitorService.AddWatchingApplication(value);
                _databaseService.SaveWatchingApplication(application);
                input1.Text = string.Empty;
                BindRulesTableCenter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加监控应用失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rulesExeProcSelectButton_Click(object? sender, EventArgs e)
        {
            using var form = new ProcessPickForm();
            if (form.ShowDialog() != DialogResult.OK) return;
            foreach (var processName in form.SelectedProcessNames)
            {
                try
                {
                    var application = _monitorService.AddWatchingApplication(processName);
                    _databaseService.SaveWatchingApplication(application);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"添加监控应用失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
            BindRulesTableCenter();
        }

        private void rulesExeProcViewTable_CellClick(object? sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.Record is not WatchingExeInfo application) return;
            rulesExeProcViewTable.SetSelected(application);
            var menu = new System.Windows.Forms.ContextMenuStrip();
            var nextStatus = application.Status == RecordStatusEnum.USING
                ? RecordStatusEnum.FORBIDDEN : RecordStatusEnum.USING;
            menu.Items.Add(nextStatus == RecordStatusEnum.USING ? "启用监控" : "暂停监控").Click += (_, _) =>
            {
                application.Status = nextStatus;
                _databaseService.SaveWatchingApplication(application);
                _monitorService.SetWatchingApplicationStatus(application.FullPath, nextStatus);
                BindRulesTableCenter();
            };
            menu.Items.Add("从列表删除").Click += (_, _) =>
            {
                _databaseService.DeleteWatchingApplication(application.FullPath);
                _monitorService.SetWatchingApplicationStatus(application.FullPath, RecordStatusEnum.DELETED);
                BindRulesTableCenter();
            };
            menu.Show(Cursor.Position);
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
            if (!exeModeRadio.Checked) return;
            if (_monitorService.IsRunning)
            {
                MessageBox.Show("需要暂停监测才能选择模式。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                defaultModeRadio.Checked = true;
                return;
            }
            _monitorService.EnableExeMode();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }
        #endregion



    }
}
