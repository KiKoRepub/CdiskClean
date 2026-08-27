using CdiskClean.Models.rules;
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
    }
}
