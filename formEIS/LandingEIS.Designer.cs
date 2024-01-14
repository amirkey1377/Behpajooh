
namespace Skydat.formEIS
{
    partial class LandingEIS
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.Open_port = new System.Windows.Forms.Button();
            this.Ts = new System.Windows.Forms.TextBox();
            this.Hs = new System.Windows.Forms.TextBox();
            this.Vmax = new System.Windows.Forms.TextBox();
            this.Vmin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CLR = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SerialPort = new System.IO.Ports.SerialPort(this.components);
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 15;
            // 
            // btnStart
            // 
            this.btnStart.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnStart.AutoSize = true;
            this.btnStart.Location = new System.Drawing.Point(139, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 91);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
            // 
            // Open_port
            // 
            this.Open_port.AutoSize = true;
            this.Open_port.Location = new System.Drawing.Point(12, 39);
            this.Open_port.Name = "Open_port";
            this.Open_port.Size = new System.Drawing.Size(121, 64);
            this.Open_port.TabIndex = 17;
            this.Open_port.Text = "Open_Port";
            this.Open_port.UseVisualStyleBackColor = true;
            this.Open_port.Click += new System.EventHandler(this.Open_port_Click);
            // 
            // Ts
            // 
            this.Ts.Location = new System.Drawing.Point(100, 280);
            this.Ts.Multiline = true;
            this.Ts.Name = "Ts";
            this.Ts.Size = new System.Drawing.Size(151, 29);
            this.Ts.TabIndex = 21;
            // 
            // Hs
            // 
            this.Hs.Location = new System.Drawing.Point(100, 225);
            this.Hs.Multiline = true;
            this.Hs.Name = "Hs";
            this.Hs.Size = new System.Drawing.Size(151, 29);
            this.Hs.TabIndex = 20;
            // 
            // Vmax
            // 
            this.Vmax.Location = new System.Drawing.Point(100, 174);
            this.Vmax.Multiline = true;
            this.Vmax.Name = "Vmax";
            this.Vmax.Size = new System.Drawing.Size(151, 29);
            this.Vmax.TabIndex = 19;
            // 
            // Vmin
            // 
            this.Vmin.Location = new System.Drawing.Point(100, 123);
            this.Vmin.Multiline = true;
            this.Vmin.Name = "Vmin";
            this.Vmin.Size = new System.Drawing.Size(151, 29);
            this.Vmin.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Ts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Hs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Vmax";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Vmin";
            // 
            // CLR
            // 
            this.CLR.Location = new System.Drawing.Point(12, 328);
            this.CLR.Name = "CLR";
            this.CLR.Size = new System.Drawing.Size(239, 60);
            this.CLR.TabIndex = 26;
            this.CLR.Text = "Chart_Reset";
            this.CLR.UseVisualStyleBackColor = true;
            this.CLR.Click += new System.EventHandler(this.CLR_Click_1);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(257, 12);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(531, 376);
            this.chart1.TabIndex = 27;
            this.chart1.Text = "chart1";
            // 
            // SerialPort
            // 
            this.SerialPort.PortName = "COM8";
            this.SerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived_1);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(691, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LandingEIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.CLR);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ts);
            this.Controls.Add(this.Hs);
            this.Controls.Add(this.Vmax);
            this.Controls.Add(this.Vmin);
            this.Controls.Add(this.Open_port);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.comboBox1);
            this.Name = "LandingEIS";
            this.Text = "LandingEIS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LandingEIS_FormClosing);
            this.Load += new System.EventHandler(this.LandingEIS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button Open_port;
        private System.Windows.Forms.TextBox Ts;
        private System.Windows.Forms.TextBox Hs;
        private System.Windows.Forms.TextBox Vmax;
        private System.Windows.Forms.TextBox Vmin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CLR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.IO.Ports.SerialPort SerialPort;
        private System.Windows.Forms.Button button1;
    }
}