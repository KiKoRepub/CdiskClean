using System.Windows.Forms.VisualStyles;

namespace CdiskClean
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            timer1 = new System.Windows.Forms.Timer(components);
            diskRefreshTimer = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            notifyMenuStrip = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            ImportFolderDialog = new FolderBrowserDialog();
            workspaceRoot = new TableLayoutPanel();
            workspaceHeader = new Panel();
            headerTitleLabel = new Label();
            minimizeButton = new Button();
            maximizeButton = new Button();
            closeButton = new Button();
            workspaceBodyLayout = new TableLayoutPanel();
            workspaceNavigation = new Panel();
            brandLabel = new Label();
            navRecordsButton = new Button();
            navCleanupButton = new Button();
            navAnalyzerButton = new Button();
            navRulesButton = new Button();
            navActivityButton = new Button();
            navDashboardButton = new Button();
            workspaceCollapseButton = new Button();
            workspaceMain = new TableLayoutPanel();
            workspacePageHeader = new Panel();
            workspacePageTitle = new Label();
            workspacePageSubtitle = new Label();
            workspaceTabControl = new TabControl();
            dashboardPage = new TabPage();
            dashboardLayout = new TableLayoutPanel();
            dashboardCapacitySurface = new Panel();
            dashboardTitleLabel = new Label();
            dashboardUsageLabel = new Label();
            dashboardDiskProgress = new ProgressBar();
            dashboardCapacityLabel = new Label();
            dashboardMetrics = new TableLayoutPanel();
            dashboardMonitorSurface = new Panel();
            dashboardMonitorTitle = new Label();
            dashboardMonitorMetric = new Label();
            dashboardRecordSurface = new Panel();
            dashboardRecordTitle = new Label();
            dashboardRecordMetric = new Label();
            dashboardRuleSurface = new Panel();
            dashboardRuleTitle = new Label();
            dashboardRuleMetric = new Label();
            dashboardRecentSurface = new Panel();
            dashboardRecentTitle = new Label();
            dashboardRecentGrid = new DataGridView();
            RecentTimestampColumn = new DataGridViewTextBoxColumn();
            RecentTypeColumn = new DataGridViewTextBoxColumn();
            RecentFileNameColumn = new DataGridViewTextBoxColumn();
            RecentSourceColumn = new DataGridViewTextBoxColumn();
            RecentDirectoryColumn = new DataGridViewTextBoxColumn();
            activityPage = new TabPage();
            activityToolbar = new FlowLayoutPanel();
            workspaceMonitorToggleButton = new Button();
            typeFilterCombo = new ComboBox();
            recordSearchBox = new TextBox();
            exportBtn = new Button();
            clearBtn = new Button();
            activityRecordCenterButton = new Button();
            activitySurface = new Panel();
            changesDataGrid = new DataGridView();
            TimeColumn = new DataGridViewTextBoxColumn();
            TypeColumn = new DataGridViewTextBoxColumn();
            FileNameColumn = new DataGridViewTextBoxColumn();
            PathColumn = new DataGridViewTextBoxColumn();
            SizeColumn = new DataGridViewTextBoxColumn();
            SourceColumn = new DataGridViewTextBoxColumn();
            rulesPage = new TabPage();
            rulesToolbar = new FlowLayoutPanel();
            rulesDirectoryTab = new Button();
            rulesProcessTab = new Button();
            rulesSurface = new Panel();
            rulesDirectoryView = new Panel();
            rulesDirToolbar = new FlowLayoutPanel();
            dirAddButton = new Button();
            betterDirAddButton = new Button();
            watcherDirListView = new ListView();
            rulesProcessView = new Panel();
            rulesProcToolbar = new FlowLayoutPanel();
            manualProcessInput = new TextBox();
            rulesProcessAddButton = new Button();
            betterProcessAddButton = new Button();
            ignoreProcessView = new ListView();
            analyzerPage = new TabPage();
            analyzerToolbar = new TableLayoutPanel();
            selectedPathTextBox = new TextBox();
            selectDirBtn = new Button();
            scanBtn = new Button();
            stopBtn = new Button();
            scanProgressBar = new ProgressBar();
            analyzerContent = new TableLayoutPanel();
            analyzerTreeSurface = new Panel();
            folderTreeView = new TreeView();
            analyzerDetailsSurface = new Panel();
            analyzerDetailsTitle = new Label();
            analyzerPathValue = new Label();
            analyzerSizeValue = new Label();
            analyzerFilesValue = new Label();
            analyzerFoldersValue = new Label();
            analyzerUseForCleanupButton = new Button();
            cleanupPage = new TabPage();
            cleanupToolbar = new TableLayoutPanel();
            cleanPathTextBox = new TextBox();
            cleanSelectDirBtn = new Button();
            cleanScanBtn = new Button();
            cleanScanProgressBar = new ProgressBar();
            cleanupContent = new TableLayoutPanel();
            cleanupTreeSurface = new Panel();
            cleanupTreeLayout = new TableLayoutPanel();
            cleanupSelectionBar = new FlowLayoutPanel();
            cleanSelectAllBtn = new Button();
            cleanSelectNoneBtn = new Button();
            cleanupSelectionLabel = new Label();
            cleanTreeView = new TreeView();
            cleanStatusLabel = new Label();
            cleanupActionSurface = new Panel();
            cleanupActionLayout = new TableLayoutPanel();
            cleanupFrequentPanel = new Panel();
            frequentRefreshButton = new Button();
            frequentPathListView = new ListView();
            frequentHintLabel = new Label();
            cleanupFrequentTitle = new Label();
            cleanupMethodPanel = new Panel();
            cleanupMethodTitle = new Label();
            cleanRecycleRadio = new RadioButton();
            cleanPermanentRadio = new RadioButton();
            cleanMoveRadio = new RadioButton();
            cleanCompressRadio = new RadioButton();
            cleanMklinkRadio = new RadioButton();
            cleanTargetLabel = new Label();
            cleanTargetTextBox = new TextBox();
            cleanTargetSelectBtn = new Button();
            cleanBtn = new Button();
            recordsPage = new TabPage();
            recordsToolbar = new FlowLayoutPanel();
            recordsNotificationTab = new Button();
            recordsStatsTab = new Button();
            recordsDetailsTab = new Button();
            recordsCleanupTab = new Button();
            recordsRefreshButton = new Button();
            recordsSurface = new Panel();
            recordViewHost = new Panel();
            cleanupRecordView = new Panel();
            cleanHistoryGrid = new DataGridView();
            cleanHistoryEmptyLabel = new Label();
            detailRecordsGrid = new DataGridView();
            DetailTimestampColumn = new DataGridViewTextBoxColumn();
            DetailSourceProcessColumn = new DataGridViewTextBoxColumn();
            DetailChangeTypeColumn = new DataGridViewTextBoxColumn();
            DetailDirectoryColumn = new DataGridViewTextBoxColumn();
            DetailFileNameColumn = new DataGridViewTextBoxColumn();
            processStatsGrid = new DataGridView();
            StatsAppNameColumn = new DataGridViewTextBoxColumn();
            StatsChangeCountColumn = new DataGridViewTextBoxColumn();
            StatsFirstChangeColumn = new DataGridViewTextBoxColumn();
            StatsLastChangeColumn = new DataGridViewTextBoxColumn();
            notificationRecordsGrid = new DataGridView();
            NotificationProcessNameColumn = new DataGridViewTextBoxColumn();
            NotificationOperationCountColumn = new DataGridViewTextBoxColumn();
            NotificationDurationColumn = new DataGridViewTextBoxColumn();
            NotificationTriggerTimeColumn = new DataGridViewTextBoxColumn();
            workspaceStatusBar = new TableLayoutPanel();
            workspaceDiskStatus = new Label();
            workspaceMonitorStatus = new Label();
            workspaceRecordStatus = new Label();
            workspaceClockStatus = new Label();
            notifyMenuStrip.SuspendLayout();
            workspaceRoot.SuspendLayout();
            workspaceHeader.SuspendLayout();
            workspaceBodyLayout.SuspendLayout();
            workspaceNavigation.SuspendLayout();
            workspaceMain.SuspendLayout();
            workspacePageHeader.SuspendLayout();
            workspaceTabControl.SuspendLayout();
            dashboardPage.SuspendLayout();
            dashboardLayout.SuspendLayout();
            dashboardCapacitySurface.SuspendLayout();
            dashboardMetrics.SuspendLayout();
            dashboardMonitorSurface.SuspendLayout();
            dashboardRecordSurface.SuspendLayout();
            dashboardRuleSurface.SuspendLayout();
            dashboardRecentSurface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dashboardRecentGrid).BeginInit();
            activityPage.SuspendLayout();
            activityToolbar.SuspendLayout();
            activitySurface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).BeginInit();
            rulesPage.SuspendLayout();
            rulesToolbar.SuspendLayout();
            rulesSurface.SuspendLayout();
            rulesDirectoryView.SuspendLayout();
            rulesDirToolbar.SuspendLayout();
            rulesProcessView.SuspendLayout();
            rulesProcToolbar.SuspendLayout();
            analyzerPage.SuspendLayout();
            analyzerToolbar.SuspendLayout();
            analyzerContent.SuspendLayout();
            analyzerTreeSurface.SuspendLayout();
            analyzerDetailsSurface.SuspendLayout();
            cleanupPage.SuspendLayout();
            cleanupToolbar.SuspendLayout();
            cleanupContent.SuspendLayout();
            cleanupTreeSurface.SuspendLayout();
            cleanupTreeLayout.SuspendLayout();
            cleanupSelectionBar.SuspendLayout();
            cleanupActionSurface.SuspendLayout();
            cleanupActionLayout.SuspendLayout();
            cleanupFrequentPanel.SuspendLayout();
            cleanupMethodPanel.SuspendLayout();
            recordsPage.SuspendLayout();
            recordsToolbar.SuspendLayout();
            recordsSurface.SuspendLayout();
            recordViewHost.SuspendLayout();
            cleanupRecordView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)detailRecordsGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)processStatsGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)notificationRecordsGrid).BeginInit();
            workspaceStatusBar.SuspendLayout();
            SuspendLayout();
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
            notifyMenuStrip.Name = "notifyMenuStrip";
            notifyMenuStrip.Size = new Size(117, 34);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(116, 30);
            exitToolStripMenuItem.Text = "退出";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // workspaceRoot
            // 
            workspaceRoot.ColumnCount = 1;
            workspaceRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            workspaceRoot.Controls.Add(workspaceHeader, 0, 0);
            workspaceRoot.Controls.Add(workspaceBodyLayout, 0, 1);
            workspaceRoot.Controls.Add(workspaceStatusBar, 0, 2);
            workspaceRoot.Dock = DockStyle.Fill;
            workspaceRoot.Location = new Point(0, 0);
            workspaceRoot.Margin = new Padding(0);
            workspaceRoot.Name = "workspaceRoot";
            workspaceRoot.RowCount = 3;
            workspaceRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            workspaceRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            workspaceRoot.Size = new Size(200, 100);
            workspaceRoot.TabIndex = 0;
            // 
            // workspaceHeader
            // 
            workspaceHeader.BackColor = Color.White;
            workspaceHeader.Controls.Add(headerTitleLabel);
            workspaceHeader.Controls.Add(minimizeButton);
            workspaceHeader.Controls.Add(maximizeButton);
            workspaceHeader.Controls.Add(closeButton);
            workspaceHeader.Dock = DockStyle.Fill;
            workspaceHeader.Location = new Point(0, 0);
            workspaceHeader.Margin = new Padding(0);
            workspaceHeader.Name = "workspaceHeader";
            workspaceHeader.Size = new Size(200, 48);
            workspaceHeader.TabIndex = 0;
            // 
            // headerTitleLabel
            // 
            headerTitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            headerTitleLabel.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            headerTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            headerTitleLabel.Location = new Point(18, 12);
            headerTitleLabel.Name = "headerTitleLabel";
            headerTitleLabel.Size = new Size(1280, 26);
            headerTitleLabel.TabIndex = 0;
            headerTitleLabel.Text = "CdiskClean  C盘监测与清理";
            headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // minimizeButton
            // 
            minimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minimizeButton.Location = new Point(1320, 8);
            minimizeButton.Name = "minimizeButton";
            minimizeButton.Size = new Size(36, 32);
            minimizeButton.TabIndex = 1;
            minimizeButton.TabStop = false;
            minimizeButton.Text = "─";
            minimizeButton.UseVisualStyleBackColor = true;
            minimizeButton.Click += minimizeButton_Click;
            // 
            // maximizeButton
            // 
            maximizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            maximizeButton.Location = new Point(1358, 8);
            maximizeButton.Name = "maximizeButton";
            maximizeButton.Size = new Size(36, 32);
            maximizeButton.TabIndex = 2;
            maximizeButton.TabStop = false;
            maximizeButton.Text = "□";
            maximizeButton.UseVisualStyleBackColor = true;
            maximizeButton.Click += maximizeButton_Click;
            // 
            // closeButton
            // 
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.Location = new Point(1396, 8);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(36, 32);
            closeButton.TabIndex = 3;
            closeButton.TabStop = false;
            closeButton.Text = "✕";
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Click += closeButton_Click;
            // 
            // workspaceBodyLayout
            // 
            workspaceBodyLayout.ColumnCount = 2;
            workspaceBodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 208F));
            workspaceBodyLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            workspaceBodyLayout.Controls.Add(workspaceNavigation, 0, 0);
            workspaceBodyLayout.Controls.Add(workspaceMain, 1, 0);
            workspaceBodyLayout.Dock = DockStyle.Fill;
            workspaceBodyLayout.Location = new Point(0, 48);
            workspaceBodyLayout.Margin = new Padding(0);
            workspaceBodyLayout.Name = "workspaceBodyLayout";
            workspaceBodyLayout.RowCount = 1;
            workspaceBodyLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceBodyLayout.Size = new Size(200, 22);
            workspaceBodyLayout.TabIndex = 1;
            // 
            // workspaceNavigation
            // 
            workspaceNavigation.BackColor = Color.White;
            workspaceNavigation.Controls.Add(brandLabel);
            workspaceNavigation.Controls.Add(navRecordsButton);
            workspaceNavigation.Controls.Add(navCleanupButton);
            workspaceNavigation.Controls.Add(navAnalyzerButton);
            workspaceNavigation.Controls.Add(navRulesButton);
            workspaceNavigation.Controls.Add(navActivityButton);
            workspaceNavigation.Controls.Add(navDashboardButton);
            workspaceNavigation.Controls.Add(workspaceCollapseButton);
            workspaceNavigation.Dock = DockStyle.Fill;
            workspaceNavigation.Location = new Point(0, 0);
            workspaceNavigation.Margin = new Padding(0);
            workspaceNavigation.Name = "workspaceNavigation";
            workspaceNavigation.Padding = new Padding(8, 12, 8, 8);
            workspaceNavigation.Size = new Size(208, 22);
            workspaceNavigation.TabIndex = 0;
            // 
            // brandLabel
            // 
            brandLabel.BackColor = Color.White;
            brandLabel.Dock = DockStyle.Top;
            brandLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            brandLabel.ForeColor = Color.FromArgb(31, 41, 55);
            brandLabel.Location = new Point(8, 264);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(192, 48);
            brandLabel.TabIndex = 0;
            brandLabel.Text = "  CDISK CLEAN";
            brandLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // navRecordsButton
            // 
            navRecordsButton.Dock = DockStyle.Top;
            navRecordsButton.FlatStyle = FlatStyle.Flat;
            navRecordsButton.ForeColor = Color.FromArgb(102, 112, 133);
            navRecordsButton.Location = new Point(8, 222);
            navRecordsButton.Margin = new Padding(4, 2, 4, 2);
            navRecordsButton.Name = "navRecordsButton";
            navRecordsButton.Size = new Size(192, 42);
            navRecordsButton.TabIndex = 6;
            navRecordsButton.Text = "记录中心";
            navRecordsButton.TextAlign = ContentAlignment.MiddleLeft;
            navRecordsButton.UseVisualStyleBackColor = true;
            navRecordsButton.Click += navRecordsButton_Click;
            // 
            // navCleanupButton
            // 
            navCleanupButton.Dock = DockStyle.Top;
            navCleanupButton.FlatStyle = FlatStyle.Flat;
            navCleanupButton.ForeColor = Color.FromArgb(102, 112, 133);
            navCleanupButton.Location = new Point(8, 180);
            navCleanupButton.Margin = new Padding(4, 2, 4, 2);
            navCleanupButton.Name = "navCleanupButton";
            navCleanupButton.Size = new Size(192, 42);
            navCleanupButton.TabIndex = 5;
            navCleanupButton.Text = "清理中心";
            navCleanupButton.TextAlign = ContentAlignment.MiddleLeft;
            navCleanupButton.UseVisualStyleBackColor = true;
            navCleanupButton.Click += navCleanupButton_Click;
            // 
            // navAnalyzerButton
            // 
            navAnalyzerButton.Dock = DockStyle.Top;
            navAnalyzerButton.FlatStyle = FlatStyle.Flat;
            navAnalyzerButton.ForeColor = Color.FromArgb(102, 112, 133);
            navAnalyzerButton.Location = new Point(8, 138);
            navAnalyzerButton.Margin = new Padding(4, 2, 4, 2);
            navAnalyzerButton.Name = "navAnalyzerButton";
            navAnalyzerButton.Size = new Size(192, 42);
            navAnalyzerButton.TabIndex = 4;
            navAnalyzerButton.Text = "空间分析";
            navAnalyzerButton.TextAlign = ContentAlignment.MiddleLeft;
            navAnalyzerButton.UseVisualStyleBackColor = true;
            navAnalyzerButton.Click += navAnalyzerButton_Click;
            // 
            // navRulesButton
            // 
            navRulesButton.Dock = DockStyle.Top;
            navRulesButton.FlatStyle = FlatStyle.Flat;
            navRulesButton.ForeColor = Color.FromArgb(102, 112, 133);
            navRulesButton.Location = new Point(8, 96);
            navRulesButton.Margin = new Padding(4, 2, 4, 2);
            navRulesButton.Name = "navRulesButton";
            navRulesButton.Size = new Size(192, 42);
            navRulesButton.TabIndex = 3;
            navRulesButton.Text = "监控规则";
            navRulesButton.TextAlign = ContentAlignment.MiddleLeft;
            navRulesButton.UseVisualStyleBackColor = true;
            navRulesButton.Click += navRulesButton_Click;
            // 
            // navActivityButton
            // 
            navActivityButton.Dock = DockStyle.Top;
            navActivityButton.FlatStyle = FlatStyle.Flat;
            navActivityButton.ForeColor = Color.FromArgb(102, 112, 133);
            navActivityButton.Location = new Point(8, 54);
            navActivityButton.Margin = new Padding(4, 2, 4, 2);
            navActivityButton.Name = "navActivityButton";
            navActivityButton.Size = new Size(192, 42);
            navActivityButton.TabIndex = 2;
            navActivityButton.Text = "实时活动";
            navActivityButton.TextAlign = ContentAlignment.MiddleLeft;
            navActivityButton.UseVisualStyleBackColor = true;
            navActivityButton.Click += navActivityButton_Click;
            // 
            // navDashboardButton
            // 
            navDashboardButton.Dock = DockStyle.Top;
            navDashboardButton.FlatStyle = FlatStyle.Flat;
            navDashboardButton.ForeColor = Color.FromArgb(102, 112, 133);
            navDashboardButton.Location = new Point(8, 12);
            navDashboardButton.Margin = new Padding(4, 2, 4, 2);
            navDashboardButton.Name = "navDashboardButton";
            navDashboardButton.Size = new Size(192, 42);
            navDashboardButton.TabIndex = 1;
            navDashboardButton.Text = "工作台";
            navDashboardButton.TextAlign = ContentAlignment.MiddleLeft;
            navDashboardButton.UseVisualStyleBackColor = true;
            navDashboardButton.Click += navDashboardButton_Click;
            // 
            // workspaceCollapseButton
            // 
            workspaceCollapseButton.Dock = DockStyle.Bottom;
            workspaceCollapseButton.FlatStyle = FlatStyle.Flat;
            workspaceCollapseButton.ForeColor = Color.FromArgb(102, 112, 133);
            workspaceCollapseButton.Location = new Point(8, -28);
            workspaceCollapseButton.Margin = new Padding(4, 2, 4, 2);
            workspaceCollapseButton.Name = "workspaceCollapseButton";
            workspaceCollapseButton.Size = new Size(192, 42);
            workspaceCollapseButton.TabIndex = 7;
            workspaceCollapseButton.Text = "折叠菜单";
            workspaceCollapseButton.TextAlign = ContentAlignment.MiddleLeft;
            workspaceCollapseButton.UseVisualStyleBackColor = true;
            workspaceCollapseButton.Click += workspaceCollapseButton_Click;
            // 
            // workspaceMain
            // 
            workspaceMain.ColumnCount = 1;
            workspaceMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            workspaceMain.Controls.Add(workspacePageHeader, 0, 0);
            workspaceMain.Controls.Add(workspaceTabControl, 0, 1);
            workspaceMain.Dock = DockStyle.Fill;
            workspaceMain.Location = new Point(208, 0);
            workspaceMain.Margin = new Padding(0);
            workspaceMain.Name = "workspaceMain";
            workspaceMain.RowCount = 2;
            workspaceMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            workspaceMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceMain.Size = new Size(1, 22);
            workspaceMain.TabIndex = 1;
            // 
            // workspacePageHeader
            // 
            workspacePageHeader.BackColor = Color.White;
            workspacePageHeader.Controls.Add(workspacePageTitle);
            workspacePageHeader.Controls.Add(workspacePageSubtitle);
            workspacePageHeader.Dock = DockStyle.Fill;
            workspacePageHeader.Location = new Point(0, 0);
            workspacePageHeader.Margin = new Padding(0);
            workspacePageHeader.Name = "workspacePageHeader";
            workspacePageHeader.Padding = new Padding(20, 8, 20, 7);
            workspacePageHeader.Size = new Size(1, 66);
            workspacePageHeader.TabIndex = 0;
            // 
            // workspacePageTitle
            // 
            workspacePageTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            workspacePageTitle.BackColor = Color.Transparent;
            workspacePageTitle.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            workspacePageTitle.ForeColor = Color.FromArgb(31, 41, 55);
            workspacePageTitle.Location = new Point(20, 8);
            workspacePageTitle.Name = "workspacePageTitle";
            workspacePageTitle.Size = new Size(961, 27);
            workspacePageTitle.TabIndex = 0;
            workspacePageTitle.Text = "工作台";
            // 
            // workspacePageSubtitle
            // 
            workspacePageSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            workspacePageSubtitle.BackColor = Color.Transparent;
            workspacePageSubtitle.Font = new Font("Microsoft YaHei UI", 9F);
            workspacePageSubtitle.ForeColor = Color.FromArgb(102, 112, 133);
            workspacePageSubtitle.Location = new Point(20, 37);
            workspacePageSubtitle.Name = "workspacePageSubtitle";
            workspacePageSubtitle.Size = new Size(961, 22);
            workspacePageSubtitle.TabIndex = 1;
            workspacePageSubtitle.Text = "查看磁盘空间、监控健康状态与最近文件活动";
            // 
            // workspaceTabControl
            // 
            workspaceTabControl.Controls.Add(dashboardPage);
            workspaceTabControl.Controls.Add(activityPage);
            workspaceTabControl.Controls.Add(rulesPage);
            workspaceTabControl.Controls.Add(analyzerPage);
            workspaceTabControl.Controls.Add(cleanupPage);
            workspaceTabControl.Controls.Add(recordsPage);
            workspaceTabControl.Dock = DockStyle.Fill;
            workspaceTabControl.Location = new Point(0, 66);
            workspaceTabControl.Margin = new Padding(0);
            workspaceTabControl.Name = "workspaceTabControl";
            workspaceTabControl.Padding = new Point(18, 18);
            workspaceTabControl.SelectedIndex = 0;
            workspaceTabControl.Size = new Size(1, 1);
            workspaceTabControl.TabIndex = 0;
            // 
            // dashboardPage
            // 
            dashboardPage.BackColor = Color.FromArgb(245, 247, 250);
            dashboardPage.Controls.Add(dashboardLayout);
            dashboardPage.Location = new Point(4, 63);
            dashboardPage.Name = "dashboardPage";
            dashboardPage.Padding = new Padding(3);
            dashboardPage.Size = new Size(0, 0);
            dashboardPage.TabIndex = 0;
            dashboardPage.Text = "工作台";
            // 
            // dashboardLayout
            // 
            dashboardLayout.ColumnCount = 1;
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            dashboardLayout.Controls.Add(dashboardCapacitySurface, 0, 0);
            dashboardLayout.Controls.Add(dashboardMetrics, 0, 1);
            dashboardLayout.Controls.Add(dashboardRecentSurface, 0, 2);
            dashboardLayout.Dock = DockStyle.Fill;
            dashboardLayout.Location = new Point(3, 3);
            dashboardLayout.Margin = new Padding(0);
            dashboardLayout.Name = "dashboardLayout";
            dashboardLayout.RowCount = 3;
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            dashboardLayout.Size = new Size(0, 0);
            dashboardLayout.TabIndex = 0;
            // 
            // dashboardCapacitySurface
            // 
            dashboardCapacitySurface.BackColor = Color.White;
            dashboardCapacitySurface.Controls.Add(dashboardTitleLabel);
            dashboardCapacitySurface.Controls.Add(dashboardUsageLabel);
            dashboardCapacitySurface.Controls.Add(dashboardDiskProgress);
            dashboardCapacitySurface.Controls.Add(dashboardCapacityLabel);
            dashboardCapacitySurface.Dock = DockStyle.Fill;
            dashboardCapacitySurface.Location = new Point(0, 0);
            dashboardCapacitySurface.Margin = new Padding(0);
            dashboardCapacitySurface.Name = "dashboardCapacitySurface";
            dashboardCapacitySurface.Size = new Size(1, 188);
            dashboardCapacitySurface.TabIndex = 0;
            // 
            // dashboardTitleLabel
            // 
            dashboardTitleLabel.AutoSize = true;
            dashboardTitleLabel.BackColor = Color.Transparent;
            dashboardTitleLabel.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            dashboardTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardTitleLabel.Location = new Point(24, 18);
            dashboardTitleLabel.Name = "dashboardTitleLabel";
            dashboardTitleLabel.Size = new Size(101, 30);
            dashboardTitleLabel.TabIndex = 0;
            dashboardTitleLabel.Text = "C 盘空间";
            // 
            // dashboardUsageLabel
            // 
            dashboardUsageLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dashboardUsageLabel.BackColor = Color.Transparent;
            dashboardUsageLabel.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            dashboardUsageLabel.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardUsageLabel.Location = new Point(24, 48);
            dashboardUsageLabel.Name = "dashboardUsageLabel";
            dashboardUsageLabel.Size = new Size(0, 42);
            dashboardUsageLabel.TabIndex = 1;
            dashboardUsageLabel.Text = "正在读取磁盘信息...";
            // 
            // dashboardDiskProgress
            // 
            dashboardDiskProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dashboardDiskProgress.Location = new Point(24, 98);
            dashboardDiskProgress.Name = "dashboardDiskProgress";
            dashboardDiskProgress.Size = new Size(0, 18);
            dashboardDiskProgress.TabIndex = 2;
            // 
            // dashboardCapacityLabel
            // 
            dashboardCapacityLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dashboardCapacityLabel.BackColor = Color.Transparent;
            dashboardCapacityLabel.Font = new Font("Microsoft YaHei UI", 9.5F);
            dashboardCapacityLabel.ForeColor = Color.FromArgb(102, 112, 133);
            dashboardCapacityLabel.Location = new Point(24, 130);
            dashboardCapacityLabel.Name = "dashboardCapacityLabel";
            dashboardCapacityLabel.Size = new Size(0, 28);
            dashboardCapacityLabel.TabIndex = 3;
            // 
            // dashboardMetrics
            // 
            dashboardMetrics.BackColor = Color.FromArgb(245, 247, 250);
            dashboardMetrics.ColumnCount = 3;
            dashboardMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            dashboardMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            dashboardMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            dashboardMetrics.Controls.Add(dashboardMonitorSurface, 0, 0);
            dashboardMetrics.Controls.Add(dashboardRecordSurface, 1, 0);
            dashboardMetrics.Controls.Add(dashboardRuleSurface, 2, 0);
            dashboardMetrics.Dock = DockStyle.Fill;
            dashboardMetrics.Location = new Point(0, 198);
            dashboardMetrics.Margin = new Padding(0, 10, 0, 0);
            dashboardMetrics.Name = "dashboardMetrics";
            dashboardMetrics.RowCount = 1;
            dashboardMetrics.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            dashboardMetrics.Size = new Size(1, 80);
            dashboardMetrics.TabIndex = 1;
            // 
            // dashboardMonitorSurface
            // 
            dashboardMonitorSurface.BackColor = Color.White;
            dashboardMonitorSurface.Controls.Add(dashboardMonitorTitle);
            dashboardMonitorSurface.Controls.Add(dashboardMonitorMetric);
            dashboardMonitorSurface.Dock = DockStyle.Fill;
            dashboardMonitorSurface.Location = new Point(0, 0);
            dashboardMonitorSurface.Margin = new Padding(0, 0, 6, 0);
            dashboardMonitorSurface.Name = "dashboardMonitorSurface";
            dashboardMonitorSurface.Size = new Size(1, 80);
            dashboardMonitorSurface.TabIndex = 0;
            // 
            // dashboardMonitorTitle
            // 
            dashboardMonitorTitle.BackColor = Color.Transparent;
            dashboardMonitorTitle.Dock = DockStyle.Top;
            dashboardMonitorTitle.Font = new Font("Microsoft YaHei UI", 9F);
            dashboardMonitorTitle.ForeColor = Color.FromArgb(102, 112, 133);
            dashboardMonitorTitle.Location = new Point(0, 0);
            dashboardMonitorTitle.Name = "dashboardMonitorTitle";
            dashboardMonitorTitle.Size = new Size(1, 25);
            dashboardMonitorTitle.TabIndex = 0;
            dashboardMonitorTitle.Text = "监控状态";
            dashboardMonitorTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardMonitorMetric
            // 
            dashboardMonitorMetric.AutoEllipsis = true;
            dashboardMonitorMetric.BackColor = Color.Transparent;
            dashboardMonitorMetric.Dock = DockStyle.Fill;
            dashboardMonitorMetric.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            dashboardMonitorMetric.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardMonitorMetric.Location = new Point(0, 0);
            dashboardMonitorMetric.Name = "dashboardMonitorMetric";
            dashboardMonitorMetric.Size = new Size(1, 80);
            dashboardMonitorMetric.TabIndex = 1;
            dashboardMonitorMetric.Text = "-";
            dashboardMonitorMetric.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecordSurface
            // 
            dashboardRecordSurface.BackColor = Color.White;
            dashboardRecordSurface.Controls.Add(dashboardRecordTitle);
            dashboardRecordSurface.Controls.Add(dashboardRecordMetric);
            dashboardRecordSurface.Dock = DockStyle.Fill;
            dashboardRecordSurface.Location = new Point(6, 0);
            dashboardRecordSurface.Margin = new Padding(6, 0, 6, 0);
            dashboardRecordSurface.Name = "dashboardRecordSurface";
            dashboardRecordSurface.Size = new Size(1, 80);
            dashboardRecordSurface.TabIndex = 1;
            // 
            // dashboardRecordTitle
            // 
            dashboardRecordTitle.BackColor = Color.Transparent;
            dashboardRecordTitle.Dock = DockStyle.Top;
            dashboardRecordTitle.Font = new Font("Microsoft YaHei UI", 9F);
            dashboardRecordTitle.ForeColor = Color.FromArgb(102, 112, 133);
            dashboardRecordTitle.Location = new Point(0, 0);
            dashboardRecordTitle.Name = "dashboardRecordTitle";
            dashboardRecordTitle.Size = new Size(1, 25);
            dashboardRecordTitle.TabIndex = 0;
            dashboardRecordTitle.Text = "当前记录";
            dashboardRecordTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecordMetric
            // 
            dashboardRecordMetric.AutoEllipsis = true;
            dashboardRecordMetric.BackColor = Color.Transparent;
            dashboardRecordMetric.Dock = DockStyle.Fill;
            dashboardRecordMetric.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            dashboardRecordMetric.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardRecordMetric.Location = new Point(0, 0);
            dashboardRecordMetric.Name = "dashboardRecordMetric";
            dashboardRecordMetric.Size = new Size(1, 80);
            dashboardRecordMetric.TabIndex = 1;
            dashboardRecordMetric.Text = "-";
            dashboardRecordMetric.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRuleSurface
            // 
            dashboardRuleSurface.BackColor = Color.White;
            dashboardRuleSurface.Controls.Add(dashboardRuleTitle);
            dashboardRuleSurface.Controls.Add(dashboardRuleMetric);
            dashboardRuleSurface.Dock = DockStyle.Fill;
            dashboardRuleSurface.Location = new Point(6, 0);
            dashboardRuleSurface.Margin = new Padding(6, 0, 0, 0);
            dashboardRuleSurface.Name = "dashboardRuleSurface";
            dashboardRuleSurface.Size = new Size(1, 80);
            dashboardRuleSurface.TabIndex = 2;
            // 
            // dashboardRuleTitle
            // 
            dashboardRuleTitle.BackColor = Color.Transparent;
            dashboardRuleTitle.Dock = DockStyle.Top;
            dashboardRuleTitle.Font = new Font("Microsoft YaHei UI", 9F);
            dashboardRuleTitle.ForeColor = Color.FromArgb(102, 112, 133);
            dashboardRuleTitle.Location = new Point(0, 0);
            dashboardRuleTitle.Name = "dashboardRuleTitle";
            dashboardRuleTitle.Size = new Size(1, 25);
            dashboardRuleTitle.TabIndex = 0;
            dashboardRuleTitle.Text = "生效规则";
            dashboardRuleTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRuleMetric
            // 
            dashboardRuleMetric.AutoEllipsis = true;
            dashboardRuleMetric.BackColor = Color.Transparent;
            dashboardRuleMetric.Dock = DockStyle.Fill;
            dashboardRuleMetric.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            dashboardRuleMetric.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardRuleMetric.Location = new Point(0, 0);
            dashboardRuleMetric.Name = "dashboardRuleMetric";
            dashboardRuleMetric.Size = new Size(1, 80);
            dashboardRuleMetric.TabIndex = 1;
            dashboardRuleMetric.Text = "-";
            dashboardRuleMetric.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecentSurface
            // 
            dashboardRecentSurface.BackColor = Color.White;
            dashboardRecentSurface.Controls.Add(dashboardRecentTitle);
            dashboardRecentSurface.Controls.Add(dashboardRecentGrid);
            dashboardRecentSurface.Dock = DockStyle.Fill;
            dashboardRecentSurface.Location = new Point(0, 278);
            dashboardRecentSurface.Margin = new Padding(0);
            dashboardRecentSurface.Name = "dashboardRecentSurface";
            dashboardRecentSurface.Size = new Size(1, 1);
            dashboardRecentSurface.TabIndex = 2;
            // 
            // dashboardRecentTitle
            // 
            dashboardRecentTitle.BackColor = Color.Transparent;
            dashboardRecentTitle.Dock = DockStyle.Top;
            dashboardRecentTitle.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            dashboardRecentTitle.ForeColor = Color.FromArgb(31, 41, 55);
            dashboardRecentTitle.Location = new Point(0, 0);
            dashboardRecentTitle.Name = "dashboardRecentTitle";
            dashboardRecentTitle.Size = new Size(1, 34);
            dashboardRecentTitle.TabIndex = 0;
            dashboardRecentTitle.Text = "最近活动";
            dashboardRecentTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecentGrid
            // 
            dashboardRecentGrid.AllowUserToAddRows = false;
            dashboardRecentGrid.AllowUserToDeleteRows = false;
            dashboardRecentGrid.AllowUserToResizeRows = false;
            dashboardRecentGrid.AutoGenerateColumns = false;
            dashboardRecentGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dashboardRecentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dashboardRecentGrid.Columns.AddRange(new DataGridViewColumn[] { RecentTimestampColumn, RecentTypeColumn, RecentFileNameColumn, RecentSourceColumn, RecentDirectoryColumn });
            dashboardRecentGrid.Dock = DockStyle.Fill;
            dashboardRecentGrid.Location = new Point(0, 0);
            dashboardRecentGrid.Margin = new Padding(16, 0, 16, 0);
            dashboardRecentGrid.Name = "dashboardRecentGrid";
            dashboardRecentGrid.ReadOnly = true;
            dashboardRecentGrid.RowHeadersVisible = false;
            dashboardRecentGrid.RowHeadersWidth = 62;
            dashboardRecentGrid.RowTemplate.Height = 25;
            dashboardRecentGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dashboardRecentGrid.Size = new Size(1, 1);
            dashboardRecentGrid.TabIndex = 1;
            // 
            // RecentTimestampColumn
            // 
            RecentTimestampColumn.DataPropertyName = "Timestamp";
            RecentTimestampColumn.FillWeight = 20F;
            RecentTimestampColumn.HeaderText = "时间";
            RecentTimestampColumn.MinimumWidth = 8;
            RecentTimestampColumn.Name = "RecentTimestampColumn";
            RecentTimestampColumn.ReadOnly = true;
            // 
            // RecentTypeColumn
            // 
            RecentTypeColumn.DataPropertyName = "TypeText";
            RecentTypeColumn.FillWeight = 12F;
            RecentTypeColumn.HeaderText = "类型";
            RecentTypeColumn.MinimumWidth = 8;
            RecentTypeColumn.Name = "RecentTypeColumn";
            RecentTypeColumn.ReadOnly = true;
            // 
            // RecentFileNameColumn
            // 
            RecentFileNameColumn.DataPropertyName = "FileName";
            RecentFileNameColumn.FillWeight = 28F;
            RecentFileNameColumn.HeaderText = "文件名";
            RecentFileNameColumn.MinimumWidth = 8;
            RecentFileNameColumn.Name = "RecentFileNameColumn";
            RecentFileNameColumn.ReadOnly = true;
            // 
            // RecentSourceColumn
            // 
            RecentSourceColumn.DataPropertyName = "SourceProcess";
            RecentSourceColumn.FillWeight = 18F;
            RecentSourceColumn.HeaderText = "来源进程";
            RecentSourceColumn.MinimumWidth = 8;
            RecentSourceColumn.Name = "RecentSourceColumn";
            RecentSourceColumn.ReadOnly = true;
            // 
            // RecentDirectoryColumn
            // 
            RecentDirectoryColumn.DataPropertyName = "Directory";
            RecentDirectoryColumn.FillWeight = 35F;
            RecentDirectoryColumn.HeaderText = "目录";
            RecentDirectoryColumn.MinimumWidth = 8;
            RecentDirectoryColumn.Name = "RecentDirectoryColumn";
            RecentDirectoryColumn.ReadOnly = true;
            // 
            // activityPage
            // 
            activityPage.BackColor = Color.FromArgb(245, 247, 250);
            activityPage.Controls.Add(activityToolbar);
            activityPage.Controls.Add(activitySurface);
            activityPage.Location = new Point(4, 63);
            activityPage.Name = "activityPage";
            activityPage.Padding = new Padding(3);
            activityPage.Size = new Size(0, 0);
            activityPage.TabIndex = 1;
            activityPage.Text = "实时活动";
            // 
            // activityToolbar
            // 
            activityToolbar.AutoScroll = true;
            activityToolbar.Controls.Add(workspaceMonitorToggleButton);
            activityToolbar.Controls.Add(typeFilterCombo);
            activityToolbar.Controls.Add(recordSearchBox);
            activityToolbar.Controls.Add(exportBtn);
            activityToolbar.Controls.Add(clearBtn);
            activityToolbar.Controls.Add(activityRecordCenterButton);
            activityToolbar.Dock = DockStyle.Top;
            activityToolbar.Location = new Point(3, 3);
            activityToolbar.Margin = new Padding(0);
            activityToolbar.Name = "activityToolbar";
            activityToolbar.Padding = new Padding(0, 4, 0, 8);
            activityToolbar.Size = new Size(0, 54);
            activityToolbar.TabIndex = 0;
            activityToolbar.WrapContents = false;
            // 
            // workspaceMonitorToggleButton
            // 
            workspaceMonitorToggleButton.FlatStyle = FlatStyle.Flat;
            workspaceMonitorToggleButton.Location = new Point(0, 8);
            workspaceMonitorToggleButton.Margin = new Padding(0, 4, 8, 0);
            workspaceMonitorToggleButton.Name = "workspaceMonitorToggleButton";
            workspaceMonitorToggleButton.Size = new Size(100, 36);
            workspaceMonitorToggleButton.TabIndex = 0;
            workspaceMonitorToggleButton.Text = "开始监测";
            workspaceMonitorToggleButton.UseVisualStyleBackColor = true;
            workspaceMonitorToggleButton.Click += pauseBtn_Click;
            // 
            // typeFilterCombo
            // 
            typeFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            typeFilterCombo.FormattingEnabled = true;
            typeFilterCombo.Items.AddRange(new object[] { "全部", "创建", "修改", "删除", "重命名" });
            typeFilterCombo.Location = new Point(116, 10);
            typeFilterCombo.Margin = new Padding(8, 6, 0, 0);
            typeFilterCombo.Name = "typeFilterCombo";
            typeFilterCombo.Size = new Size(126, 32);
            typeFilterCombo.TabIndex = 1;
            typeFilterCombo.SelectedIndexChanged += typeFilterCombo_SelectedIndexChanged;
            // 
            // recordSearchBox
            // 
            recordSearchBox.BorderStyle = BorderStyle.FixedSingle;
            recordSearchBox.Location = new Point(252, 8);
            recordSearchBox.Margin = new Padding(10, 4, 0, 0);
            recordSearchBox.Name = "recordSearchBox";
            recordSearchBox.Size = new Size(260, 30);
            recordSearchBox.TabIndex = 2;
            recordSearchBox.TextChanged += recordSearchBox_TextChanged;
            // 
            // exportBtn
            // 
            exportBtn.FlatStyle = FlatStyle.Flat;
            exportBtn.Location = new Point(524, 8);
            exportBtn.Margin = new Padding(12, 4, 8, 0);
            exportBtn.Name = "exportBtn";
            exportBtn.Size = new Size(84, 36);
            exportBtn.TabIndex = 3;
            exportBtn.Text = "导出";
            exportBtn.UseVisualStyleBackColor = true;
            exportBtn.Click += exportBtn_Click;
            // 
            // clearBtn
            // 
            clearBtn.FlatStyle = FlatStyle.Flat;
            clearBtn.Location = new Point(616, 8);
            clearBtn.Margin = new Padding(0, 4, 8, 0);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(84, 36);
            clearBtn.TabIndex = 4;
            clearBtn.Text = "清空";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += clearBtn_Click;
            // 
            // activityRecordCenterButton
            // 
            activityRecordCenterButton.FlatStyle = FlatStyle.Flat;
            activityRecordCenterButton.Location = new Point(708, 8);
            activityRecordCenterButton.Margin = new Padding(0, 4, 8, 0);
            activityRecordCenterButton.Name = "activityRecordCenterButton";
            activityRecordCenterButton.Size = new Size(100, 36);
            activityRecordCenterButton.TabIndex = 5;
            activityRecordCenterButton.Text = "记录中心";
            activityRecordCenterButton.UseVisualStyleBackColor = true;
            activityRecordCenterButton.Click += activityRecordCenterButton_Click;
            // 
            // activitySurface
            // 
            activitySurface.BackColor = Color.White;
            activitySurface.Controls.Add(changesDataGrid);
            activitySurface.Dock = DockStyle.Fill;
            activitySurface.Location = new Point(3, 3);
            activitySurface.Margin = new Padding(0);
            activitySurface.Name = "activitySurface";
            activitySurface.Padding = new Padding(12);
            activitySurface.Size = new Size(0, 0);
            activitySurface.TabIndex = 1;
            // 
            // changesDataGrid
            // 
            changesDataGrid.AllowUserToAddRows = false;
            changesDataGrid.AllowUserToDeleteRows = false;
            changesDataGrid.AllowUserToResizeRows = false;
            changesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            changesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            changesDataGrid.Columns.AddRange(new DataGridViewColumn[] { TimeColumn, TypeColumn, FileNameColumn, PathColumn, SizeColumn, SourceColumn });
            changesDataGrid.Dock = DockStyle.Fill;
            changesDataGrid.Location = new Point(12, 12);
            changesDataGrid.Margin = new Padding(0);
            changesDataGrid.Name = "changesDataGrid";
            changesDataGrid.ReadOnly = true;
            changesDataGrid.RowHeadersVisible = false;
            changesDataGrid.RowHeadersWidth = 62;
            changesDataGrid.RowTemplate.Height = 25;
            changesDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            changesDataGrid.Size = new Size(0, 0);
            changesDataGrid.TabIndex = 0;
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
            // rulesPage
            // 
            rulesPage.BackColor = Color.FromArgb(245, 247, 250);
            rulesPage.Controls.Add(rulesToolbar);
            rulesPage.Controls.Add(rulesSurface);
            rulesPage.Location = new Point(4, 63);
            rulesPage.Name = "rulesPage";
            rulesPage.Padding = new Padding(3);
            rulesPage.Size = new Size(0, 0);
            rulesPage.TabIndex = 2;
            rulesPage.Text = "监控规则";
            // 
            // rulesToolbar
            // 
            rulesToolbar.AutoScroll = true;
            rulesToolbar.Controls.Add(rulesDirectoryTab);
            rulesToolbar.Controls.Add(rulesProcessTab);
            rulesToolbar.Dock = DockStyle.Top;
            rulesToolbar.Location = new Point(3, 3);
            rulesToolbar.Margin = new Padding(0);
            rulesToolbar.Name = "rulesToolbar";
            rulesToolbar.Padding = new Padding(0, 4, 0, 8);
            rulesToolbar.Size = new Size(0, 54);
            rulesToolbar.TabIndex = 0;
            rulesToolbar.WrapContents = false;
            // 
            // rulesDirectoryTab
            // 
            rulesDirectoryTab.FlatStyle = FlatStyle.Flat;
            rulesDirectoryTab.Location = new Point(0, 8);
            rulesDirectoryTab.Margin = new Padding(0, 4, 8, 0);
            rulesDirectoryTab.Name = "rulesDirectoryTab";
            rulesDirectoryTab.Size = new Size(100, 36);
            rulesDirectoryTab.TabIndex = 0;
            rulesDirectoryTab.Text = "监控目录";
            rulesDirectoryTab.UseVisualStyleBackColor = true;
            rulesDirectoryTab.Click += rulesDirectoryTab_Click;
            // 
            // rulesProcessTab
            // 
            rulesProcessTab.FlatStyle = FlatStyle.Flat;
            rulesProcessTab.Location = new Point(108, 8);
            rulesProcessTab.Margin = new Padding(0, 4, 8, 0);
            rulesProcessTab.Name = "rulesProcessTab";
            rulesProcessTab.Size = new Size(100, 36);
            rulesProcessTab.TabIndex = 1;
            rulesProcessTab.Text = "忽略进程";
            rulesProcessTab.UseVisualStyleBackColor = true;
            rulesProcessTab.Click += rulesProcessTab_Click;
            // 
            // rulesSurface
            // 
            rulesSurface.BackColor = Color.White;
            rulesSurface.Controls.Add(rulesDirectoryView);
            rulesSurface.Controls.Add(rulesProcessView);
            rulesSurface.Dock = DockStyle.Fill;
            rulesSurface.Location = new Point(3, 3);
            rulesSurface.Margin = new Padding(0);
            rulesSurface.Name = "rulesSurface";
            rulesSurface.Padding = new Padding(14);
            rulesSurface.Size = new Size(0, 0);
            rulesSurface.TabIndex = 1;
            // 
            // rulesDirectoryView
            // 
            rulesDirectoryView.BackColor = Color.White;
            rulesDirectoryView.Controls.Add(rulesDirToolbar);
            rulesDirectoryView.Controls.Add(watcherDirListView);
            rulesDirectoryView.Dock = DockStyle.Fill;
            rulesDirectoryView.Location = new Point(14, 14);
            rulesDirectoryView.Margin = new Padding(0);
            rulesDirectoryView.Name = "rulesDirectoryView";
            rulesDirectoryView.Size = new Size(0, 0);
            rulesDirectoryView.TabIndex = 0;
            // 
            // rulesDirToolbar
            // 
            rulesDirToolbar.Controls.Add(dirAddButton);
            rulesDirToolbar.Controls.Add(betterDirAddButton);
            rulesDirToolbar.Dock = DockStyle.Top;
            rulesDirToolbar.Location = new Point(0, 0);
            rulesDirToolbar.Margin = new Padding(0);
            rulesDirToolbar.Name = "rulesDirToolbar";
            rulesDirToolbar.Padding = new Padding(0, 2, 0, 7);
            rulesDirToolbar.Size = new Size(0, 48);
            rulesDirToolbar.TabIndex = 0;
            rulesDirToolbar.WrapContents = false;
            // 
            // dirAddButton
            // 
            dirAddButton.FlatStyle = FlatStyle.Flat;
            dirAddButton.Location = new Point(0, 6);
            dirAddButton.Margin = new Padding(0, 4, 8, 0);
            dirAddButton.Name = "dirAddButton";
            dirAddButton.Size = new Size(110, 36);
            dirAddButton.TabIndex = 0;
            dirAddButton.Text = "添加目录";
            dirAddButton.UseVisualStyleBackColor = true;
            dirAddButton.Click += dirAddButton_Click;
            // 
            // betterDirAddButton
            // 
            betterDirAddButton.FlatStyle = FlatStyle.Flat;
            betterDirAddButton.Location = new Point(118, 6);
            betterDirAddButton.Margin = new Padding(0, 4, 8, 0);
            betterDirAddButton.Name = "betterDirAddButton";
            betterDirAddButton.Size = new Size(110, 36);
            betterDirAddButton.TabIndex = 1;
            betterDirAddButton.Text = "批量选择";
            betterDirAddButton.UseVisualStyleBackColor = true;
            betterDirAddButton.Click += betterDirAddButton_Click;
            // 
            // watcherDirListView
            // 
            watcherDirListView.Dock = DockStyle.Fill;
            watcherDirListView.Location = new Point(0, 0);
            watcherDirListView.Margin = new Padding(0);
            watcherDirListView.Name = "watcherDirListView";
            watcherDirListView.Size = new Size(0, 0);
            watcherDirListView.TabIndex = 1;
            watcherDirListView.UseCompatibleStateImageBehavior = false;
            watcherDirListView.ItemSelectionChanged += watcherDirListView_ItemSelectionChanged;
            watcherDirListView.Resize += watcherDirListView_Resize;
            // 
            // rulesProcessView
            // 
            rulesProcessView.BackColor = Color.White;
            rulesProcessView.Controls.Add(rulesProcToolbar);
            rulesProcessView.Controls.Add(ignoreProcessView);
            rulesProcessView.Dock = DockStyle.Fill;
            rulesProcessView.Location = new Point(14, 14);
            rulesProcessView.Margin = new Padding(0);
            rulesProcessView.Name = "rulesProcessView";
            rulesProcessView.Size = new Size(0, 0);
            rulesProcessView.TabIndex = 1;
            rulesProcessView.Visible = false;
            // 
            // rulesProcToolbar
            // 
            rulesProcToolbar.Controls.Add(manualProcessInput);
            rulesProcToolbar.Controls.Add(rulesProcessAddButton);
            rulesProcToolbar.Controls.Add(betterProcessAddButton);
            rulesProcToolbar.Dock = DockStyle.Top;
            rulesProcToolbar.Location = new Point(0, 0);
            rulesProcToolbar.Margin = new Padding(0);
            rulesProcToolbar.Name = "rulesProcToolbar";
            rulesProcToolbar.Padding = new Padding(0, 2, 0, 7);
            rulesProcToolbar.Size = new Size(0, 48);
            rulesProcToolbar.TabIndex = 0;
            rulesProcToolbar.WrapContents = false;
            // 
            // manualProcessInput
            // 
            manualProcessInput.BorderStyle = BorderStyle.FixedSingle;
            manualProcessInput.Location = new Point(0, 6);
            manualProcessInput.Margin = new Padding(0, 4, 8, 0);
            manualProcessInput.Name = "manualProcessInput";
            manualProcessInput.Size = new Size(240, 30);
            manualProcessInput.TabIndex = 0;
            // 
            // rulesProcessAddButton
            // 
            rulesProcessAddButton.FlatStyle = FlatStyle.Flat;
            rulesProcessAddButton.Location = new Point(248, 6);
            rulesProcessAddButton.Margin = new Padding(0, 4, 8, 0);
            rulesProcessAddButton.Name = "rulesProcessAddButton";
            rulesProcessAddButton.Size = new Size(84, 36);
            rulesProcessAddButton.TabIndex = 1;
            rulesProcessAddButton.Text = "添加";
            rulesProcessAddButton.UseVisualStyleBackColor = true;
            rulesProcessAddButton.Click += rulesProcessAddButton_Click;
            // 
            // betterProcessAddButton
            // 
            betterProcessAddButton.FlatStyle = FlatStyle.Flat;
            betterProcessAddButton.Location = new Point(340, 6);
            betterProcessAddButton.Margin = new Padding(0, 4, 8, 0);
            betterProcessAddButton.Name = "betterProcessAddButton";
            betterProcessAddButton.Size = new Size(130, 36);
            betterProcessAddButton.TabIndex = 2;
            betterProcessAddButton.Text = "选择运行进程";
            betterProcessAddButton.UseVisualStyleBackColor = true;
            betterProcessAddButton.Click += betterProcessAddButton_Click;
            // 
            // ignoreProcessView
            // 
            ignoreProcessView.AllowDrop = true;
            ignoreProcessView.Dock = DockStyle.Fill;
            ignoreProcessView.Location = new Point(0, 0);
            ignoreProcessView.Margin = new Padding(0);
            ignoreProcessView.Name = "ignoreProcessView";
            ignoreProcessView.Size = new Size(0, 0);
            ignoreProcessView.TabIndex = 1;
            ignoreProcessView.UseCompatibleStateImageBehavior = false;
            ignoreProcessView.ItemSelectionChanged += ignoreProcessView_ItemSelectionChanged;
            ignoreProcessView.DragDrop += ignoreProcessView_DragDrop;
            ignoreProcessView.DragEnter += ignoreProcessView_DragEnter;
            ignoreProcessView.Resize += ignoreProcessView_Resize;
            // 
            // analyzerPage
            // 
            analyzerPage.BackColor = Color.FromArgb(245, 247, 250);
            analyzerPage.Controls.Add(analyzerToolbar);
            analyzerPage.Controls.Add(scanProgressBar);
            analyzerPage.Controls.Add(analyzerContent);
            analyzerPage.Location = new Point(4, 63);
            analyzerPage.Name = "analyzerPage";
            analyzerPage.Padding = new Padding(3);
            analyzerPage.Size = new Size(0, 0);
            analyzerPage.TabIndex = 3;
            analyzerPage.Text = "空间分析";
            // 
            // analyzerToolbar
            // 
            analyzerToolbar.BackColor = Color.FromArgb(245, 247, 250);
            analyzerToolbar.ColumnCount = 4;
            analyzerToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            analyzerToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            analyzerToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            analyzerToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
            analyzerToolbar.Controls.Add(selectedPathTextBox, 0, 0);
            analyzerToolbar.Controls.Add(selectDirBtn, 1, 0);
            analyzerToolbar.Controls.Add(scanBtn, 2, 0);
            analyzerToolbar.Controls.Add(stopBtn, 3, 0);
            analyzerToolbar.Dock = DockStyle.Top;
            analyzerToolbar.Location = new Point(3, 26);
            analyzerToolbar.Margin = new Padding(0);
            analyzerToolbar.Name = "analyzerToolbar";
            analyzerToolbar.RowCount = 1;
            analyzerToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            analyzerToolbar.Size = new Size(0, 52);
            analyzerToolbar.TabIndex = 0;
            // 
            // selectedPathTextBox
            // 
            selectedPathTextBox.Dock = DockStyle.Fill;
            selectedPathTextBox.Location = new Point(0, 7);
            selectedPathTextBox.Margin = new Padding(0, 7, 8, 7);
            selectedPathTextBox.Name = "selectedPathTextBox";
            selectedPathTextBox.ReadOnly = true;
            selectedPathTextBox.Size = new Size(1, 30);
            selectedPathTextBox.TabIndex = 0;
            // 
            // selectDirBtn
            // 
            selectDirBtn.Dock = DockStyle.Fill;
            selectDirBtn.FlatStyle = FlatStyle.Flat;
            selectDirBtn.Location = new Point(-303, 6);
            selectDirBtn.Margin = new Padding(4, 6, 4, 6);
            selectDirBtn.MinimumSize = new Size(64, 34);
            selectDirBtn.Name = "selectDirBtn";
            selectDirBtn.Size = new Size(100, 40);
            selectDirBtn.TabIndex = 1;
            selectDirBtn.Text = "浏览目录";
            selectDirBtn.UseVisualStyleBackColor = true;
            selectDirBtn.Click += selectDirBtn_Click;
            // 
            // scanBtn
            // 
            scanBtn.Dock = DockStyle.Fill;
            scanBtn.FlatStyle = FlatStyle.Flat;
            scanBtn.Location = new Point(-195, 6);
            scanBtn.Margin = new Padding(4, 6, 4, 6);
            scanBtn.MinimumSize = new Size(64, 34);
            scanBtn.Name = "scanBtn";
            scanBtn.Size = new Size(100, 40);
            scanBtn.TabIndex = 2;
            scanBtn.Text = "开始扫描";
            scanBtn.UseVisualStyleBackColor = true;
            scanBtn.Click += scanBtn_Click;
            // 
            // stopBtn
            // 
            stopBtn.Dock = DockStyle.Fill;
            stopBtn.Enabled = false;
            stopBtn.FlatStyle = FlatStyle.Flat;
            stopBtn.Location = new Point(-87, 6);
            stopBtn.Margin = new Padding(4, 6, 4, 6);
            stopBtn.MinimumSize = new Size(64, 34);
            stopBtn.Name = "stopBtn";
            stopBtn.Size = new Size(84, 40);
            stopBtn.TabIndex = 3;
            stopBtn.Text = "停止";
            stopBtn.UseVisualStyleBackColor = true;
            stopBtn.Click += stopBtn_Click;
            // 
            // scanProgressBar
            // 
            scanProgressBar.Dock = DockStyle.Top;
            scanProgressBar.Location = new Point(3, 3);
            scanProgressBar.Margin = new Padding(0, 5, 0, 5);
            scanProgressBar.Name = "scanProgressBar";
            scanProgressBar.Size = new Size(0, 23);
            scanProgressBar.TabIndex = 1;
            // 
            // analyzerContent
            // 
            analyzerContent.BackColor = Color.FromArgb(245, 247, 250);
            analyzerContent.ColumnCount = 2;
            analyzerContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            analyzerContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            analyzerContent.Controls.Add(analyzerTreeSurface, 0, 0);
            analyzerContent.Controls.Add(analyzerDetailsSurface, 1, 0);
            analyzerContent.Dock = DockStyle.Fill;
            analyzerContent.Location = new Point(3, 3);
            analyzerContent.Margin = new Padding(0);
            analyzerContent.Name = "analyzerContent";
            analyzerContent.RowCount = 1;
            analyzerContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            analyzerContent.Size = new Size(0, 0);
            analyzerContent.TabIndex = 2;
            // 
            // analyzerTreeSurface
            // 
            analyzerTreeSurface.BackColor = Color.White;
            analyzerTreeSurface.Controls.Add(folderTreeView);
            analyzerTreeSurface.Dock = DockStyle.Fill;
            analyzerTreeSurface.Location = new Point(0, 10);
            analyzerTreeSurface.Margin = new Padding(0, 10, 8, 0);
            analyzerTreeSurface.Name = "analyzerTreeSurface";
            analyzerTreeSurface.Padding = new Padding(12);
            analyzerTreeSurface.Size = new Size(1, 1);
            analyzerTreeSurface.TabIndex = 0;
            // 
            // folderTreeView
            // 
            folderTreeView.Dock = DockStyle.Fill;
            folderTreeView.Location = new Point(12, 12);
            folderTreeView.Margin = new Padding(0);
            folderTreeView.Name = "folderTreeView";
            folderTreeView.Size = new Size(0, 0);
            folderTreeView.TabIndex = 0;
            folderTreeView.AfterSelect += folderTreeView_AfterSelect;
            // 
            // analyzerDetailsSurface
            // 
            analyzerDetailsSurface.BackColor = Color.White;
            analyzerDetailsSurface.Controls.Add(analyzerDetailsTitle);
            analyzerDetailsSurface.Controls.Add(analyzerPathValue);
            analyzerDetailsSurface.Controls.Add(analyzerSizeValue);
            analyzerDetailsSurface.Controls.Add(analyzerFilesValue);
            analyzerDetailsSurface.Controls.Add(analyzerFoldersValue);
            analyzerDetailsSurface.Controls.Add(analyzerUseForCleanupButton);
            analyzerDetailsSurface.Dock = DockStyle.Fill;
            analyzerDetailsSurface.Location = new Point(8, 10);
            analyzerDetailsSurface.Margin = new Padding(8, 10, 0, 0);
            analyzerDetailsSurface.Name = "analyzerDetailsSurface";
            analyzerDetailsSurface.Padding = new Padding(18);
            analyzerDetailsSurface.Size = new Size(1, 1);
            analyzerDetailsSurface.TabIndex = 1;
            // 
            // analyzerDetailsTitle
            // 
            analyzerDetailsTitle.BackColor = Color.Transparent;
            analyzerDetailsTitle.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            analyzerDetailsTitle.ForeColor = Color.FromArgb(31, 41, 55);
            analyzerDetailsTitle.Location = new Point(0, 0);
            analyzerDetailsTitle.Name = "analyzerDetailsTitle";
            analyzerDetailsTitle.Size = new Size(260, 28);
            analyzerDetailsTitle.TabIndex = 0;
            analyzerDetailsTitle.Text = "选中目录";
            // 
            // analyzerPathValue
            // 
            analyzerPathValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            analyzerPathValue.AutoEllipsis = true;
            analyzerPathValue.BackColor = Color.Transparent;
            analyzerPathValue.Font = new Font("Microsoft YaHei UI", 9.5F);
            analyzerPathValue.ForeColor = Color.FromArgb(102, 112, 133);
            analyzerPathValue.Location = new Point(0, 40);
            analyzerPathValue.Name = "analyzerPathValue";
            analyzerPathValue.Size = new Size(0, 64);
            analyzerPathValue.TabIndex = 1;
            analyzerPathValue.Text = "请先扫描并选择目录";
            // 
            // analyzerSizeValue
            // 
            analyzerSizeValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            analyzerSizeValue.AutoEllipsis = true;
            analyzerSizeValue.BackColor = Color.Transparent;
            analyzerSizeValue.Font = new Font("Microsoft YaHei UI", 9.5F);
            analyzerSizeValue.ForeColor = Color.FromArgb(102, 112, 133);
            analyzerSizeValue.Location = new Point(0, 118);
            analyzerSizeValue.Name = "analyzerSizeValue";
            analyzerSizeValue.Size = new Size(0, 28);
            analyzerSizeValue.TabIndex = 2;
            analyzerSizeValue.Text = "大小：-";
            // 
            // analyzerFilesValue
            // 
            analyzerFilesValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            analyzerFilesValue.AutoEllipsis = true;
            analyzerFilesValue.BackColor = Color.Transparent;
            analyzerFilesValue.Font = new Font("Microsoft YaHei UI", 9.5F);
            analyzerFilesValue.ForeColor = Color.FromArgb(102, 112, 133);
            analyzerFilesValue.Location = new Point(0, 156);
            analyzerFilesValue.Name = "analyzerFilesValue";
            analyzerFilesValue.Size = new Size(0, 28);
            analyzerFilesValue.TabIndex = 3;
            analyzerFilesValue.Text = "文件：-";
            // 
            // analyzerFoldersValue
            // 
            analyzerFoldersValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            analyzerFoldersValue.AutoEllipsis = true;
            analyzerFoldersValue.BackColor = Color.Transparent;
            analyzerFoldersValue.Font = new Font("Microsoft YaHei UI", 9.5F);
            analyzerFoldersValue.ForeColor = Color.FromArgb(102, 112, 133);
            analyzerFoldersValue.Location = new Point(0, 194);
            analyzerFoldersValue.Name = "analyzerFoldersValue";
            analyzerFoldersValue.Size = new Size(0, 28);
            analyzerFoldersValue.TabIndex = 4;
            analyzerFoldersValue.Text = "子目录：-";
            // 
            // analyzerUseForCleanupButton
            // 
            analyzerUseForCleanupButton.FlatStyle = FlatStyle.Flat;
            analyzerUseForCleanupButton.Location = new Point(0, 244);
            analyzerUseForCleanupButton.Name = "analyzerUseForCleanupButton";
            analyzerUseForCleanupButton.Size = new Size(160, 38);
            analyzerUseForCleanupButton.TabIndex = 5;
            analyzerUseForCleanupButton.Text = "作为清理来源";
            analyzerUseForCleanupButton.UseVisualStyleBackColor = true;
            analyzerUseForCleanupButton.Click += analyzerUseForCleanupButton_Click;
            // 
            // cleanupPage
            // 
            cleanupPage.BackColor = Color.FromArgb(245, 247, 250);
            cleanupPage.Controls.Add(cleanupToolbar);
            cleanupPage.Controls.Add(cleanScanProgressBar);
            cleanupPage.Controls.Add(cleanupContent);
            cleanupPage.Location = new Point(4, 63);
            cleanupPage.Name = "cleanupPage";
            cleanupPage.Padding = new Padding(3);
            cleanupPage.Size = new Size(0, 0);
            cleanupPage.TabIndex = 4;
            cleanupPage.Text = "清理中心";
            // 
            // cleanupToolbar
            // 
            cleanupToolbar.BackColor = Color.FromArgb(245, 247, 250);
            cleanupToolbar.ColumnCount = 3;
            cleanupToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cleanupToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            cleanupToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            cleanupToolbar.Controls.Add(cleanPathTextBox, 0, 0);
            cleanupToolbar.Controls.Add(cleanSelectDirBtn, 1, 0);
            cleanupToolbar.Controls.Add(cleanScanBtn, 2, 0);
            cleanupToolbar.Dock = DockStyle.Top;
            cleanupToolbar.Location = new Point(3, 26);
            cleanupToolbar.Margin = new Padding(0);
            cleanupToolbar.Name = "cleanupToolbar";
            cleanupToolbar.RowCount = 1;
            cleanupToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupToolbar.Size = new Size(0, 52);
            cleanupToolbar.TabIndex = 0;
            // 
            // cleanPathTextBox
            // 
            cleanPathTextBox.Dock = DockStyle.Fill;
            cleanPathTextBox.Location = new Point(0, 7);
            cleanPathTextBox.Margin = new Padding(0, 7, 8, 7);
            cleanPathTextBox.Name = "cleanPathTextBox";
            cleanPathTextBox.Size = new Size(1, 30);
            cleanPathTextBox.TabIndex = 0;
            // 
            // cleanSelectDirBtn
            // 
            cleanSelectDirBtn.Dock = DockStyle.Fill;
            cleanSelectDirBtn.FlatStyle = FlatStyle.Flat;
            cleanSelectDirBtn.Location = new Point(-211, 6);
            cleanSelectDirBtn.Margin = new Padding(4, 6, 4, 6);
            cleanSelectDirBtn.MinimumSize = new Size(64, 34);
            cleanSelectDirBtn.Name = "cleanSelectDirBtn";
            cleanSelectDirBtn.Size = new Size(100, 40);
            cleanSelectDirBtn.TabIndex = 1;
            cleanSelectDirBtn.Text = "浏览目录";
            cleanSelectDirBtn.UseVisualStyleBackColor = true;
            cleanSelectDirBtn.Click += cleanSelectDirBtn_Click;
            // 
            // cleanScanBtn
            // 
            cleanScanBtn.Dock = DockStyle.Fill;
            cleanScanBtn.FlatStyle = FlatStyle.Flat;
            cleanScanBtn.Location = new Point(-103, 6);
            cleanScanBtn.Margin = new Padding(4, 6, 4, 6);
            cleanScanBtn.MinimumSize = new Size(64, 34);
            cleanScanBtn.Name = "cleanScanBtn";
            cleanScanBtn.Size = new Size(100, 40);
            cleanScanBtn.TabIndex = 2;
            cleanScanBtn.Text = "开始扫描";
            cleanScanBtn.UseVisualStyleBackColor = true;
            cleanScanBtn.Click += cleanScanBtn_Click;
            // 
            // cleanScanProgressBar
            // 
            cleanScanProgressBar.Dock = DockStyle.Top;
            cleanScanProgressBar.Location = new Point(3, 3);
            cleanScanProgressBar.Margin = new Padding(0, 5, 0, 5);
            cleanScanProgressBar.Name = "cleanScanProgressBar";
            cleanScanProgressBar.Size = new Size(0, 23);
            cleanScanProgressBar.TabIndex = 1;
            // 
            // cleanupContent
            // 
            cleanupContent.BackColor = Color.FromArgb(245, 247, 250);
            cleanupContent.ColumnCount = 2;
            cleanupContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cleanupContent.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 344F));
            cleanupContent.Controls.Add(cleanupTreeSurface, 0, 0);
            cleanupContent.Controls.Add(cleanupActionSurface, 1, 0);
            cleanupContent.Dock = DockStyle.Fill;
            cleanupContent.Location = new Point(3, 3);
            cleanupContent.Margin = new Padding(0);
            cleanupContent.Name = "cleanupContent";
            cleanupContent.RowCount = 1;
            cleanupContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupContent.Size = new Size(0, 0);
            cleanupContent.TabIndex = 2;
            // 
            // cleanupTreeSurface
            // 
            cleanupTreeSurface.BackColor = Color.White;
            cleanupTreeSurface.Controls.Add(cleanupTreeLayout);
            cleanupTreeSurface.Dock = DockStyle.Fill;
            cleanupTreeSurface.Location = new Point(0, 10);
            cleanupTreeSurface.Margin = new Padding(0, 10, 8, 0);
            cleanupTreeSurface.Name = "cleanupTreeSurface";
            cleanupTreeSurface.Padding = new Padding(12);
            cleanupTreeSurface.Size = new Size(1, 1);
            cleanupTreeSurface.TabIndex = 0;
            // 
            // cleanupTreeLayout
            // 
            cleanupTreeLayout.BackColor = Color.White;
            cleanupTreeLayout.ColumnCount = 1;
            cleanupTreeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cleanupTreeLayout.Controls.Add(cleanupSelectionBar, 0, 0);
            cleanupTreeLayout.Controls.Add(cleanTreeView, 0, 1);
            cleanupTreeLayout.Controls.Add(cleanStatusLabel, 0, 2);
            cleanupTreeLayout.Dock = DockStyle.Fill;
            cleanupTreeLayout.Location = new Point(12, 12);
            cleanupTreeLayout.Margin = new Padding(0);
            cleanupTreeLayout.Name = "cleanupTreeLayout";
            cleanupTreeLayout.RowCount = 3;
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            cleanupTreeLayout.Size = new Size(0, 0);
            cleanupTreeLayout.TabIndex = 0;
            // 
            // cleanupSelectionBar
            // 
            cleanupSelectionBar.Controls.Add(cleanSelectAllBtn);
            cleanupSelectionBar.Controls.Add(cleanSelectNoneBtn);
            cleanupSelectionBar.Controls.Add(cleanupSelectionLabel);
            cleanupSelectionBar.Dock = DockStyle.Fill;
            cleanupSelectionBar.Location = new Point(0, 0);
            cleanupSelectionBar.Margin = new Padding(0);
            cleanupSelectionBar.Name = "cleanupSelectionBar";
            cleanupSelectionBar.Size = new Size(1, 42);
            cleanupSelectionBar.TabIndex = 0;
            cleanupSelectionBar.WrapContents = false;
            // 
            // cleanSelectAllBtn
            // 
            cleanSelectAllBtn.Dock = DockStyle.Top;
            cleanSelectAllBtn.FlatStyle = FlatStyle.Flat;
            cleanSelectAllBtn.Location = new Point(0, 4);
            cleanSelectAllBtn.Margin = new Padding(0, 4, 4, 0);
            cleanSelectAllBtn.Name = "cleanSelectAllBtn";
            cleanSelectAllBtn.Size = new Size(72, 34);
            cleanSelectAllBtn.TabIndex = 0;
            cleanSelectAllBtn.Text = "全选";
            cleanSelectAllBtn.UseVisualStyleBackColor = true;
            cleanSelectAllBtn.Click += cleanSelectAllBtn_Click;
            // 
            // cleanSelectNoneBtn
            // 
            cleanSelectNoneBtn.Dock = DockStyle.Top;
            cleanSelectNoneBtn.FlatStyle = FlatStyle.Flat;
            cleanSelectNoneBtn.Location = new Point(76, 4);
            cleanSelectNoneBtn.Margin = new Padding(0, 4, 4, 0);
            cleanSelectNoneBtn.Name = "cleanSelectNoneBtn";
            cleanSelectNoneBtn.Size = new Size(84, 34);
            cleanSelectNoneBtn.TabIndex = 1;
            cleanSelectNoneBtn.Text = "全不选";
            cleanSelectNoneBtn.UseVisualStyleBackColor = true;
            cleanSelectNoneBtn.Click += cleanSelectNoneBtn_Click;
            // 
            // cleanupSelectionLabel
            // 
            cleanupSelectionLabel.AutoEllipsis = true;
            cleanupSelectionLabel.BackColor = Color.Transparent;
            cleanupSelectionLabel.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            cleanupSelectionLabel.ForeColor = Color.FromArgb(31, 41, 55);
            cleanupSelectionLabel.Location = new Point(176, 4);
            cleanupSelectionLabel.Margin = new Padding(12, 4, 0, 0);
            cleanupSelectionLabel.Name = "cleanupSelectionLabel";
            cleanupSelectionLabel.Size = new Size(310, 34);
            cleanupSelectionLabel.TabIndex = 2;
            cleanupSelectionLabel.Text = "已选择 0 项 / 0 B";
            cleanupSelectionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanTreeView
            // 
            cleanTreeView.CheckBoxes = true;
            cleanTreeView.Dock = DockStyle.Fill;
            cleanTreeView.Location = new Point(0, 42);
            cleanTreeView.Margin = new Padding(0);
            cleanTreeView.Name = "cleanTreeView";
            cleanTreeView.Size = new Size(1, 1);
            cleanTreeView.TabIndex = 1;
            cleanTreeView.BeforeCheck += cleanTreeView_BeforeCheck;
            cleanTreeView.AfterCheck += cleanTreeView_AfterCheck;
            // 
            // cleanStatusLabel
            // 
            cleanStatusLabel.AutoEllipsis = true;
            cleanStatusLabel.Dock = DockStyle.Fill;
            cleanStatusLabel.Location = new Point(0, -33);
            cleanStatusLabel.Margin = new Padding(0);
            cleanStatusLabel.Name = "cleanStatusLabel";
            cleanStatusLabel.Size = new Size(1, 34);
            cleanStatusLabel.TabIndex = 2;
            cleanStatusLabel.Text = "请选择目录并开始扫描";
            cleanStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanupActionSurface
            // 
            cleanupActionSurface.BackColor = Color.White;
            cleanupActionSurface.Controls.Add(cleanupActionLayout);
            cleanupActionSurface.Dock = DockStyle.Fill;
            cleanupActionSurface.Location = new Point(-335, 10);
            cleanupActionSurface.Margin = new Padding(8, 10, 0, 0);
            cleanupActionSurface.Name = "cleanupActionSurface";
            cleanupActionSurface.Padding = new Padding(12);
            cleanupActionSurface.Size = new Size(336, 1);
            cleanupActionSurface.TabIndex = 1;
            // 
            // cleanupActionLayout
            // 
            cleanupActionLayout.BackColor = Color.White;
            cleanupActionLayout.ColumnCount = 1;
            cleanupActionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cleanupActionLayout.Controls.Add(cleanupFrequentPanel, 0, 0);
            cleanupActionLayout.Controls.Add(cleanupMethodPanel, 0, 1);
            cleanupActionLayout.Dock = DockStyle.Fill;
            cleanupActionLayout.Location = new Point(12, 12);
            cleanupActionLayout.Margin = new Padding(0);
            cleanupActionLayout.Name = "cleanupActionLayout";
            cleanupActionLayout.RowCount = 2;
            cleanupActionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 216F));
            cleanupActionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupActionLayout.Size = new Size(312, 0);
            cleanupActionLayout.TabIndex = 0;
            // 
            // cleanupFrequentPanel
            // 
            cleanupFrequentPanel.BackColor = Color.White;
            cleanupFrequentPanel.Controls.Add(frequentRefreshButton);
            cleanupFrequentPanel.Controls.Add(frequentPathListView);
            cleanupFrequentPanel.Controls.Add(frequentHintLabel);
            cleanupFrequentPanel.Controls.Add(cleanupFrequentTitle);
            cleanupFrequentPanel.Dock = DockStyle.Fill;
            cleanupFrequentPanel.Location = new Point(0, 0);
            cleanupFrequentPanel.Margin = new Padding(0);
            cleanupFrequentPanel.Name = "cleanupFrequentPanel";
            cleanupFrequentPanel.Size = new Size(312, 216);
            cleanupFrequentPanel.TabIndex = 0;
            // 
            // frequentRefreshButton
            // 
            frequentRefreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            frequentRefreshButton.FlatStyle = FlatStyle.Flat;
            frequentRefreshButton.Location = new Point(278, 0);
            frequentRefreshButton.Name = "frequentRefreshButton";
            frequentRefreshButton.Size = new Size(34, 32);
            frequentRefreshButton.TabIndex = 0;
            frequentRefreshButton.Text = "⟳";
            frequentRefreshButton.UseVisualStyleBackColor = true;
            frequentRefreshButton.Click += cleanRefreshFrequentBtn_Click;
            // 
            // frequentPathListView
            // 
            frequentPathListView.Dock = DockStyle.Fill;
            frequentPathListView.Location = new Point(0, 34);
            frequentPathListView.Margin = new Padding(0);
            frequentPathListView.Name = "frequentPathListView";
            frequentPathListView.Size = new Size(312, 152);
            frequentPathListView.TabIndex = 1;
            frequentPathListView.UseCompatibleStateImageBehavior = false;
            frequentPathListView.ItemSelectionChanged += frequentPathListView_ItemSelectionChanged;
            frequentPathListView.MouseDoubleClick += frequentPathListView_MouseDoubleClick;
            // 
            // frequentHintLabel
            // 
            frequentHintLabel.AutoEllipsis = true;
            frequentHintLabel.Dock = DockStyle.Bottom;
            frequentHintLabel.ForeColor = Color.Gray;
            frequentHintLabel.Location = new Point(0, 186);
            frequentHintLabel.Margin = new Padding(0);
            frequentHintLabel.Name = "frequentHintLabel";
            frequentHintLabel.Size = new Size(312, 30);
            frequentHintLabel.TabIndex = 2;
            frequentHintLabel.Text = "单击选中基础路径，双击开始扫描";
            frequentHintLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanupFrequentTitle
            // 
            cleanupFrequentTitle.BackColor = Color.White;
            cleanupFrequentTitle.Dock = DockStyle.Top;
            cleanupFrequentTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            cleanupFrequentTitle.ForeColor = Color.FromArgb(31, 41, 55);
            cleanupFrequentTitle.Location = new Point(0, 0);
            cleanupFrequentTitle.Name = "cleanupFrequentTitle";
            cleanupFrequentTitle.Size = new Size(312, 34);
            cleanupFrequentTitle.TabIndex = 3;
            cleanupFrequentTitle.Text = "高频修改路径";
            cleanupFrequentTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanupMethodPanel
            // 
            cleanupMethodPanel.BackColor = Color.White;
            cleanupMethodPanel.Controls.Add(cleanupMethodTitle);
            cleanupMethodPanel.Controls.Add(cleanRecycleRadio);
            cleanupMethodPanel.Controls.Add(cleanPermanentRadio);
            cleanupMethodPanel.Controls.Add(cleanMoveRadio);
            cleanupMethodPanel.Controls.Add(cleanCompressRadio);
            cleanupMethodPanel.Controls.Add(cleanMklinkRadio);
            cleanupMethodPanel.Controls.Add(cleanTargetLabel);
            cleanupMethodPanel.Controls.Add(cleanTargetTextBox);
            cleanupMethodPanel.Controls.Add(cleanTargetSelectBtn);
            cleanupMethodPanel.Controls.Add(cleanBtn);
            cleanupMethodPanel.Dock = DockStyle.Fill;
            cleanupMethodPanel.Location = new Point(0, 216);
            cleanupMethodPanel.Margin = new Padding(0);
            cleanupMethodPanel.Name = "cleanupMethodPanel";
            cleanupMethodPanel.Size = new Size(312, 1);
            cleanupMethodPanel.TabIndex = 1;
            cleanupMethodPanel.Resize += cleanupMethodPanel_Resize;
            // 
            // cleanupMethodTitle
            // 
            cleanupMethodTitle.BackColor = Color.White;
            cleanupMethodTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            cleanupMethodTitle.ForeColor = Color.FromArgb(31, 41, 55);
            cleanupMethodTitle.Location = new Point(4, 12);
            cleanupMethodTitle.Name = "cleanupMethodTitle";
            cleanupMethodTitle.Size = new Size(260, 28);
            cleanupMethodTitle.TabIndex = 0;
            cleanupMethodTitle.Text = "清理方式";
            // 
            // cleanRecycleRadio
            // 
            cleanRecycleRadio.AutoSize = true;
            cleanRecycleRadio.Checked = true;
            cleanRecycleRadio.Location = new Point(4, 46);
            cleanRecycleRadio.Name = "cleanRecycleRadio";
            cleanRecycleRadio.Size = new Size(215, 28);
            cleanRecycleRadio.TabIndex = 1;
            cleanRecycleRadio.TabStop = true;
            cleanRecycleRadio.Text = "回收站删除（可恢复）";
            cleanRecycleRadio.UseVisualStyleBackColor = true;
            cleanRecycleRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanPermanentRadio
            // 
            cleanPermanentRadio.AutoSize = true;
            cleanPermanentRadio.Location = new Point(4, 75);
            cleanPermanentRadio.Name = "cleanPermanentRadio";
            cleanPermanentRadio.Size = new Size(107, 28);
            cleanPermanentRadio.TabIndex = 2;
            cleanPermanentRadio.TabStop = true;
            cleanPermanentRadio.Text = "永久删除";
            cleanPermanentRadio.UseVisualStyleBackColor = true;
            cleanPermanentRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanMoveRadio
            // 
            cleanMoveRadio.AutoSize = true;
            cleanMoveRadio.Location = new Point(4, 104);
            cleanMoveRadio.Name = "cleanMoveRadio";
            cleanMoveRadio.Size = new Size(89, 28);
            cleanMoveRadio.TabIndex = 3;
            cleanMoveRadio.TabStop = true;
            cleanMoveRadio.Text = "移动到";
            cleanMoveRadio.UseVisualStyleBackColor = true;
            cleanMoveRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanCompressRadio
            // 
            cleanCompressRadio.AutoSize = true;
            cleanCompressRadio.Location = new Point(4, 133);
            cleanCompressRadio.Name = "cleanCompressRadio";
            cleanCompressRadio.Size = new Size(89, 28);
            cleanCompressRadio.TabIndex = 4;
            cleanCompressRadio.TabStop = true;
            cleanCompressRadio.Text = "压缩到";
            cleanCompressRadio.UseVisualStyleBackColor = true;
            cleanCompressRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanMklinkRadio
            // 
            cleanMklinkRadio.AutoSize = true;
            cleanMklinkRadio.Location = new Point(4, 162);
            cleanMklinkRadio.Name = "cleanMklinkRadio";
            cleanMklinkRadio.Size = new Size(174, 28);
            cleanMklinkRadio.TabIndex = 5;
            cleanMklinkRadio.TabStop = true;
            cleanMklinkRadio.Text = "mkLink 软链接到";
            cleanMklinkRadio.UseVisualStyleBackColor = true;
            cleanMklinkRadio.CheckedChanged += cleanMethodRadio_CheckedChanged;
            // 
            // cleanTargetLabel
            // 
            cleanTargetLabel.AutoSize = true;
            cleanTargetLabel.Location = new Point(4, 196);
            cleanTargetLabel.Name = "cleanTargetLabel";
            cleanTargetLabel.Size = new Size(86, 24);
            cleanTargetLabel.TabIndex = 6;
            cleanTargetLabel.Text = "目标目录:";
            // 
            // cleanTargetTextBox
            // 
            cleanTargetTextBox.Location = new Point(4, 224);
            cleanTargetTextBox.Name = "cleanTargetTextBox";
            cleanTargetTextBox.Size = new Size(228, 30);
            cleanTargetTextBox.TabIndex = 7;
            // 
            // cleanTargetSelectBtn
            // 
            cleanTargetSelectBtn.FlatStyle = FlatStyle.Flat;
            cleanTargetSelectBtn.Location = new Point(244, 223);
            cleanTargetSelectBtn.Name = "cleanTargetSelectBtn";
            cleanTargetSelectBtn.Size = new Size(76, 32);
            cleanTargetSelectBtn.TabIndex = 8;
            cleanTargetSelectBtn.Text = "浏览";
            cleanTargetSelectBtn.UseVisualStyleBackColor = true;
            cleanTargetSelectBtn.Click += cleanTargetSelectBtn_Click;
            // 
            // cleanBtn
            // 
            cleanBtn.FlatStyle = FlatStyle.Flat;
            cleanBtn.Location = new Point(4, 348);
            cleanBtn.Name = "cleanBtn";
            cleanBtn.Size = new Size(312, 42);
            cleanBtn.TabIndex = 9;
            cleanBtn.Text = "清理选中文件";
            cleanBtn.UseVisualStyleBackColor = true;
            cleanBtn.Click += cleanBtn_Click;
            // 
            // recordsPage
            // 
            recordsPage.BackColor = Color.FromArgb(245, 247, 250);
            recordsPage.Controls.Add(recordsToolbar);
            recordsPage.Controls.Add(recordsSurface);
            recordsPage.Location = new Point(4, 63);
            recordsPage.Name = "recordsPage";
            recordsPage.Padding = new Padding(3);
            recordsPage.Size = new Size(0, 0);
            recordsPage.TabIndex = 5;
            recordsPage.Text = "记录中心";
            // 
            // recordsToolbar
            // 
            recordsToolbar.AutoScroll = true;
            recordsToolbar.Controls.Add(recordsNotificationTab);
            recordsToolbar.Controls.Add(recordsStatsTab);
            recordsToolbar.Controls.Add(recordsDetailsTab);
            recordsToolbar.Controls.Add(recordsCleanupTab);
            recordsToolbar.Controls.Add(recordsRefreshButton);
            recordsToolbar.Dock = DockStyle.Top;
            recordsToolbar.Location = new Point(3, 3);
            recordsToolbar.Margin = new Padding(0);
            recordsToolbar.Name = "recordsToolbar";
            recordsToolbar.Padding = new Padding(0, 4, 0, 8);
            recordsToolbar.Size = new Size(0, 54);
            recordsToolbar.TabIndex = 0;
            recordsToolbar.WrapContents = false;
            // 
            // recordsNotificationTab
            // 
            recordsNotificationTab.FlatStyle = FlatStyle.Flat;
            recordsNotificationTab.Location = new Point(0, 8);
            recordsNotificationTab.Margin = new Padding(0, 4, 8, 0);
            recordsNotificationTab.Name = "recordsNotificationTab";
            recordsNotificationTab.Size = new Size(100, 36);
            recordsNotificationTab.TabIndex = 0;
            recordsNotificationTab.Text = "提醒记录";
            recordsNotificationTab.UseVisualStyleBackColor = true;
            recordsNotificationTab.Click += recordsNotificationTab_Click;
            // 
            // recordsStatsTab
            // 
            recordsStatsTab.FlatStyle = FlatStyle.Flat;
            recordsStatsTab.Location = new Point(108, 8);
            recordsStatsTab.Margin = new Padding(0, 4, 8, 0);
            recordsStatsTab.Name = "recordsStatsTab";
            recordsStatsTab.Size = new Size(100, 36);
            recordsStatsTab.TabIndex = 1;
            recordsStatsTab.Text = "进程统计";
            recordsStatsTab.UseVisualStyleBackColor = true;
            recordsStatsTab.Click += recordsStatsTab_Click;
            // 
            // recordsDetailsTab
            // 
            recordsDetailsTab.FlatStyle = FlatStyle.Flat;
            recordsDetailsTab.Location = new Point(216, 8);
            recordsDetailsTab.Margin = new Padding(0, 4, 8, 0);
            recordsDetailsTab.Name = "recordsDetailsTab";
            recordsDetailsTab.Size = new Size(100, 36);
            recordsDetailsTab.TabIndex = 2;
            recordsDetailsTab.Text = "变更明细";
            recordsDetailsTab.UseVisualStyleBackColor = true;
            recordsDetailsTab.Click += recordsDetailsTab_Click;
            // 
            // recordsCleanupTab
            // 
            recordsCleanupTab.FlatStyle = FlatStyle.Flat;
            recordsCleanupTab.Location = new Point(324, 8);
            recordsCleanupTab.Margin = new Padding(0, 4, 8, 0);
            recordsCleanupTab.Name = "recordsCleanupTab";
            recordsCleanupTab.Size = new Size(100, 36);
            recordsCleanupTab.TabIndex = 3;
            recordsCleanupTab.Text = "清理历史";
            recordsCleanupTab.UseVisualStyleBackColor = true;
            recordsCleanupTab.Click += recordsCleanupTab_Click;
            // 
            // recordsRefreshButton
            // 
            recordsRefreshButton.FlatStyle = FlatStyle.Flat;
            recordsRefreshButton.Location = new Point(432, 8);
            recordsRefreshButton.Margin = new Padding(0, 4, 8, 0);
            recordsRefreshButton.Name = "recordsRefreshButton";
            recordsRefreshButton.Size = new Size(84, 36);
            recordsRefreshButton.TabIndex = 4;
            recordsRefreshButton.Text = "刷新";
            recordsRefreshButton.UseVisualStyleBackColor = true;
            recordsRefreshButton.Click += recordsRefreshButton_Click;
            // 
            // recordsSurface
            // 
            recordsSurface.BackColor = Color.White;
            recordsSurface.Controls.Add(recordViewHost);
            recordsSurface.Dock = DockStyle.Fill;
            recordsSurface.Location = new Point(3, 3);
            recordsSurface.Margin = new Padding(0);
            recordsSurface.Name = "recordsSurface";
            recordsSurface.Padding = new Padding(12);
            recordsSurface.Size = new Size(0, 0);
            recordsSurface.TabIndex = 1;
            // 
            // recordViewHost
            // 
            recordViewHost.BackColor = Color.White;
            recordViewHost.Controls.Add(cleanupRecordView);
            recordViewHost.Controls.Add(detailRecordsGrid);
            recordViewHost.Controls.Add(processStatsGrid);
            recordViewHost.Controls.Add(notificationRecordsGrid);
            recordViewHost.Dock = DockStyle.Fill;
            recordViewHost.Location = new Point(12, 12);
            recordViewHost.Margin = new Padding(0);
            recordViewHost.Name = "recordViewHost";
            recordViewHost.Size = new Size(0, 0);
            recordViewHost.TabIndex = 0;
            // 
            // cleanupRecordView
            // 
            cleanupRecordView.BackColor = Color.White;
            cleanupRecordView.Controls.Add(cleanHistoryGrid);
            cleanupRecordView.Controls.Add(cleanHistoryEmptyLabel);
            cleanupRecordView.Dock = DockStyle.Fill;
            cleanupRecordView.Location = new Point(0, 0);
            cleanupRecordView.Margin = new Padding(0);
            cleanupRecordView.Name = "cleanupRecordView";
            cleanupRecordView.Size = new Size(0, 0);
            cleanupRecordView.TabIndex = 3;
            cleanupRecordView.Visible = false;
            // 
            // cleanHistoryGrid
            // 
            cleanHistoryGrid.AllowUserToAddRows = false;
            cleanHistoryGrid.AllowUserToDeleteRows = false;
            cleanHistoryGrid.AllowUserToResizeRows = false;
            cleanHistoryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cleanHistoryGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cleanHistoryGrid.Dock = DockStyle.Fill;
            cleanHistoryGrid.Location = new Point(0, 0);
            cleanHistoryGrid.Margin = new Padding(0);
            cleanHistoryGrid.Name = "cleanHistoryGrid";
            cleanHistoryGrid.ReadOnly = true;
            cleanHistoryGrid.RowHeadersVisible = false;
            cleanHistoryGrid.RowHeadersWidth = 62;
            cleanHistoryGrid.RowTemplate.Height = 25;
            cleanHistoryGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cleanHistoryGrid.Size = new Size(0, 0);
            cleanHistoryGrid.TabIndex = 0;
            cleanHistoryGrid.CellContextMenuStripNeeded += cleanHistoryGrid_CellContextMenuStripNeeded;
            // 
            // cleanHistoryEmptyLabel
            // 
            cleanHistoryEmptyLabel.BackColor = Color.White;
            cleanHistoryEmptyLabel.Dock = DockStyle.Fill;
            cleanHistoryEmptyLabel.ForeColor = Color.Gray;
            cleanHistoryEmptyLabel.Location = new Point(0, 0);
            cleanHistoryEmptyLabel.Name = "cleanHistoryEmptyLabel";
            cleanHistoryEmptyLabel.Size = new Size(0, 0);
            cleanHistoryEmptyLabel.TabIndex = 1;
            cleanHistoryEmptyLabel.Text = "暂无清理记录";
            cleanHistoryEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            cleanHistoryEmptyLabel.Visible = false;
            // 
            // detailRecordsGrid
            // 
            detailRecordsGrid.AllowUserToAddRows = false;
            detailRecordsGrid.AllowUserToDeleteRows = false;
            detailRecordsGrid.AllowUserToResizeRows = false;
            detailRecordsGrid.AutoGenerateColumns = false;
            detailRecordsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            detailRecordsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            detailRecordsGrid.Columns.AddRange(new DataGridViewColumn[] { DetailTimestampColumn, DetailSourceProcessColumn, DetailChangeTypeColumn, DetailDirectoryColumn, DetailFileNameColumn });
            detailRecordsGrid.Dock = DockStyle.Fill;
            detailRecordsGrid.Location = new Point(0, 0);
            detailRecordsGrid.Margin = new Padding(0);
            detailRecordsGrid.Name = "detailRecordsGrid";
            detailRecordsGrid.ReadOnly = true;
            detailRecordsGrid.RowHeadersVisible = false;
            detailRecordsGrid.RowHeadersWidth = 62;
            detailRecordsGrid.RowTemplate.Height = 25;
            detailRecordsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            detailRecordsGrid.Size = new Size(0, 0);
            detailRecordsGrid.TabIndex = 2;
            detailRecordsGrid.Visible = false;
            detailRecordsGrid.CellFormatting += detailRecordsGrid_CellFormatting;
            // 
            // DetailTimestampColumn
            // 
            DetailTimestampColumn.DataPropertyName = "Timestamp";
            DetailTimestampColumn.FillWeight = 20F;
            DetailTimestampColumn.HeaderText = "时间";
            DetailTimestampColumn.MinimumWidth = 8;
            DetailTimestampColumn.Name = "DetailTimestampColumn";
            DetailTimestampColumn.ReadOnly = true;
            // 
            // DetailSourceProcessColumn
            // 
            DetailSourceProcessColumn.DataPropertyName = "SourceProcess";
            DetailSourceProcessColumn.FillWeight = 18F;
            DetailSourceProcessColumn.HeaderText = "来源进程";
            DetailSourceProcessColumn.MinimumWidth = 8;
            DetailSourceProcessColumn.Name = "DetailSourceProcessColumn";
            DetailSourceProcessColumn.ReadOnly = true;
            // 
            // DetailChangeTypeColumn
            // 
            DetailChangeTypeColumn.DataPropertyName = "ChangeType";
            DetailChangeTypeColumn.FillWeight = 12F;
            DetailChangeTypeColumn.HeaderText = "类型";
            DetailChangeTypeColumn.MinimumWidth = 8;
            DetailChangeTypeColumn.Name = "DetailChangeTypeColumn";
            DetailChangeTypeColumn.ReadOnly = true;
            // 
            // DetailDirectoryColumn
            // 
            DetailDirectoryColumn.DataPropertyName = "Directory";
            DetailDirectoryColumn.FillWeight = 28F;
            DetailDirectoryColumn.HeaderText = "目录";
            DetailDirectoryColumn.MinimumWidth = 8;
            DetailDirectoryColumn.Name = "DetailDirectoryColumn";
            DetailDirectoryColumn.ReadOnly = true;
            // 
            // DetailFileNameColumn
            // 
            DetailFileNameColumn.DataPropertyName = "FileName";
            DetailFileNameColumn.FillWeight = 24F;
            DetailFileNameColumn.HeaderText = "文件名";
            DetailFileNameColumn.MinimumWidth = 8;
            DetailFileNameColumn.Name = "DetailFileNameColumn";
            DetailFileNameColumn.ReadOnly = true;
            // 
            // processStatsGrid
            // 
            processStatsGrid.AllowUserToAddRows = false;
            processStatsGrid.AllowUserToDeleteRows = false;
            processStatsGrid.AllowUserToResizeRows = false;
            processStatsGrid.AutoGenerateColumns = false;
            processStatsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            processStatsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            processStatsGrid.Columns.AddRange(new DataGridViewColumn[] { StatsAppNameColumn, StatsChangeCountColumn, StatsFirstChangeColumn, StatsLastChangeColumn });
            processStatsGrid.Dock = DockStyle.Fill;
            processStatsGrid.Location = new Point(0, 0);
            processStatsGrid.Margin = new Padding(0);
            processStatsGrid.Name = "processStatsGrid";
            processStatsGrid.ReadOnly = true;
            processStatsGrid.RowHeadersVisible = false;
            processStatsGrid.RowHeadersWidth = 62;
            processStatsGrid.RowTemplate.Height = 25;
            processStatsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            processStatsGrid.Size = new Size(0, 0);
            processStatsGrid.TabIndex = 1;
            processStatsGrid.Visible = false;
            // 
            // StatsAppNameColumn
            // 
            StatsAppNameColumn.DataPropertyName = "AppName";
            StatsAppNameColumn.FillWeight = 28F;
            StatsAppNameColumn.HeaderText = "进程名";
            StatsAppNameColumn.MinimumWidth = 8;
            StatsAppNameColumn.Name = "StatsAppNameColumn";
            StatsAppNameColumn.ReadOnly = true;
            // 
            // StatsChangeCountColumn
            // 
            StatsChangeCountColumn.DataPropertyName = "ChangeCount";
            StatsChangeCountColumn.FillWeight = 16F;
            StatsChangeCountColumn.HeaderText = "操作次数";
            StatsChangeCountColumn.MinimumWidth = 8;
            StatsChangeCountColumn.Name = "StatsChangeCountColumn";
            StatsChangeCountColumn.ReadOnly = true;
            // 
            // StatsFirstChangeColumn
            // 
            StatsFirstChangeColumn.DataPropertyName = "FirstChangeTime";
            StatsFirstChangeColumn.FillWeight = 28F;
            StatsFirstChangeColumn.HeaderText = "首次时间";
            StatsFirstChangeColumn.MinimumWidth = 8;
            StatsFirstChangeColumn.Name = "StatsFirstChangeColumn";
            StatsFirstChangeColumn.ReadOnly = true;
            // 
            // StatsLastChangeColumn
            // 
            StatsLastChangeColumn.DataPropertyName = "LastChangeTime";
            StatsLastChangeColumn.FillWeight = 28F;
            StatsLastChangeColumn.HeaderText = "最后时间";
            StatsLastChangeColumn.MinimumWidth = 8;
            StatsLastChangeColumn.Name = "StatsLastChangeColumn";
            StatsLastChangeColumn.ReadOnly = true;
            // 
            // notificationRecordsGrid
            // 
            notificationRecordsGrid.AllowUserToAddRows = false;
            notificationRecordsGrid.AllowUserToDeleteRows = false;
            notificationRecordsGrid.AllowUserToResizeRows = false;
            notificationRecordsGrid.AutoGenerateColumns = false;
            notificationRecordsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            notificationRecordsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            notificationRecordsGrid.Columns.AddRange(new DataGridViewColumn[] { NotificationProcessNameColumn, NotificationOperationCountColumn, NotificationDurationColumn, NotificationTriggerTimeColumn });
            notificationRecordsGrid.Dock = DockStyle.Fill;
            notificationRecordsGrid.Location = new Point(0, 0);
            notificationRecordsGrid.Margin = new Padding(0);
            notificationRecordsGrid.Name = "notificationRecordsGrid";
            notificationRecordsGrid.ReadOnly = true;
            notificationRecordsGrid.RowHeadersVisible = false;
            notificationRecordsGrid.RowHeadersWidth = 62;
            notificationRecordsGrid.RowTemplate.Height = 25;
            notificationRecordsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            notificationRecordsGrid.Size = new Size(0, 0);
            notificationRecordsGrid.TabIndex = 0;
            // 
            // NotificationProcessNameColumn
            // 
            NotificationProcessNameColumn.DataPropertyName = "ProcessName";
            NotificationProcessNameColumn.FillWeight = 30F;
            NotificationProcessNameColumn.HeaderText = "进程名";
            NotificationProcessNameColumn.MinimumWidth = 8;
            NotificationProcessNameColumn.Name = "NotificationProcessNameColumn";
            NotificationProcessNameColumn.ReadOnly = true;
            // 
            // NotificationOperationCountColumn
            // 
            NotificationOperationCountColumn.DataPropertyName = "OperationCount";
            NotificationOperationCountColumn.FillWeight = 16F;
            NotificationOperationCountColumn.HeaderText = "操作次数";
            NotificationOperationCountColumn.MinimumWidth = 8;
            NotificationOperationCountColumn.Name = "NotificationOperationCountColumn";
            NotificationOperationCountColumn.ReadOnly = true;
            // 
            // NotificationDurationColumn
            // 
            NotificationDurationColumn.DataPropertyName = "DurationSeconds";
            NotificationDurationColumn.FillWeight = 18F;
            NotificationDurationColumn.HeaderText = "持续时间（秒）";
            NotificationDurationColumn.MinimumWidth = 8;
            NotificationDurationColumn.Name = "NotificationDurationColumn";
            NotificationDurationColumn.ReadOnly = true;
            // 
            // NotificationTriggerTimeColumn
            // 
            NotificationTriggerTimeColumn.DataPropertyName = "TriggerTime";
            NotificationTriggerTimeColumn.FillWeight = 28F;
            NotificationTriggerTimeColumn.HeaderText = "提醒时间";
            NotificationTriggerTimeColumn.MinimumWidth = 8;
            NotificationTriggerTimeColumn.Name = "NotificationTriggerTimeColumn";
            NotificationTriggerTimeColumn.ReadOnly = true;
            // 
            // workspaceStatusBar
            // 
            workspaceStatusBar.BackColor = Color.White;
            workspaceStatusBar.ColumnCount = 4;
            workspaceStatusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            workspaceStatusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            workspaceStatusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            workspaceStatusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            workspaceStatusBar.Controls.Add(workspaceDiskStatus, 0, 0);
            workspaceStatusBar.Controls.Add(workspaceMonitorStatus, 1, 0);
            workspaceStatusBar.Controls.Add(workspaceRecordStatus, 2, 0);
            workspaceStatusBar.Controls.Add(workspaceClockStatus, 3, 0);
            workspaceStatusBar.Dock = DockStyle.Fill;
            workspaceStatusBar.Location = new Point(0, 70);
            workspaceStatusBar.Margin = new Padding(0);
            workspaceStatusBar.Name = "workspaceStatusBar";
            workspaceStatusBar.Padding = new Padding(10, 0, 10, 0);
            workspaceStatusBar.RowCount = 1;
            workspaceStatusBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceStatusBar.Size = new Size(200, 30);
            workspaceStatusBar.TabIndex = 2;
            // 
            // workspaceDiskStatus
            // 
            workspaceDiskStatus.AutoEllipsis = true;
            workspaceDiskStatus.BackColor = Color.White;
            workspaceDiskStatus.Dock = DockStyle.Fill;
            workspaceDiskStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
            workspaceDiskStatus.ForeColor = Color.FromArgb(102, 112, 133);
            workspaceDiskStatus.Location = new Point(10, 0);
            workspaceDiskStatus.Margin = new Padding(0);
            workspaceDiskStatus.Name = "workspaceDiskStatus";
            workspaceDiskStatus.Size = new Size(61, 30);
            workspaceDiskStatus.TabIndex = 0;
            workspaceDiskStatus.Text = "C: 剩余 -- / --";
            workspaceDiskStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // workspaceMonitorStatus
            // 
            workspaceMonitorStatus.AutoEllipsis = true;
            workspaceMonitorStatus.BackColor = Color.White;
            workspaceMonitorStatus.Dock = DockStyle.Fill;
            workspaceMonitorStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
            workspaceMonitorStatus.ForeColor = Color.FromArgb(102, 112, 133);
            workspaceMonitorStatus.Location = new Point(71, 0);
            workspaceMonitorStatus.Margin = new Padding(0);
            workspaceMonitorStatus.Name = "workspaceMonitorStatus";
            workspaceMonitorStatus.Size = new Size(43, 30);
            workspaceMonitorStatus.TabIndex = 1;
            workspaceMonitorStatus.Text = "监控已暂停 · 0 个目录";
            workspaceMonitorStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // workspaceRecordStatus
            // 
            workspaceRecordStatus.AutoEllipsis = true;
            workspaceRecordStatus.BackColor = Color.White;
            workspaceRecordStatus.Dock = DockStyle.Fill;
            workspaceRecordStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
            workspaceRecordStatus.ForeColor = Color.FromArgb(102, 112, 133);
            workspaceRecordStatus.Location = new Point(114, 0);
            workspaceRecordStatus.Margin = new Padding(0);
            workspaceRecordStatus.Name = "workspaceRecordStatus";
            workspaceRecordStatus.Size = new Size(39, 30);
            workspaceRecordStatus.TabIndex = 2;
            workspaceRecordStatus.Text = "当前记录 0 条";
            workspaceRecordStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // workspaceClockStatus
            // 
            workspaceClockStatus.AutoEllipsis = true;
            workspaceClockStatus.BackColor = Color.White;
            workspaceClockStatus.Dock = DockStyle.Fill;
            workspaceClockStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
            workspaceClockStatus.ForeColor = Color.FromArgb(102, 112, 133);
            workspaceClockStatus.Location = new Point(153, 0);
            workspaceClockStatus.Margin = new Padding(0);
            workspaceClockStatus.Name = "workspaceClockStatus";
            workspaceClockStatus.Size = new Size(37, 30);
            workspaceClockStatus.TabIndex = 3;
            workspaceClockStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1440, 860);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1180, 720);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C盘监测工具";
            FormClosing += WorkspaceFormClosing;
            Load += Form1_Load;
            notifyMenuStrip.ResumeLayout(false);
            workspaceRoot.ResumeLayout(false);
            workspaceHeader.ResumeLayout(false);
            workspaceBodyLayout.ResumeLayout(false);
            workspaceNavigation.ResumeLayout(false);
            workspaceMain.ResumeLayout(false);
            workspacePageHeader.ResumeLayout(false);
            workspaceTabControl.ResumeLayout(false);
            dashboardPage.ResumeLayout(false);
            dashboardLayout.ResumeLayout(false);
            dashboardCapacitySurface.ResumeLayout(false);
            dashboardCapacitySurface.PerformLayout();
            dashboardMetrics.ResumeLayout(false);
            dashboardMonitorSurface.ResumeLayout(false);
            dashboardRecordSurface.ResumeLayout(false);
            dashboardRuleSurface.ResumeLayout(false);
            dashboardRecentSurface.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dashboardRecentGrid).EndInit();
            activityPage.ResumeLayout(false);
            activityToolbar.ResumeLayout(false);
            activityToolbar.PerformLayout();
            activitySurface.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)changesDataGrid).EndInit();
            rulesPage.ResumeLayout(false);
            rulesToolbar.ResumeLayout(false);
            rulesSurface.ResumeLayout(false);
            rulesDirectoryView.ResumeLayout(false);
            rulesDirToolbar.ResumeLayout(false);
            rulesProcessView.ResumeLayout(false);
            rulesProcToolbar.ResumeLayout(false);
            rulesProcToolbar.PerformLayout();
            analyzerPage.ResumeLayout(false);
            analyzerToolbar.ResumeLayout(false);
            analyzerToolbar.PerformLayout();
            analyzerContent.ResumeLayout(false);
            analyzerTreeSurface.ResumeLayout(false);
            analyzerDetailsSurface.ResumeLayout(false);
            cleanupPage.ResumeLayout(false);
            cleanupToolbar.ResumeLayout(false);
            cleanupToolbar.PerformLayout();
            cleanupContent.ResumeLayout(false);
            cleanupTreeSurface.ResumeLayout(false);
            cleanupTreeLayout.ResumeLayout(false);
            cleanupSelectionBar.ResumeLayout(false);
            cleanupActionSurface.ResumeLayout(false);
            cleanupActionLayout.ResumeLayout(false);
            cleanupFrequentPanel.ResumeLayout(false);
            cleanupMethodPanel.ResumeLayout(false);
            cleanupMethodPanel.PerformLayout();
            recordsPage.ResumeLayout(false);
            recordsToolbar.ResumeLayout(false);
            recordsSurface.ResumeLayout(false);
            recordViewHost.ResumeLayout(false);
            cleanupRecordView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cleanHistoryGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)detailRecordsGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)processStatsGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)notificationRecordsGrid).EndInit();
            workspaceStatusBar.ResumeLayout(false);
            ResumeLayout(false);


            // 不可见组件

        //this.Controls.Add(notifyIcon1);
        //this.Controls.Add(notifyMenuStrip);
        //this.Controls.Add(exitToolStripMenuItem);
        //this.Controls.Add(ImportFolderDialog);

        // 工作区壳
        this.Controls.Add(workspaceRoot);
        this.Controls.Add(workspaceHeader);
        this.Controls.Add(headerTitleLabel);
        this.Controls.Add(minimizeButton);
        this.Controls.Add(maximizeButton);
        this.Controls.Add(closeButton);
        this.Controls.Add(workspaceBodyLayout);
        this.Controls.Add(workspaceNavigation);
        this.Controls.Add(brandLabel);
        this.Controls.Add(navDashboardButton);
        this.Controls.Add(navActivityButton);
        this.Controls.Add(navRulesButton);
        this.Controls.Add(navAnalyzerButton);
        this.Controls.Add(navCleanupButton);
        this.Controls.Add(navRecordsButton);
        this.Controls.Add(workspaceCollapseButton);
        this.Controls.Add(workspaceMain);
        this.Controls.Add(workspacePageHeader);
        this.Controls.Add(workspacePageTitle);
        this.Controls.Add(workspacePageSubtitle);
        this.Controls.Add(workspaceTabControl);
        this.Controls.Add(dashboardPage);
        this.Controls.Add(activityPage);
        this.Controls.Add(rulesPage);
        this.Controls.Add(analyzerPage);
        this.Controls.Add(cleanupPage);
        this.Controls.Add(recordsPage);
        this.Controls.Add(workspaceStatusBar);
        this.Controls.Add(workspaceDiskStatus);
        this.Controls.Add(workspaceMonitorStatus);
        this.Controls.Add(workspaceRecordStatus);
        this.Controls.Add(workspaceClockStatus);

        this.Controls.Add(dashboardLayout);
        this.Controls.Add(dashboardCapacitySurface);
        this.Controls.Add(dashboardTitleLabel);
        this.Controls.Add(dashboardUsageLabel);
        this.Controls.Add(dashboardDiskProgress);
        this.Controls.Add(dashboardCapacityLabel);
        this.Controls.Add(dashboardMetrics);
        this.Controls.Add(dashboardMonitorSurface);
        this.Controls.Add(dashboardMonitorTitle);
        this.Controls.Add(dashboardMonitorMetric);
        this.Controls.Add(dashboardRecordSurface);
        this.Controls.Add(dashboardRecordTitle);
        this.Controls.Add(dashboardRecordMetric);
        this.Controls.Add(dashboardRuleSurface);
        this.Controls.Add(dashboardRuleTitle);
        this.Controls.Add(dashboardRuleMetric);
        this.Controls.Add(dashboardRecentSurface);
        this.Controls.Add(dashboardRecentTitle);
        this.Controls.Add(dashboardRecentGrid);

        this.Controls.Add(RecentTimestampColumn);
        this.Controls.Add(RecentTypeColumn);
        this.Controls.Add(RecentFileNameColumn);
        this.Controls.Add(RecentSourceColumn);
        this.Controls.Add(RecentDirectoryColumn);

        this.Controls.Add(activityToolbar);
        this.Controls.Add(workspaceMonitorToggleButton);
        this.Controls.Add(typeFilterCombo);
        this.Controls.Add(recordSearchBox);
        this.Controls.Add(exportBtn);
        this.Controls.Add(clearBtn);
        this.Controls.Add(activityRecordCenterButton);
        this.Controls.Add(activitySurface);
        this.Controls.Add(changesDataGrid);
        this.Controls.Add(TimeColumn);
        this.Controls.Add(TypeColumn);
        this.Controls.Add(FileNameColumn);
        this.Controls.Add(PathColumn);
        this.Controls.Add(SizeColumn);
        this.Controls.Add(SourceColumn);

