using CdiskClean.Models;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CdiskClean
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            _cts = new CancellationTokenSource();


            initDataGrid();
        }

        private void initDataGrid()
        {
            // 初始化 DataGridView 的数据源
            dataGridView1.DataSource = new BindingList<Person>(Person.GetSampleData());

            dataGridView1.AutoGenerateColumns = false;



        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // 在窗体加载时启动 ETW 监控
            //StartEtwMonitoring("D:\\university\\CSharp\\projects\\Day14\\files\\defaultRead.txt");
        }
        private CancellationTokenSource _cts;

        private void StartEtwMonitoring(string targetFileName)
        {

            Task.Run(() =>
            {
                // 1. 创建 ETW 会话 (需要管理员权限)
                using (var session = new TraceEventSession("MyFileMonitorSession"))
                {
                    // 2. 开启 Kernel File Provider (监控文件操作)
                    // KernelTraceEventParser.Keywords.FileIOInit 包含了文件创建/读取/写入等信息
                    session.EnableKernelProvider(
                        KernelTraceEventParser.Keywords.FileIOInit,
                        KernelTraceEventParser.Keywords.None
                    );

                    // 3. 设置事件源
                    var source = session.Source;

                    //4.订阅文件操作事件
                    // 这里的 FileIOReadWrite 是示例，还有 FileIOCreate, FileIODelete 等
                    source.Kernel.FileIORead += (data) =>
                    {
                        // 这里的逻辑对应你的流程图：获取 ProcessId 和 ProcessName
                        if (data.FileName.Contains(targetFileName)) // 过滤你关心的文件
                        {
                            string processName = data.ProcessName;
                            int processId = data.ProcessID;

                            // 5. 更新UI (需要使用 Invoke)
                            this.Invoke((MethodInvoker)delegate
                            {
                                listBoxLog.Items.Add($"文件被修改，来源进程：{processName} (PID: {processId})");
                            });
                        }
                    };

                    // 开始处理事件（阻塞线程，直到取消）
                    source.Process();
                }
            }, _cts.Token);
        }

        // 记得在窗体关闭时停止会话，否则会占用系统资源
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cts?.Cancel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000,
                "当前时间：", DateTime.Now.ToLocalTime().ToString(),
                ToolTipIcon.Info);
        }


        // 拖拽测试

        private bool _isDragging = false;
        private Point _mouseStartPos;

        // 鼠标按下：记录起点
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseStartPos = e.Location;
                _isDragging = false;
            }
        }

        // 鼠标移动：启动拖拽
        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            // 只处理左键、未开始拖拽、移动距离达到阈值（防止点击误触发拖拽）
            if (e.Button != MouseButtons.Left || _isDragging) return;

            int moveRange = SystemInformation.DragSize.Width;
            var offset = Math.Abs(e.X - _mouseStartPos.X) + Math.Abs(e.Y - _mouseStartPos.Y);
            if (offset < moveRange) return;

            // 获取鼠标位置对应的行
            var hit = dataGridView1.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[hit.RowIndex];
            if (row.DataBoundItem is not Person record) return;

            _isDragging = true;
            // 【重点】直接拖拽实体对象
            dataGridView1.DoDragDrop(record, DragDropEffects.Copy | DragDropEffects.Move);
        }

        // 鼠标松开，重置标记
        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }


        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            // 判断是否存在我们传入的数据
            if (e.Data.GetDataPresent(typeof(Person)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            Person person = e.Data.GetData(typeof(Person)) as Person;
            if (person != null)
            {
                //处理对象
                MessageBox.Show($"Test Person is {person.Name} + {person.Sex}");

            }
        }
    }

}
