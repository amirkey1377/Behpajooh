
namespace Skydat.forms2065
{
    partial class frmChartSetting
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
            this.pnlConfiq = new System.Windows.Forms.Panel();
            this.gbAdvance = new System.Windows.Forms.GroupBox();
            this.txtSampleDelay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txtChartScale = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnColor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rbPoint = new System.Windows.Forms.RadioButton();
            this.rbCurve = new System.Windows.Forms.RadioButton();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chartColor = new System.Windows.Forms.ColorDialog();
            this.pnlConfiq.SuspendLayout();
            this.gbAdvance.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConfiq
            // 
            this.pnlConfiq.BackColor = System.Drawing.Color.LightGray;
            this.pnlConfiq.Controls.Add(this.gbAdvance);
            this.pnlConfiq.Controls.Add(this.groupBox2);
            this.pnlConfiq.Controls.Add(this.btnCancel);
            this.pnlConfiq.Controls.Add(this.btnOk);
            this.pnlConfiq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfiq.ForeColor = System.Drawing.Color.Transparent;
            this.pnlConfiq.Location = new System.Drawing.Point(0, 0);
            this.pnlConfiq.Name = "pnlConfiq";
            this.pnlConfiq.Size = new System.Drawing.Size(402, 254);
            this.pnlConfiq.TabIndex = 11;
            // 
            // gbAdvance
            // 
            this.gbAdvance.Controls.Add(this.txtSampleDelay);
            this.gbAdvance.Controls.Add(this.label5);
            this.gbAdvance.Controls.Add(this.label29);
            this.gbAdvance.Controls.Add(this.txtChartScale);
            this.gbAdvance.Enabled = false;
            this.gbAdvance.Font = new System.Drawing.Font("A Iranian Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.gbAdvance.Location = new System.Drawing.Point(9, 113);
            this.gbAdvance.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.gbAdvance.Name = "gbAdvance";
            this.gbAdvance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbAdvance.Size = new System.Drawing.Size(382, 91);
            this.gbAdvance.TabIndex = 14;
            this.gbAdvance.TabStop = false;
            this.gbAdvance.Text = "تنظیمات پیشرفته";
            // 
            // txtSampleDelay
            // 
            this.txtSampleDelay.BackColor = System.Drawing.SystemColors.Window;
            this.txtSampleDelay.Location = new System.Drawing.Point(212, 41);
            this.txtSampleDelay.Name = "txtSampleDelay";
            this.txtSampleDelay.Size = new System.Drawing.Size(100, 20);
            this.txtSampleDelay.TabIndex = 8;
            this.txtSampleDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSampleDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSampleDelay_KeyPress);
            this.txtSampleDelay.Validated += new System.EventHandler(this.txtSampleDelay_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(317, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "زمان نمونه\r\n برداری";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label29.Location = new System.Drawing.Point(126, 38);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(60, 26);
            this.label29.TabIndex = 11;
            this.label29.Text = "بازه نمایش\r\n(سیکل)";
            this.label29.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtChartScale
            // 
            this.txtChartScale.Location = new System.Drawing.Point(19, 41);
            this.txtChartScale.Name = "txtChartScale";
            this.txtChartScale.Size = new System.Drawing.Size(100, 20);
            this.txtChartScale.TabIndex = 9;
            this.txtChartScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChartScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSampleDelay_KeyPress);
            this.txtChartScale.Validated += new System.EventHandler(this.txtChartScale_Validated);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnColor);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.rbPoint);
            this.groupBox2.Controls.Add(this.rbCurve);
            this.groupBox2.Controls.Add(this.rbLine);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("A Iranian Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupBox2.Location = new System.Drawing.Point(9, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(383, 91);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "تنظیمات جدول";
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Transparent;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Location = new System.Drawing.Point(126, 61);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(75, 20);
            this.btnColor.TabIndex = 5;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(212, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "رنگ نمودار:";
            // 
            // rbPoint
            // 
            this.rbPoint.AutoSize = true;
            this.rbPoint.ForeColor = System.Drawing.Color.Black;
            this.rbPoint.Location = new System.Drawing.Point(37, 28);
            this.rbPoint.Name = "rbPoint";
            this.rbPoint.Size = new System.Drawing.Size(60, 17);
            this.rbPoint.TabIndex = 3;
            this.rbPoint.TabStop = true;
            this.rbPoint.Text = "نقطه‌ای";
            this.rbPoint.UseVisualStyleBackColor = true;
            this.rbPoint.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            // 
            // rbCurve
            // 
            this.rbCurve.AutoSize = true;
            this.rbCurve.ForeColor = System.Drawing.Color.Black;
            this.rbCurve.Location = new System.Drawing.Point(126, 28);
            this.rbCurve.Name = "rbCurve";
            this.rbCurve.Size = new System.Drawing.Size(55, 17);
            this.rbCurve.TabIndex = 2;
            this.rbCurve.TabStop = true;
            this.rbCurve.Text = "منحنی";
            this.rbCurve.UseVisualStyleBackColor = true;
            this.rbCurve.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            // 
            // rbLine
            // 
            this.rbLine.AutoSize = true;
            this.rbLine.ForeColor = System.Drawing.Color.Black;
            this.rbLine.Location = new System.Drawing.Point(211, 28);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(50, 17);
            this.rbLine.TabIndex = 1;
            this.rbLine.TabStop = true;
            this.rbLine.Text = "خطی";
            this.rbLine.UseVisualStyleBackColor = true;
            this.rbLine.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(290, 30);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "نحوه نمایش:";
            // 
            // btnCancel
            // 
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(104, 219);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Location = new System.Drawing.Point(219, 219);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chartColor
            // 
            this.chartColor.FullOpen = true;
            // 
            // frmChartSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(402, 254);
            this.Controls.Add(this.pnlConfiq);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmChartSetting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chart Setting";
            this.Load += new System.EventHandler(this.ChartSetting_Load);
            this.pnlConfiq.ResumeLayout(false);
            this.gbAdvance.ResumeLayout(false);
            this.gbAdvance.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlConfiq;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSampleDelay;
        private System.Windows.Forms.TextBox txtChartScale;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gbAdvance;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton rbPoint;
        public System.Windows.Forms.RadioButton rbCurve;
        public System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog chartColor;
        private System.Windows.Forms.Button btnColor;
    }
}