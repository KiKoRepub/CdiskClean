using AntdUI;
using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.rules;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>工作区「实时活动」页：布局已在设计器维护，本文件留空备用</summary>
public partial class Form1
{




    private void BindActivityCenter(BindingList<FileChangeRecord> records)
    {
        if (IsDisposed) return;
        // 句柄尚未创建（构造期异步完成时）挂到 Load 后执行；非 UI 线程则回传 UI 线程
        if (!IsHandleCreated)
        {
            Load += (_, _) => BindActivityCenter(records);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => BindActivityCenter(records));
            return;
        }


        activityRecordTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("Timestamp", "时间", "20%", AntdUI.ColumnAlign.Center),
                MakeColumn("ChangeType", "类型", "10%", AntdUI.ColumnAlign.Center),
                MakeColumn("FileName", "文件名", "20%", AntdUI.ColumnAlign.Center),
                MakeColumn("FullPath", "路径", "25%", AntdUI.ColumnAlign.Left),
                MakeColumn("SizeBytes", "大小", "10%", AntdUI.ColumnAlign.Center),
                MakeColumn("SourceProcess", "来源进程", "10%", AntdUI.ColumnAlign.Center)
            };
        activityRecordTable.DataSource = records;

        activityRecordTable.Refresh();
        //MessageBox.Show("体现");
    }


    #region 实时监测
    // ==================== 实时监测 ====================

    private void pauseBtn_Click(object? sender, EventArgs e)
    {
        if (!_monitorService.IsRunning)
        {
            if (exeModeRadio.Checked)
            {
                if (!_monitorService.WatchingApplications.Any(application => application.Status == RecordStatusEnum.USING))
                {
                    MessageBox.Show("请先在“监控应用”中添加至少一个启用的应用程序。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                _monitorService.EnableExeMode();
            }
            else
            {
                _monitorService.EnableDefaultMode();
            }

            _monitorService.StartMonitor(string.Empty);
            if (!_monitorService.IsRunning) return;
            _notificationService.Start();
            notifyIcon1.Text = "C盘管理工具\r\n监测中";
        }
        else
        {
            _monitorService.Stop();
            _etwService.Stop();
            _notificationService.Stop();
            notifyIcon1.Text = "C盘管理工具\r\n已暂停";
        }

        workspaceMonitorToggleButton.Text = _monitorService.IsRunning ? "暂停监测" : "开始监测";
        workspaceMonitorToggleButton.Type = _monitorService.IsRunning
            ? AntdUI.TTypeMini.Error
            : AntdUI.TTypeMini.Primary;

        // 调整托盘菜单
        startMonitorNotifyItem.Text = workspaceMonitorToggleButton.Text;
        // 刷新工作区状态
        RefreshWorkspaceStatus();
        RefreshDashboardMetrics();

        startNotifyRotate(_monitorService.IsRunning);

    }

    private void startNotifyRotate(bool status)
    {
        if (status)
        {
            // 托盘实现 旋转
            notifyRotateTimer.Start();
            //notifyIcon1.Icon = Properties.Resources.leftRotate_1;
        }
        else
        {
            notifyRotateTimer.Stop();
            notifyIcon1.Icon = Properties.Resources.defaultIcon;
        }

    }

    private void clearBtn_Click(object? sender, EventArgs e)
    {
        _recordFlushTimer.Stop();
        lock (_recordsLock)
        {
            _pendingRecords.Clear();
            _records.Clear();
        }
        UpdateRecordCount();
        RefreshDashboardMetrics();
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

        // 先取出文件路径再释放对话框，避免后台任务访问已释放对象
        var exportPath = dialog.FileName;
        Task.Run(() =>
        {
            try
            {
                List<FileChangeRecord> snapshot;
                lock (_recordsLock)
                {
                    snapshot = _records.ToList();
                }

                using var writer = new StreamWriter(exportPath, false, System.Text.Encoding.UTF8);
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
                    MessageBox.Show($"已导出 {snapshot.Count} 条记录到:\n{exportPath}",
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
        var searchText = recordSearchBox.Text.Trim();
        var targetType = filterIndex switch
        {
            1 => ChangeType.Created,
            2 => ChangeType.Changed,
            3 => ChangeType.Deleted,
            4 => ChangeType.Renamed,
            _ => (ChangeType?)null
        };
        List<FileChangeRecord> snapshot;
        lock (_recordsLock) snapshot = _records.ToList();
        var filtered = snapshot.Where(record =>
            (!targetType.HasValue || record.ChangeType == targetType.Value) &&
            (searchText.Length == 0 || record.FileName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
             record.FullPath.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
             record.Directory.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
             (record.SourceProcess?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)))
            .ToList();
        if (IsHandleCreated && !IsDisposed)
        {
            activityRecordTable.DataSource = new BindingList<FileChangeRecord>(filtered);
            activityRecordTable.Refresh();
        }
        return;
        /*            var filterIndex = typeFilterCombo.SelectedIndex;
                    var searchText = recordSearchBox.Text.Trim();
                    var hasSearch = !string.IsNullOrWhiteSpace(searchText);
                    if (filterIndex <= 0 && !hasSearch)
                    {
                        // 已绑定 _records 时无需重新赋值，避免网格滚动/选择位置被重置
                        if (!_gridBoundToRecords)
                        {
                            changesDataGrid.DataSource = _records;
                            _gridBoundToRecords = true;
                        }
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

                    IEnumerable<FileChangeRecord> filtered = _records;
                    if (targetType.HasValue)
                        filtered = filtered.Where(r => r.ChangeType == targetType.Value);
                    if (hasSearch)
                    {
                        filtered = filtered.Where(r =>
                            r.FileName.Contains(searchText!, StringComparison.OrdinalIgnoreCase) ||
                            r.FullPath.Contains(searchText!, StringComparison.OrdinalIgnoreCase) ||
                            (r.SourceProcess?.Contains(searchText!, StringComparison.OrdinalIgnoreCase) ?? false));
                    }

                    changesDataGrid.DataSource = new BindingList<FileChangeRecord>(
                        filtered.ToList());
                    _gridBoundToRecords = false;*/
    }

    /*        private void changesDataGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
            {
                if (e.ColumnIndex == TypeColumn.Index && e.Value is ChangeType changeType)
                {
                    e.Value = EnumHelper.FormatChangeType(changeType);
                    e.FormattingApplied = true;
                }
            }*/

    private void OnFileChanged(FileChangeRecord record)
    {
        // DiskMonitorService 已完成延迟归因与忽略过滤；已知进程可直接进入提醒聚合。
        if (record.SourceProcess != null)
        {
            _notificationService.RecordChange(record);
            try { _databaseService.UpdateWatchingApplicationActivity(record.SourceProcess, record.Timestamp); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"更新应用监测活动失败: {ex.Message}"); }
        }

        BeginInvoke(() =>
        {
            lock (_recordsLock)
                _pendingRecords.Add(record);
            _recordFlushTimer.Start();
        });
    }

    /// <summary>把 150ms 窗口内到达的记录批量刷入网格，避免高频事件逐条刷新导致 UI 卡顿</summary>
    private void FlushPendingRecords()
    {
        _recordFlushTimer.Stop();

        List<FileChangeRecord> batch;
        lock (_recordsLock)
        {
            if (_pendingRecords.Count == 0) return;
            batch = new List<FileChangeRecord>(_pendingRecords);
            _pendingRecords.Clear();
        }

        lock (_recordsLock)
        {
            foreach (var record in batch)
            {
                _records.Insert(0, record);
                while (_records.Count > MaxRecords)
                    _records.RemoveAt(_records.Count - 1);
            }

            activityRecordTable.DataSource = _records.ToList();
            activityRecordTable.Refresh();
        }

        UpdateRecordCount();

        ApplyFilter();

            RefreshDashboardMetrics();
    }

    private void OnMonitorError(string message)
    {
        BeginInvoke(() =>
        {
            workspaceRecordStatus.Text = message;
            workspaceRecordStatus.ForeColor = UiTheme.Danger;
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
            RefreshDashboardInsightsAsync();
        });
    }

    private void UpdateRecordCount()
    {
        workspaceRecordStatus.Text = $"当前记录 {_records.Count:N0} 条";
        workspaceRecordStatus.ForeColor = UiTheme.TextSecondary;
    }
    #endregion


}
