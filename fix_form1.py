#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import io, sys

p = "D:/university/CSharp/projects/CdiskClean/Forms/Form1.cs"
with io.open(p, encoding="utf-8") as f:
    s = f.read()

def rep(old, new, count=1):
    global s
    n = s.count(old)
    if n != count:
        print("WARN expected %d occurrences of:\n  %s\n got %d" % (count, old[:60], n))
    s = s.replace(old, new)

# ---------- Dir ListView ----------
old_dir_setup = '''        private void SetupDirListView()
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
        }'''
new_dir_setup = '''        private void SetupDirListView()
        {
            watcherDirListView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            watcherDirListView.ColumnCount = 3;
            watcherDirListView.Columns[0].HeaderText = "目录路径";
            watcherDirListView.Columns[1].HeaderText = "状态";
            watcherDirListView.Columns[2].HeaderText = "子目录";
            watcherDirListView.Columns[0].FillWeight = 70;
            watcherDirListView.Columns[1].FillWeight = 15;
            watcherDirListView.Columns[2].FillWeight = 15;
            watcherDirListView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            watcherDirListView.MultiSelect = false;
            watcherDirListView.RowHeadersVisible = false;
            watcherDirListView.AllowUserToAddRows = false;
        }'''
rep(old_dir_setup, new_dir_setup)

# PopulateDirListView + addWatchingToListView + Selection + Resize (dir)
old_dir_rest = '''        private void PopulateDirListView()
        {
            watcherDirListView.Items.Clear();

            _monitorService.WatchDirectories.ForEach(addWatchingToListView);

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
            if (watcherDirListView.SelectedItems.Count > 0)
            {
                ListViewItem item = watcherDirListView.SelectedItems[0];
                //MessageBox.Show(item.Text);
                dirSelectedTextBox.Text = item.Text;
            }
        }

        private void watcherDirListView_Resize(object sender, EventArgs e)
        {
            int totalWidth = watcherDirListView.Width;
            watcherDirListView.Columns[0].Width = (int)(totalWidth * 0.70);
            watcherDirListView.Columns[1].Width = (int)(totalWidth * 0.15);
            watcherDirListView.Columns[2].Width = (int)(totalWidth * 0.15);
        }'''
new_dir_rest = '''        private void PopulateDirListView()
        {
            watcherDirListView.Rows.Clear();

            _monitorService.WatchDirectories.ForEach(addWatchingToListView);
        }

        private void addWatchingToListView(WatchingDirectory dir)
        {
            // 根据路径 判重
            if (watcherDirListView.Rows.Cast<DataGridViewRow>()
                .Any(r => r.Cells[0].Value?.ToString() == dir.Path))
                return;

            int idx = watcherDirListView.Rows.Add(
                dir.Path,
                EnumHelper.FormatStatus(dir.Status),
                dir.IncludeSubdirs ? "是" : "否");
            var row = watcherDirListView.Rows[idx];
            row.Tag = dir;

            StyleHelper.ApplyRecordStatusStyle(row, dir.Status);
        }

        private void watcherDirListView_ItemSelectionChanged(object? sender, EventArgs e)
        {
            if (watcherDirListView.SelectedRows.Count > 0)
            {
                var row = watcherDirListView.SelectedRows[0];
                dirSelectedTextBox.Text = row.Cells[0].Value?.ToString();
            }
        }

        private void watcherDirListView_Resize(object sender, EventArgs e)
        {
            watcherDirListView.Columns[0].FillWeight = 70;
            watcherDirListView.Columns[1].FillWeight = 15;
            watcherDirListView.Columns[2].FillWeight = 15;
        }'''
rep(old_dir_rest, new_dir_rest)

# SetupProcessListView
old_proc_setup = '''        private void SetupProcessListView()
        {
            int totalWidth = ignoreProcessView.Width;

            ignoreProcessView.View = View.Details;
            ignoreProcessView.FullRowSelect = true;
            ignoreProcessView.MultiSelect = false;
            ignoreProcessView.HeaderStyle = ColumnHeaderStyle.Nonclickable;

            // 设置内部列宽度为总宽度的比例
            ignoreProcessView.Columns.Add("进程名称", (int)(totalWidth * 0.80));
            ignoreProcessView.Columns.Add("状态", (int)(totalWidth * 0.20));


            // 开启双缓冲，减少闪烁
            typeof(ListView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, ignoreProcessView, new object[] { true });
        }'''
new_proc_setup = '''        private void SetupProcessListView()
        {
            ignoreProcessView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ignoreProcessView.ColumnCount = 2;
            ignoreProcessView.Columns[0].HeaderText = "进程名称";
            ignoreProcessView.Columns[1].HeaderText = "状态";
            ignoreProcessView.Columns[0].FillWeight = 80;
            ignoreProcessView.Columns[1].FillWeight = 20;
            ignoreProcessView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ignoreProcessView.MultiSelect = false;
            ignoreProcessView.RowHeadersVisible = false;
            ignoreProcessView.AllowUserToAddRows = false;
        }'''
rep(old_proc_setup, new_proc_setup)

