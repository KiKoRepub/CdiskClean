using AntdUI;
using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.cleanUp;
using CdiskClean.Services;
using System.Diagnostics;

namespace CdiskClean;

/// <summary>工作区「清理中心」页逻辑：清理方式面板的动态布局</summary>
public partial class Form1
{
    /// <summary>清理方式单选按钮与枚举映射（初始化于 SetupCleanPage）</summary>
    private (Radio Radio, CleanupMethod Method)[] _cleanupMethodRadios = Array.Empty<(Radio, CleanupMethod)>();

    /// <summary>按面板宽度动态排列清理方式区控件（单选、目标目录、清理按钮）</summary>
    private void LayoutCleanupMethodPanel(Control panel)
    {
        if (_cleanupMethodRadios.Length == 0) return;
        var width = Math.Max(250, panel.ClientSize.Width);
        var y = 46;
        foreach (var (radio, _) in _cleanupMethodRadios)
        {
            radio.SetBounds(4, y, width - 8, 27);
            y += 29;
        }

        cleanTargetLabel.SetBounds(4, y + 3, width - 8, 24);
        y += 29;
        cleanTargetTextBox.SetBounds(4, y, Math.Max(120, width - 92), 32);
        cleanTargetSelectBtn.SetBounds(width - 80, y - 1, 76, 34);
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void cleanupMethodPanel_Resize(object? sender, EventArgs e)
    {
        if (sender is Control panel)
            LayoutCleanupMethodPanel(panel);
    }


    #region 磁盘清理
    // ==================== 磁盘清理 ====================

    private CancellationTokenSource? _cleanScanCts;
    private CancellationTokenSource? _cleanExecCts;
    private bool _treeUpdating;

    /// <summary>树节点数量上限，超过则仅显示目录节点（文件通过勾选目录整体清理）</summary>
    private const int MaxCleanTreeNodes = 50000;

    private void SetupCleanPage()
    {
        _cleanupMethodRadios = new[]
        {
                (cleanRecycleRadio, CleanupMethod.RecycleBin),
                (cleanPermanentRadio, CleanupMethod.PermanentDelete),
                (cleanMoveRadio, CleanupMethod.Move),
                (cleanCompressRadio, CleanupMethod.Compress),
                (cleanMklinkRadio, CleanupMethod.Mklink)
            };
        SetupFrequentListView();
        LayoutCleanupMethodPanel(cleanupMethodPanel);
        UpdateTargetBoxState();
        RefreshFrequentPaths();
        RefreshCleanHistory();
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

    /// <summary>从变更记录中统计高频修改目录，展示在左侧参考列表（历史记录读取在后台执行，避免卡 UI）</summary>
    private async void RefreshFrequentPaths()
    {
        List<FileChangeRecord> snapshot;
        lock (_recordsLock)
        {
            snapshot = _records.ToList();
        }

        List<FileChangeRecord> dbRecords;
        try
        {
            dbRecords = await Task.Run(() => _databaseService.GetChangeRecords(5000));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"读取历史变更记录失败: {ex.Message}");
            dbRecords = new List<FileChangeRecord>();
        }

        var seen = new HashSet<string>(
            snapshot.Select(GetChangeRecordKey),
            StringComparer.OrdinalIgnoreCase);
        foreach (var record in dbRecords)
        {
            if (seen.Add(GetChangeRecordKey(record)))
                snapshot.Add(record);
        }

        var paths = CleanupService.GetFrequentPaths(snapshot, 30);
        ApplyFrequentPaths(paths);
    }

    private void ApplyFrequentPaths(List<FrequentPathInfo> paths)
    {
        if (IsDisposed) return;
        // 句柄尚未创建（构造期异步完成时）挂到 Load 后执行；非 UI 线程则回传 UI 线程
        if (!IsHandleCreated)
        {
            Load += (_, _) => ApplyFrequentPaths(paths);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => ApplyFrequentPaths(paths));
            return;
        }

        frequentPathListView.BeginUpdate();
        frequentPathListView.Items.Clear();
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
        using var dialog = new System.Windows.Forms.FolderBrowserDialog
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
        cleanTreeView.Items.Clear();

        try
        {
            var entries = await _cleanupService.ScanDirectoryAsync(path, cts.Token);
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
                ? $"扫描完成：{fileCount} 个文件，共 {FormatHelper.FormatBytes(totalSize)}（文件过多仅显示目录，可勾选目录整体清理）"
                : $"扫描完成：{fileCount} 个文件，共 {FormatHelper.FormatBytes(totalSize)}";
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
        var rootItem = await Task.Run(() =>
            BuildCleanTreeItems(entries, rootFull, totalSize, dirOnly), ct);

        cleanTreeView.Items.Clear();
        cleanTreeView.Items.Add(rootItem);
        rootItem.Expand = true;
        UpdateCleanupSelectionSummary();
    }

    /// <summary>
    /// 在后台线程构建节点树（TreeItem 无句柄，可跨线程构建父子关系，挂载后再由 UI 线程渲染）。
    /// 跳过扫描根目录自身（entries 首项即根），避免树中出现重复根节点。
    /// </summary>
    private static TreeItem BuildCleanTreeItems(
        List<CleanupFileEntry> entries,
        string rootFull,
        long totalSize,
        bool dirOnly)
    {
        var rootName = Path.GetFileName(rootFull);
        var rootItem = new TreeItem
        {
            Text = rootName,
            SubTitle = FormatHelper.FormatBytes(totalSize),
            Tag = rootFull,
            Checkable = false,
            Expand = true
        };

        // 目录节点（扫描顺序保证父目录先于子目录）
        var dirNodes = new Dictionary<string, TreeItem>(StringComparer.OrdinalIgnoreCase)
        {
            [rootFull] = rootItem
        };

        foreach (var entry in entries.Where(e => e.IsDirectory))
        {
            var entryFull = entry.FullPath.TrimEnd('\\');
            if (string.Equals(entryFull, rootFull, StringComparison.OrdinalIgnoreCase))
                continue;

            var item = CreateFileTreeItem(entry);
            dirNodes[entryFull] = item;

            var parentDir = Path.GetDirectoryName(entryFull) ?? "";
            if (dirNodes.TryGetValue(parentDir.TrimEnd('\\'), out var parent))
                parent.Sub.Add(item);
            else
                rootItem.Sub.Add(item);
        }

        if (!dirOnly)
        {
            // 文件节点
            foreach (var entry in entries.Where(e => !e.IsDirectory))
            {
                var item = CreateFileTreeItem(entry);

                var parentDir = Path.GetDirectoryName(entry.FullPath) ?? "";
                if (dirNodes.TryGetValue(parentDir.TrimEnd('\\'), out var parent))
                    parent.Sub.Add(item);
                else
                    rootItem.Sub.Add(item);
            }
        }

        SortCleanTreeItems(rootItem.Sub);
        return rootItem;
    }

    /// <summary>目录在前，其余按大小降序排列</summary>
    private static void SortCleanTreeItems(TreeItemCollection items)
    {
        foreach (var item in items)
            SortCleanTreeItems(item.Sub);

        if (items.Count < 2) return;

        var list = items.ToArray();
        // 目录在前，其余按大小降序排列
        Array.Sort(list, (a, b) =>
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

        items.Clear();
        foreach (var item in list)
            items.Add(item);
    }

    /// <summary>
    /// 创建文件/目录的TreeItem
    /// </summary>
    private static TreeItem CreateFileTreeItem(CleanupFileEntry entry)
    {
        var item = new TreeItem
        {
            Text = entry.Name,
            SubTitle = FormatHelper.FormatBytes(entry.SizeBytes),
            Tag = entry,
            Checkable = true
        };

        // 设置图标
        if (entry.IsDirectory)
        {
            item.SetIcon("FolderOutlined");
        }
        else
        {
            item.SetIcon("FileOutlined");
        }

        // 设置风险等级背景色（简化逻辑：根据文件扩展名判断）
        item.SetBack(GetRiskLevelColor(entry));

        return item;
    }

    /// <summary>
    /// 节点风险等级背景色：目录按常见风险目录清单（段匹配），文件按扩展名，规则见 RiskLevelHelper。
    /// 红色=高风险，黄色=中风险，绿色=低风险
    /// </summary>
    private static Color GetRiskLevelColor(CleanupFileEntry entry) =>
        RiskLevelHelper.GetColor(entry.IsDirectory
            ? RiskLevelHelper.GetDirectoryRisk(entry.FullPath)
            : RiskLevelHelper.GetFileRisk(entry.FullPath));

    /// <summary>勾选状态变更事件</summary>
    private void cleanTreeView_CheckedChanged(object? sender, TreeCheckedEventArgs e)
    {
        if (_treeUpdating) return;
        if (e.Item == null) return;

        _treeUpdating = true;
        try
        {
            // AntdUI.Tree 在 CheckStrictly=false 时自动处理父子关联
            // 这里只需要更新选中摘要



        }
        finally
        {
            _treeUpdating = false;
        }
        UpdateCleanupSelectionSummary();
    }

    // ==================== 清理树交互（p017 task2） ====================

    /// <summary>节点信息气泡（单击节点显示创建时间），懒创建</summary>
    private NodeInfoPopover? _nodeInfoPopover;

    /// <summary>
    /// 单击节点文本：在节点位置弹出创建时间信息框。
    /// SelectChanged 仅在左键点击节点文本区时触发（勾选框/箭头不会误触）。
    /// </summary>
    private void cleanTreeView_SelectChanged(object? sender, AntdUI.TreeSelectEventArgs e)
    {
        if (e.Item == null) return;
        TreeItem item = e.Item;
        CleanupFileEntry entry =  item.Tag as CleanupFileEntry;
          
        string toShow = "创建时间：" + (entry.IsDirectory 
                ?  Directory.GetCreationTime(entry.FullPath).ToString() 
                : File.GetCreationTime(entry.FullPath).ToString());
        if (!entry.LastWriteTime.IsNull())
        {
            toShow += "\n修改时间：" + entry.LastWriteTime.ToString();
        }

        Popover.open(cleanTreeView, entry.Name, toShow, TAlign.Top);



    }

    /// <summary>滚动清理树时收起信息框，避免气泡位置与节点错位</summary>
    private void cleanTreeView_MouseWheel(object? sender, MouseEventArgs e)
    {
        _nodeInfoPopover?.Hide();
    }

    /// <summary>
    /// 右键节点：弹出「选中」菜单，执行逻辑与勾选框一致（勾选并级联子节点）。
    /// TreeSelectEventArgs 继承 MouseEventArgs，e.Button 可直接判断右键。
    /// </summary>
    private void cleanTreeView_NodeMouseClick(object? sender, AntdUI.TreeSelectEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.Item == null) return;

        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("选中", null, (_, _) =>
        {
            e.Item.SetChecked(true);
            e.Item.CheckedStrictly(true, true);
            UpdateCleanupSelectionSummary();
        });
        menu.Show(Cursor.Position);
    }

    /// <summary>目录节点展开/收起时切换图标（收起态 FolderOutlined 为初始值）</summary>
    private void cleanTreeView_AfterExpand(object? sender, AntdUI.TreeCheckedEventArgs e)
    {
        // 根节点 Tag 为路径字符串（必为目录），其余目录为 CleanupFileEntry
        if (e.Item?.Tag is not (string or CleanupFileEntry { IsDirectory: true })) return;
        e.Item.SetIcon(e.Value ? "FolderOpenOutlined" : "FolderOutlined");
    }
    private static void GetCheckedItems(TreeItemCollection items, List<CleanupFileEntry> list)
    {
        foreach (var item in items)
        {
            if (item.Checked && item.Tag is CleanupFileEntry entry)
            {
                // 如果是目录且不是部分勾选状态，则整体添加
                if (entry.IsDirectory && item.CheckState != CheckState.Indeterminate)
                {
                    list.Add(entry);
                }
                else if (!entry.IsDirectory)
                {
                    // 文件直接添加
                    list.Add(entry);
                }
            }
            else
            {
                // 递归检查子节点
                GetCheckedItems(item.Sub, list);
            }
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
        if (cleanTreeView.Items.Count == 0) return;

        _treeUpdating = true;
        try
        {
            foreach (var item in cleanTreeView.Items[0].Sub)
            {
                item.SetChecked(check);
                item.CheckedStrictly(check, true);
            }
        }
        finally
        {
            _treeUpdating = false;
        }
        UpdateCleanupSelectionSummary();
    }

    private void cleanTargetSelectBtn_Click(object? sender, EventArgs e)
    {
        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "选择清理操作的目标目录",
            ShowNewFolderButton = true
        };

        if (dialog.ShowDialog() == DialogResult.OK)
            cleanTargetTextBox.Text = dialog.SelectedPath;
    }

    private void cleanMethodRadio_CheckedChanged(object sender, BoolEventArgs e)
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
        foreach (var (radio, method) in _cleanupMethodRadios)
        {
            if (radio.Checked) return method;
        }
        return CleanupMethod.RecycleBin;
    }

