using CdiskClean.Models;
using CdiskClean.Models.rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class EnumHelper
    {
        /// <summary>
        /// 格式化状态枚举为中文描述
        /// </summary>
        /// <param name="status">所需格式化的状态枚举</param>
        /// <returns></returns>
        public static string FormatStatus(RecordStatusEnum status)
        {
            return status switch
            {
                RecordStatusEnum.USING => "启用",
                RecordStatusEnum.FORBIDDEN => "已禁用",
                RecordStatusEnum.DELETED => "已删除",
                _ => "未知"
            };
        }

        public static string FormatChangeType(ChangeType changeType)
        {
            return changeType switch
            {
                ChangeType.Created => "创建",
                ChangeType.Changed => "修改",
                ChangeType.Deleted => "删除",
                ChangeType.Renamed => "重命名",
                _ => "未知"
            };
        }
    }
}
