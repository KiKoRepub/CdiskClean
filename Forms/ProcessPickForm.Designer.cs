namespace CdiskClean.Forms
{
    partial class ProcessPickForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            searchLabel = new Label();
            searchTextBox = new AntdUI.Input();
            refreshBtn = new AntdUI.Button();
            procListView = new ListView();
            nameColumn = new ColumnHeader();
            pidColumn = new ColumnHeader();
            titleColumn = new ColumnHeader();
            okBtn = new AntdUI.Button();
            cancelBtn = new AntdUI.Button();
            hintLabel = new Label();
            SuspendLayout();
            // 
            // searchLabel
            // 
            searchLabel.AutoSize = true;
            searchLabel.Font = new Font("Microsoft YaHei UI", 11F);
            searchLabel.Location = new Point(12, 20);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new Size(62, 30);
            searchLabel.TabIndex = 0;
            searchLabel.Text = "搜索:";
            // 
            // searchTextBox
            // 
            searchTextBox.Font = new Font("Microsoft YaHei UI", 11F);
            searchTextBox.Location = new Point(80, 16);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(430, 30);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += searchTextBox_TextChanged;
            // 
            // refreshBtn
            // 
            refreshBtn.Font = new Font("Microsoft YaHei UI", 11F);
            refreshBtn.IconHoverSvg = "RefreshOutline";
            refreshBtn.Location = new Point(516, 9);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Radius = 1;
            refreshBtn.Size = new Size(116, 43);
            refreshBtn.TabIndex = 2;
            refreshBtn.Text = "刷新";
            refreshBtn.Type = AntdUI.TTypeMini.Primary;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // procListView
            // 
            procListView.Columns.AddRange(new ColumnHeader[] { nameColumn, pidColumn, titleColumn });
            procListView.Font = new Font("Microsoft YaHei UI", 11F);
            procListView.FullRowSelect = true;
            procListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            procListView.Location = new Point(12, 58);
            procListView.Name = "procListView";
            procListView.Size = new Size(640, 400);
            procListView.TabIndex = 3;
            procListView.UseCompatibleStateImageBehavior = false;
            procListView.View = View.Details;
            // 
            // nameColumn
            // 
            nameColumn.Text = "进程名";
            nameColumn.Width = 170;
            // 
            // pidColumn
            // 
            pidColumn.Text = "PID";
            pidColumn.Width = 90;
            // 
            // titleColumn
            // 
            titleColumn.Text = "窗口标题";
            titleColumn.Width = 370;
            // 
            // okBtn
            // 
            okBtn.Font = new Font("Microsoft YaHei UI", 11F);
            okBtn.Location = new Point(472, 480);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(85, 36);
            okBtn.TabIndex = 4;
            okBtn.Text = "确定";
            okBtn.Click += okBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Font = new Font("Microsoft YaHei UI", 11F);
            cancelBtn.Location = new Point(567, 480);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(85, 36);
            cancelBtn.TabIndex = 5;
            cancelBtn.Text = "取消";
            cancelBtn.Click += cancelBtn_Click;
            // 
            // hintLabel
            // 
            hintLabel.AutoSize = true;
            hintLabel.Font = new Font("Microsoft YaHei UI", 9F);
            hintLabel.ForeColor = Color.Gray;
            hintLabel.Location = new Point(14, 525);
            hintLabel.Name = "hintLabel";
            hintLabel.Size = new Size(676, 24);
            hintLabel.TabIndex = 6;
            hintLabel.Text = "提示：选择当前系统正在运行的进程（可多选、可搜索），确定后加入忽略进程列表";
            // 
            // ProcessPickForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 556);
            Controls.Add(hintLabel);
            Controls.Add(cancelBtn);
            Controls.Add(okBtn);
            Controls.Add(procListView);
            Controls.Add(refreshBtn);
            Controls.Add(searchTextBox);
            Controls.Add(searchLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProcessPickForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "选择要忽略的进程";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label searchLabel;
        private AntdUI.Input searchTextBox;
        private AntdUI.Button refreshBtn;
        private System.Windows.Forms.ListView procListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader pidColumn;
        private System.Windows.Forms.ColumnHeader titleColumn;
        private AntdUI.Button okBtn;
        private AntdUI.Button cancelBtn;
        private System.Windows.Forms.Label hintLabel;
    }
}
