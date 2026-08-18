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

    public static string GetCreateSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS IgnoreProcessRecord (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProcessName TEXT NOT NULL UNIQUE,
                Status TEXT NOT NULL DEFAULT 'USING',
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            );";
    }

    public static List<IgnoreProcessRecord> GetDefaultRecords()
    {
        return new List<IgnoreProcessRecord>
        {
            new IgnoreProcessRecord("SearchProtocolHost"), // Windows 搜索索引
            new IgnoreProcessRecord("SearchIndexer"),      // Windows 搜索服务
            new IgnoreProcessRecord("System"),                 // 系统内核操作
            new IgnoreProcessRecord("svchost"),            // 系统服务宿主
            new IgnoreProcessRecord("MsMpEng"),            // Windows Defender 杀毒软件
            new IgnoreProcessRecord("Explorer"),            // 资源管理器（有时也会产生干扰，视需求而定）
            new IgnoreProcessRecord("RuntimeBroker"),      // Windows 运行时代理
            new IgnoreProcessRecord("taskhostw"),          // Windows 任务宿主
            new IgnoreProcessRecord("avp")          // Kaspersky 杀毒软件
        };
    }
}
