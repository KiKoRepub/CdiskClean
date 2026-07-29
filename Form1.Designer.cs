namespace CdiskClean
{
    partial class HideButton
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TabPageControl1 = new TabControl();
            totalReviewPage = new TabPage();
            watcherPage = new TabPage();
            statusStrip1 = new StatusStrip();
            watchStatusLabel = new ToolStripStatusLabel();
            writedRecordStatusLabel = new ToolStripStatusLabel();
            timeStatusLabel = new ToolStripStatusLabel();
            NoticeIcon = new ToolStripStatusLabel();
            closeButton = new Button();
            BiggerButton = new Button();
            button1 = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            splitContainer1 = new SplitContainer();
            menuStrip1 = new MenuStrip();
            查看ToolStripMenuItem = new ToolStripMenuItem();
            TabPageControl1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // TabPageControl1
            // 
            TabPageControl1.Controls.Add(totalReviewPage);
            TabPageControl1.Controls.Add(watcherPage);
            TabPageControl1.Location = new Point(12, 41);
            TabPageControl1.Name = "TabPageControl1";
            TabPageControl1.SelectedIndex = 0;
            TabPageControl1.Size = new Size(776, 203);
            TabPageControl1.TabIndex = 0;
            // 
            // totalReviewPage
            // 
            totalReviewPage.Location = new Point(4, 33);
            totalReviewPage.Name = "totalReviewPage";
            totalReviewPage.Padding = new Padding(3);
            totalReviewPage.Size = new Size(768, 166);
            totalReviewPage.TabIndex = 0;
            totalReviewPage.Text = "概览";
            totalReviewPage.UseVisualStyleBackColor = true;
            // 
            // watcherPage
            // 
            watcherPage.Location = new Point(4, 33);
            watcherPage.Name = "watcherPage";
            watcherPage.Padding = new Padding(3);
            watcherPage.Size = new Size(768, 166);
            watcherPage.TabIndex = 1;
            watcherPage.Text = "实时监测";
            watcherPage.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { watchStatusLabel, writedRecordStatusLabel, timeStatusLabel, NoticeIcon });
            statusStrip1.Location = new Point(0, 419);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(784, 31);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // watchStatusLabel
            // 
            watchStatusLabel.BackColor = SystemColors.Control;
            watchStatusLabel.Name = "watchStatusLabel";
            watchStatusLabel.Size = new Size(100, 24);
            watchStatusLabel.Text = "未开始监测";
            watchStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            watchStatusLabel.Click += watchStatusLabel_Click;
            // 
            // writedRecordStatusLabel
            // 
            writedRecordStatusLabel.BackColor = SystemColors.Control;
            writedRecordStatusLabel.Margin = new Padding(100, 4, 0, 3);
            writedRecordStatusLabel.Name = "writedRecordStatusLabel";
            writedRecordStatusLabel.Padding = new Padding(100, 0, 0, 0);
            writedRecordStatusLabel.Size = new Size(229, 24);
            writedRecordStatusLabel.Text = "已记录             ";
            writedRecordStatusLabel.Click += WritedRecordStatusLabel_Click;
            // 
            // timeStatusLabel
            // 
            timeStatusLabel.Margin = new Padding(95, 4, 0, 3);
            timeStatusLabel.Name = "timeStatusLabel";
            timeStatusLabel.Size = new Size(189, 24);
            timeStatusLabel.Text = "\u007f2026-07-28              ";
            timeStatusLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // NoticeIcon
            // 
            NoticeIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            NoticeIcon.Image = Properties.Resources.通知;
            NoticeIcon.Margin = new Padding(15, 4, 0, 3);
            NoticeIcon.Name = "NoticeIcon";
            NoticeIcon.Size = new Size(24, 24);
            NoticeIcon.Text = " ";
            NoticeIcon.TextAlign = ContentAlignment.MiddleRight;
            NoticeIcon.TextImageRelation = TextImageRelation.Overlay;
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.Location = new Point(271, 6);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(40, 33);
            closeButton.TabIndex = 1;
            closeButton.Text = "X";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // BiggerButton
            // 
            BiggerButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BiggerButton.Location = new Point(225, 6);
            BiggerButton.Name = "BiggerButton";
            BiggerButton.Size = new Size(40, 33);
            BiggerButton.TabIndex = 2;
            BiggerButton.Text = "口";
            BiggerButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(179, 6);
            button1.Name = "button1";
            button1.Size = new Size(40, 33);
            button1.TabIndex = 3;
            button1.Text = "——";
            button1.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top;
            splitContainer1.Location = new Point(0, -2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Panel2.Controls.Add(BiggerButton);
            splitContainer1.Panel2.Controls.Add(closeButton);
            splitContainer1.Size = new Size(784, 47);
            splitContainer1.SplitterDistance = 466;
            splitContainer1.TabIndex = 5;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 查看ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(466, 32);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // 查看ToolStripMenuItem
            // 
            查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            查看ToolStripMenuItem.Size = new Size(62, 28);
            查看ToolStripMenuItem.Text = "查看";
            // 
            // HideButton
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 450);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(TabPageControl1);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuStrip1;
            Name = "HideButton";
            Text = "C盘监测工具";
            Load += Form1_Load;
            TabPageControl1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl TabPageControl1;
        private TabPage totalReviewPage;
        private TabPage watcherPage;
        private Button closeButton;
        private Button BiggerButton;
        private Button button1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel watchStatusLabel;
        private ToolStripStatusLabel writedRecordStatusLabel;
        private ToolStripStatusLabel timeStatusLabel;
        private System.Windows.Forms.Timer timer1;
        private ToolStripStatusLabel NoticeIcon;
        private NotifyIcon notifyIcon1;
        private SplitContainer splitContainer1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 查看ToolStripMenuItem;
    }
}
