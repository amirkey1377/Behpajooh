namespace Skydat.forms2065
{
    partial class frmShowPeaks12065

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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDetect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbManualPeak = new System.Windows.Forms.RadioButton();
            this.rbAutoPeak = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 95);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(252, 451);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnDetect);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 95);
            this.panel1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(3, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 24);
            this.button1.TabIndex = 30;
            this.button1.Text = "Clear Result";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDetect
            // 
            this.btnDetect.Location = new System.Drawing.Point(160, 7);
            this.btnDetect.Name = "btnDetect";
            this.btnDetect.Size = new System.Drawing.Size(75, 23);
            this.btnDetect.TabIndex = 29;
            this.btnDetect.Text = "Detect";
            this.btnDetect.UseVisualStyleBackColor = true;
            this.btnDetect.Click += new System.EventHandler(this.btnDetect_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(160, 36);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbManualPeak);
            this.groupBox1.Controls.Add(this.rbAutoPeak);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 60);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // rbManualPeak
            // 
            this.rbManualPeak.AutoSize = true;
            this.rbManualPeak.Location = new System.Drawing.Point(9, 35);
            this.rbManualPeak.Name = "rbManualPeak";
            this.rbManualPeak.Size = new System.Drawing.Size(60, 17);
            this.rbManualPeak.TabIndex = 3;
            this.rbManualPeak.Text = "Manual";
            this.rbManualPeak.UseVisualStyleBackColor = true;
            this.rbManualPeak.CheckedChanged += new System.EventHandler(this.rbAutoPeak_CheckedChanged);
            // 
            // rbAutoPeak
            // 
            this.rbAutoPeak.AutoSize = true;
            this.rbAutoPeak.Checked = true;
            this.rbAutoPeak.Location = new System.Drawing.Point(9, 10);
            this.rbAutoPeak.Name = "rbAutoPeak";
            this.rbAutoPeak.Size = new System.Drawing.Size(72, 17);
            this.rbAutoPeak.TabIndex = 3;
            this.rbAutoPeak.TabStop = true;
            this.rbAutoPeak.Text = "Automatic";
            this.rbAutoPeak.UseVisualStyleBackColor = true;
            this.rbAutoPeak.CheckedChanged += new System.EventHandler(this.rbAutoPeak_CheckedChanged);
            // 
            // frmShowPeaks12065
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 546);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panel1);
            this.Name = "frmShowPeaks12065";
            this.Text = "Peak detecting window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShowPeaks_FormClosing);
            this.Load += new System.EventHandler(this.frmShowPeaks_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rbManualPeak;
        public System.Windows.Forms.RadioButton rbAutoPeak;
        private System.Windows.Forms.Button btnDetect;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button button1;
    }
}