this.Controls.Add(rulesToolbar);
        this.Controls.Add(rulesDirectoryTab);
        this.Controls.Add(rulesProcessTab);
        this.Controls.Add(rulesSurface);
        this.Controls.Add(rulesDirectoryView);
        this.Controls.Add(rulesDirToolbar);
        this.Controls.Add(dirAddButton);
        this.Controls.Add(betterDirAddButton);
        this.Controls.Add(watcherDirListView);
        this.Controls.Add(rulesProcessView);
        this.Controls.Add(rulesProcToolbar);
        this.Controls.Add(manualProcessInput);
        this.Controls.Add(rulesProcessAddButton);
        this.Controls.Add(betterProcessAddButton);
        this.Controls.Add(ignoreProcessView);

this.Controls.Add(analyzerToolbar);
        this.Controls.Add(selectedPathTextBox);
        this.Controls.Add(selectDirBtn);
        this.Controls.Add(scanBtn);
        this.Controls.Add(stopBtn);
        this.Controls.Add(scanProgressBar);
        this.Controls.Add(analyzerContent);
        this.Controls.Add(analyzerTreeSurface);
        this.Controls.Add(folderTreeView);
        this.Controls.Add(analyzerDetailsSurface);
        this.Controls.Add(analyzerDetailsTitle);
        this.Controls.Add(analyzerPathValue);
        this.Controls.Add(analyzerSizeValue);
        this.Controls.Add(analyzerFilesValue);
        this.Controls.Add(analyzerFoldersValue);
        this.Controls.Add(analyzerUseForCleanupButton);

