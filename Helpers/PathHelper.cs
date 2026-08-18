using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class PathHelper
    {

        public static string GetDownloadsPath()
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            return downloadsPath;
        }
        public static string GetUserTempPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");
        }

        /// <summary>判断 path 是否位于 parent 之内（含相等）</summary>
        public static bool IsPathInside(string path, string parent)
        {
            var full = path.TrimEnd('\\');
            var root = parent.TrimEnd('\\');
            if (full.Equals(root, StringComparison.OrdinalIgnoreCase)) return true;
            return full.StartsWith(root + "\\", StringComparison.OrdinalIgnoreCase);
        }


    }
}
