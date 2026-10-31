using Microsoft.Office.Interop.Excel;
using Skydat.classes2065;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Chart = System.Windows.Forms.DataVisualization.Charting.Chart;
using ChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace Skydat.forms2080
{
    public partial class frmanalyseGraph2080 : Form
    {
        public frmanalyseGraph2080()
        {
            InitializeComponent();

            tChart2.Titles.Clear();
            Title header1 = this.tChart2.Titles.Add("Header");
            header1.Alignment = ContentAlignment.TopLeft;
            header1.Docking = Docking.Top;
            this.tChart2.Titles[0].Text = "";
            this.tChart2.Titles[0].Alignment = ContentAlignment.TopCenter;
            this.tChart2.Titles[0].ForeColor = Color.Black;
            // OnlineChart.ChartAreas.Add("a1");
            //OnlineChart.Legends.Add("a1");

            tChart1.ChartAreas[0].Area3DStyle.Enable3D = false;

            tChart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            tChart1.ChartAreas[0].AxisX.LineColor = Color.Black;
            tChart1.ChartAreas[0].AxisY.LineColor = Color.Black;

            tChart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //OnlineChart.ChartAreas[0].AxisY.Title = "coulomb(c)";
            //OnlineChart.ChartAreas[0].AxisX.Title = "Time(s)";
            tChart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            tChart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

            tChart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            tChart1.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
            this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            this.tChart1.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Black;
            this.tChart1.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Black;

            //   tChart2.ChartAreas.Add("b1");
            // tChart2.Legends.Add("b1");

            tChart2.ChartAreas[0].Area3DStyle.Enable3D = false;

            tChart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            tChart2.ChartAreas[0].AxisX.LineColor = Color.Black;
            tChart2.ChartAreas[0].AxisY.LineColor = Color.Black;

            tChart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //tChart2.ChartAreas[0].AxisY.Title = "coulomb(c)";
            //tChart2.ChartAreas[0].AxisX.Title = "Time(s)";
            tChart2.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            tChart2.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

            tChart2.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            tChart2.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
            this.tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.tChart2.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.tChart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.tChart2.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            this.tChart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            this.tChart2.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            this.tChart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Black;
            this.tChart2.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Black;
            tChart2.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;
            tChart2.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Black;
            tChart2.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            tChart2.ChartAreas[0].AxisY.ScrollBar.Enabled = false;
        }
        public string operation = "";
        public System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public drawline_chart drawline1;
        private string Chart1_ActionState = "", statebutton_1 = "";

        public bool isAxesRotated = false;
        public byte tech = 0;
        public int num_Series = 0;
        double[] othertech_val0 = new double[200];
        double[] othertech_val1 = new double[200];
        private int shomar_val = 0;
        public int[] Overlay_tech = new int[1000];
        public int grfNo = 0, nindexchart2 = 0;
        public frmselecttech2080 sf;
        public frmMain12080 mainf;
        public frmOption12080 opForm;
        public clasMain mc = new clasMain();

        public frmShowPeaks12080 fPeak;

        public bool isSelectedLine = false;

        double[] x1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, y1 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            x2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, y2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        KeyEventArgs ee = new KeyEventArgs(Keys.Alt);
        private int cycles = 1;
        // private int Num_Graph_Point = 0, isReversed = 0;
        private void GraphForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[5].ToString().Substring(color[5].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                tChart1.BackColor = cl;
                tChart1.BorderlineColor = cl;
                tChart1.ChartAreas[0].BackColor = cl;
                tChart2.BackColor = cl;
                tChart2.BorderlineColor = cl;
                tChart2.ChartAreas[0].BackColor = cl;
                pnlGraph.BackColor = cl;
                panel2.BackColor = cl;
                panel8.BackColor = cl;

                s = color[8].ToString().Substring(color[8].ToString().IndexOf(":") + 1);
                cl = Color.FromArgb(Convert.ToInt32(s));
                for (int i = 0; i < tChart1.Series.Count; i++)
                    tChart1.Series[i].Color = cl;




            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

            drawline1 = new drawline_chart();

            if (opForm.cbDrawLine.Checked)
                Set_line_Setting();
            else
                drawline1.Active = false;

        }

        public void Set_line_Setting()
        {
            drawline1.EnableDraw = opForm.cbDrawLine.Checked;
            drawline1.pen_ch.Color = mainf.drawColor;
            switch (Convert.ToInt16(opForm.cmbStyleDraw.SelectedIndex))
            {
                case 0: drawline1.pen_ch.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case 1: drawline1.pen_ch.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case 2: drawline1.pen_ch.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                case 3: drawline1.pen_ch.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                case 4: drawline1.pen_ch.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                default: break;
            }
            drawline1.Active = true;
        }

        public void DrawLineSelectLine(drawline_chart sender)
        {
            try
            {
                if (isSelectedLine)
                {

                    if (mainf.Detect_Peak && fPeak.rbManualPeak.Checked)
                    {
                        double Step = Math.Abs(tChart1.Series[0].Points[1].XValue - tChart1.Series[0].Points[1].XValue);
                        int X1 = sender.FromPoint.X;
                        int Y1 = sender.FromPoint.Y;
                        int X2 = sender.ToPoint.X;
                        int Y2 = sender.ToPoint.Y;
                        var results1 = tChart1.HitTest(X1, Y1, false, ChartElementType.PlottingArea);
                        double X_V1 = 0, Y_V1 = 0, X_V2 = 0, Y_V2 = 0;
                        foreach (var result in results1)
                        {
                            if (result.ChartElementType == ChartElementType.PlottingArea)
                            {
                                X_V1 = result.ChartArea.AxisX.PixelPositionToValue(X1);
                                Y_V1 = result.ChartArea.AxisY.PixelPositionToValue(Y1);

                            }
                        }
                        var results2 = tChart1.HitTest(X2, Y2, false, ChartElementType.PlottingArea);
                        foreach (var result in results2)
                        {
                            if (result.ChartElementType == ChartElementType.PlottingArea)
                            {
                                X_V2 = result.ChartArea.AxisX.PixelPositionToValue(X2);
                                Y_V2 = result.ChartArea.AxisY.PixelPositionToValue(Y2);

                            }
                        }
                        double P1 = X_V1;
                        double P2 = X_V2;
                        double[] XX = new double[(int)(Math.Abs(P2 - P1) / Step)];
                        double[] YY = new double[(int)(Math.Abs(P2 - P1) / Step)];

                        if (X_V2 < X_V1)
                        {
                            P2 = X_V1;
                            P1 = X_V2;
                        }
                        int F = Math.Abs((int)(P1 / Step));
                        for (int i = 0; i < (int)(Math.Abs(P2 - P1) / Step); i++)
                        {
                            XX[i] = Convert.ToDouble(tChart1.Series[0].Points[F + i].XValue.ToString());
                            YY[i] = Convert.ToDouble(tChart1.Series[0].Points[F + i].YValues[0].ToString());
                        }
                        try
                        {

                            //fPeak.richTextBox1.Text += cPeak.manual(OnlineChart.Series[0].,XValues.Value, OnlineChart.Series[0].YValues.Value, X_V1, Y_V1, X_V2, Y_V2, (int)(Math.Abs(P2 - P1) / Step));
                            //fPeak.richTextBox1.Text += cPeak.findp1(XX, YY, (int)((P2 - P1) / Step));
                        }
                        catch
                        {
                        }
                    }
                }
                isSelectedLine = true;
            }
            catch { }
        }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (mainf.grfNum == 0);
            if (mainf.grfNum > 0)
                mainf.grfNum--;
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (e.Button == MouseButtons.Left)
                {
                    //double xv = tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X);
                    //double yv = tChart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y);
                    //if (xv - mDown > 0)
                    //    tChart1.ChartAreas[0].AxisX.ScaleView.Position = tChart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum + (difxview / 500);
                    //else if (xv - mDown < 0)
                    //    tChart1.ChartAreas[0].AxisX.ScaleView.Position = tChart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum - (difxview / 500);
                    //else
                    //    tChart1.ChartAreas[0].AxisX.ScaleView.Position = tChart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    //if ((yv - mDowny) > 0)
                    //    tChart1.ChartAreas[0].AxisY.ScaleView.Position = tChart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum + (difyview / 500);
                    //else if ((yv - mDowny) < 0)
                    //    tChart1.ChartAreas[0].AxisY.ScaleView.Position = tChart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum - (difyview / 500);
                    //else
                    //    tChart1.ChartAreas[0].AxisY.ScaleView.Position = tChart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                    //tChart1.ChartAreas[0].AxisX.ScaleView.Size = difxview; //(OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMaximum - OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMinimum); //+ xv - mDown;OnlineChart.ChartAreas[0].AxisX.Minimum +
                    //tChart1.ChartAreas[0].AxisY.ScaleView.Size = difyview;// (OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMaximum - OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) + yv - mDowny;//OnlineChart.ChartAreas[0].AxisX.Minimum +

                    //mDown = xv;
                    //mDowny = yv;

                }
                else
                {
                    var results11 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results11)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {

                            switch (Chart1_ActionState)
                            {
                                case "selectonly":
                                    tChart1.Cursor = System.Windows.Forms.Cursors.Cross;
                                    break;
                                case "showvalue":
                                    tChart1.Cursor = System.Windows.Forms.Cursors.SizeAll;

                                    break;
                                default:
                                    tChart1.Cursor = System.Windows.Forms.Cursors.Default;
                                    break;
                            }

                        }
                        else
                            tChart1.Cursor = System.Windows.Forms.Cursors.Default;


                    }
                    switch (Chart1_ActionState)
                    {
                        case "selectonly":
                            System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                            tChart1.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);
                            tChart1.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);

                            break;
                        case "showvalue":
                            var results12 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                            double x111 = 0, y111 = 0;
                            foreach (var result in results12)
                            {

                                if (result.ChartElementType == ChartElementType.PlottingArea)
                                {
                                    x111 = result.ChartArea.AxisX.PixelPositionToValue(e.X);
                                    y111 = result.ChartArea.AxisY.PixelPositionToValue(e.Y);
                                }
                            }
                            mainf.xyLabel.Text = x111.ToString("#.##") + "," + y111.ToString("#.#e-0");

                            break;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                drawline1.EnableDraw = true;
            if (e.KeyValue == 91)
            {


                chart.Dispose();

            }

        }

        private void tChart1_KeyUp(object sender, KeyEventArgs e)
        {
            drawline1.EnableDraw = false;
        }

        private void GraphForm_Enter(object sender, EventArgs e)
        {
            if (mainf != null)
                mainf.grf = (sender as frmanalyseGraph2080);
        }

        //private void tChart1_PostPaint(object sender, ChartPaintEventArgs e)
        //{
        //    Chart1_AfterDraw(sender);
        //}

        //private void Chart1_AfterDraw(object sender)
        //{
        //    mainf.Enable_Button(OnlineChart.Series.Count);

        //    //if (OnlineChart.Series.Count == 0) 
        //    //    OnlineChart.Titles[0].Text = "Skydat";
        //    if (OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled)
        //        ShowHideGrid.Text = "Hide Grids";
        //    else
        //        ShowHideGrid.Text = "Show Grids";
        //    if (OnlineChart.ChartAreas[0].AxisY.LabelStyle.Enabled)
        //        ShowHideAxes.Text = "Hide Axes";
        //    else
        //        ShowHideAxes.Text = "Show Axes";

        //    dimensionToolStripMenuItem.Checked = OnlineChart.ChartAreas[0].Area3DStyle.Enable3D;
        //}    


        //private void dimensionToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.toolStripButton15_Click(dimensionToolStripMenuItem, e);
        //}
        private void UpdateRotateButtonState()
        {
            bool hasData = false;

            // بررسی اینکه آیا tChart2 سری داده‌ای دارد یا خیر
            if (tChart2 != null && tChart2.Series != null)
            {
                foreach (var series in tChart2.Series)
                {
                    if (series.Points.Count > 0)
                    {
                        hasData = true;
                        break;
                    }
                }
            }

            // نام دکمه خود را جایگزین btnRotateAxes کنید
            if (btnRotateAxes != null)
            {
                btnRotateAxes.Enabled = hasData;
            }
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opForm.mf = this.mainf;
            opForm.gf = this;
            opForm.ShowDialog();
        }

        private void clearPlotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearselplot();
        }

        public void clearform()
        {
            for (int i = tvTechList.Nodes[0].Nodes.Count - 1; i >= 0; i--)
                tvTechList.Nodes[0].Nodes[i].Remove();
            label41.Text = "";
            label41.Text = "";
            for (int i = tChart1.Series.Count - 1; i >= 0; i--)
                tChart1.Series.RemoveAt(i);
            tChart1.Refresh();
            dschart2.analyselist.Clear();
            tChart2.Titles[0].Text = "";
            tChart2.Series.Clear();
            drawline1.removeall();
            drawline1.removeall_master();

            TechLine.Clear();
            UpdateMyNewButtonState();
        }

        private void treeView3_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 2)
            {
                ITechniques tech = TechLine[e.Node.Parent.Index];
                //جلوگیری از اتفاق event
                tvTechList.AfterCheck -= treeView3_AfterCheck;

                if (e.Node.Checked)
                {
                    tChart1.Series[$"{tech.Name}-{e.Node.Parent.Index}_{e.Node.Index + 1}"].Enabled = true;
                    e.Node.Parent.Checked = true;
                }
                else
                {
                    tChart1.Series[$"{tech.Name}-{e.Node.Parent.Index}_{e.Node.Index + 1}"].Enabled = false;
                    bool AllUnchecked = true;
                    foreach (TreeNode item in e.Node.Parent.Nodes)
                    {
                        if (item.Checked)
                        {
                            AllUnchecked = false;
                        }
                    }
                    if (AllUnchecked)
                        e.Node.Parent.Checked = false;
                }

                tvTechList.AfterCheck += treeView3_AfterCheck;

            }
            else if (e.Node.Level == 1)
            {
                ITechniques tech = TechLine[e.Node.Index];

                if (tech.Name == "CV")
                {
                    //جلوگیری از اتفاق event
                    tvTechList.AfterCheck -= treeView3_AfterCheck;

                    if (e.Node.Checked)
                    {
                        if (!IsTheAxisTitleSame(tech))
                        {
                            e.Node.Checked = false;
                            return;
                        }

                        foreach (TreeNode item in e.Node.Nodes)
                        {

                            item.Checked = true;
                            tChart1.Series[$"{tech.Name}-{e.Node.Index}_{item.Index + 1}"].Enabled = true;
                        }

                        tChart1.Series[$"{tech.Name}-{e.Node.Index}"].Enabled = true;

                    }
                    else
                    {
                        foreach (TreeNode item in e.Node.Nodes)
                        {
                            item.Checked = false;
                            tChart1.Series[$"{tech.Name}-{e.Node.Index}_{item.Index + 1}"].Enabled = false;
                        }
                        tChart1.Series[$"{tech.Name}-{e.Node.Index}"].Enabled = false;

                    }

                    tvTechList.AfterCheck += treeView3_AfterCheck;

                }
                else
                {
                    if (e.Node.Checked)
                    {
                        if (!IsTheAxisTitleSame(tech))
                        {
                            e.Node.Checked = false;
                            return;
                        }

                        tChart1.Series[$"{tech.Name}-{e.Node.Index}"].Enabled = true;
                    }
                    else
                    {
                        tChart1.Series[$"{tech.Name}-{e.Node.Index}"].Enabled = false;
                    }

                }
                tChart1.ChartAreas[0].RecalculateAxesScale();

                bool AllUnchecked = true;
                foreach (TreeNode item in e.Node.Parent.Nodes)
                {
                    if (item.Checked)
                    {
                        AllUnchecked = false;
                    }
                }
                if (AllUnchecked)
                {
                    tvTechList.AfterCheck -= treeView3_AfterCheck;
                    e.Node.Parent.Checked = false;
                    tChart1.ChartAreas[0].AxisX.Title = "";
                    tChart1.ChartAreas[0].AxisY.Title = "";
                    tvTechList.AfterCheck += treeView3_AfterCheck;
                }
            }
            else if (e.Node.Level == 0)
            {
                if (tvTechList.Nodes[0].Checked)
                {
                    for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                        tvTechList.Nodes[0].Nodes[i].Checked = true;
                }
                else
                {
                    for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                        tvTechList.Nodes[0].Nodes[i].Checked = false;

                    tChart1.ChartAreas[0].AxisX.Title = "";
                    tChart1.ChartAreas[0].AxisY.Title = "";
                }

            }



        }

        private bool IsTheAxisTitleSame(ITechniques tech)
        {
            string TitleX = tech.XY_Axis_Title[1];
            string TitleY = tech.XY_Axis_Title[0];

            string CurrentTitleX = tChart1.ChartAreas[0].AxisX.Title;
            string CurrentTitleY = tChart1.ChartAreas[0].AxisY.Title;

            if (CurrentTitleX == "" || CurrentTitleY == "")
            {
                CurrentTitleX = TitleX;
                CurrentTitleY = TitleY;
            }

            if (TitleX == CurrentTitleX & CurrentTitleY == TitleY)
            {
                tChart1.ChartAreas[0].AxisX.Title = TitleX;
                tChart1.ChartAreas[0].AxisY.Title = TitleY;

                return true;
            }
            else
            {
                if (TitleX != CurrentTitleX)
                    MessageBox.Show("Axis X Type is Not the Same");
                else
                    MessageBox.Show("Axis Y Type is Not the Same");

                return false;
            }

        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                //label4.Text = fillparam(1, e.Node.Index);
                //fillparamdata(e.Node.Index);
                ITechniques tech = TechLine[e.Node.Index];
                label41.Text = "";
                label41.Text = tech.FullName + "\n";
                label41.Text += "\n" + "Parameters:" + "\n";
                switch (tech.Number)
                {
                    case 1:
                        NPV npv = tech as NPV;
                        label41.Text += "E1 =" + npv.E1.Value + "\n";
                        label41.Text += "E2 =" + npv.E2.Value + "\n";
                        label41.Text += "Tstep =" + npv.TStep.Value + "\n";
                        label41.Text += "Hstep =" + npv.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + npv.ScanRate.Value + "\n";
                        label41.Text += "Pulse Width =" + npv.PulseWidth.Value + "\n";
                        label41.Text += "EquilibriumTime =" + npv.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + npv.Comment + "\n";
                        break;

                    case 2:
                        DPV dpv = tech as DPV;
                        label41.Text += "E1 =" + dpv.E1.Value + "\n";
                        label41.Text += "E2 =" + dpv.E2.Value + "\n";
                        label41.Text += "Tstep =" + dpv.TStep.Value + "\n";
                        label41.Text += "Hstep =" + dpv.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + dpv.ScanRate.Value + "\n";
                        label41.Text += "Pulse Width =" + dpv.PulseWidth.Value + "\n";
                        label41.Text += "Pulse Height =" + dpv.PulseHeight.Value + "\n";
                        label41.Text += "EquilibriumTime =" + dpv.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + dpv.Comment + "\n";
                        break;

                    case 3:
                        SWV swv = tech as SWV;
                        label41.Text += "E1 =" + swv.E1.Value + "\n";
                        label41.Text += "E2 =" + swv.E2.Value + "\n";
                        label41.Text += "Frequency =" + swv.Frequency.Value + "\n";
                        label41.Text += "Hstep =" + swv.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + swv.ScanRate.Value + "\n";
                        label41.Text += "Pulse Height =" + swv.PulseHeight.Value + "\n";
                        label41.Text += "EquilibriumTime =" + swv.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + swv.Comment + "\n";
                        break;

                    case 4:
                        CV cv = tech as CV;
                        label41.Text += "E1 =" + cv.E1.Value + "\n";
                        label41.Text += "E2 =" + cv.E2.Value + "\n";
                        label41.Text += "E3 =" + cv.E3.Value + "\n";
                        label41.Text += "Tstep =" + cv.TStep.Value + "\n";
                        label41.Text += "Hstep =" + cv.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + cv.ScanRate.Value + "\n";
                        label41.Text += "EquilibriumTime =" + cv.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + cv.Comment + "\n";
                        break;

                    case 5:
                        LSV lsv = tech as LSV;
                        label41.Text += "E1 =" + lsv.E1.Value + "\n";
                        label41.Text += "E2 =" + lsv.E2.Value + "\n";
                        label41.Text += "Tstep =" + lsv.TStep.Value + "\n";
                        label41.Text += "Hstep =" + lsv.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + lsv.ScanRate.Value + "\n";
                        label41.Text += "EquilibriumTime =" + lsv.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + lsv.Comment + "\n";
                        break;

                    case 8:
                        Tafel tfl = tech as Tafel;
                        label41.Text += "E1 =" + tfl.E1.Value + "\n";
                        label41.Text += "E2 =" + tfl.E2.Value + "\n";
                        label41.Text += "Tstep =" + tfl.TStep.Value + "\n";
                        label41.Text += "Hstep =" + tfl.HStep.Value + "\n";
                        label41.Text += "Scan Rate =" + tfl.ScanRate.Value + "\n";
                        label41.Text += "EquilibriumTime =" + tfl.EquilibriumTime + "\n";
                        label41.Text += "Comment =" + tfl.Comment + "\n";
                        break;

                    default:
                        break;
                }

            }
        }

        public void opengraph()
        {
            if (tvTechList.Nodes[0].Nodes.Count > 0)
            {
                switch (MessageBox.Show("Other graphs exist in this chart.Do you want to show new graph there?\n----------------------------------\nyes : show all graphs in this chart\nno : remove other graphs in this chart and show this graph only\ncancle : dont show this graph\n--------------------------------", "question", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        clearform();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DereferenceLinks = false;
            ofd.AutoUpgradeEnabled = false;
            ofd.Multiselect = true;
            ofd.DefaultExt = "csv";
            ofd.Filter = "Data Files(*.csv)|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    LoadCsvToChart(fileName, tChart1);
                }
            }

        }


        private void restore_Data_part(string fileName, int typedata1, int numberaddnode)
        {

            double[] xTmp = { 0 };
            double[] yTmp = { 0 };
            string s = "", fs = "";
            bool isData = false;
            int row = 0;
            if (fileName != "")
                fs = fileName;
            StreamReader Fl;

            EventArgs e = new EventArgs();
            if (fs != "")
            {
                Fl = new StreamReader(fs, Encoding.ASCII);

                try
                {
                    while (!Fl.EndOfStream)
                    {
                        s = Fl.ReadLine();
                        if (!isData)
                        {

                            if (s.LastIndexOf("Potential") >= 0 || s.LastIndexOf("========") >= 0)
                            {
                                isData = true;
                                s = Fl.ReadLine();
                                shomar_val = 0;
                                Array.Clear(othertech_val0, 0, othertech_val0.Length);
                                Array.Clear(othertech_val1, 0, othertech_val1.Length);
                            }
                        }
                        else                // if (isData)
                        {
                            int l = 0;
                            string sss = "";
                            int num = 0;
                            double x = 0, y = 0, z = 0, w = 0;
                            while (l < s.Length)
                            {

                                if (s[l] != '\t' && s[l] != ';' && s[l] != ' ')
                                    sss = sss + s[l];
                                if ((s[l] == '\t' || s[l] == ';' || s[l] == ' ') && sss != "")
                                {
                                    while (l < s.Length && s[l] == ' ') l++;
                                    if (num == 0) x = Convert.ToDouble(sss); //this.grf.dataE[num_data + num_ser, row] //890220
                                    if (num == 1) y = Convert.ToDouble(sss); //this.grf.dataI1[num_data + num_ser, row] = Convert.ToDouble(sss);
                                    if (num == 2) z = Convert.ToDouble(sss); //this.grf.dataI2[num_data + num_ser, row] = Convert.ToDouble(sss);
                                    if (num == 3) w = Convert.ToDouble(sss); //this.grf.dataI2[num_data + num_ser, row] = Convert.ToDouble(sss);

                                    sss = "";
                                    num++;
                                }
                                l++;
                            }

                            if (num == 3 && sss.Trim() != "")
                                w = Convert.ToDouble(sss);
                            if (shomar_val >= othertech_val0.Length)
                            {
                                Array.Resize(ref othertech_val0, othertech_val0.Length + 10);
                                Array.Resize(ref othertech_val1, othertech_val1.Length + 10);

                            }
                            othertech_val0[shomar_val] = x;
                            switch (typedata1)
                            {
                                case 1:

                                    othertech_val1[shomar_val] = y;
                                    break;
                                case 2:
                                    othertech_val1[shomar_val] = z;

                                    break;
                                case 3:
                                    othertech_val1[shomar_val] = w;

                                    break;

                            }
                            shomar_val++;
                            row++;
                            if (row >= 10000) row = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "selected File is not in valid Format...", "Warning");
                    //if (overlayopen == 1 || this.grf.OnlineChart.Series.Count < 1)
                    //    this.grf.Close();

                    //isOkfile = false;
                }
                Fl.Close();


            }

        }

        private string fillparam(int typecall, int row)
        {
            string str_fillparam = "";
            try
            {

                if (typecall == 1)
                {
                    str_fillparam = "Properties :" + Environment.NewLine;
                    label41.Text = "";
                    label41.Text = "Technique name :" + clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[row][0].ToString())];
                }
                else
                {
                    str_fillparam = "Technique name :" + clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[row][0].ToString())] + "\n\n" + "Properties :" + "\n";
                }

                switch (byte.Parse(dschart2.analyselist.Rows[row][0].ToString()))
                {
                    case clasglobal.NPV2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][4].ToString()) != 9999) str_fillparam = str_fillparam + "HStep =" + double.Parse(dschart2.analyselist.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][8].ToString()) != 9999) str_fillparam = str_fillparam + "Pulse Width =" + double.Parse(dschart2.analyselist.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][6].ToString()) != 9999) str_fillparam = str_fillparam + "Scan Rate =" + double.Parse(dschart2.analyselist.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][7].ToString()) != 9999) str_fillparam = str_fillparam + "TStep =" + double.Parse(dschart2.analyselist.Rows[row][7].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;
                    case clasglobal.DPV2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][4].ToString()) != 9999) str_fillparam = str_fillparam + "HStep =" + double.Parse(dschart2.analyselist.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][8].ToString()) != 9999) str_fillparam = str_fillparam + "Pulse Width =" + double.Parse(dschart2.analyselist.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][9].ToString()) != 9999) str_fillparam = str_fillparam + "Pulse Height =" + double.Parse(dschart2.analyselist.Rows[row][9].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][6].ToString()) != 9999) str_fillparam = str_fillparam + "Scan Rate =" + double.Parse(dschart2.analyselist.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][7].ToString()) != 9999) str_fillparam = str_fillparam + "TStep =" + double.Parse(dschart2.analyselist.Rows[row][7].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;
                    case clasglobal.SWV2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][4].ToString()) != 9999) str_fillparam = str_fillparam + "HStep =" + double.Parse(dschart2.analyselist.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][9].ToString()) != 9999) str_fillparam = str_fillparam + "Pulse Height =" + double.Parse(dschart2.analyselist.Rows[row][9].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][6].ToString()) != 9999) str_fillparam = str_fillparam + "Scan Rate =" + double.Parse(dschart2.analyselist.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][10].ToString()) != 9999) str_fillparam = str_fillparam + "Frequency =" + double.Parse(dschart2.analyselist.Rows[row][10].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;
                    case clasglobal.CV2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][3].ToString()) != 9999) str_fillparam = str_fillparam + "E3 =" + double.Parse(dschart2.analyselist.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][4].ToString()) != 9999) str_fillparam = str_fillparam + "HStep =" + double.Parse(dschart2.analyselist.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][6].ToString()) != 9999) str_fillparam = str_fillparam + "Scan Rate =" + double.Parse(dschart2.analyselist.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][7].ToString()) != 9999) str_fillparam = str_fillparam + "TStep =" + double.Parse(dschart2.analyselist.Rows[row][7].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;
                    case clasglobal.LSV2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][4].ToString()) != 9999) str_fillparam = str_fillparam + "HStep =" + double.Parse(dschart2.analyselist.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][6].ToString()) != 9999) str_fillparam = str_fillparam + "Scan Rate =" + double.Parse(dschart2.analyselist.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][7].ToString()) != 9999) str_fillparam = str_fillparam + "TStep =" + double.Parse(dschart2.analyselist.Rows[row][7].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;

                    case clasglobal.CPC2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][13].ToString()) != 9999) str_fillparam = str_fillparam + "T1 =" + double.Parse(dschart2.analyselist.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        //if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) //str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";

                        break;
                    case clasglobal.CCC2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][13].ToString()) != 9999) str_fillparam = str_fillparam + "T1 =" + double.Parse(dschart2.analyselist.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";
                        //if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) //str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        break;
                    case clasglobal.CHP2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][15].ToString()) != 9999) str_fillparam = str_fillparam + "I1 =" + double.Parse(dschart2.analyselist.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][16].ToString()) != 9999) str_fillparam = str_fillparam + "I2 =" + double.Parse(dschart2.analyselist.Rows[row][16].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][13].ToString()) != 9999) str_fillparam = str_fillparam + "T1 =" + double.Parse(dschart2.analyselist.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][14].ToString()) != 9999) str_fillparam = str_fillparam + "T2 =" + double.Parse(dschart2.analyselist.Rows[row][14].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";
                        //if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        break;
                    case clasglobal.CHA2080:
                        if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][13].ToString()) != 9999) str_fillparam = str_fillparam + "T1 =" + double.Parse(dschart2.analyselist.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][14].ToString()) != 9999) str_fillparam = str_fillparam + "T2 =" + double.Parse(dschart2.analyselist.Rows[row][14].ToString()) + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        str_fillparam = str_fillparam + "vs_OCP =" + dschart2.analyselist.Rows[row][19].ToString() + "\n";
                        if (double.Parse(dschart2.analyselist.Rows[row][20].ToString()) != 9999) str_fillparam = str_fillparam + "OCPmeas =" + double.Parse(dschart2.analyselist.Rows[row][20].ToString()) + "\n";
                        //if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        break;
                        //case clasglobal.CHC2080:
                        //    if (double.Parse(dschart2.analyselist.Rows[row][1].ToString()) != 9999) str_fillparam = str_fillparam + "E1 =" + double.Parse(dschart2.analyselist.Rows[row][1].ToString()) + "\n";
                        //    if (double.Parse(dschart2.analyselist.Rows[row][2].ToString()) != 9999) str_fillparam = str_fillparam + "E2 =" + double.Parse(dschart2.analyselist.Rows[row][2].ToString()) + "\n";
                        //    if (double.Parse(dschart2.analyselist.Rows[row][13].ToString()) != 9999) str_fillparam = str_fillparam + "T1 =" + double.Parse(dschart2.analyselist.Rows[row][13].ToString()) + "\n";
                        //    if (double.Parse(dschart2.analyselist.Rows[row][14].ToString()) != 9999) str_fillparam = str_fillparam + "T2 =" + double.Parse(dschart2.analyselist.Rows[row][14].ToString()) + "\n";
                        //    if (double.Parse(dschart2.analyselist.Rows[row][5].ToString()) != 9999) str_fillparam = str_fillparam + "Equilibrium Time =" + double.Parse(dschart2.analyselist.Rows[row][5].ToString()) + "\n";
                        //    //if (double.Parse(dschart2.analyselist.Rows[row][11].ToString()) != 9999) str_fillparam = str_fillparam + "Cycles =" + double.Parse(dschart2.analyselist.Rows[row][11].ToString()) + "\n";
                        //    break;

                }

                //pGrid.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return str_fillparam;
        }


        //private void changeaxxy_dimension(double dimy) {
        //    double maxaxx = 0, minaxx = 0, shomartech = 0, maxaxy = 0, minaxy = 0;
        //    for (int ii = 0; ii < treeView3.Nodes[0].Nodes.Count; ii++)
        //    {
        //        if (treeView3.Nodes[0].Nodes[ii].Checked)
        //        {

        //            if (shomartech == 0)
        //            {
        //                if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) <= 8 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.LSV2080 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CPC2080 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CCC2080)
        //                {
        //                    maxaxx = double.Parse(dschart2.analyselist.Rows[ii][1].ToString());
        //                    minaxx = maxaxx;
        //                }
        //                else if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CHA2080 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CHP2080 )
        //                {
        //                    maxaxx = double.Parse(dschart2.analyselist.Rows[ii][13].ToString()) ;
        //                    minaxx = maxaxx;
        //                }
        //                maxaxy = OnlineChart.Series[ii].Points.FindMaxByValue("y1").YValues[0];
        //                minaxy = OnlineChart.Series[ii].Points.FindMinByValue("y1").YValues[0];
        //                shomartech++;

        //            }

        //            if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CV2080)
        //            {
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][1].ToString()));
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][2].ToString()));
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][5].ToString()));
        //                minaxx = Math.Min(minaxx, double.Parse(dschart2.analyselist.Rows[ii][1].ToString()));
        //                minaxx = Math.Min(minaxx, double.Parse(dschart2.analyselist.Rows[ii][2].ToString()));
        //                minaxx = Math.Min(minaxx, double.Parse(dschart2.analyselist.Rows[ii][5].ToString()));
        //            }
        //            else if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) <= 8 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.LSV2080)
        //            {
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][1].ToString()));
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][2].ToString()));
        //                minaxx = Math.Min(minaxx, double.Parse(dschart2.analyselist.Rows[ii][1].ToString()));
        //                minaxx = Math.Min(minaxx, double.Parse(dschart2.analyselist.Rows[ii][2].ToString()));

        //            }
        //            else if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CPC2080)
        //            {
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][1].ToString()));
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][13].ToString()));
        //                minaxx = Math.Min(minaxx, 0);


        //            }
        //            else if (double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CHA2080 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == 11 || double.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.CHP2080)
        //            {
        //                maxaxx = Math.Max(maxaxx, double.Parse(dschart2.analyselist.Rows[ii][13].ToString()) );
        //                minaxx = Math.Min(minaxx, 0);
        //            }
        //            maxaxy = Math.Max(maxaxy, OnlineChart.Series[ii].Points.FindMaxByValue("y1").YValues[0]);
        //            minaxy = Math.Min(minaxy, OnlineChart.Series[ii].Points.FindMinByValue("y1").YValues[0]);

        //        }


        //    }

        //        OnlineChart.ChartAreas[0].AxisX.Maximum = maxaxx;
        //        OnlineChart.ChartAreas[0].AxisX.Minimum = minaxx;
        //        OnlineChart.ChartAreas[0].AxisX.Interval = Math.Round((maxaxx - minaxx) / 10, 1);
        //        OnlineChart.ChartAreas[0].AxisY.Maximum = maxaxy*dimy;
        //        OnlineChart.ChartAreas[0].AxisY.Minimum = minaxy*dimy;
        //        OnlineChart.ChartAreas[0].AxisY.Interval = Math.Round((maxaxy - minaxy)*dimy / 10, 1);



        //}
        public void bgsubtractionf()
        {
            operation = "Overlay";
            int shomarcheck = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                    shomarcheck++;
            if (shomarcheck == 2)
            {
                int N1 = -1, N2 = -1;
                for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                    if (tvTechList.Nodes[0].Nodes[i].Checked)
                        if (N1 == -1) N1 = i;
                        else N2 = i;
                int count1 = tChart1.Series[N1].Points.Count;
                int count2 = tChart1.Series[N2].Points.Count;
                int count = 0;
                if (count1 > count2) count = count2;
                else count = count1;
                double[] x = new double[count];
                double[] y = new double[count];
                for (int i = 0; i < count; i++)
                {
                    x[i] = tChart1.Series[N1].Points[i].XValue;
                    y[i] = Math.Abs(tChart1.Series[N2].Points[i].YValues[0] - tChart1.Series[N1].Points[i].YValues[0]);
                }
                show_Graph(0, this, "SubChart", x, y, true, count);
            }
            else
                MessageBox.Show("Overlay must be checked \n Number of open graphs must be equal 2");

        }
        public void show_Graph(int num_ser, frmanalyseGraph2080 grfn, string main_subchart, double[] xx, double[] yy, bool overlaymain, int shomaresh)
        {
            if (splitContainer.Height != 0)
            {
                splitContainer.SplitterDistance = (int)splitContainer.Height / 2;
            }
            int L = 0;
            if (xx.Length != yy.Length) // Convert bigest Array to smallest if length of x and y is not equal
            {
                L = xx.Length;
                if (yy.Length < L) L = yy.Length;
            }
            if (L == 0)
            {
                Array.Resize(ref xx, shomaresh);
                Array.Resize(ref yy, shomaresh);
            }
            if (main_subchart == "MainChart")
            {
                Set_Graph(0, true, overlaymain);
                tChart1.Series[num_Series].Points.DataBindXY(xx, yy);
            }
            if (main_subchart == "SubChart")
            {
                if (operation == "i_Reverse" || operation == "i_Forward" || operation == "Integral" || operation == "Derivation" || operation == "Mathematics" || operation == "Overlay" || operation == "Tafel" || operation == "show_without_smoothing")
                {
                    tChart2.Series.Clear();
                    tChart2.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());
                    tChart2.Series[0].ChartType = SeriesChartType.FastLine;
                    tChart2.Titles[0].Text = operation;

                    // ★★★ اضافه کردن نام محورها از tChart1 به tChart2 ★★★
                    tChart2.ChartAreas[0].AxisX.Title = tChart1.ChartAreas[0].AxisX.Title;
                    tChart2.ChartAreas[0].AxisY.Title = tChart1.ChartAreas[0].AxisY.Title;
                    // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

                    this.tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled = this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled;
                    this.tChart2.ChartAreas[0].AxisX.MinorGrid.Enabled = this.tChart1.ChartAreas[0].AxisX.MinorGrid.Enabled;
                    this.tChart2.ChartAreas[0].AxisY.MajorGrid.Enabled = this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled;
                    this.tChart2.ChartAreas[0].AxisY.MinorGrid.Enabled = this.tChart1.ChartAreas[0].AxisY.MinorGrid.Enabled;

                    tChart2.ChartAreas[0].AxisX.LabelStyle.Format = "#.#e-0";
                    tChart2.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
                    if (operation == "i_Forward")
                        smootht4(0, tChart2.Series[0].Color, 2, 3);
                    else if (operation == "i_Reverse")
                        smootht4(0, tChart2.Series[0].Color, 2, 4);
                    else
                        tChart2.Series[0].Points.DataBindXY(xx, yy);
                    tChart2.Refresh();
                    UpdateMyNewButtonState();
                }
            }
        }

        public void Set_Graph(int num, bool isFromOpen, bool overlaymain)
        {
            Random Random1 = new Random();

            //OnlineChart.Titles[0].Text = "  ";
            int Random_Color_Index = 0;
            Random_Color_Index = (int)(Random1.NextDouble() * 90);
            if (!overlaymain)
                if (cycles == 1)
                    tChart1.Series.Clear();
            System.Windows.Forms.DataVisualization.Charting.Series ser1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            ser1.Color = clasglobal.color_Graph[Random_Color_Index];
            ser1.LabelFormat = "#,##0.###########";
            if (opForm.rbfLine.Checked)
            {
                ser1.ChartType = SeriesChartType.FastLine;

            }
            if (opForm.rbLine.Checked)
            {
                ser1.ChartType = SeriesChartType.Line;
            }
            if (opForm.rbPoint.Checked)
            {
                ser1.ChartType = SeriesChartType.Point;


            }
            tChart1.Series.Add(ser1);
            num_Series = tChart1.Series.Count - 1;////////////////////////////
                                                  //Num_Graph_Point = 0;

            //isReversed = 0;
            tChart1.Series[num_Series].Color = clasglobal.color_Graph[Random_Color_Index];


            if (!overlaymain)
            {
                tChart1.Series[num_Series].Points.Clear();

                tChart1.Series.Remove(tChart1.Series[0]);
            }
            tChart1.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle2080[tech];
            tChart1.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle2080[tech];
            if (num_Series < 1)
                Overlay_tech[0] = tech;
            else
                Overlay_tech[num_Series] = tech;

        }

        private void clearselplot()
        {


            if (tChart1.Series.Count > tvTechList.Nodes[0].Nodes.Count)
                for (int i = tvTechList.Nodes[0].Nodes.Count; i < tChart1.Series.Count; i++)
                {
                    tChart1.Series.Remove(this.tChart1.Series[i]);
                    num_Series--;


                }

            if (MessageBox.Show("Removed all selected Plot(s)?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int num_del = 0;
                int n = tChart1.Series.Count;
                if (n == 1)
                {

                    try
                    {
                        clearform();
                    }
                    catch { }

                }
                else
                    try
                    {
                        for (int i = 0; i < n; i++)
                            if (tvTechList.Nodes[0].Nodes[i - num_del].Checked)
                            {
                                tChart1.Series.Remove(tChart1.Series[i - num_del]);
                                tvTechList.Nodes[0].Nodes[i - num_del].Remove();
                                dschart2.analyselist.Rows[i - num_del].Delete();
                                dschart2.analyselist.AcceptChanges();
                                num_del++;
                                num_Series--;
                                tChart1.Refresh();
                            }

                    }
                    catch { }

            }
            if (this.tvTechList.Nodes[0].Nodes.Count > 0)
            {
                this.tvTechList.SelectedNode = this.tvTechList.Nodes[0].FirstNode;
                this.tvTechList.Nodes[0].FirstNode.Checked = true;

            }



        }


        public void mathematicf(int XorY, string txt, double value, int i)
        {

            double[] P = new double[this.tChart1.Series[i].Points.Count];
            double[] I = new double[this.tChart1.Series[i].Points.Count];
            for (int k = 0; k < this.tChart1.Series[i].Points.Count; k++)
            {
                P[k] = this.tChart1.Series[i].Points[k].XValue;
                I[k] = this.tChart1.Series[i].Points[k].YValues[0];
            }
            mc.Math_Operation(P, I, XorY, txt, value);


            // OnlineChart.Series[num_Series].Clear();
            operation = "Mathematics";
            show_Graph(0, this, "SubChart", mc.AR, mc.BR, true, mc.AR.Length);

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int shomarcheck = 0, i1 = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    i1 = i;
                }
            if (shomarcheck == 1)
            {
                frmMath12080 mthForm = new frmMath12080(i1);
                mthForm.gf = this;

                mthForm.TopMost = true;
                mthForm.ShowDialog();
                nindexchart2 = i1;
            }
            else
                MessageBox.Show("Mathematical Operation work with one graph" + (char)13 + "Please select ONE only!!!!!!");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int i1 = 0;
            int shomarcheck = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    i1 = i;
                }
            inja:
            if (shomarcheck == 1)
            {
                operation = "Integral";
                ITechniques tech = TechLine[i1];
                mc.tq = tech.Number;
                var Chart = tChart1.Series[$"{tech.Name}-{i1}"];
                double[] P = new double[Chart.Points.Count];
                double[] I = new double[Chart.Points.Count];
                if (tech.Name == "CV")
                {
                    int i2 = 0;
                    shomarcheck = 0;
                    for (int i = 0; i < tvTechList.Nodes[0].Nodes[i1].Nodes.Count; i++)
                        if (tvTechList.Nodes[0].Nodes[i].Checked)
                        {
                            shomarcheck++;
                            i2 = i;
                        }
                    if (shomarcheck > 1)
                        goto inja;

                    if (shomarcheck == 1)
                    {
                        Chart = tChart1.Series[$"{tech.Name}-{i1}_{i2 + 1}"];
                        P = new double[Chart.Points.Count];
                        I = new double[Chart.Points.Count];
                    }
                }
                for (int k = 0; k < this.tChart1.Series[i1].Points.Count; k++)
                {
                    P[k] = Chart.Points[k].XValue;
                    I[k] = Chart.Points[k].YValues[0];
                }
                mc.Integral(P, I, 0, 10);
                show_Graph(0, this, "SubChart", mc.AR, mc.BR, true, mc.AR.Length);
                nindexchart2 = i1;
            }
            else
                MessageBox.Show("Integration work with one graph" + (char)13 + "Please select ONE only!!!");

        }







        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            frmShowPeaks12080 fpeak = new frmShowPeaks12080();
            //bool Detect_Peak = true;
            // fpeak.fm = this;
            fpeak.Foption = this.opForm;
            fpeak.MdiParent = this;
            fpeak.Show();
        }



        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            bgsubtractionf();
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled;
            //  OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled;
            //  OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled;

            this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            //this.ShowHideGrid.Text = "Show Grids";
        }


        private void tsbOpen_Click(object sender, EventArgs e)
        {
            tChart2.Series.Clear();
            tChart2.Refresh();
            UpdateMyNewButtonState();
            pnl_peak_search.Visible = false;

            btnzoomout_Click(sender, e);//هنگام باز کردن دوباره کامل نمایش یابد
            opengraph();
            btnzoomout_Click(sender, e);//هنگام باز کردن دوباره کامل نمایش یابد
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int i1 = 0;
            int shomarcheck = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    i1 = i;
                }
            if (shomarcheck == 1)
            {
                operation = "Derivation";
                mc.tq = tech;
                double[] P = new double[this.tChart1.Series[i1].Points.Count];
                double[] I = new double[this.tChart1.Series[i1].Points.Count];
                for (int k = 0; k < this.tChart1.Series[i1].Points.Count; k++)
                {
                    P[k] = this.tChart1.Series[i1].Points[k].XValue;
                    I[k] = this.tChart1.Series[i1].Points[k].YValues[0];

                }
                mc.Derivate(P, I);
                show_Graph(0, this, "SubChart", mc.AR, mc.BR, true, mc.AR.Length);
                nindexchart2 = i1;
            }
            else
                MessageBox.Show("derivation work with one graph" + (char)13 + "Please select ONE only!!!");

        }
        public void SwapAxes(Chart chart)
        {
            if (chart.ChartAreas.Count == 0)
                return;

            var area = chart.ChartAreas[0];

            // 1. جابجا کردن داده‌های سری‌ها
            foreach (var series in chart.Series)
            {
                // فقط سری‌هایی که به همین چارت‌اریا وصل‌اند
                if (series.ChartArea == area.Name)
                {
                    foreach (var point in series.Points)
                    {
                        double temp = point.XValue;
                        point.XValue = point.YValues[0];
                        point.YValues[0] = temp;
                    }
                }
            }

            // 2. جابجا کردن محدوده‌ی محور‌ها
            double xMin = area.AxisX.Minimum;
            double xMax = area.AxisX.Maximum;
            double yMin = area.AxisY.Minimum;
            double yMax = area.AxisY.Maximum;

            // اگر محور Auto هست (Minimum/Maximum = Double.NaN)، نیازی به تغییر نداره
            if (!double.IsNaN(xMin) && !double.IsNaN(yMin))
            {
                area.AxisX.Minimum = yMin;
                area.AxisY.Minimum = xMin;
            }

            if (!double.IsNaN(xMax) && !double.IsNaN(yMax))
            {
                area.AxisX.Maximum = yMax;
                area.AxisY.Maximum = xMax;
            }

            // 3. جابجایی عنوان محور‌ها
            string xTitle = area.AxisX.Title;
            area.AxisX.Title = area.AxisY.Title;
            area.AxisY.Title = xTitle;

            chart.Invalidate();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            btnRotateAxes.Visible = true;
            if (tChart2.Annotations.Count > 0)
            {
                tChart2.Annotations.Clear();
                Series intersectionSeries = tChart2.Series.FindByName("IntersectionPoint");
                if (intersectionSeries != null)
                {
                    intersectionSeries.Points.Clear();
                }
                tChart2.Refresh();
            }

            drawline1.removeall();
            drawline1.removeall_master();
            numline = -1;
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int shomarcheck = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                    shomarcheck++;

            if (shomarcheck == 1)
            {
                btnCalculateTafel.Visible = true;
                MessageBox.Show("Please Select 2 Lines To Begin");
                tsBtnDrawLine.Checked = true;
                operation = "Tafel";// "Mathematics";
                for (int i = 0; i < tChart1.Series.Count; i++)
                    if (tChart1.Series[i].Enabled)
                    {
                        double[] P = new double[this.tChart1.Series[i].Points.Count];
                        double[] I = new double[this.tChart1.Series[i].Points.Count];
                        for (int k = 0; k < this.tChart1.Series[i].Points.Count; k++)
                        {
                            P[k] = this.tChart1.Series[i].Points[k].XValue;
                            I[k] = this.tChart1.Series[i].Points[k].YValues[0];
                        }
                        mc.Math_Operation(P, I, 1, "Tafel", 10);
                        show_Graph(0, this, "SubChart", mc.AR, mc.BR, true, mc.AR.Length);
                        nindexchart2 = i;
                    }

                // ★★★ اضافه کردن تایتل‌های جدید برای حالت Tafel ★★★
                tChart2.ChartAreas[0].AxisX.Title = "E(mV)";
                tChart2.ChartAreas[0].AxisY.Title = "Log Icorr";
                tChart2.Refresh(); // اعمال تغییرات روی نمودار
                                   // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

                drawline1.EnableDraw = true;
                tChart2.Focus();
            }
            else
            {
                btnCalculateTafel.Visible = false;
                MessageBox.Show("Tafel works with one graph" + (char)13 + "Please select ONE only");
            }
        }

        private void save_Data()
        {
            // 1. انتخاب پوشه توسط کاربر
            using (var folderDialog = new SaveFileDialog())
            {
                folderDialog.AddExtension = true;
                folderDialog.DefaultExt = "csv";
                folderDialog.Filter = "Data Files(*.csv)|*.csv";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = Path.GetDirectoryName(folderDialog.FileName);

                    foreach (TreeNode node in tvTechList.Nodes[0].Nodes)
                    {
                        if (node.Checked)
                        {
                            ITechniques tech = TechLine[node.Index];

                            // مسیر فایل مبدا
                            string sourcePath = tech.SeriesPath;

                            string UserDesierName = Path.GetFileNameWithoutExtension(folderDialog.FileName);
                            // استخراج نام فایل از مسیر
                            string fileName = UserDesierName + "_" + Path.GetFileName(sourcePath);

                            // ساخت مسیر کامل مقصد
                            string destinationPath = Path.Combine(selectedPath, fileName);

                            try
                            {
                                File.Copy(sourcePath, destinationPath);
                                MessageBox.Show($"فایل CSV با موفقیت در:\n{selectedPath} ذخیره شد.", "موفقیت",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"خطا در ذخیره فایل:\n{ex.Message}", "خطا",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }
                }
            }
        }

        int row_sig = 0;
        double[] volt = new double[300];
        double[] current = new double[300];
        int Counter_num = 300;

        public byte[] fill_matris()
        {
            byte[] b = new byte[5000];
            int Counter = 0, Teqnic = 0;

            double Startpotintial2 = 0, firstvertexpotential = 0, voltagstep = 0, Sweeprate = 0, Initial_parge_time = 0;
            double Cleaningpotintial = 0, Cleaningtime = 0, Depositionpotential = 0, depositiontime = 0, standofpotential = 0;
            double Startpotintial = 0, Endpotintial = 0, equilibtime = 0, secondvertexpotential = 0;
            double Frequency = 0, Amplitude = 0, PulseTime = 0, BasePotential = 0, VoltageStepTime = 0, PulseAmplitude = 0;
            int Cycle = 0, Electrod_type = 0, Drop_size = 0, RDE_rpm = 0, numsweep = 0, numofsave = 0;
            byte[] Temp;
            string Start_bp = "[SIGNAL_PARAMETERS]";///19
            string Start_bpe = "[SIGNAL_PARAMETERS_END]";////21
            string Start_bd = "[SIGNAL_DATA]";////11
            string Start_bde = "[SIGNAL_DATA_END]";/////15
            switch (clasglobal.TechName[int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString())])
            {
                case "NPV":
                    Teqnic = int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString());
                    Startpotintial2 = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());
                    firstvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][2].ToString());
                    voltagstep = double.Parse(dschart2.analyselist.Rows[row_sig][4].ToString());
                    VoltageStepTime = double.Parse(dschart2.analyselist.Rows[row_sig][7].ToString());
                    BasePotential = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());
                    PulseTime = double.Parse(dschart2.analyselist.Rows[row_sig][8].ToString());

                    break;
                case "DPV":
                    Teqnic = int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString());
                    Startpotintial2 = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());
                    firstvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][2].ToString());
                    voltagstep = double.Parse(dschart2.analyselist.Rows[row_sig][4].ToString());
                    Sweeprate = double.Parse(dschart2.analyselist.Rows[row_sig][8].ToString());
                    PulseAmplitude = double.Parse(dschart2.analyselist.Rows[row_sig][9].ToString());
                    PulseTime = double.Parse(dschart2.analyselist.Rows[row_sig][8].ToString());
                    VoltageStepTime = double.Parse(dschart2.analyselist.Rows[row_sig][7].ToString());

                    break;
                case "SWV":
                    Teqnic = int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString());
                    Startpotintial2 = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());
                    firstvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][2].ToString());
                    voltagstep = double.Parse(dschart2.analyselist.Rows[row_sig][4].ToString());
                    Amplitude = double.Parse(dschart2.analyselist.Rows[row_sig][9].ToString());
                    Frequency = double.Parse(dschart2.analyselist.Rows[row_sig][10].ToString());

                    break;
                case "CV":
                    Teqnic = int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString());
                    Startpotintial2 = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());// MessageBox.Show(dschart2.analyselist.Rows[row_sig][1].ToString());
                    firstvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][2].ToString());
                    secondvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][3].ToString());
                    voltagstep = double.Parse(dschart2.analyselist.Rows[row_sig][4].ToString());
                    Sweeprate = double.Parse(dschart2.analyselist.Rows[row_sig][8].ToString());
                    numsweep = int.Parse(dschart2.analyselist.Rows[row_sig][11].ToString());
                    numofsave = 1;

                    break;
                case "LSV":
                    Teqnic = int.Parse(dschart2.analyselist.Rows[row_sig][0].ToString());
                    Startpotintial2 = double.Parse(dschart2.analyselist.Rows[row_sig][1].ToString());
                    firstvertexpotential = double.Parse(dschart2.analyselist.Rows[row_sig][2].ToString());
                    voltagstep = double.Parse(dschart2.analyselist.Rows[row_sig][4].ToString());
                    VoltageStepTime = double.Parse(dschart2.analyselist.Rows[row_sig][7].ToString());

                    break;
            }

            //////////////////////////////////////////////
            Counter = 0;
            ///////////////Start parameter//////////////
            ///////////0A [SIGNAL_PARAMETERS]0A////////
            /////////////////0-14 ///////////////////
            b[Counter] = 0x0A;
            Counter++;
            for (int i = 0; i < Start_bp.Length; i++)
            {
                b[Counter] = (byte)Start_bp[i];
                Counter++;
            }
            b[Counter] = 0x0A;
            Counter++;

            //////////teqnic/////////
            ///////////np=03 , dp=0 , sqw=1 , dc=02 , cv=04 
            ///////////ac=06 , psa  , ccpsa , cpvs , cvs
            ////////////15-18 //////////////
            /*
switch (comboBox2.Items[comboBox2.SelectedIndex].ToString())
{
    case "NP - Normal Pulse":
        Teqnic = 3;
        break;
    case "DP - Differential Pulse":
        Teqnic = 0;
        break;
    case "Sqw - Squar Wave":
        Teqnic = 1;
        break;
    case "CV - cyclic Voltammetry":
        Teqnic = 4;
        break;
    case "DC - Sample Direct Current":
        Teqnic = 2;
        break;
    case "AC - Alternating Current Voltammetry":
        Teqnic = 6;
        break;
    case "PSA - Potentioetric Stripping Analysis":
        Teqnic = 8;
        break;
    case "CCPSA - Constant Current PSA":
        Teqnic = 9;
        break;
    case "CPVS - Cyclic Pulse Voltammetric Stripping":
        Teqnic = 7;
        break;
    case "CVS -Cyclic Voltammetric Stripping":
        Teqnic = 4;
        break;
}
*/

            // Counter = 0x15;
            Temp = HexstringToByteArray(inttoHex(Teqnic));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            /*
                b[Counter] = 0x04;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
          */
            ////////////no know/////////
            ////////////19-20 //////////////
            //  Counter = 0x19;
            b[Counter] = 0x03;
            Counter++;
            b[Counter] = 0x00;
            Counter++;
            /////////////High range//////////
            ////////////0B-0E //////////////
            //   Counter = 0x0B;
            Temp = HexstringToByteArray(inttoHex(8));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////Low Range ///////////
            /////////// 0F -22 ////////////
            //  Counter = 0x0F;
            Temp = HexstringToByteArray(inttoHex(4));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////////////Initial parge time/////////////
            //////////////////23-2A ////////////////////////
            //Initial_parge_time = double.Parse(textBox1.Text);
            // Counter = 0x23;
            Initial_parge_time = 0;///double.Parse(1);
            Temp = HexstringToByteArray(DoubletoHex(Initial_parge_time));//////Initial_parge_time
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }


            ////////////////cleaning potential////////////
            //////////////////2B - 32///////////////////
            // Cleaningpotintial = double.Parse(textBox5.Text);
            Counter = 0x2B;
            Cleaningpotintial = 0;
            Temp = HexstringToByteArray(DoubletoHex(Cleaningpotintial));///////Cleaningpotintial
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////cleaning Time////////////
            /////////////////33-3A/////////////////
            // Cleaningtime = double.Parse(textBox6.Text);
            Counter = 0x33;
            Cleaningtime = 0;// double.Parse(1);
            Temp = HexstringToByteArray(DoubletoHex(Cleaningtime));////Cleaningtime
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Deposition potential////////////
            /////////////// 3B-42 /////////////////
            Counter = 0x3B;
            Depositionpotential = 0;
            Temp = HexstringToByteArray(DoubletoHex(Depositionpotential));/////Depositionpotential
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Deposition Time////////////
            /////////////////43-4A///////////////////
            depositiontime = 0;
            Counter = 0x43;
            Temp = HexstringToByteArray(DoubletoHex(depositiontime));/////depositiontime
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Hydrodinamic////////////
            /////////////////4B -4E ////////////////
            //  b[0x4B] = 0x00;
            // if (checkBox2.Checked ) Temp = HexstringToByteArray(inttoHex(1));
            //else Temp = HexstringToByteArray(inttoHex(0));
            Counter = 0x4B;
            Temp = HexstringToByteArray(inttoHex(0));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////////////Cell off //////////////
            /////////////////4F -52 ////////////////
            // b[0x4F] = 0x01;
            //if (checkBox1.Checked ) Temp = HexstringToByteArray(inttoHex(1));
            //else Temp = HexstringToByteArray(inttoHex(0));
            Temp = HexstringToByteArray(inttoHex(0));
            Counter = 0x4F;
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Stand by Potintion////////////
            /////////////////53-5A///////////////////
            //standofpotential = double.Parse(textBox16.Text);
            standofpotential = 0;
            Temp = HexstringToByteArray(DoubletoHex(standofpotential));///////standofpotential
            Counter = 0x53;
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Start Potential////////////
            /////////////////5B-62///////////////////
            // Startpotintial = double.Parse(textBox2.Text);
            Startpotintial = 0;//// double.Parse(1);
            Temp = HexstringToByteArray(DoubletoHex(Startpotintial));////////Startpotintial
            Counter = 0x5B;
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////End Potential////////////
            /////////////////63-6A///////////////////
            // Endpotintial = double.Parse(textBox3.Text);
            Endpotintial = 0;
            Temp = HexstringToByteArray(DoubletoHex(Endpotintial));////////Endpotintial
            Counter = 0x63;
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Cycle //////////////////
            /////////////////6B ,6C///////////////////
            // Cycle = int.Parse(textBox4.Text);
            Cycle = 1; ///int.Parse(1);
            Temp = HexstringToByteArray(inttoHex(Cycle));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Eq Time ////////////////
            /////////////////6D-76 //////////////////
            equilibtime = 0;
            Temp = HexstringToByteArray(DoubletoHex(equilibtime));
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////electrod////////////
            ////////0<electrod<4////////
            //SMDE=01
            //HMDE=02
            //RDE=03 /////active hydro
            //   =00
            /////////////////77-7A ////////////////
            Electrod_type = 3;
            Temp = HexstringToByteArray(inttoHex(Electrod_type));//////////Electrod_type
                                                                 // Temp = HexstringToByteArray(inttoHex(1));
            Counter = 0x77;
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////Drop /////////////////
            /////////////0<Drop<9 /////////////////
            /////////////////7B -7E ////////////////
            // Drop_size = int.Parse(domainUpDown1.Items[domainUpDown1.SelectedIndex].ToString());
            Drop_size = 9;
            Temp = HexstringToByteArray(inttoHex(Drop_size));///////////Drop_size
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ////////////////RPM ///////////////////
            ///////*200 /////////
            //RPM=3000 (MAX) ,,,,,RPM=0F ///////
            /////////////////7F -82 ////////////////
            // RDE_rpm = (int.Parse(domainUpDown2.Items[domainUpDown2.SelectedIndex].ToString())) / 200;
            RDE_rpm = 4;
            Temp = HexstringToByteArray(inttoHex(RDE_rpm));////////////RDE_rpm
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////// no know /////////////
            ///////////////83 ,84 ///////////
            b[Counter] = 0x04;
            Counter++;
            b[Counter] = 0x00;
            Counter++;
            //////////Start potential////////////
            ///////////// 85 - 8C /////////////

            //Startpotintial2 = double.Parse(textBox10.Text);
            Temp = HexstringToByteArray(DoubletoHex(Startpotintial2));///////////Startpotintial2
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            //////////first  potential(cv)or Endpotintial2////////////
            ///////////// 8D - 94 /////////////
            // firstvertexpotential = double.Parse(textBox11.Text);
            Temp = HexstringToByteArray(DoubletoHex(firstvertexpotential));///////////firstvertexpotential
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            //////////voltage step////////////
            ///////////// 95 - 9C /////////////
            // voltagstep = double.Parse(textBox12.Text);
            Temp = HexstringToByteArray(DoubletoHex(voltagstep));/////voltagstep
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////// No.oF SWeeps(cv) /////////////
            //////////////9D - A0 //////////////
            if (Teqnic == 4)
            {
                /// int.Parse(textBox14.Text);
                Temp = HexstringToByteArray(inttoHex(numsweep));//////numsweep
                for (int i = 0; i < Temp.Length; i++)
                {
                    b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                    Counter++;
                }
            }
            else
            {
                b[Counter] = 0x01;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
                b[Counter] = 0x00;
                Counter++;
            }

            //////////Start  potential////////////
            ///////////// A1 - A8 /////////////
            //Startpotintial2 = double.Parse(textBox10.Text);
            Temp = HexstringToByteArray(DoubletoHex(Startpotintial2));////////Startpotintial2
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            //////////Second   potential(cv) or Start  potential ////////////
            ///////////// A9 - B0  /////////////
            if (Teqnic == 4)
            {
                //secondvertexpotential = double.Parse(textBox15.Text);
                Temp = HexstringToByteArray(DoubletoHex(secondvertexpotential));///////secondvertexpotential
            }
            else
            {
                //  Startpotintial2 = double.Parse(textBox10.Text);
                Temp = HexstringToByteArray(DoubletoHex(Startpotintial2));////////Startpotintial2
            }
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////// no know /////////////
            /////////////////B1 , B2 //////////
            b[Counter] = 0x02;
            Counter++;
            b[Counter] = 0x00;
            Counter++;
            /////////// Sweep rate(cv) or Base potential(npv) /////////////
            ////////// Pulse amplitude(dpv)or Amplitude(swv)
            ///////////// B3-BA  /////////////
            if (Teqnic == 4)
            {
                Temp = HexstringToByteArray(DoubletoHex(Sweeprate));
            }
            if (Teqnic == 1)
            {
                Temp = HexstringToByteArray(DoubletoHex(BasePotential));
            }
            if (Teqnic == 5)
            {
                Temp = HexstringToByteArray(DoubletoHex(VoltageStepTime));
            }
            if (Teqnic == 2)
            {
                Temp = HexstringToByteArray(DoubletoHex(PulseAmplitude));
            }
            if (Teqnic == 3)
            {
                Temp = HexstringToByteArray(DoubletoHex(Amplitude));
            }
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            ///////////// Save last(cv) or Pulse time(npv & dpv) /////////////
            //////////// Frequency(swv)
            //////////////BB - C2 /////////////////
            if (Teqnic == 4)
            {
                Temp = HexstringToByteArray(inttoHex(numofsave));
            }
            if (Teqnic == 3)
            {
                Temp = HexstringToByteArray(DoubletoHex(Frequency));
            }
            if (Teqnic == 1 || Teqnic == 2)
            {
                Temp = HexstringToByteArray(DoubletoHex(PulseTime));
            }
            if (Teqnic == 4 || Teqnic == 1 || Teqnic == 3 || Teqnic == 2)
            {
                for (int i = 0; i < Temp.Length; i++)
                {
                    b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                    Counter++;
                }
            }
            //////////////////////////////////////
            ///////////// Voltage step time(npv&dpv) /////////////
            //////////////C3 - CA //////////////
            if (Teqnic == 1 || Teqnic == 2)
            {
                Temp = HexstringToByteArray(DoubletoHex(VoltageStepTime));//////Sweeprate
                for (int i = 0; i < Temp.Length; i++)
                {
                    b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                    Counter++;
                }
            }
            b[Counter] = 0x0A;
            Counter++;
            for (int i = 0; i < Start_bpe.Length; i++)
            {
                b[Counter] = (byte)Start_bpe[i];
                Counter++;
            }
            b[Counter] = 0x0A;
            Counter++;
            b[Counter] = 0x0A;
            Counter++;
            for (int i = 0; i < Start_bd.Length; i++)
            {
                b[Counter] = (byte)Start_bd[i];
                Counter++;
            }
            b[Counter] = 0x0A;
            Counter++;
            ///////////////////////////////////////////////////////////////////
            ////////////////////number data ////////////////////////////
            Temp = HexstringToByteArray(inttoHex(Counter_num));////////////RDE_rpm
            for (int i = 0; i < Temp.Length; i++)
            {
                b[Counter] = (byte)Temp[(Temp.Length - 1) - i];
                Counter++;
            }
            /////////////////////////////////////////////////////////////////
            ////////////////// send data volt and current //////////////////
            for (int i1 = 0; i1 < Counter_num; i1++)
            {
                Temp = HexstringToByteArray(DoubletoHex(volt[i1]));  // volttage
                for (int i2 = 0; i2 < Temp.Length; i2++)
                {
                    b[Counter] = (byte)Temp[(Temp.Length - 1) - i2];
                    Counter++;
                }

                ///////////current

                Temp = HexstringToByteArray(DoubletoHex(current[i1]));  // jarian
                for (int i3 = 0; i3 < Temp.Length; i3++)
                {
                    b[Counter] = (byte)Temp[(Temp.Length - 1) - i3];
                    Counter++;
                }

                //MessageBox.Show(volt[i1].ToString() + Environment.NewLine + current[i1].ToString());
            }
            ///////////////////////////////////////////////////// 
            ////////////////
            b[Counter] = 0x0A;
            Counter++;
            for (int i = 0; i < Start_bde.Length; i++)
            {
                b[Counter] = (byte)Start_bde[i];
                Counter++;
            }
            b[Counter] = 0x0A;

            b[Counter] = 0x0A;
            Counter++;

            return b;
        }

        private byte[] HexstringToByteArray(string hex)
        {
            int hexstringlength = hex.Length;
            byte[] b = new byte[hexstringlength / 2];
            for (int i = 0; i < hexstringlength; i += 2)
            {
                int topChar = (hex[i] > 0x40 ? hex[i] - 0x37 : hex[i] - 0x30) << 4;
                int bottomChar = hex[i + 1] > 0x40 ? hex[i + 1] - 0x37 : hex[i + 1] - 0x30;
                b[i / 2] = Convert.ToByte(topChar + bottomChar);
            }
            return b;
        }
        private string inttoHex(int Value_int16)
        {
            string Value_String;
            Value_String = Value_int16.ToString("X8");
            return Value_String;
        }
        private string DoubletoHex(Double Value_Double)
        {
            long Value_Long = 0;
            string Value_String;
            Value_Long = BitConverter.DoubleToInt64Bits(Value_Double);
            Value_String = Value_Long.ToString("X8");
            return Value_String;
        }

        private void WriteToFile(string tchName, string fileName, int row)
        {
            string s = "";
            if (fileName != "")
                s = fileName;
            else
                s = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Prms2080.prj";

            StreamWriter Fl = new StreamWriter(s, false, Encoding.ASCII);

            if (tchName != "APP" && tchName != "SEQ")
            {
                Fl.WriteLine("Settings for Parameters:\n");
                Fl.WriteLine("Date:" + DateTime.Today.ToString());
                if (tchName == "DCV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DCV");
                    //Fl.WriteLine("TchNo: "+ DCV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "NPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: NPV");
                    //Fl.WriteLine("TchNo: " + NPV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Pulse Width:" + dschart2.analyselist.Rows[row][8].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "DPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPV");
                    //Fl.WriteLine("TchNo: " + DPV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Pulse Height:" + dschart2.analyselist.Rows[row][9].ToString());
                    Fl.WriteLine("Pulse Width:" + dschart2.analyselist.Rows[row][8].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "SWV" || tchName == "All")
                {

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: SWV");
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("Frequency:" + dschart2.analyselist.Rows[row][10].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Pulse Height:" + dschart2.analyselist.Rows[row][9].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "CV" || tchName == "All")
                {

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CV");
                    //Fl.WriteLine("TchNo: " + CV.ToString());
                    Fl.WriteLine("Current Range:0");
                    // Fl.WriteLine("Cycles:1";
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("E3:" + dschart2.analyselist.Rows[row][3].ToString());
                    Fl.WriteLine("Hold Time:0");
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("ScanRate_R:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "LSV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: LSV");
                    //Fl.WriteLine("TchNo: " + LSV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());


                }

                if (tchName == "DCS" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DCs");
                    //Fl.WriteLine("TchNo: " + DCs.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

                if (tchName == "DPs" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPS");
                    //Fl.WriteLine("TchNo: " + DPs.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("HStep:" + dschart2.analyselist.Rows[row][4].ToString());
                    Fl.WriteLine("Pulse Height:" + dschart2.analyselist.Rows[row][9].ToString());
                    Fl.WriteLine("Pulse Width:" + dschart2.analyselist.Rows[row][8].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("Scan Rate:" + dschart2.analyselist.Rows[row][6].ToString());
                    Fl.WriteLine("TStep:" + dschart2.analyselist.Rows[row][7].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }
                if (tchName == "OCP" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: OCP");
                    //Fl.WriteLine("TchNo: " + OCP.ToString());
                    Fl.WriteLine("Time:" + clasglobal.ocpprms_ocp.Time.ToString());
                    // Fl.WriteLine("Cycles:" + "1"); //clasglobal.ocpprms.Cycles.ToString());
                }

                if (tchName == "CPC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CPC");
                    //Fl.WriteLine("TchNo: " + CPC.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("T1:" + dschart2.analyselist.Rows[row][13].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                    //   Fl.WriteLine("Cycles:1");
                }

                if (tchName == "CCC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CCC");
                    //Fl.WriteLine("TchNo: " + CCC.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("T1:" + dschart2.analyselist.Rows[row][13].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                    //  Fl.WriteLine("Cycles:1");
                }
                if (tchName == "CHP" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHP");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("I1:" + dschart2.analyselist.Rows[row][15].ToString());
                    Fl.WriteLine("I2:" + dschart2.analyselist.Rows[row][16].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("T1:" + dschart2.analyselist.Rows[row][13].ToString());
                    Fl.WriteLine("T2:" + dschart2.analyselist.Rows[row][14].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                    //Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                }
                if (tchName == "CHA" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHA");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("T1:" + dschart2.analyselist.Rows[row][13].ToString());
                    Fl.WriteLine("T2:" + dschart2.analyselist.Rows[row][14].ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                    // Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());

                }
                if (tchName == "CHC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHC");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + dschart2.analyselist.Rows[row][1].ToString());
                    Fl.WriteLine("E2:" + dschart2.analyselist.Rows[row][2].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dschart2.analyselist.Rows[row][5].ToString());
                    Fl.WriteLine("T1:" + dschart2.analyselist.Rows[row][13].ToString());
                    Fl.WriteLine("T2:" + dschart2.analyselist.Rows[row][14].ToString());
                    // Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    Fl.WriteLine("vs_OCP:" + dschart2.analyselist.Rows[row][19].ToString());
                    Fl.WriteLine("OCPmeas:" + dschart2.analyselist.Rows[row][20].ToString());

                }

            }


            Fl.Close();

        }

        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            if (tvTechList.Nodes[0].Nodes.Cast<TreeNode>().Any(node => node.Checked))
                save_Data();
            else
                MessageBox.Show("There is not any case selected for saving !!!");
        }

        public string ConvertSeriesToCsv(ITechniques Tech, string delimiter = ",", bool includeHeader = true)
        {
            Series series = null;

            if (series == null)
                throw new ArgumentNullException(nameof(series));

            var sb = new StringBuilder();

            // 📌 نوشتن هدر (نام ستون‌ها)
            if (includeHeader)
            {
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "SeriesName",
                "PointIndex",
                "XValue",
                "YValues",
                "IsEmpty"
                }));
            }

            // 📌 دریافت اطلاعات کلی سری
            string seriesName = Tech.Name;

            // 📌 پیمایش تمام نقاط داده
            for (int i = 0; i < series.Points.Count; i++)
            {
                DataPoint point = series.Points[i];

                // مدیریت مقادیر Y (ممکن است چندین مقدار داشته باشد)
                string yValues = string.Join(";", point.YValues);

                // فرار از کاراکترهای ویژه در XValue (در صورت نیاز)
                string xValue = point.XValue.ToString();
                if (xValue.Contains(delimiter) || xValue.Contains("\""))
                    xValue = $"\"{xValue.Replace("\"", "\"\"")}\"";

                // تشکیل خط CSV
                string[] fields = new string[]
                {
                    seriesName,
                    i.ToString(),
                    xValue,
                    yValues,
                    point.IsEmpty.ToString()
                };

                // اعمال فرار از کاراکترهای خاص برای تمام فیلدها
                for (int j = 0; j < fields.Length; j++)
                {
                    if (fields[j].Contains(delimiter) || fields[j].Contains("\""))
                        fields[j] = $"\"{fields[j].Replace("\"", "\"\"")}\"";
                }

                sb.AppendLine(string.Join(delimiter, fields));
            }

            return sb.ToString();
        }

        private System.Drawing.Point point_start, TempPoint, point_end;
        int numline = -1;
        // bool endline = false;
        private void tChart2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (statebutton_1.Trim() == "drawline")
                {
                    if (e.Button == MouseButtons.Left && drawline1.EnableDraw == true)
                    {
                        point_start = new System.Drawing.Point(e.X, e.Y);
                        TempPoint = new System.Drawing.Point(e.X, e.Y);
                    }
                }
                else
                {
                    mDown = tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X);
                    mDowny = tChart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y);
                }

                double x = tChart2.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X);
                double y = tChart2.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y);

                if (start_click == 1)
                {
                    tChart2.Series[1].ChartType = SeriesChartType.Point;
                    tChart2.Series[1].Color = Color.Red;
                    tChart2.Series[1].BorderWidth = 3;
                    draw_point(x, y);//پیدا کردن نزدیکترین نقطه به نقطه رسم شده در گراف    
                }
                else if (end_click == 1)
                {
                    tChart2.Series[1].ChartType = SeriesChartType.Spline;
                    tChart2.Series[1].Color = Color.Red;
                    tChart2.Series[1].BorderWidth = 1;
                    draw_point(x, y);

                    end_click = 0;
                    this.Cursor = Cursors.Default;
                    tChart2.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void draw_point(double x, double y)
        {
            try
            {
                int forward_count = Counter_num;//برای گراف سی وی که رفت و برگشت دارد این رفت
                int department_back = 0;

                for (int i = 1; i < Counter_num; i++)
                {
                    if (volt[i] < volt[i - 1])//برای تکنیک سی وی هنگام برگشت
                    {
                        forward_count = i - 1;
                        department_back = 1;

                        break;
                    }
                }


                double x_2 = -32000;
                double y_2 = -32000;
                double y_2_back = 0;
                int con = 0;
                for (int i = 1; i < Counter_num; i++)//پیدا کردن نزدیک ترین ایکس به نقطه ایجاد شده در آرایه
                {
                    if (x >= volt[i - 1] && x <= volt[i])
                    {
                        if (x == volt[i])
                            x_2 = volt[i];
                        else
                            x_2 = volt[i - 1];

                        con = i;

                        break;
                    }
                }

                if (department_back == 1)
                {
                    if (x >= volt[forward_count])//برای انتخاب آخرین نقطه ای که در مرز برگشت است
                        x_2 = volt[forward_count];
                }
                else
                {
                    if (x >= volt[forward_count - 1])//برای انتخاب آخرین نقطه
                        x_2 = volt[forward_count - 1];
                }



                if (x_2 == -32000)//اگر حتما مقداری انتخاب شده است
                    return;

                int flag_back = 0;

                for (int i = 0; i < Counter_num; i++)//پیدا کردن نزدیکترین ایگرگ به نقطه ایجاد شده
                {
                    if (volt[i] == x_2)
                    {
                        if (flag_back == 0)
                        {
                            y_2 = current[i];
                            flag_back = 1;
                        }
                        else
                        {
                            y_2_back = current[i];// در صورتی که گراف برگشتی است مقدار دوم را هم بگیرد
                            flag_back = 5;
                            break;
                        }
                    }
                }

                if (y_2 == -32000)
                    return;

                double final_y = y_2;

                if (flag_back == 5)//اگر برگشتی دارد
                {
                    double y_stor = y_2;
                    double y_stor_back = y_2_back;

                    if (y < 0) y = (-1) * y;//همه نقاط را مثبت میکنیم و سپس نزدیکترین نقطه را در محور ایگرگ نسبت به رفت یا برگشت انتخاب می کنیم
                    if (y_2 < 0) y_2 = (-1) * y_2;
                    if (y_2_back < 0) y_2_back = (-1) * y_2_back;

                    double result_1 = 0;
                    double result_2 = 0;

                    if (y > y_2)
                        result_1 = y - y_2;
                    else
                        result_1 = y_2 - y;

                    if (y > y_2_back)
                        result_2 = y - y_2_back;
                    else
                        result_2 = y_2_back - y;


                    if (result_2 < result_1)
                        final_y = y_stor_back;
                    else
                        final_y = y_stor;
                }

                tChart2.Series[1].Points.AddXY(x_2, final_y);

                start_click = 0;
                end_click = 1;
            }
            catch (Exception m)
            {
                this.Cursor = Cursors.UpArrow;
                MessageBox.Show(m.Message);
                new_handle();
            }
        }
        private void tChart2_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (statebutton_1.Trim() == "")
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        var results1 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                        foreach (var result in results1)
                        {
                            if (result.ChartElementType == ChartElementType.PlottingArea)
                            {
                                double xv = tChart2.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X);
                                double yv = tChart2.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y);
                                if (xv - mDown > 0)
                                    tChart2.ChartAreas[0].AxisX.ScaleView.Position = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum + (difxview2 / 500);
                                else if (xv - mDown < 0)
                                    tChart2.ChartAreas[0].AxisX.ScaleView.Position = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum - (difxview2 / 500);
                                else
                                    tChart2.ChartAreas[0].AxisX.ScaleView.Position = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                                if ((yv - mDowny) > 0)
                                    tChart2.ChartAreas[0].AxisY.ScaleView.Position = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum + (difyview2 / 500);
                                else if ((yv - mDowny) < 0)
                                    tChart2.ChartAreas[0].AxisY.ScaleView.Position = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum - (difyview2 / 500);
                                else
                                    tChart2.ChartAreas[0].AxisY.ScaleView.Position = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                                tChart2.ChartAreas[0].AxisX.ScaleView.Size = difxview2; //(OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMaximum - OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMinimum); //+ xv - mDown;OnlineChart.ChartAreas[0].AxisX.Minimum +
                                tChart2.ChartAreas[0].AxisY.ScaleView.Size = difyview2;// (OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMaximum - OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMinimum) + yv - mDowny;//OnlineChart.ChartAreas[0].AxisX.Minimum +

                                mDown = xv;
                                mDowny = yv;
                            }

                        }
                    }
                }

                var results11 = tChart2.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                foreach (var result in results11)
                {
                    if (result.ChartElementType == ChartElementType.PlottingArea)
                    {

                        switch (statebutton_1)
                        {
                            case "selectonly":
                                tChart2.Cursor = System.Windows.Forms.Cursors.Cross;
                                break;
                            case "showvalue":
                                tChart2.Cursor = System.Windows.Forms.Cursors.SizeAll;

                                break;

                            default:
                                if (start_click == 0 && end_click == 0)
                                    tChart2.Cursor = System.Windows.Forms.Cursors.Default;
                                break;
                        }

                    }
                    else if (start_click == 0 && end_click == 0)
                        tChart2.Cursor = System.Windows.Forms.Cursors.Default;
                }
                switch (statebutton_1)
                {
                    case "selectonly":
                        System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                        tChart2.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);
                        tChart2.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);

                        break;
                    case "showvalue":
                        var results12 = tChart2.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                        double x111 = 0, y111 = 0;
                        foreach (var result in results12)
                        {
                            if (result.ChartElementType == ChartElementType.PlottingArea)
                            {
                                x111 = result.ChartArea.AxisX.PixelPositionToValue(e.X);
                                y111 = result.ChartArea.AxisY.PixelPositionToValue(e.Y);
                            }
                        }

                        string x = x111.ToString("#.##");

                        if (x111 < 1)//قرار دادن صفر قبل از ممیز
                        {
                            if (x111 >= 0)
                                x = "0" + x;
                            else
                            {
                                x = (x111 * -1).ToString("#.##");
                                x = "-0" + x;
                            }
                        }

                        mainf.xyLabel.Text = x + "," + y111.ToString("#.#e-0");
                        //mainf.xyLabel.Text = x111.ToString("#.##") + "," + y111.ToString("#.#e-0");

                        break;
                    case "drawline":
                        if (e.Button == MouseButtons.Left && drawline1.EnableDraw == true)
                        {
                            if (!(point_start == null))
                            {
                                Graphics g = this.CreateGraphics();
                                Pen ErasePen = new Pen(Color.Black);
                                if (tChart2.BackColor == Color.White)
                                {
                                    ErasePen.Color = Color.Black;
                                }
                                g.DrawLine(ErasePen, point_start, TempPoint);
                                TempPoint = new System.Drawing.Point(e.X, e.Y);
                                this.tChart2.Refresh();
                            }
                        }
                        break;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tChart2_MouseUp(object sender, MouseEventArgs e)
        {
            if (statebutton_1.Trim() == "drawline")
            {
                if (e.Button == MouseButtons.Left && drawline1.EnableDraw == true)
                {
                    point_end = new System.Drawing.Point(e.X, e.Y);
                    drawline_chart.lines l1;
                    l1.point1 = point_start;
                    l1.point2 = point_end;
                    drawline1.Lines.Add(l1);
                    // endline = true;
                    point_start = System.Drawing.Point.Empty;
                }
            }

        }

        private void tChart2_Paint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(Color.White);
            if (tChart2.BackColor == Color.White)
            {
                pen.Color = Color.Black;
            }

            if (!point_start.IsEmpty)
                e.Graphics.DrawLine(pen, point_start, TempPoint);

            for (int p = 0; p < drawline1.Lines.Count; p++)
            {
                if (p == numline)
                    e.Graphics.DrawLine(Pens.Red, drawline1.Lines[p].point1, drawline1.Lines[p].point2);
                else
                    e.Graphics.DrawLine(pen, drawline1.Lines[p].point1, drawline1.Lines[p].point2);
            }
        }


        private void tChart2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 108 || e.KeyValue == 76)
            {
                numline++;
                if (numline >= drawline1.Lines.Count)
                    numline = 0;
                tChart2.Refresh();

            }
            else if (e.KeyValue == 189)
            {
                if (numline > -1 && numline < drawline1.Lines.Count)
                {
                    drawline1.remove(numline);
                    numline--;
                    tChart2.Refresh();
                }


            }

        }


        public void conv_pixel_cord(int xx, int yy)
        {
            var results = tChart2.HitTest(xx, yy, false, ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    double xVal = result.ChartArea.AxisX.PixelPositionToValue(xx);
                    double yVal = result.ChartArea.AxisY.PixelPositionToValue(yy);
                    drawline_chart.multipoint l1;
                    l1.pointx = xVal;
                    l1.pointy = yVal;

                    drawline1.Lines_master.Add(l1);

                }
            }
        }

        public void smootht4(int row1, Color color1, int typechart, int colnum)
        {
            double I0, I1 = 0;
            int vn = shomar_val;

            for (int i = 1; i < vn - 1; i++)
            {
                if (Math.Abs(othertech_val1[i] - othertech_val1[i - 1]) > 1000)
                    othertech_val1[i] = (othertech_val1[i - 1] + othertech_val1[i + 1]) / 2;
            }
            for (int i = vn - 1; i >= 1; i--)
            {
                othertech_val1[i] = (othertech_val1[i - 1] + othertech_val1[i]) / 2;
            }
            for (int i = 1; i < vn - 1; i++)
            {
                othertech_val1[i] = (othertech_val1[i - 1] + othertech_val1[i + 1]) / 2;
            }
            for (int j1 = 3; j1 >= 0; j1--)
            {
                for (int i = 4; i <= vn - 3; i++)
                {

                    I0 = ((othertech_val1[i - 1] - othertech_val1[i - 2]) + (othertech_val1[i - 2] - othertech_val1[i - 3]) + (othertech_val1[i - 3] - othertech_val1[i - 4])) / 3;
                    I1 = Math.Abs(othertech_val1[i] - othertech_val1[i - 1]);
                    if (I1 > Math.Abs(j1 * I0))
                        othertech_val1[i] = (othertech_val1[i - 2] + othertech_val1[i - 1] + othertech_val1[i + 2] + othertech_val1[i + 1]) / 4;

                }
            }

            for (int i = 3; i <= vn - 4; i++)
            {

                othertech_val1[i] = (othertech_val1[i - 3] + othertech_val1[i - 2] + othertech_val1[i - 1] + othertech_val1[i + 3] + othertech_val1[i + 2] + othertech_val1[i + 1]) / 6;

            }
            if (typechart == 1)
            {
                tChart1.Series[row1].Color = color1;
                tChart1.Series[row1].Points.Clear();

                for (int i = 0; i < vn; i++)
                {
                    tChart1.Series[row1].Points.AddXY(othertech_val0[i], othertech_val1[i]);
                }
            }
            else
            {
                tChart2.Series[row1].Points.Clear();
                for (int i = 0; i < vn; i++)
                {

                    tChart2.Series[row1].Points.AddXY(othertech_val0[i], othertech_val1[i]);

                }
            }










        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                clearform();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int shomarcheck = 0, ii = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    ii = i;
                }
            if (shomarcheck == 1)
            {
                tvTechList.SelectedNode = tvTechList.Nodes[0].Nodes[ii];
                if (int.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.DPV2080 || int.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.SWV2080)
                {
                    restore_Data_part(dschart2.analyselist.Rows[ii][21].ToString(), 2, 1);




                    operation = "i_Forward";
                    show_Graph(0, this, "SubChart", othertech_val0, othertech_val1, true, shomar_val);
                    nindexchart2 = ii;
                }
                else
                {
                    MessageBox.Show("operation is not correct for this technique", "Error");
                    return;
                }
            }
            else
                MessageBox.Show("i_Forward Operation work with one graph" + (char)13 + "Please select ONE only!!!!!!");

        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int shomarcheck = 0, ii = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    ii = i;
                }
            if (shomarcheck == 1)
            {
                tvTechList.SelectedNode = tvTechList.Nodes[0].Nodes[ii];
                if (int.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.DPV2080 || int.Parse(dschart2.analyselist.Rows[ii][0].ToString()) == clasglobal.SWV2080)
                {
                    restore_Data_part(dschart2.analyselist.Rows[ii][21].ToString(), 3, 1);


                    operation = "i_Reverse";
                    show_Graph(0, this, "SubChart", othertech_val0, othertech_val1, true, shomar_val);
                    nindexchart2 = ii;
                }
                else
                {
                    MessageBox.Show("operation is not correct for this technique", "Error");
                    return;
                }
            }
            else
                MessageBox.Show("i_Reverse Operation work with one graph" + (char)13 + "Please select ONE only!!!!!!");

        }


        private void toolStripButton11_Click_1(object sender, EventArgs e)
        {
            drawline1.EnableDraw = false;
            drawline1.removeall();
            int shomarcheck = 0, ii = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                    ii = i;
                }
            if (shomarcheck == 1)
            {
                tvTechList.SelectedNode = tvTechList.Nodes[0].Nodes[ii];

                restore_Data_part(dschart2.analyselist.Rows[ii][21].ToString(), 1, 1);
                nindexchart2 = ii;



                operation = "show_without_smoothing";
                show_Graph(0, this, "SubChart", othertech_val0, othertech_val1, true, shomar_val);

            }
            else
                MessageBox.Show("show graph without smoothing Operation work with one graph" + (char)13 + "Please select ONE only!!!!!!");
        }



        private string ExportToExcel(int colnum1, int typecall1, string outputPath)
        {
            try
            {
                // ساخت یک شی اکسل
                //ApplicationClass excelApp = new ApplicationClass();
                Microsoft.Office.Interop.Excel.Application excelApp;
                excelApp = new Microsoft.Office.Interop.Excel.Application();

                // ساخت یک WorkBook جدید
                Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

                int sheetIndex = 0, i2 = 0, i1 = 0;

                // ساخت آرایه به طول تعداد سطرهای دیتاتیبل+1 و تعداد ستونهای دیتاتیبل
                int rownum1 = 0;
                if (typecall1 == 1)
                {
                    for (int m1 = 0; m1 < tChart1.Series.Count; m1++)
                    {
                        rownum1 += tChart1.Series[m1].Points.Count;
                    }
                }
                else
                {
                    rownum1 = tChart2.Series[0].Points.Count;
                }
                object[,] rawData = new object[rownum1 + 1, colnum1];
                if (typecall1 == 1)
                {
                    rawData[i2, 0] = "technique number";
                    rawData[i2, 1] = "technique name";
                    rawData[i2, 2] = "point of technique number";
                    rawData[i2, 3] = "X value";
                    rawData[i2, 4] = "Y value";
                    i2++;
                    for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                        if (tvTechList.Nodes[0].Nodes[i].Checked)
                        {
                            for (int cc1 = 0; cc1 < tChart1.Series[i].Points.Count; cc1++)
                            {
                                rawData[i2, 0] = i1;
                                rawData[i2, 1] = clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())];
                                rawData[i2, 2] = cc1 + 1;
                                rawData[i2, 3] = tChart1.Series[i].Points[cc1].XValue;
                                rawData[i2, 4] = tChart1.Series[i].Points[cc1].YValues[0];
                                i2++;
                            }
                            i1++;


                        }
                }
                else
                {
                    rawData[i2, 0] = "row";
                    rawData[i2, 1] = "X value";
                    rawData[i2, 2] = "Y value";
                    i2++;
                    for (int cc1 = 0; cc1 < tChart2.Series[0].Points.Count; cc1++)
                    {
                        rawData[i2, 0] = cc1 + 1;
                        rawData[i2, 1] = tChart2.Series[0].Points[cc1].XValue;
                        rawData[i2, 2] = tChart2.Series[0].Points[cc1].YValues[0];
                        i2++;
                    }
                }

                // محاسبه نام ستونهای اکسل

                string finalColLetter = string.Empty;
                string colCharset = "ABEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (colnum1 > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring((colnum1 - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring((colnum1 - 1) % colCharsetLen, 1);

                // ساخت یک Sheet
                Worksheet excelSheet = (Worksheet)excelWorkbook.Sheets.Add(
                    excelWorkbook.Sheets.get_Item(++sheetIndex),
                    Type.Missing, 1, XlSheetType.xlWorksheet);
                //تنظیم نام شیت به نام دلخواه
                //excelSheet.Name = "List";
                //تنظیم خاصیت راست به چپ برای نمایش اطلاعات
                excelSheet.DisplayRightToLeft = false;


                // تعیین محدوده سطرها و ستونها
                string excelRange = string.Format("A1:{0}{1}", finalColLetter, rownum1 + 1);
                //انتقال اطلاعات از آرایه به شیت مورد نظر
                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;

                // ضخیم کردن اولین سطر برای عنوان ستونها
                ((Range)excelSheet.Rows[1, Type.Missing]).Font.Bold = true;

                // تنظیم عرض ستونها به اندازه محتوای ستونها
                for (int col = 0; col < colnum1; col++)
                {
                    ((Range)excelSheet.Columns[col + 1]).EntireColumn.AutoFit();
                }


                //ذخیره و بستن  Workbook
                excelWorkbook.SaveAs(outputPath, XlFileFormat.xlWorkbookNormal, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                excelWorkbook.Close(true, Type.Missing, Type.Missing);
                excelWorkbook = null;

                excelApp.Quit();
                excelApp = null;

                // Collect the unreferenced objects
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return "Saving in excel file is successfully done";
            }
            catch (Exception ex)
            {
                //بدست آوردن کد خطا برای مدیریت خطاها
                int code = System.Runtime.InteropServices.Marshal.GetExceptionCode();

                return ex.Message + code;
            }
        }

        private void toolStripButton10_Click_1(object sender, EventArgs e)
        {

            int shomarcheck = 0;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                }
            if (shomarcheck > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.AddExtension = true;
                sfd.DefaultExt = "xlsx";
                sfd.Filter = "Excel 97-2003 Workbook(*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(ExportToExcel(5, 1, sfd.FileName), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select one technique!!!");
            }
        }

        private void toolStripButton9_Click_1(object sender, EventArgs e)
        {
            int shomarcheck = 0, i1 = 1, i2 = 1;
            for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                if (tvTechList.Nodes[0].Nodes[i].Checked)
                {
                    shomarcheck++;
                }
            if (shomarcheck > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.AddExtension = true;
                sfd.DefaultExt = "txt";
                sfd.Filter = "Text document(*.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StreamWriter Fl = new StreamWriter(sfd.FileName, false, Encoding.ASCII);



                        i2++;
                        for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                        {
                            if (tvTechList.Nodes[0].Nodes[i].Checked)
                            {
                                Fl.WriteLine(i1.ToString().Trim() + "-" + clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())]);
                                Fl.WriteLine("---------------------------------------------------");
                                Fl.WriteLine("row number      X value         Y value");
                                Fl.WriteLine("---------------------------------------------------");

                                for (int cc1 = 0; cc1 < tChart1.Series[i].Points.Count; cc1++)
                                {
                                    Fl.Write((cc1 + 1).ToString().Trim() + "\t\t");
                                    Fl.Write(tChart1.Series[i].Points[cc1].XValue.ToString() + "\t\t");
                                    Fl.WriteLine(tChart1.Series[i].Points[cc1].YValues[0]);
                                    i2++;
                                }
                                i1++;
                                Fl.WriteLine("");


                            }
                        }

                        Fl.Close();
                        MessageBox.Show("Done!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select one technique!!!");
            }
        }

        private void toolStripButton8_Click_1(object sender, EventArgs e)
        {
            int i1 = 1, i2 = 1;

            if (tChart2.Series[0].Points.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.AddExtension = true;
                sfd.DefaultExt = "txt";
                sfd.Filter = "Text document(*.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StreamWriter Fl = new StreamWriter(sfd.FileName, false, Encoding.ASCII);
                        i2++;

                        Fl.WriteLine(tChart2.Titles[0].Text);
                        Fl.WriteLine("---------------------------------------------------");
                        Fl.WriteLine("row number      X value         Y value");
                        Fl.WriteLine("---------------------------------------------------");

                        for (int cc1 = 0; cc1 < tChart2.Series[0].Points.Count; cc1++)
                        {
                            Fl.Write((cc1 + 1).ToString().Trim() + "\t\t");
                            Fl.Write(tChart2.Series[0].Points[cc1].XValue.ToString() + "\t\t");
                            Fl.WriteLine(tChart2.Series[0].Points[cc1].YValues[0]);
                            i2++;
                        }
                        i1++;
                        Fl.WriteLine("");




                        Fl.Close();
                        MessageBox.Show("Done!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select one technique!!!");
            }
        }

        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (tChart2.Series.Count <= 0)
                    return;
                if (tChart2.Series[0].Points.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "xls";
                    sfd.Filter = "Excel 97-2003 Workbook(*.xls)|*.xls";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show(ExportToExcel(3, 2, sfd.FileName), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            forms2065.frmselecttypeshow frm = new forms2065.frmselecttypeshow();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.type_txt_y.Trim() != "")
                    tChart1.ChartAreas[0].AxisY.LabelStyle.Format = frm.type_txt_y;
                if (frm.type_txt_x.Trim() != "")
                    tChart1.ChartAreas[0].AxisX.LabelStyle.Format = frm.type_txt_x;
                tChart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void btnshowvalue_Click(object sender, EventArgs e)
        {
            if (Chart1_ActionState == "showvalue")
                Chart1_ActionState = "";
            else
                Chart1_ActionState = "showvalue";
            selectpchart_func(false);
        }

        private void selectpchart_func(bool state)
        {

            tChart1.ChartAreas[0].CursorX.IsUserEnabled = state;
            tChart1.ChartAreas[0].CursorY.IsUserEnabled = state;
            tChart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = state;
            tChart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = state;
            tChart1.ChartAreas[0].CursorX.Interval = 0;
            tChart1.ChartAreas[0].CursorY.Interval = 0;
            if (state)
            {
                tChart1.ChartAreas[0].CursorX.LineWidth = 1;
                tChart1.ChartAreas[0].CursorY.LineWidth = 1;
            }
            else
            {
                tChart1.ChartAreas[0].CursorX.LineWidth = 0;
                tChart1.ChartAreas[0].CursorY.LineWidth = 0;
            }
        }

        private void btnselectonly_Click(object sender, EventArgs e)
        {
            if (btnSelectAreaTS.Checked)
            {
                Chart1_ActionState = "selectonly";
                selectpchart_func(true);
                tChart1.Cursor = Cursors.Default;
                btnzoominTS.Checked = false;
                btnZoomOutTS.Checked = false;
            }
            else
            {
                selectpchart_func(false);
                Chart1_ActionState = "";
            }
        }
        double mDown = double.NaN, mDowny = double.NaN, difxview2 = double.NaN, difyview2 = double.NaN;

        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadCursorFromFile(String str);

        public static System.Windows.Forms.Cursor LoadCursorFromResource(System.Drawing.Icon icono)  // Assuming that the resource is an Icon, but also could be a Image or a Bitmap
        {
            string fileName1 = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".cur";
            using (var fileStream = File.Open(fileName1, FileMode.Create))
            {
                icono.Save(fileStream);
            }
            System.Windows.Forms.Cursor result = new System.Windows.Forms.Cursor(LoadCursorFromFile(fileName1));
            File.Delete(fileName1);

            return result;
        }

        private int zoomClickCount = 0;
        private double originalMinX, originalMaxX;

        private void btnZoomIn_Click(object sender, EventArgs e)
        {

            if (btnzoominTS.Checked)
            {
                btnZoomOutTS.Checked = false;
                btnSelectAreaTS.Checked = false;
                tChart1.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.zoomin);
                Chart1_ActionState = "zoomin";
            }
            else
            {
                Chart1_ActionState = "";

                tChart1.Cursor = Cursors.Default;
            }

            // فقط بار اول، بازه‌ی اصلی محور X را ذخیره می‌کنیم
            if (zoomClickCount == 0)
            {
                System.Windows.Forms.DataVisualization.Charting.ChartArea area = tChart1.ChartAreas[0];
                originalMinX = area.AxisX.Minimum;
                originalMaxX = area.AxisX.Maximum;
            }

        }

        private DateTime lastZoomOutClickTime = DateTime.MinValue;
        private const int DoubleClickThresholdMs = 400; // بازه زمانی برای تشخیص دابل کلیک

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ChartArea area = tChart1.ChartAreas[0];
                double totalRange = originalMaxX - originalMinX;


                switch (Chart1_ActionState)
                {
                    case "zoomin":


                        // تبدیل مختصات پیکسلی کلیک به مقدار روی محور X
                        double clickValueX = area.AxisX.PixelPositionToValue(e.X);

                        zoomClickCount++;

                        double zoomFactor = 1 - (zoomClickCount * 0.10); // هر کلیک 20٪ بیشتر زوم

                        if (zoomFactor < 0.1) zoomFactor = 0.1; // جلوگیری از صفر یا منفی شدن بازه

                        double newRange = totalRange * zoomFactor;

                        double newMin = clickValueX - newRange / 2;
                        double newMax = clickValueX + newRange / 2;

                        // اگر بازه جدید از محدوده اصلی خارج شد، اصلاحش می‌کنیم
                        if (newMin < originalMinX)
                        {
                            newMin = originalMinX;
                            newMax = newMin + newRange;
                        }
                        if (newMax > originalMaxX)
                        {
                            newMax = originalMaxX;
                            newMin = newMax - newRange;
                        }

                        area.AxisX.ScaleView.Zoom(newMin, newMax);
                        break;

                    case "zoomout":

                        DateTime now = DateTime.Now;
                        bool isDoubleClick = (now - lastZoomOutClickTime).TotalMilliseconds <= DoubleClickThresholdMs;
                        lastZoomOutClickTime = now;


                        if (isDoubleClick)
                        {
                            // دو کلیک پشت‌سرهم -> بازگشت کامل به حالت اولیه
                            area.AxisX.ScaleView.ZoomReset();
                            zoomClickCount = 0;
                            lastZoomOutClickTime = DateTime.MinValue; // جلوگیری از زنجیره‌ای شدن با کلیک بعدی
                            return;
                        }

                        // اگر اصلا زومی روی نمودار نیست، کاری لازم نیست انجام شود
                        if (!area.AxisX.ScaleView.IsZoomed)
                            return;

                        double currentMin = area.AxisX.ScaleView.ViewMinimum;
                        double currentMax = area.AxisX.ScaleView.ViewMaximum;
                        double center = (currentMin + currentMax) / 2;

                        double expandAmount = totalRange * 0.10; // 10% از کل بازه اصلی باز می‌شود

                        newMin = currentMin - expandAmount / 2;
                        newMax = currentMax + expandAmount / 2;

                        if (newMin <= originalMinX && newMax >= originalMaxX)
                        {
                            // اگر با این باز شدن، کل بازه پوشش داده شد -> ریست کامل
                            area.AxisX.ScaleView.ZoomReset();
                            zoomClickCount = 0;
                        }
                        else
                        {
                            if (newMin < originalMinX) newMin = originalMinX;
                            if (newMax > originalMaxX) newMax = originalMaxX;

                            area.AxisX.ScaleView.Zoom(newMin, newMax);

                            // برای هماهنگی با شمارنده کلیک روی نمودار (اختیاری)
                            if (zoomClickCount > 0) zoomClickCount--;
                        }
                        break;

                    default:
                        break;
                }



            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnzoomout_Click(object sender, EventArgs e)
        {
            if (btnZoomOutTS.Checked)
            {
                btnzoominTS.Checked = false;
                btnSelectAreaTS.Checked = false;
                tChart1.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.Zoomout);
                Chart1_ActionState = "zoomout";
            }
            else
            {
                Chart1_ActionState = "";

                tChart1.Cursor = Cursors.Default;

            }

        }

        private void btnshowgrid_Click(object sender, EventArgs e)
        {
            Chart1_ActionState = "";
            selectpchart_func(false);
            this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled = !this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled = !this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled;
        }

        private void btnshowaxes_Click(object sender, EventArgs e)
        {
            Chart1_ActionState = "";
            selectpchart_func(false);
            tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled;
            tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled;
            tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled;
        }

        private void btnexport_Click(object sender, EventArgs e)
        {
            forms2065.frmselectfiletype frm = new forms2065.frmselectfiletype();
            frm.StartPosition = FormStartPosition.CenterScreen;
            int i1 = 0, shomarcheck = 0, filetype = 0;
            string strlegend = "";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                filetype = frm.filetype_select;
            }

            switch (filetype)
            {
                case 1:
                    toolStripButton9_Click_1(sender, e);
                    break;
                case 2:
                    toolStripButton10_Click_1(sender, e);
                    break;
                case 3:

                    double xminb = 0, xmaxb = 0, yminb = 0, ymaxb = 0, shomartypevi = 0, shomartypevt = 0, shomartypeit = 0, shomartypeqt = 0;
                    for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                        if (tvTechList.Nodes[0].Nodes[i].Checked)
                        {
                            if (clasglobal.HAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "P" && clasglobal.VAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "C")
                                shomartypevi++;
                            else if (clasglobal.HAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "T" && clasglobal.VAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "C")
                                shomartypeit++;
                            else if (clasglobal.HAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "T" && clasglobal.VAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "P")
                                shomartypevt++;
                            else if (clasglobal.HAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "T" && clasglobal.VAxisTitle2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())].Substring(0, 1) == "Q")
                                shomartypeqt++;

                            shomarcheck++;
                            i1 = i;
                            if (shomarcheck == 1)
                            {
                                xminb = tChart1.Series[i].Points.FindMinByValue("X").XValue;
                                xmaxb = tChart1.Series[i].Points.FindMaxByValue("X").XValue;
                                yminb = tChart1.Series[i].Points.FindMinByValue("Y").YValues[0];
                                ymaxb = tChart1.Series[i].Points.FindMaxByValue("Y").YValues[0];

                            }
                            else
                            {
                                if (xminb > tChart1.Series[i].Points.FindMinByValue("X").XValue)
                                    xminb = tChart1.Series[i].Points.FindMinByValue("X").XValue;
                                if (xmaxb < tChart1.Series[i].Points.FindMaxByValue("X").XValue)
                                    xmaxb = tChart1.Series[i].Points.FindMaxByValue("X").XValue;
                                if (yminb > tChart1.Series[i].Points.FindMinByValue("Y").YValues[0])
                                    yminb = tChart1.Series[i].Points.FindMinByValue("Y").YValues[0];
                                if (ymaxb < tChart1.Series[i].Points.FindMaxByValue("Y").YValues[0])
                                    ymaxb = tChart1.Series[i].Points.FindMaxByValue("Y").YValues[0];
                            }

                        }
                    string titlegraph = "";
                    if (shomartypevi > 0 && shomartypevt == 0 && shomartypeqt == 0 && shomartypeit == 0)
                        titlegraph = "IV";
                    else if (shomartypevi == 0 && (shomartypevt > 0 && shomartypeqt == 0 && shomartypeit == 0))
                        titlegraph = "VT";
                    else if (shomartypevi == 0 && (shomartypevt == 0 && shomartypeqt > 0 && shomartypeit == 0))
                        titlegraph = "QT";
                    else if (shomartypevi == 0 && (shomartypevt == 0 && shomartypeqt == 0 && shomartypeit > 0))
                        titlegraph = "IT";
                    if (shomarcheck == 0)
                    {
                        MessageBox.Show("EXPORTING works with one graph" + (char)13 + "Please select ONE !!!");
                    }
                    else if (shomarcheck == 1)
                    {
                        strlegend = fillparam(2, i1);
                        forms2065.frmshowgraph frm1 = new forms2065.frmshowgraph(strlegend, xminb, xmaxb, yminb, ymaxb, titlegraph, "");
                        if (frm1.Chart1.Series.Count > 0)
                            frm1.Chart1.Series[0].Points.Clear();
                        for (int i = 0; i < tChart1.Series[i1].Points.Count; i++)
                        {
                            frm1.Chart1.Series[0].Points.AddXY(tChart1.Series[i1].Points[i].XValue, tChart1.Series[i1].Points[i].YValues[0]);
                        }
                        frm1.StartPosition = FormStartPosition.CenterScreen;
                        frm1.ShowDialog();
                    }
                    else
                    {
                        // MessageBox.Show("EXPORTING works with one graph" + (char)13 + "Please select ONE only!!!");
                        strlegend = "";

                        forms2065.frmshowgraph frm2 = new forms2065.frmshowgraph(strlegend, xminb, xmaxb, yminb, ymaxb, titlegraph, "");
                        if (frm2.Chart1.Series.Count > 0)
                        {
                            frm2.Chart1.Series[0].Points.Clear();
                            if (dschart2.analyselist.Rows[0]["comment"].ToString().Trim() != "")
                                frm2.Chart1.Series[0].LegendText = clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[0][0].ToString())] + "(" + dschart2.analyselist.Rows[0]["comment"].ToString().Trim() + ")";
                            else
                                frm2.Chart1.Series[0].LegendText = clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[0][0].ToString())];

                        }
                        int b = 0;
                        for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                            if (tvTechList.Nodes[0].Nodes[i].Checked)
                            {
                                if (frm2.Chart1.Series.Count <= b)
                                {
                                    frm2.Chart1.Series.Add(tvTechList.Nodes[0].Nodes[i].Name);
                                    frm2.Chart1.Series[b].ChartType = SeriesChartType.FastLine;
                                    if (dschart2.analyselist.Rows[i]["comment"].ToString().Trim() != "")
                                        frm2.Chart1.Series[b].LegendText = clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())] + "(" + dschart2.analyselist.Rows[i]["comment"].ToString().Trim() + ")";
                                    else
                                        frm2.Chart1.Series[b].LegendText = clasglobal.TechName2080[int.Parse(dschart2.analyselist.Rows[i][0].ToString())];

                                }
                                for (int i11 = 0; i11 < tChart1.Series[i].Points.Count; i11++)
                                {
                                    frm2.Chart1.Series[b].Points.AddXY(tChart1.Series[i].Points[i11].XValue, tChart1.Series[i].Points[i11].YValues[0]);
                                }
                                b++;

                            }
                        frm2.StartPosition = FormStartPosition.CenterScreen;
                        frm2.ShowDialog();
                    }
                    break;
                default:
                    break;

            }
        }


        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {
            mDown = double.NaN;
            mDowny = double.NaN;
        }

        private void btnselectline_Click(object sender, EventArgs e)
        {
            numline++;
            if (numline >= drawline1.Lines.Count)
                numline = 0;
            if (drawline1.Lines.Count == 0)
            {
                MessageBox.Show("There is not any line for selecting !!!");
            }

            tChart2.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog cl = new ColorDialog();
                cl.Color = tChart1.ChartAreas[0].BackColor;
                cl.CustomColors = new int[] { tChart1.ChartAreas[0].BackColor.R, tChart1.ChartAreas[0].BackColor.G, tChart1.ChartAreas[0].BackColor.B };
                cl.FullOpen = true;

                if (cl.ShowDialog() == DialogResult.OK)
                {
                    tChart1.BackColor = cl.Color;
                    tChart1.BorderlineColor = cl.Color;
                    tChart1.ChartAreas[0].BackColor = cl.Color;
                    tChart2.BackColor = cl.Color;
                    tChart2.BorderlineColor = cl.Color;
                    tChart2.ChartAreas[0].BackColor = cl.Color;
                    pnlGraph.BackColor = cl.Color;
                    panel2.BackColor = cl.Color;
                    panel8.BackColor = cl.Color;

                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[5] = "2080 analize:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog cl = new ColorDialog();
                cl.FullOpen = true;

                if (cl.ShowDialog() == DialogResult.OK)
                {
                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[8] = "2065 graph analysis:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                    //for (int i = 0; i < OnlineChart.Series.Count; i++)
                    TreeNode node = tvTechList.SelectedNode;
                    if (node.Level > 1)
                    {
                        node = node.Parent;
                    }

                    int i = node.Index;
                    ITechniques tech = TechLine[i];
                    tech.ColorCode = cl.Color.ToArgb();
                    node.ForeColor = cl.Color;

                    tChart1.Series[$"{tech.Name}-{i}"].Color = cl.Color;
                    foreach (TreeNode child in node.Nodes)
                    {
                        child.ForeColor = cl.Color;
                        tChart1.Series[$"{tech.Name}-{i}_{child.Index + 1}"].Color = cl.Color;
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                pnl_peak_search.Visible = true;

                if (tChart2.Series.Count == 0)
                {
                    txt_res.Text = (tChart1.Series[0].Points.Count / 20).ToString();//این عدد تجربی است
                    txt_noise.Text = "0";
                }
            }
            catch { }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (tChart1.Series.Count == 0) { MessageBox.Show("گرافی موجود نیست."); return; }

                // ─── پیدا کردن نود والد تیک‌خورده ───
                int j = -1;
                for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                {
                    if (tvTechList.Nodes[0].Nodes[i].Checked)
                    {
                        j = i;
                        for (int k = i + 1; k < tvTechList.Nodes[0].Nodes.Count; k++)
                            tvTechList.Nodes[0].Nodes[k].Checked = false;
                        break;
                    }
                }
                if (j == -1 && tvTechList.Nodes[0].Nodes.Count > 0)
                {
                    j = 0;
                    tvTechList.Nodes[0].Nodes[0].Checked = true;
                }
                if (j == -1) { MessageBox.Show("لطفاً یک گراف را تیک بزنید."); return; }

                tvTechList.SelectedNode = tvTechList.Nodes[0].Nodes[j];
                tvTechList.Focus();
                radioButton1.Checked = true;

                string techName = tvTechList.Nodes[0].Nodes[j].Text;
                bool isCV = techName.ToUpper().StartsWith("CV");

                // ─── تعیین preferMax از E1/E2 ───
                bool preferMax = true;
                if (!isCV)
                {
                    double E1 = 9999, E2 = 9999;
                    try
                    {
                        if (TechLine != null && j >= 0 && j < TechLine.Count)
                        {
                            var techObj = TechLine[j];
                            switch (techObj.Number)
                            {
                                case 1: { NPV t = techObj as NPV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                                case 2: { DPV t = techObj as DPV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                                case 3: { SWV t = techObj as SWV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                                case 4: { CV t = techObj as CV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                                case 5: { LSV t = techObj as LSV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                            }
                        }
                    }
                    catch { }
                    if (E1 == 9999 || E2 == 9999)
                    {
                        try
                        {
                            if (dschart2 != null && dschart2.analyselist != null &&
                                j >= 0 && j < dschart2.analyselist.Rows.Count)
                            {
                                DataRow row = dschart2.analyselist.Rows[j];
                                E1 = Convert.ToDouble(row[1]);
                                E2 = Convert.ToDouble(row[2]);
                            }
                        }
                        catch { }
                    }
                    if (E1 != 9999 && E2 != 9999 && E1 != E2)
                        preferMax = (E1 < E2);
                }

                // ─── پیدا کردن سری مورد نظر ───
                Series ser = null;

                if (isCV && tvTechList.Nodes[0].Nodes[j].Nodes.Count > 0)
                {
                    // ★★★ تعیین سیکل: نودی که کاربر روی آن کلیک کرده (SelectedNode) یا تیک خورده ★★★
                    TreeNode targetChild = null;

                    // ۱) اگر نود انتخاب فعلی یک فرزند از همان والد است → همان
                    if (tvTechList.SelectedNode != null &&
                        tvTechList.SelectedNode.Level == 2 &&
                        tvTechList.SelectedNode.Parent == tvTechList.Nodes[0].Nodes[j])
                    {
                        targetChild = tvTechList.SelectedNode;
                    }

                    // ۲) در غیر این صورت اولین فرزند تیک‌خورده
                    if (targetChild == null)
                    {
                        foreach (TreeNode child in tvTechList.Nodes[0].Nodes[j].Nodes)
                        {
                            if (child.Checked) { targetChild = child; break; }
                        }
                    }

                    // ۳) اگر هیچکدام، اولین فرزند
                    if (targetChild == null)
                        targetChild = tvTechList.Nodes[0].Nodes[j].Nodes[0];

                    int cycleNum = targetChild.Index + 1;
                    string seriesName = techName + "-" + j + "_" + cycleNum;
                    int sIdx = tChart1.Series.IndexOf(seriesName);
                    if (sIdx >= 0) ser = tChart1.Series[sIdx];

                    // fallback
                    if (ser == null || ser.Points.Count == 0)
                    {
                        string childPrefix = techName + "-" + j + "_";
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(childPrefix) && tChart1.Series[s].Points.Count > 0)
                            { ser = tChart1.Series[s]; break; }
                    }
                }
                else
                {
                    if (tvTechList.Nodes[0].Nodes[j].Nodes.Count > 0)
                    {
                        string childPrefix = techName + "-" + j + "_";
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(childPrefix) && tChart1.Series[s].Points.Count > 0)
                            { ser = tChart1.Series[s]; break; }
                    }
                    if (ser == null)
                    {
                        string parentName = techName + "-" + j;
                        int idx = tChart1.Series.IndexOf(parentName);
                        if (idx >= 0 && tChart1.Series[parentName].Points.Count > 0)
                            ser = tChart1.Series[parentName];
                    }
                    if (ser == null)
                    {
                        string prefix = techName + "-" + j;
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(prefix) && tChart1.Series[s].Points.Count > 0)
                            { ser = tChart1.Series[s]; break; }
                    }
                    if (ser == null)
                    {
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Points.Count > 0)
                            { ser = tChart1.Series[s]; break; }
                    }
                }

                if (ser == null || ser.Points.Count == 0)
                {
                    MessageBox.Show("سری داده‌ای یافت نشد.");
                    return;
                }

                int count = ser.Points.Count;
                if (count < 20) { MessageBox.Show("تعداد نقاط کافی نیست."); return; }

                Array.Clear(volt, 0, volt.Length);
                Array.Clear(current, 0, current.Length);
                Counter_num = count;
                Array.Resize(ref volt, Counter_num);
                Array.Resize(ref current, Counter_num);
                for (int i = 0; i < Counter_num; i++)
                {
                    volt[i] = ser.Points[i].XValue;
                    current[i] = ser.Points[i].YValues[0];
                }

                Array.Clear(series_height_index, 0, series_height_index.Length);
                Array.Resize(ref series_height_index, 0);
                Array.Clear(series_width_index, 0, series_width_index.Length);
                Array.Resize(ref series_width_index, 0);
                Array.Clear(series_area_index, 0, series_area_index.Length);
                Array.Resize(ref series_area_index, 0);

                tChart2.Series.Clear();
                tChart2.Refresh();

                tChart2.Series.Add(new Series());
                tChart2.Series[0].ChartType = SeriesChartType.Spline;
                tChart2.Series[0].Color = ser.Color;
                tChart2.Series[0].BorderWidth = 2;
                tChart2.Titles[0].Text = "Peak Search - " + ser.Name;
                tChart2.ChartAreas[0].AxisY.Title = tChart1.ChartAreas[0].AxisY.Title;
                tChart2.ChartAreas[0].AxisX.Title = tChart1.ChartAreas[0].AxisX.Title;
                for (int i = 0; i < Counter_num; i++)
                    tChart2.Series[0].Points.AddXY(volt[i], current[i]);

                var finder = new FindPeakAuto();
                finder.ProminenceRatio = 0.10;
                finder.MinProminence = 0;
                finder.SmoothWindow = 5;
                finder.MaxWindow = 150;
                finder.PreferMaxOverride = preferMax;

                finder.Draw(tChart2, volt, current);

                series_height_index = finder.HeightIndices;
                series_width_index = finder.WidthIndices;
                series_area_index = finder.AreaIndices;

                ch_height.Checked = true;
                ch_width.Checked = true;
                ch_area.Checked = true;

                ch_height_CheckedChanged(null, EventArgs.Empty);
                ch_width_CheckedChanged(null, EventArgs.Empty);
                ch_area_CheckedChanged(null, EventArgs.Empty);

                tChart2.ChartAreas[0].RecalculateAxesScale();
                tChart2.Refresh();
                UpdateMyNewButtonState();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                this.Cursor = Cursors.Default;
                tChart2.Cursor = Cursors.Default;
            }
        }

        private static double InterpolateX(
        double x1,
        double y1,
        double x2,
        double y2,
        double targetY)
        {
            double dy = y2 - y1;

            if (Math.Abs(dy) < double.Epsilon)
                return x1;

            return x1 + (targetY - y1) * (x2 - x1) / dy;
        }

        private static bool TryFindHalfWidth(
        int peak,
        int startIndex,
        int endIndex,
        double halfHeight,
        double[] volt,
        double[] current,
        out double leftX,
        out double rightX)
        {
            leftX = 0;
            rightX = 0;


            // ========================================================
            // سمت چپ Peak
            // ========================================================

            bool foundLeft = false;

            for (int k = peak; k > startIndex; k--)
            {
                if (current[k] <= halfHeight)
                {
                    if (current[k] == halfHeight)
                    {
                        leftX = volt[k];
                    }
                    else
                    {
                        leftX = InterpolateX(
                            volt[k],
                            current[k],
                            volt[k + 1],
                            current[k + 1],
                            halfHeight);
                    }

                    foundLeft = true;
                    break;
                }
            }

            if (!foundLeft)
                return false;


            // ========================================================
            // سمت راست Peak
            // ========================================================

            bool foundRight = false;

            for (int k = peak; k < endIndex; k++)
            {
                if (current[k] <= halfHeight)
                {
                    if (current[k] == halfHeight)
                    {
                        rightX = volt[k];
                    }
                    else
                    {
                        rightX = InterpolateX(
                            volt[k],
                            current[k],
                            volt[k - 1],
                            current[k - 1],
                            halfHeight);
                    }

                    foundRight = true;
                    break;
                }
            }

            return foundRight;
        }

        private static int FindPeakStart(
        int peak,
        double[] current,
        int resultNoise)
        {
            double start = current[peak];
            int startIndex = peak;
            int noise = 0;

            for (int k = peak; k > 0; k--)
            {
                if (current[k - 1] <= start)
                {
                    start = current[k - 1];
                    startIndex = k - 1;
                    noise = 0;
                }
                else
                {
                    noise++;

                    if (noise >= resultNoise)
                        break;
                }
            }

            return startIndex;
        }

        private static int FindPeakEnd(
        int peak,
        int forwardCount,
        double[] current,
        int resultNoise)
        {
            double end = current[peak];
            int endIndex = peak;
            int noise = 0;

            for (int k = peak; k < forwardCount - 1; k++)
            {
                if (current[k + 1] <= end)
                {
                    end = current[k + 1];
                    endIndex = k + 1;
                    noise = 0;
                }
                else
                {
                    noise++;

                    if (noise >= resultNoise)
                        break;
                }
            }

            return endIndex;
        }

        private static double GetLineY(
        double x,
        double x1,
        double y1,
        double x2,
        double y2)
        {
            double dx = x2 - x1;

            if (Math.Abs(dx) < double.Epsilon)
                return y1;

            return y1 + (y2 - y1) * (x - x1) / dx;
        }

        private static double CalculatePeakHeight(
        int peak,
        int startIndex,
        int endIndex,
        double[] volt,
        double[] current)
        {
            double baselineAtPeak = GetLineY(
                volt[peak],
                volt[startIndex],
                current[startIndex],
                volt[endIndex],
                current[endIndex]);

            return Math.Abs(current[peak] - baselineAtPeak);
        }

        private static double CalculatePeakArea(
        int startIndex,
        int endIndex,
        double[] volt,
        double[] current)
        {
            double area = 0;

            double x1 = volt[startIndex];
            double y1 = current[startIndex];

            double x2 = volt[endIndex];
            double y2 = current[endIndex];


            for (int k = startIndex + 1; k <= endIndex; k++)
            {
                double baseline1 = GetLineY(
                    volt[k - 1],
                    x1,
                    y1,
                    x2,
                    y2);

                double baseline2 = GetLineY(
                    volt[k],
                    x1,
                    y1,
                    x2,
                    y2);


                double netCurrent1 =
                    Math.Max(0, current[k - 1] - baseline1);

                double netCurrent2 =
                    Math.Max(0, current[k] - baseline2);


                double dx =
                    volt[k] - volt[k - 1];


                area +=
                    dx *
                    (netCurrent1 + netCurrent2) /
                    2.0;
            }

            return area;
        }

        private static void FixPeakBounds(
        ref int startIndex,
        ref int endIndex,
        int peak,
        double[] volt,
        double[] current)
        {
            // ========================================================
            // اصلاح شروع Peak
            // ========================================================

            for (int k = startIndex; k < peak; k++)
            {
                double baseline = GetLineY(
                    volt[k],
                    volt[startIndex],
                    current[startIndex],
                    volt[endIndex],
                    current[endIndex]);

                if (baseline > current[k])
                {
                    startIndex = k + 1;
                }
            }



            // ========================================================
            // اصلاح پایان Peak
            // ========================================================

            for (int k = endIndex; k > peak; k--)
            {
                double baseline = GetLineY(
                    volt[k],
                    volt[startIndex],
                    current[startIndex],
                    volt[endIndex],
                    current[endIndex]);

                if (baseline > current[k])
                {
                    endIndex = k;
                }
            }
        }


        System.Drawing.PointF p1 = new System.Drawing.PointF(10, 10), p2 = new System.Drawing.PointF(100, 100);
        System.Drawing.PointF[] p1_arr = new System.Drawing.PointF[0];


        int[] series_height_index = new int[0];
        private void ch_height_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < series_height_index.Length; i++)
            {
                if (ch_height.Checked == true)
                    tChart2.Series[series_height_index[i]].Enabled = true;
                else
                    tChart2.Series[series_height_index[i]].Enabled = false;
            }
        }

        int[] series_width_index = new int[0];
        private void ch_width_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < series_width_index.Length; i++)
            {
                if (ch_width.Checked == true)
                    tChart2.Series[series_width_index[i]].Enabled = true;
                else
                    tChart2.Series[series_width_index[i]].Enabled = false;
            }
        }

        int[] series_area_index = new int[0];

        private void button5_Click(object sender, EventArgs e)
        {
            new_handle();
            button6.Enabled = true;
        }

        public void new_handle()
        {
            try
            {
                Array.Clear(series_height_index, 0, series_height_index.Length);
                Array.Resize(ref series_height_index, 0);
                Array.Clear(series_width_index, 0, series_width_index.Length);
                Array.Resize(ref series_width_index, 0);
                Array.Clear(series_area_index, 0, series_area_index.Length);
                Array.Resize(ref series_area_index, 0);

                tChart2.Series.Clear();
                tChart2.Refresh();

                if (tChart1.Series.Count == 0)
                {
                    MessageBox.Show("graph is not available.");
                    return;
                }

                // ─── پیدا کردن نود والد تیک‌خورده ───
                int j = -1;
                for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                {
                    if (tvTechList.Nodes[0].Nodes[i].Checked)
                    {
                        j = i;
                        break;
                    }
                }
                if (j == -1 && tvTechList.Nodes[0].Nodes.Count > 0)
                {
                    j = 0;
                    tvTechList.Nodes[0].Nodes[0].Checked = true;
                }
                if (j == -1)
                {
                    MessageBox.Show("A graph of the list needs to be checked..", "Error");
                    return;
                }

                tvTechList.SelectedNode = tvTechList.Nodes[0].Nodes[j];
                tvTechList.Focus();
                radioButton1.Checked = true;

                string techName = tvTechList.Nodes[0].Nodes[j].Text;
                bool isCV = techName.ToUpper().StartsWith("CV");

                // ─── پیدا کردن سری مورد نظر (دقیقاً مثل button11) ───
                Series selectedSeries = null;

                if (isCV && tvTechList.Nodes[0].Nodes[j].Nodes.Count > 0)
                {
                    // ★★★ تعیین سیکل: نودی که کاربر روی آن کلیک کرده یا تیک خورده ★★★
                    TreeNode targetChild = null;

                    // ۱) اگر SelectedNode فعلی یک فرزند از همین والد است → همان
                    if (tvTechList.SelectedNode != null &&
                        tvTechList.SelectedNode.Level == 2 &&
                        tvTechList.SelectedNode.Parent == tvTechList.Nodes[0].Nodes[j])
                    {
                        targetChild = tvTechList.SelectedNode;
                    }

                    // ۲) در غیر این صورت اولین فرزند تیک‌خورده
                    if (targetChild == null)
                    {
                        foreach (TreeNode child in tvTechList.Nodes[0].Nodes[j].Nodes)
                        {
                            if (child.Checked) { targetChild = child; break; }
                        }
                    }

                    // ۳) اگر هیچکدام، اولین فرزند
                    if (targetChild == null)
                        targetChild = tvTechList.Nodes[0].Nodes[j].Nodes[0];

                    int cycleNum = targetChild.Index + 1;
                    string seriesName = techName + "-" + j + "_" + cycleNum;
                    int sIdx = tChart1.Series.IndexOf(seriesName);
                    if (sIdx >= 0) selectedSeries = tChart1.Series[sIdx];

                    // fallback: اولین سری فرزند غیرخالی
                    if (selectedSeries == null || selectedSeries.Points.Count == 0)
                    {
                        string childPrefix = techName + "-" + j + "_";
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(childPrefix) && tChart1.Series[s].Points.Count > 0)
                            { selectedSeries = tChart1.Series[s]; break; }
                    }
                }
                else
                {
                    // ─── حالت غیر CV یا CV بدون فرزند ───
                    if (tvTechList.Nodes[0].Nodes[j].Nodes.Count > 0)
                    {
                        string childPrefix = techName + "-" + j + "_";
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(childPrefix) && tChart1.Series[s].Points.Count > 0)
                            { selectedSeries = tChart1.Series[s]; break; }
                    }
                    if (selectedSeries == null)
                    {
                        string parentName = techName + "-" + j;
                        int idx = tChart1.Series.IndexOf(parentName);
                        if (idx >= 0 && tChart1.Series[parentName].Points.Count > 0)
                            selectedSeries = tChart1.Series[parentName];
                    }
                    if (selectedSeries == null)
                    {
                        string prefix = techName + "-" + j;
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Name.StartsWith(prefix) && tChart1.Series[s].Points.Count > 0)
                            { selectedSeries = tChart1.Series[s]; break; }
                    }
                    if (selectedSeries == null)
                    {
                        for (int s = 0; s < tChart1.Series.Count; s++)
                            if (tChart1.Series[s].Points.Count > 0)
                            { selectedSeries = tChart1.Series[s]; break; }
                    }
                }

                if (selectedSeries == null || selectedSeries.Points.Count == 0)
                {
                    MessageBox.Show("سری داده‌ای برای این گراف یافت نشد.");
                    return;
                }

                Array.Clear(volt, 0, volt.Length);
                Array.Clear(current, 0, current.Length);
                Counter_num = selectedSeries.Points.Count;
                Array.Resize(ref volt, Counter_num);
                Array.Resize(ref current, Counter_num);

                txt_res.Text = (Convert.ToInt16(Counter_num / 20)).ToString();
                txt_noise.Text = "0";

                tChart2.Series.Clear();
                tChart2.Refresh();
                Thread.Sleep(50);

                tChart2.Series.Add(new Series());
                tChart2.Series[0].ChartType = SeriesChartType.Spline;
                tChart2.Series[0].Color = selectedSeries.Color;
                tChart2.Series[0].BorderWidth = 2;
                tChart2.Titles[0].Text = "Peak Search - " + selectedSeries.Name;
                tChart2.ChartAreas[0].AxisY.Title = tChart1.ChartAreas[0].AxisY.Title;
                tChart2.ChartAreas[0].AxisX.Title = tChart1.ChartAreas[0].AxisX.Title;

                for (int i = 0; i < Counter_num; i++)
                {
                    volt[i] = selectedSeries.Points[i].XValue;
                    current[i] = selectedSeries.Points[i].YValues[0];
                    tChart2.Series[0].Points.AddXY(volt[i], current[i]);
                }
                UpdateMyNewButtonState();
            }
            catch (Exception x)
            {
                MessageBox.Show("خطا در new_handle:\n" + x.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                tChart2.Cursor = Cursors.Default;
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tChart2.Series[0].ChartType = SeriesChartType.Line;
                tChart2.Series[0].BorderWidth = 2;
            }
            catch { }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tChart2.Series[0].ChartType = SeriesChartType.Point;
                tChart2.Series[0].BorderWidth = 1;
            }
            catch { }
        }

        int start_click = 0;
        int end_click = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            if (tChart2.Series.Count == 1)
            {
                tChart2.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());

                start_click = 1;
                button6.Enabled = false;

                this.Cursor = Cursors.UpArrow;
                tChart2.Cursor = Cursors.UpArrow;
            }
            else
                MessageBox.Show("You must first click the new button.", "Error", MessageBoxButtons.OK);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (tChart2.Series.Count < 2 || tChart2.Series[1].Points.Count != 2)
                {
                    MessageBox.Show("You should choose the start point and end of base line.",
                                    "Error", MessageBoxButtons.OK);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                Thread.Sleep(50);

                Array.Clear(series_height_index, 0, series_height_index.Length);
                Array.Resize(ref series_height_index, 0);
                Array.Clear(series_width_index, 0, series_width_index.Length);
                Array.Resize(ref series_width_index, 0);
                Array.Clear(series_area_index, 0, series_area_index.Length);
                Array.Resize(ref series_area_index, 0);

                double startX = tChart2.Series[1].Points[0].XValue;
                double startY = tChart2.Series[1].Points[0].YValues[0];
                double endX = tChart2.Series[1].Points[1].XValue;
                double endY = tChart2.Series[1].Points[1].YValues[0];

                if (endX < startX)
                {
                    double tmpX = startX; startX = endX; endX = tmpX;
                    double tmpY = startY; startY = endY; endY = tmpY;
                }

                // ─── گرفتن E1/E2 از TechLine ───
                double E1 = 9999, E2 = 9999;
                int j = -1;
                for (int i = 0; i < tvTechList.Nodes[0].Nodes.Count; i++)
                    if (tvTechList.Nodes[0].Nodes[i].Checked) { j = i; break; }

                if (j >= 0 && TechLine != null && j < TechLine.Count)
                {
                    var techObj = TechLine[j];
                    try
                    {
                        switch (techObj.Number)
                        {
                            case 1: { NPV t = techObj as NPV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                            case 2: { DPV t = techObj as DPV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                            case 3: { SWV t = techObj as SWV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                            case 4: { CV t = techObj as CV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                            case 5: { LSV t = techObj as LSV; if (t != null) { E1 = t.E1; E2 = t.E2; } } break;
                        }
                    }
                    catch { }
                }

                var finder = new Skydat.FindPeakManual();

                // ★★★ منطق E1/E2 حالا داخل Draw انجام میشود ★★★
                // فقط E1 و E2 را پاس میدهیم
                bool ok = finder.Draw(tChart2, volt, current,
                                      startSeriesIndex: 2,
                                      userSeriesIndex: 1,
                                      startX, startY, endX, endY,
                                      E1, E2);

                if (!ok)
                {
                    MessageBox.Show("محاسبه قله با خطا مواجه شد.");
                    return;
                }

                series_height_index = finder.HeightIndices;
                series_width_index = finder.WidthIndices;
                series_area_index = finder.AreaIndices;

                ch_height_CheckedChanged(null, EventArgs.Empty);
                ch_width_CheckedChanged(null, EventArgs.Empty);
                ch_area_CheckedChanged(null, EventArgs.Empty);

                tChart2.ChartAreas[0].RecalculateAxesScale();
                tChart2.Refresh();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                tChart2.Cursor = Cursors.Default;
            }
        }

        private void toolStrip2_Click(object sender, EventArgs e)
        {
            pnl_peak_search.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (tChart2.Series.Count > 0)
            {
                forms2065.frm_big_analys frm = new forms2065.frm_big_analys();
                frm.WindowState = FormWindowState.Maximized;

                foreach (Series ser in tChart2.Series)
                    frm.chart1.Series.Add(ser);

                frm.chart1.BackColor = tChart1.BackColor;
                frm.chart1.BorderlineColor = tChart1.BackColor;
                frm.chart1.ChartAreas[0].BackColor = tChart1.BackColor;
                frm.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                frm.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                if (tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled == true)
                {
                    frm.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    frm.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                }
                frm.ShowDialog();
            }
        }

        private void tChart1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton11_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn.Checked)
            {
                btn.BackColor = Color.Gray;
            }
            else
            {
                btn.BackColor = Color.Transparent;
            }
        }

        private void ch_area_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < series_area_index.Length; i++)
            {
                if (ch_area.Checked == true)
                    tChart2.Series[series_area_index[i]].Enabled = true;
                else
                    tChart2.Series[series_area_index[i]].Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (statebutton_1.Trim() == "drawline")
            {
                tsBtnDrawLine.Checked = false;
                statebutton_1 = "";
            }
            else
            {
                statebutton_1 = "drawline";
                tsBtnDrawLine.Checked = true;
            }
            selectpchart_func_1(false);
        }

        private void timerResizeFrm_Tick(object sender, EventArgs e)
        {
            timerResizeFrm.Stop();
            tsGraphTools.Height = (int)(this.Height * 0.05);
            tsFile.Height = (int)(this.Height * 0.07);

            tvTechList.Height = (int)(this.Height * 0.3);

            panel8.Width = (int)(tChart1.Width * 0.03);
            foreach (System.Windows.Forms.Button item in panel8.Controls)
            {
                item.Height = item.Width;
            }

            panel2.Width = (int)(tChart2.Width * 0.03);
            foreach (System.Windows.Forms.Button item in panel2.Controls)
            {
                item.Height = item.Width;
            }

            int imageSize = (int)(tChart2.Width * 0.025);
            toolStrip2.ImageScalingSize = toolStrip1.ImageScalingSize = new Size(imageSize, imageSize);
            toolStrip2.Width = toolStrip1.Width = imageSize + 10;


            if (this.Height != 0) splitContainer.SplitterDistance = (int)(splitContainer.Height - panel2.Height - 5);


        }

        private void frmanalyseGraph2080_Resize(object sender, EventArgs e)
        {
            timerResizeFrm.Stop();
            timerResizeFrm.Start();
        }

        private void selectpchart_func_1(bool state)
        {

            tChart2.ChartAreas[0].CursorX.IsUserEnabled = state;
            tChart2.ChartAreas[0].CursorY.IsUserEnabled = state;
            tChart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = state;
            tChart2.ChartAreas[0].CursorY.IsUserSelectionEnabled = state;
            tChart2.ChartAreas[0].CursorX.Interval = 0;
            tChart2.ChartAreas[0].CursorY.Interval = 0;
            if (state)
            {
                tChart2.ChartAreas[0].CursorX.LineWidth = 1;
                tChart2.ChartAreas[0].CursorY.LineWidth = 1;

            }
            else
            {
                tChart2.ChartAreas[0].CursorX.LineWidth = 0;
                tChart2.ChartAreas[0].CursorY.LineWidth = 0;



            }
            tChart2.Focus();
        }

        private void btnzoominTS_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectAreaTS_CheckedChanged(object sender, EventArgs e)
        {
            if (btnSelectAreaTS.Checked)
            {
                Chart1_ActionState = "selectonly";
                selectpchart_func(true);
                tChart1.Cursor = Cursors.Default;
                btnzoominTS.Checked = false;
                btnZoomOutTS.Checked = false;
            }
            else
            {
                selectpchart_func(false);
                Chart1_ActionState = "";
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (tChart2.Series.Count > 0)
            {
                forms2065.frm_big_analys frm = new forms2065.frm_big_analys();
                frm.WindowState = FormWindowState.Maximized;

                foreach (Series ser in tChart2.Series)
                    frm.chart1.Series.Add(ser);

                frm.chart1.BackColor = tChart1.BackColor;
                frm.chart1.BorderlineColor = tChart1.BackColor;
                frm.chart1.ChartAreas[0].BackColor = tChart1.BackColor;
                frm.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                frm.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                if (tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled == true)
                {
                    frm.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    frm.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                }
                frm.Show();
            }
        }

        private void btndeleteline_Click(object sender, EventArgs e)
        {
            if (drawline1.Lines.Count == 0)
            {
                MessageBox.Show("There is not any line for deleting !!!");


            }
            if (numline > -1 && numline < drawline1.Lines.Count)
            {
                drawline1.remove(numline);
                numline--;
                tChart2.Refresh();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnzoominTS_Click_1(object sender, EventArgs e)
        {

        }

        private void toolTipBtnshowvalue_Click(object sender, EventArgs e)
        {
            if (Chart1_ActionState == "showvalue")
                Chart1_ActionState = "";
            else
                Chart1_ActionState = "showvalue";
            selectpchart_func(false);
        }

        private void grayToolStripButton8_CheckedChanged(object sender, EventArgs e)
        {
            this.btnSelectLines.CheckedChanged -= new System.EventHandler(this.grayToolStripButton8_CheckedChanged);


            numline++;
            if (numline >= drawline1.Lines.Count)
            {
                numline = 0;
            }
            btnSelectLines.Checked = true;

            if (drawline1.Lines.Count == 0)
            {
                MessageBox.Show("There is not any line for selecting !!!");
                btnSelectLines.Checked = false;
                numline = -1;
            }

            tChart2.Refresh();

            this.btnSelectLines.CheckedChanged += new System.EventHandler(this.grayToolStripButton8_CheckedChanged);
        }

        private void grayToolStripButton9_Click(object sender, EventArgs e)
        {

        }

        private void grayToolStripButton8_Click(object sender, EventArgs e)
        {

        }

        private void grayToolStripButton9_CheckedChanged(object sender, EventArgs e)
        {
            this.grayToolStripButton9.CheckedChanged -= new System.EventHandler(this.grayToolStripButton9_CheckedChanged);

            if (drawline1.Lines.Count == 0)
            {
                MessageBox.Show("There is not any line for deleting !!!");
                grayToolStripButton9.Checked = false;
                btnSelectLines.Checked = false;

            }

            if (numline > -1 && numline < drawline1.Lines.Count)
            {
                drawline1.remove(numline);
                numline--;
                tChart2.Refresh();

                grayToolStripButton9.Checked = true;
            }



            this.grayToolStripButton9.CheckedChanged += new System.EventHandler(this.grayToolStripButton9_CheckedChanged);
        }
        private void ShowCustomResultsForm(double ecorr, double logIcorr, double icorr, double ba, double bc)
        {
            // 1. تنظیمات کلی فرم
            System.Windows.Forms.Form resultForm = new System.Windows.Forms.Form();
            resultForm.Text = "نتایج محاسبات تافل";
            resultForm.Size = new System.Drawing.Size(460, 500);
            resultForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            resultForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            resultForm.MaximizeBox = false;
            resultForm.MinimizeBox = false;
            resultForm.BackColor = System.Drawing.Color.White;
            resultForm.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            resultForm.RightToLeftLayout = true;
            resultForm.ShowIcon = false;

            // 2. ساخت هدر (Header)
            System.Windows.Forms.Panel headerPanel = new System.Windows.Forms.Panel();
            headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label();
            lblTitle.Text = "نتایج آنالیز تافل (Tafel Results)";
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            headerPanel.Controls.Add(lblTitle);

            // 3. ساخت فوتر (Footer)
            System.Windows.Forms.Panel footerPanel = new System.Windows.Forms.Panel();
            footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            footerPanel.Height = 70;
            footerPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            System.Windows.Forms.Button btnOk = new System.Windows.Forms.Button();
            btnOk.Text = "تایید و بستن";
            btnOk.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            btnOk.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Size = new System.Drawing.Size(140, 40);
            btnOk.Location = new System.Drawing.Point((resultForm.ClientSize.Width - btnOk.Width) / 2, 15);
            btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            footerPanel.Controls.Add(btnOk);

            // 4. ساخت جدول
            System.Windows.Forms.TableLayoutPanel table = new System.Windows.Forms.TableLayoutPanel();
            table.Dock = System.Windows.Forms.DockStyle.Fill;
            table.ColumnCount = 2;
            table.RowCount = 5;

            for (int i = 0; i < 5; i++)
            {
                table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            }

            table.Padding = new System.Windows.Forms.Padding(15);
            table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));

            // متد کمکی برای اضافه کردن سطر
            Action<string, string, System.Drawing.Color> AddRow = (labelText, valueText, valueColor) =>
            {
                // ستون اول (متن سمت راست)
                System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
                lbl.Text = labelText;
                lbl.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                lbl.Dock = System.Windows.Forms.DockStyle.Fill;
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                lbl.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);

                // ستون دوم (مقدار سمت چپ)
                System.Windows.Forms.Label val = new System.Windows.Forms.Label();
                val.Text = valueText;
                val.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                val.ForeColor = valueColor;
                val.Dock = System.Windows.Forms.DockStyle.Fill;
                // ★★★ تغییر به MiddleCenter برای اینکه مقدار دقیقاً وسط سلول قرار بگیرد ★★★
                val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                val.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);

                table.Controls.Add(lbl);
                table.Controls.Add(val);
            };

            System.Drawing.Color colorEcorr = System.Drawing.Color.DarkBlue;
            System.Drawing.Color colorIcorr = System.Drawing.Color.DarkRed;
            System.Drawing.Color colorSlope = System.Drawing.Color.FromArgb(64, 64, 64);

            // ★★★ اضافه کردن کاراکتر \u200E (چپ‌چین اجباری) به ابتدای رشته‌ها ★★★
            AddRow("پتانسیل خوردگی (Ecorr):", "\u200E" + $"{ecorr:0.0000} V", colorEcorr);
            AddRow("جریان خوردگی (Icorr):", "\u200E" + $"{icorr:0.00E+00} A", colorIcorr);
            AddRow("لگاریتم جریان (log Icorr):", "\u200E" + $"{logIcorr:0.0000}", colorIcorr);
            AddRow("شیب آندی (Ba):", "\u200E" + $"{ba:0.0000} mVolt/dec", colorSlope);
            AddRow("شیب کاتدی (Bc):", "\u200E" + $"{bc:0.0000} mVolt/dec", colorSlope);

            resultForm.Controls.Add(table);
            resultForm.Controls.Add(headerPanel);
            resultForm.Controls.Add(footerPanel);

            resultForm.ShowDialog(this);
        }

        private void btnCalculateTafel_Click(object sender, EventArgs e)
        {
            // ۱. بررسی برای پاک کردن خطوط در صورت کلیک مجدد (Toggle)
            if (tChart2.Annotations.Count > 0 || tChart2.Series.FindByName("IntersectionPoint") != null)
            {
                tChart2.Annotations.Clear();
                Series intersectionSeries = tChart2.Series.FindByName("IntersectionPoint");
                if (intersectionSeries != null)
                {
                    tChart2.Series.Remove(intersectionSeries); // حذف کامل سری
                }
                tChart2.Refresh();
                UpdateRotateButtonState(); // به‌روزرسانی وضعیت دکمه چرخش
                return; // خروج از متد برای پاک کردن
            }

            // ۲. بررسی وجود خطوط سفید
            if (drawline1.Lines.Count != 2)
            {
                MessageBox.Show("the number of line must be equal to 2 ." + Environment.NewLine + "please try again.");
                return;
            }

            // ۳. تبدیل مختصات پیکسلی خطوط به مختصات داده‌ای (ریاضی)
            drawline1.removeall_master();
            conv_pixel_cord(drawline1.Lines[0].point1.X, drawline1.Lines[0].point1.Y);
            conv_pixel_cord(drawline1.Lines[0].point2.X, drawline1.Lines[0].point2.Y);
            conv_pixel_cord(drawline1.Lines[1].point1.X, drawline1.Lines[1].point1.Y);
            conv_pixel_cord(drawline1.Lines[1].point2.X, drawline1.Lines[1].point2.Y);

            if (!drawline1.intersect_line())
            {
                MessageBox.Show("there is not intersection point ." + Environment.NewLine + "please try again.");
                return;
            }

            try
            {
                // ۴. استخراج مختصات خطوط
                double x1 = drawline1.Lines_master[0].pointx;
                double y1 = drawline1.Lines_master[0].pointy;
                double x2 = drawline1.Lines_master[1].pointx;
                double y2 = drawline1.Lines_master[1].pointy;

                double x3 = drawline1.Lines_master[2].pointx;
                double y3 = drawline1.Lines_master[2].pointy;
                double x4 = drawline1.Lines_master[3].pointx;
                double y4 = drawline1.Lines_master[3].pointy;

                // ۵. محاسبه شیب‌ها
                double slop1 = (y2 - y1) / (x2 - x1);
                double slop2 = (y4 - y3) / (x4 - x3);

                if (slop1 == slop2)
                {
                    MessageBox.Show("خطوط موازی هستند و نقطه تقاطع ندارند.", "خطا");
                    return;
                }

                // ۶. محاسبه عرض از مبدأ
                double b1 = y1 - (slop1 * x1);
                double b2 = y3 - (slop2 * x3);

                // ۷. محاسبه نقطه تقاطع هندسی (مختصات ایکس و ایگرگ)
                double intX = (b2 - b1) / (slop1 - slop2);
                double intY = (slop1 * intX) + b1;

                double Ecorr = 0, logIcorr = 0, Icorr = 0, Ba = 0, Bc = 0;

                // ★★★ ۸. تشخیص هوشمند محورها بر اساس عنوان آن‌ها ★★★
                string xTitle = tChart2.ChartAreas[0].AxisX.Title.ToLower();
                bool isXCurrent = xTitle.Contains("current") || xTitle.Contains("جریان") || xTitle.Contains("i");

                if (isXCurrent)
                {
                    // حالت چرخیده: X = جریان (لگاریتمی) ، Y = ولتاژ
                    logIcorr = intX;
                    Ecorr = intY;

                    // طبق فرمول نارنجی: Icorr = 10^(log Icorr)
                    Icorr = Math.Pow(10, logIcorr);
                    Icorr = Icorr / 1000.0; // تبدیل میلی‌آمپر به آمپر

                    // محاسبه شیب تافل (چون Y ولتاژ است، slop همان mV/dec است، نیازی به ضرب در 1000 نیست)
                    Ba = Math.Abs(slop1);
                    Bc = Math.Abs(slop2);
                }
                else
                {
                    // حالت عادی: X = ولتاژ ، Y = جریان (لگاریتمی)
                    Ecorr = intX;
                    logIcorr = intY;

                    // طبق فرمول نارنجی: Icorr = 10^(log Icorr)
                    Icorr = Math.Pow(10, logIcorr);
                    Icorr = Icorr / 1000.0; // تبدیل میلی‌آمپر به آمپر

                    // محاسبه شیب تافل (چون Y لگاریتمی است، 1/slop همان mV/dec است، نیازی به ضرب در 1000 نیست)
                    Ba = Math.Abs(1 / slop1);
                    Bc = Math.Abs(1 / slop2);
                }

                // اگر ولتاژ در حدود میلی‌ولت بود، آن را به ولت تبدیل می‌کنیم تا فرم زیبا باشد
                if (Math.Abs(Ecorr) > 10)
                    Ecorr = Ecorr / 1000.0;

                // ==========================================
                // ۹. رسم نقطه قرمز (نقطه تقاطع)
                // ==========================================
                Series intersectionSeries = tChart2.Series.FindByName("IntersectionPoint");
                if (intersectionSeries == null)
                {
                    intersectionSeries = new Series("IntersectionPoint");
                    intersectionSeries.ChartType = SeriesChartType.Point;
                    intersectionSeries.MarkerStyle = MarkerStyle.Circle;
                    intersectionSeries.MarkerColor = Color.Red;
                    intersectionSeries.MarkerBorderColor = Color.Black;
                    intersectionSeries.MarkerBorderWidth = 1;
                    intersectionSeries.IsVisibleInLegend = false;
                    tChart2.Series.Add(intersectionSeries);
                }
                else
                {
                    intersectionSeries.Points.Clear();
                }

                intersectionSeries.MarkerSize = 6;
                intersectionSeries.Points.AddXY(intX, intY);

                // ==========================================
                // ۱۰. رسم خطوط نقطه چین عمودی و افقی
                // ==========================================
                tChart2.Annotations.Clear();

                double minY = tChart2.ChartAreas[0].AxisY.Minimum;
                double minX = tChart2.ChartAreas[0].AxisX.Minimum;
                double maxY = tChart2.ChartAreas[0].AxisY.Maximum;
                double maxX = tChart2.ChartAreas[0].AxisX.Maximum;

                if (double.IsNaN(minY)) minY = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                if (double.IsNaN(minX)) minX = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                if (double.IsNaN(maxY)) maxY = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMaximum;
                if (double.IsNaN(maxX)) maxX = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum;

                // خط عمودی
                LineAnnotation verticalLine = new LineAnnotation();
                verticalLine.AxisX = tChart2.ChartAreas[0].AxisX;
                verticalLine.AxisY = tChart2.ChartAreas[0].AxisY;
                verticalLine.IsSizeAlwaysRelative = false;
                verticalLine.LineColor = Color.Red;
                verticalLine.LineDashStyle = ChartDashStyle.Dash;
                verticalLine.LineWidth = 1;
                verticalLine.X = intX;
                verticalLine.Y = intY;
                verticalLine.Width = 0;
                verticalLine.Height = minY - intY;
                tChart2.Annotations.Add(verticalLine);

                // خط افقی
                LineAnnotation horizontalLine = new LineAnnotation();
                horizontalLine.AxisX = tChart2.ChartAreas[0].AxisX;
                horizontalLine.AxisY = tChart2.ChartAreas[0].AxisY;
                horizontalLine.IsSizeAlwaysRelative = false;
                horizontalLine.LineColor = Color.Red;
                horizontalLine.LineDashStyle = ChartDashStyle.Dash;
                horizontalLine.LineWidth = 1;
                horizontalLine.X = intX;
                horizontalLine.Y = intY;
                horizontalLine.Width = minX - intX;
                horizontalLine.Height = 0;
                tChart2.Annotations.Add(horizontalLine);

                // ==========================================
                // ۱۱. نمایش مقادیر دقیق روی محورها
                // ==========================================

                TextAnnotation textX = new TextAnnotation();
                textX.Text = intX.ToString("0.0000");
                textX.ForeColor = Color.Red;
                textX.Font = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
                textX.AxisX = tChart2.ChartAreas[0].AxisX;
                textX.AxisY = tChart2.ChartAreas[0].AxisY;
                textX.X = intX;
                textX.Y = minY - (maxY - minY) * 0.04;
                textX.Alignment = ContentAlignment.TopCenter;
                tChart2.Annotations.Add(textX);

                TextAnnotation textY = new TextAnnotation();
                textY.Text = intY.ToString("0.0000");
                textY.ForeColor = Color.Red;
                textY.Font = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
                textY.AxisX = tChart2.ChartAreas[0].AxisX;
                textY.AxisY = tChart2.ChartAreas[0].AxisY;
                textY.X = minX - (maxX - minX) * 0.04;
                textY.Y = intY;
                textY.Alignment = ContentAlignment.MiddleRight;
                tChart2.Annotations.Add(textY);

                // به‌روزرسانی تایتل
                tChart2.Titles[0].Text = "Tafel Results :     ";
                tChart2.Titles[0].Text += "Ba=" + Ba.ToString("#0.0000") + " mV/dec        Bc=" + Bc.ToString("#0.0000") + " mV/dec";

                tChart2.Refresh();
                UpdateRotateButtonState();

                // ۱۲. نمایش فرم نتایج
                ShowCustomResultsForm(Ecorr, logIcorr, Icorr, Ba, Bc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در محاسبات: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRotateAxes_Click(object sender, EventArgs e)
        {
            // تغییر وضعیت چرخش
            isAxesRotated = !isAxesRotated;

            // ۱. پاک کردن کامل خطوط قرمز، متن‌ها و نقطه تقاطع
            tChart2.Annotations.Clear();
            Series intersectionSeries = tChart2.Series.FindByName("IntersectionPoint");
            if (intersectionSeries != null)
            {
                tChart2.Series.Remove(intersectionSeries); // حذف کامل سری از چارت
            }

            // ۲. پاک کردن خطوط سفیدی که کاربر رسم کرده است
            drawline1.removeall();
            drawline1.removeall_master();
            numline = -1;

            // ۳. رفرش چارت و اعمال چرخش
            tChart2.Refresh();
            SwapAxes(tChart2);
        }
        private void UpdateMyNewButtonState()
        {
            bool hasData = false;

            // بررسی وجود tChart2 و داشتن حداقل یک سری داده با نقطه
            if (tChart2 != null && tChart2.Series != null)
            {
                foreach (var series in tChart2.Series)
                {
                    if (series.Points.Count > 0)
                    {
                        hasData = true;
                        break;
                    }
                }
            }

            // اگر داده وجود داشت دکمه فعال می‌شود، در غیر این صورت غیرفعال است
            btnExelExport.Enabled = hasData;
        }
        private void btnExelExport_Click(object sender, EventArgs e)
        {
            // ۱. بررسی وجود داده در tChart2
            if (tChart2.Series.Count == 0 || tChart2.Series[0].Points.Count == 0)
            {
                MessageBox.Show("داده‌ای در نمودار دوم (tChart2) برای ذخیره وجود ندارد.", "هشدار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ۲. باز کردن پنجره ذخیره فایل
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = "csv";
                sfd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";

                // پیشنهاد نام فایل بر اساس عنوان نمودار
                string title = tChart2.Titles.Count > 0 ? tChart2.Titles[0].Text.Trim() : "Chart2_Data";
                if (title == "") title = "Chart2_Data";
                sfd.FileName = title.Replace(" ", "_").Replace(":", "");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // ۳. نوشتن داده‌ها در فایل CSV
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            // نوشتن هدر (نام ستون‌ها)
                            sw.WriteLine($"SeriesName,{tChart2.ChartAreas[0].AxisX.Title},{tChart2.ChartAreas[0].AxisY.Title}");

                            // پیمایش تمام سری‌های موجود در tChart2 (شامل خط اصلی، نقاط قله، نقطه تقاطع و ...)
                            foreach (var series in tChart2.Series)
                            {
                                // اگر سری نقطه تقاطع بود یا مخفی بود، میتوانید اینجا شرط بگذارید تا ذخیره نشود
                                // اما برای کامل بودن همه را ذخیره می‌کنیم
                                string seriesName = string.IsNullOrEmpty(series.Name) ? "Data" : series.Name;

                                foreach (var point in series.Points)
                                {
                                    // استفاده از CultureInfo.InvariantCulture برای جلوگیری از مشکل کاما در اکسل
                                    string xValue = point.XValue.ToString(CultureInfo.InvariantCulture);
                                    string yValue = point.YValues.Length > 0 ? point.YValues[0].ToString(CultureInfo.InvariantCulture) : "0";

                                    sw.WriteLine($"{seriesName},{xValue},{yValue}");
                                }
                            }
                        }
                        MessageBox.Show("فایل CSV با موفقیت ذخیره شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("خطا در ذخیره فایل:\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void showDataPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvTechList.SelectedNode.Index >= 0 && tvTechList.SelectedNode.Level == 1)
            {
                forms2065.frmshowdata frm = new forms2065.frmshowdata();
                frm.listView1.Items.Clear();
                for (int i = 0; i < tChart1.Series[tvTechList.SelectedNode.Index].Points.Count; i++)
                {
                    frm.listView1.Items.Add(tChart1.Series[tvTechList.SelectedNode.Index].Points[i].XValue + "          " + tChart1.Series[tvTechList.SelectedNode.Index].Points[i].YValues[0]);
                }
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            forms2065.frmselecttypeshow frm = new forms2065.frmselecttypeshow();
            frm.StartPosition = FormStartPosition.CenterScreen;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.type_txt_y.Trim() != "")
                    tChart2.ChartAreas[0].AxisY.LabelStyle.Format = frm.type_txt_y;
                if (frm.type_txt_x.Trim() != "")
                    tChart2.ChartAreas[0].AxisX.LabelStyle.Format = frm.type_txt_x;
                tChart2.ChartAreas[0].RecalculateAxesScale();
            }
        }

        private void btnshowval_1_Click(object sender, EventArgs e)
        {
            if (statebutton_1 == "showvalue")
                statebutton_1 = "";
            else
                statebutton_1 = "showvalue";
            selectpchart_func_1(false);
        }

        private void btnselectonly_1_Click(object sender, EventArgs e)
        {
            if (statebutton_1 == "selectonly")
            {
                statebutton_1 = "";
                selectpchart_func_1(false);
                btnselectonly_1.FlatAppearance.BorderSize = 0;
            }
            else
            {
                statebutton_1 = "selectonly";
                selectpchart_func_1(true);
                btnselectonly_1.FlatAppearance.BorderSize = 1;
                btnselectonly_1.FlatAppearance.BorderColor = Color.Black;

            }
        }

        private void btnzoomin_1_Click(object sender, EventArgs e)
        {
            statebutton_1 = "";
            selectpchart_func_1(false);
            this.tChart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            double xMin = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
            double xMax = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
            double yMin = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
            double yMax = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMaximum;
            tChart2.ChartAreas[0].AxisX.ScaleView.Zoom(xMin, 0.95 * (xMax - xMin), DateTimeIntervalType.Auto, true);
            tChart2.ChartAreas[0].AxisY.ScaleView.Zoom(yMin, 0.95 * (yMax - yMin), DateTimeIntervalType.Auto, true);
            difxview2 = tChart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum - tChart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
            difyview2 = tChart2.ChartAreas[0].AxisY.ScaleView.ViewMaximum - tChart2.ChartAreas[0].AxisY.ScaleView.ViewMinimum;

        }

        private void btnzoomout_1_Click(object sender, EventArgs e)
        {
            if (statebutton_1 != "")
            {
                btnzoomout_1.FlatAppearance.BorderColor = Color.White;
                pictureBox1.Image = pictureBox1.Image.ApplyGrayscaleFilter();
            }
            else
            {
                btnzoomout_1.FlatAppearance.BorderColor = Color.DarkCyan;
            }
            statebutton_1 = "";
            selectpchart_func_1(false);
            this.tChart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            tChart2.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
            tChart2.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
        }

        private void btnshowgrid_1_Click(object sender, EventArgs e)
        {
            statebutton_1 = "";
            selectpchart_func_1(false);
            this.tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled = !this.tChart2.ChartAreas[0].AxisX.MajorGrid.Enabled;
            this.tChart2.ChartAreas[0].AxisY.MajorGrid.Enabled = !this.tChart2.ChartAreas[0].AxisY.MajorGrid.Enabled;

        }

        private void btnshowaxes_1_Click(object sender, EventArgs e)
        {
            statebutton_1 = "";
            selectpchart_func_1(false);
            tChart2.ChartAreas[0].AxisX.LabelStyle.Enabled = !tChart2.ChartAreas[0].AxisX.LabelStyle.Enabled;
            tChart2.ChartAreas[0].AxisX.MajorTickMark.Enabled = !tChart2.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            tChart2.ChartAreas[0].AxisY.LabelStyle.Enabled = !tChart2.ChartAreas[0].AxisY.LabelStyle.Enabled;
            tChart2.ChartAreas[0].AxisY.MajorTickMark.Enabled = !tChart2.ChartAreas[0].AxisY.MajorTickMark.Enabled;

        }

        private void btnexport_1_Click(object sender, EventArgs e)
        {
            statebutton_1 = "";
            selectpchart_func(false);
            forms2065.frmselectfiletype frm = new forms2065.frmselectfiletype();
            frm.StartPosition = FormStartPosition.CenterScreen;
            int i1 = 0, shomarcheck = 0, filetype = 0;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                filetype = frm.filetype_select;
            }

            switch (filetype)
            {
                case 1:
                    toolStripButton8_Click_1(sender, e);
                    break;
                case 2:
                    toolStripButton7_Click_1(sender, e);
                    break;
                case 3:
                    if (tChart2.Series.Count > 0 && tChart2.Series[0].Points.Count > 0)
                        shomarcheck = 1;
                    i1 = 0;
                    if (shomarcheck == 1)
                    {
                        string strlegend = fillparam(2, nindexchart2);
                        forms2065.frmshowgraph frm1 = new forms2065.frmshowgraph(strlegend.Trim(), tChart2.Series[i1].Points.FindMinByValue("X").XValue, tChart2.Series[i1].Points.FindMaxByValue("X").XValue, tChart2.Series[i1].Points.FindMinByValue("Y").YValues[0], tChart2.Series[i1].Points.FindMaxByValue("Y").YValues[0], "", tChart2.Titles[0].Text.Trim());
                        if (frm1.Chart1.Series.Count > 0)
                            frm1.Chart1.Series[0].Points.Clear();
                        for (int i = 0; i < tChart2.Series[i1].Points.Count; i++)
                        {
                            frm1.Chart1.Series[0].Points.AddXY(tChart2.Series[i1].Points[i].XValue, tChart2.Series[i1].Points[i].YValues[0]);
                        }
                        frm1.StartPosition = FormStartPosition.CenterScreen;
                        frm1.ShowDialog();
                    }
                    else
                        MessageBox.Show("EXPORTING works with one graph" + (char)13 + "Please select ONE only!!!");


                    break;
                default:
                    break;

            }
        }

        List<ITechniques> TechLine = new List<ITechniques>();

        public void AddTechnique(ITechniques Tech)
        {
            try
            {
                LoadCsvToChart(Tech.SeriesPath, tChart1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AddTechnique_Error");
            }


        }

        public void LoadCsvToChart(string fileName, Chart chart)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);

                if (lines.Length < 2)
                    return;

                //-----------------------------------
                // نام تکنیک از اولین سطر داده
                //-----------------------------------
                string[] firstRow = lines[1].Split(',');

                string techName = firstRow[0].Trim();
                ITechniques Tech = null;
                DeviceSpecification device = frmMain12080.Device;

                try
                {
                    switch (techName)
                    {
                        case "NPV":
                            double E1 = Convert.ToDouble(firstRow[2]);
                            double E2 = Convert.ToDouble(firstRow[3]);
                            double ScanRate = Convert.ToDouble(firstRow[4]);
                            double Hstep = Convert.ToDouble(firstRow[5]);
                            double PulseWidth = Convert.ToDouble(firstRow[6]);
                            int DelayTime = Convert.ToInt32(firstRow[7]);
                            string Comment = firstRow[8].Trim();

                            Tech = new NPV(device.Techniques["NPV"], E1, E2, ScanRate, Hstep, PulseWidth, DelayTime, Comment)
                            {
                                ColorCode = Convert.ToInt32(firstRow[1])
                            };
                            break;

                        case "DPV":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            ScanRate = Convert.ToDouble(firstRow[4]);
                            Hstep = Convert.ToDouble(firstRow[5]);
                            PulseWidth = Convert.ToDouble(firstRow[6]);
                            double PulseHeight = Convert.ToDouble(firstRow[7]);
                            DelayTime = Convert.ToInt32(firstRow[8]);
                            Comment = firstRow[9].Trim();

                            Tech = new DPV(device.Techniques["DPV"], E1, E2, ScanRate, Hstep, PulseWidth, PulseHeight, DelayTime, Comment)
                            {
                                ColorCode = Convert.ToInt32(firstRow[1])
                            };
                            break;

                        case "SWV":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            ScanRate = Convert.ToDouble(firstRow[4]);
                            Hstep = Convert.ToDouble(firstRow[5]);
                            PulseHeight = Convert.ToDouble(firstRow[6]);
                            DelayTime = Convert.ToInt32(firstRow[7]);
                            Comment = firstRow[8].Trim();

                            Tech = new SWV(device.Techniques["SWV"], E1, E2, ScanRate, Hstep, PulseHeight, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                        case "CV":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            ScanRate = Convert.ToDouble(firstRow[4]);
                            Hstep = Convert.ToDouble(firstRow[5]);
                            int Cycle = Convert.ToInt32(firstRow[6]);
                            DelayTime = Convert.ToInt32(firstRow[7]);
                            Comment = firstRow[8].Trim();

                            Tech = new CV(device.Techniques["CV"], E1, E2, ScanRate, Hstep, Cycle, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                        case "LSV":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            ScanRate = Convert.ToDouble(firstRow[4]);
                            Hstep = Convert.ToDouble(firstRow[5]);
                            DelayTime = Convert.ToInt32(firstRow[6]);
                            Comment = firstRow[7].Trim();

                            Tech = new LSV(device.Techniques["LSV"], E1, E2, ScanRate, Hstep, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                        case "OCP":
                            int Time = Convert.ToInt16(firstRow[2]);
                            DelayTime = Convert.ToInt16(firstRow[3]);
                            Comment = firstRow[3].Trim();

                            Tech = new OCP(device.Techniques["OCP"], Time, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                        case "CA":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            int T1 = Convert.ToInt16(firstRow[4]);
                            int T2 = Convert.ToInt16(firstRow[5]);
                            Cycle = Convert.ToInt16(firstRow[6]);
                            DelayTime = Convert.ToInt32(firstRow[7]);
                            Comment = firstRow[8].Trim();

                            Tech = new CA(device.Techniques["CA"], E1, E2, T1, T2, Cycle, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                        case "Tafel":
                            E1 = Convert.ToDouble(firstRow[2]);
                            E2 = Convert.ToDouble(firstRow[3]);
                            ScanRate = Convert.ToDouble(firstRow[4]);
                            Hstep = Convert.ToDouble(firstRow[5]);
                            DelayTime = Convert.ToInt32(firstRow[6]);
                            Comment = firstRow[7].Trim();

                            Tech = new Tafel(device.Techniques["Tafel"], E1, E2, ScanRate, Hstep, DelayTime, Comment);
                            Tech.ColorCode = Convert.ToInt32(firstRow[1]);
                            break;

                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show($"فرمت خواندن فایل مناسب نیست \n {Ex.Message}");
                    return;
                }

                Tech.SeriesPath = fileName;
                TechLine.Add(Tech);

                //-----------------------------------
                // نود اصلی
                //-----------------------------------
                TreeNode Node = new TreeNode()
                {
                    Text = Tech.Name,
                    ForeColor = Color.FromArgb(Tech.ColorCode)
                };

                if (Tech.Number == 4)
                {
                    CV cv = Tech as CV;
                    for (int i = 0; i < cv.Cycles; i++)
                    {
                        TreeNode ChildNode = new TreeNode()
                        {
                            Text = Tech.Name + "_" + (i + 1).ToString(),
                            ForeColor = Color.FromArgb(Tech.ColorCode),
                            Checked = true
                        };

                        Node.Nodes.Add(ChildNode);
                    }
                }

                tvTechList.Nodes[0].Nodes.Add(Node);

                tvTechList.ExpandAll();

                //جلوگیری از اتفاق event
                tvTechList.AfterCheck -= treeView3_AfterCheck;
                tvTechList.Nodes[0].LastNode.Checked = true;
                tvTechList.AfterCheck += treeView3_AfterCheck;

                Series currentSeries = null;
                int currentCycle = -1;

                currentSeries = new Series($"{Tech.Name}-{Node.Index}")
                {
                    ChartType = Tech.SeriesType,
                    BorderWidth = 2,
                    Color = Color.FromArgb(Tech.ColorCode)
                };

                chart.Series.Add(currentSeries);
                tChart1.ChartAreas[0].AxisX.Title = Tech.XY_Axis_Title[1];
                tChart1.ChartAreas[0].AxisY.Title = Tech.XY_Axis_Title[0];

                for (int i = 3; i < lines.Length; i++)
                {
                    string[] cols = lines[i].Split(',');

                    double x = double.Parse(cols[1], CultureInfo.InvariantCulture);
                    double y = double.Parse(cols[2], CultureInfo.InvariantCulture);
                    int cycle = int.Parse(cols[3]);

                    // اگر Cycle عوض شد یک سری جدید بساز
                    if (cycle != currentCycle)
                    {
                        currentCycle = cycle;

                        if (cycle >= 1)
                        {
                            currentSeries = new Series($"{Tech.Name}-{Node.Index}_{cycle}")
                            {
                                ChartType = Tech.SeriesType,
                                BorderWidth = 2,
                                Color = Color.FromArgb(Tech.ColorCode)
                            };

                            chart.Series.Add(currentSeries);
                        }

                    }

                    currentSeries.Points.AddXY(x, y);


                }

                tvTechList.ExpandAll();
            }
            catch (Exception)
            {

            }
        }
    }
}