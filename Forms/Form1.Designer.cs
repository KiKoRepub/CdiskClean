    namespace CdiskClean
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TabPageControl1 = new TabControl();
            totalReviewPage = new TabPage();
            progressBar1 = new ProgressBar();
            warningLabel = new Label();
            freeSpaceLabel = new Label();
            usedSpaceLabel = new Label();
            totalSpaceLabel = new Label();
            usageProgressBar = new ProgressBar();
            dashboardTitleLabel = new Label();
            watcherPage = new TabPage();
            ignoreProcessBox = new GroupBox();
            betterProcessAddButton = new Button();
            processAddButton = new Button();
            procSelectedTextBox = new TextBox();
            label3 = new Label();
            ignoreProcessView = new ListView();
            statisticButton = new Button();
            WatcherDirectoryBox = new GroupBox();
            betterDirAddButton = new Button();
            dirSelectedTextBox = new TextBox();
            watcherDirListView = new ListView();
            label2 = new Label();
            dirAddButton = new Button();
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
            SourceColumn = new DataGridViewTextBoxColumn();
            folderAnalyzerPage = new TabPage();
            folderTreeView = new TreeView();
            scanProgressBar = new ProgressBar();
            stopBtn = new Button();
            scanBtn = new Button();
            selectDirBtn = new Button();
            selectedPathTextBox = new TextBox();
            diskCleanPage = new TabPage();
            cleanHistoryBox = new GroupBox();
            cleanHistoryGrid = new DataGridView();
            cleanMethodBox = new GroupBox();
            cleanRecycleRadio = new RadioButton();
            cleanPermanentRadio = new RadioButton();
            cleanMoveRadio = new RadioButton();
            cleanCompressRadio = new RadioButton();
            cleanMklinkRadio = new RadioButton();
            cleanTargetLabel = new Label();
            cleanTargetTextBox = new TextBox();
            cleanTargetSelectBtn = new Button();
            cleanBtn = new Button();
            cleanTreeBox = new GroupBox();
            cleanTreeView = new TreeView();
            cleanSelectAllBtn = new Button();
            cleanSelectNoneBtn = new Button();
            cleanStatusLabel = new Label();
            frequentPathBox = new GroupBox();
            frequentPathListView = new ListView();
            frequentHintLabel = new Label();
            cleanRefreshFrequentBtn = new Button();
            cleanScanProgressBar = new ProgressBar();
            cleanScanBtn = new Button();
            cleanSelectDirBtn = new Button();
            cleanPathTextBox = new TextBox();
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
            notifyMenuStrip = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            panelTitle = new SplitContainer();
            label1 = new Label();
            ImportFolderDialog = new FolderBrowserDialog();
            TabPageControl1.SuspendLayout();
            totalReviewPage.SuspendLayout();
            watcherPage.SuspendLayout();
            ignoreProcessBox.SuspendLayout();
            WatcherDirectoryBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).BeginInit();
            folderAnalyzerPage.SuspendLayout();
            diskCleanPage.SuspendLayout();
            cleanHistoryBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).BeginInit();
            cleanMethodBox.SuspendLayout();
            cleanTreeBox.SuspendLayout();
            frequentPathBox.SuspendLayout();
            statusStrip1.SuspendLayout();
            notifyMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelTitle).BeginInit();
            panelTitle.Panel1.SuspendLayout();
            panelTitle.Panel2.SuspendLayout();
            panelTitle.SuspendLayout();
            SuspendLayout();
            // 
            // TabPageControl1
            // 
            TabPageControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TabPageControl1.Controls.Add(totalReviewPage);
            TabPageControl1.Controls.Add(watcherPage);
            TabPageControl1.Controls.Add(folderAnalyzerPage);
            TabPageControl1.Controls.Add(diskCleanPage);
            TabPageControl1.Location = new Point(0, 53);
            TabPageControl1.Name = "TabPageControl1";
            TabPageControl1.SelectedIndex = 0;
            TabPageControl1.Size = new Size(1437, 693);
            TabPageControl1.TabIndex = 0;
            // 
            // totalReviewPage
            // 
            totalReviewPage.Controls.Add(progressBar1);
            totalReviewPage.Controls.Add(warningLabel);
            totalReviewPage.Controls.Add(freeSpaceLabel);
            totalReviewPage.Controls.Add(usedSpaceLabel);
            totalReviewPage.Controls.Add(totalSpaceLabel);
            totalReviewPage.Controls.Add(usageProgressBar);
            totalReviewPage.Controls.Add(dashboardTitleLabel);
            totalReviewPage.Location = new Point(4, 33);
            totalReviewPage.Name = "totalReviewPage";
            totalReviewPage.Padding = new Padding(3);
            totalReviewPage.Size = new Size(1429, 656);
            totalReviewPage.TabIndex = 0;
            totalReviewPage.Text = "概览";
            totalReviewPage.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(248, 412);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(8, 8);
            progressBar1.TabIndex = 6;
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 134);
            warningLabel.ForeColor = Color.Red;
            warningLabel.Location = new Point(20, 250);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(398, 27);
            warningLabel.TabIndex = 5;
            warningLabel.Text = "⚠ C盘剩余空间不足 10GB，请及时清理！";
            warningLabel.Visible = false;
            // 
            // freeSpaceLabel
            // 
            freeSpaceLabel.AutoSize = true;
            freeSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            freeSpaceLabel.Location = new Point(20, 200);
            freeSpaceLabel.Name = "freeSpaceLabel";
            freeSpaceLabel.Size = new Size(138, 27);
            freeSpaceLabel.TabIndex = 4;
            freeSpaceLabel.Text = "剩余: 加载中...";
            // 
            // usedSpaceLabel
            // 
            usedSpaceLabel.AutoSize = true;
            usedSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            usedSpaceLabel.Location = new Point(20, 160);
            usedSpaceLabel.Name = "usedSpaceLabel";
            usedSpaceLabel.Size = new Size(138, 27);
            usedSpaceLabel.TabIndex = 3;
            usedSpaceLabel.Text = "已用: 加载中...";
            // 
            // totalSpaceLabel
            // 
            totalSpaceLabel.AutoSize = true;
            totalSpaceLabel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            totalSpaceLabel.Location = new Point(20, 120);
            totalSpaceLabel.Name = "totalSpaceLabel";
            totalSpaceLabel.Size = new Size(158, 27);
            totalSpaceLabel.TabIndex = 2;
            totalSpaceLabel.Text = "总容量: 加载中...";
            // 
            // usageProgressBar
            // 
            usageProgressBar.Location = new Point(20, 70);
            usageProgressBar.Name = "usageProgressBar";
            usageProgressBar.Size = new Size(711, 30);
            usageProgressBar.TabIndex = 1;
            // 
            // dashboardTitleLabel
            // 
            dashboardTitleLabel.AutoSize = true;
            dashboardTitleLabel.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            dashboardTitleLabel.Location = new Point(20, 15);
            dashboardTitleLabel.Name = "dashboardTitleLabel";
            dashboardTitleLabel.Size = new Size(150, 31);
            dashboardTitleLabel.TabIndex = 0;
            dashboardTitleLabel.Text = "C盘空间概况";
            // 
            // watcherPage
            // 
            watcherPage.Controls.Add(ignoreProcessBox);
            watcherPage.Controls.Add(statisticButton);
            watcherPage.Controls.Add(WatcherDirectoryBox);
            watcherPage.Controls.Add(exportBtn);
            watcherPage.Controls.Add(typeFilterCombo);
            watcherPage.Controls.Add(typeFilterLabel);
            watcherPage.Controls.Add(clearBtn);
            watcherPage.Controls.Add(pauseBtn);
            watcherPage.Controls.Add(changesDataGrid);
            watcherPage.Location = new Point(4, 33);
            watcherPage.Name = "watcherPage";
            watcherPage.Padding = new Padding(3);
            watcherPage.Size = new Size(1429, 656);
            watcherPage.TabIndex = 1;
            watcherPage.Text = "实时监测";
            watcherPage.UseVisualStyleBackColor = true;
            // 
            // ignoreProcessBox
            // 
            ignoreProcessBox.Controls.Add(betterProcessAddButton);
            ignoreProcessBox.Controls.Add(processAddButton);
            ignoreProcessBox.Controls.Add(procSelectedTextBox);
            ignoreProcessBox.Controls.Add(label3);
            ignoreProcessBox.Controls.Add(ignoreProcessView);
            ignoreProcessBox.Location = new Point(848, 318);
            ignoreProcessBox.Name = "ignoreProcessBox";
            ignoreProcessBox.Size = new Size(559, 323);
            ignoreProcessBox.TabIndex = 13;
            ignoreProcessBox.TabStop = false;
            ignoreProcessBox.Text = "忽略进程列表";
            // 
            // betterProcessAddButton
            // 
            betterProcessAddButton.Font = new Font("Microsoft YaHei UI", 11F);
            betterProcessAddButton.Location = new Point(259, 22);
            betterProcessAddButton.Name = "betterProcessAddButton";
            betterProcessAddButton.Size = new Size(115, 44);
            betterProcessAddButton.TabIndex = 11;
            betterProcessAddButton.Text = "高级添加";
            betterProcessAddButton.UseVisualStyleBackColor = true;
            betterProcessAddButton.Click += betterProcessAddButton_Click;
            // 
            // processAddButton
            // 
            processAddButton.Font = new Font("Microsoft YaHei UI", 11F);
            processAddButton.Location = new Point(380, 23);
            processAddButton.Name = "processAddButton";
            processAddButton.Size = new Size(159, 43);
            processAddButton.TabIndex = 3;
            processAddButton.Text = "添加忽略进程";
            processAddButton.UseVisualStyleBackColor = true;
            processAddButton.Click += ProcessAddButton_Click;
            // 
            // procSelectedTextBox
            // 
            procSelectedTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            procSelectedTextBox.Location = new Point(191, 250);
            procSelectedTextBox.Multiline = true;
            procSelectedTextBox.Name = "procSelectedTextBox";
            procSelectedTextBox.ReadOnly = true;
            procSelectedTextBox.Size = new Size(357, 49);
            procSelectedTextBox.TabIndex = 2;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 15F);
            label3.Location = new Point(18, 252);
            label3.Name = "label3";
            label3.Size = new Size(167, 39);
            label3.TabIndex = 1;
            label3.Text = "选中进程为";
            // 
            // ignoreProcessView
            // 
            ignoreProcessView.AllowDrop = true;
            ignoreProcessView.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ignoreProcessView.Location = new Point(18, 72);
            ignoreProcessView.Name = "ignoreProcessView";
            ignoreProcessView.Size = new Size(521, 159);
            ignoreProcessView.TabIndex = 0;
            ignoreProcessView.UseCompatibleStateImageBehavior = false;
            ignoreProcessView.ItemSelectionChanged += ignoreProcessView_ItemSelectionChanged;
            ignoreProcessView.DragDrop += ignoreProcessView_DragDrop;
            ignoreProcessView.DragEnter += ignoreProcessView_DragEnter;
            ignoreProcessView.Resize += ignoreProcessView_Resize;
            // 
            // statisticButton
            // 
            statisticButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            statisticButton.Location = new Point(12, 584);
            statisticButton.Name = "statisticButton";
            statisticButton.Size = new Size(139, 47);
            statisticButton.TabIndex = 11;
            statisticButton.Text = "查看详情记录";
            statisticButton.UseVisualStyleBackColor = true;
            statisticButton.Click += statisticButton_Click;
            // 
            // WatcherDirectoryBox
            // 
            WatcherDirectoryBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            WatcherDirectoryBox.Controls.Add(betterDirAddButton);
            WatcherDirectoryBox.Controls.Add(dirSelectedTextBox);
            WatcherDirectoryBox.Controls.Add(watcherDirListView);
            WatcherDirectoryBox.Controls.Add(label2);
            WatcherDirectoryBox.Controls.Add(dirAddButton);
            WatcherDirectoryBox.Location = new Point(848, 10);
            WatcherDirectoryBox.Name = "WatcherDirectoryBox";
            WatcherDirectoryBox.Size = new Size(559, 288);
            WatcherDirectoryBox.TabIndex = 7;
            WatcherDirectoryBox.TabStop = false;
            WatcherDirectoryBox.Text = "监测目录列表";
            // 
            // betterDirAddButton
            // 
            betterDirAddButton.Font = new Font("Microsoft YaHei UI", 11F);
            betterDirAddButton.Location = new Point(306, 23);
            betterDirAddButton.Name = "betterDirAddButton";
            betterDirAddButton.Size = new Size(115, 46);
            betterDirAddButton.TabIndex = 10;
            betterDirAddButton.Text = "高级添加";
            betterDirAddButton.UseVisualStyleBackColor = true;
            betterDirAddButton.Click += betterDirAddButton_Click;
            // 
            // dirSelectedTextBox
            // 
            dirSelectedTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            dirSelectedTextBox.Location = new Point(181, 219);
            dirSelectedTextBox.Multiline = true;
            dirSelectedTextBox.Name = "dirSelectedTextBox";
            dirSelectedTextBox.ReadOnly = true;
            dirSelectedTextBox.Size = new Size(358, 52);
            dirSelectedTextBox.TabIndex = 9;
            // 
            // watcherDirListView
            // 
            watcherDirListView.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            watcherDirListView.Location = new Point(34, 75);
            watcherDirListView.Name = "watcherDirListView";
            watcherDirListView.Size = new Size(505, 138);
            watcherDirListView.TabIndex = 6;
            watcherDirListView.UseCompatibleStateImageBehavior = false;
            watcherDirListView.View = View.Tile;
            watcherDirListView.ItemSelectionChanged += watcherDirListView_ItemSelectionChanged;
            watcherDirListView.Resize += watcherDirListView_Resize;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 15F);
            label2.Location = new Point(8, 219);
            label2.Name = "label2";
            label2.Size = new Size(167, 39);
            label2.TabIndex = 8;
            label2.Text = "选中目录为";
            // 
            // dirAddButton
            // 
            dirAddButton.Font = new Font("Microsoft YaHei UI", 11F);
            dirAddButton.Location = new Point(427, 23);
            dirAddButton.Name = "dirAddButton";
            dirAddButton.Size = new Size(112, 46);
            dirAddButton.TabIndex = 11;
            dirAddButton.Text = "添加目录";
            dirAddButton.UseVisualStyleBackColor = true;
            dirAddButton.Click += dirAddButton_Click;
            // 
            // exportBtn
            // 
            exportBtn.Location = new Point(219, 8);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(80, 30);
            exportBtn.TabIndex = 2;
            exportBtn.Text = "导出";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;
            // 
            // typeFilterCombo
            // 
            typeFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            typeFilterCombo.FormattingEnabled = true;
            typeFilterCombo.Items.AddRange(new object[] { "全部", "创建", "修改", "删除", "重命名" });
            typeFilterCombo.Location = new Point(567, 8);
            typeFilterCombo.Name = "typeFilterCombo";
            typeFilterCombo.Size = new Size(121, 32);
            typeFilterCombo.TabIndex = 4;
            typeFilterCombo.SelectedIndexChanged += typeFilterCombo_SelectedIndexChanged;
            // 
            // typeFilterLabel
            // 
            typeFilterLabel.AutoSize = true;
            typeFilterLabel.Location = new Point(475, 10);
            typeFilterLabel.Name = "typeFilterLabel";
            typeFilterLabel.Size = new Size(86, 24);
            typeFilterLabel.TabIndex = 3;
            typeFilterLabel.Text = "筛选类型:";
            typeFilterLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // clearBtn
            // 
            clearBtn.Location = new Point(133, 8);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(80, 30);
            clearBtn.TabIndex = 1;
            clearBtn.Text = "清空";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += clearBtn_Click;
            // 
            // pauseBtn
            // 
            pauseBtn.Location = new Point(12, 7);
            pauseBtn.Name = "pauseBtn";
            pauseBtn.Size = new Size(115, 30);
            pauseBtn.TabIndex = 0;
            pauseBtn.Text = "开始监测";
            pauseBtn.UseVisualStyleBackColor = true;
            pauseBtn.Click += pauseBtn_Click;
            // 
            // changesDataGrid
            // 
            changesDataGrid.AllowUserToAddRows = false;
            changesDataGrid.AllowUserToDeleteRows = false;
            changesDataGrid.AllowUserToResizeRows = false;
            changesDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            changesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            changesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            changesDataGrid.Columns.AddRange(new DataGridViewColumn[] { TimeColumn, TypeColumn, FileNameColumn, PathColumn, SizeColumn, SourceColumn });
            changesDataGrid.Location = new Point(12, 48);
            changesDataGrid.Name = "changesDataGrid";
            changesDataGrid.ReadOnly = true;
            changesDataGrid.RowHeadersVisible = false;
            changesDataGrid.RowHeadersWidth = 62;
            changesDataGrid.RowTemplate.Height = 25;
            changesDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            changesDataGrid.Size = new Size(676, 520);
            changesDataGrid.TabIndex = 5;
            changesDataGrid.MouseDown += changesDataGrid_MouseDown;
            changesDataGrid.MouseMove += changesDataGrid_MouseMove;
            changesDataGrid.MouseUp += changesDataGrid_MouseUp;
            // 
            // TimeColumn
            // 
            TimeColumn.DataPropertyName = "TimeStamp";
            TimeColumn.FillWeight = 80F;
            TimeColumn.HeaderText = "时间";
            TimeColumn.MinimumWidth = 8;
            TimeColumn.Name = "TimeColumn";
            TimeColumn.ReadOnly = true;
            // 
            // TypeColumn
            // 
            TypeColumn.DataPropertyName = "ChangeType";
            TypeColumn.FillWeight = 50F;
            TypeColumn.HeaderText = "类型";
            TypeColumn.MinimumWidth = 8;
            TypeColumn.Name = "TypeColumn";
            TypeColumn.ReadOnly = true;
            // 
            // FileNameColumn
            // 
            FileNameColumn.DataPropertyName = "FileName";
            FileNameColumn.FillWeight = 70F;
            FileNameColumn.HeaderText = "文件名";
            FileNameColumn.MinimumWidth = 8;
            FileNameColumn.Name = "FileNameColumn";
            FileNameColumn.ReadOnly = true;
            // 
            // PathColumn
            // 
            PathColumn.DataPropertyName = "FullPath";
            PathColumn.FillWeight = 120F;
            PathColumn.HeaderText = "路径";
            PathColumn.MinimumWidth = 8;
            PathColumn.Name = "PathColumn";
            PathColumn.ReadOnly = true;
            // 
            // SizeColumn
            // 
            SizeColumn.DataPropertyName = "SizeBytes";
            SizeColumn.FillWeight = 50F;
            SizeColumn.HeaderText = "大小";
            SizeColumn.MinimumWidth = 8;
            SizeColumn.Name = "SizeColumn";
            SizeColumn.ReadOnly = true;
            // 
            // SourceColumn
            // 
            SourceColumn.DataPropertyName = "SourceProcess";
            SourceColumn.FillWeight = 60F;
            SourceColumn.HeaderText = "来源进程";
            SourceColumn.MinimumWidth = 8;
            SourceColumn.Name = "SourceColumn";
            SourceColumn.ReadOnly = true;
            // 
            // folderAnalyzerPage
            // 
            folderAnalyzerPage.Controls.Add(folderTreeView);
            folderAnalyzerPage.Controls.Add(scanProgressBar);
            folderAnalyzerPage.Controls.Add(stopBtn);
            folderAnalyzerPage.Controls.Add(scanBtn);
            folderAnalyzerPage.Controls.Add(selectDirBtn);
            folderAnalyzerPage.Controls.Add(selectedPathTextBox);
            folderAnalyzerPage.Location = new Point(4, 33);
            folderAnalyzerPage.Name = "folderAnalyzerPage";
            folderAnalyzerPage.Size = new Size(1429, 656);
            folderAnalyzerPage.TabIndex = 2;
            folderAnalyzerPage.Text = "文件夹分析";
            folderAnalyzerPage.UseVisualStyleBackColor = true;
            // 
            // folderTreeView
            // 
            folderTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            folderTreeView.Location = new Point(8, 80);
            folderTreeView.Name = "folderTreeView";
            folderTreeView.Size = new Size(678, 256);
            folderTreeView.TabIndex = 5;
            // 
            // scanProgressBar
            // 
            scanProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            scanProgressBar.Location = new Point(8, 48);
            scanProgressBar.Name = "scanProgressBar";
            scanProgressBar.Size = new Size(678, 23);
            scanProgressBar.TabIndex = 4;
            // 
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
            // 
            // scanBtn
            // 
            scanBtn.Location = new Point(520, 8);
            scanBtn.Name = "scanBtn";
            scanBtn.Size = new Size(100, 30);
            scanBtn.TabIndex = 2;
            scanBtn.Text = "开始扫描";
            scanBtn.UseVisualStyleBackColor = true;
            scanBtn.Click += scanBtn_Click;
            // 
            // selectDirBtn
            // 
            selectDirBtn.Location = new Point(414, 8);
            selectDirBtn.Name = "selectDirBtn";
            selectDirBtn.Size = new Size(100, 30);
            selectDirBtn.TabIndex = 1;
            selectDirBtn.Text = "选择目录";
            selectDirBtn.UseVisualStyleBackColor = true;
            selectDirBtn.Click += selectDirBtn_Click;
            // 
            // selectedPathTextBox
            // 
            selectedPathTextBox.Location = new Point(8, 10);
            selectedPathTextBox.Name = "selectedPathTextBox";
            selectedPathTextBox.ReadOnly = true;
            selectedPathTextBox.Size = new Size(400, 30);
            selectedPathTextBox.TabIndex = 0;
            // 
            // diskCleanPage
            // 
            diskCleanPage.Controls.Add(cleanHistoryBox);
            diskCleanPage.Controls.Add(cleanMethodBox);
            diskCleanPage.Controls.Add(cleanTreeBox);
            diskCleanPage.Controls.Add(frequentPathBox);
            diskCleanPage.Controls.Add(cleanRefreshFrequentBtn);
            diskCleanPage.Controls.Add(cleanScanProgressBar);
            diskCleanPage.Controls.Add(cleanScanBtn);
            diskCleanPage.Controls.Add(cleanSelectDirBtn);
            diskCleanPage.Controls.Add(cleanPathTextBox);
            diskCleanPage.Location = new Point(4, 33);
            diskCleanPage.Name = "diskCleanPage";
            diskCleanPage.Padding = new Padding(3);
            diskCleanPage.Size = new Size(1429, 656);
            diskCleanPage.TabIndex = 3;
            diskCleanPage.Text = "磁盘清理";
            diskCleanPage.UseVisualStyleBackColor = true;
            // 
            // cleanHistoryBox
            // 
            cleanHistoryBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cleanHistoryBox.Controls.Add(cleanHistoryGrid);
            cleanHistoryBox.Location = new Point(8, 490);
            cleanHistoryBox.Name = "cleanHistoryBox";
            cleanHistoryBox.Size = new Size(1413, 150);
            cleanHistoryBox.TabIndex = 8;
            cleanHistoryBox.TabStop = false;
            cleanHistoryBox.Text = "清理历史记录";
            // 
            // cleanHistoryGrid
            // 
            cleanHistoryGrid.AllowUserToAddRows = false;
            cleanHistoryGrid.AllowUserToDeleteRows = false;
            cleanHistoryGrid.AllowUserToResizeRows = false;
            cleanHistoryGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cleanHistoryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cleanHistoryGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cleanHistoryGrid.Location = new Point(14, 28);
            cleanHistoryGrid.Name = "cleanHistoryGrid";
            cleanHistoryGrid.ReadOnly = true;
            cleanHistoryGrid.RowHeadersVisible = false;
            cleanHistoryGrid.RowHeadersWidth = 62;
            cleanHistoryGrid.RowTemplate.Height = 25;
            cleanHistoryGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cleanHistoryGrid.Size = new Size(1385, 110);
            cleanHistoryGrid.TabIndex = 0;
            // 
            // cleanMethodBox
            // 
            cleanMethodBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cleanMethodBox.Controls.Add(cleanRecycleRadio);
            cleanMethodBox.Controls.Add(cleanPermanentRadio);
            cleanMethodBox.Controls.Add(cleanMoveRadio);
            cleanMethodBox.Controls.Add(cleanCompressRadio);
            cleanMethodBox.Controls.Add(cleanMklinkRadio);
            cleanMethodBox.Controls.Add(cleanTargetLabel);
            cleanMethodBox.Controls.Add(cleanTargetTextBox);
            cleanMethodBox.Controls.Add(cleanTargetSelectBtn);
            cleanMethodBox.Controls.Add(cleanBtn);
            cleanMethodBox.Location = new Point(8, 390);
            cleanMethodBox.Name = "cleanMethodBox";
            cleanMethodBox.Size = new Size(1413, 90);
            cleanMethodBox.TabIndex = 7;
            cleanMethodBox.TabStop = false;
            cleanMethodBox.Text = "清理方式";
            // 
            // cleanRecycleRadio
            // 
            cleanRecycleRadio.AutoSize = true;
            cleanRecycleRadio.Checked = true;
            cleanRecycleRadio.Location = new Point(16, 28);
            cleanRecycleRadio.Name = "cleanRecycleRadio";
            cleanRecycleRadio.Size = new Size(215, 28);
            cleanRecycleRadio.TabIndex = 0;
            cleanRecycleRadio.TabStop = true;
            cleanRecycleRadio.Text = "回收站删除（可恢复）";
            cleanRecycleRadio.UseVisualStyleBackColor = true;
            cleanRecycleRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanPermanentRadio
            // 
            cleanPermanentRadio.AutoSize = true;
            cleanPermanentRadio.Location = new Point(233, 28);
            cleanPermanentRadio.Name = "cleanPermanentRadio";
            cleanPermanentRadio.Size = new Size(107, 28);
            cleanPermanentRadio.TabIndex = 1;
            cleanPermanentRadio.TabStop = true;
            cleanPermanentRadio.Text = "永久删除";
            cleanPermanentRadio.UseVisualStyleBackColor = true;
            cleanPermanentRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanMoveRadio
            // 
            cleanMoveRadio.AutoSize = true;
            cleanMoveRadio.Location = new Point(351, 28);
            cleanMoveRadio.Name = "cleanMoveRadio";
            cleanMoveRadio.Size = new Size(89, 28);
            cleanMoveRadio.TabIndex = 2;
            cleanMoveRadio.TabStop = true;
            cleanMoveRadio.Text = "移动到";
            cleanMoveRadio.UseVisualStyleBackColor = true;
            cleanMoveRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanCompressRadio
            // 
            cleanCompressRadio.AutoSize = true;
            cleanCompressRadio.Location = new Point(462, 28);
            cleanCompressRadio.Name = "cleanCompressRadio";
            cleanCompressRadio.Size = new Size(89, 28);
            cleanCompressRadio.TabIndex = 3;
            cleanCompressRadio.TabStop = true;
            cleanCompressRadio.Text = "压缩到";
            cleanCompressRadio.UseVisualStyleBackColor = true;
            cleanCompressRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanMklinkRadio
            // 
            cleanMklinkRadio.AutoSize = true;
            cleanMklinkRadio.Location = new Point(584, 28);
            cleanMklinkRadio.Name = "cleanMklinkRadio";
            cleanMklinkRadio.Size = new Size(174, 28);
            cleanMklinkRadio.TabIndex = 4;
            cleanMklinkRadio.TabStop = true;
            cleanMklinkRadio.Text = "mkLink 软链接到";
            cleanMklinkRadio.UseVisualStyleBackColor = true;
            cleanMklinkRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanTargetLabel
            // 
            cleanTargetLabel.AutoSize = true;
            cleanTargetLabel.Location = new Point(16, 61);
            cleanTargetLabel.Name = "cleanTargetLabel";
            cleanTargetLabel.Size = new Size(86, 24);
            cleanTargetLabel.TabIndex = 5;
            cleanTargetLabel.Text = "目标目录:";
            // 
            // cleanTargetTextBox
            // 
            cleanTargetTextBox.Location = new Point(106, 57);
            cleanTargetTextBox.Name = "cleanTargetTextBox";
            cleanTargetTextBox.Size = new Size(500, 30);
            cleanTargetTextBox.TabIndex = 6;
            // 
            // cleanTargetSelectBtn
            // 
            cleanTargetSelectBtn.Location = new Point(614, 55);
            cleanTargetSelectBtn.Name = "cleanTargetSelectBtn";
            cleanTargetSelectBtn.Size = new Size(90, 30);
            cleanTargetSelectBtn.TabIndex = 7;
            cleanTargetSelectBtn.Text = "浏览";
            cleanTargetSelectBtn.UseVisualStyleBackColor = true;
            cleanTargetSelectBtn.Click += cleanTargetSelectBtn_Click;
            // 
            // cleanBtn
            // 
            cleanBtn.Location = new Point(1290, 24);
            cleanBtn.Name = "cleanBtn";
            cleanBtn.Size = new Size(110, 58);
            cleanBtn.TabIndex = 8;
            cleanBtn.Text = "清理选中文件";
            cleanBtn.UseVisualStyleBackColor = true;
            cleanBtn.Click += cleanBtn_Click;
            // 
            // cleanTreeBox
            // 
            cleanTreeBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cleanTreeBox.Controls.Add(cleanTreeView);
            cleanTreeBox.Controls.Add(cleanSelectAllBtn);
            cleanTreeBox.Controls.Add(cleanSelectNoneBtn);
            cleanTreeBox.Controls.Add(cleanStatusLabel);
            cleanTreeBox.Location = new Point(376, 50);
            cleanTreeBox.Name = "cleanTreeBox";
            cleanTreeBox.Size = new Size(1045, 330);
            cleanTreeBox.TabIndex = 6;
            cleanTreeBox.TabStop = false;
            cleanTreeBox.Text = "扫描结果（勾选要清理的文件）";
            // 
            // cleanTreeView
            // 
            cleanTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cleanTreeView.CheckBoxes = true;
            cleanTreeView.Location = new Point(14, 28);
            cleanTreeView.Name = "cleanTreeView";
            cleanTreeView.Size = new Size(1015, 250);
            cleanTreeView.TabIndex = 0;
            cleanTreeView.BeforeCheck += cleanTreeView_BeforeCheck;
            cleanTreeView.AfterCheck += cleanTreeView_AfterCheck;
            // 
            // cleanSelectAllBtn
            // 
            cleanSelectAllBtn.Location = new Point(14, 286);
            cleanSelectAllBtn.Name = "cleanSelectAllBtn";
            cleanSelectAllBtn.Size = new Size(70, 30);
            cleanSelectAllBtn.TabIndex = 1;
            cleanSelectAllBtn.Text = "全选";
            cleanSelectAllBtn.UseVisualStyleBackColor = true;
            cleanSelectAllBtn.Click += cleanSelectAllBtn_Click;
            // 
            // cleanSelectNoneBtn
            // 
            cleanSelectNoneBtn.Location = new Point(90, 286);
            cleanSelectNoneBtn.Name = "cleanSelectNoneBtn";
            cleanSelectNoneBtn.Size = new Size(70, 30);
            cleanSelectNoneBtn.TabIndex = 2;
            cleanSelectNoneBtn.Text = "全不选";
            cleanSelectNoneBtn.UseVisualStyleBackColor = true;
            cleanSelectNoneBtn.Click += cleanSelectNoneBtn_Click;
            // 
            // cleanStatusLabel
            // 
            cleanStatusLabel.Location = new Point(170, 290);
            cleanStatusLabel.Name = "cleanStatusLabel";
            cleanStatusLabel.Size = new Size(850, 26);
            cleanStatusLabel.TabIndex = 3;
            cleanStatusLabel.Text = "请选择目录并开始扫描";
            cleanStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frequentPathBox
            // 
            frequentPathBox.Controls.Add(frequentPathListView);
            frequentPathBox.Controls.Add(frequentHintLabel);
            frequentPathBox.Location = new Point(8, 50);
            frequentPathBox.Name = "frequentPathBox";
            frequentPathBox.Size = new Size(360, 330);
            frequentPathBox.TabIndex = 5;
            frequentPathBox.TabStop = false;
            frequentPathBox.Text = "高频修改文件路径参考";
            // 
            // frequentPathListView
            // 
            frequentPathListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            frequentPathListView.Location = new Point(14, 30);
            frequentPathListView.Name = "frequentPathListView";
            frequentPathListView.Size = new Size(332, 255);
            frequentPathListView.TabIndex = 0;
            frequentPathListView.UseCompatibleStateImageBehavior = false;
            frequentPathListView.ItemSelectionChanged += frequentPathListView_ItemSelectionChanged;
            frequentPathListView.MouseDoubleClick += frequentPathListView_MouseDoubleClick;
            // 
            // frequentHintLabel
            // 
            frequentHintLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            frequentHintLabel.ForeColor = Color.Gray;
            frequentHintLabel.Location = new Point(14, 292);
            frequentHintLabel.Name = "frequentHintLabel";
            frequentHintLabel.Size = new Size(332, 25);
            frequentHintLabel.TabIndex = 1;
            frequentHintLabel.Text = "单击选中基础路径，双击开始扫描";
            frequentHintLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanRefreshFrequentBtn
            // 
            cleanRefreshFrequentBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cleanRefreshFrequentBtn.Location = new Point(1210, 8);
            cleanRefreshFrequentBtn.Name = "cleanRefreshFrequentBtn";
            cleanRefreshFrequentBtn.Size = new Size(100, 30);
            cleanRefreshFrequentBtn.TabIndex = 4;
            cleanRefreshFrequentBtn.Text = "刷新高频参考";
            cleanRefreshFrequentBtn.UseVisualStyleBackColor = true;
            cleanRefreshFrequentBtn.Click += cleanRefreshFrequentBtn_Click;
            // 
            // cleanScanProgressBar
            // 
            cleanScanProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cleanScanProgressBar.Location = new Point(582, 8);
            cleanScanProgressBar.Name = "cleanScanProgressBar";
            cleanScanProgressBar.Size = new Size(620, 23);
            cleanScanProgressBar.TabIndex = 3;
            // 
            // cleanScanBtn
            // 
            cleanScanBtn.Location = new Point(474, 8);
            cleanScanBtn.Name = "cleanScanBtn";
            cleanScanBtn.Size = new Size(100, 30);
            cleanScanBtn.TabIndex = 2;
            cleanScanBtn.Text = "开始扫描";
            cleanScanBtn.UseVisualStyleBackColor = true;
            cleanScanBtn.Click += cleanScanBtn_Click;
            // 
            // cleanSelectDirBtn
            // 
            cleanSelectDirBtn.Location = new Point(366, 8);
            cleanSelectDirBtn.Name = "cleanSelectDirBtn";
            cleanSelectDirBtn.Size = new Size(100, 30);
            cleanSelectDirBtn.TabIndex = 1;
            cleanSelectDirBtn.Text = "选择目录";
            cleanSelectDirBtn.UseVisualStyleBackColor = true;
            cleanSelectDirBtn.Click += cleanSelectDirBtn_Click;
            // 
            // cleanPathTextBox
            // 
            cleanPathTextBox.Location = new Point(8, 10);
            cleanPathTextBox.Name = "cleanPathTextBox";
            cleanPathTextBox.Size = new Size(350, 30);
            cleanPathTextBox.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusStrip1.AutoSize = false;
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Dock = DockStyle.None;
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { watchStatusLabel, writedRecordStatusLabel, timeStatusLabel, NoticeIcon });
            statusStrip1.Location = new Point(0, 749);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1437, 30);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // watchStatusLabel
            // 
            watchStatusLabel.BackColor = SystemColors.Control;
            watchStatusLabel.Name = "watchStatusLabel";
            watchStatusLabel.Size = new Size(100, 23);
            watchStatusLabel.Text = "未开始监测";
            watchStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            watchStatusLabel.Click += watchStatusLabel_Click;
            // 
            // writedRecordStatusLabel
            // 
            writedRecordStatusLabel.BackColor = SystemColors.Control;
            writedRecordStatusLabel.Margin = new Padding(80, 4, 0, 3);
            writedRecordStatusLabel.Name = "writedRecordStatusLabel";
            writedRecordStatusLabel.Padding = new Padding(100, 0, 0, 0);
            writedRecordStatusLabel.Size = new Size(544, 23);
            writedRecordStatusLabel.Spring = true;
            writedRecordStatusLabel.Text = "已记录 0 条";
            writedRecordStatusLabel.Click += WritedRecordStatusLabel_Click;
            // 
            // timeStatusLabel
            // 
            timeStatusLabel.Margin = new Padding(50, 4, 0, 3);
            timeStatusLabel.Name = "timeStatusLabel";
            timeStatusLabel.Overflow = ToolStripItemOverflow.Never;
            timeStatusLabel.Size = new Size(574, 23);
            timeStatusLabel.Spring = true;
            timeStatusLabel.Text = "2005-02-05 03:14:15";
            timeStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            timeStatusLabel.TextImageRelation = TextImageRelation.Overlay;
            // 
            // NoticeIcon
            // 
            NoticeIcon.DisplayStyle = ToolStripItemDisplayStyle.Image;
            NoticeIcon.Image = Properties.Resources.通知;
            NoticeIcon.Margin = new Padding(50, 4, 0, 3);
            NoticeIcon.Name = "NoticeIcon";
            NoticeIcon.Size = new Size(24, 23);
            NoticeIcon.Text = " ";
            NoticeIcon.TextAlign = ContentAlignment.MiddleRight;
            NoticeIcon.TextImageRelation = TextImageRelation.Overlay;
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.Location = new Point(539, 6);
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
            BiggerButton.Location = new Point(493, 6);
            BiggerButton.Name = "BiggerButton";
            BiggerButton.Size = new Size(40, 33);
            BiggerButton.TabIndex = 2;
            BiggerButton.Text = "口";
            BiggerButton.UseVisualStyleBackColor = true;
            BiggerButton.Click += BiggerButton_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(447, 6);
            button1.Name = "button1";
            button1.Size = new Size(40, 33);
            button1.TabIndex = 3;
            button1.Text = "——";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
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
            notifyIcon1.ContextMenuStrip = notifyMenuStrip;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "C盘管理工具";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // notifyMenuStrip
            // 
            notifyMenuStrip.ImageScalingSize = new Size(24, 24);
            notifyMenuStrip.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            notifyMenuStrip.Name = "contextMenuStrip1";
            notifyMenuStrip.Size = new Size(117, 34);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(116, 30);
            exitToolStripMenuItem.Text = "退出";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.FromArgb(192, 255, 255);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.IsSplitterFixed = true;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Name = "panelTitle";
            // 
            // panelTitle.Panel1
            // 
            panelTitle.Panel1.Controls.Add(label1);
            panelTitle.Panel1.MouseDown += splitContainer1_MouseDown;
            panelTitle.Panel1.MouseMove += panelTitle_MouseMove;
            // 
            // panelTitle.Panel2
            // 
            panelTitle.Panel2.Controls.Add(button1);
            panelTitle.Panel2.Controls.Add(BiggerButton);
            panelTitle.Panel2.Controls.Add(closeButton);
            panelTitle.Size = new Size(1440, 47);
            panelTitle.SplitterDistance = 854;
            panelTitle.TabIndex = 5;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1440, 860);
            Controls.Add(panelTitle);
            Controls.Add(statusStrip1);
            Controls.Add(TabPageControl1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1180, 720);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C盘监测工具";
            Load += Form1_Load;
            TabPageControl1.ResumeLayout(false);
            totalReviewPage.ResumeLayout(false);
            totalReviewPage.PerformLayout();
            watcherPage.ResumeLayout(false);
            watcherPage.PerformLayout();
            ignoreProcessBox.ResumeLayout(false);
            ignoreProcessBox.PerformLayout();
            WatcherDirectoryBox.ResumeLayout(false);
            WatcherDirectoryBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).EndInit();
            folderAnalyzerPage.ResumeLayout(false);
            folderAnalyzerPage.PerformLayout();
            diskCleanPage.ResumeLayout(false);
            diskCleanPage.PerformLayout();
            cleanHistoryBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).EndInit();
            cleanMethodBox.ResumeLayout(false);
            cleanMethodBox.PerformLayout();
            cleanTreeBox.ResumeLayout(false);
            frequentPathBox.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            notifyMenuStrip.ResumeLayout(false);
            panelTitle.Panel1.ResumeLayout(false);
            panelTitle.Panel1.PerformLayout();
            panelTitle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelTitle).EndInit();
            panelTitle.ResumeLayout(false);
            ResumeLayout(false);
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

        // 状态栏
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel watchStatusLabel;
        private ToolStripStatusLabel writedRecordStatusLabel;
        private ToolStripStatusLabel timeStatusLabel;
        private ToolStripStatusLabel NoticeIcon;

        // 不可见的 组件
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer diskRefreshTimer;
        private NotifyIcon notifyIcon1;
        private SplitContainer panelTitle;
        private Label label1;
        private ContextMenuStrip notifyMenuStrip;
        private ToolStripMenuItem exitToolStripMenuItem;
        private GroupBox WatcherDirectoryBox;
        /// <summary>
        ///  <para>Text 存储 目录名</para>
        ///  Tag 存储 WatchingDirectory 对象
        /// </summary>
        private ListView watcherDirListView; 
        private Button exportBtn;
        private ComboBox typeFilterCombo;
        private Label typeFilterLabel;
        private Button clearBtn;
        private Button pauseBtn;
        private DataGridView changesDataGrid;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn TypeColumn;
        private DataGridViewTextBoxColumn FileNameColumn;
        private DataGridViewTextBoxColumn PathColumn;
        private DataGridViewTextBoxColumn SizeColumn;
        private DataGridViewTextBoxColumn SourceColumn;
        private TextBox dirSelectedTextBox;
        private Label label2;
        private Button dirAddButton;
        private FolderBrowserDialog ImportFolderDialog;
        private Button statisticButton;
        private GroupBox ignoreProcessBox;
        private ListView ignoreProcessView;
        private Label label3;
        private TextBox procSelectedTextBox;
        private Button processAddButton;
        private TabPage diskCleanPage;
        private Button betterDirAddButton;
        private Button betterProcessAddButton;

        // 磁盘清理 tab
        private TextBox cleanPathTextBox;
        private Button cleanSelectDirBtn;
        private Button cleanScanBtn;
        private ProgressBar cleanScanProgressBar;
        private Button cleanRefreshFrequentBtn;
        private GroupBox frequentPathBox;
        private ListView frequentPathListView;
        private Label frequentHintLabel;
        private GroupBox cleanTreeBox;
        private TreeView cleanTreeView;
        private Button cleanSelectAllBtn;
        private Button cleanSelectNoneBtn;
        private Label cleanStatusLabel;
        private GroupBox cleanMethodBox;
        private RadioButton cleanRecycleRadio;
        private RadioButton cleanPermanentRadio;
        private RadioButton cleanMoveRadio;
        private RadioButton cleanCompressRadio;
        private RadioButton cleanMklinkRadio;
        private Label cleanTargetLabel;
        private TextBox cleanTargetTextBox;
        private Button cleanTargetSelectBtn;
        private Button cleanBtn;
        private GroupBox cleanHistoryBox;
        private DataGridView cleanHistoryGrid;
        private DataGridViewTextBoxColumn CleanTimeColumn;
        private DataGridViewTextBoxColumn CleanPathColumn;
        private DataGridViewTextBoxColumn CleanSizeColumn;
        private DataGridViewTextBoxColumn CleanMethodColumn;
        private DataGridViewTextBoxColumn CleanResultColumn;
        private ProgressBar progressBar1;
    }
}
