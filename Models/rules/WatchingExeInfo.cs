using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Models.rules
{
    internal class WatchingExeInfo
    {
        public string ExeName { get; set; }
        public string FullPath { get; set; }
        public string SizeBytes { get; set; }
        public string RunningState { get; set; }
        public string MonitoringState { get; set; }


        public static string GetCreateSQL()
        {
            return @"CREATE TABLE IF NOT EXISTS WatchingExeInfo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExeName TEXT NOT NULL,
                FullPath TEXT NOT NULL UNIQUE,
                SizeBytes TEXT NOT NULL,
                RunningState TEXT NOT NULL DEFAULT '运行中',
                MonitoringState TEXT NOT NULL DEFAULT '未监视',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            );";
        }
    }



}
