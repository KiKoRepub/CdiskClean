using CdiskClean.Helpers;
using Sunny.UI;
using System.Security.Principal;

namespace CdiskClean
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            UIStyles.SetStyle(UIStyle.Gray);
            ConfigureExceptionHandling();
            if (!IsElevated())
            {
                MessageBox.Show("此程序需要管理员权限才能正常运行。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new Form1("管理员身份运行中..."));
        }

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
