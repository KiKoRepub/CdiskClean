using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
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
        private readonly NotificationService _notificationService;
        private readonly CleanupService _cleanupService;
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
                var defaultProcesses = IgnoreProcessRecord.getDefaultRecords();
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

            // 设置数据绑定
            _records = new BindingList<FileChangeRecord>();
            changesDataGrid.DataSource = _records;
            changesDataGrid.CellFormatting += changesDataGrid_CellFormatting;

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

            // 低空间警告可点击跳转磁盘清理 Tab
            warningLabel.Cursor = Cursors.Hand;
            warningLabel.Click += warningLabel_Click;
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

        // ==================== 磁盘概览 ====================

        /// <summary>低空间警告可点击跳转磁盘清理 Tab</summary>
        private void warningLabel_Click(object? sender, EventArgs e)
        {
            TabPageControl1.SelectedIndex = 3; // diskCleanPage
        }

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
                _notificationService.Start();
                pauseBtn.Text = "暂停";
                watchStatusLabel.Text = "监测中";
                watchStatusLabel.ForeColor = Color.Green;
                notifyIcon1.Text = "C盘管理工具\r\n监测中";
            }
            else
            {
                _monitorService.Stop();
                _etwService.Stop();
                _notificationService.Stop();
                pauseBtn.Text = "开始监测";
                watchStatusLabel.Text = "已暂停";
                watchStatusLabel.ForeColor = Color.Gray;
                notifyIcon1.Text = "C盘管理工具\r\n已暂停";
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
                        writer.WriteLine(
                            $"{r.Timestamp:yyyy-MM-dd HH:mm:ss},{EnumHelper.FormatChangeType(r.ChangeType)}," +
                            $"{EscapeCsv(r.FileName)},{EscapeCsv(r.FullPath)},{size},{EscapeCsv(proc)}");
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

        private void changesDataGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == TypeColumn.Index + 1 && e.Value is ChangeType changeType)
            {
                e.Value = EnumHelper.FormatChangeType(changeType);
                e.FormattingApplied = true;
            }
        }

        private void OnFileChanged(FileChangeRecord record)
        {
            // DiskMonitorService 已完成延迟归因与忽略过滤；已知进程可直接进入提醒聚合。
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

        // ==================== 磁盘清理 ====================

        private CancellationTokenSource? _cleanScanCts;
        private CancellationTokenSource? _cleanExecCts;
        private bool _treeUpdating;
        private Label? _cleanHistoryEmptyLabel;

        /// <summary>树节点数量上限，超过则仅显示目录节点（文件通过勾选目录整体清理）</summary>
        private const int MaxCleanTreeNodes = 50000;

        // 树勾选状态图标索引（StateImageList）：0=未选 1=全选 2=部分
        private const int StateUnchecked = 0;
        private const int StateChecked = 1;
        private const int StatePartial = 2;

        private void SetupCleanPage()
        {
            SetupFrequentListView();
            SetupTreeStateImages();
            SetupCleanHistoryGrid();
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

        /// <summary>从变更记录中统计高频修改目录，展示在左侧参考列表</summary>
        private void RefreshFrequentPaths()
        {
            frequentPathListView.BeginUpdate();
            frequentPathListView.Items.Clear();

            List<FileChangeRecord> snapshot;
            lock (_recordsLock)
            {
                snapshot = _records.ToList();
            }

            try
            {
                var seen = new HashSet<string>(
                    snapshot.Select(GetChangeRecordKey),
                    StringComparer.OrdinalIgnoreCase);
                foreach (var record in _databaseService.GetChangeRecords(5000))
                {
                    if (seen.Add(GetChangeRecordKey(record)))
                        snapshot.Add(record);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取历史变更记录失败: {ex.Message}");
            }

            var paths = CleanupService.GetFrequentPaths(snapshot, 30);
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
                var progress = new Progress<int>(_ => { });
                var entries = await _cleanupService.ScanDirectoryAsync(path, progress, cts.Token);
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
                    ? $"扫描完成：{fileCount} 个文件，共 {FormatBytes(totalSize)}（文件过多仅显示目录，可勾选目录整体清理）"
                    : $"扫描完成：{fileCount} 个文件，共 {FormatBytes(totalSize)}";
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
            var rootNode = new TreeNode($"{rootName}  [{FormatBytes(totalSize)}]")
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

                var node = new TreeNode($"{entry.Name}  [{FormatBytes(entry.SizeBytes)}]")
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
                    var node = new TreeNode($"{entry.Name}  [{FormatBytes(entry.SizeBytes)}, {entry.LastWriteTime:yyyy-MM-dd HH:mm}]")
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
            if (cleanPermanentRadio.Checked) return CleanupMethod.PermanentDelete;
            if (cleanMoveRadio.Checked) return CleanupMethod.Move;
            if (cleanCompressRadio.Checked) return CleanupMethod.Compress;
            if (cleanMklinkRadio.Checked) return CleanupMethod.Mklink;
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
                if (!string.IsNullOrEmpty(basePath) && IsPathUnder(basePath, targetDir))
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
                    $"确定要将选中的 {entries.Count} 项（共 {FormatBytes(totalSize)}）移入回收站吗？",
                CleanupMethod.PermanentDelete =>
                    $"确定要永久删除选中的 {entries.Count} 项（共 {FormatBytes(totalSize)}）吗？\n\n此操作不可恢复！",
                CleanupMethod.Move =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatBytes(totalSize)}）移动到：\n{targetDir}\n\n吗？",
                CleanupMethod.Compress =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatBytes(totalSize)}）压缩到：\n{targetDir}\n\n并删除原文件吗？",
                CleanupMethod.Mklink =>
                    $"确定要将选中的 {entries.Count} 项（共 {FormatBytes(totalSize)}）迁移到：\n{targetDir}\n\n并在原位置创建软链接吗？",
                _ => $"确定要清理选中的 {entries.Count} 项吗？"
            };

            if (MessageBox.Show(confirmText, $"确认清理（{methodName}）",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            _cleanExecCts = new CancellationTokenSource();
            var cts = _cleanExecCts;

            cleanBtn.Enabled = false;
            cleanBtn.Text = "取消清理";
            cleanScanProgressBar.Style = ProgressBarStyle.Marquee;
            var progress = new Progress<string>(s => cleanStatusLabel.Text = s);

            long freeBefore = GetFreeSpaceSafe();
            try
            {
                var result = await _cleanupService.ExecuteAsync(entries, method, targetDir, progress, cts.Token);
                long freedDelta = Math.Max(0, GetFreeSpaceSafe() - freeBefore);

                var summary = $"清理完成：成功 {result.Success} 项，失败 {result.Fail} 项";
                if (method is CleanupMethod.PermanentDelete or CleanupMethod.Compress)
                    summary += $"\n释放空间约 {FormatBytes(freedDelta)}";
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
                cleanBtn.Enabled = true;
                cleanBtn.Text = "清理选中文件";
                cleanScanProgressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private static bool IsPathUnder(string basePath, string path)
        {
            var full = Path.GetFullPath(path).TrimEnd('\\');
            var root = Path.GetFullPath(basePath).TrimEnd('\\');
            return full.Equals(root, StringComparison.OrdinalIgnoreCase) ||
                   full.StartsWith(root + "\\", StringComparison.OrdinalIgnoreCase);
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
                CleanupMethod.PermanentDelete => $"已永久删除 {result.Success} 项，释放约 {FormatBytes(freedDelta)}",
                CleanupMethod.Move => $"已将 {result.Success} 项移动到目标目录",
                CleanupMethod.Compress =>
                    $"已压缩 {result.Success} 项到目标目录，选中文件总大小 {FormatBytes(selectedTotalSize)}",
                CleanupMethod.Mklink => $"已将 {result.Success} 项迁移至目标目录并创建软链接",
                _ => $"清理完成，成功 {result.Success} 项"
            };
            if (result.Fail > 0) msg += $"，失败 {result.Fail} 项";

            notifyIcon1.ShowBalloonTip(3000, "清理完成", msg, ToolTipIcon.Info);
        }

        private long GetFreeSpaceSafe()
        {
            try { return _diskSpaceService.GetDriveInfo("C:").FreeSpaceBytes; }
            catch { return 0; }
        }

        private void RefreshCleanHistory()
        {
            var records = _databaseService.GetCleanupRecords(200);
            cleanHistoryGrid.DataSource = records;
            if (_cleanHistoryEmptyLabel != null)
                _cleanHistoryEmptyLabel.Visible = records.Count == 0;

            // 列配置（数据绑定后列才存在）：隐藏原始列，其余按比例分配宽度
            if (records.Count > 0 && cleanHistoryGrid.Columns.Count > 0)
            {
                cleanHistoryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                foreach (DataGridViewColumn col in cleanHistoryGrid.Columns)
                {
                    col.Visible = col.Name is not ("Id" or "SizeBytes" or "Success");
                    col.FillWeight = col.Name switch
                    {
                        "FullPath" => 30,
                        "CleanupTime" => 16,
                        "FileName" => 14,
                        "Message" => 12,
                        "Method" => 8,
                        "SizeText" => 8,
                        "ResultText" => 6,
                        _ => 8
                    };
                }
            }
        }

        /// <summary>历史表格：隐藏原始列、设置列宽比例、右键菜单与空状态提示</summary>
        private void SetupCleanHistoryGrid()
        {
            cleanHistoryGrid.CellContextMenuStripNeeded += cleanHistoryGrid_CellContextMenuStripNeeded;

            // 空状态提示标签（覆盖在表格上方）
            _cleanHistoryEmptyLabel = new Label
            {
                Text = "暂无清理记录",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray,
                BackColor = Color.White,
                Visible = false,
                Bounds = cleanHistoryGrid.Bounds,
                Anchor = cleanHistoryGrid.Anchor
            };
            cleanHistoryGrid.Parent!.Controls.Add(_cleanHistoryEmptyLabel);
            _cleanHistoryEmptyLabel.BringToFront();
        }

        private void cleanHistoryGrid_CellContextMenuStripNeeded(
            object? sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex < 0 || cleanHistoryGrid.Rows[e.RowIndex].DataBoundItem is not CleanupRecord record)
                return;

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

            e.ContextMenuStrip = menu;
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

        private static string GetChangeRecordKey(FileChangeRecord record) =>
            $"{record.Timestamp.Ticks}|{record.ChangeType}|{record.FullPath}";

        private static string EscapeCsv(string? value)
        {
            const char quote = (char)34;
            return quote + (value ?? string.Empty).Replace(
                quote.ToString(),
                new string(quote, 2)) + quote;
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

        /// <summary>
        /// 添加忽略进程：去重 → 同步 ETW 黑名单 → 入库 → 刷新列表
        /// </summary>
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

        private void betterDirAddButton_Click(object sender, EventArgs e)
        {
            //TODO 添加监控目录可以以一个目录为基础，在新窗口中勾选子路径 [树状列表]
            LogHelper.showDefaultToDoMessage("添加监控目录可以以一个目录为基础，在新窗口中勾选子路径");
        }

        private void betterProcessAddButton_Click(object sender, EventArgs e)
        {
            //TODO 任务管理器中选择进程，添加到忽略列表
           LogHelper.showDefaultToDoMessage("任务管理器中选择进程，添加到忽略列表");
        }
    }
}
