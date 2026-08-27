using AntdUI;
using CdiskClean.Models.cleanUp;

namespace CdiskClean;

/// <summary>
/// 清理树节点信息气泡：单击节点后显示在其文本下方，展示创建时间与最后修改时间。
/// 激活后显示，失焦（点击其他区域/滚动）时由外部或 Deactivate 自动关闭。
/// </summary>
public sealed class NodeInfoPopover : Form
{
    private readonly System.Windows.Forms.Label _createLabel;
    private readonly System.Windows.Forms.Label _modifyLabel;
    private CancellationTokenSource? _loadCts;

    public NodeInfoPopover()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(252, 46);
        BackColor = Color.FromArgb(31, 41, 55);

        var font = new Font("Microsoft YaHei UI", 9F);
        _createLabel = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Location = new Point(14, 8),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = font
        };
        _modifyLabel = new System.Windows.Forms.Label
        {
            AutoSize = true,
            Location = new Point(14, 26),
            ForeColor = Color.FromArgb(196, 202, 214),
            BackColor = Color.Transparent,
            Font = font
        };
        Controls.Add(_modifyLabel);
        Controls.Add(_createLabel);
    }

    /// <summary>显示在节点屏幕矩形下方（越界时翻转到上方），已显示时仅更新位置</summary>
    public void ShowAt(Control anchor, Rectangle nodeScreenRect)
    {
        var work = Screen.FromControl(anchor).WorkingArea;
        var x = nodeScreenRect.Left;
        var y = nodeScreenRect.Bottom + 6;
        if (x + Width > work.Right) x = work.Right - Width - 8;
        if (x < work.Left) x = work.Left + 4;
        if (y + Height > work.Bottom) y = nodeScreenRect.Top - Height - 6;
        if (y < work.Top) y = nodeScreenRect.Bottom + 6;
        Location = new Point(x, y);
        if (!Visible) Show(anchor);
    }

    /// <summary>后台读取节点创建/修改时间并刷新气泡内容；连续调用会自动取消上一次读取</summary>
    public void BeginLoad(TreeItem item)
    {
        _loadCts?.Cancel();
        var cts = _loadCts = new CancellationTokenSource();

        var (path, isDir) = item.Tag switch
        {
            string p => (p, true),                       // 根节点 Tag 为路径字符串
            CleanupFileEntry e => (e.FullPath, e.IsDirectory),
            _ => (null, false)
        };

        if (path == null)
        {
            _createLabel.Text = "创建时间：未知";
            _modifyLabel.Text = string.Empty;
            return;
        }

        _createLabel.Text = "创建时间：读取中…";
        _modifyLabel.Text = string.Empty;

        Task.Run(() =>
        {
            try
            {
                var create = isDir ? Directory.GetCreationTime(path) : File.GetCreationTime(path);
                var modify = isDir ? Directory.GetLastWriteTime(path) : File.GetLastWriteTime(path);
                return $"创建时间：{create:yyyy-MM-dd HH:mm:ss}\t最后修改:{modify:yyyy-MM-dd HH:mm:ss}";
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
            {
                return "创建时间：无法读取";
            }
        }, cts.Token).ContinueWith(t =>
        {
            if (cts.IsCancellationRequested || IsDisposed) return;
            if (t.IsFaulted)
            {
                _createLabel.Text = "创建时间：读取失败";
                return;
            }
            var text = t.Result;
            if (text.StartsWith("创建时间：", StringComparison.Ordinal) && text.Contains('\t'))
            {
                var parts = text.Split('\t');
                _createLabel.Text = parts[0];
                _modifyLabel.Text = parts[1];
            }
            else
            {
                _createLabel.Text = text;
                _modifyLabel.Text = string.Empty;
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    /// <summary>失焦自动关闭并取消未完成的读取</summary>
    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        _loadCts?.Cancel();
        Hide();
    }
}