    private List<CleanupFileEntry> GetCheckedEntries()
    {
        var list = new List<CleanupFileEntry>();
        if (cleanTreeView.Items.Count > 0)
            GetCheckedItems(cleanTreeView.Items[0].Sub, list);
        return list;
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
            if (!string.IsNullOrEmpty(basePath) && PathHelper.IsPathInside(targetDir, basePath))
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
                $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）移入回收站吗？",
            CleanupMethod.PermanentDelete =>
                $"确定要永久删除选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）吗？\n\n此操作不可恢复！",
            CleanupMethod.Move =>
                $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）移动到：\n{targetDir}\n\n吗？",
            CleanupMethod.Compress =>
                $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）压缩到：\n{targetDir}\n\n并删除原文件吗？",
            CleanupMethod.Mklink =>
                $"确定要将选中的 {entries.Count} 项（共 {FormatHelper.FormatBytes(totalSize)}）迁移到：\n{targetDir}\n\n并在原位置创建软链接吗？",
            _ => $"确定要清理选中的 {entries.Count} 项吗？"
        };

        if (MessageBox.Show(confirmText, $"确认清理（{methodName}）",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        _cleanExecCts = new CancellationTokenSource();
        var cts = _cleanExecCts;

        cleanButton.Enabled = false;
        cleanButton.Text = "取消清理";
        cleanScanProgressBar.Style = ProgressBarStyle.Marquee;
        var progress = new Progress<string>(s => cleanStatusLabel.Text = s);

        // 按待清理文件所在盘统计释放空间（清理对象可能不在 C 盘）
        var freedDriveRoot = entries[0].FullPath.Length >= 2
            ? Path.GetPathRoot(entries[0].FullPath)
            : null;
        long freeBefore = GetFreeSpaceSafe(freedDriveRoot);
        try
        {
            var result = await _cleanupService.ExecuteAsync(entries, method, targetDir, progress, cts.Token);
            long freedDelta = Math.Max(0, GetFreeSpaceSafe(freedDriveRoot) - freeBefore);

            var summary = $"清理完成：成功 {result.Success} 项，失败 {result.Fail} 项";
            if (method is CleanupMethod.PermanentDelete or CleanupMethod.Compress)
                summary += $"\n释放空间约 {FormatHelper.FormatBytes(freedDelta)}";
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
            cleanButton.Enabled = true;
            cleanButton.Text = "清理选中文件";
            cleanScanProgressBar.Style = ProgressBarStyle.Blocks;
        }
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
            CleanupMethod.PermanentDelete => $"已永久删除 {result.Success} 项，释放约 {FormatHelper.FormatBytes(freedDelta)}",
            CleanupMethod.Move => $"已将 {result.Success} 项移动到目标目录",
            CleanupMethod.Compress =>
                $"已压缩 {result.Success} 项到目标目录，选中文件总大小 {FormatHelper.FormatBytes(selectedTotalSize)}",
            CleanupMethod.Mklink => $"已将 {result.Success} 项迁移至目标目录并创建软链接",
            _ => $"清理完成，成功 {result.Success} 项"
        };
        if (result.Fail > 0) msg += $"，失败 {result.Fail} 项";

        notifyIcon1.ShowBalloonTip(3000, "清理完成", msg, ToolTipIcon.Info);
    }

