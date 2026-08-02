using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CdiskClean
{
    public partial class Form1 : Form
    {
        private readonly IDatabaseService _databaseService;
        private readonly EtwMonitorService _etwService;
        private readonly DiskMonitorService _monitorService;
        private readonly DiskSpaceService _diskSpaceService;
        private readonly FolderSizeAnalyzer _folderAnalyzer;
        private readonly StatisticsService _statisticsService;
        private readonly NotificationService _notificationService;
        private readonly BindingList<FileChangeRecord> _records;

        private readonly object _recordsLock = new();
        private const int MaxRecords = 5000;

        public Form1()
        {
            InitializeComponent();

            // 初始化 ETW 监控
            _etwService = new EtwMonitorService();

            // 初始化数据库
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CdiskClean.db");
            _databaseService = new DatabaseService(dbPath);
            _databaseService.Initialize();

            // 从数据库加载监视目录（空则用默认列表）
            var savedDirs = _databaseService.GetWatchDirectories();
            _monitorService = new DiskMonitorService(_etwService);
            if (savedDirs.Count > 0)
                _monitorService.LoadDirectories(savedDirs);
            else
                _monitorService.LoadDefaults();

            // 从数据库加载忽略进程（空则用默认列表），并同步进 ETW 黑名单
            var savedProcs = _databaseService.GetIgnoreProcessRecords();
            if (savedProcs.Count > 0)
                _monitorService.LoadIgnoreProcesses(savedProcs);
            else
                _monitorService.LoadIgnoreProcesses(IgnoreProcessRecord.getDefaultRecords());

            // 初始化磁盘空间服务和文件夹分析器
            _diskSpaceService = new DiskSpaceService();
            _folderAnalyzer = new FolderSizeAnalyzer();

            // 初始化统计服务
            _statisticsService = new StatisticsService();
            _statisticsService.CountdownChanged += OnCountdownChanged;
            _statisticsService.StatsReady += OnStatsReady;

            // 初始化右下角提醒服务（与统计按钮相互独立）
            _notificationService = new NotificationService();
            _notificationService.NotificationTriggered += OnNotificationTriggered;

            // 设置数据绑定
            _records = new BindingList<FileChangeRecord>();
            changesDataGrid.DataSource = _records;


            typeFilterCombo.SelectedIndex = 0;

            // 订阅监视服务事件
            _monitorService.FileChanged += OnFileChanged;
            _monitorService.FileRecordUpdated += OnFileRecordUpdated;
            _monitorService.MonitorError += OnMonitorError;

            // 初始化监视目录列表视图
            SetupDirListView();
            PopulateDirListView();
            SetupDirContextMenu();

            // 初始化忽略进程列表视图
            SetupProcessListView();
            PopulateProcessListView();
            SetupProcessContextMenu();

        }



        // ==================== 窗体加载 ====================

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            diskRefreshTimer.Start();
            RefreshDiskInfo();
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
        /// <summary>
        ///  填充监视目录列表
        /// </summary>
        private void PopulateDirListView()
        {
            watcherDirListView.Items.Clear();

            _monitorService.WatchDirectories.ForEach(addWatchingToListView);

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
            if (watcherDirListView.SelectedItems.Count > 0)
            {
                ListViewItem item = watcherDirListView.SelectedItems[0];
                //MessageBox.Show(item.Text);
                dirSelectedTextBox.Text = item.Text;
            }
        }

        private void watcherDirListView_Resize(object sender, EventArgs e)
        {
            int totalWidth = watcherDirListView.Width;
            watcherDirListView.Columns[0].Width = (int)(totalWidth * 0.70);
            watcherDirListView.Columns[1].Width = (int)(totalWidth * 0.15);
            watcherDirListView.Columns[2].Width = (int)(totalWidth * 0.15);
        }

        // ==================== 忽略进程列表 ====================

        private void SetupProcessListView()
        {
            int totalWidth = ignoreProcessView.Width;

            ignoreProcessView.View = View.Details;
            ignoreProcessView.FullRowSelect = true;
            ignoreProcessView.MultiSelect = false;
            ignoreProcessView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            // 设置内部列宽度为总宽度的比例
            ignoreProcessView.Columns.Add("进程名称", (int)(totalWidth * 0.80));
            ignoreProcessView.Columns.Add("状态", (int)(totalWidth * 0.20));


            // 开启双缓冲，减少闪烁
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, ignoreProcessView, new object[] { true });
        }


        private void PopulateProcessListView()
        {
            ignoreProcessView.Items.Clear();
            foreach (var proc in _monitorService.IgnoreProcessRecords)
            {
                var item = new ListViewItem(proc.ProcessName);
                item.SubItems.Add(EnumHelper.FormatStatus(proc.Status));
                item.Tag = proc;

                StyleHelper.ApplyRecordStatusStyle(item, proc.Status);
                ignoreProcessView.Items.Add(item);
            }
        }

        private void ignoreProcessView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ignoreProcessView.SelectedItems.Count > 0)
            {
                ListViewItem item = ignoreProcessView.SelectedItems[0];

                procSelectedTextBox.Text = item.Text;
            }
        }

        
        private void ignoreProcessView_Resize(object sender, EventArgs e)
        {
            int totalWidth = ignoreProcessView.Width;
            ignoreProcessView.Columns[0].Width = (int)(totalWidth * 0.80);
            ignoreProcessView.Columns[1].Width = (int)(totalWidth * 0.20);
        }

        /// <summary>
        /// 调整 ListViewItem 的前景色和背景色，以便根据状态显示不同的颜色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status"></param>


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

            // 前置判断
            if (e.Button != MouseButtons.Right) return;

            var item = watcherDirListView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not WatchingDirectory dir) return;

            if (dir.Status == RecordStatusEnum.DELETED) return;

            var menu = new ContextMenuStrip();

            if (dir.Status == RecordStatusEnum.USING)
            {
                var disableItem = menu.Items.Add("禁用监测");
                disableItem.Click += (s, ev) => ChangeDirStatus(dir, RecordStatusEnum.FORBIDDEN);
            }
            else if (dir.Status == RecordStatusEnum.FORBIDDEN)
            {
                var enableItem = menu.Items.Add("启用监测");
                enableItem.Click += (s, ev) => ChangeDirStatus(dir, RecordStatusEnum.USING);
            }

            var deleteItem = menu.Items.Add("从列表删除");
            deleteItem.Click += (s, ev) => ChangeDirStatus(dir, RecordStatusEnum.DELETED);

            menu.Show(watcherDirListView, e.Location);
        }

        private void ChangeDirStatus(WatchingDirectory dir, RecordStatusEnum newStatus)
        {
            dir.Status = newStatus;
            _monitorService.SetDirectoryStatus(dir.Path, newStatus);

            // 同步数据库
            if (newStatus == RecordStatusEnum.DELETED)
                _databaseService.DeleteWatchDirectory(dir.Path);
            else
                _databaseService.SaveWatchDirectory(dir);

            PopulateDirListView();
        }

        // ==================== 忽略进程右键菜单 ====================

        private void SetupProcessContextMenu()
        {
            ignoreProcessView.MouseClick += ignoreProcessView_MouseClick;
        }

        private void ignoreProcessView_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var item = ignoreProcessView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not IgnoreProcessRecord proc) return;

            if (proc.Status == RecordStatusEnum.DELETED) return;

            var menu = new ContextMenuStrip();

            if (proc.Status == RecordStatusEnum.USING)
            {
                var disableItem = menu.Items.Add("禁用监测");
                disableItem.Click += (s, ev) => ChangeProcessStatus(proc, RecordStatusEnum.FORBIDDEN);
            }
            else if (proc.Status == RecordStatusEnum.FORBIDDEN)
            {
                var enableItem = menu.Items.Add("启用监测");
                enableItem.Click += (s, ev) => ChangeProcessStatus(proc, RecordStatusEnum.USING);
            }

            var deleteItem = menu.Items.Add("从列表删除");
            deleteItem.Click += (s, ev) => ChangeProcessStatus(proc, RecordStatusEnum.DELETED);

            menu.Show(ignoreProcessView, e.Location);
        }

        private void ChangeProcessStatus(IgnoreProcessRecord proc, RecordStatusEnum newStatus)
        {
            // 同步 ETW 黑名单（USING 加入、FORBIDDEN/DELETED 移出）
            _monitorService.SetIgnoreProcessStatus(proc.ProcessName, newStatus);

            // 同步数据库：DELETED 物理删除，其余更新状态
            if (newStatus == RecordStatusEnum.DELETED)
                _databaseService.DeleteIgnoreProcessRecord(proc.ProcessName);
            else
                _databaseService.SaveIgnoreProcessRecord(proc);

            PopulateProcessListView();
        }

        // ==================== 磁盘概览 ====================

        private void RefreshDiskInfo()
        {
            try
            {
                var info = _diskSpaceService.GetDriveInfo("C:");
                UpdateDiskUI(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取磁盘信息失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateDiskUI(DriveInfoModel info)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateDiskUI(info));
                return;
            }

            totalSpaceLabel.Text = $"总容量: {FormatBytes(info.TotalSizeBytes)}";
            usedSpaceLabel.Text = $"已用: {FormatBytes(info.UsedSpaceBytes)}";
            freeSpaceLabel.Text = $"剩余: {FormatBytes(info.FreeSpaceBytes)}";

            usageProgressBar.Value = (int)Math.Min(info.UsagePercent, 100);

            if (info.UsagePercent > 90)
                usageProgressBar.ForeColor = Color.Red;
            else if (info.UsagePercent > 70)
                usageProgressBar.ForeColor = Color.Orange;
            else
                usageProgressBar.ForeColor = Color.LimeGreen;

            warningLabel.Visible = info.IsLowSpace;
        }

        private void diskRefreshTimer_Tick(object? sender, EventArgs e)
        {
            RefreshDiskInfo();
        }

        // ==================== 实时监测 ====================

        private void pauseBtn_Click(object? sender, EventArgs e)
        {
            if (!_monitorService.IsRunning)
            {
                _etwService.Start();
                _monitorService.Start();
                _statisticsService.Start();
                _notificationService.Start();
                pauseBtn.Text = "暂停";
                watchStatusLabel.Text = "监测中";
                watchStatusLabel.ForeColor = Color.Green;
                notifyIcon1.Text += "\r\n监测中...";
            }
            else
            {
                _monitorService.Stop();
                _etwService.Stop();
                _statisticsService.Stop();
                _notificationService.Stop();
                pauseBtn.Text = "开始监测";
                watchStatusLabel.Text = "已暂停";
                watchStatusLabel.ForeColor = Color.Gray;
            }
        }

        private void clearBtn_Click(object? sender, EventArgs e)
        {
            lock (_recordsLock)
            {
                _records.Clear();
            }
            UpdateRecordCount();
        }

        private void exportBtn_Click(object? sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "CSV文件|*.csv|文本文件|*.txt",
                DefaultExt = "csv",
                FileName = $"C盘监测记录_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            Task.Run(() =>
            {
                try
                {
                    List<FileChangeRecord> snapshot;
                    lock (_recordsLock)
                    {
                        snapshot = _records.ToList();
                    }

                    using var writer = new StreamWriter(dialog.FileName, false, System.Text.Encoding.UTF8);
                    writer.WriteLine("时间,类型,文件名,路径,大小,来源进程");
                    foreach (var r in snapshot)
                    {
                        var size = r.SizeBytes.HasValue ? r.SizeBytes.ToString() : "";
                        var proc = r.SourceProcess ?? "";
                        writer.WriteLine($"{r.Timestamp:yyyy-MM-dd HH:mm:ss},{r.ChangeType},\"{r.FileName}\",\"{r.FullPath}\",{size},\"{proc}\"");
                    }

                    BeginInvoke(() =>
                        MessageBox.Show($"已导出 {snapshot.Count} 条记录到:\n{dialog.FileName}",
                            "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information));
                }
                catch (Exception ex)
                {
                    BeginInvoke(() =>
                        MessageBox.Show($"导出失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
            });
        }

        private void typeFilterCombo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }
        /// <summary>
        /// 添加类型过滤器的逻辑，根据选择的类型过滤显示的记录
        /// </summary>
        private void ApplyFilter()
        {
            var filterIndex = typeFilterCombo.SelectedIndex;
            if (filterIndex <= 0)
            {
                changesDataGrid.DataSource = _records;
                return;
            }

            var targetType = filterIndex switch
            {
                1 => ChangeType.Created,
                2 => ChangeType.Changed,
                3 => ChangeType.Deleted,
                4 => ChangeType.Renamed,
                _ => (ChangeType?)null
            };

            var filtered = _records.Where(r => r.ChangeType == targetType).ToList();
            changesDataGrid.DataSource = new BindingList<FileChangeRecord>(
                new List<FileChangeRecord>(filtered));

            changesDataGrid.AutoGenerateColumns = false;
        }

        private void OnFileChanged(FileChangeRecord record)
        {
            // 喂入统计服务
            _statisticsService.RecordChange(record);

            // 进程已知时直接喂入提醒服务；未知的等待 ETW 延迟解析后在 OnFileRecordUpdated 补入
            if (record.SourceProcess != null)
                _notificationService.RecordChange(record);

            BeginInvoke(() =>
            {
                lock (_recordsLock)
                {
                    _records.Insert(0, record);

                    while (_records.Count > MaxRecords)
                        _records.RemoveAt(_records.Count - 1);
                }

                UpdateRecordCount();

                if (typeFilterCombo.SelectedIndex <= 0)
                {
                    changesDataGrid.DataSource = _records;
                    changesDataGrid.Refresh();
                }
                else
                {
                    ApplyFilter();
                }
            });
        }

        private void OnMonitorError(string message)
        {
            BeginInvoke(() =>
            {
                writedRecordStatusLabel.Text = message;
            });
        }

        private void OnFileRecordUpdated(FileChangeRecord record)
        {
            // ETW 延迟解析到来源进程后，补喂提醒服务（避免归入"未知进程"）
            if (record.SourceProcess != null)
                _notificationService.RecordChange(record);

            BeginInvoke(() =>
            {
                changesDataGrid.Refresh();
            });
        }

        private void OnNotificationTriggered(ProcessNotificationRecord record)
        {
            try
            {
                _databaseService.SaveProcessNotification(record);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存提醒记录失败: {ex.Message}");
            }

            if (!IsHandleCreated) return;

            BeginInvoke(() =>
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(3000,
                    "进程操作提醒",
                    $"进程 {record.ProcessName} 在 {record.DurationSeconds} 秒 内 对监控目录 执行了 {record.OperationCount} 次操作。",
                    ToolTipIcon.Info);
            });
        }

        private void UpdateRecordCount()
        {
            writedRecordStatusLabel.Text = $"已记录 {_records.Count} 条";
        }

        // ==================== 文件夹分析 ====================

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
                var progress = new Progress<int>(_ => { });

                var cts = new CancellationTokenSource();
                stopBtn.Tag = cts;

                var result = await _folderAnalyzer.ScanFolderAsync(path, progress, cts.Token);

                await Task.Run(() =>
                {
                    BeginInvoke(() => PopulateTreeView(result));
                });
            }
            catch (OperationCanceledException)
            {
                writedRecordStatusLabel.Text = "扫描已取消";
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
            _folderAnalyzer.CancelScan();
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
            var displayText = $"{info.Name}  [{FormatBytes(info.SizeBytes)}, {info.FileCount} 个文件]";
            var node = new TreeNode(displayText);

            foreach (var sub in info.SubFolders.OrderByDescending(s => s.SizeBytes))
            {
                node.Nodes.Add(CreateTreeNode(sub));
            }

            return node;
        }

        // ==================== 状态栏交互 ====================

        private void WritedRecordStatusLabel_Click(object? sender, EventArgs e)
        {
            MessageBox.Show($"当前共记录 {_records.Count} 条文件变化。", "记录统计",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void watchStatusLabel_Click(object? sender, EventArgs e)
        {
            TabPageControl1.SelectedIndex = 1;
        }

        // ==================== 时钟 ====================

        private void timer1_Tick(object? sender, EventArgs e)
        {
            timeStatusLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timeStatusLabel.Size = new Size(150, 24);
        }

        // ==================== 右上角 ====================

        private void BiggerButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void closeButton_Click(object? sender, EventArgs e)
        {
            this.Hide();
            closeApplication();
        }

        // ==================== 工具方法 ====================

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private Point mouseDownPoint;
        private void splitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPoint = new Point(e.X, e.Y);
            }
        }

        private void panelTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(
                    this.Location.X + e.X - mouseDownPoint.X,
                    this.Location.Y + e.Y - mouseDownPoint.Y);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        public void closeApplication()
        {
            _monitorService.Dispose();
            _etwService.Dispose();
            _statisticsService.Dispose();
            _notificationService.Dispose();
            diskRefreshTimer.Stop();
            timer1.Stop();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;
            const int HTCAPTION = 2;
            const int HTCLIENT = 1;

            if (m.Msg == WM_NCHITTEST)
            {
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);

                int resizeBorderWidth = 10;

                if (clientPoint.X >= this.ClientSize.Width - resizeBorderWidth &&
                    clientPoint.Y >= this.ClientSize.Height - resizeBorderWidth)
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                    return;
                }
                else if (clientPoint.Y >= this.ClientSize.Height - resizeBorderWidth)
                {
                    m.Result = (IntPtr)HTBOTTOM;
                    return;
                }
                else if (clientPoint.X >= this.ClientSize.Width - resizeBorderWidth)
                {
                    m.Result = (IntPtr)HTRIGHT;
                    return;
                }

                if (panelTitle.RectangleToScreen(panelTitle.ClientRectangle).Contains(screenPoint))
                {
                    m.Result = (IntPtr)HTCAPTION;
                    return;
                }
            }

            base.WndProc(ref m);
        }


        private void dirAddButton_Click(object sender, EventArgs e)
        {

            DialogResult result = ImportFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedPath = ImportFolderDialog.SelectedPath;
                //MessageBox.Show(selectedPath);
                // 将选中的目录添加到监视列表
                WatchingDirectory dir = _monitorService.AddDirectoryToEtwArr(selectedPath, true);
                // 显示在列表
                addWatchingToListView(dir);
            }
        }

        private void statisticButton_Click(object sender, EventArgs e)
        {
            List<FileChangeRecord> snapshot;
            lock (_recordsLock)
            {
                snapshot = _records.ToList();
            }

            if (snapshot.Count == 0)
            {
                MessageBox.Show("暂无变更记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 提醒记录来自数据库，与右下角提示展示逻辑相互独立
            var notifications = _databaseService.GetProcessNotifications();
            var form4 = new Form4(snapshot, notifications);
            form4.ShowDialog();
        }





        private void OnCountdownChanged(int remaining)
        {
            BeginInvoke(() =>
            {
                if (remaining > 0)
                {
                    statsCountdownLabel.Text = $"倒计时: {remaining}s";
                    statsCountdownLabel.ForeColor = Color.SteelBlue;
                }
                else
                {
                    statsCountdownLabel.Text = "统计就绪";
                    statsCountdownLabel.ForeColor = Color.Green;
                }
            });
        }

        private void OnStatsReady(List<Models.AppChangeStats> stats)
        {
            BeginInvoke(() =>
            {
                if (stats.Count == 0)
                {
                    statsSummaryLabel.Text = "暂无统计数据";
                    return;
                }

                var lines = new List<string>();
                foreach (var s in stats.Take(5))
                {
                    var timeStr = s.LastChangeTime.ToString("HH:mm:ss");
                    lines.Add($"{s.AppName}: {s.ChangeCount}次 (最后: {timeStr})");
                }

                if (stats.Count > 5)
                    lines.Add($"...及其他 {stats.Count - 5} 个应用");

                statsSummaryLabel.Text = string.Join("  |  ", lines);
            });
        }

        private void statsResetBtn_Click(object? sender, EventArgs e)
        {
            _statisticsService.Reset();
            statsCountdownLabel.Text = "倒计时: --s";
            statsCountdownLabel.ForeColor = Color.SteelBlue;
            statsSummaryLabel.Text = "等待数据收集中...";
        }

        private void ProcessAddButton_Click(object sender, EventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox("请输入进程名:", "添加忽略进程", "");
            var name = input?.Trim();
            if (string.IsNullOrEmpty(name)) return;

            AddIgnoreProcessInternal(name);
        }

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
        private void changesDataGrid_MouseMove(object sender, MouseEventArgs e)
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

        // 鼠标松开：重置标记
        private void changesDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            _isGridDragging = false;
        }

        private void ignoreProcessView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FileChangeRecord)) &&
                (e.Data.GetData(typeof(FileChangeRecord)) as FileChangeRecord)?.SourceProcess != null)
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
            if (e.Data.GetData(typeof(FileChangeRecord)) is not FileChangeRecord record ||
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

        /// <summary>
        /// 添加忽略进程：去重 → 同步 ETW 黑名单 → 入库 → 刷新列表
        /// </summary>
        private void AddIgnoreProcessInternal(string processName)
        {
            if (_monitorService.IgnoreProcessRecords.Any(r =>
                    string.Equals(r.ProcessName, processName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"进程「{processName}」已在忽略列表中。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var record = _monitorService.AddIgnoreProcess(processName);

            try
            {
                _databaseService.SaveIgnoreProcessRecord(record);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存忽略进程失败: {ex.Message}");
            }

            PopulateProcessListView();

            // 选中刚添加的项
            foreach (ListViewItem item in ignoreProcessView.Items)
            {
                if (string.Equals(item.Text, processName, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    ignoreProcessView.EnsureVisible(item.Index);
                    break;
                }
            }
        }


    }
}
