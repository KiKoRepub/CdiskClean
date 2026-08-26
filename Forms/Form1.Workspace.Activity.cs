using AntdUI;
using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean;

/// <summary>工作区「实时活动」页：布局已在设计器维护，本文件留空备用</summary>
public partial class Form1
{




    private void BindActivityCenter(BindingList<FileChangeRecord> records)
    {
        if (IsDisposed) return;
        // 句柄尚未创建（构造期异步完成时）挂到 Load 后执行；非 UI 线程则回传 UI 线程
        if (!IsHandleCreated)
        {
            Load += (_, _) => BindActivityCenter(records);
            return;
        }
        if (InvokeRequired)
        {
            BeginInvoke(() => BindActivityCenter(records));
            return;
        }


        activityRecordTable.Columns = new AntdUI.ColumnCollection
            {
                MakeColumn("Timestamp", "时间", "20%", AntdUI.ColumnAlign.Center),
                MakeColumn("ChangeType", "类型", "10%", AntdUI.ColumnAlign.Center),
                MakeColumn("FileName", "文件名", "20%", AntdUI.ColumnAlign.Center),
                MakeColumn("FullPath", "路径", "25%", AntdUI.ColumnAlign.Left),
                MakeColumn("SizeBytes", "大小", "10%", AntdUI.ColumnAlign.Center),
                MakeColumn("SourceProcess", "来源进程", "10%", AntdUI.ColumnAlign.Center)
            };
        activityRecordTable.DataSource = records;

        activityRecordTable.Refresh();
        //MessageBox.Show("体现");
    }




}
