namespace CdiskClean.Forms
{
    partial class BetterDirAddForm
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
            basePathLabel = new System.Windows.Forms.Label();
            basePathTextBox = new AntdUI.Input();
            browseBtn = new AntdUI.Button();
            dirTreeView = new System.Windows.Forms.TreeView();
            selectAllBtn = new AntdUI.Button();
            selectNoneBtn = new AntdUI.Button();
            okBtn = new AntdUI.Button();
            cancelBtn = new AntdUI.Button();
            hintLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            //
            // basePathLabel
            //
            basePathLabel.AutoSize = true;
            basePathLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            basePathLabel.Location = new System.Drawing.Point(12, 20);
            basePathLabel.Name = "basePathLabel";
            basePathLabel.Size = new System.Drawing.Size(92, 24);
            basePathLabel.TabIndex = 0;
            basePathLabel.Text = "基础目录:";
            //
            // basePathTextBox
            //
            basePathTextBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            basePathTextBox.Location = new System.Drawing.Point(110, 16);
            basePathTextBox.Name = "basePathTextBox";
            basePathTextBox.ReadOnly = true;
            basePathTextBox.Size = new System.Drawing.Size(470, 30);
            basePathTextBox.TabIndex = 1;
            //
            // browseBtn
            //
            browseBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            browseBtn.Location = new System.Drawing.Point(588, 14);
            browseBtn.Name = "browseBtn";
            browseBtn.Size = new System.Drawing.Size(100, 34);
            browseBtn.TabIndex = 2;
            browseBtn.Text = "浏览...";
            browseBtn.Click += browseBtn_Click;
            //
            // dirTreeView
            //
            dirTreeView.CheckBoxes = true;
            dirTreeView.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            dirTreeView.HideSelection = false;
            dirTreeView.Location = new System.Drawing.Point(12, 58);
            dirTreeView.Name = "dirTreeView";
            dirTreeView.Size = new System.Drawing.Size(676, 440);
            dirTreeView.TabIndex = 3;
            dirTreeView.BeforeExpand += dirTreeView_BeforeExpand;
            //
            // selectAllBtn
            //
            selectAllBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            selectAllBtn.Location = new System.Drawing.Point(12, 514);
            selectAllBtn.Name = "selectAllBtn";
            selectAllBtn.Size = new System.Drawing.Size(90, 36);
            selectAllBtn.TabIndex = 4;
            selectAllBtn.Text = "全选";
            selectAllBtn.Click += selectAllBtn_Click;
            //
            // selectNoneBtn
            //
            selectNoneBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            selectNoneBtn.Location = new System.Drawing.Point(108, 514);
            selectNoneBtn.Name = "selectNoneBtn";
            selectNoneBtn.Size = new System.Drawing.Size(90, 36);
            selectNoneBtn.TabIndex = 5;
            selectNoneBtn.Text = "全不选";
            selectNoneBtn.Click += selectNoneBtn_Click;
            //
            // okBtn
            //
            okBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            okBtn.Location = new System.Drawing.Point(512, 514);
            okBtn.Name = "okBtn";
            okBtn.Size = new System.Drawing.Size(80, 36);
            okBtn.TabIndex = 6;
            okBtn.Text = "确定";
            okBtn.Click += okBtn_Click;
            //
            // cancelBtn
            //
            cancelBtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            cancelBtn.Location = new System.Drawing.Point(598, 514);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new System.Drawing.Size(80, 36);
            cancelBtn.TabIndex = 7;
            cancelBtn.Text = "取消";
            cancelBtn.Click += cancelBtn_Click;
            //
            // hintLabel
            //
            hintLabel.AutoSize = true;
            hintLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            hintLabel.ForeColor = System.Drawing.Color.Gray;
            hintLabel.Location = new System.Drawing.Point(14, 559);
            hintLabel.Name = "hintLabel";
            hintLabel.Size = new System.Drawing.Size(500, 20);
            hintLabel.TabIndex = 8;
            hintLabel.Text = "提示：勾选基础目录本身或其任意子路径，确定后全部作为监测目录添加";
            //
            // BetterDirAddForm
            //
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(700, 590);
            Controls.Add(hintLabel);
            Controls.Add(cancelBtn);
            Controls.Add(okBtn);
            Controls.Add(selectNoneBtn);
            Controls.Add(selectAllBtn);
            Controls.Add(dirTreeView);
            Controls.Add(browseBtn);
            Controls.Add(basePathTextBox);
            Controls.Add(basePathLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BetterDirAddForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "高级添加监测目录";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label basePathLabel;
        private AntdUI.Input basePathTextBox;
        private AntdUI.Button browseBtn;
        private System.Windows.Forms.TreeView dirTreeView;
        private AntdUI.Button selectAllBtn;
        private AntdUI.Button selectNoneBtn;
        private AntdUI.Button okBtn;
        private AntdUI.Button cancelBtn;
        private System.Windows.Forms.Label hintLabel;
    }
}
