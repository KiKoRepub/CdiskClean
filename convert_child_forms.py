# -*- coding: utf-8 -*-
import os

BASE = r"D:\university\CSharp\projects\CdiskClean\Forms"

def ensure_using(content, ns_line_start="namespace CdiskClean"):
    if "using SunnyUI;" in content:
        return content
    idx = content.find(ns_line_start)
    if idx == -1:
        return content
    return content[:idx] + "using SunnyUI;\n\n" + content[idx:]

def write(path, content):
    with open(path, "w", encoding="utf-8") as f:
        f.write(content)
    print("updated:", os.path.basename(path))

# ---------- StatisticForm.Designer.cs ----------
p = os.path.join(BASE, "StatisticForm.Designer.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
s = s.replace("mainTabControl = new TabControl();", "mainTabControl = new UITabControl();")
s = s.replace("notificationGrid = new DataGridView();", "notificationGrid = new UIDataGridView();")
s = s.replace("statsGrid = new DataGridView();", "statsGrid = new UIDataGridView();")
s = s.replace("detailDataGrid = new DataGridView();", "detailDataGrid = new UIDataGridView();")
s = s.replace("titleLabel = new Label();", "titleLabel = new UILabel();")
# remove closeBtn instantiation line
s = s.replace("            closeBtn = new Button();\n", "")
# remove closeBtn property block
s = s.replace(
    '            titleLabel.Text = "统计与提醒记录";\n'
    '            //\n'
    '            // closeBtn\n'
    '            //\n'
    '            closeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;\n'
    '            closeBtn.Location = new Point(1004, 9);\n'
    '            closeBtn.Name = "closeBtn";\n'
    '            closeBtn.Size = new Size(84, 34);\n'
    '            closeBtn.TabIndex = 2;\n'
    '            closeBtn.Text = "关闭";\n'
    '            closeBtn.UseVisualStyleBackColor = true;\n'
    '            closeBtn.Click += closeBtn_Click;\n'
    '            //\n'
    '            // Form4\n'
    '            //\n',
    '            titleLabel.Text = "统计与提醒记录";\n'
    '            //\n'
    '            // Form4\n'
    '            //\n')
# remove Controls.Add(closeBtn)
s = s.replace("            Controls.Add(closeBtn);\n", "")
# remove FormBorderStyle = FixedDialog
s = s.replace(
    "            Controls.Add(mainTabControl);\n            FormBorderStyle = FormBorderStyle.FixedDialog;\n            MaximizeBox = false;\n",
    "            Controls.Add(mainTabControl);\n            MaximizeBox = false;\n")
# field declarations
s = s.replace(
    "        private Label titleLabel;\n        private Button closeBtn;\n",
    "        private UILabel titleLabel;\n")
write(p, s)

# ---------- BetterDirAddForm.Designer.cs ----------
p = os.path.join(BASE, "BetterDirAddForm.Designer.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
s = s.replace("System.Windows.Forms.Label", "UILabel")
s = s.replace("System.Windows.Forms.TextBox", "UITextBox")
s = s.replace("System.Windows.Forms.Button", "UIButton")
# remove UseVisualStyleBackColor for the 5 buttons (keep Click wiring)
for btn in ["browseBtn", "selectAllBtn", "selectNoneBtn", "okBtn", "cancelBtn"]:
    old = f"            {btn}.UseVisualStyleBackColor = true;\n            {btn}.Click += {btn}_Click;"
    new = f"            {btn}.Click += {btn}_Click;"
    assert old in s, f"missing {btn} UseVisualStyleBackColor block"
    s = s.replace(old, new)
# remove FormBorderStyle = FixedDialog
s = s.replace(
    "            Controls.Add(basePathLabel);\n            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;\n            MaximizeBox = false;\n",
    "            Controls.Add(basePathLabel);\n            MaximizeBox = false;\n")
write(p, s)

# ---------- ProcessPickForm.Designer.cs ----------
p = os.path.join(BASE, "ProcessPickForm.Designer.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
s = s.replace("System.Windows.Forms.Label", "UILabel")
s = s.replace("System.Windows.Forms.TextBox", "UITextBox")
s = s.replace("System.Windows.Forms.Button", "UIButton")
s = s.replace("System.Windows.Forms.ListView", "UIDataGridView")
s = s.replace("System.Windows.Forms.ColumnHeader", "DataGridViewTextBoxColumn")
# remove ListView-only procListView props (FullRowSelect/HeaderStyle/HideSelection)
s = s.replace(
    "            procListView.FullRowSelect = true;\n"
    "            procListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;\n"
    "            procListView.HideSelection = false;\n",
    "")
# replace UseCompatibleStateImageBehavior + View with DataGridView-friendly props
s = s.replace(
    "            procListView.UseCompatibleStateImageBehavior = false;\n"
    "            procListView.View = System.Windows.Forms.View.Details;\n",
    "            procListView.RowHeadersVisible = false;\n"
    "            procListView.AllowUserToAddRows = false;\n"
    "            procListView.ReadOnly = true;\n")
# column .Text -> .HeaderText
s = s.replace('nameColumn.Text = "进程名";', 'nameColumn.HeaderText = "进程名";')
s = s.replace('pidColumn.Text = "PID";', 'pidColumn.HeaderText = "PID";')
s = s.replace('titleColumn.Text = "窗口标题";', 'titleColumn.HeaderText = "窗口标题";')
# remove UseVisualStyleBackColor for refreshBtn/okBtn/cancelBtn
for btn in ["refreshBtn", "okBtn", "cancelBtn"]:
    old = f"            {btn}.UseVisualStyleBackColor = true;\n            {btn}.Click += {btn}_Click;"
    new = f"            {btn}.Click += {btn}_Click;"
    assert old in s, f"missing {btn} UseVisualStyleBackColor block"
    s = s.replace(old, new)
# remove FormBorderStyle = FixedDialog
s = s.replace(
    "            Controls.Add(searchLabel);\n            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;\n            MaximizeBox = false;\n",
    "            Controls.Add(searchLabel);\n            MaximizeBox = false;\n")
write(p, s)

# ---------- BetterDirAddForm.cs (using + MessageBox) ----------
p = os.path.join(BASE, "BetterDirAddForm.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
s = s.replace(
    '                MessageBox.Show("请至少勾选一个目录路径。", "提示",\n'
    '                    MessageBoxButtons.OK, MessageBoxIcon.Information);\n',
    '                UIMessageBox.ShowInfo("请至少勾选一个目录路径。");\n')
assert "MessageBox.Show" not in s, "BetterDirAddForm MessageBox not fully replaced"
write(p, s)

# ---------- ProcessPickForm.cs (using only; logic done via Edit) ----------
p = os.path.join(BASE, "ProcessPickForm.cs")
s = open(p, encoding="utf-8").read()
s = ensure_using(s)
write(p, s)

# ---------- StatisticForm.cs (remove closeBtn_Click) ----------
p = os.path.join(BASE, "StatisticForm.cs")
s = open(p, encoding="utf-8").read()
s = s.replace(
    "        private void closeBtn_Click(object? sender, EventArgs e)\n"
    "        {\n"
    "            Close();\n"
    "        }\n"
    "    }\n",
    "    }\n")
assert "closeBtn_Click" not in s, "closeBtn_Click not removed"
write(p, s)

print("ALL DONE")
