namespace Skydat.forms2080
{
    partial class frmshowGraph2080
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmshowGraph2080));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.dschart1 = new Skydat.datasets.Dschart1();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnshowvalue = new System.Windows.Forms.Button();
            this.btnselectonly = new System.Windows.Forms.Button();
            this.btnzoomin = new System.Windows.Forms.Button();
            this.btnzoomout = new System.Windows.Forms.Button();
            this.btnshowgrid = new System.Windows.Forms.Button();
            this.btnshowaxes = new System.Windows.Forms.Button();
            this.tChart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsbstop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tsbshowd = new System.Windows.Forms.ToolStripButton();
            this.tsbsave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.sp1 = new System.IO.Ports.SerialPort(this.components);
            this.reSize1 = new LarcomAndYoung.Windows.Forms.ReSize(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dschart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).BeginInit();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView3
            // 
            this.treeView3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.treeView3.ForeColor = System.Drawing.Color.Black;
            this.treeView3.LineColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.treeView3, "treeView3");
            this.treeView3.Name = "treeView3";
            this.treeView3.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView3.Nodes")))});
            // 
            // dschart1
            // 
            this.dschart1.DataSetName = "Dschart1";
            this.dschart1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.btnshowvalue);
            this.panel4.Controls.Add(this.btnselectonly);
            this.panel4.Controls.Add(this.btnzoomin);
            this.panel4.Controls.Add(this.btnzoomout);
            this.panel4.Controls.Add(this.btnshowgrid);
            this.panel4.Controls.Add(this.btnshowaxes);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImage = global::Skydat.Properties.Resources.color;
            resources.ApplyResources(this.button1, "button1");
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnshowvalue
            // 
            this.btnshowvalue.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnshowvalue, "btnshowvalue");
            this.btnshowvalue.FlatAppearance.BorderSize = 0;
            this.btnshowvalue.Image = global::Skydat.Properties.Resources.cursor_icon;
            this.btnshowvalue.Name = "btnshowvalue";
            this.btnshowvalue.UseVisualStyleBackColor = false;
            this.btnshowvalue.Click += new System.EventHandler(this.btnshowvalue_Click);
            // 
            // btnselectonly
            // 
            this.btnselectonly.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnselectonly, "btnselectonly");
            this.btnselectonly.FlatAppearance.BorderSize = 0;
            this.btnselectonly.Image = global::Skydat.Properties.Resources.select_all_text_icon1;
            this.btnselectonly.Name = "btnselectonly";
            this.btnselectonly.UseVisualStyleBackColor = false;
            this.btnselectonly.Click += new System.EventHandler(this.btnselectonly_Click);
            // 
            // btnzoomin
            // 
            this.btnzoomin.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnzoomin, "btnzoomin");
            this.btnzoomin.FlatAppearance.BorderSize = 0;
            this.btnzoomin.Image = global::Skydat.Properties.Resources.zoom_in_icon;
            this.btnzoomin.Name = "btnzoomin";
            this.btnzoomin.UseVisualStyleBackColor = false;
            this.btnzoomin.Click += new System.EventHandler(this.btnzoomin_Click);
            // 
            // btnzoomout
            // 
            this.btnzoomout.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnzoomout, "btnzoomout");
            this.btnzoomout.FlatAppearance.BorderSize = 0;
            this.btnzoomout.Image = global::Skydat.Properties.Resources.zoom_out_icon__1_;
            this.btnzoomout.Name = "btnzoomout";
            this.btnzoomout.UseVisualStyleBackColor = false;
            this.btnzoomout.Click += new System.EventHandler(this.btnzoomout_Click);
            // 
            // btnshowgrid
            // 
            this.btnshowgrid.BackColor = System.Drawing.SystemColors.Control;
            this.btnshowgrid.BackgroundImage = global::Skydat.Properties.Resources.activity_grid21600;
            resources.ApplyResources(this.btnshowgrid, "btnshowgrid");
            this.btnshowgrid.FlatAppearance.BorderSize = 0;
            this.btnshowgrid.Name = "btnshowgrid";
            this.btnshowgrid.UseVisualStyleBackColor = false;
            this.btnshowgrid.Click += new System.EventHandler(this.btnshowgrid_Click);
            // 
            // btnshowaxes
            // 
            this.btnshowaxes.BackColor = System.Drawing.SystemColors.Control;
            this.btnshowaxes.BackgroundImage = global::Skydat.Properties.Resources.dimensions_icon;
            resources.ApplyResources(this.btnshowaxes, "btnshowaxes");
            this.btnshowaxes.FlatAppearance.BorderSize = 0;
            this.btnshowaxes.Name = "btnshowaxes";
            this.btnshowaxes.UseVisualStyleBackColor = false;
            this.btnshowaxes.Click += new System.EventHandler(this.btnshowaxes_Click);
            // 
            // OnlineChart
            // 
            this.tChart1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tChart1.BorderlineColor = System.Drawing.SystemColors.ActiveCaptionText;
            chartArea1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.tChart1.ChartAreas.Add(chartArea1);
            resources.ApplyResources(this.tChart1, "OnlineChart");
            this.tChart1.Name = "OnlineChart";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            series1.Name = "Series1";
            this.tChart1.Series.Add(series1);
            this.tChart1.Click += new System.EventHandler(this.tChart1_Click);
            this.tChart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseMove);
            this.tChart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseUp);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.toolStrip1);
            this.panel3.Controls.Add(this.treeView3);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // Parameters
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            resources.ApplyResources(this.label4, "Parameters");
            this.label4.Name = "Parameters";
            this.label4.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackColor = System.Drawing.Color.Gainsboro;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.tsbstop,
            this.toolStripLabel1,
            this.tsbshowd,
            this.tsbsave});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // tsbstop
            // 
            resources.ApplyResources(this.tsbstop, "tsbstop");
            this.tsbstop.Image = global::Skydat.Properties.Resources.Stop_icon;
            this.tsbstop.Margin = new System.Windows.Forms.Padding(0);
            this.tsbstop.Name = "tsbstop";
            this.tsbstop.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // tsbshowd
            // 
            resources.ApplyResources(this.tsbshowd, "tsbshowd");
            this.tsbshowd.Image = global::Skydat.Properties.Resources.checklist_icon;
            this.tsbshowd.Margin = new System.Windows.Forms.Padding(0);
            this.tsbshowd.Name = "tsbshowd";
            this.tsbshowd.Click += new System.EventHandler(this.tsbshowd_Click);
            // 
            // tsbsave
            // 
            this.tsbsave.Image = global::Skydat.Properties.Resources.Save;
            resources.ApplyResources(this.tsbsave, "tsbsave");
            this.tsbsave.Name = "tsbsave";
            this.tsbsave.Click += new System.EventHandler(this.tsbsave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // timer3
            // 
            this.timer3.Interval = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // shapeContainer1
            // 
            resources.ApplyResources(this.shapeContainer1, "shapeContainer1");
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            resources.ApplyResources(this.lineShape1, "lineShape1");
            // 
            // sp1
            // 
            this.sp1.BaudRate = 115200;
            this.sp1.ReadBufferSize = 524288;
            this.sp1.ReadTimeout = 5000;
            // 
            // reSize1
            // 
            this.reSize1.About = null;
            this.reSize1.AutoCenterFormOnLoad = false;
            this.reSize1.Enabled = true;
            this.reSize1.HostContainer = this;
            this.reSize1.InitialHostContainerHeight = 702D;
            this.reSize1.InitialHostContainerWidth = 884D;
            this.reSize1.Tag = null;
            // 
            // frmshowGraph2080
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.shapeContainer1);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmshowGraph2080";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphForm_FormClosing);
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.Enter += new System.EventHandler(this.GraphForm_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dschart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart Chart2;
        public System.Windows.Forms.TreeView treeView3;
        public datasets.Dschart1 dschart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.DataVisualization.Charting.Chart tChart1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        public System.IO.Ports.SerialPort sp1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbstop;
        private System.Windows.Forms.ToolStripButton tsbshowd;
        private System.Windows.Forms.ToolStripButton tsbsave;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnshowvalue;
        private System.Windows.Forms.Button btnselectonly;
        private System.Windows.Forms.Button btnzoomin;
        private System.Windows.Forms.Button btnzoomout;
        private System.Windows.Forms.Button btnshowgrid;
        private System.Windows.Forms.Button btnshowaxes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.Button button1;
        private LarcomAndYoung.Windows.Forms.ReSize reSize1;
        private System.Windows.Forms.RichTextBox label4;
    }
}