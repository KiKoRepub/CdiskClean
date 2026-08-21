using Sunny.UI;

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
            searchLabel = new UILabel();
            searchTextBox = new UITextBox();
            refreshBtn = new UIButton();
            procListView = new UIDataGridView();
            nameColumn = new DataGridViewTextBoxColumn();
            pidColumn = new DataGridViewTextBoxColumn();
            titleColumn = new DataGridViewTextBoxColumn();
            okBtn = new UIButton();
            cancelBtn = new UIButton();
            hintLabel = new UILabel();
            SuspendLayout();
            //
            // searchLabel
            //
            searchLabel.AutoSize = true;
            searchLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            searchLabel.Location = new System.Drawing.Point(12, 20);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new System.Drawing.Size(62, 24);
            searchLabel.TabIndex = 0;
            searchLabel.Text = "搜索:";
            //
            // searchTextBox
            //
            searchTextBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            searchTextBox.Location = new System.Drawing.Point(80, 16);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new System.Drawing.Size(430, 30);
            searchTextBox.TabIndex = 1;
            searchTextBox.TextChanged += searchTextBox_TextChanged;
            //
            // refreshBtn
            //
            refreshBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            refreshBtn.Location = new System.Drawing.Point(526, 14);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new System.Drawing.Size(80, 34);
            refreshBtn.TabIndex = 2;
            refreshBtn.Text = "刷新";
            refreshBtn.Click += refreshBtn_Click;
            //
            // procListView
            //
            procListView.Columns.AddRange(new DataGridViewTextBoxColumn[]
            {
                nameColumn, pidColumn, titleColumn
            });
            procListView.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            procListView.Location = new System.Drawing.Point(12, 58);
            procListView.MultiSelect = true;
            procListView.Name = "procListView";
            procListView.Size = new System.Drawing.Size(640, 400);
            procListView.TabIndex = 3;
            procListView.RowHeadersVisible = false;
            procListView.AllowUserToAddRows = false;
            procListView.ReadOnly = true;
            procListView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //
            // nameColumn
            //
            nameColumn.HeaderText = "进程名";
            nameColumn.Width = 170;
            //
            // pidColumn
            //
            pidColumn.HeaderText = "PID";
            pidColumn.Width = 90;
            //
            // titleColumn
            //
            titleColumn.HeaderText = "窗口标题";
            titleColumn.Width = 370;
            //
            // okBtn
            //
            okBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            okBtn.Location = new System.Drawing.Point(472, 480);
            okBtn.Name = "okBtn";
            okBtn.Size = new System.Drawing.Size(85, 36);
            okBtn.TabIndex = 4;
            okBtn.Text = "确定";
            okBtn.Click += okBtn_Click;
            //
            // cancelBtn
            //
            cancelBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            cancelBtn.Location = new System.Drawing.Point(567, 480);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new System.Drawing.Size(85, 36);
            cancelBtn.TabIndex = 5;
            cancelBtn.Text = "取消";
            cancelBtn.Click += cancelBtn_Click;
            //
            // hintLabel
            //
            hintLabel.AutoSize = true;
            hintLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            hintLabel.ForeColor = System.Drawing.Color.Gray;
            hintLabel.Location = new System.Drawing.Point(14, 525);
            hintLabel.Name = "hintLabel";
            hintLabel.Size = new System.Drawing.Size(550, 20);
            hintLabel.TabIndex = 6;
            hintLabel.Text = "提示：选择当前系统正在运行的进程（可多选、可搜索），确定后加入忽略进程列表";
            //
            // ProcessPickForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(664, 556);
            Controls.Add(hintLabel);
            Controls.Add(cancelBtn);
            Controls.Add(okBtn);
            Controls.Add(procListView);
            Controls.Add(refreshBtn);
            Controls.Add(searchTextBox);
            Controls.Add(searchLabel);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProcessPickForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "选择要忽略的进程";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private UILabel searchLabel;
        private UITextBox searchTextBox;
        private UIButton refreshBtn;
        private UIDataGridView procListView;
        private DataGridViewTextBoxColumn nameColumn;
        private DataGridViewTextBoxColumn pidColumn;
        private DataGridViewTextBoxColumn titleColumn;
        private UIButton okBtn;
        private UIButton cancelBtn;
        private UILabel hintLabel;
    }
}
