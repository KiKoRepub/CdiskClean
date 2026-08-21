using CdiskClean.Helpers;
using CdiskClean.Services;

namespace CdiskClean;

/// <summary>工作区「清理中心」页逻辑：清理方式面板的动态布局</summary>
public partial class Form1
{
    /// <summary>清理方式单选按钮与枚举映射（初始化于 SetupCleanPage）</summary>
    private (RadioButton Radio, CleanupMethod Method)[] _cleanupMethodRadios = Array.Empty<(RadioButton, CleanupMethod)>();

    /// <summary>按面板宽度动态排列清理方式区控件（单选、目标目录、清理按钮）</summary>
    private void LayoutCleanupMethodPanel(Control panel)
    {
        if (_cleanupMethodRadios.Length == 0) return;
        var width = Math.Max(250, panel.ClientSize.Width);
        var y = 46;
        foreach (var (radio, _) in _cleanupMethodRadios)
        {
            radio.SetBounds(4, y, width - 8, 27);
            y += 29;
        }

        cleanTargetLabel.SetBounds(4, y + 3, width - 8, 24);
        y += 29;
        cleanTargetTextBox.SetBounds(4, y, Math.Max(120, width - 92), 32);
        cleanTargetSelectBtn.SetBounds(width - 80, y - 1, 76, 34);
        cleanBtn.SetBounds(4, Math.Max(y + 46, panel.ClientSize.Height - 48), width - 8, 42);
    }

    // ==================== 事件包装方法（设计器绑定） ====================

    private void cleanupMethodPanel_Resize(object? sender, EventArgs e)
    {
        if (sender is Control panel)
            LayoutCleanupMethodPanel(panel);
    }
}
