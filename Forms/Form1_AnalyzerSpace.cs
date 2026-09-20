using TreeItem = AntdUI.TreeItem;
using CdiskClean.Helpers;
using CdiskClean.Models;
using System.Text;

namespace CdiskClean;

/// <summary>工作区「空间分析」页逻辑：选中节点详情展示与带入清理流程</summary>
public partial class Form1
{
    //private Label analyzerAccessValue = null!;
    //private Label analyzerExtensionValue = null!;

    private bool _analyzerSelectedCanRead;

    private void InitializeAnalyzerDetails()
    {
        analyzerDetailsSurface.AutoScroll = false;

        //analyzerAccessValue = CreateAnalyzerLabel("访问权限：未分析", 270, Color.FromArgb(102, 112, 133));
        //analyzerExtensionValue = CreateAnalyzerLabel("文件类型：未分析", 300, Color.FromArgb(102, 112, 133));


    }

    private static Label CreateAnalyzerLabel(string text, int top, Color color) => new()
    {
        AutoEllipsis = true,
        BackColor = Color.Transparent,
        ForeColor = color,
        Location = new Point(18, top),
        Size = new Size(300, 28),
        Text = text
    };

    private void folderTreeView_SelectChanged(object? sender, AntdUI.TreeSelectEventArgs e)
    {
        if (e.Item?.Tag is not FolderSizeInfo info) return;
        analyzerPathValue.Text = info.Path;
        analyzerSizeValue.Text = $"大小：{FormatHelper.FormatBytes(info.SizeBytes)}";
        analyzerFilesValue.Text = $"文件：{info.FileCount:N0}";
        analyzerFoldersValue.Text = $"子目录：{info.SubFolders.Count:N0}";
        var inaccessibleText = info.InaccessibleCount == 0 ? "" : $"，不可访问 {info.InaccessibleCount:N0} 项";
        analyzerAccessValue.Text = $"访问状态：{GetAccessStatusText(info.AccessStatus)}{inaccessibleText}";
        _analyzerSelectedCanRead = info.AccessStatus is not (FolderAccessStatus.Denied or FolderAccessStatus.Missing or FolderAccessStatus.Error);
        analyzerAccessValue.ForeColor = info.AccessStatus is FolderAccessStatus.Denied or FolderAccessStatus.Error
            ? UiTheme.Danger
            : info.AccessStatus == FolderAccessStatus.Partial ? Color.FromArgb(217, 119, 6) : UiTheme.TextSecondary;
        analyzerExtensionValue.Text = $"文件类型：{FormatExtensionSummary(info)}";
        analyzerRelatedValue.Text = "正在读取关联变更…";
        var version = ++_analyzerScanVersion;
        _ = LoadAnalyzerRelationsAsync(info.Path, version);
    }

    private async Task LoadAnalyzerRelationsAsync(string path, int version)
    {
        List<FileChangeRecord> records;
        try
        {
            records = await Task.Run(() => _databaseService.History.GetChangeRecordsUnderPath(path, 50));
        }
        catch (Exception ex)
        {
            records = new List<FileChangeRecord>();
            if (version == _analyzerScanVersion) analyzerRelatedValue.Text = $"读取失败：{ex.Message}";
            return;
        }

        if (version != _analyzerScanVersion || IsDisposed) return;
        var builder = new StringBuilder();
        foreach (var record in records.Take(8))
        {
            var process = string.IsNullOrWhiteSpace(record.SourceProcess) ? "未知进程" : record.SourceProcess;
            builder.AppendLine($"{record.Timestamp:MM-dd HH:mm}  {process}  {record.FileName}");
        }
        analyzerRelatedValue.Text = builder.Length == 0 ? "暂无关联变更记录" : builder.ToString().TrimEnd();
    }

    private void analyzerOpenRecordsButton_Click(object? sender, EventArgs e)
    {
        var path = analyzerPathValue.Text;
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path)) return;
        recordSearchBox.Text = path;
        typeFilterCombo.SelectedIndex = 0;
        ShowWorkspacePage(ActivityPageId);
    }

    private void analyzerOpenHistoryButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(analyzerPathValue.Text) || !Directory.Exists(analyzerPathValue.Text)) return;
        ShowWorkspacePage(RecordsPageId);
        ShowRecordView("details");
    }

    private static string GetAccessStatusText(FolderAccessStatus status) => status switch
    {
        FolderAccessStatus.Accessible => "可读取",
        FolderAccessStatus.Partial => "部分可读取",
        FolderAccessStatus.Denied => "无读取权限",
        FolderAccessStatus.Missing => "目录不存在",
        _ => "读取失败"
    };

    private void UpdateAnalyzerPermission(FolderPermissionInfo permission)
    {
        _analyzerSelectedCanRead = permission.CanRead;
        var ownerText = string.IsNullOrWhiteSpace(permission.Owner) ? "未知所有者" : permission.Owner;
        analyzerAccessValue.Text = $"访问状态：{permission.Status} · ACL {permission.RuleCount} 条 · {ownerText}";
        analyzerAccessValue.ForeColor = permission.CanRead ? UiTheme.Success : UiTheme.Danger;
    }

    private static string FormatExtensionSummary(FolderSizeInfo info)
    {
        if (info.ExtensionSizes.Count == 0) return "暂无文件";
        var largest = info.ExtensionSizes
            .OrderByDescending(pair => pair.Value)
            .First();
        return $"{largest.Key} {FormatHelper.FormatBytes(largest.Value)}";
    }

    private void UseAnalyzerPathForCleanup()
    {
        var path = analyzerPathValue.Text;
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            MessageBox.Show("请先在分析结果中选择有效目录", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!_analyzerSelectedCanRead)
        {
            MessageBox.Show("当前目录不可读取，无法安全带入清理。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        cleanPathTextBox.Text = path;
        ShowWorkspacePage(CleanupPageId);
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void analyzerUseForCleanupButton_Click(object? sender, EventArgs e) => UseAnalyzerPathForCleanup();

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
        folderTreeView.Items.Clear();
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
                analyzerAccessValue.Text = "访问状态：扫描已取消";
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
        folderTreeView.Items.Clear();
        var rootItem = CreateTreeItem(info);
        folderTreeView.Items.Add(rootItem);
        rootItem.Expand = true;
    }

    private static TreeItem CreateTreeItem(FolderSizeInfo info)
    {
        var accessSuffix = info.InaccessibleCount > 0 ? $"，不可访问 {info.InaccessibleCount} 项" : string.Empty;
        var item = new TreeItem
        {
            Text = info.Name,
            SubTitle = $"{FormatHelper.FormatBytes(info.SizeBytes)}, {info.FileCount} 个文件{accessSuffix}",
            Tag = info
        };

        foreach (var sub in info.SubFolders.OrderByDescending(s => s.SizeBytes))
        {
            item.Sub.Add(CreateTreeItem(sub));
        }

        return item;
    }

    #endregion
}
