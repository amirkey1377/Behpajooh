namespace Skydat.forms2080
{
    partial class frmRDECtrl2080

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
            this.components = new System.ComponentModel.Container();
            this.btnRunStop = new System.Windows.Forms.Button();
            this.txtBRate = new System.Windows.Forms.TextBox();
            this.cmbComport = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDesiredRPM = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.txtRealRPM = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.RDEPort = new System.IO.Ports.SerialPort(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRunStop
            // 
            this.btnRunStop.Location = new System.Drawing.Point(181, 88);
            this.btnRunStop.Name = "btnRunStop";
            this.btnRunStop.Size = new System.Drawing.Size(69, 23);
            this.btnRunStop.TabIndex = 0;
            this.btnRunStop.Text = "Run";
            this.btnRunStop.UseVisualStyleBackColor = true;
            this.btnRunStop.Click += new System.EventHandler(this.btnRunStop_Click);
            // 
            // txtBRate
            // 
            this.txtBRate.Enabled = false;
            this.txtBRate.Location = new System.Drawing.Point(283, 10);
            this.txtBRate.Name = "txtBRate";
            this.txtBRate.Size = new System.Drawing.Size(61, 20);
            this.txtBRate.TabIndex = 1;
            this.txtBRate.Text = "19200";
            this.txtBRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbComport
            // 
            this.cmbComport.FormattingEnabled = true;
            this.cmbComport.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8"});
            this.cmbComport.Location = new System.Drawing.Point(102, 10);
            this.cmbComport.Name = "cmbComport";
            this.cmbComport.Size = new System.Drawing.Size(69, 21);
            this.cmbComport.TabIndex = 2;
            this.cmbComport.Text = "COM2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port Com:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "BaudRate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Desired RPM";
            // 
            // Parameters
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 93);
            this.label4.Name = "Parameters";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Real RPM:";
            // 
            // txtDesiredRPM
            // 
            this.txtDesiredRPM.Location = new System.Drawing.Point(102, 64);
            this.txtDesiredRPM.Name = "txtDesiredRPM";
            this.txtDesiredRPM.Size = new System.Drawing.Size(69, 20);
            this.txtDesiredRPM.TabIndex = 4;
            this.txtDesiredRPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(181, 62);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(69, 22);
            this.btnSet.TabIndex = 0;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtRealRPM
            // 
            this.txtRealRPM.BackColor = System.Drawing.SystemColors.Info;
            this.txtRealRPM.Enabled = false;
            this.txtRealRPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtRealRPM.ForeColor = System.Drawing.Color.Indigo;
            this.txtRealRPM.Location = new System.Drawing.Point(102, 90);
            this.txtRealRPM.Margin = new System.Windows.Forms.Padding(5);
            this.txtRealRPM.MaxLength = 5;
            this.txtRealRPM.Name = "txtRealRPM";
            this.txtRealRPM.ReadOnly = true;
            this.txtRealRPM.Size = new System.Drawing.Size(69, 21);
            this.txtRealRPM.TabIndex = 4;
            this.txtRealRPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(274, 88);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(69, 23);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // RDEPort
            // 
            this.RDEPort.BaudRate = 19200;
            this.RDEPort.PortName = "COM2";
            this.RDEPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SPort_DataReceived);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Min Allowed RPM";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Max Allowed RPM";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.textBox1.ForeColor = System.Drawing.Color.Indigo;
            this.textBox1.Location = new System.Drawing.Point(102, 37);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(69, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Info;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.textBox2.ForeColor = System.Drawing.Color.Indigo;
            this.textBox2.Location = new System.Drawing.Point(284, 37);
            this.textBox2.Margin = new System.Windows.Forms.Padding(5);
            this.textBox2.MaxLength = 5;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(60, 21);
            this.textBox2.TabIndex = 4;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmRDECtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 115);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRealRPM);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtDesiredRPM);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbComport);
            this.Controls.Add(this.txtBRate);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRunStop);
            this.Name = "frmRDECtrl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RDE Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRDECtrl_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunStop;
        private System.Windows.Forms.TextBox txtBRate;
        private System.Windows.Forms.ComboBox cmbComport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDesiredRPM;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox txtRealRPM;
        private System.Windows.Forms.Button btnExit;
        private System.IO.Ports.SerialPort RDEPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

