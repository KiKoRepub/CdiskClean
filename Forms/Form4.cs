using CdiskClean.Models;
using CdiskClean.Helpers;
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
            detailDataGrid.CellFormatting += detailDataGrid_CellFormatting;
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

        private void detailDataGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == DetailTypeColumn.Index && e.Value is ChangeType changeType)
            {
                e.Value = EnumHelper.FormatChangeType(changeType);
                e.FormattingApplied = true;
            }
        }

        private void closeBtn_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