# PopulateProcessListView
old_proc_pop = '''        private void PopulateProcessListView()
        {
            ignoreProcessView.Items.Clear();
            foreach (var proc in _monitorService.IgnoreProcessRecords)
            {
                var item = new ListViewItem(proc.ProcessName);
                item.SubItems.Add(EnumHelper.FormatStatus(proc.Status));
                item.Tag = proc;

                StyleHelper.ApplyRecordStatusStyle(item, proc.Status);
                ignoreProcessView.Items.Add(item);
            }
        }

        private void ignoreProcessView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ignoreProcessView.SelectedItems.Count > 0)
            {
                ListViewItem item = ignoreProcessView.SelectedItems[0];

                procSelectedTextBox.Text = item.Text;
            }
        }


        private void ignoreProcessView_Resize(object sender, EventArgs e)
        {
            int totalWidth = ignoreProcessView.Width;
            ignoreProcessView.Columns[0].Width = (int)(totalWidth * 0.80);
            ignoreProcessView.Columns[1].Width = (int)(totalWidth * 0.20);
        }'''
new_proc_pop = '''        private void PopulateProcessListView()
        {
            ignoreProcessView.Rows.Clear();
            foreach (var proc in _monitorService.IgnoreProcessRecords)
            {
                int idx = ignoreProcessView.Rows.Add(proc.ProcessName, EnumHelper.FormatStatus(proc.Status));
                var row = ignoreProcessView.Rows[idx];
                row.Tag = proc;

                StyleHelper.ApplyRecordStatusStyle(row, proc.Status);
            }
        }

        private void ignoreProcessView_ItemSelectionChanged(object? sender, EventArgs e)
        {
            if (ignoreProcessView.SelectedRows.Count > 0)
            {
                var row = ignoreProcessView.SelectedRows[0];
                procSelectedTextBox.Text = row.Cells[0].Value?.ToString();
            }
        }


        private void ignoreProcessView_Resize(object sender, EventArgs e)
        {
            ignoreProcessView.Columns[0].FillWeight = 80;
            ignoreProcessView.Columns[1].FillWeight = 20;
        }'''
rep(old_proc_pop, new_proc_pop)

# SetupFrequentListView
old_freq_setup = '''        private void SetupFrequentListView()
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
        }'''
new_freq_setup = '''        private void SetupFrequentListView()
        {
            frequentPathListView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            frequentPathListView.ColumnCount = 2;
            frequentPathListView.Columns[0].HeaderText = "目录路径";
            frequentPathListView.Columns[1].HeaderText = "变更次数";
            frequentPathListView.Columns[0].FillWeight = 70;
            frequentPathListView.Columns[1].FillWeight = 30;
            frequentPathListView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            frequentPathListView.MultiSelect = false;
            frequentPathListView.RowHeadersVisible = false;
            frequentPathListView.AllowUserToAddRows = false;
        }'''
rep(old_freq_setup, new_freq_setup)

# RefreshFrequentPaths
old_freq_pop = '''            frequentPathListView.BeginUpdate();
            frequentPathListView.Items.Clear();

            List<FileChangeRecord> snapshot;
            lock (_recordsLock)
            {
                snapshot = _records.ToList();
            }

            try
            {
                var seen = new HashSet<string>(
                    snapshot.Select(GetChangeRecordKey),
                    StringComparer.OrdinalIgnoreCase);
                foreach (var record in _databaseService.GetChangeRecords(5000))
                {
                    if (seen.Add(GetChangeRecordKey(record)))
                        snapshot.Add(record);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取历史变更记录失败: {ex.Message}");
            }

            var paths = CleanupService.GetFrequentPaths(snapshot, 30);
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

            frequentPathListView.EndUpdate();'''
new_freq_pop = '''            frequentPathListView.Rows.Clear();

            List<FileChangeRecord> snapshot;
            lock (_recordsLock)
            {
                snapshot = _records.ToList();
            }

            try
            {
                var seen = new HashSet<string>(
                    snapshot.Select(GetChangeRecordKey),
                    StringComparer.OrdinalIgnoreCase);
                foreach (var record in _databaseService.GetChangeRecords(5000))
                {
                    if (seen.Add(GetChangeRecordKey(record)))
                        snapshot.Add(record);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"读取历史变更记录失败: {ex.Message}");
            }

            var paths = CleanupService.GetFrequentPaths(snapshot, 30);
            if (paths.Count == 0)
            {
                frequentPathListView.Rows.Add("暂无变更记录", "");
            }
            else
            {
                foreach (var p in paths)
                {
                    int idx = frequentPathListView.Rows.Add(p.Path, $"{p.ChangeCount}次");
                    frequentPathListView.Rows[idx].Tag = p;
                }
            }'''
rep(old_freq_pop, new_freq_pop)

# frequentPathListView_ItemSelectionChanged
old_freq_sel = '''        private void frequentPathListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
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
        }'''
