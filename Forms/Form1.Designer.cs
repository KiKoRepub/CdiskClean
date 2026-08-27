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
            AntdUI.MenuItem menuItem1 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem2 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem3 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem4 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem5 = new AntdUI.MenuItem();
            AntdUI.MenuItem menuItem6 = new AntdUI.MenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            diskRefreshTimer = new System.Windows.Forms.Timer(components);
            notifyIcon1 = new NotifyIcon(components);
            notifyMenuStrip = new ContextMenuStrip(components);
            startMonitorNotifyItem = new ToolStripMenuItem();
            选择模式ToolStripMenuItem = new ToolStripMenuItem();
            进程模式ToolStripMenuItem = new ToolStripMenuItem();
            默认ToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            ImportFolderDialog = new FolderBrowserDialog();
            workspaceRoot = new TableLayoutPanel();
            workspaceHeader = new AntdUI.PageHeader();
            workspaceBodyLayout = new TableLayoutPanel();
            workspaceNavigation = new AntdUI.Panel();
            brandLabel = new Label();
            workspaceMenu = new AntdUI.Menu();
            workspaceCollapseButton = new AntdUI.Button();
            workspaceMain = new TableLayoutPanel();
            workspacePageHeader = new Panel();
            workspacePageTitle = new Label();
            workspacePageSubtitle = new Label();
            workspacePageContainer = new Panel();
            cleanupPanel = new Panel();
            cleanupToolbar = new TableLayoutPanel();
            cleanPathTextBox = new TextBox();
            cleanSelectDirBtn = new Button();
            cleanScanBtn = new Button();
            cleanScanProgressBar = new ProgressBar();
            cleanupContent = new TableLayoutPanel();
            cleanupTreeSurface = new AntdUI.Panel();
            cleanupTreeLayout = new TableLayoutPanel();
            cleanupSelectionBarRow = new Panel();
            cleanupSelectionBar = new FlowLayoutPanel();
            cleanSelectAllBtn = new Button();
            cleanSelectNoneBtn = new Button();
            cleanupSelectionLabel = new Label();
            riskLegendBar = new FlowLayoutPanel();
            riskHighChip = new Panel();
            riskHighLabel = new Label();
            riskMediumChip = new Panel();
            riskMediumLabel = new Label();
            riskLowChip = new Panel();
            riskLowLabel = new Label();
            cleanTreeView = new AntdUI.Tree();
            cleanStatusLabel = new Label();
            cleanupActionSurface = new AntdUI.Panel();
            cleanupActionLayout = new TableLayoutPanel();
            cleanupFrequentPanel = new Panel();
            frequentRefreshButton = new AntdUI.Button();
            frequentPathListView = new ListView();
            frequentHintLabel = new Label();
            cleanupFrequentTitle = new Label();
            cleanupMethodPanel = new Panel();
            cleanButton = new AntdUI.Button();
            cleanupMethodTitle = new Label();
            cleanRecycleRadio = new RadioButton();
            cleanPermanentRadio = new RadioButton();
            cleanMoveRadio = new RadioButton();
            cleanCompressRadio = new RadioButton();
            cleanMklinkRadio = new RadioButton();
            cleanTargetLabel = new Label();
            cleanTargetTextBox = new TextBox();
            cleanTargetSelectBtn = new Button();
            rulesPanel = new Panel();
            rulesExeProcessView = new Panel();
            rulesExeProcViewTable = new AntdUI.Table();
            rulesExeProcToolBar = new FlowLayoutPanel();
            input1 = new AntdUI.Input();
            rulesExeProcAddButton = new AntdUI.Button();
            button1 = new AntdUI.Button();
            rulesToolbar = new FlowLayoutPanel();
            rulesDirectoryTab = new AntdUI.Button();
            rulesProcessTab = new AntdUI.Button();
            rulesExeTab = new AntdUI.Button();
            rulesSurface = new AntdUI.Panel();
            rulesIgnoreProcessView = new Panel();
            ignoreProcessListView = new ListView();
            rulesIgnoreProcToolbar = new FlowLayoutPanel();
            manualProcessInput = new AntdUI.Input();
            rulesProcessAddButton = new AntdUI.Button();
            betterIngnoreProcAddButton = new AntdUI.Button();
            rulesDirectoryView = new Panel();
            rulesDirToolbar = new FlowLayoutPanel();
            dirAddButton = new AntdUI.Button();
            betterDirAddButton = new AntdUI.Button();
            watcherDirListView = new ListView();
            activityPanel = new Panel();
            activityToolbar = new FlowLayoutPanel();
            panel2 = new AntdUI.Panel();
            workspaceMonitorToggleButton = new AntdUI.Button();
            clearBtn = new AntdUI.Button();
            activityRecordCenterButton = new AntdUI.Button();
            typeFilterCombo = new ComboBox();
            exportBtn = new AntdUI.Button();
            recordSearchBox = new AntdUI.Input();
            panel1 = new AntdUI.Panel();
            label1 = new AntdUI.Label();
            exeModeRadio = new AntdUI.Radio();
            defaultModeRadio = new AntdUI.Radio();
            activitySurface = new AntdUI.Panel();
            activityRecordTable = new AntdUI.Table();
            recordsPanel = new Panel();
            recordsToolbar = new FlowLayoutPanel();
            recordsNotificationTab = new AntdUI.Button();
            recordsStatsTab = new AntdUI.Button();
            recordsDetailsTab = new AntdUI.Button();
            recordsCleanupTab = new AntdUI.Button();
            recordsRefreshButton = new AntdUI.Button();
            recordsSurface = new AntdUI.Panel();
            recordViewHost = new Panel();
            cleanupRecordView = new Panel();
            cleanHistoryTable = new AntdUI.Table();
            cleanHistoryEmptyLabel = new Label();
            detailRecordsTable = new AntdUI.Table();
            processStatsTable = new AntdUI.Table();
            notificationRecordsTable = new AntdUI.Table();
            dashboardPanel = new Panel();
            dashboardLayout = new TableLayoutPanel();
            dashboardCapacitySurface = new AntdUI.Panel();
            dashboardTitleLabel = new Label();
            dashboardUsageLabel = new Label();
            dashboardDiskProgress = new AntdUI.Progress();
            dashboardCapacityLabel = new Label();
            dashboardMetrics = new TableLayoutPanel();
            dashboardMonitorSurface = new AntdUI.Panel();
            dashboardMonitorTitle = new Label();
            dashboardMonitorMetric = new Label();
            dashboardRecordSurface = new AntdUI.Panel();
            dashboardRecordTitle = new Label();
            dashboardRecordMetric = new Label();
            dashboardRuleSurface = new AntdUI.Panel();
            dashboardRuleTitle = new Label();
            dashboardRuleMetric = new Label();
            dashboardRecentSurface = new AntdUI.Panel();
            dashboardRecentTitle = new Label();
            dashboardRecentTable = new AntdUI.Table();
            analyzerPanel = new Panel();
            analyzerToolbar = new TableLayoutPanel();
            selectedPathTextBox = new TextBox();
            selectDirBtn = new Button();
            scanBtn = new Button();
            stopBtn = new Button();
            scanProgressBar = new ProgressBar();
            analyzerContent = new TableLayoutPanel();
            analyzerTreeSurface = new AntdUI.Panel();
            folderTreeView = new TreeView();
            analyzerDetailsSurface = new AntdUI.Panel();
            analyzerDetailsTitle = new Label();
            analyzerPathValue = new Label();
            analyzerSizeValue = new Label();
            analyzerFilesValue = new Label();
            analyzerFoldersValue = new Label();
            analyzerUseForCleanupButton = new AntdUI.Button();
            workspaceStatusBar = new TableLayoutPanel();
            workspaceDiskStatus = new Label();
            workspaceMonitorStatus = new Label();
            workspaceRecordStatus = new Label();
            workspaceClockStatus = new Label();
            notifyRotateTimer = new System.Windows.Forms.Timer(components);
            notifyMenuStrip.SuspendLayout();
            workspaceRoot.SuspendLayout();
            workspaceBodyLayout.SuspendLayout();
            workspaceNavigation.SuspendLayout();
            workspaceMain.SuspendLayout();
            workspacePageHeader.SuspendLayout();
            workspacePageContainer.SuspendLayout();
            cleanupPanel.SuspendLayout();
            cleanupToolbar.SuspendLayout();
            cleanupContent.SuspendLayout();
            cleanupTreeSurface.SuspendLayout();
            cleanupTreeLayout.SuspendLayout();
            cleanupSelectionBarRow.SuspendLayout();
            cleanupSelectionBar.SuspendLayout();
            riskLegendBar.SuspendLayout();
            cleanupActionSurface.SuspendLayout();
            cleanupActionLayout.SuspendLayout();
            cleanupFrequentPanel.SuspendLayout();
            cleanupMethodPanel.SuspendLayout();
            rulesPanel.SuspendLayout();
            rulesExeProcessView.SuspendLayout();
            rulesExeProcToolBar.SuspendLayout();
            rulesToolbar.SuspendLayout();
            rulesSurface.SuspendLayout();
            rulesIgnoreProcessView.SuspendLayout();
            rulesIgnoreProcToolbar.SuspendLayout();
            rulesDirectoryView.SuspendLayout();
            rulesDirToolbar.SuspendLayout();
            activityPanel.SuspendLayout();
            activityToolbar.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            activitySurface.SuspendLayout();
            recordsPanel.SuspendLayout();
            recordsToolbar.SuspendLayout();
            recordsSurface.SuspendLayout();
            recordViewHost.SuspendLayout();
            cleanupRecordView.SuspendLayout();
            dashboardPanel.SuspendLayout();
            dashboardLayout.SuspendLayout();
            dashboardCapacitySurface.SuspendLayout();
            dashboardMetrics.SuspendLayout();
            dashboardMonitorSurface.SuspendLayout();
            dashboardRecordSurface.SuspendLayout();
            dashboardRuleSurface.SuspendLayout();
            dashboardRecentSurface.SuspendLayout();
            analyzerPanel.SuspendLayout();
            analyzerToolbar.SuspendLayout();
            analyzerContent.SuspendLayout();
            analyzerTreeSurface.SuspendLayout();
            analyzerDetailsSurface.SuspendLayout();
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
            notifyMenuStrip.Items.AddRange(new ToolStripItem[] { startMonitorNotifyItem, 选择模式ToolStripMenuItem, exitToolStripMenuItem });
            notifyMenuStrip.Name = "notifyMenuStrip";
            notifyMenuStrip.Size = new Size(153, 94);
            // 
            // startMonitorNotifyItem
            // 
            startMonitorNotifyItem.Name = "startMonitorNotifyItem";
            startMonitorNotifyItem.Size = new Size(152, 30);
            startMonitorNotifyItem.Text = "启动监测";
            startMonitorNotifyItem.Click += startMonitorNotifyItem_Click;
            // 
            // 选择模式ToolStripMenuItem
            // 
            选择模式ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 进程模式ToolStripMenuItem, 默认ToolStripMenuItem });
            选择模式ToolStripMenuItem.Name = "选择模式ToolStripMenuItem";
            选择模式ToolStripMenuItem.Size = new Size(152, 30);
            选择模式ToolStripMenuItem.Text = "监控规则";
            // 
            // 进程模式ToolStripMenuItem
            // 
            进程模式ToolStripMenuItem.Name = "进程模式ToolStripMenuItem";
            进程模式ToolStripMenuItem.Size = new Size(182, 34);
            进程模式ToolStripMenuItem.Text = "特定程序";
            // 
            // 默认ToolStripMenuItem
            // 
            默认ToolStripMenuItem.Checked = true;
            默认ToolStripMenuItem.CheckState = CheckState.Checked;
            默认ToolStripMenuItem.Name = "默认ToolStripMenuItem";
            默认ToolStripMenuItem.Size = new Size(182, 34);
            默认ToolStripMenuItem.Text = "默认";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(152, 30);
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
            workspaceRoot.Size = new Size(1440, 860);
            workspaceRoot.TabIndex = 0;
            // 
            // workspaceHeader
            // 
            workspaceHeader.BackColor = Color.Cyan;
            workspaceHeader.Dock = DockStyle.Fill;
            workspaceHeader.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold);
            workspaceHeader.ForeColor = Color.FromArgb(31, 41, 55);
            workspaceHeader.Location = new Point(0, 0);
            workspaceHeader.Margin = new Padding(0);
            workspaceHeader.Name = "workspaceHeader";
            workspaceHeader.ShowButton = true;
            workspaceHeader.Size = new Size(1440, 48);
            workspaceHeader.TabIndex = 0;
            workspaceHeader.Text = "CdiskClean  C盘监测与清理";
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
            workspaceBodyLayout.Size = new Size(1440, 782);
            workspaceBodyLayout.TabIndex = 1;
            // 
            // workspaceNavigation
            // 
            workspaceNavigation.BackColor = Color.White;
            workspaceNavigation.Controls.Add(brandLabel);
            workspaceNavigation.Controls.Add(workspaceMenu);
            workspaceNavigation.Controls.Add(workspaceCollapseButton);
            workspaceNavigation.Dock = DockStyle.Fill;
            workspaceNavigation.Location = new Point(0, 0);
            workspaceNavigation.Margin = new Padding(0);
            workspaceNavigation.Name = "workspaceNavigation";
            workspaceNavigation.Padding = new Padding(8, 12, 8, 8);
            workspaceNavigation.Radius = 0;
            workspaceNavigation.Size = new Size(208, 782);
            workspaceNavigation.TabIndex = 0;
            // 
            // brandLabel
            // 
            brandLabel.BackColor = Color.White;
            brandLabel.Dock = DockStyle.Top;
            brandLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            brandLabel.ForeColor = Color.FromArgb(31, 41, 55);
            brandLabel.Location = new Point(8, 12);
            brandLabel.Name = "brandLabel";
            brandLabel.Size = new Size(192, 48);
            brandLabel.TabIndex = 0;
            brandLabel.Text = "  CDISK CLEAN";
            brandLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // workspaceMenu
            // 
            workspaceMenu.BackColor = Color.White;
            menuItem1.IconSvg = "DashboardOutlined";
            menuItem1.ID = "dashboard";
            menuItem1.Select = true;
            menuItem1.Text = "工作台";
            menuItem2.IconSvg = "MonitorOutlined";
            menuItem2.ID = "activity";
            menuItem2.Text = "实时活动";
            menuItem3.IconSvg = "ControlOutlined";
            menuItem3.ID = "rules";
            menuItem3.Text = "监控规则";
            menuItem4.IconSvg = "PieChartOutlined";
            menuItem4.ID = "analyzer";
            menuItem4.Text = "空间分析";
            menuItem5.IconSvg = "DeleteOutlined";
            menuItem5.ID = "cleanup";
            menuItem5.Text = "清理中心";
            menuItem6.IconSvg = "HistoryOutlined";
            menuItem6.ID = "records";
            menuItem6.Text = "记录中心";
            workspaceMenu.Items.Add(menuItem1);
            workspaceMenu.Items.Add(menuItem2);
            workspaceMenu.Items.Add(menuItem3);
            workspaceMenu.Items.Add(menuItem4);
            workspaceMenu.Items.Add(menuItem5);
            workspaceMenu.Items.Add(menuItem6);
            workspaceMenu.Location = new Point(8, 66);
            workspaceMenu.Margin = new Padding(0);
            workspaceMenu.Name = "workspaceMenu";
            workspaceMenu.Radius = 5;
            workspaceMenu.Size = new Size(192, 664);
            workspaceMenu.TabIndex = 1;
            workspaceMenu.ItemClick += workspaceMenu_ItemClick;
            // 
            // workspaceCollapseButton
            // 
            workspaceCollapseButton.Dock = DockStyle.Bottom;
            workspaceCollapseButton.IconSvg = "ControlOutlined";
            workspaceCollapseButton.Location = new Point(8, 732);
            workspaceCollapseButton.Margin = new Padding(4, 2, 4, 2);
            workspaceCollapseButton.Name = "workspaceCollapseButton";
            workspaceCollapseButton.Radius = 1;
            workspaceCollapseButton.Size = new Size(192, 42);
            workspaceCollapseButton.TabIndex = 2;
            workspaceCollapseButton.Text = "折叠菜单";
            workspaceCollapseButton.Click += workspaceCollapseButton_Click;
            // 
            // workspaceMain
            // 
            workspaceMain.ColumnCount = 1;
            workspaceMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            workspaceMain.Controls.Add(workspacePageHeader, 0, 0);
            workspaceMain.Controls.Add(workspacePageContainer, 0, 1);
            workspaceMain.Dock = DockStyle.Fill;
            workspaceMain.Location = new Point(208, 0);
            workspaceMain.Margin = new Padding(0);
            workspaceMain.Name = "workspaceMain";
            workspaceMain.RowCount = 2;
            workspaceMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            workspaceMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceMain.Size = new Size(1232, 782);
            workspaceMain.TabIndex = 1;
            // 
            // workspacePageHeader
            // 
            workspacePageHeader.BackColor = Color.White;
            workspacePageHeader.Controls.Add(workspacePageTitle);
            workspacePageHeader.Controls.Add(workspacePageSubtitle);
            workspacePageHeader.Dock = DockStyle.Top;
            workspacePageHeader.Location = new Point(0, 0);
            workspacePageHeader.Margin = new Padding(0);
            workspacePageHeader.Name = "workspacePageHeader";
            workspacePageHeader.Padding = new Padding(20, 8, 20, 7);
            workspacePageHeader.Size = new Size(1232, 90);
            workspacePageHeader.TabIndex = 0;
            // 
            // workspacePageTitle
            // 
            workspacePageTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            workspacePageTitle.BackColor = Color.Transparent;
            workspacePageTitle.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            workspacePageTitle.ForeColor = Color.FromArgb(31, 41, 55);
            workspacePageTitle.Location = new Point(3, 8);
            workspacePageTitle.Name = "workspacePageTitle";
            workspacePageTitle.Size = new Size(1206, 43);
            workspacePageTitle.TabIndex = 0;
            workspacePageTitle.Text = "工作台";
            // 
            // workspacePageSubtitle
            // 
            workspacePageSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            workspacePageSubtitle.BackColor = Color.Transparent;
            workspacePageSubtitle.Font = new Font("Microsoft YaHei UI", 9F);
            workspacePageSubtitle.ForeColor = Color.FromArgb(102, 112, 133);
            workspacePageSubtitle.Location = new Point(2, 61);
            workspacePageSubtitle.Name = "workspacePageSubtitle";
            workspacePageSubtitle.Size = new Size(1207, 22);
            workspacePageSubtitle.TabIndex = 1;
            workspacePageSubtitle.Text = "查看磁盘空间、监控健康状态与最近文件活动";
            // 
            // workspacePageContainer
            // 
            workspacePageContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            workspacePageContainer.Controls.Add(cleanupPanel);
            workspacePageContainer.Controls.Add(rulesPanel);
            workspacePageContainer.Controls.Add(activityPanel);
            workspacePageContainer.Controls.Add(recordsPanel);
            workspacePageContainer.Controls.Add(dashboardPanel);
            workspacePageContainer.Controls.Add(analyzerPanel);
            workspacePageContainer.Location = new Point(0, 90);
            workspacePageContainer.Margin = new Padding(0);
            workspacePageContainer.Name = "workspacePageContainer";
            workspacePageContainer.Size = new Size(1232, 692);
            workspacePageContainer.TabIndex = 0;
            // 
            // cleanupPanel
            // 
            cleanupPanel.BackColor = Color.FromArgb(245, 247, 250);
            cleanupPanel.Controls.Add(cleanupToolbar);
            cleanupPanel.Controls.Add(cleanScanProgressBar);
            cleanupPanel.Controls.Add(cleanupContent);
            cleanupPanel.Location = new Point(8, 8);
            cleanupPanel.Name = "cleanupPanel";
            cleanupPanel.Padding = new Padding(18);
            cleanupPanel.Size = new Size(1232, 692);
            cleanupPanel.TabIndex = 8;
            cleanupPanel.Visible = false;
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
            cleanupToolbar.Location = new Point(18, 41);
            cleanupToolbar.Margin = new Padding(0);
            cleanupToolbar.Name = "cleanupToolbar";
            cleanupToolbar.RowCount = 1;
            cleanupToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupToolbar.Size = new Size(1196, 52);
            cleanupToolbar.TabIndex = 0;
            // 
            // cleanPathTextBox
            // 
            cleanPathTextBox.Dock = DockStyle.Fill;
            cleanPathTextBox.Location = new Point(0, 7);
            cleanPathTextBox.Margin = new Padding(0, 7, 8, 7);
            cleanPathTextBox.Name = "cleanPathTextBox";
            cleanPathTextBox.Size = new Size(972, 30);
            cleanPathTextBox.TabIndex = 0;
            // 
            // cleanSelectDirBtn
            // 
            cleanSelectDirBtn.Dock = DockStyle.Fill;
            cleanSelectDirBtn.FlatStyle = FlatStyle.Flat;
            cleanSelectDirBtn.Location = new Point(984, 6);
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
            cleanScanBtn.Location = new Point(1092, 6);
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
            cleanScanProgressBar.Location = new Point(18, 18);
            cleanScanProgressBar.Margin = new Padding(0, 5, 0, 5);
            cleanScanProgressBar.Name = "cleanScanProgressBar";
            cleanScanProgressBar.Size = new Size(1196, 23);
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
            cleanupContent.Dock = DockStyle.Bottom;
            cleanupContent.Location = new Point(18, 103);
            cleanupContent.Margin = new Padding(0);
            cleanupContent.Name = "cleanupContent";
            cleanupContent.RowCount = 1;
            cleanupContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupContent.Size = new Size(1196, 571);
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
            cleanupTreeSurface.Size = new Size(844, 561);
            cleanupTreeSurface.TabIndex = 0;
            // 
            // cleanupTreeLayout
            // 
            cleanupTreeLayout.BackColor = Color.White;
            cleanupTreeLayout.ColumnCount = 1;
            cleanupTreeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cleanupTreeLayout.Controls.Add(cleanupSelectionBarRow, 0, 0);
            cleanupTreeLayout.Controls.Add(cleanTreeView, 0, 1);
            cleanupTreeLayout.Controls.Add(cleanStatusLabel, 0, 2);
            cleanupTreeLayout.Dock = DockStyle.Top;
            cleanupTreeLayout.Location = new Point(12, 12);
            cleanupTreeLayout.Margin = new Padding(0);
            cleanupTreeLayout.Name = "cleanupTreeLayout";
            cleanupTreeLayout.RowCount = 3;
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            cleanupTreeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            cleanupTreeLayout.Size = new Size(820, 549);
            cleanupTreeLayout.TabIndex = 0;
            // 
            // cleanupSelectionBarRow
            // 
            cleanupSelectionBarRow.BackColor = Color.White;
            cleanupSelectionBarRow.Controls.Add(cleanupSelectionBar);
            cleanupSelectionBarRow.Controls.Add(riskLegendBar);
            cleanupSelectionBarRow.Dock = DockStyle.Fill;
            cleanupSelectionBarRow.Location = new Point(0, 0);
            cleanupSelectionBarRow.Margin = new Padding(0);
            cleanupSelectionBarRow.Name = "cleanupSelectionBarRow";
            cleanupSelectionBarRow.Size = new Size(820, 42);
            cleanupSelectionBarRow.TabIndex = 0;
            // 
            // cleanupSelectionBar
            // 
            cleanupSelectionBar.Controls.Add(cleanSelectAllBtn);
            cleanupSelectionBar.Controls.Add(cleanSelectNoneBtn);
            cleanupSelectionBar.Controls.Add(cleanupSelectionLabel);
            cleanupSelectionBar.Dock = DockStyle.Left;
            cleanupSelectionBar.Location = new Point(0, 0);
            cleanupSelectionBar.Margin = new Padding(0);
            cleanupSelectionBar.Name = "cleanupSelectionBar";
            cleanupSelectionBar.Size = new Size(500, 42);
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
            // riskLegendBar
            // 
            riskLegendBar.BackColor = Color.White;
            riskLegendBar.Controls.Add(riskHighChip);
            riskLegendBar.Controls.Add(riskHighLabel);
            riskLegendBar.Controls.Add(riskMediumChip);
            riskLegendBar.Controls.Add(riskMediumLabel);
            riskLegendBar.Controls.Add(riskLowChip);
            riskLegendBar.Controls.Add(riskLowLabel);
            riskLegendBar.Dock = DockStyle.Right;
            riskLegendBar.Location = new Point(605, 0);
            riskLegendBar.Margin = new Padding(0);
            riskLegendBar.Name = "riskLegendBar";
            riskLegendBar.Size = new Size(215, 42);
            riskLegendBar.TabIndex = 3;
            riskLegendBar.WrapContents = false;
            // 
            // riskHighChip
            // 
            riskHighChip.BackColor = Color.FromArgb(255, 230, 230);
            riskHighChip.Location = new Point(0, 13);
            riskHighChip.Margin = new Padding(0, 13, 4, 0);
            riskHighChip.Name = "riskHighChip";
            riskHighChip.Size = new Size(12, 12);
            riskHighChip.TabIndex = 4;
            // 
            // riskHighLabel
            // 
            riskHighLabel.AutoSize = true;
            riskHighLabel.BackColor = Color.Transparent;
            riskHighLabel.Font = new Font("Microsoft YaHei UI", 9.5F);
            riskHighLabel.ForeColor = Color.FromArgb(107, 114, 128);
            riskHighLabel.Location = new Point(16, 12);
            riskHighLabel.Margin = new Padding(0, 12, 8, 0);
            riskHighLabel.Name = "riskHighLabel";
            riskHighLabel.Size = new Size(69, 25);
            riskHighLabel.TabIndex = 5;
            riskHighLabel.Text = "高风险";
            // 
            // riskMediumChip
            // 
            riskMediumChip.BackColor = Color.FromArgb(255, 255, 220);
            riskMediumChip.Location = new Point(93, 13);
            riskMediumChip.Margin = new Padding(0, 13, 4, 0);
            riskMediumChip.Name = "riskMediumChip";
            riskMediumChip.Size = new Size(12, 12);
            riskMediumChip.TabIndex = 6;
            // 
            // riskMediumLabel
            // 
            riskMediumLabel.AutoSize = true;
            riskMediumLabel.BackColor = Color.Transparent;
            riskMediumLabel.Font = new Font("Microsoft YaHei UI", 9.5F);
            riskMediumLabel.ForeColor = Color.FromArgb(107, 114, 128);
            riskMediumLabel.Location = new Point(109, 12);
            riskMediumLabel.Margin = new Padding(0, 12, 8, 0);
            riskMediumLabel.Name = "riskMediumLabel";
            riskMediumLabel.Size = new Size(69, 25);
            riskMediumLabel.TabIndex = 7;
            riskMediumLabel.Text = "中风险";
            // 
            // riskLowChip
            // 
            riskLowChip.BackColor = Color.FromArgb(230, 255, 230);
            riskLowChip.Location = new Point(186, 13);
            riskLowChip.Margin = new Padding(0, 13, 4, 0);
            riskLowChip.Name = "riskLowChip";
            riskLowChip.Size = new Size(12, 12);
            riskLowChip.TabIndex = 8;
            // 
            // riskLowLabel
            // 
            riskLowLabel.AutoSize = true;
            riskLowLabel.BackColor = Color.Transparent;
            riskLowLabel.Font = new Font("Microsoft YaHei UI", 9.5F);
            riskLowLabel.ForeColor = Color.FromArgb(107, 114, 128);
            riskLowLabel.Location = new Point(202, 12);
            riskLowLabel.Margin = new Padding(0, 12, 8, 0);
            riskLowLabel.Name = "riskLowLabel";
            riskLowLabel.Size = new Size(69, 25);
            riskLowLabel.TabIndex = 9;
            riskLowLabel.Text = "低风险";
            // 
            // cleanTreeView
            // 
            cleanTreeView.BlockNode = true;
            cleanTreeView.Checkable = true;
            cleanTreeView.CheckStrictly = false;
            cleanTreeView.Dock = DockStyle.Fill;
            cleanTreeView.Location = new Point(0, 42);
            cleanTreeView.Margin = new Padding(0);
            cleanTreeView.Name = "cleanTreeView";
            cleanTreeView.Size = new Size(820, 473);
            cleanTreeView.TabIndex = 1;
            cleanTreeView.SelectChanged += cleanTreeView_SelectChanged;
            cleanTreeView.AfterExpand += cleanTreeView_AfterExpand;
            cleanTreeView.CheckedChanged += cleanTreeView_CheckedChanged;
            cleanTreeView.NodeMouseClick += cleanTreeView_NodeMouseClick;
            cleanTreeView.MouseWheel += cleanTreeView_MouseWheel;
            // 
            // cleanStatusLabel
            // 
            cleanStatusLabel.AutoEllipsis = true;
            cleanStatusLabel.Dock = DockStyle.Fill;
            cleanStatusLabel.Location = new Point(0, 515);
            cleanStatusLabel.Margin = new Padding(0);
            cleanStatusLabel.Name = "cleanStatusLabel";
            cleanStatusLabel.Size = new Size(820, 34);
            cleanStatusLabel.TabIndex = 2;
            cleanStatusLabel.Text = "请选择目录并开始扫描";
            cleanStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cleanupActionSurface
            // 
            cleanupActionSurface.BackColor = Color.White;
            cleanupActionSurface.Controls.Add(cleanupActionLayout);
            cleanupActionSurface.Dock = DockStyle.Fill;
            cleanupActionSurface.Location = new Point(860, 10);
            cleanupActionSurface.Margin = new Padding(8, 10, 0, 0);
            cleanupActionSurface.Name = "cleanupActionSurface";
            cleanupActionSurface.Padding = new Padding(12);
            cleanupActionSurface.Size = new Size(336, 561);
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
            cleanupActionLayout.Size = new Size(312, 537);
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
            frequentRefreshButton.IconSvg = "ReloadOutlined";
            frequentRefreshButton.Location = new Point(390, 0);
            frequentRefreshButton.Name = "frequentRefreshButton";
            frequentRefreshButton.Padding = new Padding(8);
            frequentRefreshButton.Radius = 1;
            frequentRefreshButton.Size = new Size(34, 32);
            frequentRefreshButton.TabIndex = 0;
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
            cleanupMethodPanel.Controls.Add(cleanButton);
            cleanupMethodPanel.Controls.Add(cleanupMethodTitle);
            cleanupMethodPanel.Controls.Add(cleanRecycleRadio);
            cleanupMethodPanel.Controls.Add(cleanPermanentRadio);
            cleanupMethodPanel.Controls.Add(cleanMoveRadio);
            cleanupMethodPanel.Controls.Add(cleanCompressRadio);
            cleanupMethodPanel.Controls.Add(cleanMklinkRadio);
            cleanupMethodPanel.Controls.Add(cleanTargetLabel);
            cleanupMethodPanel.Controls.Add(cleanTargetTextBox);
            cleanupMethodPanel.Controls.Add(cleanTargetSelectBtn);
            cleanupMethodPanel.Dock = DockStyle.Fill;
            cleanupMethodPanel.Location = new Point(0, 216);
            cleanupMethodPanel.Margin = new Padding(0);
            cleanupMethodPanel.Name = "cleanupMethodPanel";
            cleanupMethodPanel.Size = new Size(312, 321);
            cleanupMethodPanel.TabIndex = 1;
            // 
            // cleanButton
            // 
            cleanButton.Location = new Point(4, 271);
            cleanButton.Name = "cleanButton";
            cleanButton.Radius = 3;
            cleanButton.Size = new Size(300, 42);
            cleanButton.TabIndex = 10;
            cleanButton.Text = "清理选中文件";
            cleanButton.Type = AntdUI.TTypeMini.Primary;
            cleanButton.Click += cleanBtn_Click;
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
            // rulesPanel
            // 
            rulesPanel.BackColor = Color.FromArgb(245, 247, 250);
            rulesPanel.Controls.Add(rulesExeProcessView);
            rulesPanel.Controls.Add(rulesToolbar);
            rulesPanel.Controls.Add(rulesSurface);
            rulesPanel.Location = new Point(18, 18);
            rulesPanel.Name = "rulesPanel";
            rulesPanel.Padding = new Padding(18);
            rulesPanel.Size = new Size(1196, 656);
            rulesPanel.TabIndex = 6;
            rulesPanel.Visible = false;
            // 
            // rulesExeProcessView
            // 
            rulesExeProcessView.BackColor = Color.White;
            rulesExeProcessView.Controls.Add(rulesExeProcViewTable);
            rulesExeProcessView.Controls.Add(rulesExeProcToolBar);
            rulesExeProcessView.Location = new Point(16, 75);
            rulesExeProcessView.Name = "rulesExeProcessView";
            rulesExeProcessView.Size = new Size(1160, 563);
            rulesExeProcessView.TabIndex = 2;
            // 
            // rulesExeProcViewTable
            // 
            rulesExeProcViewTable.Dock = DockStyle.Bottom;
            rulesExeProcViewTable.EmptyHeader = true;
            rulesExeProcViewTable.Gap = 12;
            rulesExeProcViewTable.Location = new Point(0, 49);
            rulesExeProcViewTable.Name = "rulesExeProcViewTable";
            rulesExeProcViewTable.Size = new Size(1160, 514);
            rulesExeProcViewTable.TabIndex = 1;
            rulesExeProcViewTable.Text = "table1";
            // 
            // rulesExeProcToolBar
            // 
            rulesExeProcToolBar.Controls.Add(input1);
            rulesExeProcToolBar.Controls.Add(rulesExeProcAddButton);
            rulesExeProcToolBar.Controls.Add(button1);
            rulesExeProcToolBar.Dock = DockStyle.Top;
            rulesExeProcToolBar.Location = new Point(0, 0);
            rulesExeProcToolBar.Name = "rulesExeProcToolBar";
            rulesExeProcToolBar.Size = new Size(1160, 48);
            rulesExeProcToolBar.TabIndex = 0;
            // 
            // input1
            // 
            input1.Location = new Point(3, 3);
            input1.Name = "input1";
            input1.PlaceholderText = "输入应用程序名称";
            input1.PrefixSvg = "SearchOutlined";
            input1.Size = new Size(240, 40);
            input1.TabIndex = 0;
            // 
            // rulesExeProcAddButton
            // 
            rulesExeProcAddButton.IconSvg = "PlusOutlined";
            rulesExeProcAddButton.Location = new Point(246, 4);
            rulesExeProcAddButton.Margin = new Padding(0, 4, 8, 0);
            rulesExeProcAddButton.Name = "rulesExeProcAddButton";
            rulesExeProcAddButton.Radius = 1;
            rulesExeProcAddButton.Size = new Size(84, 36);
            rulesExeProcAddButton.TabIndex = 1;
            rulesExeProcAddButton.Text = "添加";
            rulesExeProcAddButton.ToggleType = AntdUI.TTypeMini.Primary;
            rulesExeProcAddButton.Type = AntdUI.TTypeMini.Primary;
            // 
            // button1
            // 
            button1.IconSvg = "ControlOutlined";
            button1.Location = new Point(341, 3);
            button1.Name = "button1";
            button1.Size = new Size(139, 34);
            button1.TabIndex = 2;
            button1.Text = "选择运行进程";
            // 
            // rulesToolbar
            // 
            rulesToolbar.AutoScroll = true;
            rulesToolbar.Controls.Add(rulesDirectoryTab);
            rulesToolbar.Controls.Add(rulesProcessTab);
            rulesToolbar.Controls.Add(rulesExeTab);
            rulesToolbar.Dock = DockStyle.Top;
            rulesToolbar.Location = new Point(18, 18);
            rulesToolbar.Margin = new Padding(0);
            rulesToolbar.Name = "rulesToolbar";
            rulesToolbar.Padding = new Padding(0, 4, 0, 8);
            rulesToolbar.Size = new Size(1160, 54);
            rulesToolbar.TabIndex = 0;
            rulesToolbar.WrapContents = false;
            // 
            // rulesDirectoryTab
            // 
            rulesDirectoryTab.IconSvg = "FolderOpenOutlined";
            rulesDirectoryTab.Location = new Point(0, 8);
            rulesDirectoryTab.Margin = new Padding(0, 4, 8, 0);
            rulesDirectoryTab.Name = "rulesDirectoryTab";
            rulesDirectoryTab.Radius = 1;
            rulesDirectoryTab.Size = new Size(124, 36);
            rulesDirectoryTab.TabIndex = 0;
            rulesDirectoryTab.Text = "监控目录";
            rulesDirectoryTab.Type = AntdUI.TTypeMini.Primary;
            rulesDirectoryTab.Click += rulesDirectoryTab_Click;
            // 
            // rulesProcessTab
            // 
            rulesProcessTab.IconSvg = "ControlOutlined";
            rulesProcessTab.Location = new Point(132, 8);
            rulesProcessTab.Margin = new Padding(0, 4, 8, 0);
            rulesProcessTab.Name = "rulesProcessTab";
            rulesProcessTab.Radius = 1;
            rulesProcessTab.Size = new Size(113, 36);
            rulesProcessTab.TabIndex = 1;
            rulesProcessTab.Text = "忽略进程";
            rulesProcessTab.Click += rulesProcessTab_Click;
            // 
            // rulesExeTab
            // 
            rulesExeTab.IconSvg = "ControlOutlined";
            rulesExeTab.Location = new Point(256, 7);
            rulesExeTab.Name = "rulesExeTab";
            rulesExeTab.Radius = 1;
            rulesExeTab.Size = new Size(113, 36);
            rulesExeTab.TabIndex = 2;
            rulesExeTab.Text = "监控应用";
            rulesExeTab.Click += rulesExeTab_Click;
            // 
            // rulesSurface
            // 
            rulesSurface.BackColor = Color.White;
            rulesSurface.Controls.Add(rulesIgnoreProcessView);
            rulesSurface.Controls.Add(rulesDirectoryView);
            rulesSurface.Location = new Point(18, 18);
            rulesSurface.Margin = new Padding(0);
            rulesSurface.Name = "rulesSurface";
            rulesSurface.Padding = new Padding(2);
            rulesSurface.Size = new Size(1160, 620);
            rulesSurface.TabIndex = 1;
            // 
            // rulesIgnoreProcessView
            // 
            rulesIgnoreProcessView.BackColor = Color.White;
            rulesIgnoreProcessView.Controls.Add(ignoreProcessListView);
            rulesIgnoreProcessView.Controls.Add(rulesIgnoreProcToolbar);
            rulesIgnoreProcessView.Location = new Point(0, 57);
            rulesIgnoreProcessView.Margin = new Padding(0);
            rulesIgnoreProcessView.Name = "rulesIgnoreProcessView";
            rulesIgnoreProcessView.Size = new Size(1160, 577);
            rulesIgnoreProcessView.TabIndex = 1;
            rulesIgnoreProcessView.Visible = false;
            // 
            // ignoreProcessListView
            // 
            ignoreProcessListView.AllowDrop = true;
            ignoreProcessListView.Dock = DockStyle.Bottom;
            ignoreProcessListView.Location = new Point(0, 49);
            ignoreProcessListView.Margin = new Padding(0);
            ignoreProcessListView.Name = "ignoreProcessListView";
            ignoreProcessListView.Size = new Size(1160, 528);
            ignoreProcessListView.TabIndex = 1;
            ignoreProcessListView.UseCompatibleStateImageBehavior = false;
            ignoreProcessListView.ItemSelectionChanged += ignoreProcessView_ItemSelectionChanged;
            ignoreProcessListView.DragDrop += ignoreProcessView_DragDrop;
            ignoreProcessListView.DragEnter += ignoreProcessView_DragEnter;
            ignoreProcessListView.MouseClick += ignoreProcessView_MouseClick;
            ignoreProcessListView.Resize += ignoreProcessView_Resize;
            // 
            // rulesIgnoreProcToolbar
            // 
            rulesIgnoreProcToolbar.Controls.Add(manualProcessInput);
            rulesIgnoreProcToolbar.Controls.Add(rulesProcessAddButton);
            rulesIgnoreProcToolbar.Controls.Add(betterIngnoreProcAddButton);
            rulesIgnoreProcToolbar.Dock = DockStyle.Top;
            rulesIgnoreProcToolbar.Location = new Point(0, 0);
            rulesIgnoreProcToolbar.Margin = new Padding(0);
            rulesIgnoreProcToolbar.Name = "rulesIgnoreProcToolbar";
            rulesIgnoreProcToolbar.Padding = new Padding(0, 2, 0, 7);
            rulesIgnoreProcToolbar.Size = new Size(1160, 48);
            rulesIgnoreProcToolbar.TabIndex = 0;
            rulesIgnoreProcToolbar.WrapContents = false;
            // 
            // manualProcessInput
            // 
            manualProcessInput.Location = new Point(0, 6);
            manualProcessInput.Margin = new Padding(0, 4, 8, 0);
            manualProcessInput.Name = "manualProcessInput";
            manualProcessInput.PlaceholderText = "输入进程名";
            manualProcessInput.PrefixSvg = "SearchOutlined";
            manualProcessInput.Radius = 5;
            manualProcessInput.Size = new Size(240, 40);
            manualProcessInput.TabIndex = 0;
            // 
            // rulesProcessAddButton
            // 
            rulesProcessAddButton.IconSvg = "PlusOutlined";
            rulesProcessAddButton.Location = new Point(248, 6);
            rulesProcessAddButton.Margin = new Padding(0, 4, 8, 0);
            rulesProcessAddButton.Name = "rulesProcessAddButton";
            rulesProcessAddButton.Radius = 1;
            rulesProcessAddButton.Size = new Size(126, 36);
            rulesProcessAddButton.TabIndex = 1;
            rulesProcessAddButton.Text = "添加";
            rulesProcessAddButton.Type = AntdUI.TTypeMini.Primary;
            rulesProcessAddButton.Click += rulesProcessAddButton_Click;
            // 
            // betterIngnoreProcAddButton
            // 
            betterIngnoreProcAddButton.IconSvg = "ControlOutlined";
            betterIngnoreProcAddButton.Location = new Point(382, 6);
            betterIngnoreProcAddButton.Margin = new Padding(0, 4, 8, 0);
            betterIngnoreProcAddButton.Name = "betterIngnoreProcAddButton";
            betterIngnoreProcAddButton.Radius = 1;
            betterIngnoreProcAddButton.Size = new Size(166, 36);
            betterIngnoreProcAddButton.TabIndex = 2;
            betterIngnoreProcAddButton.Text = "选择运行进程";
            betterIngnoreProcAddButton.Click += betterProcessAddButton_Click;
            // 
            // rulesDirectoryView
            // 
            rulesDirectoryView.BackColor = Color.White;
            rulesDirectoryView.Controls.Add(rulesDirToolbar);
            rulesDirectoryView.Controls.Add(watcherDirListView);
            rulesDirectoryView.Dock = DockStyle.Bottom;
            rulesDirectoryView.Location = new Point(2, 57);
            rulesDirectoryView.Margin = new Padding(0);
            rulesDirectoryView.Name = "rulesDirectoryView";
            rulesDirectoryView.Size = new Size(1156, 561);
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
            rulesDirToolbar.Size = new Size(1156, 48);
            rulesDirToolbar.TabIndex = 0;
            rulesDirToolbar.WrapContents = false;
            // 
            // dirAddButton
            // 
            dirAddButton.IconSvg = "PlusOutlined";
            dirAddButton.Location = new Point(0, 6);
            dirAddButton.Margin = new Padding(0, 4, 8, 0);
            dirAddButton.Name = "dirAddButton";
            dirAddButton.Radius = 1;
            dirAddButton.Size = new Size(110, 36);
            dirAddButton.TabIndex = 0;
            dirAddButton.Text = "添加目录";
            dirAddButton.Type = AntdUI.TTypeMini.Primary;
            dirAddButton.Click += dirAddButton_Click;
            // 
            // betterDirAddButton
            // 
            betterDirAddButton.IconSvg = "FolderOpenOutlined";
            betterDirAddButton.Location = new Point(118, 6);
            betterDirAddButton.Margin = new Padding(0, 4, 8, 0);
            betterDirAddButton.Name = "betterDirAddButton";
            betterDirAddButton.Radius = 1;
            betterDirAddButton.Size = new Size(110, 36);
            betterDirAddButton.TabIndex = 1;
            betterDirAddButton.Text = "批量选择";
            betterDirAddButton.Click += betterDirAddButton_Click;
            // 
            // watcherDirListView
            // 
            watcherDirListView.Dock = DockStyle.Bottom;
            watcherDirListView.Location = new Point(0, 48);
            watcherDirListView.Margin = new Padding(0);
            watcherDirListView.Name = "watcherDirListView";
            watcherDirListView.Size = new Size(1156, 513);
            watcherDirListView.TabIndex = 1;
            watcherDirListView.UseCompatibleStateImageBehavior = false;
            watcherDirListView.ItemSelectionChanged += watcherDirListView_ItemSelectionChanged;
            watcherDirListView.MouseClick += watcherDirListView_MouseClick;
            watcherDirListView.Resize += watcherDirListView_Resize;
            // 
            // activityPanel
            // 
            activityPanel.BackColor = Color.FromArgb(245, 247, 250);
            activityPanel.Controls.Add(activityToolbar);
            activityPanel.Controls.Add(activitySurface);
            activityPanel.Dock = DockStyle.Fill;
            activityPanel.Location = new Point(0, 0);
            activityPanel.Name = "activityPanel";
            activityPanel.Padding = new Padding(18);
            activityPanel.Size = new Size(1232, 692);
            activityPanel.TabIndex = 3;
            activityPanel.Visible = false;
            // 
            // activityToolbar
            // 
            activityToolbar.AutoScroll = true;
            activityToolbar.Controls.Add(panel2);
            activityToolbar.Controls.Add(panel1);
            activityToolbar.Dock = DockStyle.Top;
            activityToolbar.FlowDirection = FlowDirection.TopDown;
            activityToolbar.Location = new Point(18, 18);
            activityToolbar.Margin = new Padding(0);
            activityToolbar.Name = "activityToolbar";
            activityToolbar.Padding = new Padding(0, 4, 0, 8);
            activityToolbar.Size = new Size(1196, 113);
            activityToolbar.TabIndex = 0;
            activityToolbar.WrapContents = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(workspaceMonitorToggleButton);
            panel2.Controls.Add(clearBtn);
            panel2.Controls.Add(activityRecordCenterButton);
            panel2.Controls.Add(typeFilterCombo);
            panel2.Controls.Add(exportBtn);
            panel2.Controls.Add(recordSearchBox);
            panel2.Location = new Point(3, 7);
            panel2.Name = "panel2";
            panel2.Size = new Size(1158, 45);
            panel2.TabIndex = 7;
            panel2.Text = "panel2";
            // 
            // workspaceMonitorToggleButton
            // 
            workspaceMonitorToggleButton.IconSvg = "PlayCircleOutlined";
            workspaceMonitorToggleButton.Location = new Point(5, -1);
            workspaceMonitorToggleButton.Margin = new Padding(0, 4, 8, 0);
            workspaceMonitorToggleButton.Name = "workspaceMonitorToggleButton";
            workspaceMonitorToggleButton.Radius = 1;
            workspaceMonitorToggleButton.Size = new Size(121, 36);
            workspaceMonitorToggleButton.TabIndex = 0;
            workspaceMonitorToggleButton.Text = "开始监测";
            workspaceMonitorToggleButton.Type = AntdUI.TTypeMini.Primary;
            workspaceMonitorToggleButton.Click += pauseBtn_Click;
            // 
            // clearBtn
            // 
            clearBtn.IconSvg = "ClearOutlined";
            clearBtn.Location = new Point(674, 1);
            clearBtn.Margin = new Padding(0, 4, 8, 0);
            clearBtn.Name = "clearBtn";
            clearBtn.Radius = 1;
            clearBtn.Size = new Size(97, 36);
            clearBtn.TabIndex = 4;
            clearBtn.Text = "清空";
            clearBtn.Type = AntdUI.TTypeMini.Error;
            clearBtn.Click += clearBtn_Click;
            // 
            // activityRecordCenterButton
            // 
            activityRecordCenterButton.IconSvg = "HistoryOutlined";
            activityRecordCenterButton.Location = new Point(788, 3);
            activityRecordCenterButton.Margin = new Padding(0, 4, 8, 0);
            activityRecordCenterButton.Name = "activityRecordCenterButton";
            activityRecordCenterButton.Radius = 1;
            activityRecordCenterButton.Size = new Size(100, 36);
            activityRecordCenterButton.TabIndex = 5;
            activityRecordCenterButton.Text = "记录中心";
            activityRecordCenterButton.Click += activityRecordCenterButton_Click;
            // 
            // typeFilterCombo
            // 
            typeFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            typeFilterCombo.FormattingEnabled = true;
            typeFilterCombo.Items.AddRange(new object[] { "全部", "创建", "修改", "删除", "重命名" });
            typeFilterCombo.Location = new Point(147, 3);
            typeFilterCombo.Margin = new Padding(8, 6, 0, 0);
            typeFilterCombo.Name = "typeFilterCombo";
            typeFilterCombo.Size = new Size(126, 32);
            typeFilterCombo.TabIndex = 1;
            typeFilterCombo.SelectedIndexChanged += typeFilterCombo_SelectedIndexChanged;
            // 
            // exportBtn
            // 
            exportBtn.IconSvg = "ExportOutlined";
            exportBtn.Location = new Point(572, 2);
            exportBtn.Margin = new Padding(12, 4, 8, 0);
            exportBtn.Name = "exportBtn";
            exportBtn.Radius = 1;
            exportBtn.Size = new Size(94, 36);
            exportBtn.TabIndex = 3;
            exportBtn.Text = "导出";
            exportBtn.Click += exportBtn_Click;
            // 
            // recordSearchBox
            // 
            recordSearchBox.Location = new Point(287, 2);
            recordSearchBox.Margin = new Padding(10, 4, 0, 0);
            recordSearchBox.Name = "recordSearchBox";
            recordSearchBox.PlaceholderText = "搜索文件、路径或来源进程";
            recordSearchBox.PrefixSvg = "SearchOutlined";
            recordSearchBox.Radius = 5;
            recordSearchBox.Size = new Size(282, 36);
            recordSearchBox.TabIndex = 2;
            recordSearchBox.TextChanged += recordSearchBox_TextChanged;
            recordSearchBox.KeyDown += recordSearchBox_KeyDown;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(exeModeRadio);
            panel1.Controls.Add(defaultModeRadio);
            panel1.Location = new Point(3, 58);
            panel1.Name = "panel1";
            panel1.Size = new Size(1158, 41);
            panel1.TabIndex = 6;
            panel1.Text = "panel1";
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(13, 3);
            label1.Name = "label1";
            label1.Size = new Size(105, 34);
            label1.TabIndex = 2;
            label1.Text = "监测模式";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // exeModeRadio
            // 
            exeModeRadio.BackColor = Color.Transparent;
            exeModeRadio.Location = new Point(240, 3);
            exeModeRadio.Name = "exeModeRadio";
            exeModeRadio.Size = new Size(149, 34);
            exeModeRadio.TabIndex = 1;
            exeModeRadio.Text = "应用程序";
            exeModeRadio.CheckedChanged += exeModeRadio_CheckedChanged;
            // 
            // defaultModeRadio
            // 
            defaultModeRadio.BackColor = Color.Transparent;
            defaultModeRadio.Checked = true;
            defaultModeRadio.Location = new Point(124, 3);
            defaultModeRadio.Name = "defaultModeRadio";
            defaultModeRadio.Size = new Size(93, 34);
            defaultModeRadio.TabIndex = 0;
            defaultModeRadio.Text = "默认";
            // 
            // activitySurface
            // 
            activitySurface.BackColor = Color.White;
            activitySurface.Controls.Add(activityRecordTable);
            activitySurface.Dock = DockStyle.Fill;
            activitySurface.Location = new Point(18, 18);
            activitySurface.Margin = new Padding(0);
            activitySurface.Name = "activitySurface";
            activitySurface.Padding = new Padding(12);
            activitySurface.Size = new Size(1196, 656);
            activitySurface.TabIndex = 1;
            // 
            // activityRecordTable
            // 
            activityRecordTable.ColumnBack = SystemColors.ActiveBorder;
            activityRecordTable.ColumnFont = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            activityRecordTable.ColumnFore = Color.Aqua;
            activityRecordTable.Dock = DockStyle.Bottom;
            activityRecordTable.EmptyHeader = true;
            activityRecordTable.EmptyText = "-";
            activityRecordTable.Gap = 12;
            activityRecordTable.Location = new Point(12, 117);
            activityRecordTable.Name = "activityRecordTable";
            activityRecordTable.Size = new Size(1172, 527);
            activityRecordTable.TabIndex = 1;
            // 
            // recordsPanel
            // 
            recordsPanel.BackColor = Color.FromArgb(245, 247, 250);
            recordsPanel.Controls.Add(recordsToolbar);
            recordsPanel.Controls.Add(recordsSurface);
            recordsPanel.Dock = DockStyle.Fill;
            recordsPanel.Location = new Point(0, 0);
            recordsPanel.Name = "recordsPanel";
            recordsPanel.Padding = new Padding(18);
            recordsPanel.Size = new Size(1232, 692);
            recordsPanel.TabIndex = 9;
            recordsPanel.Visible = false;
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
            recordsToolbar.Location = new Point(18, 18);
            recordsToolbar.Margin = new Padding(0);
            recordsToolbar.Name = "recordsToolbar";
            recordsToolbar.Padding = new Padding(0, 4, 0, 8);
            recordsToolbar.Size = new Size(1196, 54);
            recordsToolbar.TabIndex = 0;
            recordsToolbar.WrapContents = false;
            // 
            // recordsNotificationTab
            // 
            recordsNotificationTab.IconSvg = "MonitorOutlined";
            recordsNotificationTab.Location = new Point(0, 8);
            recordsNotificationTab.Margin = new Padding(0, 4, 8, 0);
            recordsNotificationTab.Name = "recordsNotificationTab";
            recordsNotificationTab.Radius = 1;
            recordsNotificationTab.Size = new Size(123, 36);
            recordsNotificationTab.TabIndex = 0;
            recordsNotificationTab.Text = "提醒记录";
            recordsNotificationTab.Type = AntdUI.TTypeMini.Primary;
            recordsNotificationTab.Click += recordsNotificationTab_Click;
            // 
            // recordsStatsTab
            // 
            recordsStatsTab.IconSvg = "LineChartOutlined";
            recordsStatsTab.Location = new Point(131, 8);
            recordsStatsTab.Margin = new Padding(0, 4, 8, 0);
            recordsStatsTab.Name = "recordsStatsTab";
            recordsStatsTab.Radius = 1;
            recordsStatsTab.Size = new Size(100, 36);
            recordsStatsTab.TabIndex = 1;
            recordsStatsTab.Text = "进程统计";
            recordsStatsTab.Click += recordsStatsTab_Click;
            // 
            // recordsDetailsTab
            // 
            recordsDetailsTab.IconSvg = "DatabaseOutlined";
            recordsDetailsTab.Location = new Point(239, 8);
            recordsDetailsTab.Margin = new Padding(0, 4, 8, 0);
            recordsDetailsTab.Name = "recordsDetailsTab";
            recordsDetailsTab.Radius = 1;
            recordsDetailsTab.Size = new Size(100, 36);
            recordsDetailsTab.TabIndex = 2;
            recordsDetailsTab.Text = "变更明细";
            recordsDetailsTab.Click += recordsDetailsTab_Click;
            // 
            // recordsCleanupTab
            // 
            recordsCleanupTab.IconSvg = "HistoryOutlined";
            recordsCleanupTab.Location = new Point(347, 8);
            recordsCleanupTab.Margin = new Padding(0, 4, 8, 0);
            recordsCleanupTab.Name = "recordsCleanupTab";
            recordsCleanupTab.Radius = 1;
            recordsCleanupTab.Size = new Size(100, 36);
            recordsCleanupTab.TabIndex = 3;
            recordsCleanupTab.Text = "清理历史";
            recordsCleanupTab.Click += recordsCleanupTab_Click;
            // 
            // recordsRefreshButton
            // 
            recordsRefreshButton.IconSvg = "ReloadOutlined";
            recordsRefreshButton.Location = new Point(455, 8);
            recordsRefreshButton.Margin = new Padding(0, 4, 8, 0);
            recordsRefreshButton.Name = "recordsRefreshButton";
            recordsRefreshButton.Radius = 1;
            recordsRefreshButton.Size = new Size(84, 36);
            recordsRefreshButton.TabIndex = 4;
            recordsRefreshButton.Text = "刷新";
            recordsRefreshButton.Click += recordsRefreshButton_Click;
            // 
            // recordsSurface
            // 
            recordsSurface.BackColor = Color.White;
            recordsSurface.Controls.Add(recordViewHost);
            recordsSurface.Dock = DockStyle.Bottom;
            recordsSurface.Location = new Point(18, 77);
            recordsSurface.Margin = new Padding(0);
            recordsSurface.Name = "recordsSurface";
            recordsSurface.Padding = new Padding(12);
            recordsSurface.Size = new Size(1196, 597);
            recordsSurface.TabIndex = 1;
            // 
            // recordViewHost
            // 
            recordViewHost.BackColor = Color.White;
            recordViewHost.Controls.Add(cleanupRecordView);
            recordViewHost.Controls.Add(detailRecordsTable);
            recordViewHost.Controls.Add(processStatsTable);
            recordViewHost.Controls.Add(notificationRecordsTable);
            recordViewHost.Dock = DockStyle.Fill;
            recordViewHost.Location = new Point(12, 12);
            recordViewHost.Margin = new Padding(0);
            recordViewHost.Name = "recordViewHost";
            recordViewHost.Size = new Size(1172, 573);
            recordViewHost.TabIndex = 0;
            // 
            // cleanupRecordView
            // 
            cleanupRecordView.BackColor = Color.White;
            cleanupRecordView.Controls.Add(cleanHistoryTable);
            cleanupRecordView.Controls.Add(cleanHistoryEmptyLabel);
            cleanupRecordView.Location = new Point(0, 67);
            cleanupRecordView.Margin = new Padding(0);
            cleanupRecordView.Name = "cleanupRecordView";
            cleanupRecordView.Size = new Size(1172, 565);
            cleanupRecordView.TabIndex = 3;
            cleanupRecordView.Visible = false;
            // 
            // cleanHistoryTable
            // 
            cleanHistoryTable.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            cleanHistoryTable.Dock = DockStyle.Bottom;
            cleanHistoryTable.EmptyText = "暂无清理记录";
            cleanHistoryTable.Gap = 12;
            cleanHistoryTable.Location = new Point(0, -69);
            cleanHistoryTable.Margin = new Padding(0);
            cleanHistoryTable.Name = "cleanHistoryTable";
            cleanHistoryTable.Radius = 6;
            cleanHistoryTable.Size = new Size(1172, 634);
            cleanHistoryTable.TabIndex = 0;
            cleanHistoryTable.CellClick += cleanHistoryGrid_CellClick;
            // 
            // cleanHistoryEmptyLabel
            // 
            cleanHistoryEmptyLabel.BackColor = Color.White;
            cleanHistoryEmptyLabel.Dock = DockStyle.Fill;
            cleanHistoryEmptyLabel.ForeColor = Color.Gray;
            cleanHistoryEmptyLabel.Location = new Point(0, 0);
            cleanHistoryEmptyLabel.Name = "cleanHistoryEmptyLabel";
            cleanHistoryEmptyLabel.Size = new Size(1172, 565);
            cleanHistoryEmptyLabel.TabIndex = 1;
            cleanHistoryEmptyLabel.Text = "暂无清理记录";
            cleanHistoryEmptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            cleanHistoryEmptyLabel.Visible = false;
            // 
            // detailRecordsTable
            // 
            detailRecordsTable.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            detailRecordsTable.Dock = DockStyle.Fill;
            detailRecordsTable.EmptyText = "暂无变更记录";
            detailRecordsTable.Gap = 12;
            detailRecordsTable.Location = new Point(0, 0);
            detailRecordsTable.Margin = new Padding(0);
            detailRecordsTable.Name = "detailRecordsTable";
            detailRecordsTable.Radius = 6;
            detailRecordsTable.Size = new Size(1172, 573);
            detailRecordsTable.TabIndex = 2;
            detailRecordsTable.Visible = false;
            // 
            // processStatsTable
            // 
            processStatsTable.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            processStatsTable.Dock = DockStyle.Fill;
            processStatsTable.EmptyText = "暂无统计记录";
            processStatsTable.Gap = 12;
            processStatsTable.Location = new Point(0, 0);
            processStatsTable.Margin = new Padding(0);
            processStatsTable.Name = "processStatsTable";
            processStatsTable.Radius = 6;
            processStatsTable.Size = new Size(1172, 573);
            processStatsTable.TabIndex = 1;
            processStatsTable.Visible = false;
            // 
            // notificationRecordsTable
            // 
            notificationRecordsTable.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            notificationRecordsTable.Dock = DockStyle.Fill;
            notificationRecordsTable.EmptyText = "暂无提醒记录";
            notificationRecordsTable.Gap = 12;
            notificationRecordsTable.Location = new Point(0, 0);
            notificationRecordsTable.Margin = new Padding(0);
            notificationRecordsTable.Name = "notificationRecordsTable";
            notificationRecordsTable.Radius = 6;
            notificationRecordsTable.Size = new Size(1172, 573);
            notificationRecordsTable.TabIndex = 0;
            // 
            // dashboardPanel
            // 
            dashboardPanel.BackColor = Color.FromArgb(245, 247, 250);
            dashboardPanel.Controls.Add(dashboardLayout);
            dashboardPanel.Dock = DockStyle.Fill;
            dashboardPanel.Location = new Point(0, 0);
            dashboardPanel.Name = "dashboardPanel";
            dashboardPanel.Padding = new Padding(18);
            dashboardPanel.Size = new Size(1232, 692);
            dashboardPanel.TabIndex = 0;
            // 
            // dashboardLayout
            // 
            dashboardLayout.ColumnCount = 1;
            dashboardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            dashboardLayout.Controls.Add(dashboardCapacitySurface, 0, 0);
            dashboardLayout.Controls.Add(dashboardMetrics, 0, 1);
            dashboardLayout.Controls.Add(dashboardRecentSurface, 0, 2);
            dashboardLayout.Dock = DockStyle.Fill;
            dashboardLayout.Location = new Point(18, 18);
            dashboardLayout.Margin = new Padding(0);
            dashboardLayout.Name = "dashboardLayout";
            dashboardLayout.RowCount = 3;
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 188F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            dashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            dashboardLayout.Size = new Size(1196, 656);
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
            dashboardCapacitySurface.Size = new Size(1196, 188);
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
            dashboardUsageLabel.Size = new Size(1160, 42);
            dashboardUsageLabel.TabIndex = 1;
            dashboardUsageLabel.Text = "正在读取磁盘信息...";
            // 
            // dashboardDiskProgress
            // 
            dashboardDiskProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dashboardDiskProgress.Location = new Point(24, 98);
            dashboardDiskProgress.Name = "dashboardDiskProgress";
            dashboardDiskProgress.Radius = 4;
            dashboardDiskProgress.ShowTextDot = 1;
            dashboardDiskProgress.Size = new Size(1168, 18);
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
            dashboardCapacityLabel.Size = new Size(1172, 28);
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
            dashboardMetrics.Size = new Size(1196, 80);
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
            dashboardMonitorSurface.Size = new Size(392, 80);
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
            dashboardMonitorTitle.Size = new Size(392, 25);
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
            dashboardMonitorMetric.Size = new Size(392, 80);
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
            dashboardRecordSurface.Location = new Point(404, 0);
            dashboardRecordSurface.Margin = new Padding(6, 0, 6, 0);
            dashboardRecordSurface.Name = "dashboardRecordSurface";
            dashboardRecordSurface.Size = new Size(386, 80);
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
            dashboardRecordTitle.Size = new Size(386, 25);
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
            dashboardRecordMetric.Size = new Size(386, 80);
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
            dashboardRuleSurface.Location = new Point(802, 0);
            dashboardRuleSurface.Margin = new Padding(6, 0, 0, 0);
            dashboardRuleSurface.Name = "dashboardRuleSurface";
            dashboardRuleSurface.Size = new Size(394, 80);
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
            dashboardRuleTitle.Size = new Size(394, 25);
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
            dashboardRuleMetric.Size = new Size(394, 80);
            dashboardRuleMetric.TabIndex = 1;
            dashboardRuleMetric.Text = "-";
            dashboardRuleMetric.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecentSurface
            // 
            dashboardRecentSurface.BackColor = Color.White;
            dashboardRecentSurface.Controls.Add(dashboardRecentTitle);
            dashboardRecentSurface.Controls.Add(dashboardRecentTable);
            dashboardRecentSurface.Dock = DockStyle.Fill;
            dashboardRecentSurface.Location = new Point(0, 278);
            dashboardRecentSurface.Margin = new Padding(0);
            dashboardRecentSurface.Name = "dashboardRecentSurface";
            dashboardRecentSurface.Size = new Size(1196, 378);
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
            dashboardRecentTitle.Size = new Size(1196, 34);
            dashboardRecentTitle.TabIndex = 0;
            dashboardRecentTitle.Text = "最近活动";
            dashboardRecentTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardRecentTable
            // 
            dashboardRecentTable.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            dashboardRecentTable.Dock = DockStyle.Bottom;
            dashboardRecentTable.EmptyHeader = true;
            dashboardRecentTable.EmptyText = "暂无活动记录";
            dashboardRecentTable.Gap = 12;
            dashboardRecentTable.Location = new Point(0, 34);
            dashboardRecentTable.Margin = new Padding(16, 0, 16, 0);
            dashboardRecentTable.Name = "dashboardRecentTable";
            dashboardRecentTable.Radius = 6;
            dashboardRecentTable.Size = new Size(1196, 344);
            dashboardRecentTable.TabIndex = 1;
            // 
            // analyzerPanel
            // 
            analyzerPanel.BackColor = Color.FromArgb(245, 247, 250);
            analyzerPanel.Controls.Add(analyzerToolbar);
            analyzerPanel.Controls.Add(scanProgressBar);
            analyzerPanel.Controls.Add(analyzerContent);
            analyzerPanel.Dock = DockStyle.Fill;
            analyzerPanel.Location = new Point(0, 0);
            analyzerPanel.Name = "analyzerPanel";
            analyzerPanel.Padding = new Padding(18);
            analyzerPanel.Size = new Size(1232, 692);
            analyzerPanel.TabIndex = 7;
            analyzerPanel.Visible = false;
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
            analyzerToolbar.Location = new Point(18, 41);
            analyzerToolbar.Margin = new Padding(0);
            analyzerToolbar.Name = "analyzerToolbar";
            analyzerToolbar.RowCount = 1;
            analyzerToolbar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            analyzerToolbar.Size = new Size(1196, 52);
            analyzerToolbar.TabIndex = 0;
            // 
            // selectedPathTextBox
            // 
            selectedPathTextBox.Dock = DockStyle.Fill;
            selectedPathTextBox.Location = new Point(0, 7);
            selectedPathTextBox.Margin = new Padding(0, 7, 8, 7);
            selectedPathTextBox.Name = "selectedPathTextBox";
            selectedPathTextBox.ReadOnly = true;
            selectedPathTextBox.Size = new Size(880, 30);
            selectedPathTextBox.TabIndex = 0;
            // 
            // selectDirBtn
            // 
            selectDirBtn.Dock = DockStyle.Fill;
            selectDirBtn.FlatStyle = FlatStyle.Flat;
            selectDirBtn.Location = new Point(892, 6);
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
            scanBtn.Location = new Point(1000, 6);
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
            stopBtn.Location = new Point(1108, 6);
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
            scanProgressBar.Location = new Point(18, 18);
            scanProgressBar.Margin = new Padding(0, 5, 0, 5);
            scanProgressBar.Name = "scanProgressBar";
            scanProgressBar.Size = new Size(1196, 23);
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
            analyzerContent.Dock = DockStyle.Bottom;
            analyzerContent.Location = new Point(18, 93);
            analyzerContent.Margin = new Padding(0);
            analyzerContent.Name = "analyzerContent";
            analyzerContent.RowCount = 1;
            analyzerContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            analyzerContent.Size = new Size(1196, 581);
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
            analyzerTreeSurface.Size = new Size(829, 571);
            analyzerTreeSurface.TabIndex = 0;
            // 
            // folderTreeView
            // 
            folderTreeView.Dock = DockStyle.Bottom;
            folderTreeView.Location = new Point(12, 0);
            folderTreeView.Margin = new Padding(0);
            folderTreeView.Name = "folderTreeView";
            folderTreeView.Size = new Size(805, 559);
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
            analyzerDetailsSurface.Location = new Point(845, 10);
            analyzerDetailsSurface.Margin = new Padding(8, 10, 0, 0);
            analyzerDetailsSurface.Name = "analyzerDetailsSurface";
            analyzerDetailsSurface.Padding = new Padding(18);
            analyzerDetailsSurface.Size = new Size(351, 571);
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
            analyzerPathValue.Location = new Point(36, 76);
            analyzerPathValue.Name = "analyzerPathValue";
            analyzerPathValue.Size = new Size(980, 64);
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
            analyzerSizeValue.Location = new Point(36, 154);
            analyzerSizeValue.Name = "analyzerSizeValue";
            analyzerSizeValue.Size = new Size(980, 28);
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
            analyzerFilesValue.Location = new Point(36, 192);
            analyzerFilesValue.Name = "analyzerFilesValue";
            analyzerFilesValue.Size = new Size(980, 28);
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
            analyzerFoldersValue.Location = new Point(36, 230);
            analyzerFoldersValue.Name = "analyzerFoldersValue";
            analyzerFoldersValue.Size = new Size(980, 28);
            analyzerFoldersValue.TabIndex = 4;
            analyzerFoldersValue.Text = "子目录：-";
            // 
            // analyzerUseForCleanupButton
            // 
            analyzerUseForCleanupButton.IconSvg = "DeleteOutlined";
            analyzerUseForCleanupButton.Location = new Point(23, 292);
            analyzerUseForCleanupButton.Name = "analyzerUseForCleanupButton";
            analyzerUseForCleanupButton.Radius = 1;
            analyzerUseForCleanupButton.Size = new Size(160, 38);
            analyzerUseForCleanupButton.TabIndex = 5;
            analyzerUseForCleanupButton.Text = "作为清理来源";
            analyzerUseForCleanupButton.Type = AntdUI.TTypeMini.Primary;
            analyzerUseForCleanupButton.Click += analyzerUseForCleanupButton_Click;
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
            workspaceStatusBar.Location = new Point(0, 830);
            workspaceStatusBar.Margin = new Padding(0);
            workspaceStatusBar.Name = "workspaceStatusBar";
            workspaceStatusBar.Padding = new Padding(10, 0, 10, 0);
            workspaceStatusBar.RowCount = 1;
            workspaceStatusBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            workspaceStatusBar.Size = new Size(1440, 30);
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
            workspaceDiskStatus.Size = new Size(482, 30);
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
            workspaceMonitorStatus.Location = new Point(492, 0);
            workspaceMonitorStatus.Margin = new Padding(0);
            workspaceMonitorStatus.Name = "workspaceMonitorStatus";
            workspaceMonitorStatus.Size = new Size(340, 30);
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
            workspaceRecordStatus.Location = new Point(832, 0);
            workspaceRecordStatus.Margin = new Padding(0);
            workspaceRecordStatus.Name = "workspaceRecordStatus";
            workspaceRecordStatus.Size = new Size(312, 30);
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
            workspaceClockStatus.Location = new Point(1144, 0);
            workspaceClockStatus.Margin = new Padding(0);
            workspaceClockStatus.Name = "workspaceClockStatus";
            workspaceClockStatus.Size = new Size(286, 30);
            workspaceClockStatus.TabIndex = 3;
            workspaceClockStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // notifyRotateTimer
            // 
            notifyRotateTimer.Interval = 300;
            notifyRotateTimer.Tick += notifyRotateTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderColor = Color.Cyan;
            ClientSize = new Size(1440, 860);
            Controls.Add(workspaceRoot);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1180, 720);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C盘监测工具";
            FormClosing += WorkspaceFormClosing;
            Load += Form1_Load;
            notifyMenuStrip.ResumeLayout(false);
            workspaceRoot.ResumeLayout(false);
            workspaceBodyLayout.ResumeLayout(false);
            workspaceNavigation.ResumeLayout(false);
            workspaceMain.ResumeLayout(false);
            workspacePageHeader.ResumeLayout(false);
            workspacePageContainer.ResumeLayout(false);
            cleanupPanel.ResumeLayout(false);
            cleanupToolbar.ResumeLayout(false);
            cleanupToolbar.PerformLayout();
            cleanupContent.ResumeLayout(false);
            cleanupTreeSurface.ResumeLayout(false);
            cleanupTreeLayout.ResumeLayout(false);
            cleanupSelectionBarRow.ResumeLayout(false);
            cleanupSelectionBar.ResumeLayout(false);
            riskLegendBar.ResumeLayout(false);
            riskLegendBar.PerformLayout();
            cleanupActionSurface.ResumeLayout(false);
            cleanupActionLayout.ResumeLayout(false);
            cleanupFrequentPanel.ResumeLayout(false);
            cleanupMethodPanel.ResumeLayout(false);
            cleanupMethodPanel.PerformLayout();
            rulesPanel.ResumeLayout(false);
            rulesExeProcessView.ResumeLayout(false);
            rulesExeProcToolBar.ResumeLayout(false);
            rulesToolbar.ResumeLayout(false);
            rulesSurface.ResumeLayout(false);
            rulesIgnoreProcessView.ResumeLayout(false);
            rulesIgnoreProcToolbar.ResumeLayout(false);
            rulesDirectoryView.ResumeLayout(false);
            rulesDirToolbar.ResumeLayout(false);
            activityPanel.ResumeLayout(false);
            activityToolbar.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            activitySurface.ResumeLayout(false);
            recordsPanel.ResumeLayout(false);
            recordsToolbar.ResumeLayout(false);
            recordsSurface.ResumeLayout(false);
            recordViewHost.ResumeLayout(false);
            cleanupRecordView.ResumeLayout(false);
            dashboardPanel.ResumeLayout(false);
            dashboardLayout.ResumeLayout(false);
            dashboardCapacitySurface.ResumeLayout(false);
            dashboardCapacitySurface.PerformLayout();
            dashboardMetrics.ResumeLayout(false);
            dashboardMonitorSurface.ResumeLayout(false);
            dashboardRecordSurface.ResumeLayout(false);
            dashboardRuleSurface.ResumeLayout(false);
            dashboardRecentSurface.ResumeLayout(false);
            analyzerPanel.ResumeLayout(false);
            analyzerToolbar.ResumeLayout(false);
            analyzerToolbar.PerformLayout();
            analyzerContent.ResumeLayout(false);
            analyzerTreeSurface.ResumeLayout(false);
            analyzerDetailsSurface.ResumeLayout(false);
            workspaceStatusBar.ResumeLayout(false);
            ResumeLayout(false);
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
        private AntdUI.PageHeader workspaceHeader;
        private TableLayoutPanel workspaceBodyLayout;
        private AntdUI.Panel workspaceNavigation;
        private Label brandLabel;
        private AntdUI.Menu workspaceMenu;
        private AntdUI.Button workspaceCollapseButton;
        private TableLayoutPanel workspaceMain;
        private Panel workspacePageHeader;
        private Label workspacePageTitle;
        private Label workspacePageSubtitle;
        private Panel workspacePageContainer;
        private Panel dashboardPanel;
        private TableLayoutPanel workspaceStatusBar;
        private Label workspaceDiskStatus;
        private Label workspaceMonitorStatus;
        private Label workspaceRecordStatus;
        private Label workspaceClockStatus;

        // 工作台页
        private TableLayoutPanel dashboardLayout;
        private AntdUI.Panel dashboardCapacitySurface;
        private Label dashboardTitleLabel;
        private Label dashboardUsageLabel;
        private AntdUI.Progress dashboardDiskProgress;
        private Label dashboardCapacityLabel;
        private TableLayoutPanel dashboardMetrics;
        private AntdUI.Panel dashboardMonitorSurface;
        private Label dashboardMonitorTitle;
        private Label dashboardMonitorMetric;
        private AntdUI.Panel dashboardRecordSurface;
        private Label dashboardRecordTitle;
        private Label dashboardRecordMetric;
        private AntdUI.Panel dashboardRuleSurface;
        private Label dashboardRuleTitle;
        private Label dashboardRuleMetric;
        private AntdUI.Panel dashboardRecentSurface;
        private Label dashboardRecentTitle;
        private AntdUI.Table dashboardRecentTable;
        private Panel activityPanel;
        private FlowLayoutPanel activityToolbar;
        private AntdUI.Button workspaceMonitorToggleButton;
        private ComboBox typeFilterCombo;
        private AntdUI.Input recordSearchBox;
        private AntdUI.Button exportBtn;
        private AntdUI.Button clearBtn;
        private AntdUI.Button activityRecordCenterButton;
        private AntdUI.Panel activitySurface;
        private Panel rulesPanel;
        private FlowLayoutPanel rulesToolbar;
        private AntdUI.Button rulesDirectoryTab;
        private AntdUI.Button rulesProcessTab;
        private AntdUI.Panel rulesSurface;
        private Panel rulesDirectoryView;
        private FlowLayoutPanel rulesDirToolbar;
        private AntdUI.Button dirAddButton;
        private AntdUI.Button betterDirAddButton;
        private ListView watcherDirListView;
        private Panel rulesIgnoreProcessView;
        private FlowLayoutPanel rulesIgnoreProcToolbar;
        private AntdUI.Input manualProcessInput;
        private AntdUI.Button rulesProcessAddButton;
        private AntdUI.Button betterIngnoreProcAddButton;
        private ListView ignoreProcessListView;
        private Panel cleanupPanel;
        private TableLayoutPanel cleanupToolbar;
        private TextBox cleanPathTextBox;
        private Button cleanSelectDirBtn;
        private Button cleanScanBtn;
        private ProgressBar cleanScanProgressBar;
        private TableLayoutPanel cleanupContent;
        private AntdUI.Panel cleanupTreeSurface;
        private TableLayoutPanel cleanupTreeLayout;
        private FlowLayoutPanel cleanupSelectionBar;
        private Panel cleanupSelectionBarRow;
        private FlowLayoutPanel riskLegendBar;
        private Panel riskHighChip;
        private Label riskHighLabel;
        private Panel riskMediumChip;
        private Label riskMediumLabel;
        private Panel riskLowChip;
        private Label riskLowLabel;
        private Button cleanSelectAllBtn;
        private Button cleanSelectNoneBtn;
        private Label cleanupSelectionLabel;
        private AntdUI.Tree cleanTreeView;
        private Label cleanStatusLabel;
        private AntdUI.Panel cleanupActionSurface;
        private TableLayoutPanel cleanupActionLayout;
        private Panel cleanupFrequentPanel;
        private AntdUI.Button frequentRefreshButton;
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
        private Panel analyzerPanel;
        private TableLayoutPanel analyzerToolbar;
        private TextBox selectedPathTextBox;
        private Button selectDirBtn;
        private Button scanBtn;
        private Button stopBtn;
        private ProgressBar scanProgressBar;
        private TableLayoutPanel analyzerContent;
        private AntdUI.Panel analyzerTreeSurface;
        private TreeView folderTreeView;
        private AntdUI.Panel analyzerDetailsSurface;
        private Label analyzerDetailsTitle;
        private Label analyzerPathValue;
        private Label analyzerSizeValue;
        private Label analyzerFilesValue;
        private Label analyzerFoldersValue;
        private AntdUI.Button analyzerUseForCleanupButton;
        private Panel recordsPanel;
        private FlowLayoutPanel recordsToolbar;
        private AntdUI.Button recordsNotificationTab;
        private AntdUI.Button recordsStatsTab;
        private AntdUI.Button recordsDetailsTab;
        private AntdUI.Button recordsCleanupTab;
        private AntdUI.Button recordsRefreshButton;
        private AntdUI.Panel recordsSurface;
        private Panel recordViewHost;
        private Panel cleanupRecordView;
        private AntdUI.Table cleanHistoryTable;
        private Label cleanHistoryEmptyLabel;
        private AntdUI.Table detailRecordsTable;
        private AntdUI.Table processStatsTable;
        private AntdUI.Table notificationRecordsTable;
        private ToolStripMenuItem 选择模式ToolStripMenuItem;
        private ToolStripMenuItem 进程模式ToolStripMenuItem;
        private ToolStripMenuItem 默认ToolStripMenuItem;
        private ToolStripMenuItem startMonitorNotifyItem;
        private System.Windows.Forms.Timer notifyRotateTimer;
        private AntdUI.Panel panel1;
        private AntdUI.Radio exeModeRadio;
        private AntdUI.Radio defaultModeRadio;
        private AntdUI.Panel panel2;
        private AntdUI.Label label1;
        private AntdUI.Button rulesExeTab;
        private AntdUI.Table activityRecordTable;
        private Panel rulesExeProcessView;
        private FlowLayoutPanel rulesExeProcToolBar;
        private AntdUI.Input input1;
        private AntdUI.Button rulesExeProcAddButton;
        private AntdUI.Button button1;
        private AntdUI.Table rulesExeProcViewTable;
        private AntdUI.Button cleanButton;
    }
}
