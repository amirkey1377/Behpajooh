using Skydat.classes2065;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PVOID = System.IntPtr;

namespace Skydat.forms2065
{

    public partial class frmshowGraph2065 : Form
    {
        //Declare your global variable here
        private int shomar_cha_chc = 0, countcv = 0, shomar_othertech = 0, shomarchpa = 0, num_Series = 0, num_cycles = -1, cl = 0, counterforstop = 1;
        private bool flag_running = false, detect_Error = false, S_et = false, fst1, fst2, fst = false, Xinv = false, pulsy = false, ngtv, N_gtv, is_set_graph = false, set_ListBoxOK = false, isSelectedLine = false, runerror = false;
        private double cycles = 1, NUM, Analiz_St1 = 0, Analiz_St2 = 0, Analiz_CT1 = 0, Analiz_CT2 = 0, Analiz_CT3 = 0, Analiz_CP1 = 0, Analiz_CP2 = 0, Analiz_CP3 = 0, Analiz_T3 = 0, Elo, ti = 0;
        double[] time_cha_chc = new double[200];
        double[] volt_cha_chc = new double[200];
        double[] othertech_val0 = new double[200];
        double[] othertech_val1 = new double[200];
        double[] othertech_val2 = new double[200];
        double[] othertech_val3 = new double[200];
        private byte Range1, outc = 0x20, outc_b = 0x20;
        byte[] datas2_0 = new byte[100];
        byte[] datas2_1 = new byte[100];
        byte[] datas2_2 = new byte[100];
        byte[] datas2_state = new byte[100];
        private string staterun = "";
        private Thread thread_grf;
        private delegate void showOnlineData(double v, double i1, double i2, int row);
        private delegate void showDataon(int row);

        private clasmpusbapi usb_fun = new clasmpusbapi();
        private const byte P_A = 0xC0;
        private const byte P_B = 0xC4;
        private const byte P_C = 0xC8;
        private const byte P_P = 0xCC;

        private drawline_chart drawline1;
        public int[] Overlay_tech = new int[1000];
        public frmMain12065 mainf;
        public frmOption12065 opForm;

        private string statebutton = "";

        //Adding library
        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadCursorFromFile(String str);
        public int flag_cd = 0;

        //Setting for set customize icon for zoomin & zoomoutButton
        public static System.Windows.Forms.Cursor LoadCursorFromResource(Icon icono)  // Assuming that the resource is an Icon, but also could be a Image or a Bitmap
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
        public frmshowGraph2065()
        {
            InitializeComponent();
            //this.MouseWheel += new MouseEventHandler(frmshowGraph2065_MouseWheel);
            try
            {
                serialPort2.PortName = Properties.Settings.Default.portname;
                serialPort2.BaudRate = int.Parse(Properties.Settings.Default.baudrate);

                //Set parameter like color & ... for graph
                System.Drawing.Color ChartColor = Color.Black;
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisX.LineColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisY.LineColor = ChartColor;

                OnlineChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisY.Title = "coulomb(c)";
                OnlineChart.ChartAreas[0].AxisX.Title = "Time(s)";
                OnlineChart.ChartAreas[0].AxisX.TitleForeColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisY.TitleForeColor = ChartColor;

                OnlineChart.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
                this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.LineColor = ChartColor;
                this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.LineColor = ChartColor;
                this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.LineColor = ChartColor;
                this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.LineColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.BackColor = ChartColor;
                OnlineChart.ChartAreas[0].AxisY.ScrollBar.BackColor = ChartColor;
                OnlineChart.Series[0].Color = Color.DarkCyan;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);

                this.treeView3.ExpandAll();
                thread_grf = new Thread(Run_key);
                thread_grf.Name = "RunnigTech";
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا پورت سریال خود را به درستی انتخاب کنید");
                return;
            }

        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            try
            {
                IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
                PVOID myOutPipe = INVALID_HANDLE_VALUE;
                PVOID myInPipe = INVALID_HANDLE_VALUE;
                usb_fun.ClosePipes();
                usb_fun.OpenPipes();
                usb_fun.Outprt(P_P, 0x07);
                usb_fun.Outprt(P_A, 0x00);
                usb_fun.Outprt(P_B, 0x00);
                usb_fun.Outprt(P_C, 0x00);
                System.Threading.Thread.Sleep(1);
                drawline1 = new drawline_chart();
                if (opForm.cbDrawLine.Checked)
                    Set_line_Setting();
                else
                    drawline1.Active = false;

                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[0].ToString().Substring(color[0].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                OnlineChart.BackColor = cl;
                OnlineChart.BorderlineColor = cl;
                OnlineChart.ChartAreas[0].BackColor = cl;
                panel1.BackColor = cl;
                panel2.BackColor = cl;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

        }
        private void Set_line_Setting()
        {
            drawline1.EnableDraw = opForm.cbDrawLine.Checked;
            drawline1.pen_ch.Color = ((frmMain12065)(this.MdiParent)).drawColor;
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


        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = (mainf.grfNum == 0);
                if (mainf.grfNum > 0)
                    mainf.grfNum--;

                staterun = "stop";
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (OnlineChart.Series.Count > 0)
                {
                    var results1 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    double x111 = 0, y111 = 0;
                    foreach (var result in results1)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            x111 = result.ChartArea.AxisX.PixelPositionToValue(e.X);
                            y111 = result.ChartArea.AxisY.PixelPositionToValue(e.Y);

                        }
                    }
                    mainf.xyLabel.Text = Math.Round(x111, 5).ToString() + "," + y111.ToString("00e-0");

                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }



        private void Chart1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                    drawline1.EnableDraw = true;
                if (e.KeyData == Keys.Delete)
                    drawline1.removeall();
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }

