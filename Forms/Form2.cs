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
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // 在窗体加载时启动 ETW 监控
            StartEtwMonitoring("D:\\university\\CSharp\\projects\\Day14\\files\\defaultRead.txt");
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
    }

    // 定义一个后台任务来运行 ETW 监控



}