new_freq_sel = '''        private void frequentPathListView_ItemSelectionChanged(object? sender, EventArgs e)
        {
            if (frequentPathListView.SelectedRows.Count > 0 &&
                frequentPathListView.SelectedRows[0].Tag is FrequentPathInfo info)
                cleanPathTextBox.Text = info.Path;
        }

        private void frequentPathListView_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            var hit = frequentPathListView.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;
            var row = frequentPathListView.Rows[hit.RowIndex];
            if (row.Tag is not FrequentPathInfo info) return;

            cleanPathTextBox.Text = info.Path;
            _ = TryScanCurrentPathAsync();
        }'''
rep(old_freq_sel, new_freq_sel)

# watcherDirListView_MouseClick
old_dir_click = '''        private void watcherDirListView_MouseClick(object? sender, MouseEventArgs e)
        {

            // 前置判断
            if (e.Button != MouseButtons.Right) return;

            var item = watcherDirListView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not WatchingDirectory dir) return;'''
new_dir_click = '''        private void watcherDirListView_MouseClick(object? sender, MouseEventArgs e)
        {
            // 前置判断
            if (e.Button != MouseButtons.Right) return;

            var hit = watcherDirListView.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;
            var row = watcherDirListView.Rows[hit.RowIndex];
            if (row.Tag is not WatchingDirectory dir) return;'''
rep(old_dir_click, new_dir_click)

# ignoreProcessView_MouseClick
old_proc_click = '''        private void ignoreProcessView_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var item = ignoreProcessView.GetItemAt(e.X, e.Y);
            if (item?.Tag is not IgnoreProcessRecord proc) return;'''
new_proc_click = '''        private void ignoreProcessView_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var hit = ignoreProcessView.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;
            var row = ignoreProcessView.Rows[hit.RowIndex];
            if (row.Tag is not IgnoreProcessRecord proc) return;'''
rep(old_proc_click, new_proc_click)

# SelectProcessInListView
old_sel_proc = '''        private void SelectProcessInListView(string processName)
        {
            foreach (ListViewItem item in ignoreProcessView.Items)
            {
                if (string.Equals(item.Text, processName, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    ignoreProcessView.EnsureVisible(item.Index);
                    break;
                }
            }
        }'''
new_sel_proc = '''        private void SelectProcessInListView(string processName)
        {
            foreach (DataGridViewRow row in ignoreProcessView.Rows)
            {
                if (string.Equals(row.Cells[0].Value?.ToString(), processName, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    if (ignoreProcessView.FirstDisplayedScrollingRowIndex != row.Index)
                        ignoreProcessView.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }'''
rep(old_sel_proc, new_sel_proc)

# ---------- usageProgressBar ----------
old_usage = '''            usageProgressBar.Value = (int)Math.Min(info.UsagePercent, 100);

            if (info.UsagePercent > 90)
                usageProgressBar.ForeColor = Color.Red;
            else if (info.UsagePercent > 70)
                usageProgressBar.ForeColor = Color.Orange;
            else
                usageProgressBar.ForeColor = Color.LimeGreen;'''
new_usage = '''            usageProgressBar.Maximum = 100;
            usageProgressBar.StyleCustomMode = true;
            usageProgressBar.Value = (int)Math.Min(info.UsagePercent, 100);

            if (info.UsagePercent > 90)
                usageProgressBar.FillColor = Color.Red;
            else if (info.UsagePercent > 70)
                usageProgressBar.FillColor = Color.Orange;
            else
                usageProgressBar.FillColor = Color.LimeGreen;'''
rep(old_usage, new_usage)

# ---------- InputBox -> UIInputForm ----------
old_input = '''            var input = Microsoft.VisualBasic.Interaction.InputBox("请输入进程名:", "添加忽略进程", "");
            var name = input?.Trim();
            if (string.IsNullOrEmpty(name)) return;

            AddIgnoreProcessInternal(name);'''
new_input = '''            using var inputForm = new UIInputForm { Text = "添加忽略进程", MaxLength = 120 };
            inputForm.Label.Text = "请输入进程名:";
            if (inputForm.ShowDialog() != DialogResult.OK) return;
            var name = inputForm.Editor.Text?.Trim();
            if (string.IsNullOrEmpty(name)) return;

            AddIgnoreProcessInternal(name);'''
rep(old_input, new_input)

# ---------- MessageBox -> UIMessageBox ----------
icon_map = {
    "Information": "ShowInfo",
    "Warning": "ShowWarning",
    "Error": "ShowError",
}

import re
def conv(m):
    msg = m.group(1)
    icon = m.group(2)
    return icon_map[icon] + "(" + msg + ")"

# Standard OK dialogs
s = re.sub(
    r'MessageBox\.Show\(([^,]*?),\s*"[^"]*",\s*MessageBoxButtons\.OK,\s*MessageBoxIcon\.(Information|Warning|Error)\)',
    conv, s, flags=re.S)

# The YesNo confirm dialog (multi-line)
s = s.replace(
    'if (MessageBox.Show(confirmText, $"确认清理（{methodName}）",\n                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)',
    'if (!UIMessageBox.ShowAsk(confirmText))')

with io.open(p, "w", encoding="utf-8") as f:
    f.write(s)

print("done")