        private void tChart1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                drawline1.EnableDraw = false;
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }

        private void GraphForm_Enter(object sender, EventArgs e)
        {
            try
            {
                if (mainf != null)
                    mainf.grf1 = (sender as frmshowGraph2065);
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }



        private void Chart1_AfterDraw(object sender)
        {
            try
            {
                mainf.Enable_Button(OnlineChart.Series.Count);

                if (OnlineChart.Series.Count == 0)
                    OnlineChart.ChartAreas[0].AxisX.Title = "BHP20xx";

            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }

        private void treeView3_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Level > 0)
                {
                    for (int i = 0; i < treeView3.Nodes[0].Nodes.Count; i++)
                    {
                        if (treeView3.Nodes[0].Nodes[i].Checked)
                        {
                            OnlineChart.Series[i].Enabled = true;
                            OnlineChart.Titles[0].Text = classes2065.clasglobal.TechName[Overlay_tech[i]];
                            // OnlineChart.Series[num_Series].t = OnlineChart.Titles[0].Text;
                            OnlineChart.ChartAreas[0].AxisY.LabelStyle.Angle = 0;
                            OnlineChart.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle[Overlay_tech[i]];
                            OnlineChart.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle[Overlay_tech[i]];
                        }
                        else
                        {
                            OnlineChart.Series[i].Enabled = false;

                        }
                    }

                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }

        delegate void SetfillparamCallback(int row, int state);

        private void fillTechParameter(int row, int state)
        {

            if (state == 1)
            {
                SetfillparamCallback d = new SetfillparamCallback(fillTechParameter);
                this.Invoke(d, new object[] { row, 2 });
            }
            else
            {
                this.treeView3.SelectedNode = this.treeView3.Nodes[0].Nodes[row];

                Parameters.Text = "";
                switch (byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()))
                {
                    case clasglobal.DCV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";

                        break;
                    case clasglobal.NPV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Width =" + double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                        break;
                    case clasglobal.DPV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Width =" + double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Height =" + double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                        break;
                    case clasglobal.SWV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Height =" + double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) != 9999) Parameters.Text = Parameters.Text + "Frequency =" + double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) + "\n";
                        break;
                    case clasglobal.CV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) != 9999) Parameters.Text = Parameters.Text + "E3 =" + double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                        break;
                    case clasglobal.LSV:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                        break;
                    case clasglobal.DCs:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";

                        break;
                    case clasglobal.DPs:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Height =" + double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) != 9999) Parameters.Text = Parameters.Text + "Pulse Width =" + double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) != 9999) Parameters.Text = Parameters.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) != 9999) Parameters.Text = Parameters.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";

                        break;
                    case clasglobal.CPC:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) //Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CCC:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) //Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;

                    case clasglobal.CHP:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) != 9999) Parameters.Text = Parameters.Text + "I1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) != 9999) Parameters.Text = Parameters.Text + "I2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";

                        if (double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) != 9999) Parameters.Text = Parameters.Text + "EUP =" + double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) != 9999) Parameters.Text = Parameters.Text + "EDO =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + " Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CHA:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CHC:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CD:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) != 9999) Parameters.Text = Parameters.Text + "I1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) != 9999) Parameters.Text = Parameters.Text + "I2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) != 9999) Parameters.Text = Parameters.Text + "EUP =" + double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) != 9999) Parameters.Text = Parameters.Text + "EDO =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + " Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";

                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                        break;
                }
                Parameters.Text = Parameters.Text + "comment =" + dschart1.chartlist1_run.Rows[row]["comment"].ToString();
            }
            //pGrid.Refresh();
        }

        public void Set_Graph(bool overlaymain, int row1)
        {


            Setgrftchart1(overlaymain, 0, 1, 1);//پاک کردن نمودار و افزودن تایتل خالی به نمودار

            Series ser1 = new Series();
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
            Setgrftchart11(ser1, 1, 1);



            Setgrftchart1(overlaymain, row1, 2, 1);// افزودن تایتل محور عمودی و افقی و کلی به نمودار
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            try
            {
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Enabled = !OnlineChart.ChartAreas[0].AxisX.LabelStyle.Enabled;
                // OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled;
                OnlineChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisX.MajorTickMark.Enabled;
                OnlineChart.ChartAreas[0].AxisY.LabelStyle.Enabled = !OnlineChart.ChartAreas[0].AxisY.LabelStyle.Enabled;
                // OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled;
                OnlineChart.ChartAreas[0].AxisY.MajorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisY.MajorTickMark.Enabled;

                this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }


        unsafe private void doRun2_1(int row)
        {
            byte num1, num2;
            byte* OutBuff = &num1;
            byte* InBuff = &num2;
            byte oinp1 = 0, oinp2 = 0, oinp3 = 0;
            byte[] data = new byte[65520];
            byte[] Powr = new Byte[32710];//توان
            double[] Crnt = new double[32710];//جریان
            double[] C = new double[32710];//بار خازنی
            byte[] Po = new Byte[32710];
            double[] Crn = new double[32710];
            Boolean[] signe = new Boolean[32710];

            int number = 0, sign = 0, end = 0, counter = 0;
            string string1;
            string[] strings = new string[4];
            int i = 0;
            flag_running = true;
            shomarchpa = 0;
            System.Threading.Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["qt"].ToString()));//  گرفتن زمان تعادل و بخواب بردن دستگاه
            serialPort2.DiscardInBuffer();
            var watch = System.Diagnostics.Stopwatch.StartNew();// ایجاد کردن تایمر

            //OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
            {
                
                while (outc != 0xA0 && end == 0)//((shomarchpa * ti < Analiz_t3 ) && (outc != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "stop") // اگر وضعیت stop شد 
                    {
                        runerror = true;
                        watch.Stop();
                        staterun = "";
                        break;
                    }
                    if (shomarchpa == 0)//شماره چاپ     -> به نظر دارد یک تاخیر ایجاد میکند
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    else
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    counter++;
                    //Settxtfill_func(2, $"counter:{counter}", 2, 1);
                    // serialPort2.WriteLine("step");
                    number = serialPort2.BytesToRead;
                    //Setbtnenabled(true, 1, 2);

                    if (number >= 0)
                    {
                        try
                        {
                            //Settxtfill_func(2, $"counter1", 2, 1);
                            string1 = serialPort2.ReadLine();


                            //string1.Remove(string1.Length-1);
                            if (sign == 0)
                            {

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                                {
                                    double pos = 0;
                                    double size = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 2000;
                                    OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                                    OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                                    OnlineChart.ChartAreas[0].AxisY.Title = dschart1.chartlist1_run.Rows[row][15].ToString();
                                    //OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                                    //OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.ChartAreas[0].AxisX.Maximum;
                                    OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                                    OnlineChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;

                                    //double size = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + 100 ;
                                    OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                    //pos =+ 5;
                                    if (OnlineChart.ChartAreas[0].AxisX.Maximum >= 5)
                                    {
                                        OnlineChart.ChartAreas[0].AxisX.ScaleView.Position = OnlineChart.ChartAreas[0].AxisX.Maximum - 5;
                                    }
                                    OnlineChart.ChartAreas[0].CursorX.AutoScroll = true;
                                    if (string1.Contains("end10"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {
                                        //Settxtfill_func(2, $"counter2", 2, 1);
                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = 6;
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                                {
                                    if (string1.Contains("end11"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        counter = (int)Int32.Parse(strings[4]);
                                        //  Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
                                {
                                    if (string1.Contains("end12"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                                {
                                    if (string1.Contains("end15"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }
                                if (outc == 0x60) //اگر pause بود 
                                {
                                    outc_b = 0x60;
                                    watch.Start();
                                    continue;
                                }
                                if (outc == 0x20 && outc_b == 0x60)//
                                {
                                    outc_b = 0x20;
                                    watch.Start();
                                    continue;
                                }

                                if ((byte)(oinp3 & 0x0f) <= 7)
                                {
                                    if (shomarchpa >= datas2_state.Length)
                                    {
                                        Array.Resize(ref datas2_2, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_0, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_1, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_state, datas2_state.Length + 100);
                                        for (int l = shomarchpa; l < datas2_state.Length; l++)
                                            datas2_state[l] = 2;

                                    }
                                    sign = 0;
                                    datas2_2[shomarchpa] = (byte)(oinp3 & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                                    datas2_0[shomarchpa] = (byte)oinp1;//read porta
                                    datas2_1[shomarchpa] = (byte)oinp2;//read portb
                                    datas2_state[shomarchpa] = 0;
                                    showdata(row);
                                    watch.Start();
                                    shomarchpa++;
                                }

                            }

                        }
                        catch
                        {

                        }
                    }

                    if (outc == 0xA0 || end == 1)//((shomarchpa * ti>= Analiz_T3) || outc ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        int poi = 1;
                        end = 0;
                        serialPort2.Write("stop");
                        System.Threading.Thread.Sleep(100);
                        serialPort2.Close();

                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80
                        watch.Start();
                        while (watch.ElapsedMilliseconds < 100) ;//4  delay for 100ms 
                        watch.Stop();
                        watch.Reset();

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running-" + poi.ToString().Trim(), 2, 1);
                        System.Threading.Thread.Sleep(100);
                        usb_fun.Outprt(P_C, 0x00); //transmit 0x00 to portc for accept stop
                        usb_fun.ClosePipes();
                        detect_Error = false;
                        return;
                    }
                }
            }
            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11) {
                while (shomarchpa * ti < Analiz_T3*50 && outc != 0xA0)//((shomarchpa * ti < Analiz_t3 ) && (outc != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "stop") // اگر وضعیت stop شد 
                    {
                        runerror = true;
                        watch.Stop();
                        staterun = "";
                        break;
                    }
                    if (shomarchpa == 0)//شماره چاپ     -> به نظر دارد یک تاخیر ایجاد میکند
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    else
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    counter++;
                    //Settxtfill_func(2, $"counter:{counter}", 2, 1);
                    // serialPort2.WriteLine("step");
                    number = serialPort2.BytesToRead;
                    Setbtnenabled(true, 1, 2);
                    if (number >= 0)
                    {
                        try
                        {
                            //Settxtfill_func(2, $"counter1", 2, 1);
                            string1 = serialPort2.ReadLine();


                            //string1.Remove(string1.Length-1);
                            if (sign == 0)
                            {

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                                {
                                    if (string1.Contains("end10"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {
                                        //Settxtfill_func(2, $"counter2", 2, 1);
                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = 6;
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                                {
                                    if (string1.Contains("end11"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        counter = (int)Int32.Parse(strings[4]);
                                        //  Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }
                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
                                {
                                    if (string1.Contains("end12"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }



                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                                {
                                    if (string1.Contains("end15"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }
                                if (outc == 0x60) //اگر pause بود 
                                {
                                    outc_b = 0x60;
                                    watch.Start();
                                    continue;
                                }
                                if (outc == 0x20 && outc_b == 0x60)//
                                {
                                    outc_b = 0x20;
                                    watch.Start();
                                    continue;
                                }

                                if ((byte)(oinp3 & 0x0f) <= 7)
                                {
                                    if (shomarchpa >= datas2_state.Length)
                                    {
                                        Array.Resize(ref datas2_2, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_0, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_1, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_state, datas2_state.Length + 100);
                                        for (int l = shomarchpa; l < datas2_state.Length; l++)
                                            datas2_state[l] = 2;

                                    }
                                    sign = 0;
                                    datas2_2[shomarchpa] = (byte)(oinp3 & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                                    datas2_0[shomarchpa] = (byte)oinp1;//read porta
                                    datas2_1[shomarchpa] = (byte)oinp2;//read portb
                                    datas2_state[shomarchpa] = 0;
                                    showdata(row);
                                    watch.Start();
                                    shomarchpa++;
                                }

                            }

                        }
                        catch
                        {

                        }
                    }
                    if (shomarchpa * ti >= (Analiz_T3*50) || outc == 0xA0 || end == 1)//((shomarchpa * ti>= Analiz_T3) || outc ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        byte inpt = 0;
                        int poi = 1;
                        end = 0;
                        serialPort2.Write("stop");
                        System.Threading.Thread.Sleep(100);
                        serialPort2.Close();

                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80
                        watch.Start();
                        while (watch.ElapsedMilliseconds < 100) ;//4  delay for 100ms 
                        watch.Stop();
                        watch.Reset();

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running-" + poi.ToString().Trim(), 2, 1);
                        System.Threading.Thread.Sleep(100);


                        usb_fun.Outprt(P_C, 0x00); //transmit 0x00 to portc for accept stop
                        usb_fun.ClosePipes();
                        detect_Error = false;
                        return;
                    }
                }
            }
            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
            {
                while (shomarchpa * ti < Analiz_T3 && outc != 0xA0)//((shomarchpa * ti < Analiz_t3 ) && (outc != 0xa0))   ->  if state run 
                {



                    if (staterun.Trim() == "stop") // اگر وضعیت stop شد 
                    {
                        runerror = true;
                        watch.Stop();
                        staterun = "";
                        break;
                    }
                    if (shomarchpa == 0)//شماره چاپ     -> به نظر دارد یک تاخیر ایجاد میکند
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    else
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    counter++;
                    //Settxtfill_func(2, $"counter:{counter}", 2, 1);
                    // serialPort2.WriteLine("step");
                    number = serialPort2.BytesToRead;
                    Setbtnenabled(true, 1, 2);
                    if (number >= 0)
                    {
                        try
                        {
                            //Settxtfill_func(2, $"counter1", 2, 1);
                            string1 = serialPort2.ReadLine();


                            //string1.Remove(string1.Length-1);
                            if (sign == 0)
                            {

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                                {
                                    if (string1.Contains("end10"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {
                                        //Settxtfill_func(2, $"counter2", 2, 1);
                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = 6;
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                                {
                                    if (string1.Contains("end11"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        counter = (int)Int32.Parse(strings[4]);
                                        //  Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        // Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }
                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
                                {
                                    if (string1.Contains("end12"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }



                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                                {
                                    if (string1.Contains("end15"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {

                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = (byte)Int32.Parse(strings[1]);
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;

                                    }
                                }
                                if (outc == 0x60) //اگر pause بود 
                                {
                                    outc_b = 0x60;
                                    watch.Start();
                                    continue;
                                }
                                if (outc == 0x20 && outc_b == 0x60)//
                                {
                                    outc_b = 0x20;
                                    watch.Start();
                                    continue;
                                }

                                if ((byte)(oinp3 & 0x0f) <= 7)
                                {
                                    if (shomarchpa >= datas2_state.Length)
                                    {
                                        Array.Resize(ref datas2_2, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_0, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_1, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_state, datas2_state.Length + 100);
                                        for (int l = shomarchpa; l < datas2_state.Length; l++)
                                            datas2_state[l] = 2;

                                    }
                                    sign = 0;
                                    datas2_2[shomarchpa] = (byte)(oinp3 & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                                    datas2_0[shomarchpa] = (byte)oinp1;//read porta
                                    datas2_1[shomarchpa] = (byte)oinp2;//read portb
                                    datas2_state[shomarchpa] = 0;
                                    showdata(row);
                                    watch.Start();
                                    shomarchpa++;
                                }

                            }

                        }
                        catch
                        {

                        }



                    }




                    if (shomarchpa * ti >= (Analiz_T3) || outc == 0xA0 || end == 1)//((shomarchpa * ti>= Analiz_T3) || outc ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        byte inpt = 0;
                        int poi = 1;
                        end = 0;
                        serialPort2.Write("stop");
                        System.Threading.Thread.Sleep(100);
                        serialPort2.Close();

                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80
                        watch.Start();
                        while (watch.ElapsedMilliseconds < 100) ;//4  delay for 100ms 
                        watch.Stop();
                        watch.Reset();

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running-" + poi.ToString().Trim(), 2, 1);
                        System.Threading.Thread.Sleep(100);


                        usb_fun.Outprt(P_C, 0x00); //transmit 0x00 to portc for accept stop
                        usb_fun.ClosePipes();
                        detect_Error = false;
                        return;
                    }


                }
            }
            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 14)
            {
                
                while (outc != 0xA0 && end == 0)//((shomarchpa * ti < Analiz_t3 ) && (outc != 0xa0))   ->  if state run 
                {
                    

                    if (staterun.Trim() == "stop") // اگر وضعیت stop شد 
                    {
                        runerror = true;
                        watch.Stop();
                        staterun = "";
                        break;
                    }
                    if (shomarchpa == 0)//شماره چاپ     -> به نظر دارد یک تاخیر ایجاد میکند
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    else
                    {
                        while (watch.ElapsedMilliseconds < (ti * 10)) ;//4  
                        watch.Stop();
                        watch.Reset();
                    }
                    counter++;
                    //Settxtfill_func(2, $"counter:{counter}", 2, 1);
                    // serialPort2.WriteLine("step");
                    number = serialPort2.BytesToRead;
                    Setbtnenabled(true, 1, 2);

                    if (number >= 0)
                    {
                        double pos = 0;
                        double size = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 2000;
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                        OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                        //OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                        //OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.ChartAreas[0].AxisX.Maximum;
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;

                        //double size = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + 100 ;
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                        //pos =+ 5;
                        if (OnlineChart.ChartAreas[0].AxisX.Maximum >= 5)
                        {
                            OnlineChart.ChartAreas[0].AxisX.ScaleView.Position = OnlineChart.ChartAreas[0].AxisX.Maximum - 5;
                        }

                        OnlineChart.ChartAreas[0].CursorX.AutoScroll = true;

                        try
                        {
                            //Settxtfill_func(2, $"counter1", 2, 1);
                            string1 = serialPort2.ReadLine();


                            //string1.Remove(string1.Length-1);
                            if (sign == 0)
                            {

                                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 14)
                                {
                                    if (string1.Contains("end14"))
                                    {
                                        end = 1;
                                        number = 0;
                                        Settxtfill_func(2, "end", 2, 1);
                                    }
                                    else
                                    {
                                        //Settxtfill_func(2, $"counter2", 2, 1);
                                        strings = string1.Split('@');
                                        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                        oinp3 = 6;
                                        oinp1 = (byte)Int32.Parse(strings[2]);
                                        oinp2 = (byte)Int32.Parse(strings[3]);
                                        //counter = (int)Int32.Parse(strings[4]);
                                        //Settxtfill_func(2, $"c:{oinp3},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"a:{oinp2},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"b:{oinp1},i{i}", 2, 1);
                                        //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                        counter++;
                                        sign = 1;
                                        number = 0;
                                        flag_running = true;
                                    }
                                }

                                if (outc == 0x60) //اگر pause بود 
                                {
                                    outc_b = 0x60;
                                    watch.Start();
                                    continue;
                                }
                                if (outc == 0x20 && outc_b == 0x60)//
                                {
                                    outc_b = 0x20;
                                    watch.Start();
                                    continue;
                                }

                                if ((byte)(oinp3 & 0x0f) <= 7)
                                {
                                    if (shomarchpa >= datas2_state.Length)
                                    {
                                        Array.Resize(ref datas2_2, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_0, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_1, datas2_state.Length + 100);
                                        Array.Resize(ref datas2_state, datas2_state.Length + 100);
                                        for (int l = shomarchpa; l < datas2_state.Length; l++)
                                            datas2_state[l] = 2;
                                    }
                                    sign = 0;
                                    datas2_2[shomarchpa] = (byte)(oinp3 & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                                    datas2_0[shomarchpa] = (byte)oinp1;//read porta
                                    datas2_1[shomarchpa] = (byte)oinp2;//read portb
                                    datas2_state[shomarchpa] = 0;
                                    showdata(row);
                                    watch.Start();
                                    shomarchpa++;
                                }
                            }
                        }
                        catch
                        {

                        }
                    }




                    if (outc == 0xA0 || end == 1)//((shomarchpa * ti>= Analiz_T3) || outc ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        byte inpt = 0;
                        int poi = 1;
                        end = 0;
                        serialPort2.Write("stop");
                        System.Threading.Thread.Sleep(100);
                        serialPort2.Close();

                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80
                        watch.Start();
                        while (watch.ElapsedMilliseconds < 100) ;//4  delay for 100ms 
                        watch.Stop();
                        watch.Reset();

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running-" + poi.ToString().Trim(), 2, 1);
                        System.Threading.Thread.Sleep(100);
                        usb_fun.Outprt(P_C, 0x00); //transmit 0x00 to portc for accept stop
                        usb_fun.ClosePipes();
                        detect_Error = false;
                        return;
                    }
                }
            }
        }

        unsafe private void doRun2(int row)
        {
            byte[] datas = new byte[3];
            Int32 n = 0;
            Int64 j = 0, h = 0;
            byte num1, num2;
            bool evn = true, E_nd = false, back_direction = false;
            byte* OutBuff = &num1;
            byte* InBuff = &num2;
            string string1;
            byte end = 0;
            int counter = 0, number = 0;
            //byte[] data = new byte[65520];//قبلا این مقادیر بود اما اگر تعداد سیکل ها بالا رود رویداد این پارامترها رفرش نمیشود بنابراین هر سیکل در ادامه هم ذخیره میشد و نهایتا اگر از تعداد آرایه بالا میزد خطا میداد
            //byte[] Powr = new Byte[32710];
            //double[] Crnt = new double[32710];
            //double[] C = new double[32710];
            //byte[] Po = new Byte[32710];
            //double[] Crn = new double[32710];
            //Boolean[] signe = new Boolean[32710];
            byte[] Powr = new Byte[4000000];
            double[] Crnt = new double[4000000];
            double[] C = new double[4000000];
            byte[] Po = new Byte[4000000];
            double[] Crn = new double[4000000];
            Boolean[] signe = new Boolean[4000000];
            int sign = 0;
            int i = 0;
            double Itot = 0;
            double V = 0, I1 = 0, I2 = 0, Vold;
            

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8)   // if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps
                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());// e1 
            else if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) >= 9)//cpc ,chp,cha,chc
                V = ti;
            Vold = V;
            flag_running = true;
            serialPort2.DiscardInBuffer();
            serialPort2.ReadTimeout = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + 5000 + (clasglobal.cpctprms.CT1 * 1000) + (clasglobal.cpctprms.CT2 * 1000) + (clasglobal.cpctprms.CT3 * 1000));
            for (n = 1; n < 4000000; n++)//تا زمانی که دستور پایان را از دستگاه نگیرد میچرخد
            {
                if (staterun.Trim() == "stop")
                {
                    runerror = true;
                    staterun = "";
                    break;
                }
                string1 = null;
                string[] strings = new string[4];
                Setbtnenabled(true, 1, 1);
                // serialPort2.WriteLine("start");
                number = serialPort2.BytesToRead;
                //Settxtfill_func(2, "wait", 2, 1);
                if (number >= 0)
                {
                    try
                    {

                        string1 = serialPort2.ReadLine();
                        // Settxtfill_func(2, "start", 2, 1);
                        //string1.Remove(string1.Length-1);
                        if (sign == 0)
                        {
                            if (string1.Contains("end"))
                            {
                                end = 1;
                                string1 = "";
                                Settxtfill_func(2, "end", 2, 1);
                            }
                            else
                            {
                                strings = string1.Split('@');
                                //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                datas[2] = (byte)Int32.Parse(strings[1]);
                                datas[0] = (byte)Int32.Parse(strings[2]);
                                datas[1] = (byte)Int32.Parse(strings[3]);
                                //counter = (int)Int32.Parse(strings[4]);
                                Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                                //Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                                //Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                                //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                counter++;
                                sign = 1;
                                number = 0;
                                flag_running = true;
                            }

                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 0)
                            //{

                            //    if (string1.Contains("end0"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end0", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 1)
                            //{
                            //    if (string1.Contains("end1"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end1", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 2)
                            //{

                            //    if (string1.Contains("end2"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end2", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 3)
                            //{
                            //    if (string1.Contains("end3"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end3", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)
                            //{

                            //    if (string1.Contains("end4"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end4", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 5)
                            //{

                            //    if (string1.Contains("end5"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end5", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 6)
                            //{
                            //    if (string1.Contains("end6"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end6", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 7)
                            //{
                            //    if (string1.Contains("end7"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end7", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9)
                            //{
                            //    if (string1.Contains("end9"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end9", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }
                            //}
                            //if (end == 0)
                            //{
                            //    serialPort2.DiscardOutBuffer();
                            //    serialPort2.Write("next");
                            //    serialPort2.DiscardInBuffer();
                            //}

                        }
                    }
                    catch (TimeoutException)
                    {
                        if (outc != 0x60)
                        {
                            serialPort2.Write("stop");
                            serialPort2.Close();
                            Settxtfill_func(2, "Time out eror happen", 2, 1);
                            return;
                        }
                    }
                }
                if (sign == 1)
                {
                    if (fst) //IF fst TRUE 
                    {
                        sign = 0;
                        Powr[j] = datas[2];//SAVE IN BUFFER 
                        Crnt[j] = ((datas[0] << 6) | (datas[1] >> 2)); //???
                                                                       // Settxtfill_func(2, $"power={Powr[j]},{oinp}", 2, 1);
                        if (Crnt[j] > 0x1FFF)//0x1fff = 8191 
                        {
                            Crnt[j] = (0x4000 - Crnt[j]);//16384 - current ->negative current
                            signe[j] = true;
                        }
                        else
                            signe[j] = false;
                        if (Range1 != 0)
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8) && (Crnt[j] > 6560))//if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps && current >6560
                                Crnt[j] = 6560;//current 6560
                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9) && (Powr[j] == 8)) //if tecno cpc or ccc
                            Crnt[j] = Crnt[j] * 2; //current upto 1A
                    }
                    else//if fst false ??
                    {// for tecno under 8
                        Po[h] = datas[2];//save power in arry po
                        Crn[h] = ((datas[0] << 6) | (datas[1] >> 2));
                        if (Crn[h] > 0x1FFF)
                        {
                            Crn[h] = (0x4000 - Crn[h]);
                            Crn[h] = Crn[h] * -1;
                        }
                        sign = 0;
                        Settxtfill_func(2, $"test1{h}", 2, 1);
                        h++;
                        if (h == 5)
                        {

                            i++;
                            Settxtfill_func(2, $"test2{i}", 2, 1);
                            Itot = (Crn[0] * Math.Pow(10, Po[0] - Po[4]) + Crn[1] * Math.Pow(10, Po[1] - Po[4]) + Crn[2] * Math.Pow(10, Po[2] - Po[4]) + Crn[3] * Math.Pow(10, Po[3] - Po[4]) + Crn[4]) / 5;
                            Powr[j] = Po[4];
                            Crnt[j] = Math.Round(Itot, 2);//////trunc(Itot);
                            if (Crnt[j] > 6560)
                            {
                                Crnt[j] = Crnt[j] / 10;
                                Powr[j]++;
                            }
                            if (Crnt[j] < 0)
                            {
                                Crnt[j] = Crnt[j] * -1;
                                signe[j] = true;
                            }
                            else
                                signe[j] = false;
                            S_et = true;
                        }
                    }

                    if (S_et) //if s_et ==true ??   ->for tecno cpc
                    {

                        if ((Powr[j] == 8) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 9))
                        {//techno <8 and >9

                            C[j] = Crnt[j] * 1.02207 * Math.Pow(10, Powr[j]);/////// - 6
                        }
                        else
                        {
                            C[j] = Crnt[j] * 0.61035 * Math.Pow(10, Powr[j] - 6); ////- 6  -> for cpc
                                                                                  //Settxtfill_func(2, $"flag1={C[j]},range{Powr[j]} ", 2, 1);
                        }
                        if (signe[j] ^ (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10))
                            C[j] = C[j] * -1;

                        if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8)//if techno == dcv,npv,dpv,swv,cv,lsv,dcs,dps,
                        {
                            //////mn////////////if (Xinv)
                            //////mn////////////    C[j] = C[j] * -1;//در گراف سی وی اگر ولتاژ از بزرگ به کوچک برود جریان را منفی نمی کند

                            if (pulsy) //if pulsy ==true
                            {
                                

                                if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 2)|| (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 1)|| (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 7))
                                {

                                    if (!evn)//
                                    {
                                        if (ngtv) //if ngtv == true
                                        {
                                            //I1 = C[j];
                                            I1 = C[j];
                                            I2 = C[j - 1];

                                            Settxtfill_func(2, $"negative", 2, 1);
                                            //  I1 = C[j] - C[j - 1];
                                            // I2 = 0;
                                        }
                                        else  ////if ngtv == false
                                        {

                                            I1 = C[j];
                                            I2 = C[j - 1];
                                            Settxtfill_func(2, $"positive", 2, 1);
                                            //I1 = C[j - 1] - C[j];
                                            // I2 = 0;
                                        }

                                        // Addxy(V1,I0,'',Colr[cl])
                                    }

                                }
                                else
                                {

                                    if (!evn)//
                                    {
                                        if (ngtv) //if ngtv == true
                                        {
                                            //I1 = C[j];
                                            I1 = C[j];
                                            I2 = C[j - 1];

                                            Settxtfill_func(2, $"negative2", 2, 1);
                                            //  I1 = C[j] - C[j - 1];
                                            // I2 = 0;
                                        }
                                        else  ////if ngtv == false
                                        {

                                            I1 = C[j];
                                            I2 = C[j - 1];
                                            Settxtfill_func(2, $"positive2", 2, 1);
                                            //I1 = C[j - 1] - C[j];
                                            // I2 = 0;
                                        }

                                        // Addxy(V1,I0,'',Colr[cl])
                                    }
                                }

                            }
                            else//if pulsy == false
                            {
                                I1 = C[j];
                                I2 = 0;
                            }
                            int numdigit = dschart1.chartlist1_run.Rows[row][6].ToString().Length - dschart1.chartlist1_run.Rows[row][6].ToString().IndexOf('.') - 1;// calculate digit number in of heght step
                            if (!(pulsy && evn))
                                if (N_gtv)
                                {
                                    Vold = V;
                                    V = Math.Round(V - double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()), numdigit);
                                }
                                else
                                {
                                    Vold = V;
                                    V = Math.Round(V + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()), numdigit);
                                }

                            if (!fst)
                            {
                                h = 0;
                                S_et = false;
                            }

                            j++;
                            evn = !evn;


                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)// if techno ==   cv
                            {
                                if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString())) //if (e1<e2)
                                {
                                    if (V >= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString())) // if v>e2
                                    {
                                        back_direction = true;
                                        N_gtv = !N_gtv;
                                        fst = fst2;
                                        if (fst)
                                            S_et = true;
                                        else
                                            S_et = false;
                                    }
                                    if (V < Elo && back_direction)
                                    {
                                        dschart1.chartlist1_run.Rows[row][13] = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) - 1;// cycle = cycle-1
                                        if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) >= 0) //if cycle >= 0
                                        {
                                            E_nd = true;
                                            countcv++;
                                            Settxtfill_func(2, $"endcycle", 2, 1);
                                            if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)
                                                number_runing_cv++;              //mn
                                        }
                                    }
                                }
                                else//if e1>e2
                                {
                                    if (V <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))//if v<=e2
                                    {
                                        back_direction = true;
                                        N_gtv = !N_gtv;
                                        fst = fst2;
                                        if (fst)
                                            S_et = true;
                                        else
                                            S_et = false;
                                    }

                                    if (V > Elo && back_direction)
                                    {
                                        dschart1.chartlist1_run.Rows[row][13] = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) - 1;
                                        if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) >= 0)
                                        {
                                            E_nd = true;
                                            countcv++;

                                            if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)
                                                number_runing_cv++;        //mn2
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {// techno ccc,cpc,chp,cha,chc

                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)//if techno ==chc
                                I1 = I1 + C[j] * ti;
                            else//techno ...
                                I1 = C[j]; // Addxy(V1,C[j],'',Colr[cl]);
                            I2 = 0;
                            Vold = V;
                            V = V + ti;
                            sign = 0;
                            j++;
                        }


                        if (pulsy)
                        {
                            if (evn)
                            {
                                Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                                                           //Settxtfill_func(2, $"1={Vold}:{I1}:{I2} ", 2, 1);
                                Settxtfill_func(2, $"vold1{Vold}", 2, 1);
                                Settxtfill_func(2, $"I={I1},{I2}", 2, 1);

                            }
                        }
                        else
                        {
                           
                            Settxtfill_func(2, $"vold2{Vold}", 2, 1);
                            Settxtfill_func(2, $"i={I1}", 2, 1);
                            Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                                                       //Settxtfill_func(2, $"2={Vold}:{I1}:{I2} ", 2, 1);
                        }

                        if (E_nd)
                        {
                            Settxtfill_func(2, $"endcv2", 2, 1);
                            transferdata(row, 1);
                            double temp1, temp2, temp3, temp4;
                            temp1 = othertech_val0[shomar_othertech - 1];
                            temp2 = othertech_val1[shomar_othertech - 1];
                            temp3 = othertech_val2[shomar_othertech - 1];
                            temp4 = othertech_val3[shomar_othertech - 1];
                            smootht4(row, OnlineChart.Series[0].Color);
                            Setgrfnodetext(row, 2, 1);
                            show_cycle_runing_CV(number_runing_cv);//show number running in toolStripStatusLabel5
                            if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)//cycle >0
                            {
                                shomar_othertech = 1;
                                Array.Clear(othertech_val0, 1, othertech_val0.Length - 1);
                                Array.Clear(othertech_val1, 1, othertech_val1.Length - 1);
                                Array.Clear(othertech_val2, 1, othertech_val2.Length - 1);
                                Array.Clear(othertech_val3, 1, othertech_val3.Length - 1);
                                othertech_val0[0] = temp1;
                                othertech_val1[0] = temp2;
                                othertech_val2[0] = temp3;
                                othertech_val3[0] = temp4;
                                Setgrfnodetext(row, 4, 1);
                                show_cycle_runing_CV(number_runing_cv);//show number running in toolStripStatusLabel5
                            }
                            else if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) == 0) // cycle =0
                                countcv = 0;

                            fst = fst1;
                            if (fst)
                                S_et = true;
                            else
                                S_et = false;

                            N_gtv = !N_gtv;
                            if (N_gtv)
                            {
                                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) - double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());//v =e1-hs
                            }
                            else
                            {
                                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());//v=e1+hs
                            }

                            E_nd = false;
                            back_direction = false;
                        }

                    } //if S_et
                      ///////////////////////////////////////          
                }
                else if (end == 1)  //if portc=0x09
                {
                    end = 0;
                    serialPort2.Close();
                    System.Threading.Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["qt"].ToString()));//delay with equlibirum time                                                                                                           // System.Threading.Thread.Sleep(int.Parse(this.dschart1.chartlist.Rows[row][7].ToString()));
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, $"finish running-1 :{end} :{string1}", 2, 1);
                    detect_Error = false;
                    return;
                }
                // other input portc 
                //if (outc == 0x60)
                //    thread_grf.Suspend();
                if (outc == 0xA0)//if software ==pause 
                {
                    //Settxtfill_func(1, $"goodbay{oinp}", 2, 1);
                    serialPort2.Write("stop");
                    System.Threading.Thread.Sleep(100);
                    serialPort2.Close();
                    detect_Error = true;
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, "finish running-2", 2, 1);
                    return;
                }


            }
        }

        unsafe private void dorun2_costum_2(int row)
        {


            byte[] datas = new byte[3];
            Int32 n = 0;
            byte num1, num2;
            bool evn = true, back_direction = false;
            byte* OutBuff = &num1;
            byte* InBuff = &num2;
            string string1;
            byte end = 0;
            int counter = 0, number = 0;
            double volt = 0;

           
            //byte[] data = new byte[65520];//قبلا این مقادیر بود اما اگر تعداد سیکل ها بالا رود رویداد این پارامترها رفرش نمیشود بنابراین هر سیکل در ادامه هم ذخیره میشد و نهایتا اگر از تعداد آرایه بالا میزد خطا میداد
            //byte[] Powr = new Byte[32710];
            //double[] Crnt = new double[32710];
            //double[] C = new double[32710];
            //byte[] Po = new Byte[32710];
            //double[] Crn = new double[32710];
            //Boolean[] signe = new Boolean[32710];
            byte[] Powr = new Byte[4000000];
            double[] Crnt = new double[4000000];
            double[] C = new double[4000000];
            byte[] Po = new Byte[4000000];
            double[] Crn = new double[4000000];
            Boolean[] signe = new Boolean[4000000];
            int sign = 0;
            int i = 0;
            double V = 0, I1 = 0, I2 = 0, Vold;

          

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8)   // if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps
                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());// e1 
            else if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) >= 9)//cpc ,chp,cha,chc
                V = ti;
            Vold = V;
            flag_running = true;
            serialPort2.ReadTimeout = 3000;
            float volt1 = 0;
            serialPort2.DiscardInBuffer();
            for (n = 1; n < 4000000; n++)//تا زمانی که دستور پایان را از دستگاه نگیرد میچرخد
            {
                if (staterun.Trim() == "stop")
                {
                    runerror = true;
                    staterun = "";
                    break;
                }
                string1 = null;
                string[] strings = new string[4];
                char char1 = '0';
                Setbtnenabled(true, 1, 2);
                // serialPort2.WriteLine("start");
                number = serialPort2.BytesToRead;
                if (number >= 0)
                {

                    try
                    {

                        string1 = serialPort2.ReadLine();
                        //string1.Remove(string1.Length-1);
                        if (sign == 0)
                        {
                            

                            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                            {
                                if (string1.Contains("end15"))
                                {
                                    end = 1;
                                    string1 = "";
                                    Settxtfill_func(2, "end15", 2, 1);
                                }
                                else
                                {
                                    strings = string1.Split('@');
                                    //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                    volt = double.Parse(strings[1]);
                                    datas[0] = (byte)Int32.Parse(strings[2]);
                                    datas[1] = (byte)Int32.Parse(strings[3]);
                                    counter = (int)Int32.Parse(strings[4]);
                                    volt1 = (int)Int32.Parse(strings[5]);
                                    Settxtfill_func(2, $"voltage:{volt1}", 2, 1);
                                    //Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                                    //Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                                    //Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                    counter++;
                                    sign = 1;
                                    number = 0;
                                    flag_running = true;
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                if (sign == 1)
                {
                    Vold = counter;
                    I1 = volt1;
                    //Settxtfill_func(2, $"vold2{Vold}", 2, 1);
                    Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                    //Settxtfill_func(2, $"2={Vold}:{I1}:{I2} ", 2, 1);
                    sign = 0;
                }
                else if (end == 1)  //if portc=0x09
                {
                    end = 0;
                    serialPort2.Close();
                    // System.Threading.Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["qt"].ToString()));//delay with equlibirum 
                    // System.Threading.Thread.Sleep(int.Parse(this.dschart1.chartlist.Rows[row][7].ToString()));
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, $"finish running-1 :{end} :{string1}", 2, 1);
                    detect_Error = false;
                    return;
                }

                if (outc == 0xA0)//if software ==pause 
                {

                    serialPort2.Write("stop");
                    System.Threading.Thread.Sleep(100);
                    serialPort2.Close();
                    detect_Error = true;
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, "finish running-2", 2, 1);
                    return;

                }


            }




        }

        public void Runnig2(double V, double I1, double I2, int row)
        {
            if (this.InvokeRequired)
            {
                showOnlineData d = new showOnlineData(Runnig2);
                this.Invoke(d, new object[] { V, I1, I2, row });
            }
            else
            {
                if (!is_set_graph)
                {
                    Set_Graph(false, row);
                    //checkedListBox1.Items.Add("Plot"+(this.num_Series+1).ToString());
                    //try
                    //{
                    //    treeView3.Nodes[0].Nodes[this.num_Series].Checked = true;
                    //}
                    //catch { }
                    is_set_graph = true;
                }
                /////////////////////////////////////
                // do_smooth_Run_Time(false, Already_Drawing_Graph);
                // this.OnlineChart.Series[row].Points.AddXY(V, I1 - I2);



                this.OnlineChart.Series[0].Points.AddXY(V, I1 - I2);///// رسم گراف اصلی


                if (shomar_othertech >= othertech_val0.Length)
                {
                    Array.Resize(ref othertech_val0, othertech_val0.Length + 200);
                    Array.Resize(ref othertech_val1, othertech_val1.Length + 200);
                    Array.Resize(ref othertech_val2, othertech_val2.Length + 200);
                    Array.Resize(ref othertech_val3, othertech_val3.Length + 200);

                }

                othertech_val0[shomar_othertech] = V;
                othertech_val1[shomar_othertech] = I1;
                othertech_val2[shomar_othertech] = I2;
                othertech_val3[shomar_othertech] = I1 - I2;

                shomar_othertech++;
                mainf.toolStripProgressBar1.Value++;
                if (mainf.toolStripProgressBar1.Value == 100)
                    mainf.toolStripProgressBar1.Value = 0;
                if (set_ListBoxOK)
                {

                    int num_Items = treeView3.Nodes[0].Nodes.Count;
                    string[] items_Name = new string[num_Items];
                    for (int i = 0; i < treeView3.Nodes[0].Nodes.Count; i++)
                        items_Name[i] = treeView3.Nodes[0].Nodes[i].ToString();
                    treeView3.Nodes[0].Nodes.Clear();
                    if (OnlineChart.Series.Count > 0)
                    {
                        int n = this.OnlineChart.Series.Count;
                        for (int num_s = 0; num_s < n; num_s++)
                        {
                            if (num_s < num_Items)
                                treeView3.Nodes[0].Nodes.Add(items_Name[num_s].ToString());
                            else
                                treeView3.Nodes[0].Nodes.Add("Plot" + (num_s + 1).ToString());
                            treeView3.Nodes[0].Nodes[num_s].Checked = true;
                        }
                        treeView3.SelectedNode = treeView3.Nodes[0].LastNode;
                    }
                    set_ListBoxOK = false;
                }
                if (!flag_running)
                {
                    mainf.toolStripProgressBar1.Visible = false;
                    //do_smooth_Run_Time(false, Already_Drawing_Graph);
                    mainf.xyLabel.Visible = true;
                    flag_running = false;
                    //if (thread_grf != null)
                    //    if (thread_grf.ThreadState == ThreadState.Running)
                    //        thread_grf.Abort();
                    OnlineChart.Update();
                }
            }
        }

        int number_runing_cv = 0, show_number_runing = 0;///mehrdad   
        public int cycle_number_in_file = 1;//mn
        unsafe void Run_key()
        {
            try
            {
                Setgrfnodetext(0, 3, 1);//add tech3 from chartlist run to tree3  اضافه کردن نام تکنیک در حال اجرا به درخت

                for (int TechSeries = 0; TechSeries < dschart1.chartlist1_run.Rows.Count && outc != 0xA0; TechSeries++)//چرخ حلقه به تعداد تکنیک های انتخاب شده
                {
                    //Declare Your Varible Here
                    cycle_number_in_file = 1;//save in file
                    number_runing_cv = 1;
                    show_number_runing = TechSeries;

                    Setgrfnodetext(TechSeries, 1, 1);//show number running in toolStripStatusLabel5  نمایش شماره تکنیک به ترتیب درخت

                    fillTechParameter(treeView3.Nodes[0].Nodes[TechSeries].Index, 1);//show status in Parameters  نمایش وضعیت در حال اجرا
                    //fillparamdata(treeView3.Nodes[0].LastNode.Index.Index);   

                    movedata.main_cycle = dschart1.chartlist1_run.Rows[TechSeries][13].ToString();

                    Set_value2(TechSeries); //ست کردن و پردازش اولیه اطلاعات خام وارد شده توسط کاربر و ست درون دیتاست اجرا و ارسال به دستگاه
                    if (detect_Error)
                    {
                        this.mainf.stf.runToolStripMenuItem1.Enabled = true;
                        this.mainf.stf.tsbRun.Enabled = true;
                        return;
                    }
                    Set_Graph(false, TechSeries);//پاک کردن نمودار و افزودن تایتل محورها و کل و فرمت نمایش به نمودار

                    switch (int.Parse(dschart1.chartlist1_run.Rows[TechSeries][1].ToString()))// techno 
                    {
                        case 0:
                            run_key_tch0(TechSeries);// و نمایش همزمان اطلاعات روی نمودار  DCV اجرا تکنیک 
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);// ذخیره دیتای اجرا شده به صورت خودکار در پوشه تمپ نرم افزار
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);// پس از اتمام اجرای تکنیک و ذخیره دیتا نیاز است تا گراف نرم نمایش داده شود. این الگوریتم میانگیری برای نرم کردن از زمان دلفی پیاده سازی شده و ممکن است در آینده نسبت به الگوریتم های جدید بهینه و با کیفیت نباشد
                            }
                            break;

                        case 1:
                            run_key_tch1(TechSeries);//NPV
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 2:
                            run_key_tch2(TechSeries);//DPV
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 3:
                            run_key_tch3(TechSeries);//SWF
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 4:
                            run_key_tch4(TechSeries);//CV   به علت داشتن سیکل ذخیره فایل و نرم کردن درون توابع زیرمجموعه انجام می شود
                            dschart1.chartlist1_run.Rows[TechSeries][13] = movedata.main_cycle;//به علت این که سیکل دارد و هنگام اجرا خانه این فیلد مورد استفاده قرار میگیرد بنابراین با اجرای این تابع دیتای سیکل را از بین نمیبریم
                            break;

                        case 5:
                            run_key_tch5(TechSeries);//LSV
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 6:
                            run_key_tch6(TechSeries);//DCS
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 7:
                            run_key_tch7(TechSeries);//DPS
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht0_t1(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 8:
                            run_key_tch8(TechSeries);//CCC
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 9:
                            run_key_tch9(TechSeries);//CPC
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht9(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 10:
                            run_key_tch10(TechSeries);//CHP
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                                // smootht10(TechSeries, OnlineChart.Series[0].Color,ti,25);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 11:
                            run_key_tch11(TechSeries);//CHA
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                                // smootht10(TechSeries, OnlineChart.Series[0].Color,ti,100);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 12:
                            run_key_tch12(TechSeries);//CHC
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                                //  smootht12(TechSeries, OnlineChart.Series[0].Color);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 14:

                            run_key_tch14(TechSeries);//Cd
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                                // smootht10(TechSeries, OnlineChart.Series[0].Color,ti,25);
                            }
                            else
                            {
                                break;
                            }
                            break;

                        case 15:
                            run_key_tch15(TechSeries);//OCP
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                            }
                            else
                            {
                                break;
                            }
                            break;
                    }

                    if (!runerror)
                    {
                        if (int.Parse(dschart1.chartlist1_run.Rows[TechSeries][1].ToString()) != clasglobal.CV || outc == 0xA0)//if techno != cv or stop flag ==1
                        {
                            Setgrfnodetext(TechSeries, 2, 1);// ست کردن دیتا درون دیتاست و نمودار فرم بعدی که آنالیز می باشد
                        }
                    }
                    
                    //System.Threading.Thread.Sleep(3000);
                }
                if (!runerror)
                {
                    Setbtnenabled(true, 2, 1);//نمایش دکمه و متن پایان اجرا
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void transferdata(int rownum, int typecall)
        {
            string strpath = "";// fName = "";

            for (long i = 100001; i < 1000000; i++)
            {
                if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat"))
                {
                    if (cycle_number_in_file <= 1)// without cycle
                    {
                        strpath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        //fName = clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        break;
                    }
                    else
                    {
                        strpath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + (i - 1).ToString().Trim() + ".dat";
                        //fName = clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        break;
                    }
                }
            }

            if (cycle_number_in_file <= 1)
                clasglobal.WriteToFile(dschart1.chartlist1_run.Rows[rownum], clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())], strpath, rownum, 1);//header in file
            else
            {
                StreamWriter s = new StreamWriter(strpath, true);
                s.WriteLine();
                s.Close();
            }

            StreamWriter sa1 = new StreamWriter(strpath, true);
            sa1.WriteLine("============================================================");

            if (cycle_number_in_file > 1)
            {
                sa1.WriteLine("Cycle Number: " + (cycle_number_in_file).ToString());
                sa1.WriteLine();
            }

            cycle_number_in_file++;

            string Ln = "";

            if (typecall == 1)
            {
                for (int i = 0; i < shomar_othertech; i++)
                {
                    Ln = othertech_val0[i].ToString().Trim();
                    Ln = Ln + "\t" + othertech_val3[i].ToString().Trim();
                    Ln = Ln + "\t" + othertech_val1[i].ToString().Trim();
                    Ln = Ln + "\t" + othertech_val2[i].ToString().Trim();
                    sa1.WriteLine(Ln.Trim());
                }
            }
            else
            {
                for (int count1 = 0; count1 < shomar_cha_chc; count1++)
                {
                    if (datas2_state[count1] == 1)
                    {
                        Ln = time_cha_chc[count1].ToString();
                        Ln = Ln + "\t" + volt_cha_chc[count1].ToString();
                        Ln = Ln + "\t" + volt_cha_chc[count1].ToString();
                        Ln = Ln + "\t";
                        sa1.WriteLine(Ln);
                    }
                }
            }
            sa1.Close();
            dschart1.chartlist1_run.Rows[rownum][23] = strpath;
        }

        private void run_key_tch0(int row)
        {

            fst1 = false;
            fst = fst1;
            S_et = false;

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))//if e1<e2
            {

                N_gtv = false; //voltage is not negative 
                Xinv = false;//voltage not inversion
            }
            else
            {

                N_gtv = true;//voltage is negative
                Xinv = true;// voltage is inversion
            }
            pulsy = false; //pulse is 

            doRun2(row);//اجرای تکنیک و نمایش همزمان جزییات روی نمودار
        }
        private void run_key_tch1(int row)
        {

            fst1 = false;
            fst = fst1;
            S_et = false;

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            pulsy = false;

            doRun2(row);
        }
        private void run_key_tch2(int row)
        {

            fst1 = false;
            fst = fst1;
            S_et = false;

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            pulsy = true;
            ngtv = !N_gtv;
            doRun2(row);
        }
        private void run_key_tch3(int row)
        {

            if (Analiz_St1 < 100)
            {
                fst1 = true;
                S_et = true;
            }
            else
            {
                fst1 = false;
                S_et = false;

            }
            fst = fst1;
            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            if (Analiz_St1 <= 2)
                Analiz_St1 = 1;
            pulsy = true;
            ngtv = N_gtv;
            if (!fst)
                doRun2(row);
            else
                dorun3(row);

        }
        private void run_key_tch4(int row)
        {
            if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 < 50) || (Analiz_St1 < 1))
            {
                fst1 = true;
                S_et = true;
            }
            else
            {

                fst1 = false;
                S_et = false;
            }

            if ((double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000 < 50) || (Analiz_St2 < 1))
            {
                fst2 = true;

            }
            else
            {
                fst2 = false;

            }

            fst = (fst1 & fst2);

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {
                N_gtv = false;
                Xinv = false;
            }
            else
            {
                N_gtv = true;
                Xinv = true;
            }
            pulsy = false;



            Elo = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
            if (double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) != double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()))
            {

                Elo = double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString());
            }
            NUM = (int)(32700 - (Math.Abs(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) - double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString())) * 2 * 1000));


            if (!fst)
            {
                // scanrate <= .2
                fst = fst1;
                doRun2(row);
            }
            else
            {
                //dorun3(row);//دکتر از این تابع استفاده کرده بود اما در تکنیک سی وی وقتی اسکن ریت بالا می رفت گراف خطا میزد

                doRun2(row);/////mn
            }
        }
        private void run_key_tch5(int row)
        {

            if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 < 50) || (Analiz_St1 < 1))
            {
                fst1 = true;
                S_et = true;
            }
            else
            {
                fst1 = false;
                S_et = false;

            }
            fst = fst1;
            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString())) // if e2>e1
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            pulsy = false;

            if (!fst)
                doRun2(row);
            else
                dorun3(row);
        }
        private void run_key_tch6(int row)
        {

            fst1 = false;
            fst = fst1;
            S_et = false;

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            pulsy = false;

            doRun2(row);
        }
        private void run_key_tch7(int row)
        {

            fst1 = false;
            fst = fst1;
            S_et = false;

            if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
            {

                N_gtv = false;
                Xinv = false;
            }
            else
            {

                N_gtv = true;
                Xinv = true;
            }
            pulsy = true;
            ngtv = !N_gtv;

            doRun2(row);
        }
        private void run_key_tch8(int row)
        {

            doRun2(row);
        }
        private void run_key_tch9(int row)
        {

            N_gtv = false;
            Xinv = false;
            dschart1.chartlist1_run.Rows[row][6] = 1;
            //v_ry = true;

            doRun2(row);
        }
        private void run_key_tch10(int row)
        {

            N_gtv = false;
            Xinv = false;
            Array.Clear(datas2_0, 0, datas2_0.Length);
            Array.Clear(datas2_1, 0, datas2_1.Length);
            Array.Clear(datas2_2, 0, datas2_2.Length);
            Array.Clear(datas2_state, 0, datas2_state.Length);
            for (int i = 0; i < datas2_state.Length; i++)
                datas2_state[i] = 2;
            Array.Clear(time_cha_chc, 0, time_cha_chc.Length);
            Array.Clear(volt_cha_chc, 0, volt_cha_chc.Length);
            shomar_cha_chc = 0;
            shomarchpa = 0;

            //dorun2_cha_chp(row);
            doRun2_1(row);
        }
        private void run_key_tch14(int row)
        {
            N_gtv = false;
            Xinv = false;
            Array.Clear(datas2_0, 0, datas2_0.Length);
            Array.Clear(datas2_1, 0, datas2_1.Length);
            Array.Clear(datas2_2, 0, datas2_2.Length);
            Array.Clear(datas2_state, 0, datas2_state.Length);
            for (int i = 0; i < datas2_state.Length; i++)
                datas2_state[i] = 2;
            Array.Clear(time_cha_chc, 0, time_cha_chc.Length);
            Array.Clear(volt_cha_chc, 0, volt_cha_chc.Length);
            shomar_cha_chc = 0;
            shomarchpa = 0;
            // dorun2_costum(row);
            doRun2_1(row);
        }
        private void run_key_tch11(int row)
        {
            N_gtv = false;
            Xinv = false;
            Array.Clear(datas2_0, 0, datas2_0.Length);
            Array.Clear(datas2_1, 0, datas2_1.Length);
            Array.Clear(datas2_2, 0, datas2_2.Length);
            Array.Clear(datas2_state, 0, datas2_state.Length);
            for (int i = 0; i < datas2_state.Length; i++)
                datas2_state[i] = 2;
            Array.Clear(time_cha_chc, 0, time_cha_chc.Length);
            Array.Clear(volt_cha_chc, 0, volt_cha_chc.Length);
            shomar_cha_chc = 0;
            shomarchpa = 0;

            doRun2_1(row);
        }
        private void run_key_tch12(int row)
        {
            N_gtv = false;
            Xinv = false;
            Array.Clear(datas2_0, 0, datas2_0.Length);
            Array.Clear(datas2_1, 0, datas2_1.Length);
            Array.Clear(datas2_2, 0, datas2_2.Length);
            Array.Clear(datas2_state, 0, datas2_state.Length);
            for (int i = 0; i < datas2_state.Length; i++)
                datas2_state[i] = 2;
            Array.Clear(time_cha_chc, 0, time_cha_chc.Length);
            Array.Clear(volt_cha_chc, 0, volt_cha_chc.Length);
            shomar_cha_chc = 0;
            shomarchpa = 0;
            //dorun2_costum(row);
            doRun2_1(row);
        }
        private void run_key_tch15(int row)
        {
            N_gtv = false;
            Xinv = false;
            dorun2_costum_2(row);
        }

        public void runf(byte rng, double cp11, double ct11, double cp12, double ct12, double cp13, double ct13)
        {
            this.treeView3.Nodes[0].Nodes.Clear();
            Analiz_CP1 = cp11 * 1000;
            Analiz_CP2 = cp12 * 1000;
            Analiz_CP3 = cp13 * 1000;
            Analiz_CT1 = ct11;
            Analiz_CT2 = ct12;
            Analiz_CT3 = ct13;
            Range1 = rng;

            if (outc == 0xA0)
            {
                outc = 0x20;
            }
            if (thread_grf.ThreadState == ThreadState.Aborted || thread_grf.ThreadState == ThreadState.Unstarted)
                thread_grf.Start();
            else if (thread_grf.ThreadState == ThreadState.Stopped)
            {
                thread_grf = new Thread(Run_key);

                thread_grf.Start();
            }

        }

        delegate void Setgrftchart1Callback(bool overlaymain, int row1, int code, int state);
        private void Setgrftchart1(bool overlaymain, int row1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback d = new Setgrftchart1Callback(Setgrftchart1);
                        this.Invoke(d, new object[] { overlaymain, row1, code, 2 });
                    }
                    else
                    {
                        OnlineChart.Titles.Clear();
                        OnlineChart.Titles.Add("t1");
                    }
                    break;
                case 2:

                    if (state == 1)
                    {
                        Setgrftchart1Callback d = new Setgrftchart1Callback(Setgrftchart1);
                        this.Invoke(d, new object[] { overlaymain, row1, code, 2 });
                    }
                    else
                    {
                        ////Ye seri kar alaki bara rangi kardan line chart :| (keighobadi)
                        //num_Series = OnlineChart.Series.Count - 1;
                        //Random Random1 = new Random();
                        //int Random_Color_Index = 0;
                        //Random_Color_Index = (int)(Random1.NextDouble() * 90);
                        //OnlineChart.Series[0].Color = Color.Red;

                        if (!overlaymain)
                        {
                            OnlineChart.Series[0].Points.Clear();
                            OnlineChart.Series.Remove(OnlineChart.Series[0]);
                            if (OnlineChart.Series.Count > 1)
                            {
                                OnlineChart.Series[1].Points.Clear();
                                OnlineChart.Series.Remove(OnlineChart.Series[1]);
                            }
                        }

                        OnlineChart.Titles[0].Text = clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        OnlineChart.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        OnlineChart.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        if (num_Series < 1)
                            Overlay_tech[0] = int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString());
                        else
                            Overlay_tech[num_Series] = int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString());
                    }
                    break;
                case 3:
                    if (state == 1)
                    {
                        Setgrftchart1Callback d = new Setgrftchart1Callback(Setgrftchart1);
                        this.Invoke(d, new object[] { overlaymain, row1, code, 2 });
                    }
                    else
                    {
                        OnlineChart.Series[row1].Points.Clear();
                    }
                    break;

            }
        }
        delegate void Setgrftchart1Callback1(Series ser1, int code, int state);
        private void Setgrftchart11(Series ser1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback1 d = new Setgrftchart1Callback1(Setgrftchart11);
                        this.Invoke(d, new object[] { ser1, code, 2 });
                    }
                    else
                    {
                        OnlineChart.Series.Add(ser1);
                    }
                    break;

            }
        }
        delegate void Setgrftchart1Callback2(int ser1, double V1, double I1, int code, int state);
        private void Setgrftchart12(int ser1, double V1, double I1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback2 d = new Setgrftchart1Callback2(Setgrftchart12);
                        this.Invoke(d, new object[] { ser1, V1, I1, code, 2 });
                    }
                    else
                    {
                        OnlineChart.Series[ser1].Points.AddXY(V1, I1);
                    }
                    break;
            }
        }

        delegate void Settxtfill(int typetxt, string txt, int code, int state);
        private void Settxtfill_func(int typetxt, string txt, int code, int state)
        {

            switch (code)
            {
                case 1:
                    if (state == 1)
                    {
                        Settxtfill d = new Settxtfill(Settxtfill_func);
                        this.Invoke(d, new object[] { typetxt, txt, code, 2 });
                    }
                    else
                    {
                        txtcomment.Text = txt.Trim();
                    }
                    break;
                case 2:
                    if (state == 1)
                    {
                        Settxtfill d = new Settxtfill(Settxtfill_func);
                        this.Invoke(d, new object[] { typetxt, txt, code, 2 });
                    }
                    else
                    {
                        //  txtcomment.Text= txt + Environment.NewLine + txtcomment.Text.Trim();
                        if (typetxt == 1)
                            clasglobal.AppendText(txtcomment, txt + Environment.NewLine, Color.Black);
                        else if (typetxt == 2)
                            clasglobal.AppendText(txtcomment, txt + Environment.NewLine, Color.Red);

                        string s = "";
                        s = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\history_act.txt";

                        StreamWriter Fl = new StreamWriter(s, true, Encoding.ASCII);

                        Fl.WriteLine(DateTime.Now.ToString() + "     " + txt.Trim());
                        Fl.Close();
                    }
                    break;
            }
        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        public void show_cycle_runing_CV(int number)
        {
            if (number > 1)
                mainf.TechStatus.Text = (show_number_runing + 1).ToString().Trim() + "_" + number.ToString() + " - Running technique " + treeView3.Nodes[0].Nodes[show_number_runing].Text;
            else
                mainf.TechStatus.Text = (show_number_runing + 1).ToString().Trim() + " - Running technique " + treeView3.Nodes[0].Nodes[show_number_runing].Text;
        }

        delegate void SetgrfnodeCallback(int txt, int code, int state);
        private void Setgrfnodetext(int txt, int code, int state)
        {
            switch (code)
            {
                case 1:
                    if (state == 1)//Vazeiaty Mibashad Baraye Neshan Dadan Technique Dar Hal Ejra
                    {
                        SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        this.treeView3.Nodes[0].Nodes[txt].ForeColor = Color.DarkCyan;
                        mainf.TechStatus.ForeColor = Color.DarkCyan;
                        mainf.TechStatus.Text = (txt + 1).ToString().Trim() + "- Running technique " + treeView3.Nodes[0].Nodes[txt].Text;
                    }
                    break;
                case 2:
                    if (state == 1)
                    {
                        SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        this.treeView3.Nodes[0].Nodes[txt].ForeColor = Color.Green;
                        if (OnlineChart.Series[0].Points.Count > 0)
                            Parameters.Text = Parameters.Text + "\t\t*******************\nE1:" + OnlineChart.Series[0].Points.FindMinByValue("X").XValue.ToString("000e-0") +
                                "\nE2:" + OnlineChart.Series[0].Points.FindMaxByValue("X").XValue.ToString("000e-0") + "\nMin. I:" + OnlineChart.Series[0].Points.FindMinByValue("Y").YValues[0].ToString("000e-0") +
                                "\nMax. I:" + OnlineChart.Series[0].Points.FindMaxByValue("Y").YValues[0].ToString("000e-0");

                        DataRow dr = this.mainf.grf.dschart2.analyselist.NewRow();
                        dr[0] = int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());
                        dr[1] = double.Parse(dschart1.chartlist1_run.Rows[txt][3].ToString());
                        dr[2] = double.Parse(dschart1.chartlist1_run.Rows[txt][4].ToString());
                        dr[3] = double.Parse(dschart1.chartlist1_run.Rows[txt][5].ToString());
                        dr[4] = double.Parse(dschart1.chartlist1_run.Rows[txt][6].ToString());
                        dr[5] = double.Parse(dschart1.chartlist1_run.Rows[txt][7].ToString());
                        dr[6] = double.Parse(dschart1.chartlist1_run.Rows[txt][8].ToString());
                        dr[7] = double.Parse(dschart1.chartlist1_run.Rows[txt][9].ToString());
                        dr[8] = double.Parse(dschart1.chartlist1_run.Rows[txt][10].ToString());
                        dr[9] = double.Parse(dschart1.chartlist1_run.Rows[txt][11].ToString());
                        dr[10] = double.Parse(dschart1.chartlist1_run.Rows[txt][12].ToString());
                        dr[11] = movedata.main_cycle;// double.Parse(dschart1.chartlist1_run.Rows[txt][13].ToString());//i dont know why set value 1 less 
                        dr[12] = double.Parse(dschart1.chartlist1_run.Rows[txt][14].ToString());
                        if (int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 9)
                            dr[13] = double.Parse(dschart1.chartlist1_run.Rows[txt][15].ToString()) * 0.001;
                        else
                        dr[13] = double.Parse(dschart1.chartlist1_run.Rows[txt][15].ToString()) * 1000;
                        dr[14] = double.Parse(dschart1.chartlist1_run.Rows[txt][16].ToString()) * 1000;
                        dr[15] = double.Parse(dschart1.chartlist1_run.Rows[txt][17].ToString());
                        dr[16] = double.Parse(dschart1.chartlist1_run.Rows[txt][18].ToString());
                        dr[17] = double.Parse(dschart1.chartlist1_run.Rows[txt][2].ToString());
                        dr[18] = 0;
                        dr[19] = 0;
                        dr[20] = 0;
                        dr[21] = dschart1.chartlist1_run.Rows[txt][23].ToString();
                        dr[22] = dschart1.chartlist1_run.Rows[txt][24].ToString();
                        this.mainf.grf.dschart2.analyselist.Rows.Add(dr);
                        this.mainf.grf.dschart2.analyselist.AcceptChanges();
                        int maxrana = this.mainf.grf.dschart2.analyselist.Rows.Count;
                        if (countcv == 0)
                            if (dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() == "")//comment
                                this.mainf.grf.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())]);
                            else
                                this.mainf.grf.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())] + "(" + dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() + ")");
                        else
                            if (dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() == "")
                            this.mainf.grf.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())] + "-" + countcv);
                        else
                            this.mainf.grf.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())] + "(" + dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() + ") -" + countcv);

                        this.mainf.grf.treeView3.Nodes[0].LastNode.ForeColor = Color.Blue;
                        long countarr = 0;
                        if (int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 10 || int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 11 || int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 12)
                        {
                            Array.Resize(ref time_cha_chc, shomar_cha_chc);
                            Array.Resize(ref volt_cha_chc, shomar_cha_chc);

                            countarr = shomar_cha_chc;
                        }
                        else
                        {
                            Array.Resize(ref othertech_val0, Convert.ToInt32(shomar_othertech));
                            Array.Resize(ref othertech_val1, Convert.ToInt32(shomar_othertech));
                            Array.Resize(ref othertech_val2, Convert.ToInt32(shomar_othertech));
                            Array.Resize(ref othertech_val3, Convert.ToInt32(shomar_othertech));
                            countarr = shomar_othertech;
                        }

                        if (int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 10 || int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 11 || int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString()) == 12)
                        {
                            this.mainf.grf.tech = byte.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());
                            // this.mainf.grf.Set_Graph(0, false, true);

                            this.mainf.grf.show_Graph(maxrana - 1, this.mainf.grf, "MainChart", time_cha_chc, volt_cha_chc, true, shomar_cha_chc);
                            this.mainf.grf.treeView3.SelectedNode = this.mainf.grf.treeView3.Nodes[0].Nodes[maxrana - 1];
                            this.mainf.grf.tChart1.Series[maxrana - 1].Enabled = false;
                        }
                        else
                        {
                            this.mainf.grf.tech = byte.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());
                            // this.mainf.grf.Set_Graph(0, false, true);

                            this.mainf.grf.show_Graph(maxrana - 1, this.mainf.grf, "MainChart", othertech_val0, othertech_val3, true, shomar_othertech);
                            this.mainf.grf.treeView3.SelectedNode = this.mainf.grf.treeView3.Nodes[0].Nodes[maxrana - 1];
                            this.mainf.grf.tChart1.Series[maxrana - 1].Enabled = false;
                        }
                    }
                    break;
                case 3:// add to top_left
                    if (state == 1)
                    {
                        SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        this.treeView3.Nodes[0].Nodes.Clear();
                        for (int iii1 = 0; iii1 < dschart1.chartlist1_run.Rows.Count; iii1++)
                        {
                            this.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(this.dschart1.chartlist1_run.Rows[iii1][1].ToString())]);
                        }
                    }
                    break;
                case 4:
                    if (state == 1)
                    {
                        SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        OnlineChart.Series[0].Points.Clear();
                        OnlineChart.Series[0].Points.AddXY(othertech_val0[0], othertech_val1[0]);

                    }
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ColorDialog cl = new ColorDialog();
                cl.Color = OnlineChart.ChartAreas[0].BackColor;
                cl.CustomColors = new int[] { OnlineChart.ChartAreas[0].BackColor.R, OnlineChart.ChartAreas[0].BackColor.G, OnlineChart.ChartAreas[0].BackColor.B };
                cl.FullOpen = true;

                if (cl.ShowDialog() == DialogResult.OK)
                {
                    OnlineChart.BackColor = cl.Color;
                    OnlineChart.BorderlineColor = cl.Color;
                    OnlineChart.ChartAreas[0].BackColor = cl.Color;
                    panel1.BackColor = cl.Color;
                    panel2.BackColor = cl.Color;

                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[0] = "runing:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        delegate void SetgrfselectnodeCallback(int row, int code, int state);
        private void Setgrfnodeselect(int row, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SetgrfselectnodeCallback d = new SetgrfselectnodeCallback(Setgrfnodeselect);
                        this.Invoke(d, new object[] { row, code, 2 });
                    }
                    else
                    {
                        this.treeView3.SelectedNode = this.treeView3.Nodes[0].Nodes[row];
                    }
                    break;
                case 2:
                    if (state == 1)
                    {
                        SetgrfselectnodeCallback d = new SetgrfselectnodeCallback(Setgrfnodeselect);
                        this.Invoke(d, new object[] { row, code, 2 });
                    }
                    else
                    {
                        // OnlineChart.Refresh();
                    }
                    break;

            }
        }

        delegate void SetenabledCallback(bool bl, int code, int state);
        private void Setbtnenabled(bool bl, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SetenabledCallback d = new SetenabledCallback(Setbtnenabled);
                        this.Invoke(d, new object[] { bl, code, 2 });
                    }
                    else
                    {
                        this.mainf.stf.tsbRun.Enabled = false;
                        this.mainf.stf.tsbNew.Enabled = false;
                        mainf.stf.runToolStripMenuItem1.Enabled = false;
                        tsbhold.Enabled = true;
                        tsbstop.Enabled = true;
                        mainf.toolStripProgressBar1.Visible = true;
                    }
                    break;
                case 2:
                    if (state == 1)
                    {
                        SetenabledCallback d = new SetenabledCallback(Setbtnenabled);
                        this.Invoke(d, new object[] { bl, code, 2 });
                    }
                    else
                    {
                        mainf.TechStatus.Text = "Stop";
                        this.mainf.stf.tsbNew.Enabled = true;
                        mainf.toolStripProgressBar1.Visible = false;

                        tsbhold.Enabled = false;
                        tsbstop.Enabled = false;

                        mainf.stf.runToolStripMenuItem1.Enabled = true;
                        mainf.stf.tsbRun.Enabled = true;
                        // timer3.Enabled = false;
                    }
                    break;
            }
        }
        delegate void SetvisibleCallback(bool bl, int code, int state);

        private void Setvisibleitem(bool bl, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SetenabledCallback d = new SetenabledCallback(Setbtnenabled);
                        this.Invoke(d, new object[] { bl, code, 2 });
                    }
                    else
                    {

                        mainf.toolStripProgressBar1.Visible = true;
                    }
                    break;
            }
        }
        delegate void SetcheckedCallback(bool bl, int code, int state);
        private void Setcheckeditem(bool bl, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SetenabledCallback d = new SetenabledCallback(Setbtnenabled);
                        this.Invoke(d, new object[] { bl, code, 2 });
                    }
                    else
                    {
                        //cbOverlay.Checked = bl;
                    }
                    break;
            }
        }
        private void Set_value2(int row)
        {
            //_Timer = 0;
            detect_Error = false;
            shomar_othertech = 0;
            Array.Clear(othertech_val0, 0, othertech_val0.Length);
            Array.Clear(othertech_val1, 0, othertech_val1.Length);
            Array.Clear(othertech_val2, 0, othertech_val2.Length);
            Array.Clear(othertech_val3, 0, othertech_val3.Length);

            switch (byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()))
            {
                case clasglobal.DCV:
                case clasglobal.NPV:
                case clasglobal.DPV:
                    dschart1.chartlist1_run.Rows[row][15] = dschart1.chartlist1_run.Rows[row][9];
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][9];
                    // Analiz_R_ng = Range1;
                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;
                    break;

                case clasglobal.SWV:
                    //  Analiz_R_ng = Range1;
                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;
                    break;

                case clasglobal.CV:
                    dschart1.chartlist1_run.Rows[row][15] = dschart1.chartlist1_run.Rows[row][9];
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][9];
                    //  Analiz_R_ng = Range1;

                    //Analiz_Hld_t = 0;

                    /////////////////////////////////////////////به علت عدم انتساب غیر فعال کردم
                    //if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) < 1)
                    //    Analiz_St1 = (int)((int)(double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000) | 0x8000);
                    //else
                    //    Analiz_St1 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()));

                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;//   قبلا این فعال بود


                    if (Analiz_St1 < 1)
                        dschart1.chartlist1_run.Rows[row][15] = (int)((Math.Truncate(Analiz_St1 * 1000.0))) | 0x8000;
                    else
                        dschart1.chartlist1_run.Rows[row][15] = (int)(Math.Truncate(Analiz_St1));



                    Analiz_St2 = Analiz_St1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][15];
                    //if (!cbOverlay.Checked)
                    //{
                    //    cbOverlay.Checked = (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 1);
                    //}

                    break;
                case clasglobal.LSV:
                    dschart1.chartlist1_run.Rows[row][15] = dschart1.chartlist1_run.Rows[row][9];
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][9];

                    if (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) < 1)
                        Analiz_St1 = (int)((int)(double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000) | 0x8000);
                    else
                        Analiz_St1 = double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString());
                    /////////////////////////////////////////////

                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;
                    if (Analiz_St1 < 1)
                        dschart1.chartlist1_run.Rows[row][15] = (int)(Math.Round(Analiz_St1 * 1000.0, 2)) | 0x8000;
                    else
                        dschart1.chartlist1_run.Rows[row][15] = Math.Round(Analiz_St1, 2);
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][15];
                    Analiz_St2 = Analiz_St1;
                    break;
                case clasglobal.DCs:
                    dschart1.chartlist1_run.Rows[row][15] = dschart1.chartlist1_run.Rows[row][9];
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][9];
                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;
                    if (Analiz_St1 < 1)
                        dschart1.chartlist1_run.Rows[row][15] = (int)(Math.Round(Analiz_St1 * 1000.0, 2)) | 0x8000;
                    else
                        dschart1.chartlist1_run.Rows[row][15] = Math.Round(Analiz_St1, 2);
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][15];
                    Analiz_St2 = Analiz_St1;

                    break;
                case clasglobal.DPs:
                    dschart1.chartlist1_run.Rows[row][15] = dschart1.chartlist1_run.Rows[row][9];
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][9];
                    Analiz_St1 = ((double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000) / (double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) * 1000)) * 1000;
                    if (Analiz_St1 < 1)
                        dschart1.chartlist1_run.Rows[row][15] = (int)(Math.Round(Analiz_St1 * 1000.0, 2)) | 0x8000;
                    else
                        dschart1.chartlist1_run.Rows[row][15] = Math.Round(Analiz_St1, 2);
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = dschart1.chartlist1_run.Rows[row][15];
                    Analiz_St2 = Analiz_St1;
                    break;
                case clasglobal.CPC:

                    dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());
                    break;
                case clasglobal.CHP:

                    //dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;

                    break;
                case clasglobal.CHA:

                    dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;

                    break;
                case clasglobal.CHC:

                    dschart1.chartlist1_run.Rows[row][13] = 1;

                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;

                    break;
                case clasglobal.CD:

                    //dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;

                    dschart1.chartlist1_run.Rows[row][12] = double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString());
                    //dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;
                    //dschart1.chartlist1_run.Rows[row][13] = double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000;
                    break;

                case clasglobal.OCP:
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    break;
                default: break;
            }
            ////////////////////////////////////
            detect_Error = false;
            Send_usb_Parameter(row);//ارسال پارامتر های تکنیک به ترتیب ول تا آخر به دستگاه

        }
        /////////////////////////////////////////////////////////////

        unsafe public void Send_usb_Parameter(int row)
        {
            byte num;
            byte* OutBuff = &num;
            byte* InBuff = &num;
            byte[] datas = new byte[3];
            byte[] data = new byte[65520];
            // int j = 0;
            double[] C = new double[32710];
            byte t_mp;
            Int64 Temp = 0;

            if (serialPort2.IsOpen)
            {
                Settxtfill_func(2, $"start", 2, 1);
                //System.Threading.Thread.Sleep(1);
                serialPort2.Write("@");
                t_mp = byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());

                serialPort2.Write(t_mp.ToString() + "@");
                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 15)
                {
                    Temp = (long)Analiz_CP1;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CP2;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CP3;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT1;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT2;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT3;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (int)double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString());//quesent time
                    serialPort2.Write(Temp.ToString() + "@");
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 0)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");

                    }

                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 1)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);//pw
                        serialPort2.Write(Temp.ToString() + "@");

                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 2)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 3)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = long.Parse(dschart1.chartlist1_run.Rows[row][12].ToString());//fr
                        Settxtfill_func(2, $"fr {Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//e3
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString());//cycle
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 5)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 6)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 7)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 8)
                    {
                        //t_mp = Range1;
                        //serialPort2.Write(t_mp.ToString() + "@");
                        //Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        //serialPort2.Write(Temp.ToString() + "@");
                        //Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        //serialPort2.Write(Temp.ToString() + "@");
                        //Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        //serialPort2.Write(Temp.ToString() + "@");
                        //Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        //serialPort2.Write(Temp.ToString() + "@");
                        //serialPort2.Write("finish_p");


                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9)
                    {
                        fst = true;
                        S_et = true;
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()));// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()));//for low cpc,and high cpc
                        Settxtfill_func(2, $"cpc_mode={Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                        ti = 0.02;
                        if (Analiz_T3 > 100)
                            ti = ((int)(Analiz_T3 / 100)) * ti;
                        //serialPort2.Write("finish_p");

                    }

                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                    {


                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");

                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));// CYCLE
                        serialPort2.Write(Temp.ToString() + "@");

                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//eup
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);// edown
                        serialPort2.Write(Temp.ToString() + "@");
                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                    //Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                                    Settxtfill_func(2, $"analiz2={tmep_t}", 2, 1);
                                    //Settxtfill_func(2, $"t_cloun={Temp}", 2, 1);

                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Settxtfill_func(2, $"analiz={Temp}", 2, 1);

                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {
                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);


                                    //Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                                    //Settxtfill_func(2, $"analiz={tmep_t}", 2, 1);
                                    //Settxtfill_func(2, $"t_cloun={Temp}", 2, 1);

                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
                    {

                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) * 1000);//i1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) * 1000);//i2 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1 
                        serialPort2.Write(Temp.ToString() + "@");



                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 14)
                    {

                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//ecut 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);//ecutoff
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString())); //i1 (me)
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()));//i2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));//cycle
                        serialPort2.Write(Temp.ToString() + "@");
                        Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                        serialPort2.Write(".");

                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                    serialPort2.Write(tmep_t.ToString() + "@");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                        //serialPort2.Write("finish_p");
                    }

                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                    {

                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 16))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()));//t1+t2
                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }






                    }
                }

            }
            else
            {
                serialPort2.Open();
                //serialPort2.Write("start_p");
                //System.Threading.Thread.Sleep(1);
                serialPort2.Write("@");
                t_mp = byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());
                Settxtfill_func(2, $"start", 2, 1);
                serialPort2.Write(t_mp.ToString() + "@");
                if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 15)
                {

                    Temp = (long)Analiz_CP1;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CP2;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CP3;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT1;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT2;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (long)Analiz_CT3;
                    serialPort2.Write(Temp.ToString() + "@");
                    Temp = (int)double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString());//quesent time
                    serialPort2.Write(Temp.ToString() + "@");
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 0)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                    }

                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 1)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);//pw
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        // serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 2)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 3)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = long.Parse(dschart1.chartlist1_run.Rows[row][12].ToString());//fr
                                                                                            // Settxtfill_func(2, $"fr {Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)
                    {
                        int TEMP2;
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//e3
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");

                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 

                        Settxtfill_func(2, $"t1={Temp}", 2, 1);
                        TEMP2 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);// t1 
                        Settxtfill_func(2, $"t2={TEMP2}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString());//cycle
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 5)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 6)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 7)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");

                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 8)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9)
                    {
                        fst = true;
                        S_et = true;
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()));// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()));//for low cpc,and high cpc
                        Settxtfill_func(2, $"cpc_mode={Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");

                        Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                        ti = 0.02;
                        if (Analiz_T3 > 100) ti = ((int)(Analiz_T3 / 100)) * ti;
                        //serialPort2.Write("finish_p");

                    }


                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                    {


                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");

                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));// CYCLE
                        serialPort2.Write(Temp.ToString() + "@");

                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//eup
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);// edown
                        serialPort2.Write(Temp.ToString() + "@");
                    
                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
                                    //Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                                    Settxtfill_func(2, $"analiz2={tmep_t}", 2, 1);
                                    //Settxtfill_func(2, $"t_cloun={Temp}", 2, 1);
                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                    }



                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                        serialPort2.Write(Temp.ToString() + "@");
                        Settxtfill_func(2, $"t1{Temp}", 2, 1);

                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);


                                    //Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                                    Settxtfill_func(2, $"analiz={tmep_t}", 2, 1);
                                    //Settxtfill_func(2, $"t_cloun={Temp}", 2, 1);

                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");
                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }
                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)
                    {
                        t_mp = Range1;
                        serialPort2.Write(t_mp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//i1 
                        Settxtfill_func(2, $"current={Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//i2 
                        Settxtfill_func(2, $"current2={Temp}", 2, 1);
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                        serialPort2.Write(Temp.ToString() + "@");
                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
                                    Settxtfill_func(2, $"analiz={tmep_t}", 2, 1);
                                    serialPort2.Write(tmep_t.ToString() + "@");
                                    serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                        //serialPort2.Write("finish_p");
                    }
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 14)
                    {

                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//ecut 
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);//ecutoff
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString())); //i1 (me)
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()));//i2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                        serialPort2.Write(Temp.ToString() + "@");
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));//cycle

                        serialPort2.Write(Temp.ToString() + "@");
                        Settxtfill_func(2, $"cycle={Temp}", 2, 1);
                        serialPort2.Write(".");

                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                Int16 tmep_t = 0;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                {
                                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                    //serialPort2.Write(tmep_t.ToString() + "@");
                                    //serialPort2.Write(".");

                                }
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                        //serialPort2.Write("finish_p");
                    }


                    if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 15)
                    {
                        Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                        serialPort2.Write(Temp.ToString() + "@");
                        serialPort2.Write(".");
                        if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 15))
                        {

                            fst = true;
                            S_et = true;
                            if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) > 9) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) <= 15))//if techno >9 and techno  < 15  
                            {
                                Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                                if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000 > 5024) //if (t1+t2)*1000  > 5024
                                {
                                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                                    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                                                                         //Settxtfill_func(2, $"t_cloun3={Temp}", 2, 1);
                                }
                                else
                                {
                                    Temp = (long)(5024 - (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000));
                                    ti = 1;

                                }
                                //if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 14)
                                //{
                                //    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);

                                //    //serialPort2.Write(tmep_t.ToString() + "@");
                                //    //serialPort2.Write(".");

                                //}
                            }
                            else  //Tch=13
                            {

                                Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                                ti = 0.02;
                                if (Analiz_T3 > 100)
                                    ti = ((int)(Analiz_T3 / 100)) * ti;

                            }

                            // V = ti; 
                        }

                        //serialPort2.Write("finish_p");
                    }

                }
            }

            Setbtnenabled(false, 1, 1);


        }

        //private void smoothexam(int row1, bool state)
        //{
        //    int i, m = 9, cmakhraj, j = 0, k = 0;
        //    double[] csorat = new double[m];
        //    double y = 0;
        //    cmakhraj = m * (m * m - 4) / 3;
        //    for (i = -(m - 1) / 2; i <= (m - 1) / 2; i++)
        //    {
        //        csorat[j] = 0.25 * (3 * m * m - 7 - 20 * i * i);
        //        j++;
        //    }

        //    if (row1 < (m - 1) / 2 || state)
        //        OnlineChart.Series[1].Points.AddXY(othertech_val3[row1], othertech_val3[row1]);
        //    else
        //    {
        //        for (i = -(m - 1) / 2; i <= (m - 1) / 2; i++)
        //        {
        //            y += (csorat[k] * othertech_val3[row1 + i]);
        //            k++;
        //        }
        //        y /= cmakhraj;
        //        OnlineChart.Series[1].Points.AddXY(othertech_val3[row1], y);
        //    }
        //}
        public void smootht0_t1(int row1, Color color1)
        {
            double Stp = 0;
            double IE = 0, FE = 0, I_tot = 0, A0, A1, B0, AB, A2, ai, I0, I1 = 0, b, Ec, V1, SS, EE = 0, EE1 = 0, EE2 = 0, mini, Mp, Vi = 0, Tss = 0;
            long Ts, len, k, j = 0, cl = 0, k1, NUM, MaxC, ngtv = 0, NM = 0, R_cmp = 0, vn = 0;
            bool cmpn = false, Colm = false, ctrl = false, Drvt = false, mx = false, Suby = false, grftfl = false;
            int Tq = 0, L_Teq, ser = 0; //,a;
            // char Sign = '0';
            len = shomar_othertech;
            L_Teq = int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()); //techno
            FE = double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString()) * 1000;//e2
            IE = double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()) * 1000;//e1
            Stp = double.Parse(dschart1.chartlist1_run.Rows[row1][6].ToString()) * 1000;//hs 
            Ts = (int)(double.Parse(dschart1.chartlist1_run.Rows[row1][9].ToString()) * 1000);//ts
            if (Ts > 32768)
            {
                Ts = (Ts & 32767) / 100;
                Tss = Ts / 10;
                //Rng = 10;
            }
            else
            {
                Tss = Ts;
            }

            NUM = len - 1;
            SS = (Tss / 1000);

            if (Suby)
                if ((Tq != L_Teq) || (NM != NUM) || ((L_Teq < 8) && ((EE1 != IE) || (EE2 != FE))))
                {

                    return;
                }
                else
                    ser++;
            MaxC = 0;
            vn = NUM;

            if ((FE - EE) > 0)
            {
                ngtv = 1;
                //  Sign = ' ';
            }
            else
            {
                ngtv = -1;
                // Sign = '-';
            }
            k = 1;
            for (int i = 1; i <= vn - 1; i++)
            {

                if (Math.Abs(othertech_val3[k] - othertech_val3[k - 1]) > 1000)
                    othertech_val3[k] = (othertech_val3[k - 1] + othertech_val3[k + 1]) / 2;
                k = k + 1;
            }
            k = vn;
            for (long i = vn; i >= 2; i--)
            {

                othertech_val3[k] = (othertech_val3[k - 1] + othertech_val3[k]) / 2;
                k = k - 1;

            }
            k = 1;
            for (int i = 1; i <= vn - 2; i++)
            {

                othertech_val3[k] = (othertech_val3[k - 1] + othertech_val3[k + 1]) / 2;
                k = k + 1;
            }
            for (int j1 = 3; j1 >= 0; j1--)
            {
                k = 4;
                for (int i = 4; i <= vn - 3; i++)
                {
                    I0 = ((othertech_val3[k - 1] - othertech_val3[k - 2]) + (othertech_val3[k - 2] - othertech_val3[k - 3]) + (othertech_val3[k - 3] - othertech_val3[k - 4])) / 3;
                    I1 = Math.Abs(othertech_val3[k] - othertech_val3[k - 1]);
                    if (I1 > Math.Abs(j1 * I0))
                        othertech_val3[k] = (othertech_val3[k - 2] + othertech_val3[k - 1] + othertech_val3[k + 2] + othertech_val3[k + 1]) / 4;
                    k = k + 1;
                }
            }
            k = 3;
            for (int i = 3; i <= vn - 4; i++)
            {
                othertech_val3[k] = (othertech_val3[k - 3] + othertech_val3[k - 2] + othertech_val3[k - 1] + othertech_val3[k + 3] + othertech_val3[k + 2] + othertech_val3[k + 1]) / 6;
                k = k + 1;
            }
            if (cl == 1)
            {
                I_tot = 0;
                MaxC = 0;
                mx = false;
                mini = 10000;
                k = NUM;

                for (int i = 1; i <= k - 5; i++)
                {
                    if ((i > 10) && (othertech_val3[i] > I_tot) && (othertech_val3[i] >= othertech_val3[i + 1]) && (othertech_val3[i] >= othertech_val3[i - 1]))
                    {
                        I_tot = othertech_val3[i];
                        MaxC = i;
                        mx = true;
                    }
                    if (othertech_val3[i] < mini)
                        mini = othertech_val3[i];
                }
            }

            if (Colm)
            {
                othertech_val3[10000] = 0;
                k = 10001;
                I_tot = 0;
                for (int j1 = 0; j1 <= NUM - 1; j1++)
                {
                    I_tot = I_tot + (othertech_val3[j1] + othertech_val3[j1 + 1]) * SS / 2;
                    othertech_val3[k] = I_tot;
                    k = k + 1;
                }
            }


            ///**************************************************/
            if (Drvt)
            {
                othertech_val3[10000] = 0;
                k = 10001;
                for (int j1 = 1; j1 <= NUM; j1++)
                {
                    othertech_val3[k] = (othertech_val3[j1] - othertech_val3[j1 - 1]) / SS;
                    k = k + 1;
                }
            }


            ser = 0;
            len = NUM;
            for (int la = OnlineChart.Series.Count - 1; la >= 0; la--)
                Setgrftchart1(false, la, 3, 1);


            if (ctrl)
            {

                for (long i = len - 1; i >= 1; i--)
                {
                    V1 = 1 / Math.Sqrt(SS * i);
                    I1 = othertech_val3[i];
                    OnlineChart.Series[ser].Points.AddXY(V1, I1, "", color1);

                }
            }
            else
            {
                k = 0;
                // a=0;
                if (IE > FE)
                    Stp *= (-1);
                if (L_Teq < 8)
                    Vi = IE + Stp * k;
                k1 = 0;
                I1 = othertech_val3[0];
                if ((Colm) || (Drvt))
                {
                    k1 = 10000;
                    I1 = othertech_val3[k1];
                }

                I_tot = 0;
                for (long i = k; i <= len; i++)
                {
                    if ((Colm) || (Drvt))
                        I1 = othertech_val3[k + k1];
                    else
                        I1 = othertech_val3[k];
                    if (L_Teq < 8)
                        V1 = Vi / 1000;
                    else
                        V1 = SS * (i + 1);
                    if ((I1 == 0) || (othertech_val3[k] < 0))
                        Ec = V1;

                    if ((L_Teq == 4) && (cmpn)) { }
                    else
                        V1 = V1 - (ngtv * R_cmp * I1) / 1000000;

                    {
                        if (grftfl)
                        {
                            if (I1 != 0)
                            {
                                OnlineChart.Series[ser].Points.AddXY(V1, Math.Log10(Math.Abs(I1)) - 6);
                            }
                        }
                        else
                        {
                            Setgrftchart12(ser, V1, I1, 1, 1);

                        }
                    }
                    Vi = Vi + Stp;

                    k = k + 1;
                }

            }

            if ((cl == 1) && (!grftfl) && (!Colm) && (!Drvt))
            {
                I0 = 10000;
                Mp = (25 / Math.Abs(Stp));
                if (Mp < 5)
                    Mp = 5;
                if (mx)
                {
                    for (long j1 = MaxC - (MaxC / 4); j1 >= Mp; j1--)
                    {
                        A0 = 0;
                        A1 = 0;
                        B0 = 0;

                        AB = 0;
                        V1 = (IE + Stp * j1) / 1000;
                        for (long i = j1; i >= (j1 - Mp + 1); i--)
                        {
                            V1 = V1 - Stp / 1000;
                            A0 = A0 + V1;
                            A1 = A1 + V1 * V1;
                            B0 = B0 + othertech_val3[i];
                            AB = AB + V1 * othertech_val3[i];
                        }
                        AB = Mp * AB - A0 * B0;
                        A2 = Mp * A1 - A0 * A0;
                        b = AB / A2;
                        ai = (B0 - b * A0) / Mp;

                        if (Math.Abs(b) < I0)
                        {
                            I0 = Math.Abs(b);
                            I1 = b;
                            I_tot = ai;
                            k = j;//-Mp;
                        }
                    }

                    AB = I1 * (IE + MaxC * Stp) / 1000 + I_tot;
                }
            }
        }

        public void smootht4(int row1, Color color1)
        {
            // string bat = "";
            long len, Fore = 0, Rev = 0, j = 0, NUM, vx = 0, vn = 0, k, Frs, Lst;
            double Stp = 0;
            double IE = 0, FE = 0, EE3 = 0, I_tot = 0, A0, A1, B0, AB, A2, ai, I0, I1 = 0, b, Ec, V1, SS, EE = 0, EE1 = 0, EE2 = 0, mini, Mp, Vi = 0, Tss = 0;
            int Ts, c_ycl = 0, cl = 1, k1, ser = 0, MaxC, ngtv = 0, NM = 0, R_cmp = 0;
            bool onc = false, cmpn = false, Conv = false, ctrl = false, Drvt = false, mx = false, Suby = false, grftfl = false;
            int Tq = 0, L_Teq; //,a;
            // char Sign = '0';
            len = shomar_othertech;
            L_Teq = int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString());
            IE = double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()) * 1000;
            FE = double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString()) * 1000;
            EE3 = double.Parse(dschart1.chartlist1_run.Rows[row1][5].ToString()) * 1000;
            onc = true;
            Stp = double.Parse(dschart1.chartlist1_run.Rows[row1][6].ToString()) * 1000;
            Ts = (int)(double.Parse(dschart1.chartlist1_run.Rows[row1][9].ToString()) * 1000);
            if (Ts > 32768)
            {
                Ts = (Ts & 32767) / 100;
                Tss = Ts / 10;
            }
            else
            {
                Tss = Ts;
            }

            Fore = (len / 4) + 1;
            Rev = len + 1;
            j = len;
            k = 0;
            NUM = j - 1;
            SS = (Tss / 1000);
            vx = NUM / 2;
            if (IE == EE3)
                vn = (int)(((FE - IE) / Stp) * 2);
            else
                vn = (int)(((FE - IE) / Stp) + ((FE - EE3) / Stp));
            if (vn >= NUM)
                onc = true;
            else
                onc = false;

            if (Suby)
                if ((Tq != L_Teq) || (NM != NUM) || ((EE1 != IE) || (EE2 != FE)))
                {

                    return;
                }
                else
                    ser++;
            MaxC = 0;
            vn = NUM;
            if (L_Teq == 4)
            {
                IE = IE + (FE - IE) % Stp;
                EE3 = EE3 + (FE - EE3) % Stp;
                if (EE3 != IE)
                    EE = EE3;
                else
                    EE = IE;
            }
            if ((FE - EE) > 0)
            {
                ngtv = 1;
                // Sign = ' ';
            }
            else
            {
                ngtv = -1;
                //  Sign = '-';
            }

            for (int i = 1; i <= vn - 1; i++)
            {
                if (Math.Abs(othertech_val3[i] - othertech_val3[i - 1]) > 1000)
                    othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }
            for (long i = vn; i >= 2; i--)
            {
                othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i]) / 2;
            }
            for (int i = 1; i <= vn - 2; i++)
            {
                othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }
            for (int j1 = 3; j1 >= 0; j1--)
            {
                for (int i = 4; i <= vn - 3; i++)
                {

                    I0 = ((othertech_val3[i - 1] - othertech_val3[i - 2]) + (othertech_val3[i - 2] - othertech_val3[i - 3]) + (othertech_val3[i - 3] - othertech_val3[i - 4])) / 3;
                    I1 = Math.Abs(othertech_val3[i] - othertech_val3[i - 1]);
                    if (I1 > Math.Abs(j1 * I0))
                        othertech_val3[i] = (othertech_val3[i - 2] + othertech_val3[i - 1] + othertech_val3[i + 2] + othertech_val3[i + 1]) / 4;
                }
            }

            for (int i = 3; i <= vn - 4; i++)
            {

                othertech_val3[i] = (othertech_val3[i - 3] + othertech_val3[i - 2] + othertech_val3[i - 1] + othertech_val3[i + 3] + othertech_val3[i + 2] + othertech_val3[i + 1]) / 6;

            }
            if ((cl == 1))
            {
                I_tot = 0;
                MaxC = 0;
                mx = false;
                mini = 10000;
                k = vx;
                for (int i = 1; i <= k - 5; i++)
                {
                    if ((i > 10) && (othertech_val3[i] > I_tot) && (othertech_val3[i] >= othertech_val3[i + 1]) && (othertech_val3[i] >= othertech_val3[i - 1]))
                    {
                        I_tot = othertech_val3[i];
                        MaxC = i;
                        mx = true;
                    }
                    if (othertech_val3[i] < mini)
                        mini = othertech_val3[i];
                }
                I_tot = 0;
                Rev = 0;

                for (long i = k + Convert.ToInt64(10); i <= NUM - 10; i++)
                {
                    if ((othertech_val3[i] < I_tot) && (othertech_val3[i] <= othertech_val3[i + 1]) && (othertech_val3[i] <= othertech_val3[i - 1]))
                    {
                        I_tot = othertech_val3[i];
                        Rev = i;

                    }
                }
            }
            ///**************************************************/
            if ((NUM < 10000) && onc && Drvt)
            {
                othertech_val3[10000] = 0;
                k = 10001;
                for (int j1 = 1; j1 <= NUM; j1++)
                {
                    othertech_val3[k] = (othertech_val3[j1] - othertech_val3[j1 - 1]) / SS;
                    k = k + 1;
                }
            }
            else
                Drvt = false;
            //{/**************************************************/
            if (Conv)  //C
            {
                othertech_val3[Fore] = 0;
                k = Fore + 1;
                Rev = 0;
                for (int j1 = 1; j1 <= NUM; j1++)
                {
                    if (IE + (j1 * Stp) == FE)
                    {
                        Rev = j1;
                        Stp = Stp * (-1);
                    }
                    I_tot = 0;
                    for (int i = 0; i <= j1 - 1; i++)
                    {
                        I_tot = I_tot + othertech_val3[i] * 1.0 / Math.Sqrt(j1 - i);
                    }
                    I_tot = I_tot * Math.Sqrt(Tss) * 0.017841241;
                    othertech_val3[k] = I_tot;
                    k = k + 1;
                }
                Stp = Stp * (-1);
            }
            len = NUM;
            ser = 0;
            for (int la = OnlineChart.Series.Count - 1; la >= 0; la--)
                Setgrftchart1(false, la, 3, 1);// OnlineChart.Series[la].Points.Clear();

            if (ctrl)
            {
                // bat = "(t:sec)-1/2";
                for (long i = len - Convert.ToInt64(1); i >= 1; i--)
                {
                    V1 = 1 / Math.Sqrt(SS * i);
                    I1 = othertech_val3[i];
                    OnlineChart.Series[ser].Points.AddXY(V1, I1 * 10, "", color1);

                }
            }
            else
            {
                k = 0;
                Vi = IE + Stp * k;
                k1 = 0;
                I1 = othertech_val3[0];//$$$$
                if (Drvt)
                {
                    k1 = 10000;
                    I1 = othertech_val3[k1];
                }
                if (Conv)   //C
                {
                    cl = 2;
                    ser = 2;
                    OnlineChart.Series[ser].Color = color1;
                    len = NUM + Fore;
                    k = Fore;
                }

                I_tot = 0;
                if (double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()) > double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString()))
                    Stp *= (-1);
                for (long i = k; i <= len; i++)
                {
                    if (Drvt)
                        I1 = othertech_val3[k + k1];
                    else
                        I1 = othertech_val3[k];
                    V1 = Vi / 1000;

                    if ((I1 == 0) || (othertech_val3[k] < 0))
                        Ec = V1;

                    if (cmpn) { }
                    else
                        V1 = V1 - (ngtv * R_cmp * I1) / 1000000;

                    {
                        if (grftfl)
                        {
                            if (I1 != 0)
                            {

                                OnlineChart.Series[ser].Points.AddXY(V1, Math.Log10(Math.Abs(I1)) - 6);
                                //if ((Math.Abs(Math.Log10(Math.Abs(I1))-6) < 1000) && (Math.Abs(V1) < 1000))
                                //    Writeln(tmpDat, floattostrf(V1, ffNumber, 4, 3) + "  " + floattostrf(Math.Log10(Math.Abs(I1)) - 6, ffNumber, 4, 3));
                                // else
                                //    Writeln(tmpDat, V1.ToString() + "  " + (Math.Log10(Math.Abs(I1)) - 6).ToString());
                            }
                        }
                        else
                        {

                            Setgrftchart12(ser, V1, I1, 1, 1);// OnlineChart.Series[ser].Points.AddXY(V1, I1);

                        }
                    }

                    Vi = Vi + Stp;
                    if (Vi == FE)
                    {
                        //if (!Draw_Itself)
                        //    ser = ser + 1;
                        //OnlineChart.Series[ser].Color =color1;
                        //(OnlineChart.Series[ser] as Tfastlineseries).LinePen.width=Thik[cl];
                        Stp = Stp * (-1);

                    }
                    if (Vi == EE)
                    {
                        if (ser < 30)
                        {
                            if (cl == 1)
                                Frs = k;
                            Lst = k;
                            //if (!Draw_Itself)
                            //    ser = ser + 1;
                            cl = cl + 1;
                            c_ycl = c_ycl + 1;
                            // OnlineChart.Series[ser].Color = color1;
                            //(OnlineChart.Series[ser] as Tfastlineseries).LinePen.width=Thik[cl];
                            Stp = Stp * (-1);
                        }
                        else
                            break;
                    }

                    k = k + 1;
                }
                Stp = Stp * (-1);
            }
            if ((cl == 1) && (!grftfl) && (!Drvt))
            {
                I0 = 10000;
                Mp = (25 / Math.Abs(Stp));
                if (Mp < 5)
                    Mp = 5;
                if (mx)
                {
                    for (int j1 = MaxC - (MaxC / 4); j1 >= Mp; j1--)
                    {
                        A0 = 0;
                        A1 = 0;
                        B0 = 0;

                        AB = 0;
                        V1 = (IE + Stp * j1) / 1000;
                        for (int i = j1; i >= (j1 - Mp + 1); i--)
                        {
                            V1 = V1 - Stp / 1000;
                            A0 = A0 + V1;
                            A1 = A1 + V1 * V1;
                            B0 = B0 + othertech_val3[i];
                            AB = AB + V1 * othertech_val3[i];
                        }
                        AB = Mp * AB - A0 * B0;
                        A2 = Mp * A1 - A0 * A0;
                        b = AB / A2;
                        ai = (B0 - b * A0) / Mp;

                        if (Math.Abs(b) < I0)
                        {
                            I0 = Math.Abs(b);
                            I1 = b;
                            I_tot = ai;
                            k = j;//-Mp;
                        }
                    }
                    AB = I1 * (IE + MaxC * Stp) / 1000 + I_tot;
                }
            }
        }
        public void smootht9(int row1, Color color1)
        {
            //byte[] data = new byte[65000];
            //int[] Powr = new int[65000];
            //int[] Crnt = new int[65000];
            //double[] C = new double[65000];
            // Color[] Colr = new Color[100];

            double Stp = 0;
            double IE = 0, I0, I1 = 0, Ec, V1, SS, Vi = 0, Tstp;
            int Ts, k, k1, ser = 0, ngtv = 0, R_cmp = 0;
            long len, Fore = 0, Rev = 0, j = 0, NUM, vn = 0;
            bool ctrl = false, Drvt = false, grftfl = false;
            int L_Teq; //,a;

            len = shomar_othertech;
            L_Teq = int.Parse(dschart1.chartlist1_run.Rows[row1][2].ToString());
            //if (double.Parse(dschart1.chartlist1_run.Rows[row1][6].ToString()) > 0)
            //    Vry = true;
            IE = double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()) * 1000;
            Fore = (len / 2) + 1;
            Rev = (len / 1) + 1;
            j = len;
            k = 0;
            NUM = j - 1;
            Tstp = double.Parse(dschart1.chartlist1_run.Rows[row1][15].ToString());
            Ts = (int)Tstp;
            SS = 0.02;
            if (Tstp > 100)
                SS = (Tstp / 100) * SS;

            vn = NUM;

            for (long i = 1; i <= vn - 1; i++)
            {
                if (Math.Abs(othertech_val3[i] - othertech_val3[i - 1]) > 1000)
                    othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }

            for (long i = vn; i >= 2; i--)
            {
                othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i]) / 2;
            }

            for (long i = 1; i <= vn - 2; i++)
            {
                othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }
            for (int j1 = 3; j1 >= 0; j1--)
            {
                for (int i = 4; i <= vn - 3; i++)
                {
                    I0 = ((othertech_val3[i - 1] - othertech_val3[i - 2]) + (othertech_val3[i - 2] - othertech_val3[i - 3]) + (othertech_val3[i - 3] - othertech_val3[i - 4])) / 3;
                    I1 = Math.Abs(othertech_val3[i] - othertech_val3[i - 1]);
                    if (I1 > Math.Abs(j1 * I0))
                        othertech_val3[i] = (othertech_val3[i - 2] + othertech_val3[i - 1] + othertech_val3[i + 2] + othertech_val3[i + 1]) / 4;

                }
            }
            for (int i = 3; i <= vn - 4; i++)
            {
                othertech_val3[i] = (othertech_val3[i - 3] + othertech_val3[i - 2] + othertech_val3[i - 1] + othertech_val3[i + 3] + othertech_val3[i + 2] + othertech_val3[i + 1]) / 6;
            }



            ///**************************************************/
            if (Drvt)
            {
                othertech_val3[10000] = 0;
                k = 10001;
                for (int j1 = 1; j1 <= NUM; j1++)
                {
                    othertech_val3[k] = (othertech_val3[j1] - othertech_val3[j1 - 1]) / SS;
                    k = k + 1;
                }
            }
            else
                Drvt = false;
            //{/**************************************************/

            len = NUM;
            ser = 0;
            for (int la = OnlineChart.Series.Count - 1; la >= 0; la--)
                Setgrftchart1(false, la, 3, 1);// OnlineChart.Series[la].Points.Clear();
            if (ctrl)
            {

                for (long i = len; i >= 1; i--)
                {
                    V1 = 1 / Math.Sqrt(SS * i);
                    I1 = othertech_val3[i];
                    OnlineChart.Series[ser].Points.AddXY(V1, I1, "", color1);
                }
            }
            else
            {
                k = 0;
                k1 = 0;
                I1 = othertech_val3[0];//$$$$
                if (Drvt)
                {
                    k1 = 10000;
                    I1 = othertech_val3[k1];
                }

                //  I_tot = 0;
                for (int i = k; i <= len - 2; i++)
                {
                    if (Drvt)
                        I1 = othertech_val3[k + k1];
                    else
                        I1 = othertech_val3[k];
                    V1 = SS * (i + 1);
                    if ((I1 == 0) || (othertech_val3[k] < 0))
                        Ec = V1;
                    V1 = V1 - (ngtv * R_cmp * I1) / 1000000;

                    if (grftfl)
                    {
                        if (I1 != 0)
                        {
                            OnlineChart.Series[ser].Points.AddXY(V1, Math.Log10(Math.Abs(I1)) - 6);
                        }
                    }
                    else
                    {
                        Setgrftchart12(ser, V1, I1, 1, 1);// OnlineChart.Series[ser].Points.AddXY(V1, I1);                              
                    }

                    Vi = Vi + Stp;
                    k = k + 1;
                }
            }
        }
        private bool statehc = false;
        private string bpausetxt = "";
        private void hToolStripMenuItem_Click(object sender, EventArgs e)// دکمه hold   cont
        {
            if (statehc)//for again run -> countiue
            {
                serialPort2.Write("run");
                Settxtfill_func(2, $"run again", 2, 1);
                outc = 0x20;
                statehc = false;
                mainf.TechStatus.ForeColor = Color.Green;
                mainf.TechStatus.Text = bpausetxt;
                this.tsbhold.Enabled = true;
                this.tsbcont.Enabled = false;
                this.tsbstop.Enabled = true;
            }
            else
            {// for pause
                outc = 0x60;
                serialPort2.Write("puase");
                statehc = true;
                bpausetxt = mainf.TechStatus.Text;
                mainf.TechStatus.ForeColor = Color.Brown;
                mainf.TechStatus.Text = bpausetxt + "- Pause";
                this.tsbhold.Enabled = false;
                this.tsbstop.Enabled = false;
                this.tsbcont.Enabled = true;
            }
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)//stop
        {
            outc = 0xA0;
            counterforstop = 1;
        }

        private void tChart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var results11 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                foreach (var result in results11)
                {
                    if (result.ChartElementType == ChartElementType.PlottingArea)
                    {
                        switch (statebutton)
                        {
                            case "selectview":
                                OnlineChart.Cursor = System.Windows.Forms.Cursors.Cross;

                                break;
                            case "selectonly":
                                OnlineChart.Cursor = System.Windows.Forms.Cursors.Cross;
                                break;
                            case "showvalue":
                                OnlineChart.Cursor = System.Windows.Forms.Cursors.SizeAll;

                                break;
                            case "zoomin":

                                OnlineChart.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.zoomin);

                                break;
                            case "zoomout":
                                OnlineChart.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.Zoomout);

                                break;
                            default:
                                OnlineChart.Cursor = System.Windows.Forms.Cursors.Default;
                                break;
                        }
                    }
                    else
                        OnlineChart.Cursor = System.Windows.Forms.Cursors.Default;
                }
                switch (statebutton)
                {

                    case "selectonly":
                        Point mousePoint = new Point(e.X, e.Y);
                        OnlineChart.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, false);
                        OnlineChart.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, false);

                        break;
                    case "showvalue":
                        var results12 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
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
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtcomment, 2); }
        }
        private void showdata(int row)
        {
            if (this.InvokeRequired)
            {
                showDataon d = new showDataon(showdata);
                this.Invoke(d, new object[] { row });
            }
            else
            {
                byte Powr = 0;
                double C = 0, Crnt = 0;
                bool signe = false;

                for (int i = 0; i < datas2_state.Length; i++)
                {
                    if (datas2_state[i] == 0)
                    {
                        ///////////////////////////////
                        // if (outc = 0x60) H_ld = true;
                        if (fst)
                        {

                            Powr = datas2_2[i];
                            Crnt = ((datas2_0[i] << 6) | (datas2_1[i] >> 2));
                            if (Crnt > 0x1FFF)
                            {
                                Crnt = (0x4000 - Crnt);
                                signe = true;
                            }
                            else
                                signe = false;
                           
                            //if (Range1 != 0)
                            //    if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8) && (Crnt > 6560))
                            //        Crnt = 6560;
                            //if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9) && (Powr == 8))
                            //    Crnt = Crnt * 2; //current upto 1A
                        }
                        if (S_et)
                        {
                            if (Powr == 8)
                                C = Crnt * 1.02207 * Math.Pow(10, Powr);/////// - 6
                            else
                                C = Crnt * 0.61035 * Math.Pow(10, Powr - 6); ////- 6
                            if (signe ^ (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10))
                                C = C * -1;

                            if (shomar_cha_chc >= time_cha_chc.Length)
                            {
                                Array.Resize(ref time_cha_chc, time_cha_chc.Length + 200);
                                Array.Resize(ref volt_cha_chc, volt_cha_chc.Length + 200);
                            }
                            time_cha_chc[shomar_cha_chc] = Math.Round((i + 1) * ti, 3, MidpointRounding.AwayFromZero);
                         
                            //if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                            //{
                            //    time_cha_chc[shomar_cha_chc] /= 50;
                            //}
                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)//CHC
                                if (shomar_cha_chc == 0)
                                    volt_cha_chc[shomar_cha_chc] = C * ti;
                                else
                                    volt_cha_chc[shomar_cha_chc] = volt_cha_chc[shomar_cha_chc - 1] + (C * ti);
                            else
                                volt_cha_chc[shomar_cha_chc] = C;

                            //this.OnlineChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(dschart1.chartlist1_run.Rows[row][3]) - 1;
                            //this.OnlineChart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(dschart1.chartlist1_run.Rows[row][4]) + 1;

                            //رسم برای سه تکنیک  chp cha chc
                            if (shomar_cha_chc == 0 && int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 12)//برای رسم در نقطه صفر زمان غیر از تکنیک سی اچ سیmn
                                this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc] - 1, volt_cha_chc[shomar_cha_chc]);
                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)
                            {
                                time_cha_chc[shomar_cha_chc] /= 50;
                            }
                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)
                            {
                                time_cha_chc[shomar_cha_chc] /= 10;
                            }
                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 14)
                            {
                                time_cha_chc[shomar_cha_chc] /= 10;
                            }

                            if (shomar_cha_chc == 0 && int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)//برای رسم در نقطه صفر زمان در  تکنیک سی اچ سیmn
                            {
                                this.OnlineChart.Series[0].Points.AddXY(0, volt_cha_chc[shomar_cha_chc]);
                                shomar_cha_chc++;
                                continue;
                            }

                            datas2_state[i] = 1;

                            if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 12)//mn
                                this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);
                            else//mn
                                this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);//CHC

                            shomar_cha_chc++;

                        } //if S_et
                    }
                }
            }
        }

        private void btnshowvalue_Click(object sender, EventArgs e)
        {
            if (statebutton == "showvalue")
                statebutton = "";
            else
                statebutton = "showvalue";
            selectpchart_func(false);
        }

        private void selectpchart_func(bool state)
        {

            OnlineChart.ChartAreas[0].CursorX.IsUserEnabled = state;
            OnlineChart.ChartAreas[0].CursorY.IsUserEnabled = state;
            OnlineChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = state;
            OnlineChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = state;
            OnlineChart.ChartAreas[0].CursorX.Interval = 0;
            OnlineChart.ChartAreas[0].CursorY.Interval = 0;
            if (state)
            {
                OnlineChart.ChartAreas[0].CursorX.LineWidth = 1;
                OnlineChart.ChartAreas[0].CursorY.LineWidth = 1;
            }
            else
            {
                OnlineChart.ChartAreas[0].CursorX.LineWidth = 0;
                OnlineChart.ChartAreas[0].CursorY.LineWidth = 0;
            }
        }

        private void btnselectonly_Click(object sender, EventArgs e)
        {
            if (statebutton == "selectonly")
            {
                statebutton = "";
                selectpchart_func(false);
                btnselectonly.FlatAppearance.BorderSize = 0;
            }
            else
            {
                statebutton = "selectonly";
                selectpchart_func(true);
                btnselectonly.FlatAppearance.BorderSize = 1;
                btnselectonly.FlatAppearance.BorderColor = Color.Black;
            }
        }


        private void btnzoomin_Click(object sender, EventArgs e)
        {
            if (statebutton == "zoomin")
                statebutton = "";
            else
                statebutton = "zoomin";
            selectpchart_func(false);
        }

        private void btnzoomout_Click(object sender, EventArgs e)
        {
            if (statebutton == "zoomout")
                statebutton = "";
            else
                statebutton = "zoomout";
            selectpchart_func(false);
        }
        private void btnshowgrid_Click(object sender, EventArgs e)
        {
            statebutton = "";
            selectpchart_func(false);
            this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled = !this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled;
            this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled = !this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled;
        }
        private void btnshowaxes_Click(object sender, EventArgs e)
        {
            statebutton = "";
            selectpchart_func(false);
            OnlineChart.ChartAreas[0].AxisX.LabelStyle.Enabled = !OnlineChart.ChartAreas[0].AxisX.LabelStyle.Enabled;
            OnlineChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            OnlineChart.ChartAreas[0].AxisY.LabelStyle.Enabled = !OnlineChart.ChartAreas[0].AxisY.LabelStyle.Enabled;
            OnlineChart.ChartAreas[0].AxisY.MajorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisY.MajorTickMark.Enabled;
        }
        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (statebutton)
            {
                case "zoomin":
                    var results11 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results11)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            this.OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            double xMin = OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                            double xMax = OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                            double yMin = OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                            double yMax = OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMaximum;
                            double posXStart = (OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - xMin) / 1.1;
                            double posXFinish = (OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + xMax) / 1.1;
                            double posYStart = (OnlineChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - yMin) / 1.1;
                            double posYFinish = (OnlineChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + yMax) / 1.1;

                            OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - posXStart, OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + posXFinish, DateTimeIntervalType.Auto, true);
                            OnlineChart.ChartAreas[0].AxisY.ScaleView.Zoom(OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) - posYStart, OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) + posYFinish, DateTimeIntervalType.Auto, true);

                            //OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(xMin, 0.95 * xMax, DateTimeIntervalType.Auto, true);
                            //OnlineChart.ChartAreas[0].AxisY.ScaleView.Zoom(yMin, 0.95 * yMax, DateTimeIntervalType.Auto, true);
                        }
                    }
                    break;
                case "zoomout":
                    var results12 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results12)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            this.OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                            OnlineChart.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);

                        }
                    }
                    break;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)//show data
        {
            forms2065.frmshowdata frm = new forms2065.frmshowdata();
            frm.listView1.Items.Clear();
            for (int i = 0; i < OnlineChart.Series[0].Points.Count; i++)
            {
                frm.listView1.Items.Add(OnlineChart.Series[0].Points[i].XValue + "          " + OnlineChart.Series[0].Points[i].YValues[0]);
            }
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void tsbsave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (num_cycles < 0)
            //    {
            //        MessageBox.Show("data not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    if (double.Parse(dschart1.chartlist1_run.Rows[num_cycles][1].ToString()) > 9 && double.Parse(dschart1.chartlist1_run.Rows[num_cycles][1].ToString()) < 13)
            //    {// در تکنیک 10 و 11 و12 آرایه های ولتاژ و جریان تایم و ولت است
            //        if (shomar_cha_chc == 0)
            //        {
            //            MessageBox.Show("data not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //        SaveFileDialog sfd = new SaveFileDialog();
            //        sfd.AddExtension = true;
            //        sfd.DefaultExt = "txt";
            //        sfd.Filter = "Text document(*.txt)|*.txt";
            //        if (sfd.ShowDialog() == DialogResult.OK)
            //        {
            //            string ext = sfd.FileName.Substring(sfd.FileName.Length - 3, 3);

            //            if (ext == "txt")
            //            {
            //                StreamWriter Fl = new StreamWriter(sfd.FileName, false, Encoding.ASCII);

            //                Fl.WriteLine("---------------------------------------------------");
            //                Fl.WriteLine("row number      X value         Y value");
            //                Fl.WriteLine("---------------------------------------------------");

            //                for (int cc1 = 0; cc1 < shomar_cha_chc; cc1++)
            //                {
            //                    Fl.WriteLine((cc1 + 1).ToString().Trim() + "\t\t" + time_cha_chc[cc1].ToString().Trim() + "\t\t" + volt_cha_chc[cc1].ToString().Trim());
            //                }
            //                Fl.Close();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (shomar_othertech == 0)
            //        {
            //            MessageBox.Show("data not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }

            //        SaveFileDialog sfd1 = new SaveFileDialog();
            //        sfd1.AddExtension = true;
            //        sfd1.DefaultExt = "txt";
            //        sfd1.Filter = "Text document(*.txt)|*.txt";
            //        if (sfd1.ShowDialog() == DialogResult.OK)
            //        {
            //            string name = sfd1.FileName.Substring(0, sfd1.FileName.Length - 4);
            //            string ext = sfd1.FileName.Substring(sfd1.FileName.Length - 3, 3);
            //            string file_name = name /*+ "_" + (k + 1).ToString()*/ + "." + ext;

            //            if (ext == "txt")
            //            {
            //                //int Counter_ser = OnlineChart.Series.Count;

            //                //for (int ser = 0; ser < Counter_ser; ser++)
            //                //    for (int i = 0; i < Counter_num; i++)
            //                //    {
            //                //        othertech_val0[i] = OnlineChart.Series[ser].Points[i].XValue;
            //                //        current[i] = OnlineChart.Series[ser].Points[i].YValues[0];
            //                //    }

            //                StreamWriter Fl = new StreamWriter(file_name, false, Encoding.ASCII);
            //                Fl.WriteLine("---------------------------------------------------");
            //                Fl.WriteLine("row number      X value         Y value");
            //                Fl.WriteLine("---------------------------------------------------");

            //                for (int cc1 = 0; cc1 < shomar_othertech; cc1++)
            //                {
            //                    Fl.WriteLine((cc1 + 1).ToString().Trim() + "\t\t" + othertech_val0[cc1].ToString().Trim() + "\t\t" + othertech_val1[cc1].ToString().Trim());
            //                }
            //                Fl.Close();
            //            }
            //        }
            //    }
            //    clasglobal.AppendText(txtcomment,"save successfuly"+Environment.NewLine,Color.Green);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        //private byte[] HexstringToByteArray(string hex)
        //{
        //    int hexstringlength = hex.Length;
        //    byte[] b = new byte[hexstringlength / 2];
        //    for (int i = 0; i < hexstringlength; i += 2)
        //    {
        //        int topChar = (hex[i] > 0x40 ? hex[i] - 0x37 : hex[i] - 0x30) << 4;
        //        int bottomChar = hex[i + 1] > 0x40 ? hex[i + 1] - 0x37 : hex[i + 1] - 0x30;
        //        b[i / 2] = Convert.ToByte(topChar + bottomChar);
        //    }
        //    return b;
        //}
        //private string inttoHex(int Value_int16)
        //{
        //    string Value_String;
        //    Value_String = Value_int16.ToString("X8");
        //    return Value_String;
        //}
        //private string DoubletoHex(Double Value_Double)
        //{
        //    long Value_Long = 0;
        //    string Value_String;
        //    Value_Long = BitConverter.DoubleToInt64Bits(Value_Double);
        //    Value_String = Value_Long.ToString("X8");
        //    return Value_String;
        //}
        unsafe private void dorun3(int row)
        {
            double V = 0, I1 = 0, I2 = 0;
            Int64 j = 0;
            bool evn = true;
            Boolean slw, strt = true, E_nd = false, back_direction = false;
            byte num;
            byte* OutBuff = &num;
            byte* InBuff = &num;
            Int32 m2;
            int i = 0;
            byte[] datas = new byte[3];
            byte[] Powr = new Byte[32710];
            double[] Crnt = new double[32710];
            double[] C = new double[32710];
            Boolean[] signe = new Boolean[32710];
            int counter = 0, number = 0, sign = 0, end = 0;
            String string1;

            serialPort2.DiscardInBuffer();
            //serialPort2.ReadTimeout = 10000;
            serialPort2.ReadTimeout = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + 5000 + (clasglobal.cpctprms.CT1 * 1000) + (clasglobal.cpctprms.CT2 * 1000) + (clasglobal.cpctprms.CT3 * 1000));
            OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);

            if (((Analiz_St1 > 1) && (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 > 5) && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 3)) || ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 3) && (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 > 20)))
                slw = true;
            else
                slw = false;
            //updt = false;
            strt = true;
            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8)
                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
            else if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) >= 9)
                V = ti;
            while (true)
            {
                Settxtfill_func(2, "dorun3", 2, 1);
                if (staterun.Trim() == "stop")
                {
                    runerror = true;
                    staterun = "";
                    break;
                }
                string1 = null;
                string[] strings = new string[4];
                char char1 = '0';
                Setbtnenabled(true, 1, 2);
                // serialPort2.WriteLine("start");
                number = serialPort2.BytesToRead;
                if (number >= 0)
                {

                    try
                    {

                        string1 = serialPort2.ReadLine();
                        //string1.Remove(string1.Length-1);
                        if (sign == 0)
                        {
                            if (string1.Contains("end"))
                            {
                                end = 1;
                                string1 = "";
                                Settxtfill_func(2, "end", 2, 1);
                            }
                            else
                            {
                                strings = string1.Split('@');
                                //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                                datas[2] = (byte)Int32.Parse(strings[1]);
                                datas[0] = (byte)Int32.Parse(strings[2]);
                                datas[1] = (byte)Int32.Parse(strings[3]);
                                //counter = (int)Int32.Parse(strings[4]);
                                Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                                Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                                Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                                Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                                counter++;
                                sign = 1;
                                number = 0;
                                flag_running = true;
                            }

                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 0)
                            //{

                            //    if (string1.Contains("end0"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end0", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 1)
                            //{
                            //    if (string1.Contains("end1"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end1", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 2)
                            //{

                            //    if (string1.Contains("end2"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end2", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 3)
                            //{
                            //    if (string1.Contains("end3"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end3", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)
                            //{

                            //    if (string1.Contains("end4"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end4", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 5)
                            //{

                            //    if (string1.Contains("end5"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end5", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }

                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 6)
                            //{
                            //    if (string1.Contains("end6"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end6", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 7)
                            //{
                            //    if (string1.Contains("end7"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end7", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                            //if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 9)
                            //{
                            //    if (string1.Contains("end9"))
                            //    {
                            //        end = 1;
                            //        string1 = "";
                            //        Settxtfill_func(2, "end9", 2, 1);
                            //    }
                            //    else
                            //    {
                            //        strings = string1.Split('@');
                            //        //Settxtfill_func(2, $"endpacket:{string1}", 2, 1);
                            //        datas[2] = (byte)Int32.Parse(strings[1]);
                            //        datas[0] = (byte)Int32.Parse(strings[2]);
                            //        datas[1] = (byte)Int32.Parse(strings[3]);
                            //        //counter = (int)Int32.Parse(strings[4]);
                            //        Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);
                            //        Settxtfill_func(2, $"counter:{counter},i{i}", 2, 1);
                            //        counter++;
                            //        sign = 1;
                            //        number = 0;
                            //        flag_running = true;
                            //    }


                            //}
                        }
                        //serialPort2.Write("next");
                        //serialPort2.DiscardInBuffer();
                    }
                    catch (TimeoutException)
                    {
                        if (outc != 0x60)
                        {
                            serialPort2.Write("stop");
                            serialPort2.Close();
                            Settxtfill_func(2, "Time out eror happen", 2, 1);
                            return;
                        }
                    }



                }
                if (sign == 1)
                {
                    sign = 0;
                    Powr[j] = datas[2];
                    Crnt[j] = ((datas[0] << 6) | (datas[1] >> 2));
                    if (Crnt[j] > 0x1FFF)
                    {
                        Crnt[j] = (0x4000 - Crnt[j]);
                        signe[j] = true;
                    }
                    else signe[j] = false;

                    C[j] = Math.Round(Crnt[j] * 0.61035 * Math.Pow(10, Powr[j] - 6), 2);//111111111111111111111111111111111111111111111111111111
                    if (signe[j])
                        C[j] = C[j] * -1;
                    if (pulsy)
                    {
                        if (!evn)
                        {
                            if (ngtv)
                            {
                                I1 = C[j] - C[j - 1];
                                I2 = 0;
                            }
                            else
                            {
                                I1 = C[j - 1] - C[j];
                                I2 = 0;
                            }

                            Runnig2(V, I1, I2, row);
                        }
                    }
                    else if (strt)
                    {
                        I1 = C[j];
                        I2 = 0;
                        strt = false;

                        Runnig2(V, I1, I2, row);
                        //Settxtfill_func(2, $"dorun3{V}", 2, 1);
                    }
                    else
                    {
                        I1 = (C[j] + C[j - 1]) / 2;
                        I2 = 0;
                        Runnig2(V, I1, I2, row);
                        // Settxtfill_func(2, $"dorun3{V}", 2, 1);
                    }

                    if (!(pulsy && evn))
                    {
                        if (N_gtv) V = V - double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());
                        else V = V + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());
                    }
                    j++;
                    evn = !evn;
                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 4)
                    {
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) < double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                        {
                            if (V >= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                            {
                                back_direction = true;
                                N_gtv = !N_gtv;
                                fst = fst2;
                                if (fst)
                                    S_et = true;
                                else
                                    S_et = false;
                            }
                            if (V < Elo && back_direction)
                            {
                                dschart1.chartlist1_run.Rows[row][13] = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) - 1;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) >= 0) //
                                {
                                    E_nd = true;
                                    countcv++;

                                    if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)
                                        number_runing_cv++;
                                }
                            }
                        }
                        else
                        {
                            if (V <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                            {
                                back_direction = true;
                                N_gtv = !N_gtv;
                                fst = fst2;
                                if (fst)
                                    S_et = true;
                                else
                                    S_et = false;
                            }
                            if (V > Elo && back_direction)
                            {
                                dschart1.chartlist1_run.Rows[row][13] = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) - 1;
                                if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) >= 0) //E_nd:=true
                                {
                                    E_nd = true;
                                    countcv++;

                                    if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)
                                        number_runing_cv++;
                                }
                            }
                        }
                    }
                    if (E_nd)
                    {
                        transferdata(row, 1);
                        double temp1, temp2, temp3, temp4;
                        temp1 = othertech_val0[shomar_othertech - 1];
                        temp2 = othertech_val1[shomar_othertech - 1];
                        temp3 = othertech_val2[shomar_othertech - 1];
                        temp4 = othertech_val3[shomar_othertech - 1];
                        smootht4(row, OnlineChart.Series[0].Color);
                        Setgrfnodetext(row, 2, 1);
                        show_cycle_runing_CV(number_runing_cv);//show number running in toolStripStatusLabel5
                        if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) > 0)
                        {
                            shomar_othertech = 1;
                            Array.Clear(othertech_val0, 1, othertech_val0.Length - 1);
                            Array.Clear(othertech_val1, 1, othertech_val1.Length - 1);
                            Array.Clear(othertech_val2, 1, othertech_val2.Length - 1);
                            Array.Clear(othertech_val3, 1, othertech_val3.Length - 1);
                            othertech_val0[0] = temp1;
                            othertech_val1[0] = temp2;
                            othertech_val2[0] = temp3;
                            othertech_val3[0] = temp4;
                            Setgrfnodetext(row, 4, 1);
                            show_cycle_runing_CV(number_runing_cv);//show number running in toolStripStatusLabel5
                        }
                        else if (int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) == 0)
                            countcv = 0;
                        if (fst)
                            S_et = true;
                        else
                            S_et = false;

                        N_gtv = !N_gtv;
                        if (N_gtv)
                        {
                            V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) - double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());
                        }
                        else
                        {
                            V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString());
                        }
                        E_nd = false;
                        back_direction = false;

                    }

                }
                else if (end == 1)  //if portc=0x09
                {



                    serialPort2.Close();




                    //////////////////////////////////
                    if (row < this.dschart1.chartlist1_run.Rows.Count && num_cycles >= cycles)//(rbSequence.Checked &&
                    {

                        //Seq_Run_Num++;
                        //    isOverlay = false;
                        //////////////////
                        while (this.OnlineChart.Series.Count > 0)
                        {
                            this.OnlineChart.Series.Remove(this.OnlineChart.Series[0]);
                            //      is_smoothed[this.OnlineChart.Series.Count] = false;
                        }
                        Setcheckeditem(true, 1, 1);
                        Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["Dly"].ToString())); // i dont know valu field is null and get error
                        Thread.Sleep(3000); //test
                                            // isSet_Grid_Cells = true;
                        detect_Error = false;
                        //  showMessageError = true;
                        OnlineChart.Series[num_Series].Label = this.dschart1.chartlist1_run.Rows[row]["Tch"].ToString();

                        Setgrfnodeselect(row, 1, 1);

                        try
                        {
                            //  tech = Convert.ToByte(this.dschart1.chartlist1_run.Rows[row][1].ToString());
                            //SetParamsFromDs(row);
                        }
                        catch { }
                        //if (!cbOverlay.Checked)
                        //{
                        //    cbOverlay.Checked = (cycles > 1);
                        //    //overlayToolStripMenuItem.Checked = (cycles > 1);
                        //}
                        long k = 0;
                        while (k < 10) k++;
                        num_cycles = 1;   // (int)cycles;
                        System.Threading.Thread.Sleep(int.Parse(this.dschart1.chartlist1_run.Rows[row][7].ToString()));
                        //EquilibriumTime = 0;
                        i = 0;
                        // goto test;

                        return;
                    }
                    else
                    {
                        Setbtnenabled(true, 2, 1);
                        return;
                        ////////////////////////////
                        // break;
                    }
                }
                // other input portc 

                //if (outc == 0x60)
                //    thread_grf.Suspend();
                if (outc == 0xA0)//if software ==pause 
                {

                    //Settxtfill_func(1, $"goodbay{oinp}", 2, 1);

                    serialPort2.Write("stop");
                    System.Threading.Thread.Sleep(100);
                    serialPort2.Close();
                    detect_Error = true;
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, "finish running-2", 2, 1);

                    return;

                }
                //else
                //{
                //    //-----get error sleep
                //    if (usb_fun.Inprt(P_C) == 0x09)
                //    {
                //        usb_fun.Outprt(P_C, 0x80);
                //        while (usb_fun.Inprt(P_C) == 0x89) ;
                //        usb_fun.Outprt(P_C, 0x00);
                //        usb_fun.ClosePipes();
                //        //////////////////////////////////
                //        if (row < this.dschart1.chartlist1_run.Rows.Count && num_cycles >= cycles)//(rbSequence.Checked &&
                //        {

                //            //Seq_Run_Num++;
                //            //    isOverlay = false;
                //            //////////////////
                //            while (this.OnlineChart.Series.Count > 0)
                //            {
                //                this.OnlineChart.Series.Remove(this.OnlineChart.Series[0]);
                //                //      is_smoothed[this.OnlineChart.Series.Count] = false;
                //            }
                //            Setcheckeditem(true, 1, 1);
                //            Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["Dly"].ToString())); // i dont know valu field is null and get error
                //            Thread.Sleep(3000); //test
                //                                // isSet_Grid_Cells = true;
                //            detect_Error = false;
                //            //  showMessageError = true;
                //            OnlineChart.Series[num_Series].Label = this.dschart1.chartlist1_run.Rows[row]["Tch"].ToString();

                //            Setgrfnodeselect(row, 1, 1);

                //            try
                //            {
                //                //  tech = Convert.ToByte(this.dschart1.chartlist1_run.Rows[row][1].ToString());
                //                //SetParamsFromDs(row);
                //            }
                //            catch { }
                //            //if (!cbOverlay.Checked)
                //            //{
                //            //    cbOverlay.Checked = (cycles > 1);
                //            //    //overlayToolStripMenuItem.Checked = (cycles > 1);
                //            //}
                //            long i = 0;
                //            while (i < 10) i++;
                //            num_cycles = 1;   // (int)cycles;
                //            System.Threading.Thread.Sleep(int.Parse(this.dschart1.chartlist1_run.Rows[row][7].ToString()));
                //            //EquilibriumTime = 0;
                //            i = 0;
                //            // goto test;

                //            return;
                //        }
                //        else
                //        {
                //            Setbtnenabled(true, 2, 1);
                //            return;
                //            ////////////////////////////
                //            // break;
                //        }

                //    }
                //    else
                //    {
                //        //if (outc == 0x60)
                //        //    thread_grf.Suspend();
                //        if (outc == 0xA0)
                //        {
                //            if (oinp == 0x00)
                //                counterforstop++;
                //            if (counterforstop > 5)
                //            {
                //                usb_fun.Outprt(P_C, 0x80);
                //                while (usb_fun.Inprt(P_C) == 0x89) ;
                //                //usb_fun.Outprt(P_C, 0x00);
                //                usb_fun.ClosePipes();
                //                detect_Error = true;
                //                Setbtnenabled(true, 2, 1);
                //                Settxtfill_func(1, "finish running-2", 2, 1);

                //                return;
                //            }
                //        }
                //    }
                //}

                Chart2 = this.OnlineChart;

            } //while true              
        }

    }
}