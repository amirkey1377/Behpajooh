namespace Skydat.forms2065
{
    partial class frmofflinestate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmofflinestate));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tChart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolstripChart = new System.Windows.Forms.ToolStrip();
            this.tsbrun = new System.Windows.Forms.ToolStripButton();
            this.tsbReceive = new System.Windows.Forms.ToolStripButton();
            this.tsbshowdata = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnselimage = new System.Windows.Forms.Button();
            this.cmbPrtName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rowDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.sp1 = new System.IO.Ports.SerialPort(this.components);
            this.dschart11 = new Skydat.datasets.Dschart1();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.reSize1 = new LarcomAndYoung.Windows.Forms.ReSize(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.toolstripChart.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dschart11)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // tChart1
            // 
            this.tChart1.BackColor = System.Drawing.Color.Black;
            this.tChart1.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.tChart1.ChartAreas.Add(chartArea1);
            this.tChart1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tChart1, "tChart1");
            this.tChart1.Name = "tChart1";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.tChart1.Series.Add(series1);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.tChart1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Controls.Add(this.button4);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.Control;
            this.button4.BackgroundImage = global::Skydat.Properties.Resources.color;
            resources.ApplyResources(this.button4, "button4");
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.Name = "button4";
            this.toolTip1.SetToolTip(this.button4, resources.GetString("button4.ToolTip"));
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label4);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.toolstripChart);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // toolstripChart
            // 
            this.toolstripChart.AllowDrop = true;
            resources.ApplyResources(this.toolstripChart, "toolstripChart");
            this.toolstripChart.BackColor = System.Drawing.Color.Gainsboro;
            this.toolstripChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbrun,
            this.tsbReceive,
            this.tsbshowdata,
            this.tsbSaveAll});
            this.toolstripChart.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolstripChart.Name = "toolstripChart";
            this.toolstripChart.TabStop = true;
            // 
            // tsbrun
            // 
            resources.ApplyResources(this.tsbrun, "tsbrun");
            this.tsbrun.ForeColor = System.Drawing.Color.Black;
            this.tsbrun.Image = global::Skydat.Properties.Resources.Run;
            this.tsbrun.Margin = new System.Windows.Forms.Padding(0);
            this.tsbrun.Name = "tsbrun";
            this.tsbrun.Click += new System.EventHandler(this.tsbRun_Click);
            // 
            // tsbReceive
            // 
            resources.ApplyResources(this.tsbReceive, "tsbReceive");
            this.tsbReceive.ForeColor = System.Drawing.Color.Black;
            this.tsbReceive.Image = global::Skydat.Properties.Resources.Mailbox_receive_message_2_icon;
            this.tsbReceive.Margin = new System.Windows.Forms.Padding(0);
            this.tsbReceive.Name = "tsbReceive";
            // 
            // tsbshowdata
            // 
            resources.ApplyResources(this.tsbshowdata, "tsbshowdata");
            this.tsbshowdata.ForeColor = System.Drawing.Color.Black;
            this.tsbshowdata.Image = global::Skydat.Properties.Resources.checklist_icon;
            this.tsbshowdata.Margin = new System.Windows.Forms.Padding(0);
            this.tsbshowdata.Name = "tsbshowdata";
            this.tsbshowdata.Click += new System.EventHandler(this.tsbshowdata_Click);
            // 
            // tsbSaveAll
            // 
            resources.ApplyResources(this.tsbSaveAll, "tsbSaveAll");
            this.tsbSaveAll.ForeColor = System.Drawing.Color.Black;
            this.tsbSaveAll.Image = global::Skydat.Properties.Resources.flappy;
            this.tsbSaveAll.Margin = new System.Windows.Forms.Padding(0);
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Gainsboro;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.btnselimage);
            this.panel5.Controls.Add(this.cmbPrtName);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.panel6);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // btnselimage
            // 
            this.btnselimage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnselimage.BackgroundImage = global::Skydat.Properties.Resources.OK;
            resources.ApplyResources(this.btnselimage, "btnselimage");
            this.btnselimage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnselimage.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnselimage.FlatAppearance.BorderSize = 2;
            this.btnselimage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnselimage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnselimage.Name = "btnselimage";
            this.toolTip1.SetToolTip(this.btnselimage, resources.GetString("btnselimage.ToolTip"));
            this.btnselimage.UseVisualStyleBackColor = false;
            this.btnselimage.Click += new System.EventHandler(this.btnselimage_Click);
            // 
            // cmbPrtName
            // 
            this.cmbPrtName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbPrtName, "cmbPrtName");
            this.cmbPrtName.FormattingEnabled = true;
            this.cmbPrtName.Items.AddRange(new object[] {
            resources.GetString("cmbPrtName.Items"),
            resources.GetString("cmbPrtName.Items1"),
            resources.GetString("cmbPrtName.Items2"),
            resources.GetString("cmbPrtName.Items3"),
            resources.GetString("cmbPrtName.Items4"),
            resources.GetString("cmbPrtName.Items5"),
            resources.GetString("cmbPrtName.Items6"),
            resources.GetString("cmbPrtName.Items7"),
            resources.GetString("cmbPrtName.Items8"),
            resources.GetString("cmbPrtName.Items9"),
            resources.GetString("cmbPrtName.Items10"),
            resources.GetString("cmbPrtName.Items11"),
            resources.GetString("cmbPrtName.Items12"),
            resources.GetString("cmbPrtName.Items13"),
            resources.GetString("cmbPrtName.Items14"),
            resources.GetString("cmbPrtName.Items15"),
            resources.GetString("cmbPrtName.Items16"),
            resources.GetString("cmbPrtName.Items17"),
            resources.GetString("cmbPrtName.Items18"),
            resources.GetString("cmbPrtName.Items19")});
            this.cmbPrtName.Name = "cmbPrtName";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // rowDataGridViewTextBoxColumn1
            // 
            this.rowDataGridViewTextBoxColumn1.DataPropertyName = "row";
            resources.ApplyResources(this.rowDataGridViewTextBoxColumn1, "rowDataGridViewTextBoxColumn1");
            this.rowDataGridViewTextBoxColumn1.Name = "rowDataGridViewTextBoxColumn1";
            // 
            // xDataGridViewTextBoxColumn1
            // 
            this.xDataGridViewTextBoxColumn1.DataPropertyName = "x";
            resources.ApplyResources(this.xDataGridViewTextBoxColumn1, "xDataGridViewTextBoxColumn1");
            this.xDataGridViewTextBoxColumn1.Name = "xDataGridViewTextBoxColumn1";
            // 
            // yDataGridViewTextBoxColumn1
            // 
            this.yDataGridViewTextBoxColumn1.DataPropertyName = "y";
            resources.ApplyResources(this.yDataGridViewTextBoxColumn1, "yDataGridViewTextBoxColumn1");
            this.yDataGridViewTextBoxColumn1.Name = "yDataGridViewTextBoxColumn1";
            // 
            // y1DataGridViewTextBoxColumn
            // 
            this.y1DataGridViewTextBoxColumn.DataPropertyName = "y1";
            resources.ApplyResources(this.y1DataGridViewTextBoxColumn, "y1DataGridViewTextBoxColumn");
            this.y1DataGridViewTextBoxColumn.Name = "y1DataGridViewTextBoxColumn";
            // 
            // y2DataGridViewTextBoxColumn
            // 
            this.y2DataGridViewTextBoxColumn.DataPropertyName = "y2";
            resources.ApplyResources(this.y2DataGridViewTextBoxColumn, "y2DataGridViewTextBoxColumn");
            this.y2DataGridViewTextBoxColumn.Name = "y2DataGridViewTextBoxColumn";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // sp1
            // 
            this.sp1.ReadBufferSize = 524288;
            this.sp1.ReadTimeout = 5000;
            this.sp1.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.sp1_ErrorReceived);
            this.sp1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sp1_DataReceived);
            // 
            // dschart11
            // 
            this.dschart11.DataSetName = "Dschart1";
            this.dschart11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label2);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // reSize1
            // 
            this.reSize1.About = null;
            this.reSize1.AutoCenterFormOnLoad = false;
            this.reSize1.Enabled = true;
            this.reSize1.HostContainer = this;
            this.reSize1.InitialHostContainerHeight = 700D;
            this.reSize1.InitialHostContainerWidth = 901D;
            this.reSize1.Tag = null;
            // 
            // frmofflinestate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmofflinestate";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.toolstripChart.ResumeLayout(false);
            this.toolstripChart.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dschart11)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.DataVisualization.Charting.Chart tChart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn y1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn y2DataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStrip toolstripChart;
        private System.Windows.Forms.ToolStripButton tsbReceive;
        private System.Windows.Forms.ToolStripButton tsbrun;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton tsbshowdata;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        public System.Windows.Forms.ComboBox cmbPrtName;
        public System.Windows.Forms.Label label9;
        public System.IO.Ports.SerialPort sp1;
        private System.Windows.Forms.Button btnselimage;
        private datasets.Dschart1 dschart11;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel4;
        private LarcomAndYoung.Windows.Forms.ReSize reSize1;
    }
}