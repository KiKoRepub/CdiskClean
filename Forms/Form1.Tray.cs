namespace CdiskClean;

/// <summary>托盘动画、显示窗口和监控模式切换。</summary>
public partial class Form1
{
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

    private bool _modeSwitching;

    private void defaultModeRadio_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
    {
        if (_modeSwitching) return;
        if (defaultModeRadio.Checked) SwitchMonitorMode(false);
        
    }

    private void exeModeRadio_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
    {
        if (_modeSwitching) return;
        if (exeModeRadio.Checked) SwitchMonitorMode(true);
        else if (_monitorService.IsRunning && defaultModeRadio.Checked)
        {
            // 尝试 在运行的时候 切换到默认模式，提示用户是否继续
            SwitchMonitorMode(false);
        }
    }

    /// <summary>统一切换监测模式：监测运行中需先暂停；同步界面单选与托盘菜单</summary>
    private void SwitchMonitorMode(bool toExeMode)
    {
        if (_monitorService.IsRunning)
        {
            MessageBox.Show("需要暂停监测才能选择模式。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            SyncModeUi(!toExeMode);
            return;
        }

        if (toExeMode) _monitorService.EnableExeMode();
        else _monitorService.EnableDefaultMode();
        SyncModeUi(toExeMode);
    }

    /// <summary>将监测模式 UI（单选按钮 + 托盘菜单）同步到指定模式</summary>
    private void SyncModeUi(bool exeMode)
    {
        _modeSwitching = true;
        try
        {
            if (defaultModeRadio.Checked != !exeMode) defaultModeRadio.Checked = !exeMode;
            if (exeModeRadio.Checked != exeMode) exeModeRadio.Checked = exeMode;
            默认ToolStripMenuItem.Checked = !exeMode;
            进程模式ToolStripMenuItem.Checked = exeMode;
        }
        finally
        {
            _modeSwitching = false;
        }
    }

    private void 默认ToolStripMenuItem_Click(object? sender, EventArgs e) => SwitchMonitorMode(false);

    private void 进程模式ToolStripMenuItem_Click(object? sender, EventArgs e) => SwitchMonitorMode(true);

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;

    }
    #endregion
}
