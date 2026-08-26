namespace CdiskClean.Forms
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
            tree1 = new AntdUI.Tree();
            SuspendLayout();
            // 
            // tree1
            // 
            tree1.BlockNode = true;
            tree1.Checkable = true;
            tree1.CheckStrictly = false;
            tree1.Location = new Point(12, 12);
            tree1.Multiple = true;
            tree1.Name = "tree1";
            tree1.Radius = 2;
            tree1.Size = new Size(1155, 610);
            tree1.TabIndex = 0;
            tree1.Text = "tree1";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1461, 857);
            Controls.Add(tree1);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Tree tree1;
    }
}