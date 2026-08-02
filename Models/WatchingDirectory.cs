using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Models
{
    public enum RecordStatusEnum
    {
        USING,
        FORBIDDEN,
        DELETED,
    }
    public class WatchingDirectory
    {
        public string Path { get; set; }
        public bool IncludeSubdirs {  get; set; }

        public RecordStatusEnum Status { get; set; }

        public WatchingDirectory(string path, bool includeSubdirs)
        {
            Path = path;
            IncludeSubdirs = includeSubdirs;
            Status = RecordStatusEnum.USING;
        }

        public static string getCreateSQL()
        {
            return @"CREATE TABLE IF NOT EXISTS WatchDirectories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Path TEXT NOT NULL UNIQUE,
                IncludeSubdirs INTEGER NOT NULL DEFAULT 1,
                Status TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );";
        }
    }
}
