using CdiskClean.Models;
using System.ComponentModel;

namespace CdiskClean
{
    public partial class Form3 : Form
    {
        public Form3(List<FileChangeRecord> records)
        {
            InitializeComponent();
            LoadRecords(records);
        }

        private void LoadRecords(List<FileChangeRecord> records)
        {
            var sorted = records
                .OrderByDescending(r => r.Timestamp)
                .ToList();

            var bindingList = new BindingList<FileChangeRecord>(sorted);
            detailDataGrid.DataSource = bindingList;
        }

        private void closeBtn_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
