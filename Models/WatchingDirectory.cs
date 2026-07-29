using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Models
{
    internal enum DirectoryStatusEnum
    {
        USING,
        FORBIDDEN,
        DELETED,
    }
    internal class WatchingDirectory
    {
        public string Path { get; set; }
        public bool IncludeSubdirs {  get; set; }

        public DirectoryStatusEnum Status { get; set; }

        public WatchingDirectory(string path, bool includeSubdirs)
        {
            Path = path;
            IncludeSubdirs = includeSubdirs;
            Status = DirectoryStatusEnum.USING;
        }
    }
}
