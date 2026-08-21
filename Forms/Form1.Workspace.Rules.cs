using CdiskClean.Helpers;

namespace CdiskClean;

/// <summary>工作区「监控规则」页逻辑：目录/进程视图切换与手动添加忽略进程</summary>
public partial class Form1
{
    private void ShowRulesView(bool directories)
    {
        rulesDirectoryView.Visible = directories;
        rulesProcessView.Visible = !directories;
        if (directories) rulesDirectoryView.BringToFront();
        else rulesProcessView.BringToFront();

        SetTabActive(rulesDirectoryTab, directories);
        SetTabActive(rulesProcessTab, !directories);
    }

    /// <summary>子页签按钮选中态：激活为蓝底白字，否则白底</summary>
    private static void SetTabActive(Button button, bool active)
    {
        button.BackColor = active ? UiTheme.Primary : Color.White;
        button.ForeColor = active ? Color.White : UiTheme.TextPrimary;
    }

    private void AddManualIgnoreProcess()
    {
        var processName = manualProcessInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(processName))
        {
            MessageBox.Show("请输入进程名", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        AddIgnoreProcessInternal(processName);
        manualProcessInput.Text = string.Empty;
        RefreshDashboardMetrics();
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void rulesDirectoryTab_Click(object? sender, EventArgs e) => ShowRulesView(true);
    private void rulesProcessTab_Click(object? sender, EventArgs e) => ShowRulesView(false);
    private void rulesProcessAddButton_Click(object? sender, EventArgs e) => AddManualIgnoreProcess();
}
