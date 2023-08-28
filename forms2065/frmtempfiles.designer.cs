namespace Skydat.forms2065
{
    partial class frmtempfiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmtempfiles));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label4 = new System.Windows.Forms.Label();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sortFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameascToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namedescToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateascToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datedescToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tChart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label41 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolstripChart = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbshowdata = new System.Windows.Forms.ToolStripButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.rowDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.reSize1 = new LarcomAndYoung.Windows.Forms.ReSize(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.toolstripChart.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label4.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // treeView3
            // 
            this.treeView3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.treeView3.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView3.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.treeView3, "treeView3");
            this.treeView3.Name = "treeView3";
            this.treeView3.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView3.Nodes")))});
            this.treeView3.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView3_AfterCheck);
            this.treeView3.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView3_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.sortFilesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // sortFilesToolStripMenuItem
            // 
            this.sortFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameascToolStripMenuItem,
            this.namedescToolStripMenuItem,
            this.dateascToolStripMenuItem,
            this.datedescToolStripMenuItem});
            this.sortFilesToolStripMenuItem.Name = "sortFilesToolStripMenuItem";
            resources.ApplyResources(this.sortFilesToolStripMenuItem, "sortFilesToolStripMenuItem");
            // 
            // nameascToolStripMenuItem
            // 
            this.nameascToolStripMenuItem.Name = "nameascToolStripMenuItem";
            resources.ApplyResources(this.nameascToolStripMenuItem, "nameascToolStripMenuItem");
            this.nameascToolStripMenuItem.Click += new System.EventHandler(this.nameascToolStripMenuItem_Click);
            // 
            // namedescToolStripMenuItem
            // 
            this.namedescToolStripMenuItem.Name = "namedescToolStripMenuItem";
            resources.ApplyResources(this.namedescToolStripMenuItem, "namedescToolStripMenuItem");
            this.namedescToolStripMenuItem.Click += new System.EventHandler(this.namedescToolStripMenuItem_Click);
            // 
            // dateascToolStripMenuItem
            // 
            this.dateascToolStripMenuItem.Name = "dateascToolStripMenuItem";
            resources.ApplyResources(this.dateascToolStripMenuItem, "dateascToolStripMenuItem");
            this.dateascToolStripMenuItem.Click += new System.EventHandler(this.dateascToolStripMenuItem_Click);
            // 
            // datedescToolStripMenuItem
            // 
            this.datedescToolStripMenuItem.Name = "datedescToolStripMenuItem";
            resources.ApplyResources(this.datedescToolStripMenuItem, "datedescToolStripMenuItem");
            this.datedescToolStripMenuItem.Click += new System.EventHandler(this.datedescToolStripMenuItem_Click);
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
            this.tChart1.Click += new System.EventHandler(this.tChart1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tChart1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.button4);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
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
            this.panel3.Controls.Add(this.label41);
            this.panel3.Controls.Add(this.treeView3);
            this.panel3.Controls.Add(this.label4);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label41
            // 
            this.label41.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label41.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label41.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
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
            this.tsbRefresh,
            this.tsbSaveAll,
            this.tsbDelete,
            this.tsbshowdata});
            this.toolstripChart.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolstripChart.Name = "toolstripChart";
            this.toolstripChart.TabStop = true;
            this.toolstripChart.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolstripChart_ItemClicked);
            // 
            // tsbRefresh
            // 
            resources.ApplyResources(this.tsbRefresh, "tsbRefresh");
            this.tsbRefresh.ForeColor = System.Drawing.Color.Black;
            this.tsbRefresh.Image = global::Skydat.Properties.Resources.refresh;
            this.tsbRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbSaveAll
            // 
            resources.ApplyResources(this.tsbSaveAll, "tsbSaveAll");
            this.tsbSaveAll.ForeColor = System.Drawing.Color.Black;
            this.tsbSaveAll.Image = global::Skydat.Properties.Resources.flappy;
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Click += new System.EventHandler(this.tsbSaveAll_Click);
            // 
            // tsbDelete
            // 
            resources.ApplyResources(this.tsbDelete, "tsbDelete");
            this.tsbDelete.ForeColor = System.Drawing.Color.Black;
            this.tsbDelete.Image = global::Skydat.Properties.Resources.delete1;
            this.tsbDelete.Margin = new System.Windows.Forms.Padding(0);
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbshowdata
            // 
            this.tsbshowdata.ForeColor = System.Drawing.Color.Black;
            this.tsbshowdata.Image = global::Skydat.Properties.Resources.Data_Sheet;
            resources.ApplyResources(this.tsbshowdata, "tsbshowdata");
            this.tsbshowdata.Name = "tsbshowdata";
            this.tsbshowdata.Click += new System.EventHandler(this.tsbshowdata_Click);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.panel6);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
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
            // frmtempfiles
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmtempfiles";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tChart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.toolstripChart.ResumeLayout(false);
            this.toolstripChart.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TreeView treeView3;
        public System.Windows.Forms.DataVisualization.Charting.Chart tChart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn y1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn y2DataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStrip toolstripChart;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton tsbshowdata;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sortFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameascToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namedescToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateascToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datedescToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel2;
        private LarcomAndYoung.Windows.Forms.ReSize reSize1;
    }
}