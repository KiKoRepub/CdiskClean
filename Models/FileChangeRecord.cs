using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CdiskClean.Models;

public enum ChangeType
{
    Created,
    Changed,
    Deleted,
    Renamed
}

public class FileChangeRecord : INotifyPropertyChanged
{
    private DateTime _timestamp = DateTime.Now;
    private ChangeType _changeType;
    private string _fullPath = string.Empty;
    private string _fileName = string.Empty;
    private string _directory = string.Empty;
    private long? _sizeBytes;
    private string? _sourceProcess;

    public DateTime Timestamp
    {
        get => _timestamp;
        set { _timestamp = value; OnPropertyChanged(); }
    }

    public ChangeType ChangeType
    {
        get => _changeType;
        set { _changeType = value; OnPropertyChanged(); }
    }

    public string FullPath
    {
        get => _fullPath;
        set { _fullPath = value; OnPropertyChanged(); }
    }

    public string FileName
    {
        get => _fileName;
        set { _fileName = value; OnPropertyChanged(); }
    }

    public string Directory
    {
        get => _directory;
        set { _directory = value; OnPropertyChanged(); }
    }

    public long? SizeBytes
    {
        get => _sizeBytes;
        set { _sizeBytes = value; OnPropertyChanged(); }
    }

    public string? SourceProcess
    {
        get => _sourceProcess;
        set { _sourceProcess = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
