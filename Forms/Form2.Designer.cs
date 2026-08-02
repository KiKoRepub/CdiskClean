namespace CdiskClean
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            listBoxLog = new ListBox();
            notifyIcon1 = new NotifyIcon(components);
            button1 = new Button();
            groupBox1 = new GroupBox();
            listView1 = new ListView();
            dataGridView1 = new DataGridView();
            personName = new DataGridViewTextBoxColumn();
            PersonSex = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // listBoxLog
            // 
            listBoxLog.FormattingEnabled = true;
            listBoxLog.ItemHeight = 24;
            listBoxLog.Location = new Point(65, 76);
            listBoxLog.Name = "listBoxLog";
            listBoxLog.Size = new Size(228, 124);
            listBoxLog.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // button1
            // 
            button1.Location = new Point(128, 294);
            button1.Name = "button1";
            button1.Size = new Size(207, 82);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Location = new Point(444, 70);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(435, 348);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "拖拽测试";
            // 
            // listView1
            // 
            listView1.AllowDrop = true;
            listView1.Location = new Point(29, 205);
            listView1.Name = "listView1";
            listView1.Size = new Size(378, 101);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.DragDrop += panel1_DragDrop;
            listView1.DragEnter += panel1_DragEnter;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { personName, PersonSex });
            dataGridView1.Location = new Point(6, 29);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(423, 132);
            dataGridView1.TabIndex = 0;
            dataGridView1.MouseDown += dataGridView1_MouseDown;
            dataGridView1.MouseMove += dataGridView1_MouseMove;
            dataGridView1.MouseUp += dataGridView1_MouseUp;
            // 
            // personName
            // 
            personName.DataPropertyName = "Name";
            personName.HeaderText = "姓名";
            personName.MinimumWidth = 8;
            personName.Name = "personName";
            personName.Width = 150;
            // 
            // PersonSex
            // 
            PersonSex.DataPropertyName = "Sex";
            PersonSex.HeaderText = "性别";
            PersonSex.MinimumWidth = 8;
            PersonSex.Name = "PersonSex";
            PersonSex.Width = 150;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 450);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(listBoxLog);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxLog;
        private NotifyIcon notifyIcon1;
        private Button button1;
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private ListView listView1;
        private DataGridViewTextBoxColumn personName;
        private DataGridViewTextBoxColumn PersonSex;
    }
}