using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CdiskClean.Forms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();


            setTreeNodes();
        }
        /*
         * -----------------------页面------------------------
         * | item1                          文件大小，创建时间|
         * |   |-item2                      文件大小，创建时间| 
         * |   |-item3                      文件大小，创建时间|
         * |       |-item4                  文件大小，创建时间|
         * ----------------------------------------------------
         * 
         */

        private void setTreeNodes()
        {
            TreeItem item = new TreeItem();
            item.Text = "Root";
            item.SubTitle = "文件大小，创建时间";

            // 角标 徽章
            item.SetBadge("666", TAlign.TR);
            item.SetBadgeFore(Color.White);
            
            item.SetBadgeOffset(30, 0);
            

            item.SetCheckable(true);
            item.CheckedStrictly(false,false);

            item.SetSub( new TreeItem
            {
                Text = "Sub1",
                SubTitle = "文件大小，创建时间"
            });



           

            tree1.Items.Add(item);
        }
    }
}
