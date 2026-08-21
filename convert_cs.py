# -*- coding: utf-8 -*-
import os, re

BASE = r"D:\university\CSharp\projects\CdiskClean\Forms"

def ensure_using(content):
    if "using SunnyUI;" in content:
        return content
    idx = content.find("namespace CdiskClean")
    if idx == -1:
        return content
    return content[:idx] + "using SunnyUI;\n\n" + content[idx:]

def write(path, content):
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)
    print("updated:", os.path.basename(path))

# ---------- BetterDirAddForm.cs ----------
p = os.path.join(BASE, "BetterDirAddForm.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
s = re.sub(r'MessageBox\.Show\(\s*"请至少勾选一个目录路径。"[^;]*?\);',
           'UIMessageBox.ShowInfo("请至少勾选一个目录路径。");', s, flags=re.S)
if "MessageBox.Show" in s:
    print("WARN BetterDirAddForm still has MessageBox.Show")
write(p, s)

# ---------- ProcessPickForm.cs ----------
p = os.path.join(BASE, "ProcessPickForm.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
# ListView -> DataGridView API
s = s.replace("procListView.SelectedItems", "procListView.SelectedRows")
s = s.replace("Cast<ListViewItem>()", "Cast<DataGridViewRow>()")
# rewrite ApplyFilter body (BeginUpdate ... EndUpdate span)
new_body = (
    "            procListView.Rows.Clear();\n"
    "            foreach (var p in _allProcesses)\n"
    "            {\n"
    "                if (keyword.Length > 0 &&\n"
    "                    !p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) &&\n"
    "                    !p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))\n"
    "                    continue;\n"
    "\n"
    "                var idx = procListView.Rows.Add(p.Name, p.Pid.ToString(), p.Title);\n"
    "                procListView.Rows[idx].Tag = p.Name;\n"
    "            }"
)
s = re.sub(r'procListView\.BeginUpdate\(\);.*?procListView\.EndUpdate\(\);', new_body, s, flags=re.S)
# MessageBox in okBtn_Click
s = re.sub(r'MessageBox\.Show\(\s*"请先选择要添加的进程。"[^;]*?\);',
           'UIMessageBox.ShowInfo("请先选择要添加的进程。");', s, flags=re.S)
if "MessageBox.Show" in s or "ListViewItem" in s or "SelectedItems" in s or "BeginUpdate" in s:
    print("WARN ProcessPickForm residual:",
          [t for t in ("MessageBox.Show","ListViewItem","SelectedItems","BeginUpdate") if t in s])
write(p, s)

# ---------- StatisticForm.cs ----------
p = os.path.join(BASE, "StatisticForm.cs")
s = open(p, encoding="utf-8").read()
s = re.sub(r'\n        private void closeBtn_Click\(object\? sender, EventArgs e\)\n        \{\n            Close\(\);\n        \}\n',
           '\n', s)
if "closeBtn_Click" in s:
    print("WARN StatisticForm still has closeBtn_Click")
write(p, s)

print("ALL DONE")
