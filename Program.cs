using CdiskClean.Helpers;
using System.Security.Principal;

namespace CdiskClean
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            AntdUI.Config.TextRenderingHighQuality = true;
            ConfigureExceptionHandling();
#if DEBUG
            if (TryRunUiPreview(args))
                return;
#endif
            if (!IsElevated())
            {
                MessageBox.Show("此程序需要管理员权限才能正常运行。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new Form1());
        }

#if DEBUG
        private static bool TryRunUiPreview(string[] args)
        {
            if (args.Length == 0)
                return false;

            switch (args[0].ToLowerInvariant())
            {
                case "--preview-main":
                    ShowMainPreview(0);
                    return true;
                case "--preview-watcher":
                    ShowMainPreview(1);
                    return true;
                case "--preview-analyzer":
                    ShowMainPreview(2);
                    return true;
                case "--preview-clean":
                    ShowMainPreview(3);
                    return true;
                case "--preview-rules":
                    ShowMainPreview(4);
                    return true;
                case "--preview-records":
                    ShowMainPreview(5);
                    return true;
                case "--preview-directories":
                    Application.Run(new Forms.BetterDirAddForm());
                    return true;
                case "--preview-processes":
                    Application.Run(new Forms.ProcessPickForm());
                    return true;
                default:
                    return false;
            }
        }

        private static void ShowMainPreview(int selectedIndex)
        {
            var form = new Form1();
            form.ShowWorkspacePage(selectedIndex);
            Application.Run(form);
        }
#endif

        private static void ConfigureExceptionHandling()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) =>
            {
                var logPath = LogHelper.LogException(e.Exception, "UI 线程异常");
                var detail = logPath == null
                    ? string.Empty
                    : $"{Environment.NewLine}错误日志: {logPath}";
                MessageBox.Show(
                    $"程序遇到未处理的错误: {e.Exception.Message}{detail}",
                    "程序错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception exception)
                    LogHelper.LogException(exception, "后台线程异常");
            };

            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                LogHelper.LogException(e.Exception, "未观察的任务异常");
                e.SetObserved();
            };
        }
        public static bool IsElevated()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
