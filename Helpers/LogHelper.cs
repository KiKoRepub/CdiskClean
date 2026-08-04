using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Helpers
{
    internal class LogHelper
    {

        public static void showDefaultToDoMessage(string msg)
        {
            MessageBox.Show(msg,"还没完成呢，等一等",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }
    }
}
