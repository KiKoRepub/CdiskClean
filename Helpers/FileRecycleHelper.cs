using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace CdiskClean.Helpers
{

    /// <summary>
    /// 文件移入回收站工具（Win32 SHFileOperation）
    /// </summary>
    public static class FileRecycleHelper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd; // 窗口句柄
            public uint wFunc; // 操作类型
            public string pFrom; // 源文件路径（多个路径使用 \0 分隔，末尾必须双\0）-
            public string pTo; // 目标文件路径（对于删除操作不使用）
            public ushort fFlags; // 操作标志
            public IntPtr fAnyOperationsAborted; // 是否中止操作
            public IntPtr hNameMappings; // 文件名映射
            public string lpszProgressTitle; // 进度窗口标题
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        // 操作：删除
        private const uint FO_DELETE = 3;
        // 允许撤销 → 放入回收站【关键标识】
        private const ushort FOF_ALLOWUNDO = 0x0040;
        // 不弹出确认对话框
        private const ushort FOF_NOCONFIRMATION = 0x0010;
        // 不显示进度窗口
        private const ushort FOF_SILENT = 0x0004;


        /// <summary>
        /// 将文件/文件夹移动到回收站
        /// </summary>
        /// <param name="path">文件或文件夹完整路径</param>
        /// <param name="showConfirmDialog">是否弹出系统确认删除弹窗</param>
        /// <param name="showProgress">是否显示进度窗口</param>
        /// <returns>true=操作成功；false=失败/用户取消</returns>
        public static bool SendToRecycleBin(string path, bool showConfirmDialog = true, bool showProgress = false)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return false;
            }

            SHFILEOPSTRUCT op = new SHFILEOPSTRUCT();
            op.hwnd = IntPtr.Zero;
            op.wFunc = FO_DELETE;
            // ⚠️必须拼接双空字符 "\0\0"，Windows Shell API要求
            op.pFrom = path + "\0";

            ushort flags = FOF_ALLOWUNDO;

            if (!showConfirmDialog)
            {
                flags |= FOF_NOCONFIRMATION;
            }
            if (!showProgress)
            {
                flags |= FOF_SILENT;
            }

            op.fFlags = flags;

            int ret = SHFileOperation(ref op);
            // 返回0 代表操作正常完成
            return ret == 0;
        }


        /// <summary>
        /// 批量删除多个文件到回收站
        /// </summary>
        /// <param name="paths">路径数组</param>
        /// <param name="showConfirmDialog">弹窗确认</param>
        /// <returns></returns>
        public static bool SendFilesToRecycleBin(string[] paths, bool showConfirmDialog = true)
        {
            if (paths == null || paths.Length == 0)
                return false;

            SHFILEOPSTRUCT op = new SHFILEOPSTRUCT();
            op.hwnd = IntPtr.Zero;
            op.wFunc = FO_DELETE;
            // 多个路径使用 \0 分隔，末尾必须双\0
            op.pFrom = string.Join("\0", paths) + "\0";

            ushort flags = FOF_ALLOWUNDO;
            if (!showConfirmDialog)
                flags |= FOF_NOCONFIRMATION;

            op.fFlags = flags;

            int ret = SHFileOperation(ref op);
            return ret == 0;
        }
    }

    //public static struct 

}