this.Controls.Add(cleanupToolbar);
        this.Controls.Add(cleanPathTextBox);
        this.Controls.Add(cleanSelectDirBtn);
        this.Controls.Add(cleanScanBtn);
        this.Controls.Add(cleanScanProgressBar);
        this.Controls.Add(cleanupContent);
        this.Controls.Add(cleanupTreeSurface);
        this.Controls.Add(cleanupTreeLayout);
        this.Controls.Add(cleanupSelectionBar);
        this.Controls.Add(cleanSelectAllBtn);
        this.Controls.Add(cleanSelectNoneBtn);
        this.Controls.Add(cleanupSelectionLabel);
        this.Controls.Add(cleanTreeView);
        this.Controls.Add(cleanStatusLabel);
        this.Controls.Add(cleanupActionSurface);
        this.Controls.Add(cleanupActionLayout);
        this.Controls.Add(cleanupFrequentPanel);
        this.Controls.Add(frequentRefreshButton);
        this.Controls.Add(frequentPathListView);
        this.Controls.Add(frequentHintLabel);
        this.Controls.Add(cleanupFrequentTitle);
        this.Controls.Add(cleanupMethodPanel);
        this.Controls.Add(cleanupMethodTitle);
        this.Controls.Add(cleanRecycleRadio);
        this.Controls.Add(cleanPermanentRadio);
        this.Controls.Add(cleanMoveRadio);
        this.Controls.Add(cleanCompressRadio);
        this.Controls.Add(cleanMklinkRadio);
        this.Controls.Add(cleanTargetLabel);
        this.Controls.Add(cleanTargetTextBox);
        this.Controls.Add(cleanTargetSelectBtn);
        this.Controls.Add(cleanBtn);

        this.Controls.Add(recordsToolbar);
        this.Controls.Add(recordsNotificationTab);
        this.Controls.Add(recordsStatsTab);
        this.Controls.Add(recordsDetailsTab);
        this.Controls.Add(recordsCleanupTab);
        this.Controls.Add(recordsRefreshButton);
        this.Controls.Add(recordsSurface);
        this.Controls.Add(recordViewHost);
        this.Controls.Add(notificationRecordsGrid);
        this.Controls.Add(NotificationProcessNameColumn);
        this.Controls.Add(NotificationOperationCountColumn);
        this.Controls.Add(NotificationDurationColumn);
        this.Controls.Add(NotificationTriggerTimeColumn);
        this.Controls.Add(processStatsGrid);
        this.Controls.Add(StatsAppNameColumn);
        this.Controls.Add(StatsChangeCountColumn);
        this.Controls.Add(StatsFirstChangeColumn);
        this.Controls.Add(StatsLastChangeColumn);
        this.Controls.Add(detailRecordsGrid);
        this.Controls.Add(DetailTimestampColumn);
        this.Controls.Add(DetailSourceProcessColumn);
        this.Controls.Add(DetailChangeTypeColumn);
        this.Controls.Add(DetailDirectoryColumn);
        this.Controls.Add(DetailFileNameColumn);
        this.Controls.Add(cleanupRecordView);
        this.Controls.Add(cleanHistoryGrid);
        this.Controls.Add(cleanHistoryEmptyLabel);

        }

        #endregion

        // 不可见组件
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer diskRefreshTimer;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip notifyMenuStrip;
        private ToolStripMenuItem exitToolStripMenuItem;
        private FolderBrowserDialog ImportFolderDialog;

        // 工作区壳
        private TableLayoutPanel workspaceRoot;
        private Panel workspaceHeader;
        private Label headerTitleLabel;
        private Button minimizeButton;
        private Button maximizeButton;
        private Button closeButton;
        private TableLayoutPanel workspaceBodyLayout;
        private Panel workspaceNavigation;
        private Label brandLabel;
        private Button navDashboardButton;
        private Button navActivityButton;
        private Button navRulesButton;
        private Button navAnalyzerButton;
        private Button navCleanupButton;
        private Button navRecordsButton;
        private Button workspaceCollapseButton;
        private TableLayoutPanel workspaceMain;
        private Panel workspacePageHeader;
        private Label workspacePageTitle;
        private Label workspacePageSubtitle;
        private TabControl workspaceTabControl;
        private TabPage dashboardPage;
        private TabPage activityPage;
        private TabPage rulesPage;
        private TabPage analyzerPage;
        private TabPage cleanupPage;
        private TabPage recordsPage;
        private TableLayoutPanel workspaceStatusBar;
        private Label workspaceDiskStatus;
        private Label workspaceMonitorStatus;
        private Label workspaceRecordStatus;
        private Label workspaceClockStatus;

        // 工作台页
        private TableLayoutPanel dashboardLayout;
        private Panel dashboardCapacitySurface;
        private Label dashboardTitleLabel;
        private Label dashboardUsageLabel;
        private ProgressBar dashboardDiskProgress;
        private Label dashboardCapacityLabel;
        private TableLayoutPanel dashboardMetrics;
        private Panel dashboardMonitorSurface;
        private Label dashboardMonitorTitle;
        private Label dashboardMonitorMetric;
        private Panel dashboardRecordSurface;
        private Label dashboardRecordTitle;
        private Label dashboardRecordMetric;
        private Panel dashboardRuleSurface;
        private Label dashboardRuleTitle;
        private Label dashboardRuleMetric;
        private Panel dashboardRecentSurface;
        private Label dashboardRecentTitle;
        private DataGridView dashboardRecentGrid;
        private DataGridViewTextBoxColumn RecentTimestampColumn;
        private DataGridViewTextBoxColumn RecentTypeColumn;
        private DataGridViewTextBoxColumn RecentFileNameColumn;
        private DataGridViewTextBoxColumn RecentSourceColumn;
        private DataGridViewTextBoxColumn RecentDirectoryColumn;

        // 实时活动页
        private FlowLayoutPanel activityToolbar;
        private Button workspaceMonitorToggleButton;
        private ComboBox typeFilterCombo;
        private TextBox recordSearchBox;
        private Button exportBtn;
        private Button clearBtn;
        private Button activityRecordCenterButton;
        private Panel activitySurface;
        private DataGridView changesDataGrid;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn TypeColumn;
        private DataGridViewTextBoxColumn FileNameColumn;
        private DataGridViewTextBoxColumn PathColumn;
        private DataGridViewTextBoxColumn SizeColumn;
        private DataGridViewTextBoxColumn SourceColumn;

        // 监控规则页
        private FlowLayoutPanel rulesToolbar;
        private Button rulesDirectoryTab;
        private Button rulesProcessTab;
        private Panel rulesSurface;
        private Panel rulesDirectoryView;
        private FlowLayoutPanel rulesDirToolbar;
        private Button dirAddButton;
        private Button betterDirAddButton;
        private ListView watcherDirListView;
        private Panel rulesProcessView;
        private FlowLayoutPanel rulesProcToolbar;
        private TextBox manualProcessInput;
        private Button rulesProcessAddButton;
        private Button betterProcessAddButton;
        private ListView ignoreProcessView;

        // 空间分析页
        private TableLayoutPanel analyzerToolbar;
        private TextBox selectedPathTextBox;
        private Button selectDirBtn;
        private Button scanBtn;
        private Button stopBtn;
        private ProgressBar scanProgressBar;
        private TableLayoutPanel analyzerContent;
        private Panel analyzerTreeSurface;
        private TreeView folderTreeView;
        private Panel analyzerDetailsSurface;
        private Label analyzerDetailsTitle;
        private Label analyzerPathValue;
        private Label analyzerSizeValue;
        private Label analyzerFilesValue;
        private Label analyzerFoldersValue;
        private Button analyzerUseForCleanupButton;

        // 清理中心页
        private TableLayoutPanel cleanupToolbar;
        private TextBox cleanPathTextBox;
        private Button cleanSelectDirBtn;
        private Button cleanScanBtn;
        private ProgressBar cleanScanProgressBar;
        private TableLayoutPanel cleanupContent;
        private Panel cleanupTreeSurface;
        private TableLayoutPanel cleanupTreeLayout;
        private FlowLayoutPanel cleanupSelectionBar;
        private Button cleanSelectAllBtn;
        private Button cleanSelectNoneBtn;
        private Label cleanupSelectionLabel;
        private TreeView cleanTreeView;
        private Label cleanStatusLabel;
        private Panel cleanupActionSurface;
        private TableLayoutPanel cleanupActionLayout;
        private Panel cleanupFrequentPanel;
        private Button frequentRefreshButton;
        private ListView frequentPathListView;
        private Label frequentHintLabel;
        private Label cleanupFrequentTitle;
        private Panel cleanupMethodPanel;
        private Label cleanupMethodTitle;
        private RadioButton cleanRecycleRadio;
        private RadioButton cleanPermanentRadio;
        private RadioButton cleanMoveRadio;
        private RadioButton cleanCompressRadio;
        private RadioButton cleanMklinkRadio;
        private Label cleanTargetLabel;
        private TextBox cleanTargetTextBox;
        private Button cleanTargetSelectBtn;
        private Button cleanBtn;

        // 记录中心页
        private FlowLayoutPanel recordsToolbar;
        private Button recordsNotificationTab;
        private Button recordsStatsTab;
        private Button recordsDetailsTab;
        private Button recordsCleanupTab;
        private Button recordsRefreshButton;
        private Panel recordsSurface;
        private Panel recordViewHost;
        private DataGridView notificationRecordsGrid;
        private DataGridViewTextBoxColumn NotificationProcessNameColumn;
        private DataGridViewTextBoxColumn NotificationOperationCountColumn;
        private DataGridViewTextBoxColumn NotificationDurationColumn;
        private DataGridViewTextBoxColumn NotificationTriggerTimeColumn;
        private DataGridView processStatsGrid;
        private DataGridViewTextBoxColumn StatsAppNameColumn;
        private DataGridViewTextBoxColumn StatsChangeCountColumn;
        private DataGridViewTextBoxColumn StatsFirstChangeColumn;
        private DataGridViewTextBoxColumn StatsLastChangeColumn;
        private DataGridView detailRecordsGrid;
        private DataGridViewTextBoxColumn DetailTimestampColumn;
        private DataGridViewTextBoxColumn DetailSourceProcessColumn;
        private DataGridViewTextBoxColumn DetailChangeTypeColumn;
        private DataGridViewTextBoxColumn DetailDirectoryColumn;
        private DataGridViewTextBoxColumn DetailFileNameColumn;
        private Panel cleanupRecordView;
        private DataGridView cleanHistoryGrid;
        private Label cleanHistoryEmptyLabel;
    }
}
