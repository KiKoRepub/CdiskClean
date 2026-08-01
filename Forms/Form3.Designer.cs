namespace CdiskClean
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            detailDataGrid = new DataGridView();
            AppColumn = new DataGridViewTextBoxColumn();
            TimeColumnDetail = new DataGridViewTextBoxColumn();
            DirColumn = new DataGridViewTextBoxColumn();
            FileColumn = new DataGridViewTextBoxColumn();
            TypeColDetail = new DataGridViewTextBoxColumn();
            titleLabel = new Label();
            closeBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)detailDataGrid).BeginInit();
            SuspendLayout();
            //
            // detailDataGrid
            //
            detailDataGrid.AllowUserToAddRows = false;
            detailDataGrid.AllowUserToDeleteRows = false;
            detailDataGrid.AllowUserToResizeRows = false;
            detailDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            detailDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            detailDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            detailDataGrid.Columns.AddRange(new DataGridViewColumn[] { AppColumn, TimeColumnDetail, TypeColDetail, DirColumn, FileColumn });
            detailDataGrid.Location = new Point(12, 50);
            detailDataGrid.Name = "detailDataGrid";
            detailDataGrid.ReadOnly = true;
            detailDataGrid.RowHeadersVisible = false;
            detailDataGrid.RowHeadersWidth = 62;
            detailDataGrid.RowTemplate.Height = 25;
            detailDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            detailDataGrid.Size = new Size(960, 500);
            detailDataGrid.TabIndex = 0;
            //
            // AppColumn
            //
            AppColumn.DataPropertyName = "SourceProcess";
            AppColumn.FillWeight = 70F;
            AppColumn.HeaderText = "应用程序";
            AppColumn.MinimumWidth = 8;
            AppColumn.Name = "AppColumn";
            AppColumn.ReadOnly = true;
            //
            // TimeColumnDetail
            //
            TimeColumnDetail.DataPropertyName = "Timestamp";
            TimeColumnDetail.FillWeight = 80F;
            TimeColumnDetail.HeaderText = "时间";
            TimeColumnDetail.MinimumWidth = 8;
            TimeColumnDetail.Name = "TimeColumnDetail";
            TimeColumnDetail.ReadOnly = true;
            //
            // DirColumn
            //
            DirColumn.DataPropertyName = "Directory";
            DirColumn.FillWeight = 100F;
            DirColumn.HeaderText = "目录";
            DirColumn.MinimumWidth = 8;
            DirColumn.Name = "DirColumn";
            DirColumn.ReadOnly = true;
            //
            // FileColumn
            //
            FileColumn.DataPropertyName = "FileName";
            FileColumn.FillWeight = 70F;
            FileColumn.HeaderText = "文件名称";
            FileColumn.MinimumWidth = 8;
            FileColumn.Name = "FileColumn";
            FileColumn.ReadOnly = true;
            //
            // TypeColDetail
            //
            TypeColDetail.DataPropertyName = "ChangeType";
            TypeColDetail.FillWeight = 50F;
            TypeColDetail.HeaderText = "类型";
            TypeColDetail.MinimumWidth = 8;
            TypeColDetail.Name = "TypeColDetail";
            TypeColDetail.ReadOnly = true;
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            titleLabel.Location = new Point(12, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(200, 31);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "文件变更详细记录";
            //
            // closeBtn
            //
            closeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeBtn.Location = new Point(888, 9);
            closeBtn.Name = "closeBtn";
            closeBtn.Size = new Size(84, 34);
            closeBtn.TabIndex = 2;
            closeBtn.Text = "关闭";
            closeBtn.UseVisualStyleBackColor = true;
            closeBtn.Click += closeBtn_Click;
            //
            // Form3
            //
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(closeBtn);
            Controls.Add(titleLabel);
            Controls.Add(detailDataGrid);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form3";
            StartPosition = FormStartPosition.CenterParent;
            Text = "变更记录详情";
            ((System.ComponentModel.ISupportInitialize)detailDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView detailDataGrid;
        private DataGridViewTextBoxColumn AppColumn;
        private DataGridViewTextBoxColumn TimeColumnDetail;
        private DataGridViewTextBoxColumn TypeColDetail;
        private DataGridViewTextBoxColumn DirColumn;
        private DataGridViewTextBoxColumn FileColumn;
        private Label titleLabel;
        private Button closeBtn;
    }
}
