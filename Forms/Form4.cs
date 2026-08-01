using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean
{
    public partial class Form4 : Form
    {
        public Form4(List<FileChangeRecord> records, List<ProcessNotificationRecord> notifications)
        {
            InitializeComponent();
            LoadNotifications(notifications);
            LoadProcessStats(records);
            LoadDetailRecords(records);
        }

        private void LoadNotifications(List<ProcessNotificationRecord> notifications)
        {
            notificationGrid.DataSource = new BindingList<ProcessNotificationRecord>(notifications);
        }

        private void LoadProcessStats(List<FileChangeRecord> records)
        {
            var stats = records
                .GroupBy(r => string.IsNullOrWhiteSpace(r.SourceProcess) ? "未知进程" : r.SourceProcess,
                    StringComparer.OrdinalIgnoreCase)
                .Select(g => new AppChangeStats
                {
                    AppName = g.Key,
                    ChangeCount = g.Count(),
                    FirstChangeTime = g.Min(r => r.Timestamp),
                    LastChangeTime = g.Max(r => r.Timestamp)
                })
                .OrderByDescending(s => s.ChangeCount)
                .ThenByDescending(s => s.LastChangeTime)
                .ToList();

            statsGrid.DataSource = new BindingList<AppChangeStats>(stats);
        }

        private void LoadDetailRecords(List<FileChangeRecord> records)
        {
            var sorted = records
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            detailDataGrid.DataSource = new BindingList<FileChangeRecord>(sorted);
        }

        private void closeBtn_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
