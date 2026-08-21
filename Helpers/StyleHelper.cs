using CdiskClean.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class StyleHelper
    {

        public static void ApplyRecordStatusStyle(ListViewItem item, RecordStatusEnum status)
        {
            switch (status)
            {
                case RecordStatusEnum.USING:
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.FromArgb(230, 255, 230); // 浅绿底
                    break;
                case RecordStatusEnum.FORBIDDEN:
                    item.ForeColor = Color.Gray;
                    item.BackColor = Color.FromArgb(255, 255, 230); // 浅黄底
                    break;
                case RecordStatusEnum.DELETED:
                    item.ForeColor = Color.LightGray;
                    item.BackColor = Color.White;
                    break;
            }
        }

        /// <summary>DataGridView 行版：与 ListViewItem 版保持相同配色语义。</summary>
        public static void ApplyRecordStatusStyle(DataGridViewRow row, RecordStatusEnum status)
        {
            switch (status)
            {
                case RecordStatusEnum.USING:
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 255, 230);
                    break;
                case RecordStatusEnum.FORBIDDEN:
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 230);
                    break;
                case RecordStatusEnum.DELETED:
                    row.DefaultCellStyle.ForeColor = Color.LightGray;
                    row.DefaultCellStyle.BackColor = Color.White;
                    break;
            }
        }
    }
}
