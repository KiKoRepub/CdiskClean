using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.Models;

// This class represents a record of a process that should be ignored during monitoring or logging. It contains the name of the process to be ignored.
public class IgnoreProcessRecord
{
    public string ProcessName { get; set; } = string.Empty;
    public RecordStatusEnum Status { get; set; }

    public IgnoreProcessRecord(string processName)
    {
        ProcessName = processName;
        Status = RecordStatusEnum.USING;
    }

    public static string getCreateSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS IgnoreProcessRecord (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProcessName TEXT NOT NULL,
                Status TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP   
            );";
    }

}
