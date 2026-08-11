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
            searchLabel = new System.Windows.Forms.Label();
            searchTextBox = new System.Windows.Forms.TextBox();
            refreshBtn = new System.Windows.Forms.Button();
            procListView = new System.Windows.Forms.ListView();
            nameColumn = new System.Windows.Forms.ColumnHeader();
            pidColumn = new System.Windows.Forms.ColumnHeader();
            titleColumn = new System.Windows.Forms.ColumnHeader();
            okBtn = new System.Windows.Forms.Button();
            cancelBtn = new System.Windows.Forms.Button();
            hintLabel = new System.Windows.Forms.Label();
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
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            //
            // procListView
            //
            procListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                nameColumn, pidColumn, titleColumn
            });
            procListView.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            procListView.FullRowSelect = true;
            procListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            procListView.HideSelection = false;
            procListView.Location = new System.Drawing.Point(12, 58);
            procListView.MultiSelect = true;
            procListView.Name = "procListView";
            procListView.Size = new System.Drawing.Size(640, 400);
            procListView.TabIndex = 3;
            procListView.UseCompatibleStateImageBehavior = false;
            procListView.View = System.Windows.Forms.View.Details;
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
            okBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            okBtn.Location = new System.Drawing.Point(472, 480);
            okBtn.Name = "okBtn";
            okBtn.Size = new System.Drawing.Size(85, 36);
            okBtn.TabIndex = 4;
            okBtn.Text = "确定";
            okBtn.UseVisualStyleBackColor = true;
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
            cancelBtn.UseVisualStyleBackColor = true;
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
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProcessPickForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "选择要忽略的进程";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.ListView procListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader pidColumn;
        private System.Windows.Forms.ColumnHeader titleColumn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label hintLabel;
    }
}
