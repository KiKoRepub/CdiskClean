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

            // 初始化目录列表视图
            SetupDirListView();
            PopulateDirListView();
            SetupDirContextMenu();
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

            foreach (var dir in _monitorService.WatchDirectories)
            {
                addWatchingToListView(dir);
            }
        }

        private void addWatchingToListView(WatchingDirectory dir)
        {
            // 根据路径 判重
            if (watcherDirListView.Items.Cast<ListViewItem>()
                .Any(item => item.Text == dir.Path))
                return;

            var item = new ListViewItem(dir.Path);
            item.SubItems.Add(FormatStatus(dir.Status));
            item.SubItems.Add(dir.IncludeSubdirs ? "是" : "否");
            item.Tag = dir;


            ApplyDirItemStyle(item, dir.Status);
            watcherDirListView.Items.Add(item);
        }

        private static string FormatStatus(RecordStatusEnum status)
        {
            return status switch
            {
                RecordStatusEnum.USING => "启用",
                RecordStatusEnum.FORBIDDEN => "已禁用",
                RecordStatusEnum.DELETED => "已删除",
                _ => "未知"
            };
        }
        /// <summary>
        /// 调整 ListViewItem 的前景色和背景色，以便根据状态显示不同的颜色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="status"></param>
        private void ApplyDirItemStyle(ListViewItem item, RecordStatusEnum status)
        {
            switch (status)
            {
                case RecordStatusEnum.USING:
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.FromArgb(230, 255, 230); // 浅绿底
                    break;
                case RecordStatusEnum.FORBIDDEN:
                    item.ForeColor = Color.Gray;
                    item.BackColor = Color.FromArgb(255, 255, 230); // 浅黄底
                    break;
                case RecordStatusEnum.DELETED:
                    item.ForeColor = Color.LightGray;
                    item.BackColor = Color.White;
                    break;
            }
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
            // 弹出输入框让用户输入进程名
            var input = Microsoft.VisualBasic.Interaction.InputBox("请输入进程名:", "添加进程", "");
            if (!string.IsNullOrEmpty(input))
            {
                // 处理添加进程的逻辑
                MessageBox.Show($"已添加进程: {input}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // 处理拖拽事件
        private void ignoreProcessView_DragEnter(object sender, DragEventArgs e)
        {
           
        }
    }
}
