using CdiskClean.agents;
using System.Text.Json;
namespace CdiskClean;

public partial class Form1
{
    private readonly Panel agentPanel = new() { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };
    private CancellationTokenSource? _agentRequestCts;
    private void InitializeAgentPage()
    {
        workspacePageContainer.Controls.Add(agentPanel);
        workspaceMenu.Items.Add(new AntdUI.MenuItem { ID = "ai", Text = "AI 服务", IconSvg = "RobotOutlined" });
        var layout = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(24) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        agentPanel.Controls.Add(layout);
        var enabled = new CheckBox { Text = "启用 AI 服务", AutoSize = true };
        var endpoint = new AntdUI.Input { PlaceholderText = "https://你的网关/完整请求路径" };
        var model = new AntdUI.Input { PlaceholderText = "模型或 Agent 名称" };
        var key = new AntdUI.Input { UseSystemPasswordChar = true, PlaceholderText = "API Key" };
        var timeout = new NumericUpDown { Minimum = 1, Maximum = 300, Value = 30 };
        var status = new Label { AutoSize = true, MaximumSize = new Size(800, 0) };
        void AddRow(string text, Control control)
        {
            var row = layout.RowCount++;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label { Text = text, AutoSize = true, Margin = new Padding(0, 14, 8, 14) }, 0, row);
            control.Dock = DockStyle.Top;
            control.Margin = new Padding(0, 6, 0, 6);
            control.AccessibleName = text;
            if (control is AntdUI.Input) control.Height = 40;
            layout.Controls.Add(control, 1, row);
        }
        AddRow("服务状态", enabled);
        AddRow("请求地址", endpoint);
        AddRow("模型 / Agent", model);
        AddRow("API Key", key);
        AddRow("超时（秒）", timeout);
        AddRow("协议", new Label { AutoSize = true, Text = "JSON 网关：POST { model, data, prompt, returnFormat }，Bearer 鉴权，直接返回 JSON。\n地址必须支持上述协议；不自动适配供应商 API。密钥使用 Windows 当前用户加密保存。" });
        var buttons = new FlowLayoutPanel { AutoSize = true };
        var save = new AntdUI.Button { Text = "保存配置", Type = AntdUI.TTypeMini.Primary, Size = new Size(120, 40) };
        var test = new AntdUI.Button { Text = "测试连接", Size = new Size(120, 40) };
        var cancel = new AntdUI.Button { Text = "取消请求", Enabled = false, Size = new Size(120, 40) };
        buttons.Controls.AddRange(new Control[] { save, test, cancel });
        AddRow("操作", buttons);
        AddRow("发送预览", new Label { AutoSize = true, Text = "测试仅发送固定提示词：请返回 JSON 对象 {\"ok\":true}。\n不发送本地文件、路径或日志；测试可能产生服务费用。" });
        AddRow("结果", status);
        AgentSettings Read() => new()
        {
            Enabled = enabled.Checked, Endpoint = endpoint.Text.Trim(), Model = model.Text.Trim(),
            ApiKey = key.Text.Trim(), TimeoutSeconds = (int)timeout.Value
        };
        try
        {
            var settings = AgentSettings.Load();
            enabled.Checked = settings.Enabled;
            endpoint.Text = settings.Endpoint;
            model.Text = settings.Model;
            key.Text = settings.ApiKey;
            timeout.Value = Math.Clamp(settings.TimeoutSeconds, 1, 300);
            status.Text = "配置就绪。仅主动测试或请求时连接网络。";
        }
        catch { status.Text = "配置读取失败，请重新填写并保存；其他功能仍可使用。"; }
        save.Click += (_, _) =>
        {
            try { Read().Save(); status.Text = "配置已加密保存。"; }
            catch (InvalidOperationException ex) { status.Text = ex.Message; }
            catch { status.Text = "保存失败，请检查本地配置目录权限。"; }
        };
        cancel.Click += (_, _) => _agentRequestCts?.Cancel();
        Disposed += (_, _) => _agentRequestCts?.Cancel();
        test.Click += async (_, _) =>
        {
            using var cts = new CancellationTokenSource();
            _agentRequestCts = cts;
            test.Enabled = save.Enabled = false;
            cancel.Enabled = true;
            status.Text = "正在测试…";
            try
            {
                var result = await AgentRequestHelper.SendAsync<JsonElement>(Read(), "请返回 JSON 对象 {\"ok\":true}。", cancellationToken: cts.Token);
                if (cts.IsCancellationRequested || IsDisposed) return;
                status.Text = result.ValueKind == JsonValueKind.Object && result.TryGetProperty("ok", out var ok) && ok.ValueKind == JsonValueKind.True
                    ? "连接成功，响应格式有效。" : "服务已响应，但未返回约定的 {\"ok\":true}。";
            }
            catch (OperationCanceledException) { if (!IsDisposed) status.Text = "请求已取消。"; }
            catch (InvalidOperationException ex) { if (!IsDisposed) status.Text = ex.Message; }
            catch { if (!IsDisposed) status.Text = "AI 测试失败，请检查配置。"; }
            finally
            {
                _agentRequestCts = null;
                if (!IsDisposed) { test.Enabled = save.Enabled = true; cancel.Enabled = false; }
            }
        };
    }
}
