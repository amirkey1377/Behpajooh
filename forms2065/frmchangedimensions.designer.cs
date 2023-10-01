namespace Skydat.forms2065
{
    partial class frmchangedimensions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmchangedimensions));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtxmin = new System.Windows.Forms.TextBox();
            this.txtxmax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtymax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtymin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Skydat.Properties.Resources.OK;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(120, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 33);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::Skydat.Properties.Resources.Exit;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Location = new System.Drawing.Point(203, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 33);
            this.button2.TabIndex = 1;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtxmax);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtxmin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(4, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X dimension";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(4, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(359, 42);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Min :";
            // 
            // txtxmin
            // 
            this.txtxmin.Location = new System.Drawing.Point(37, 23);
            this.txtxmin.Name = "txtxmin";
            this.txtxmin.Size = new System.Drawing.Size(100, 20);
            this.txtxmin.TabIndex = 0;
            this.txtxmin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtxmin_KeyPress);
            // 
            // txtxmax
            // 
            this.txtxmax.Location = new System.Drawing.Point(249, 23);
            this.txtxmax.Name = "txtxmax";
            this.txtxmax.Size = new System.Drawing.Size(100, 20);
            this.txtxmax.TabIndex = 1;
            this.txtxmax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtxmin_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Max :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtymax);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtymin);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(4, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y dimension";
            // 
            // txtymax
            // 
            this.txtymax.Location = new System.Drawing.Point(249, 23);
            this.txtymax.Name = "txtymax";
            this.txtymax.Size = new System.Drawing.Size(100, 20);
            this.txtymax.TabIndex = 1;
            this.txtymax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtxmin_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Max :";
            // 
            // txtymin
            // 
            this.txtymin.Location = new System.Drawing.Point(37, 23);
            this.txtymin.Name = "txtymin";
            this.txtymin.Size = new System.Drawing.Size(100, 20);
            this.txtymin.TabIndex = 0;
            this.txtymin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtxmin_KeyPress);
            // 
            // Parameters
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 26);
            this.label4.Name = "Parameters";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Min :";
            // 
            // frmchangedimensions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 158);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmchangedimensions";
            this.Text = "select type of file for exporting";
            this.Load += new System.EventHandler(this.frmchangedimensions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtxmax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtxmin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtymax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtymin;
        private System.Windows.Forms.Label label4;
    }
}