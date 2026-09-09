using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CdiskClean.Models;

public class ProcessNotificationRecord : INotifyPropertyChanged
{
    public string ProcessName { get; set; } = string.Empty;

    public int OperationCount { get; set; }

    public int DurationSeconds { get; set; }

    public DateTime TriggerTime { get; set; }

    // 委托
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