    private long GetFreeSpaceSafe(string? driveRoot = null)
    {
        try
        {
            var drive = string.IsNullOrEmpty(driveRoot) ? "C:" : driveRoot;
            return _diskSpaceService.GetDriveInfo(drive).FreeSpaceBytes;
        }
        catch { return 0; }
    }

    private void RefreshCleanHistory()
    {
        var records = _databaseService.GetCleanupRecords(200);
        cleanHistoryTable.DataSource = records;
        cleanHistoryEmptyLabel.Visible = records.Count == 0;

        // 列配置统一在 ConfigureTableColumns() 中完成，此处只需绑定数据
    }

    /// <summary>
    /// 清理历史表格右键菜单：选中行后弹出（回收站记录可定位回收站 / 复制路径）。
    /// AntdUI.Table 无 CellContextMenuStripNeeded，改用 CellClick 监听右键。
    /// </summary>
    private void cleanHistoryGrid_CellClick(object? sender, AntdUI.TableClickEventArgs e)
    {
        if (e.Button != MouseButtons.Right || e.Record is not CleanupRecord record)
            return;

        cleanHistoryTable.SetSelected(record);

        var menu = new System.Windows.Forms.ContextMenuStrip();

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

        menu.Show(Cursor.Position);
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
    #endregion
}
