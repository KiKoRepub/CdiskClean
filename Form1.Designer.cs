namespace CdiskClean
{
    partial class HideButton
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            TabPageControl1 = new TabControl();
            totalReviewPage = new TabPage();
            warningLabel = new Label();
            freeSpaceLabel = new Label();
            usedSpaceLabel = new Label();
            totalSpaceLabel = new Label();
            usageProgressBar = new ProgressBar();
            dashboardTitleLabel = new Label();
            watcherPage = new TabPage();
            exportBtn = new Button();
            typeFilterCombo = new ComboBox();
            typeFilterLabel = new Label();
            clearBtn = new Button();
            pauseBtn = new Button();
            changesDataGrid = new DataGridView();
            TimeColumn = new DataGridViewTextBoxColumn();
            TypeColumn = new DataGridViewTextBoxColumn();
            FileNameColumn = new DataGridViewTextBoxColumn();
            PathColumn = new DataGridViewTextBoxColumn();
            SizeColumn = new DataGridViewTextBoxColumn();
            folderAnalyzerPage = new TabPage();
            folderTreeView = new TreeView();
            scanProgressBar = new ProgressBar();
            stopBtn = new Button();
            scanBtn = new Button();
            selectDirBtn = new Button();
            selectedPathTextBox = new TextBox();
            statusStrip1 = new StatusStrip();
            watchStatusLabel = new ToolStripStatusLabel();
            writedRecordStatusLabel = new ToolStripStatusLabel();
            timeStatusLabel = new ToolStripStatusLabel();
            NoticeIcon = new ToolStripStatusLabel();
            closeButton = new Button();
            BiggerButton = new Button();
            button1 = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            diskRefreshTimer = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            splitContainer1 = new SplitContainer();
            label1 = new Label();
            TabPageControl1.SuspendLayout();
            totalReviewPage.SuspendLayout();
            watcherPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).BeginInit();
            folderAnalyzerPage.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();

            //
            // TabPageControl1
            //
            TabPageControl1.Controls.Add(totalReviewPage);
            TabPageControl1.Controls.Add(watcherPage);
            TabPageControl1.Controls.Add(folderAnalyzerPage);
            TabPageControl1.Location = new Point(12, 41);
            TabPageControl1.Name = "TabPageControl1";
            TabPageControl1.SelectedIndex = 0;
            TabPageControl1.Size = new Size(776, 378);
            TabPageControl1.TabIndex = 0;

            //
            // totalReviewPage (概览)
            //
            totalReviewPage.Controls.Add(warningLabel);
            totalReviewPage.Controls.Add(freeSpaceLabel);
            totalReviewPage.Controls.Add(usedSpaceLabel);
            totalReviewPage.Controls.Add(totalSpaceLabel);
            totalReviewPage.Controls.Add(usageProgressBar);
            totalReviewPage.Controls.Add(dashboardTitleLabel);
            totalReviewPage.Location = new Point(4, 33);
            totalReviewPage.Name = "totalReviewPage";
            totalReviewPage.Padding = new Padding(3);
            totalReviewPage.Size = new Size(768, 341);
            totalReviewPage.TabIndex = 0;
            totalReviewPage.Text = "概览";
            totalReviewPage.UseVisualStyleBackColor = true;

            // dashboardTitleLabel
            //
            dashboardTitleLabel.AutoSize = true;
            dashboardTitleLabel.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            dashboardTitleLabel.Location = new Point(20, 15);
            dashboardTitleLabel.Name = "dashboardTitleLabel";
            dashboardTitleLabel.Size = new Size(120, 27);
            dashboardTitleLabel.TabIndex = 0;
            dashboardTitleLabel.Text = "C盘空间概况";

            // usageProgressBar
            //
            usageProgressBar.Location = new Point(20, 70);
            usageProgressBar.Name = "usageProgressBar";
            usageProgressBar.Size = new Size(720, 30);
            usageProgressBar.TabIndex = 1;

            // totalSpaceLabel
            //
            totalSpaceLabel.AutoSize = true;
            totalSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            totalSpaceLabel.Location = new Point(20, 120);
            totalSpaceLabel.Name = "totalSpaceLabel";
            totalSpaceLabel.Size = new Size(131, 23);
            totalSpaceLabel.TabIndex = 2;
            totalSpaceLabel.Text = "总容量: 加载中...";

            // usedSpaceLabel
            //
            usedSpaceLabel.AutoSize = true;
            usedSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            usedSpaceLabel.Location = new Point(20, 160);
            usedSpaceLabel.Name = "usedSpaceLabel";
            usedSpaceLabel.Size = new Size(131, 23);
            usedSpaceLabel.TabIndex = 3;
            usedSpaceLabel.Text = "已用: 加载中...";

            // freeSpaceLabel
            //
            freeSpaceLabel.AutoSize = true;
            freeSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            freeSpaceLabel.Location = new Point(20, 200);
            freeSpaceLabel.Name = "freeSpaceLabel";
            freeSpaceLabel.Size = new Size(131, 23);
            freeSpaceLabel.TabIndex = 4;
            freeSpaceLabel.Text = "剩余: 加载中...";

            // warningLabel
            //
            warningLabel.AutoSize = true;
            warningLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            warningLabel.ForeColor = Color.Red;
            warningLabel.Location = new Point(20, 250);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(263, 23);
            warningLabel.TabIndex = 5;
            warningLabel.Text = "⚠ C盘剩余空间不足 10GB，请及时清理！";
            warningLabel.Visible = false;

            //
            // watcherPage (实时监测)
            //
            watcherPage.Controls.Add(exportBtn);
            watcherPage.Controls.Add(typeFilterCombo);
            watcherPage.Controls.Add(typeFilterLabel);
            watcherPage.Controls.Add(clearBtn);
            watcherPage.Controls.Add(pauseBtn);
            watcherPage.Controls.Add(changesDataGrid);
            watcherPage.Location = new Point(4, 33);
            watcherPage.Name = "watcherPage";
            watcherPage.Padding = new Padding(3);
            watcherPage.Size = new Size(768, 341);
            watcherPage.TabIndex = 1;
            watcherPage.Text = "实时监测";
            watcherPage.UseVisualStyleBackColor = true;

            // pauseBtn
            //
            pauseBtn.Location = new Point(8, 8);
            pauseBtn.Name = "pauseBtn";
            pauseBtn.Size = new Size(80, 30);
            pauseBtn.TabIndex = 0;
            pauseBtn.Text = "开始监测";
            pauseBtn.UseVisualStyleBackColor = true;
            pauseBtn.Click += pauseBtn_Click;

            // clearBtn
            //
            clearBtn.Location = new Point(94, 8);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(80, 30);
            clearBtn.TabIndex = 1;
            clearBtn.Text = "清空";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += clearBtn_Click;

            // exportBtn
            //
            exportBtn.Location = new Point(180, 8);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(80, 30);
            exportBtn.TabIndex = 2;
            exportBtn.Text = "导出";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;

            // typeFilterLabel
            //
            typeFilterLabel.AutoSize = true;
            typeFilterLabel.Location = new Point(280, 12);
            typeFilterLabel.Name = "typeFilterLabel";
            typeFilterLabel.Size = new Size(68, 23);
            typeFilterLabel.TabIndex = 3;
            typeFilterLabel.Text = "筛选类型:";
            typeFilterLabel.TextAlign = ContentAlignment.MiddleLeft;

            // typeFilterCombo
            //
            typeFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            typeFilterCombo.FormattingEnabled = true;
            typeFilterCombo.Items.AddRange(new object[] { "全部", "创建", "修改", "删除", "重命名" });
            typeFilterCombo.Location = new Point(354, 8);
            typeFilterCombo.Name = "typeFilterCombo";
            typeFilterCombo.Size = new Size(121, 31);
            typeFilterCombo.TabIndex = 4;
            typeFilterCombo.SelectedIndexChanged += typeFilterCombo_SelectedIndexChanged;

            // changesDataGrid
            //
            changesDataGrid.AllowUserToAddRows = false;
            changesDataGrid.AllowUserToDeleteRows = false;
            changesDataGrid.AllowUserToResizeRows = false;
            changesDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            changesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            changesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            changesDataGrid.Columns.AddRange(new DataGridViewColumn[] {
                TimeColumn,
                TypeColumn,
                FileNameColumn,
                PathColumn,
                SizeColumn
            });
            changesDataGrid.Location = new Point(8, 48);
            changesDataGrid.Name = "changesDataGrid";
            changesDataGrid.ReadOnly = true;
            changesDataGrid.RowHeadersVisible = false;
            changesDataGrid.RowTemplate.Height = 25;
            changesDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            changesDataGrid.Size = new Size(752, 283);
            changesDataGrid.TabIndex = 5;

            // TimeColumn
            //
            TimeColumn.HeaderText = "时间";
            TimeColumn.Name = "TimeColumn";
            TimeColumn.FillWeight = 80F;

            // TypeColumn
            //
            TypeColumn.HeaderText = "类型";
            TypeColumn.Name = "TypeColumn";
            TypeColumn.FillWeight = 50F;

            // FileNameColumn
            //
            FileNameColumn.HeaderText = "文件名";
            FileNameColumn.Name = "FileNameColumn";
            FileNameColumn.FillWeight = 70F;

            // PathColumn
            //
            PathColumn.HeaderText = "路径";
            PathColumn.Name = "PathColumn";
            PathColumn.FillWeight = 120F;

            // SizeColumn
            //
            SizeColumn.HeaderText = "大小";
            SizeColumn.Name = "SizeColumn";
            SizeColumn.FillWeight = 50F;

            //
            // folderAnalyzerPage (文件夹分析)
            //
            folderAnalyzerPage.Controls.Add(folderTreeView);
            folderAnalyzerPage.Controls.Add(scanProgressBar);
            folderAnalyzerPage.Controls.Add(stopBtn);
            folderAnalyzerPage.Controls.Add(scanBtn);
            folderAnalyzerPage.Controls.Add(selectDirBtn);
            folderAnalyzerPage.Controls.Add(selectedPathTextBox);
            folderAnalyzerPage.Location = new Point(4, 33);
            folderAnalyzerPage.Name = "folderAnalyzerPage";
            folderAnalyzerPage.Size = new Size(768, 341);
            folderAnalyzerPage.TabIndex = 2;
            folderAnalyzerPage.Text = "文件夹分析";
            folderAnalyzerPage.UseVisualStyleBackColor = true;

            // selectedPathTextBox
            //
            selectedPathTextBox.Location = new Point(8, 10);
            selectedPathTextBox.Name = "selectedPathTextBox";
            selectedPathTextBox.ReadOnly = true;
            selectedPathTextBox.Size = new Size(400, 30);
            selectedPathTextBox.TabIndex = 0;

            // selectDirBtn
            //
            selectDirBtn.Location = new Point(414, 8);
            selectDirBtn.Name = "selectDirBtn";
            selectDirBtn.Size = new Size(100, 30);
            selectDirBtn.TabIndex = 1;
            selectDirBtn.Text = "选择目录";
            selectDirBtn.UseVisualStyleBackColor = true;
            selectDirBtn.Click += selectDirBtn_Click;

            // scanBtn
            //
            scanBtn.Location = new Point(520, 8);
            scanBtn.Name = "scanBtn";
            scanBtn.Size = new Size(100, 30);
            scanBtn.TabIndex = 2;
            scanBtn.Text = "开始扫描";
            scanBtn.UseVisualStyleBackColor = true;
            scanBtn.Click += scanBtn_Click;

            // stopBtn
            //
            stopBtn.Enabled = false;
            stopBtn.Location = new Point(626, 8);
            stopBtn.Name = "stopBtn";
            stopBtn.Size = new Size(80, 30);
            stopBtn.TabIndex = 3;
            stopBtn.Text = "停止";
            stopBtn.UseVisualStyleBackColor = true;
            stopBtn.Click += stopBtn_Click;

            // scanProgressBar
            //
            scanProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            scanProgressBar.Location = new Point(8, 48);
            scanProgressBar.Name = "scanProgressBar";
            scanProgressBar.Size = new Size(752, 23);
            scanProgressBar.TabIndex = 4;

            // folderTreeView
            //
            folderTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            folderTreeView.Location = new Point(8, 80);
            folderTreeView.Name = "folderTreeView";
            folderTreeView.Size = new Size(752, 253);
            folderTreeView.TabIndex = 5;

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

            // watchStatusLabel
            //
            watchStatusLabel.BackColor = SystemColors.Control;
            watchStatusLabel.Name = "watchStatusLabel";
            watchStatusLabel.Size = new Size(100, 24);
            watchStatusLabel.Text = "未开始监测";
            watchStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            watchStatusLabel.Click += watchStatusLabel_Click;

            // writedRecordStatusLabel
            //
            writedRecordStatusLabel.BackColor = SystemColors.Control;
            writedRecordStatusLabel.Margin = new Padding(100, 4, 0, 3);
            writedRecordStatusLabel.Name = "writedRecordStatusLabel";
            writedRecordStatusLabel.Padding = new Padding(100, 0, 0, 0);
            writedRecordStatusLabel.Size = new Size(229, 24);
            writedRecordStatusLabel.Text = "已记录 0 条";
            writedRecordStatusLabel.Click += WritedRecordStatusLabel_Click;

            // timeStatusLabel
            //
            timeStatusLabel.Margin = new Padding(95, 4, 0, 3);
            timeStatusLabel.Name = "timeStatusLabel";
            timeStatusLabel.Size = new Size(189, 24);
            timeStatusLabel.Text = "2026-07-28              ";
            timeStatusLabel.TextAlign = ContentAlignment.MiddleRight;

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
            // diskRefreshTimer
            //
            diskRefreshTimer.Interval = 30000;
            diskRefreshTimer.Tick += diskRefreshTimer_Tick;

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

            // splitContainer1.Panel1
            //
            splitContainer1.Panel1.Controls.Add(label1);

            // splitContainer1.Panel2
            //
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Panel2.Controls.Add(BiggerButton);
            splitContainer1.Panel2.Controls.Add(closeButton);
            splitContainer1.Size = new Size(784, 47);
            splitContainer1.SplitterDistance = 466;
            splitContainer1.TabIndex = 5;

            //
            // label1
            //
            label1.AutoSize = true;
            label1.Image = Properties.Resources.清理工具图标;
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(16, 11);
            label1.Name = "label1";
            label1.Size = new Size(142, 24);
            label1.TabIndex = 0;
            label1.Text = "      C盘管理工具";
            label1.TextAlign = ContentAlignment.MiddleRight;

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
            Name = "HideButton";
            Text = "      C盘监测工具";
            Load += Form1_Load;
            TabPageControl1.ResumeLayout(false);
            totalReviewPage.ResumeLayout(false);
            totalReviewPage.PerformLayout();
            watcherPage.ResumeLayout(false);
            watcherPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).EndInit();
            folderAnalyzerPage.ResumeLayout(false);
            folderAnalyzerPage.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // 主容器
        private TabControl TabPageControl1;
        private TabPage totalReviewPage;
        private TabPage watcherPage;
        private TabPage folderAnalyzerPage;

        // 概览 tab
        private Label dashboardTitleLabel;
        private ProgressBar usageProgressBar;
        private Label totalSpaceLabel;
        private Label usedSpaceLabel;
        private Label freeSpaceLabel;
        private Label warningLabel;

        // 实时监测 tab
        private Button pauseBtn;
        private Button clearBtn;
        private Button exportBtn;
        private Label typeFilterLabel;
        private ComboBox typeFilterCombo;
        private DataGridView changesDataGrid;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn TypeColumn;
        private DataGridViewTextBoxColumn FileNameColumn;
        private DataGridViewTextBoxColumn PathColumn;
        private DataGridViewTextBoxColumn SizeColumn;

        // 文件夹分析 tab
        private TextBox selectedPathTextBox;
        private Button selectDirBtn;
        private Button scanBtn;
        private Button stopBtn;
        private ProgressBar scanProgressBar;
        private TreeView folderTreeView;

        // 标题栏
        private Button closeButton;
        private Button BiggerButton;
        private Button button1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel watchStatusLabel;
        private ToolStripStatusLabel writedRecordStatusLabel;
        private ToolStripStatusLabel timeStatusLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer diskRefreshTimer;
        private ToolStripStatusLabel NoticeIcon;
        private NotifyIcon notifyIcon1;
        private SplitContainer splitContainer1;
        private Label label1;
    }
}
