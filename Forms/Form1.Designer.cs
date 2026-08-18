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
            pauseBtn = new Button();
            exportBtn = new Button();
            typeFilterCombo = new ComboBox();
            clearBtn = new Button();
            changesDataGrid = new DataGridView();
            TimeColumn = new DataGridViewTextBoxColumn();
            TypeColumn = new DataGridViewTextBoxColumn();
            FileNameColumn = new DataGridViewTextBoxColumn();
            PathColumn = new DataGridViewTextBoxColumn();
            SizeColumn = new DataGridViewTextBoxColumn();
            SourceColumn = new DataGridViewTextBoxColumn();
            watcherDirListView = new ListView();
            ignoreProcessView = new ListView();
            dirAddButton = new Button();
            betterDirAddButton = new Button();
            betterProcessAddButton = new Button();
            selectedPathTextBox = new TextBox();
            scanProgressBar = new ProgressBar();
            stopBtn = new Button();
            scanBtn = new Button();
            selectDirBtn = new Button();
            folderTreeView = new TreeView();
            cleanHistoryGrid = new DataGridView();
            cleanRecycleRadio = new RadioButton();
            cleanPermanentRadio = new RadioButton();
            cleanMoveRadio = new RadioButton();
            cleanCompressRadio = new RadioButton();
            cleanMklinkRadio = new RadioButton();
            cleanTargetLabel = new Label();
            cleanTargetTextBox = new TextBox();
            cleanTargetSelectBtn = new Button();
            cleanBtn = new Button();
            cleanTreeView = new TreeView();
            cleanSelectAllBtn = new Button();
            cleanSelectNoneBtn = new Button();
            cleanStatusLabel = new Label();
            frequentPathListView = new ListView();
            frequentHintLabel = new Label();
            cleanScanProgressBar = new ProgressBar();
            cleanScanBtn = new Button();
            cleanSelectDirBtn = new Button();
            cleanPathTextBox = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            diskRefreshTimer = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            notifyMenuStrip = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            ImportFolderDialog = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).BeginInit();
            notifyMenuStrip.SuspendLayout();
            SuspendLayout();
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
            // changesDataGrid
            // 
            changesDataGrid.AllowUserToAddRows = false;
            changesDataGrid.AllowUserToDeleteRows = false;
            changesDataGrid.AllowUserToResizeRows = false;
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
            TimeColumn.DataPropertyName = "Timestamp";
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
            // watcherDirListView
            // 
            watcherDirListView.Location = new Point(34, 75);
            watcherDirListView.Name = "watcherDirListView";
            watcherDirListView.Size = new Size(505, 138);
            watcherDirListView.TabIndex = 6;
            watcherDirListView.UseCompatibleStateImageBehavior = false;
            watcherDirListView.View = View.Details;
            watcherDirListView.ItemSelectionChanged += watcherDirListView_ItemSelectionChanged;
            watcherDirListView.Resize += watcherDirListView_Resize;
            // 
            // ignoreProcessView
            // 
            ignoreProcessView.AllowDrop = true;
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
            // selectedPathTextBox
            // 
            selectedPathTextBox.Location = new Point(8, 10);
            selectedPathTextBox.Name = "selectedPathTextBox";
            selectedPathTextBox.ReadOnly = true;
            selectedPathTextBox.Size = new Size(400, 30);
            selectedPathTextBox.TabIndex = 0;
            // 
            // scanProgressBar
            // 
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
            // folderTreeView
            // 
            folderTreeView.LineColor = Color.Empty;
            folderTreeView.Location = new Point(8, 80);
            folderTreeView.Name = "folderTreeView";
            folderTreeView.Size = new Size(678, 256);
            folderTreeView.TabIndex = 5;
            // 
            // cleanHistoryGrid
            // 
            cleanHistoryGrid.AllowUserToAddRows = false;
            cleanHistoryGrid.AllowUserToDeleteRows = false;
            cleanHistoryGrid.AllowUserToResizeRows = false;
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
            // cleanTreeView
            // 
            cleanTreeView.CheckBoxes = true;
            cleanTreeView.LineColor = Color.Empty;
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
            // frequentPathListView
            // 
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
            frequentHintLabel.ForeColor = Color.Gray;
            frequentHintLabel.Location = new Point(14, 292);
            frequentHintLabel.Name = "frequentHintLabel";
            frequentHintLabel.Size = new Size(332, 25);
            frequentHintLabel.TabIndex = 1;
            frequentHintLabel.Text = "单击选中基础路径，双击开始扫描";
            frequentHintLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanScanProgressBar
            // 
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(1440, 780);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1440, 780);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C盘监测工具";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).EndInit();
            notifyMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // 实时监测页（工作区复用）
        private Button pauseBtn;
        private Button exportBtn;
        private ComboBox typeFilterCombo;
        private Button clearBtn;
        private DataGridView changesDataGrid;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn TypeColumn;
        private DataGridViewTextBoxColumn FileNameColumn;
        private DataGridViewTextBoxColumn PathColumn;
        private DataGridViewTextBoxColumn SizeColumn;
        private DataGridViewTextBoxColumn SourceColumn;

        // 监控规则页（工作区复用）
        private ListView watcherDirListView;
        private ListView ignoreProcessView;
        private Button dirAddButton;
        private Button betterDirAddButton;
        private Button betterProcessAddButton;
        private FolderBrowserDialog ImportFolderDialog;

        // 空间分析页（工作区复用）
        private TextBox selectedPathTextBox;
        private Button selectDirBtn;
        private Button scanBtn;
        private Button stopBtn;
        private ProgressBar scanProgressBar;
        private TreeView folderTreeView;

        // 清理中心页（工作区复用）
        private TextBox cleanPathTextBox;
        private Button cleanSelectDirBtn;
        private Button cleanScanBtn;
        private ProgressBar cleanScanProgressBar;
        private ListView frequentPathListView;
        private Label frequentHintLabel;
        private TreeView cleanTreeView;
        private Button cleanSelectAllBtn;
        private Button cleanSelectNoneBtn;
        private Label cleanStatusLabel;
        private RadioButton cleanRecycleRadio;
        private RadioButton cleanPermanentRadio;
        private RadioButton cleanMoveRadio;
        private RadioButton cleanCompressRadio;
        private RadioButton cleanMklinkRadio;
        private Label cleanTargetLabel;
        private TextBox cleanTargetTextBox;
        private Button cleanTargetSelectBtn;
        private Button cleanBtn;
        private DataGridView cleanHistoryGrid;

        // 不可见组件
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer diskRefreshTimer;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip notifyMenuStrip;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
