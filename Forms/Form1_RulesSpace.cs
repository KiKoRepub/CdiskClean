using CdiskClean.Forms;
using CdiskClean.Models;
using CdiskClean.Helpers;
using CdiskClean.Models.rules;

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


    public void  BindRulesTableCenter() 
    { 
        // 应用程序

        rulesExeProcViewTable.Columns = new AntdUI.ColumnCollection
        {
            MakeColumn("ExeName", "应用程序名", "15%", AntdUI.ColumnAlign.Center),
            MakeColumn("DisplayPath", "路径", "30%", AntdUI.ColumnAlign.Left),
            MakeColumn("SizeBytes", "大小", "20%", AntdUI.ColumnAlign.Center),
            MakeColumn("RunningState", "运行状态", "10%", AntdUI.ColumnAlign.Center),
            MakeColumn("MonitoringState", "监测状态", "10%", AntdUI.ColumnAlign.Center),
        };

        rulesExeProcViewTable.Columns["SizeBytes"]?.SetRender((value, record, _) =>
            record is WatchingExeInfo app ? app.SizeText : value);
        rulesExeProcViewTable.Columns["MonitoringState"]?.SetRender((value, record, _) =>
            record is WatchingExeInfo app ? app.MonitoringState : value);

        var keyword = input1.Text.Trim();
        var rows = _monitorService.WatchingApplications
            .Where(application => keyword.Length == 0
                || application.ExeName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || application.FullPath.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(application =>
            {
                application.RunningState = application.UsesProcessIdentity
                    ? (System.Diagnostics.Process.GetProcessesByName(application.ExeName).Length > 0 ? "运行中" : "未运行")
                    : (File.Exists(application.FullPath) ? "可用" : "文件不存在");
                return application;
            })
            .ToList();
        rulesExeProcViewTable.DataSource = new System.ComponentModel.BindingList<WatchingExeInfo>(rows);
        rulesExeProcViewTable.Refresh();

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

    #region 监视目录列表 相关操作
    private void dirAddButton_Click(object? sender, EventArgs e)
    {

        DialogResult result = ImportFolderDialog.ShowDialog();

        if (result == DialogResult.OK)
        {
            string selectedPath = ImportFolderDialog.SelectedPath;
            var newDir = new WatchingDirectory(selectedPath, true);
            try
            {
                _databaseService.Rules.SaveWatchDirectory(newDir);
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

    private void PopulateDirListView()
    {
        watcherDirListView.Items.Clear();

        _monitorService.WatchDirectories.ForEach(addWatchingToListView);
        RefreshDashboardMetrics();
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
        // 仅保留选中状态（原"选中目录为"提示框已随旧界面移除）
    }

    private void watcherDirListView_Resize(object sender, EventArgs e)
    {
        int totalWidth = watcherDirListView.Width;
        watcherDirListView.Columns[0].Width = (int)(totalWidth * 0.70);
        watcherDirListView.Columns[1].Width = (int)(totalWidth * 0.15);
        watcherDirListView.Columns[2].Width = (int)(totalWidth * 0.15);
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
        if (e.Button != MouseButtons.Right) return;

        var item = watcherDirListView.GetItemAt(e.X, e.Y);
        if (item?.Tag is not WatchingDirectory dir) return;
        if (dir.Status == RecordStatusEnum.DELETED) return;

        BuildStatusContextMenu(dir.Status, status => ChangeDirStatus(dir, status))
            .Show(watcherDirListView, e.Location);
    }

    private void ChangeDirStatus(WatchingDirectory dir, RecordStatusEnum newStatus)
    {
        try
        {
            if (newStatus == RecordStatusEnum.DELETED)
            {
                _databaseService.Rules.DeleteWatchDirectory(dir.Path);
            }
            else
            {
                _databaseService.Rules.SaveWatchDirectory(new WatchingDirectory(dir.Path, dir.IncludeSubdirs)
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



    private void betterDirAddButton_Click(object? sender, EventArgs e)
    {
        using var form = new BetterDirAddForm();
        if (form.ShowDialog() != DialogResult.OK) return;

        var addedCount = 0;
        foreach (var path in form.SelectedPaths)
        {
            // 已在监视列表中（非删除状态）的路径跳过
            if (_monitorService.WatchDirectories.Any(d =>
                    string.Equals(d.Path, path, StringComparison.OrdinalIgnoreCase) &&
                    d.Status != RecordStatusEnum.DELETED))
                continue;

            try
            {
                _databaseService.Rules.SaveWatchDirectory(new WatchingDirectory(path, true));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存监测目录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                continue;
            }

            var dir = _monitorService.AddDirectoryToEtwArr(path, true);
            _monitorService.SetDirectoryStatus(dir.Path, RecordStatusEnum.USING);
            addedCount++;
        }

        PopulateDirListView();
        if (addedCount == 0)
        {
            MessageBox.Show("所选路径已在监测列表中。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void betterProcessAddButton_Click(object? sender, EventArgs e)
    {
        using var form = new ProcessPickForm();
        if (form.ShowDialog() != DialogResult.OK) return;

        foreach (var name in form.SelectedProcessNames)
            AddIgnoreProcessInternal(name);
    }

    #endregion


    #region 忽略进程列表 相关操作
    private void SetupProcessListView()
    {
        int totalWidth = ignoreProcessListView.Width;

        ignoreProcessListView.View = View.Details;
        ignoreProcessListView.FullRowSelect = true;
        ignoreProcessListView.MultiSelect = false;
        ignoreProcessListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

        // 设置内部列宽度为总宽度的比例
        ignoreProcessListView.Columns.Add("进程名称", (int)(totalWidth * 0.80));
        ignoreProcessListView.Columns.Add("状态", (int)(totalWidth * 0.20));


        // 开启双缓冲，减少闪烁
        typeof(ListView).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
            null, ignoreProcessListView, new object[] { true });
    }

    private void PopulateProcessListView()
    {
        ignoreProcessListView.Items.Clear();
        foreach (var proc in _monitorService.IgnoreProcessRecords)
        {
            var item = new ListViewItem(proc.ProcessName);
            item.SubItems.Add(EnumHelper.FormatStatus(proc.Status));
            item.Tag = proc;

            StyleHelper.ApplyRecordStatusStyle(item, proc.Status);
            ignoreProcessListView.Items.Add(item);
        }
        RefreshDashboardMetrics();
    }

    private void ignoreProcessView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
        // 仅保留选中状态（原"选中进程为"提示框已随旧界面移除）
    }

    private void ignoreProcessView_Resize(object sender, EventArgs e)
    {
        int totalWidth = ignoreProcessListView.Width;
        ignoreProcessListView.Columns[0].Width = (int)(totalWidth * 0.80);
        ignoreProcessListView.Columns[1].Width = (int)(totalWidth * 0.20);
    }


    // ==================== 忽略进程右键菜单 ====================

    private void SetupProcessContextMenu()
    {
        ignoreProcessListView.MouseClick += ignoreProcessView_MouseClick;
    }

    private void ignoreProcessView_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;

        var item = ignoreProcessListView.GetItemAt(e.X, e.Y);
        if (item?.Tag is not IgnoreProcessRecord proc) return;
        if (proc.Status == RecordStatusEnum.DELETED) return;

        BuildStatusContextMenu(proc.Status, status => ChangeProcessStatus(proc, status))
            .Show(ignoreProcessListView, e.Location);
    }

    /// <summary>按记录状态构建统一的"禁用/启用/删除"右键菜单</summary>
    private static System.Windows.Forms.ContextMenuStrip BuildStatusContextMenu(
        RecordStatusEnum status,
        Action<RecordStatusEnum> changeStatus)
    {
        var menu = new System.Windows.Forms.ContextMenuStrip();

        if (status == RecordStatusEnum.USING)
            menu.Items.Add("禁用监测").Click += (_, _) => changeStatus(RecordStatusEnum.FORBIDDEN);
        else if (status == RecordStatusEnum.FORBIDDEN)
            menu.Items.Add("启用监测").Click += (_, _) => changeStatus(RecordStatusEnum.USING);

        menu.Items.Add("从列表删除").Click += (_, _) => changeStatus(RecordStatusEnum.DELETED);
        return menu;
    }

    private void ChangeProcessStatus(IgnoreProcessRecord proc, RecordStatusEnum newStatus)
    {
        try
        {
            if (newStatus == RecordStatusEnum.DELETED)
            {
                _databaseService.Rules.DeleteIgnoreProcessRecord(proc.ProcessName);
            }
            else
            {
                _databaseService.Rules.SaveIgnoreProcessRecord(new IgnoreProcessRecord(proc.ProcessName)
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
            _databaseService.Rules.SaveIgnoreProcessRecord(record);
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
        foreach (ListViewItem item in ignoreProcessListView.Items)
        {
            if (string.Equals(item.Text, processName, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                ignoreProcessListView.EnsureVisible(item.Index);
                break;
            }
        }
    }

    #endregion

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

    private void rulesExeProcInput_TextChanged(object? sender, EventArgs e) => BindRulesTableCenter();

    private void rulesExeProcAddButton_Click(object? sender, EventArgs e)
    {
        var value = input1.Text.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            MessageBox.Show("请输入应用程序名或可执行文件路径。", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var application = _monitorService.AddWatchingApplication(value);
            _databaseService.Rules.SaveWatchingApplication(application);
            input1.Text = string.Empty;
            BindRulesTableCenter();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"添加监控应用失败: {ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void rulesExeProcSelectButton_Click(object? sender, EventArgs e)
    {
        using var form = new ProcessPickForm();
        if (form.ShowDialog() != DialogResult.OK) return;
        foreach (var processName in form.SelectedProcessNames)
        {
            try
            {
                var application = _monitorService.AddWatchingApplication(processName);
                _databaseService.Rules.SaveWatchingApplication(application);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加监控应用失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;
            }
        }
        BindRulesTableCenter();
    }

    private void rulesExeProcViewTable_CellClick(object? sender, AntdUI.TableClickEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.Record is not WatchingExeInfo application) return;
        rulesExeProcViewTable.SetSelected(application);
        var menu = new System.Windows.Forms.ContextMenuStrip();
        var nextStatus = application.Status == RecordStatusEnum.USING
            ? RecordStatusEnum.FORBIDDEN : RecordStatusEnum.USING;
        menu.Items.Add(nextStatus == RecordStatusEnum.USING ? "启用监控" : "暂停监控").Click += (_, _) =>
        {
            application.Status = nextStatus;
            _databaseService.Rules.SaveWatchingApplication(application);
            _monitorService.SetWatchingApplicationStatus(application.FullPath, nextStatus);
            BindRulesTableCenter();
        };
        menu.Items.Add("从列表删除").Click += (_, _) =>
        {
            _databaseService.Rules.DeleteWatchingApplication(application.FullPath);
            _monitorService.SetWatchingApplicationStatus(application.FullPath, RecordStatusEnum.DELETED);
            BindRulesTableCenter();
        };
        menu.Show(Cursor.Position);
    }
}
