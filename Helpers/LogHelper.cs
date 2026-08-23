using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class LogHelper
    {
        private static readonly object LogLock = new();

        public static void showDefaultToDoMessage(string msg)
        {
            MessageBox.Show(msg, "还没完成呢，等一等", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static string? LogException(Exception exception, string source)
        {
            try
            {
                var logDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CdiskClean",
                    "logs");
                Directory.CreateDirectory(logDir);

                var logPath = Path.Combine(logDir, $"error-{DateTime.Now:yyyyMMdd}.log");
                var message =
                    $"[{DateTime.Now:O}] {source}{Environment.NewLine}" +
                    $"{exception}{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";
                lock (LogLock)
                {
                    File.AppendAllText(logPath, message, System.Text.Encoding.UTF8);
                }
                return logPath;
            }
            catch
            {
                return null;
            }
        }
    }
}
