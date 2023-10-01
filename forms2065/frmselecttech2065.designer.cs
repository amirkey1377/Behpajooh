namespace Skydat.forms2065
{
    partial class frmselecttech2065
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmselecttech2065));
            this.panel6 = new System.Windows.Forms.Panel();
            this.toolstripChart = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.tsbRun = new System.Windows.Forms.ToolStripButton();
            this.colAA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQ1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQ2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel8 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pGrid2 = new System.Windows.Forms.PropertyGrid();
            this.checkCPsCTs = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMultiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveTechniquesparametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreTechniquesparametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pGrid = new System.Windows.Forms.PropertyGrid();
            this.dschart1 = new Skydat.datasets.Dschart1();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rd_low_cpc = new System.Windows.Forms.RadioButton();
            this.rd_hieght_cpc = new System.Windows.Forms.RadioButton();
            this.reSize1 = new LarcomAndYoung.Windows.Forms.ReSize(this.components);
            this.panel6.SuspendLayout();
            this.toolstripChart.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dschart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.toolstripChart);
            this.panel6.Name = "panel6";
            // 
            // toolstripChart
            // 
            this.toolstripChart.AllowDrop = true;
            resources.ApplyResources(this.toolstripChart, "toolstripChart");
            this.toolstripChart.BackColor = System.Drawing.Color.Transparent;
            this.toolstripChart.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbOpen,
            this.tsbSaveAll,
            this.tsbRun});
            this.toolstripChart.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolstripChart.Name = "toolstripChart";
            this.toolstripChart.TabStop = true;
            this.toolstripChart.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolstripChart_ItemClicked);
            // 
            // tsbNew
            // 
            resources.ApplyResources(this.tsbNew, "tsbNew");
            this.tsbNew.ForeColor = System.Drawing.Color.Black;
            this.tsbNew.Image = global::Skydat.Properties.Resources.eraser1;
            this.tsbNew.Margin = new System.Windows.Forms.Padding(0);
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbOpen
            // 
            resources.ApplyResources(this.tsbOpen, "tsbOpen");
            this.tsbOpen.ForeColor = System.Drawing.Color.Black;
            this.tsbOpen.Image = global::Skydat.Properties.Resources.OpenNew;
            this.tsbOpen.Margin = new System.Windows.Forms.Padding(0);
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Click += new System.EventHandler(this.restoreTechniquesparametersToolStripMenuItem_Click);
            // 
            // tsbSaveAll
            // 
            resources.ApplyResources(this.tsbSaveAll, "tsbSaveAll");
            this.tsbSaveAll.ForeColor = System.Drawing.Color.Black;
            this.tsbSaveAll.Image = global::Skydat.Properties.Resources.flappy;
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Click += new System.EventHandler(this.saveTechniquesparametersToolStripMenuItem_Click);
            // 
            // tsbRun
            // 
            resources.ApplyResources(this.tsbRun, "tsbRun");
            this.tsbRun.ForeColor = System.Drawing.Color.Black;
            this.tsbRun.Image = global::Skydat.Properties.Resources.Run1;
            this.tsbRun.Name = "tsbRun";
            this.tsbRun.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // colAA
            // 
            resources.ApplyResources(this.colAA, "colAA");
            this.colAA.Name = "colAA";
            this.colAA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colQ1
            // 
            resources.ApplyResources(this.colQ1, "colQ1");
            this.colQ1.Name = "colQ1";
            this.colQ1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colQ2
            // 
            resources.ApplyResources(this.colQ2, "colQ2");
            this.colQ2.Name = "colQ2";
            this.colQ2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // sfd
            // 
            this.sfd.InitialDirectory = "D:\\Shaemi\\Skydat\\Skydat\\bin\\Debug";
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel8.Controls.Add(this.label16);
            this.panel8.Controls.Add(this.radioButton7);
            this.panel8.Controls.Add(this.radioButton6);
            this.panel8.Controls.Add(this.radioButton5);
            this.panel8.Controls.Add(this.radioButton4);
            this.panel8.Controls.Add(this.radioButton3);
            this.panel8.Controls.Add(this.radioButton2);
            this.panel8.Controls.Add(this.radioButton1);
            this.panel8.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // radioButton7
            // 
            resources.ApplyResources(this.radioButton7, "radioButton7");
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton6
            // 
            resources.ApplyResources(this.radioButton6, "radioButton6");
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton5
            // 
            resources.ApplyResources(this.radioButton5, "radioButton5");
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.pGrid2);
            this.panel5.Controls.Add(this.checkCPsCTs);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // pGrid2
            // 
            this.pGrid2.BackColor = System.Drawing.Color.White;
            this.pGrid2.CategoryForeColor = System.Drawing.Color.Black;
            this.pGrid2.CommandsActiveLinkColor = System.Drawing.Color.Red;
            this.pGrid2.CommandsBackColor = System.Drawing.Color.White;
            this.pGrid2.CommandsForeColor = System.Drawing.Color.Black;
            this.pGrid2.HelpBackColor = System.Drawing.Color.White;
            this.pGrid2.HelpForeColor = System.Drawing.Color.Black;
            this.pGrid2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.pGrid2, "pGrid2");
            this.pGrid2.Name = "pGrid2";
            this.pGrid2.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pGrid2.ToolbarVisible = false;
            this.pGrid2.ViewBackColor = System.Drawing.Color.White;
            this.pGrid2.ViewForeColor = System.Drawing.Color.Black;
            this.pGrid2.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pGrid2_PropertyValueChanged);
            this.pGrid2.Click += new System.EventHandler(this.pGrid2_Click);
            // 
            // checkCPsCTs
            // 
            resources.ApplyResources(this.checkCPsCTs, "checkCPsCTs");
            this.checkCPsCTs.ForeColor = System.Drawing.Color.Black;
            this.checkCPsCTs.Name = "checkCPsCTs";
            this.checkCPsCTs.UseVisualStyleBackColor = true;
            this.checkCPsCTs.CheckedChanged += new System.EventHandler(this.checkCPsCTs_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // treeView2
            // 
            this.treeView2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.treeView2.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView2.ForeColor = System.Drawing.Color.Black;
            this.treeView2.LineColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.treeView2, "treeView2");
            this.treeView2.Name = "treeView2";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView2.Nodes")))});
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.copyOneToolStripMenuItem,
            this.copyMultiToolStripMenuItem,
            this.toolStripSeparator5,
            this.runToolStripMenuItem1,
            this.toolStripSeparator3,
            this.saveTechniquesparametersToolStripMenuItem,
            this.restoreTechniquesparametersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // copyOneToolStripMenuItem
            // 
            this.copyOneToolStripMenuItem.Name = "copyOneToolStripMenuItem";
            resources.ApplyResources(this.copyOneToolStripMenuItem, "copyOneToolStripMenuItem");
            this.copyOneToolStripMenuItem.Click += new System.EventHandler(this.copyOneToolStripMenuItem_Click);
            // 
            // copyMultiToolStripMenuItem
            // 
            this.copyMultiToolStripMenuItem.Name = "copyMultiToolStripMenuItem";
            resources.ApplyResources(this.copyMultiToolStripMenuItem, "copyMultiToolStripMenuItem");
            this.copyMultiToolStripMenuItem.Click += new System.EventHandler(this.copyMultiToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            resources.ApplyResources(this.runToolStripMenuItem1, "runToolStripMenuItem1");
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // saveTechniquesparametersToolStripMenuItem
            // 
            this.saveTechniquesparametersToolStripMenuItem.Name = "saveTechniquesparametersToolStripMenuItem";
            resources.ApplyResources(this.saveTechniquesparametersToolStripMenuItem, "saveTechniquesparametersToolStripMenuItem");
            this.saveTechniquesparametersToolStripMenuItem.Click += new System.EventHandler(this.saveTechniquesparametersToolStripMenuItem_Click);
            // 
            // restoreTechniquesparametersToolStripMenuItem
            // 
            this.restoreTechniquesparametersToolStripMenuItem.Name = "restoreTechniquesparametersToolStripMenuItem";
            resources.ApplyResources(this.restoreTechniquesparametersToolStripMenuItem, "restoreTechniquesparametersToolStripMenuItem");
            this.restoreTechniquesparametersToolStripMenuItem.Click += new System.EventHandler(this.restoreTechniquesparametersToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.ForeColor = System.Drawing.Color.Black;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.LineColor = System.Drawing.Color.Silver;
            this.treeView1.Name = "treeView1";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes")))});
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // pGrid
            // 
            this.pGrid.BackColor = System.Drawing.Color.White;
            this.pGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.pGrid.CommandsActiveLinkColor = System.Drawing.Color.Red;
            this.pGrid.CommandsBackColor = System.Drawing.Color.White;
            this.pGrid.CommandsForeColor = System.Drawing.Color.Black;
            this.pGrid.HelpBackColor = System.Drawing.Color.White;
            this.pGrid.HelpForeColor = System.Drawing.Color.Black;
            this.pGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.pGrid, "pGrid");
            this.pGrid.Name = "pGrid";
            this.pGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pGrid.ToolbarVisible = false;
            this.pGrid.ViewBackColor = System.Drawing.Color.White;
            this.pGrid.ViewForeColor = System.Drawing.Color.Black;
            this.pGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pGrid_PropertyValueChanged);
            this.pGrid.Click += new System.EventHandler(this.pGrid_Click);
            // 
            // dschart1
            // 
            this.dschart1.DataSetName = "Dschart1";
            this.dschart1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rd_low_cpc
            // 
            resources.ApplyResources(this.rd_low_cpc, "rd_low_cpc");
            this.rd_low_cpc.BackColor = System.Drawing.Color.White;
            this.rd_low_cpc.Checked = true;
            this.rd_low_cpc.ForeColor = System.Drawing.Color.Black;
            this.rd_low_cpc.Name = "rd_low_cpc";
            this.rd_low_cpc.TabStop = true;
            this.rd_low_cpc.UseVisualStyleBackColor = false;
            // 
            // rd_hieght_cpc
            // 
            resources.ApplyResources(this.rd_hieght_cpc, "rd_hieght_cpc");
            this.rd_hieght_cpc.BackColor = System.Drawing.Color.White;
            this.rd_hieght_cpc.ForeColor = System.Drawing.Color.Black;
            this.rd_hieght_cpc.Name = "rd_hieght_cpc";
            this.rd_hieght_cpc.UseVisualStyleBackColor = false;
            this.rd_hieght_cpc.CheckedChanged += new System.EventHandler(this.rd_hieght_cpc_CheckedChanged);
            // 
            // reSize1
            // 
            this.reSize1.About = null;
            this.reSize1.AutoCenterFormOnLoad = false;
            this.reSize1.Enabled = true;
            this.reSize1.HostContainer = this;
            this.reSize1.InitialHostContainerHeight = 603D;
            this.reSize1.InitialHostContainerWidth = 743D;
            this.reSize1.Tag = null;
            // 
            // frmselecttech2065
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.rd_hieght_cpc);
            this.Controls.Add(this.rd_low_cpc);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.treeView2);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.pGrid);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmselecttech2065";
            this.ShowIcon = false;
            this.TransparencyKey = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.panel6.ResumeLayout(false);
            this.toolstripChart.ResumeLayout(false);
            this.toolstripChart.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dschart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ToolStrip toolstripChart;
        public System.Windows.Forms.ToolStripButton tsbRun;
        public System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQ1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQ2;
        private System.Windows.Forms.ToolTip toolTip1;
     
      
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.CheckBox checkCPsCTs;
        public System.Windows.Forms.PropertyGrid pGrid;
        private datasets.Dschart1 dschart1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.PropertyGrid pGrid2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolStripMenuItem copyOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMultiToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveTechniquesparametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreTechniquesparametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton rd_low_cpc;
        private System.Windows.Forms.RadioButton rd_hieght_cpc;
        private LarcomAndYoung.Windows.Forms.ReSize reSize1;
    }
}

