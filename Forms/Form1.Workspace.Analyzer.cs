using CdiskClean.Helpers;
using CdiskClean.Models;

namespace CdiskClean;

/// <summary>工作区「空间分析」页逻辑：选中节点详情展示与带入清理流程</summary>
public partial class Form1
{
    private void folderTreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not FolderSizeInfo info) return;
        analyzerPathValue.Text = info.Path;
        analyzerSizeValue.Text = $"大小：{FormatHelper.FormatBytes(info.SizeBytes)}";
        analyzerFilesValue.Text = $"文件：{info.FileCount:N0}";
        analyzerFoldersValue.Text = $"子目录：{info.SubFolders.Count:N0}";
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

        cleanPathTextBox.Text = path;
        ShowWorkspacePage(CleanupPageId);
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void analyzerUseForCleanupButton_Click(object? sender, EventArgs e) => UseAnalyzerPathForCleanup();
}
