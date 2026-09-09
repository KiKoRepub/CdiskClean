using CdiskClean.Helpers;
using CdiskClean.Models;
using CdiskClean.Models.rules;
using CdiskClean.Services.database;
using CdiskClean.Services;
using System.ComponentModel;

namespace CdiskClean
{
    public partial class Form1 : AntdUI.Window
    {
        private readonly SqliteDatabaseService _databaseService;
        private readonly EtwMonitorService _etwService;
        private readonly DiskMonitorService _monitorService;
        private readonly DiskSpaceService _diskSpaceService;
        private readonly FolderSizeAnalyzer _folderAnalyzer;
        private readonly FolderPermissionAnalyzer _folderPermissionAnalyzer;
        private readonly NotificationService _notificationService;
        private readonly CleanupService _cleanupService;
        private readonly DashboardQueryService _dashboardQueryService;
        private readonly BindingList<FileChangeRecord> _exeChangeRecords;
        private readonly BindingList<FileChangeRecord> _records;
        private CancellationTokenSource? _analyzerScanCts;
        private int _analyzerScanVersion;

        private readonly object _recordsLock = new();
        private const int MaxRecords = 5000;

        /// <summary>等待批量刷入网格的变更记录（UI 线程队列，由 _recordFlushTimer 触发合并）</summary>
        private readonly List<FileChangeRecord> _pendingRecords = new();
        private System.Windows.Forms.Timer _recordFlushTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeAnalyzerDetails();

            // 记录批量刷新定时器：合并 150ms 内到达的变更记录，避免高频事件逐条刷新网格
            _recordFlushTimer = new System.Windows.Forms.Timer { Interval = 150 };
            _recordFlushTimer.Tick += (_, _) => FlushPendingRecords();

            // 初始化 ETW 监控
            _etwService = new EtwMonitorService();

            // 初始化数据库
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CdiskClean.db");
            _databaseService = new SqliteDatabaseService(dbPath);
            _databaseService.Initialize();

            // 清理服务（清理期间监控联动过滤自身动作）
            _cleanupService = new CleanupService(_databaseService.History);
            _dashboardQueryService = new DashboardQueryService(_databaseService.History);

            // 从数据库加载监视目录（空则用默认列表）
            var savedDirs = _databaseService.Rules.GetWatchDirectories();
            _monitorService = new DiskMonitorService(_etwService, _databaseService.History, _cleanupService);
            if (savedDirs.Count > 0)
                _monitorService.LoadDirectories(savedDirs);
            else
            {
                _monitorService.LoadDefaults();
                foreach (var dir in _monitorService.WatchDirectories)
                    _databaseService.Rules.SaveWatchDirectory(dir);
            }

            // 从数据库加载忽略进程（空则用默认列表），并同步进 ETW 黑名单
            var savedProcs = _databaseService.Rules.GetIgnoreProcessRecords();
            if (savedProcs.Count > 0)
                _monitorService.LoadIgnoreProcesses(savedProcs);
            else
            {
                var defaultProcesses = IgnoreProcessRecord.GetDefaultRecords();
                _monitorService.LoadIgnoreProcesses(defaultProcesses);
                foreach (var process in defaultProcesses)
                    _databaseService.Rules.SaveIgnoreProcessRecord(process);
            }

            _monitorService.LoadWatchingApplications(_databaseService.Rules.GetWatchingApplications());

            // 初始化磁盘空间服务和文件夹分析器
            _diskSpaceService = new DiskSpaceService();
            _folderAnalyzer = new FolderSizeAnalyzer();
            _folderPermissionAnalyzer = new FolderPermissionAnalyzer();

            // 初始化右下角提醒服务（与统计按钮相互独立）
            _notificationService = new NotificationService();
            _notificationService.NotificationTriggered += OnNotificationTriggered;

            // 设置数据绑定（关闭自动生成列，使用设计器定义的手动列）
            _records = new BindingList<FileChangeRecord>();
            _exeChangeRecords = new BindingList<FileChangeRecord>();

            typeFilterCombo.SelectedIndex = 0;

            // 订阅监视服务事件
            _monitorService.FileChanged += OnFileChanged;
            _monitorService.MonitorError += OnMonitorError;

            // 初始化监视目录列表视图
            SetupDirListView();
            PopulateDirListView();
            SetupDirContextMenu();

            // 初始化忽略进程列表视图
            SetupProcessListView();
            PopulateProcessListView();
            SetupProcessContextMenu();

            input1.TextChanged += rulesExeProcInput_TextChanged;
            rulesExeProcAddButton.Click += rulesExeProcAddButton_Click;
            button1.Click += rulesExeProcSelectButton_Click;
            rulesExeProcViewTable.CellClick += rulesExeProcViewTable_CellClick;

            ConfigureTableColumns();
            BindRulesTableCenter();
            // 初始化磁盘清理页
            SetupCleanPage();
            SetupDashboardEnhancements();

            // 统一外观：设计器布局 + 原生控件样式（AntdUI 控件样式由自身 Type 管理）
            UiTheme.Apply(this);

            // 运行时工作区初始化：设置初始页面与子视图（须在 Apply 之后，避免选中态颜色被覆盖）
            ShowWorkspacePage(DashboardPageId);
            ShowRulesView(true);
            ShowRecordView("notifications");
            RefreshWorkspaceStatus();

        }

        // ==================== 窗体加载 ====================

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
            timer1.Start();
            diskRefreshTimer.Start();
            RefreshDiskInfo();

            BindActivityCenter(_records);
            RefreshDashboardInsightsAsync();
        }

        // ==================== 时钟 ====================

        private void timer1_Tick(object? sender, EventArgs e)
        {
            workspaceClockStatus.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region 工具方法
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeApplication();
        }

        public void closeApplication()
        {
            _isExiting = true;
            _analyzerScanCts?.Cancel();
            _cleanScanCts?.Cancel();
            _cleanExecCts?.Cancel();
            _dashboardQuickCleanupCts?.Cancel();
            _recordFlushTimer.Stop();
            _recordFlushTimer.Dispose();
            _dashboardToolTip?.Dispose();
            _monitorService.Dispose();
            _etwService.Dispose();
            _notificationService.Dispose();
            diskRefreshTimer.Stop();
            timer1.Stop();
            Application.Exit();
        }

        #endregion

    }
}
