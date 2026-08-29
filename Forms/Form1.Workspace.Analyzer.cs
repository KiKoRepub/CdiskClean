using CdiskClean.Helpers;
using CdiskClean.Models;
using System.Text;

namespace CdiskClean;

/// <summary>工作区「空间分析」页逻辑：选中节点详情展示与带入清理流程</summary>
public partial class Form1
{
    private Label _analyzerAccessValue = null!;
    private Label _analyzerExtensionValue = null!;
    private TextBox _analyzerRelatedValue = null!;
    private AntdUI.Button _analyzerOpenRecordsButton = null!;
    private AntdUI.Button _analyzerOpenHistoryButton = null!;
    private bool _analyzerSelectedCanRead;

    private void InitializeAnalyzerDetails()
    {
        analyzerDetailsSurface.AutoScroll = true;
        analyzerUseForCleanupButton.Location = new Point(18, 545);
        analyzerUseForCleanupButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

        _analyzerAccessValue = CreateAnalyzerLabel("访问权限：未分析", 270, Color.FromArgb(102, 112, 133));
        _analyzerExtensionValue = CreateAnalyzerLabel("文件类型：未分析", 300, Color.FromArgb(102, 112, 133));
        analyzerDetailsSurface.Controls.Add(_analyzerAccessValue);
        analyzerDetailsSurface.Controls.Add(_analyzerExtensionValue);

        var relatedTitle = CreateAnalyzerLabel("最近关联变更", 332, Color.FromArgb(31, 41, 55));
        relatedTitle.Font = new Font(relatedTitle.Font, FontStyle.Bold);
        analyzerDetailsSurface.Controls.Add(relatedTitle);

        _analyzerRelatedValue = new TextBox
        {
            Location = new Point(18, 360),
            Size = new Size(300, 100),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(102, 112, 133),
            Text = "扫描后显示最近变更"
        };
        analyzerDetailsSurface.Controls.Add(_analyzerRelatedValue);

        _analyzerOpenRecordsButton = new AntdUI.Button
        {
            Location = new Point(190, 500),
            Size = new Size(130, 38),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
            Text = "实时活动",
            IconSvg = "HistoryOutlined",
            Type = AntdUI.TTypeMini.Default,
            Radius = 1
        };
        _analyzerOpenRecordsButton.Click += analyzerOpenRecordsButton_Click;
        analyzerDetailsSurface.Controls.Add(_analyzerOpenRecordsButton);

        _analyzerOpenHistoryButton = new AntdUI.Button
        {
            Location = new Point(18, 500),
            Size = new Size(160, 38),
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
            Text = "记录中心",
            IconSvg = "HistoryOutlined",
            Type = AntdUI.TTypeMini.Default,
            Radius = 1
        };
        _analyzerOpenHistoryButton.Click += analyzerOpenHistoryButton_Click;
        analyzerDetailsSurface.Controls.Add(_analyzerOpenHistoryButton);
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

    private void folderTreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not FolderSizeInfo info) return;
        analyzerPathValue.Text = info.Path;
        analyzerSizeValue.Text = $"大小：{FormatHelper.FormatBytes(info.SizeBytes)}";
        analyzerFilesValue.Text = $"文件：{info.FileCount:N0}";
        analyzerFoldersValue.Text = $"子目录：{info.SubFolders.Count:N0}";
        var inaccessibleText = info.InaccessibleCount == 0 ? "" : $"，不可访问 {info.InaccessibleCount:N0} 项";
        _analyzerAccessValue.Text = $"访问状态：{GetAccessStatusText(info.AccessStatus)}{inaccessibleText}";
        _analyzerSelectedCanRead = info.AccessStatus is not (FolderAccessStatus.Denied or FolderAccessStatus.Missing or FolderAccessStatus.Error);
        _analyzerAccessValue.ForeColor = info.AccessStatus is FolderAccessStatus.Denied or FolderAccessStatus.Error
            ? UiTheme.Danger
            : info.AccessStatus == FolderAccessStatus.Partial ? Color.FromArgb(217, 119, 6) : UiTheme.TextSecondary;
        _analyzerExtensionValue.Text = $"文件类型：{FormatExtensionSummary(info)}";
        _analyzerRelatedValue.Text = "正在读取关联变更…";
        var version = ++_analyzerScanVersion;
        _ = LoadAnalyzerRelationsAsync(info.Path, version);
    }

    private async Task LoadAnalyzerRelationsAsync(string path, int version)
    {
        List<FileChangeRecord> records;
        try
        {
            records = await Task.Run(() => _databaseService.GetChangeRecordsUnderPath(path, 50));
        }
        catch (Exception ex)
        {
            records = new List<FileChangeRecord>();
            if (version == _analyzerScanVersion) _analyzerRelatedValue.Text = $"读取失败：{ex.Message}";
            return;
        }

        if (version != _analyzerScanVersion || IsDisposed) return;
        var builder = new StringBuilder();
        foreach (var record in records.Take(8))
        {
            var process = string.IsNullOrWhiteSpace(record.SourceProcess) ? "未知进程" : record.SourceProcess;
            builder.AppendLine($"{record.Timestamp:MM-dd HH:mm}  {process}  {record.FileName}");
        }
        _analyzerRelatedValue.Text = builder.Length == 0 ? "暂无关联变更记录" : builder.ToString().TrimEnd();
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
        _analyzerAccessValue.Text = $"访问状态：{permission.Status} · ACL {permission.RuleCount} 条 · {ownerText}";
        _analyzerAccessValue.ForeColor = permission.CanRead ? UiTheme.Success : UiTheme.Danger;
    }

    private static string FormatExtensionSummary(FolderSizeInfo info)
    {
        if (info.ExtensionSizes.Count == 0) return "暂无文件";
        return string.Join("、", info.ExtensionSizes
            .OrderByDescending(pair => pair.Value)
            .Take(4)
            .Select(pair => $"{pair.Key} {FormatHelper.FormatBytes(pair.Value)}"));
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
}
