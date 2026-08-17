namespace CdiskClean
{
    partial class StatisticForm
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
            mainTabControl = new TabControl();
            notificationTabPage = new TabPage();
            notificationGrid = new DataGridView();
            ProcessNameColumn = new DataGridViewTextBoxColumn();
            OpCountColumn = new DataGridViewTextBoxColumn();
            DurationColumn = new DataGridViewTextBoxColumn();
            TriggerTimeColumn = new DataGridViewTextBoxColumn();
            statsTabPage = new TabPage();
            statsGrid = new DataGridView();
            StatsAppColumn = new DataGridViewTextBoxColumn();
            StatsCountColumn = new DataGridViewTextBoxColumn();
            StatsFirstTimeColumn = new DataGridViewTextBoxColumn();
            StatsLastTimeColumn = new DataGridViewTextBoxColumn();
            detailTabPage = new TabPage();
            detailDataGrid = new DataGridView();
            DetailAppColumn = new DataGridViewTextBoxColumn();
            DetailTimeColumn = new DataGridViewTextBoxColumn();
            DetailTypeColumn = new DataGridViewTextBoxColumn();
            DetailDirColumn = new DataGridViewTextBoxColumn();
            DetailFileColumn = new DataGridViewTextBoxColumn();
            titleLabel = new Label();
            closeBtn = new AntdUI.Button();
            mainTabControl.SuspendLayout();
            notificationTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)notificationGrid).BeginInit();
            statsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)statsGrid).BeginInit();
            detailTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)detailDataGrid).BeginInit();
            SuspendLayout();
            //
            // mainTabControl
            //
            mainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainTabControl.Controls.Add(notificationTabPage);
            mainTabControl.Controls.Add(statsTabPage);
            mainTabControl.Controls.Add(detailTabPage);
            mainTabControl.Location = new Point(12, 50);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(1076, 590);
            mainTabControl.TabIndex = 0;
            //
            // notificationTabPage
            //
            notificationTabPage.Controls.Add(notificationGrid);
            notificationTabPage.Location = new Point(4, 33);
            notificationTabPage.Name = "notificationTabPage";
            notificationTabPage.Padding = new Padding(3);
            notificationTabPage.Size = new Size(1068, 553);
            notificationTabPage.TabIndex = 0;
            notificationTabPage.Text = "提醒记录";
            notificationTabPage.UseVisualStyleBackColor = true;
            //
            // notificationGrid
            //
            notificationGrid.AllowUserToAddRows = false;
            notificationGrid.AllowUserToDeleteRows = false;
            notificationGrid.AllowUserToResizeRows = false;
            notificationGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            notificationGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            notificationGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            notificationGrid.Columns.AddRange(new DataGridViewColumn[] { ProcessNameColumn, OpCountColumn, DurationColumn, TriggerTimeColumn });
            notificationGrid.Location = new Point(3, 3);
            notificationGrid.Name = "notificationGrid";
            notificationGrid.ReadOnly = true;
            notificationGrid.RowHeadersVisible = false;
            notificationGrid.RowHeadersWidth = 62;
            notificationGrid.RowTemplate.Height = 25;
            notificationGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            notificationGrid.Size = new Size(1062, 547);
            notificationGrid.TabIndex = 0;
            //
            // ProcessNameColumn
            //
            ProcessNameColumn.DataPropertyName = "ProcessName";
            ProcessNameColumn.FillWeight = 80F;
            ProcessNameColumn.HeaderText = "进程名";
            ProcessNameColumn.MinimumWidth = 8;
            ProcessNameColumn.Name = "ProcessNameColumn";
            ProcessNameColumn.ReadOnly = true;
            //
            // OpCountColumn
            //
            OpCountColumn.DataPropertyName = "OperationCount";
            OpCountColumn.FillWeight = 50F;
            OpCountColumn.HeaderText = "操作次数";
            OpCountColumn.MinimumWidth = 8;
            OpCountColumn.Name = "OpCountColumn";
            OpCountColumn.ReadOnly = true;
            //
            // DurationColumn
            //
            DurationColumn.DataPropertyName = "DurationSeconds";
            DurationColumn.FillWeight = 60F;
            DurationColumn.HeaderText = "持续时间(秒)";
            DurationColumn.MinimumWidth = 8;
            DurationColumn.Name = "DurationColumn";
            DurationColumn.ReadOnly = true;
            //
            // TriggerTimeColumn
            //
            TriggerTimeColumn.DataPropertyName = "TriggerTime";
            TriggerTimeColumn.FillWeight = 80F;
            TriggerTimeColumn.HeaderText = "提醒时间";
            TriggerTimeColumn.MinimumWidth = 8;
            TriggerTimeColumn.Name = "TriggerTimeColumn";
            TriggerTimeColumn.ReadOnly = true;
            //
            // statsTabPage
            //
            statsTabPage.Controls.Add(statsGrid);
            statsTabPage.Location = new Point(4, 33);
            statsTabPage.Name = "statsTabPage";
            statsTabPage.Padding = new Padding(3);
            statsTabPage.Size = new Size(1068, 553);
            statsTabPage.TabIndex = 1;
            statsTabPage.Text = "进程统计";
            statsTabPage.UseVisualStyleBackColor = true;
            //
            // statsGrid
            //
            statsGrid.AllowUserToAddRows = false;
            statsGrid.AllowUserToDeleteRows = false;
            statsGrid.AllowUserToResizeRows = false;
            statsGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            statsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            statsGrid.Columns.AddRange(new DataGridViewColumn[] { StatsAppColumn, StatsCountColumn, StatsFirstTimeColumn, StatsLastTimeColumn });
            statsGrid.Location = new Point(3, 3);
            statsGrid.Name = "statsGrid";
            statsGrid.ReadOnly = true;
            statsGrid.RowHeadersVisible = false;
            statsGrid.RowHeadersWidth = 62;
            statsGrid.RowTemplate.Height = 25;
            statsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            statsGrid.Size = new Size(1062, 547);
            statsGrid.TabIndex = 0;
            //
            // StatsAppColumn
            //
            StatsAppColumn.DataPropertyName = "AppName";
            StatsAppColumn.FillWeight = 80F;
            StatsAppColumn.HeaderText = "进程名";
            StatsAppColumn.MinimumWidth = 8;
            StatsAppColumn.Name = "StatsAppColumn";
            StatsAppColumn.ReadOnly = true;
            //
            // StatsCountColumn
            //
            StatsCountColumn.DataPropertyName = "ChangeCount";
            StatsCountColumn.FillWeight = 50F;
            StatsCountColumn.HeaderText = "操作次数";
            StatsCountColumn.MinimumWidth = 8;
            StatsCountColumn.Name = "StatsCountColumn";
            StatsCountColumn.ReadOnly = true;
            //
            // StatsFirstTimeColumn
            //
            StatsFirstTimeColumn.DataPropertyName = "FirstChangeTime";
            StatsFirstTimeColumn.FillWeight = 80F;
            StatsFirstTimeColumn.HeaderText = "首次时间";
            StatsFirstTimeColumn.MinimumWidth = 8;
            StatsFirstTimeColumn.Name = "StatsFirstTimeColumn";
            StatsFirstTimeColumn.ReadOnly = true;
            //
            // StatsLastTimeColumn
            //
            StatsLastTimeColumn.DataPropertyName = "LastChangeTime";
            StatsLastTimeColumn.FillWeight = 80F;
            StatsLastTimeColumn.HeaderText = "最后时间";
            StatsLastTimeColumn.MinimumWidth = 8;
            StatsLastTimeColumn.Name = "StatsLastTimeColumn";
            StatsLastTimeColumn.ReadOnly = true;
            //
            // detailTabPage
            //
            detailTabPage.Controls.Add(detailDataGrid);
            detailTabPage.Location = new Point(4, 33);
            detailTabPage.Name = "detailTabPage";
            detailTabPage.Padding = new Padding(3);
            detailTabPage.Size = new Size(1068, 553);
            detailTabPage.TabIndex = 2;
            detailTabPage.Text = "详细记录";
            detailTabPage.UseVisualStyleBackColor = true;
            //
            // detailDataGrid
            //
            detailDataGrid.AllowUserToAddRows = false;
            detailDataGrid.AllowUserToDeleteRows = false;
            detailDataGrid.AllowUserToResizeRows = false;
            detailDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            detailDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            detailDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            detailDataGrid.Columns.AddRange(new DataGridViewColumn[] { DetailAppColumn, DetailTimeColumn, DetailTypeColumn, DetailDirColumn, DetailFileColumn });
            detailDataGrid.Location = new Point(3, 3);
            detailDataGrid.Name = "detailDataGrid";
            detailDataGrid.ReadOnly = true;
            detailDataGrid.RowHeadersVisible = false;
            detailDataGrid.RowHeadersWidth = 62;
            detailDataGrid.RowTemplate.Height = 25;
            detailDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            detailDataGrid.Size = new Size(1062, 547);
            detailDataGrid.TabIndex = 0;
            //
            // DetailAppColumn
            //
            DetailAppColumn.DataPropertyName = "SourceProcess";
            DetailAppColumn.FillWeight = 70F;
            DetailAppColumn.HeaderText = "应用程序";
            DetailAppColumn.MinimumWidth = 8;
            DetailAppColumn.Name = "DetailAppColumn";
            DetailAppColumn.ReadOnly = true;
            //
            // DetailTimeColumn
            //
            DetailTimeColumn.DataPropertyName = "Timestamp";
            DetailTimeColumn.FillWeight = 80F;
            DetailTimeColumn.HeaderText = "时间";
            DetailTimeColumn.MinimumWidth = 8;
            DetailTimeColumn.Name = "DetailTimeColumn";
            DetailTimeColumn.ReadOnly = true;
            //
            // DetailTypeColumn
            //
            DetailTypeColumn.DataPropertyName = "ChangeType";
            DetailTypeColumn.FillWeight = 50F;
            DetailTypeColumn.HeaderText = "类型";
            DetailTypeColumn.MinimumWidth = 8;
            DetailTypeColumn.Name = "DetailTypeColumn";
            DetailTypeColumn.ReadOnly = true;
            //
            // DetailDirColumn
            //
            DetailDirColumn.DataPropertyName = "Directory";
            DetailDirColumn.FillWeight = 100F;
            DetailDirColumn.HeaderText = "目录";
            DetailDirColumn.MinimumWidth = 8;
            DetailDirColumn.Name = "DetailDirColumn";
            DetailDirColumn.ReadOnly = true;
            //
            // DetailFileColumn
            //
            DetailFileColumn.DataPropertyName = "FileName";
            DetailFileColumn.FillWeight = 70F;
            DetailFileColumn.HeaderText = "文件名称";
            DetailFileColumn.MinimumWidth = 8;
            DetailFileColumn.Name = "DetailFileColumn";
            DetailFileColumn.ReadOnly = true;
            //
            // titleLabel
            //
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            titleLabel.Location = new Point(12, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(200, 31);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "统计与提醒记录";
            //
            // closeBtn
            //
            closeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeBtn.Location = new Point(1004, 9);
            closeBtn.Name = "closeBtn";
            closeBtn.Size = new Size(84, 34);
            closeBtn.TabIndex = 2;
            closeBtn.Text = "关闭";
            closeBtn.Click += closeBtn_Click;
            //
            // Form4
            //
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 651);
            Controls.Add(closeBtn);
            Controls.Add(titleLabel);
            Controls.Add(mainTabControl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form4";
            StartPosition = FormStartPosition.CenterParent;
            Text = "统计与提醒记录";
            mainTabControl.ResumeLayout(false);
            notificationTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)notificationGrid).EndInit();
            statsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)statsGrid).EndInit();
            detailTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)detailDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl mainTabControl;
        private TabPage notificationTabPage;
        private TabPage statsTabPage;
        private TabPage detailTabPage;
        private DataGridView notificationGrid;
        private DataGridView statsGrid;
        private DataGridView detailDataGrid;
        private DataGridViewTextBoxColumn ProcessNameColumn;
        private DataGridViewTextBoxColumn OpCountColumn;
        private DataGridViewTextBoxColumn DurationColumn;
        private DataGridViewTextBoxColumn TriggerTimeColumn;
        private DataGridViewTextBoxColumn StatsAppColumn;
        private DataGridViewTextBoxColumn StatsCountColumn;
        private DataGridViewTextBoxColumn StatsFirstTimeColumn;
        private DataGridViewTextBoxColumn StatsLastTimeColumn;
        private DataGridViewTextBoxColumn DetailAppColumn;
        private DataGridViewTextBoxColumn DetailTimeColumn;
        private DataGridViewTextBoxColumn DetailTypeColumn;
        private DataGridViewTextBoxColumn DetailDirColumn;
        private DataGridViewTextBoxColumn DetailFileColumn;
        private Label titleLabel;
        private AntdUI.Button closeBtn;
    }
}
