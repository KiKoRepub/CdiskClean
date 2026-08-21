using CdiskClean.Models;
using CdiskClean.Helpers;
using System.ComponentModel;
using Sunny.UI;

namespace CdiskClean
{
    /// <summary>
    /// 统计窗口
    /// </summary>
    public partial class StatisticForm : Sunny.UI.UIForm
    {
        public StatisticForm(List<FileChangeRecord> records, List<ProcessNotificationRecord> notifications)
        {
            InitializeComponent();
#if DEBUG
            ApplyDebugLayout();
#endif
            LoadNotifications(notifications);
            LoadProcessStats(records);
            LoadDetailRecords(records);
            detailDataGrid.CellFormatting += detailDataGrid_CellFormatting;
        }

        private void ApplyDebugLayout()
        {
            ShowTitle = true;
            ControlBox = true;
            TitleHeight = 42;
            Text = "记录中心";
            ClientSize = new Size(1120, 680);
            MinimumSize = new Size(860, 540);
            MaximizeBox = true;
            MinimizeBox = true;
            BackColor = Color.FromArgb(244, 246, 249);

            Controls.Clear();
            titleLabel.Visible = false;
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(244, 246, 249),
                ColumnCount = 1,
                RowCount = 1,
                Padding = new Padding(14)
            };
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Margin = new Padding(0);
            mainTabControl.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);

            foreach (var page in new[] { notificationTabPage, statsTabPage, detailTabPage })
            {
                page.BackColor = Color.White;
                page.Padding = new Padding(8);
            }
            foreach (var grid in new DataGridView[] { notificationGrid, statsGrid, detailDataGrid })
            {
                ConfigureRecordGrid(grid);
                grid.Dock = DockStyle.Fill;
                grid.Margin = new Padding(0);
            }

            root.Controls.Add(mainTabControl, 0, 0);
            Controls.Add(root);
        }

        private static void ConfigureRecordGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(232, 236, 241);
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(71, 84, 103);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 238, 252);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(24, 77, 120);
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);
            grid.RowTemplate.Height = 34;
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

    }
}
