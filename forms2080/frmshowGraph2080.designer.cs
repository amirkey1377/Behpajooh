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
            this.tvTechniqueQueue = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OnlineChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmGraphStates = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemSmothState = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlChartTools = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnshowvalue = new System.Windows.Forms.Button();
            this.btnselectonly = new System.Windows.Forms.Button();
            this.btnzoomin = new System.Windows.Forms.Button();
            this.btnzoomout = new System.Windows.Forms.Button();
            this.btnshowgrid = new System.Windows.Forms.Button();
            this.btnshowaxes = new System.Windows.Forms.Button();
            this.pnl2 = new System.Windows.Forms.Panel();
            this.rtbLogger = new System.Windows.Forms.RichTextBox();
            this.pnlParam = new System.Windows.Forms.Panel();
            this.rtbParameters = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbstop = new System.Windows.Forms.ToolStripButton();
            this.tsbhold = new System.Windows.Forms.ToolStripButton();
            this.tsbshowd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.sp1 = new System.IO.Ports.SerialPort(this.components);
            this.timerResizeFrm = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OnlineChart)).BeginInit();
            this.cmGraphStates.SuspendLayout();
            this.pnlChartTools.SuspendLayout();
            this.pnl2.SuspendLayout();
            this.pnlParam.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvTechniqueQueue
            // 
            this.tvTechniqueQueue.BackColor = System.Drawing.Color.Gainsboro;
            resources.ApplyResources(this.tvTechniqueQueue, "tvTechniqueQueue");
            this.tvTechniqueQueue.ForeColor = System.Drawing.Color.Black;
            this.tvTechniqueQueue.LineColor = System.Drawing.Color.Silver;
            this.tvTechniqueQueue.Name = "tvTechniqueQueue";
            this.tvTechniqueQueue.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvTechniqueQueue.Nodes")))});
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.OnlineChart);
            this.panel1.Controls.Add(this.pnlChartTools);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // OnlineChart
            // 
            this.OnlineChart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            this.OnlineChart.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.OnlineChart.BorderlineColor = System.Drawing.SystemColors.ActiveCaptionText;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.OnlineChart.ChartAreas.Add(chartArea1);
            this.OnlineChart.ContextMenuStrip = this.cmGraphStates;
            resources.ApplyResources(this.OnlineChart, "OnlineChart");
            this.OnlineChart.Name = "OnlineChart";
            this.OnlineChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.Name = "live";
            this.OnlineChart.Series.Add(series1);
            this.OnlineChart.SuppressExceptions = true;
            this.OnlineChart.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.Normal;
            this.OnlineChart.Click += new System.EventHandler(this.tChart1_Click);
            this.OnlineChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseMove);
            this.OnlineChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseUp);
            // 
            // cmGraphStates
            // 
            this.cmGraphStates.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmGraphStates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemSmothState});
            this.cmGraphStates.Name = "contextMenuStrip1";
            resources.ApplyResources(this.cmGraphStates, "cmGraphStates");
            // 
            // itemSmothState
            // 
            this.itemSmothState.Checked = true;
            this.itemSmothState.CheckOnClick = true;
            this.itemSmothState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.itemSmothState.Name = "itemSmothState";
            resources.ApplyResources(this.itemSmothState, "itemSmothState");
            this.itemSmothState.Click += new System.EventHandler(this.itemSmothState_Click);
            // 
            // pnlChartTools
            // 
            this.pnlChartTools.BackColor = System.Drawing.Color.Transparent;
            this.pnlChartTools.Controls.Add(this.button1);
            this.pnlChartTools.Controls.Add(this.btnshowvalue);
            this.pnlChartTools.Controls.Add(this.btnselectonly);
            this.pnlChartTools.Controls.Add(this.btnzoomin);
            this.pnlChartTools.Controls.Add(this.btnzoomout);
            this.pnlChartTools.Controls.Add(this.btnshowgrid);
            this.pnlChartTools.Controls.Add(this.btnshowaxes);
            resources.ApplyResources(this.pnlChartTools, "pnlChartTools");
            this.pnlChartTools.Name = "pnlChartTools";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
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
            this.btnshowvalue.Name = "btnshowvalue";
            this.btnshowvalue.UseVisualStyleBackColor = false;
            this.btnshowvalue.Click += new System.EventHandler(this.btnshowvalue_Click);
            // 
            // btnselectonly
            // 
            this.btnselectonly.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnselectonly, "btnselectonly");
            this.btnselectonly.FlatAppearance.BorderSize = 0;
            this.btnselectonly.Name = "btnselectonly";
            this.btnselectonly.UseVisualStyleBackColor = false;
            this.btnselectonly.Click += new System.EventHandler(this.btnselectonly_Click);
            // 
            // btnzoomin
            // 
            this.btnzoomin.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnzoomin, "btnzoomin");
            this.btnzoomin.FlatAppearance.BorderSize = 0;
            this.btnzoomin.Name = "btnzoomin";
            this.btnzoomin.UseVisualStyleBackColor = false;
            this.btnzoomin.Click += new System.EventHandler(this.btnzoomin_Click);
            // 
            // btnzoomout
            // 
            this.btnzoomout.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnzoomout, "btnzoomout");
            this.btnzoomout.FlatAppearance.BorderSize = 0;
            this.btnzoomout.Name = "btnzoomout";
            this.btnzoomout.UseVisualStyleBackColor = false;
            this.btnzoomout.Click += new System.EventHandler(this.btnzoomout_Click);
            // 
            // btnshowgrid
            // 
            this.btnshowgrid.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnshowgrid, "btnshowgrid");
            this.btnshowgrid.FlatAppearance.BorderSize = 0;
            this.btnshowgrid.Name = "btnshowgrid";
            this.btnshowgrid.UseVisualStyleBackColor = false;
            this.btnshowgrid.Click += new System.EventHandler(this.btnshowgrid_Click);
            // 
            // btnshowaxes
            // 
            this.btnshowaxes.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.btnshowaxes, "btnshowaxes");
            this.btnshowaxes.FlatAppearance.BorderSize = 0;
            this.btnshowaxes.Name = "btnshowaxes";
            this.btnshowaxes.UseVisualStyleBackColor = false;
            this.btnshowaxes.Click += new System.EventHandler(this.btnshowaxes_Click);
            // 
            // pnl2
            // 
            this.pnl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnl2.Controls.Add(this.rtbLogger);
            this.pnl2.Controls.Add(this.pnlParam);
            this.pnl2.Controls.Add(this.tvTechniqueQueue);
            this.pnl2.Controls.Add(this.toolStrip1);
            resources.ApplyResources(this.pnl2, "pnl2");
            this.pnl2.Name = "pnl2";
            // 
            // rtbLogger
            // 
            this.rtbLogger.BackColor = System.Drawing.SystemColors.MenuText;
            this.rtbLogger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rtbLogger, "rtbLogger");
            this.rtbLogger.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rtbLogger.Name = "rtbLogger";
            // 
            // pnlParam
            // 
            this.pnlParam.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlParam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlParam.Controls.Add(this.rtbParameters);
            resources.ApplyResources(this.pnlParam, "pnlParam");
            this.pnlParam.Name = "pnlParam";
            // 
            // rtbParameters
            // 
            this.rtbParameters.BackColor = System.Drawing.Color.White;
            this.rtbParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rtbParameters, "rtbParameters");
            this.rtbParameters.Name = "rtbParameters";
            this.rtbParameters.ReadOnly = true;
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbstop,
            this.tsbhold,
            this.tsbshowd});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // tsbstop
            // 
            resources.ApplyResources(this.tsbstop, "tsbstop");
            this.tsbstop.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tsbstop.Margin = new System.Windows.Forms.Padding(0);
            this.tsbstop.Name = "tsbstop";
            this.tsbstop.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // tsbhold
            // 
            resources.ApplyResources(this.tsbhold, "tsbhold");
            this.tsbhold.ForeColor = System.Drawing.SystemColors.InfoText;
            this.tsbhold.Image = global::Skydat.Properties.Resources.Pause_icon;
            this.tsbhold.Margin = new System.Windows.Forms.Padding(0);
            this.tsbhold.Name = "tsbhold";
            this.tsbhold.Click += new System.EventHandler(this.tsbhold_Click);
            // 
            // tsbshowd
            // 
            this.tsbshowd.ForeColor = System.Drawing.SystemColors.MenuText;
            resources.ApplyResources(this.tsbshowd, "tsbshowd");
            this.tsbshowd.Margin = new System.Windows.Forms.Padding(0);
            this.tsbshowd.Name = "tsbshowd";
            this.tsbshowd.Click += new System.EventHandler(this.tsbshowd_Click);
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
            // timer
            // 
            this.timer.Interval = 1000;
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
            this.sp1.BaudRate = 57600;
            this.sp1.ReadBufferSize = 524288;
            this.sp1.ReceivedBytesThreshold = 4;
            // 
            // timerResizeFrm
            // 
            this.timerResizeFrm.Interval = 200;
            this.timerResizeFrm.Tick += new System.EventHandler(this.timerResizeFrm_Tick);
            // 
            // frmshowGraph2080
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnl2);
            this.Controls.Add(this.shapeContainer1);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmshowGraph2080";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphForm_FormClosing);
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.Enter += new System.EventHandler(this.GraphForm_Enter);
            this.Resize += new System.EventHandler(this.frmshowGraph2080_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OnlineChart)).EndInit();
            this.cmGraphStates.ResumeLayout(false);
            this.pnlChartTools.ResumeLayout(false);
            this.pnl2.ResumeLayout(false);
            this.pnlParam.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart Chart2;
        public System.Windows.Forms.TreeView tvTechniqueQueue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnl2;
        public System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer timer;
        public System.Windows.Forms.DataVisualization.Charting.Chart OnlineChart;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        public System.IO.Ports.SerialPort sp1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbstop;
        private System.Windows.Forms.ToolStripButton tsbshowd;
        private System.Windows.Forms.Panel pnlChartTools;
        private System.Windows.Forms.Button btnshowvalue;
        private System.Windows.Forms.Button btnselectonly;
        private System.Windows.Forms.Button btnzoomin;
        private System.Windows.Forms.Button btnzoomout;
        private System.Windows.Forms.Button btnshowgrid;
        private System.Windows.Forms.Button btnshowaxes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rtbParameters;
        private System.Windows.Forms.Timer timerResizeFrm;
        private System.Windows.Forms.RichTextBox rtbLogger;
        private System.Windows.Forms.ToolStripButton tsbhold;
        private System.Windows.Forms.Panel pnlParam;
        private System.Windows.Forms.ContextMenuStrip cmGraphStates;
        private System.Windows.Forms.ToolStripMenuItem itemSmothState;
    }
}