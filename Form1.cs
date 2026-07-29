namespace CdiskClean
{
    public partial class HideButton : Form
    {
        public HideButton()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void WritedRecordStatusLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("当前未记录数据");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeStatusLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timeStatusLabel.Size = new System.Drawing.Size(150,24);
        }

        private void watchStatusLabel_Click(object sender, EventArgs e)
        {
            TabPageControl1.SelectTab(1);
        }
    }
}
