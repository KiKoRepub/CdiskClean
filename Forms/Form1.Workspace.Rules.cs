using CdiskClean.Helpers;

namespace CdiskClean;

/// <summary>工作区「监控规则」页逻辑：目录/进程视图切换与手动添加忽略进程</summary>
public partial class Form1
{
    private void ShowRulesView(bool directories)
    {
        rulesDirectoryView.Visible = directories;
        rulesIgnoreProcessView.Visible = !directories;
        rulesExeProcessView.Visible = false;
        if (directories) rulesDirectoryView.BringToFront();
        else rulesIgnoreProcessView.BringToFront();

        SetTabActive(rulesDirectoryTab, directories);
        SetTabActive(rulesProcessTab, !directories);
        SetTabActive(rulesExeTab, false);
    }

    private void ShowExeProcView(bool show)
    {
        rulesExeProcessView.Visible = show;
        rulesDirectoryView.Visible = !show;
        rulesIgnoreProcessView.Visible = !show;

        if (show) rulesExeProcessView.BringToFront();
        else if (rulesDirectoryView.Visible) rulesDirectoryView.BringToFront();
        else rulesIgnoreProcessView.BringToFront();


        SetTabActive(rulesExeTab, show);
        SetTabActive(rulesProcessTab, !show);
        SetTabActive(rulesDirectoryTab, !show);
    }
    /// <summary>子页签按钮选中态：激活为主题色，否则默认态</summary>
    private static void SetTabActive(AntdUI.Button button, bool active)
    {
        button.Type = active ? AntdUI.TTypeMini.Primary : AntdUI.TTypeMini.Default;
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

    private void rulesExeTab_Click(object sender, EventArgs e) => ShowExeProcView(true);


}
