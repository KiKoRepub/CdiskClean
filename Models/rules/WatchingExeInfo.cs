using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Models.rules
{
    public class WatchingExeInfo
    {
        public string ExeName { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public long? SizeBytes { get; set; }
        public RecordStatusEnum Status { get; set; } = RecordStatusEnum.USING;
        public DateTime? LastActivityAt { get; set; }
        public string RunningState { get; set; } = "未知";
        public string MonitoringState => Status == RecordStatusEnum.USING ? "启用" : "已暂停";
        public string SizeText => SizeBytes.HasValue ? CdiskClean.Helpers.FormatHelper.FormatBytes(SizeBytes.Value) : "未知";

        public bool UsesProcessIdentity => FullPath.StartsWith(
            "process://", StringComparison.OrdinalIgnoreCase);

        public string DisplayPath => UsesProcessIdentity ? "按进程名匹配" : FullPath;

        public static WatchingExeInfo Create(string value)
        {
            var trimmed = value.Trim().Trim('"');
            if (File.Exists(trimmed))
            {
                var fullPath = Path.GetFullPath(trimmed);
                return new WatchingExeInfo
                {
                    ExeName = NormalizeProcessName(Path.GetFileName(fullPath)),
                    FullPath = fullPath,
                    SizeBytes = GetFileSizeSafe(fullPath)
                };
            }

            var processName = NormalizeProcessName(trimmed);
            return new WatchingExeInfo
            {
                ExeName = processName,
                FullPath = "process://" + processName
            };
        }

        public static string NormalizeProcessName(string? processName) =>
            Path.GetFileNameWithoutExtension(processName?.Trim() ?? string.Empty);


        public static string GetCreateSQL()
        {
            return @"CREATE TABLE IF NOT EXISTS WatchingExeInfo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExeName TEXT NOT NULL,
                FullPath TEXT NOT NULL UNIQUE,
                SizeBytes INTEGER NULL,
                RunningState TEXT NOT NULL DEFAULT '未知',
                MonitoringState TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                LastActivityAt TEXT NULL
            );";
        }

        private static long? GetFileSizeSafe(string path)
        {
            try { return new FileInfo(path).Length; }
            catch { return null; }
        }
    }



}
