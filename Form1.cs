using System.ComponentModel;
using CdiskClean.Models;
using CdiskClean.Services;

namespace CdiskClean
{
    public partial class HideButton : Form
    {
        private readonly DiskMonitorService _monitorService;
        private readonly DiskSpaceService _diskSpaceService;
        private readonly FolderSizeAnalyzer _folderAnalyzer;
        private readonly BindingList<FileChangeRecord> _records;
        private readonly object _recordsLock = new();
        private const int MaxRecords = 5000;

        public HideButton()
        {
            InitializeComponent();
            _monitorService = new DiskMonitorService();
            _diskSpaceService = new DiskSpaceService();
            _folderAnalyzer = new FolderSizeAnalyzer();
            _records = new BindingList<FileChangeRecord>();

            changesDataGrid.DataSource = _records;
            typeFilterCombo.SelectedIndex = 0;

            _monitorService.FileChanged += OnFileChanged;
            _monitorService.MonitorError += OnMonitorError;
        }

        // ==================== 窗体加载 ====================

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            diskRefreshTimer.Start();
            RefreshDiskInfo();
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
                _monitorService.Start();
                pauseBtn.Text = "暂停";
                watchStatusLabel.Text = "监测中";
                watchStatusLabel.ForeColor = Color.Green;
            }
            else
            {
                _monitorService.Stop();
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
                    writer.WriteLine("时间,类型,文件名,路径,大小");
                    foreach (var r in snapshot)
                    {
                        var size = r.SizeBytes.HasValue ? r.SizeBytes.ToString() : "";
                        writer.WriteLine($"{r.Timestamp:yyyy-MM-dd HH:mm:ss},{r.ChangeType},\"{r.FileName}\",\"{r.FullPath}\",{size}");
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
        }

        private void OnFileChanged(FileChangeRecord record)
        {
            BeginInvoke(() =>
            {
                lock (_recordsLock)
                {
                    _records.Insert(0, record);

                    while (_records.Count > MaxRecords)
                        _records.RemoveAt(_records.Count - 1);
                }

                UpdateRecordCount();

                // 如果筛选是"全部"，直接绑定；否则重新应用筛选
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
                var progress = new Progress<int>(_ =>
                {
                    // 进度更新（当前只做脉冲效果）
                });

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

            // 按大小降序排列子节点
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

        // ==================== 窗口关闭 ====================

        private void closeButton_Click(object? sender, EventArgs e)
        {
            _monitorService.Dispose();
            diskRefreshTimer.Stop();
            timer1.Stop();
            Application.Exit();
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
    }
}
