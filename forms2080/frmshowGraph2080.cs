using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using Skydat.classes2065;


namespace Skydat.forms2080
{
    public partial class frmshowGraph2080 : Form
    {
        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadCursorFromFile(String str);

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
        public frmshowGraph2080()
        {
            InitializeComponent();
          
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
            tChart1.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;
            tChart1.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Black;
            this.tChart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
          
            this.treeView3.ExpandAll();
           
            thread_grf = new Thread(Run_key);

        }

        bool detect_Error = false, is_set_graph = false, showMessageError = false;
        private double Prior_I2 = 0, Analiz_CT1 = 0, Analiz_CT2 = 0, Analiz_CT3 = 0, Analiz_CP1 = 0, Analiz_CP2 = 0, Analiz_CP3 = 0,Analiz_CP4 = 0,Analiz_CP5 = 0,Analiz_CT4=0,Analiz_CT5 = 0;
        byte Teq = 1, Run = 2, E1 = 3, E2 = 4, E3 = 5, CP1 = 40, CP2 = 7, CP3 = 8, CP4 = 9, CP5 = 10;
        byte CT1 = 11, CT2 = 12, CT3 = 13, CT4 = 14, CT5 = 15, HS = 16, TS = 17, SR = 18, QT = 19, CY = 20;
        byte frequency = 22, PulsWidth = 24, PulsHeight = 25, Norma_Seq = 26, Multirun_Delay = 27;
        byte Code_Check_Lock = 33, vs_OCP = 34, OCP_Meas = 35, Soft_Lock = 36, Current_Range = 37;
        int Num_OverFlow_CPC_CA = 0,number = 0;
        private byte Range1;
        private string statebutton = "";
        private Thread thread_grf; 
        public delegate void showOnlineData1(double v, double i1, double i2, int row);     
      
        public bool OCP_Ok = false;
        private bool flag_running = false;
        public int OCP_Timer = 0;
        public drawline_chart drawline1;
        public frmMain12080 mainf;
        public frmOption12080 opForm;

        double[] othertech_val0 = new double[200];
        double[] othertech_val1 = new double[200];
        double[] othertech_val2 = new double[200];
        double[] othertech_val3 = new double[200];

       // int shomar_cha_chc = 0;
        long shomar_othertech = 0;
        KeyEventArgs ee = new KeyEventArgs(Keys.Alt);

        private void GraphForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[4].ToString().Substring(color[4].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                tChart1.BackColor = cl;
                tChart1.BorderlineColor = cl;
                tChart1.ChartAreas[0].BackColor = cl;
                panel1.BackColor = cl;
                panel4.BackColor = cl;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

            try
            {
                sp1.PortName = Properties.Settings.Default.portname;
                sp1.BaudRate = int.Parse(Properties.Settings.Default.baudrate);
                sp1.ReadTimeout = int.Parse(Properties.Settings.Default.rtport);
                sp1.WriteTimeout = int.Parse(Properties.Settings.Default.wtport);
                if (!sp1.IsOpen)
                    sp1.Open();

                drawline1 = new drawline_chart();
                if (opForm.cbDrawLine.Checked)
                    Set_line_Setting();
                else
                    drawline1.Active = false;
            }
            catch 
            {
                try
                {
                    if (!sp1.IsOpen)
                        sp1.Open();
                }
                catch { }
            }
           
                      
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

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (mainf.grfNum == 0);
            if (mainf.grfNum > 0)
                mainf.grfNum--;

            if (sp1.IsOpen)
                sp1.Close();
        }    

    

        private void GraphForm_Enter(object sender, EventArgs e)
        {
            if (mainf != null)
                mainf.grf1 = (sender as frmshowGraph2080);
        }
            
      
        public void clearform()
        {
            for (int i = treeView3.Nodes[0].Nodes.Count - 1; i >= 0; i--)
                treeView3.Nodes[0].Nodes[i].Remove();
            label4.Text = "";
            for (int i = tChart1.Series.Count - 1; i >= 0; i--)
                tChart1.Series.RemoveAt(i);
            tChart1.Refresh();
            dschart1.chartlist1_run.Clear();
            dschart1.graphlist.Clear();
           
        }     
      
        delegate void SetfillparamCallback(int row, int state);

        private void fillparam(int row, int state)
        {
            if (state == 1)
            {
                SetfillparamCallback d = new SetfillparamCallback(fillparam);
                this.Invoke(d, new object[] { row, 2 });
            }
            else
            {
                this.treeView3.SelectedNode = this.treeView3.Nodes[0].Nodes[row];

                label4.Text = "";
                switch (byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()))
                {

                    case clasglobal.NPV2080:
                       
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                         label4.Text = label4.Text + "Pulse Width =" + double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        label4.Text = label4.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                        label4.Text = label4.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                      label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        break;
                    case clasglobal.DPV2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                         label4.Text = label4.Text + "Pulse Width =" + double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) + "\n";
                         label4.Text = label4.Text + "Pulse Height =" + double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                         label4.Text = label4.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        break;
                    case clasglobal.SWV2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                         label4.Text = label4.Text + "Pulse Height =" + double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                         label4.Text = label4.Text + "Frequency =" + double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        break;
                    case clasglobal.CV2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "E3 =" + double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) + "\n";
                         label4.Text = label4.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                        //if (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                         label4.Text = label4.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        break;
                    case clasglobal.LSV2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "Scan Rate =" + double.Parse(dschart1.chartlist1_run.Rows[row][8].ToString()) + "\n";
                         label4.Text = label4.Text + "TStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][9].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        break;

                    case clasglobal.CPC2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        // //Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CCC2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        // //Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CHP2080:
                         label4.Text = label4.Text + "I1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString()) + "\n";
                         label4.Text = label4.Text + "I2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()) + "\n";
                         label4.Text = label4.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                         label4.Text = label4.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        // Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    case clasglobal.CHA2080:
                         label4.Text = label4.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                         label4.Text = label4.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                         label4.Text = label4.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                         label4.Text = label4.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                         label4.Text = label4.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                         label4.Text = label4.Text + "vs_OCP =" +dschart1.chartlist1_run.Rows[row][19].ToString() + "\n";
                         label4.Text = label4.Text + "OCP Measurment =" + dschart1.chartlist1_run.Rows[row][20].ToString() + "\n";

                        // Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                        break;
                    //case clasglobal.CHC2080:
                    //     Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                    //     Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                    //     Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                    //     Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                    //     Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                    //    // Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
                    //    break;
                }
            }
            //pGrid.Refresh();
        }      
      
    
        public void Set_Graph(bool overlaymain,int row1)
        {


            Setgrftchart1(overlaymain, 0, 1, 1);

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

            Setgrftchart11(ser1, 1, 1);///////

            Setgrftchart1(overlaymain, row1, 2, 1);///
        }






        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled;
           // OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisX.MinorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled;
         //   OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled = !OnlineChart.ChartAreas[0].AxisY.MinorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled;

            this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.tChart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
           
        }
     
     
        private bool Read_Lock()
        {
            string Read_Lock1 = "3E8", Read_Lock2 = "BB8";
            int Write_Lock = 2000;
            //byte[] buf = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] buf = new byte[13];//mn
           string Lock_String_Num = "";
            if (sp1.IsOpen)
            {
                try
                {
                    sp1.DiscardInBuffer();
                    Set_Command(Soft_Lock, 1);//mn
                    Set_Command(38, 2);
                    System.Threading.Thread.Sleep(600);
                    if (sp1.BytesToRead > 0)
                        sp1.Read(buf, 0, 7);

                   
                    // for (int i = 0; i < result.Length; i++)
                    //    Lock_String_Num = Lock_String_Num + (char)buf[i];// +(char)buf[2] + (char)buf[3] + (char)buf[4];

                    for (int i = 0; i < buf.Length; i++)
                        Lock_String_Num = Lock_String_Num + (char)buf[i];// +(char)buf[2] + (char)buf[3] + (char)buf[4];

                    if (Lock_String_Num.IndexOf(Read_Lock1) >= 0)
                    {
                        sp1.DiscardInBuffer();
                        for (int k = 0; k < 13; k++) buf[k] = 0;
                        Set_Command(Code_Check_Lock, Write_Lock);
                        System.Threading.Thread.Sleep(600);
                        if (sp1.BytesToRead > 0)
                            sp1.Read(buf, 0, 7);
                        Lock_String_Num = "";
                        for (int i = 0; i < buf.Length; i++)
                            Lock_String_Num = Lock_String_Num + (char)buf[i]; // +(char)buf[2] + (char)buf[3] + (char)buf[4];
                    }
                }
                catch { }
            }
            if (Lock_String_Num.IndexOf(Read_Lock2) >= 0)
                return true;
            else
                return false;
        }
        public void smootht4(int row)
        {             
            double I0, I1 = 0;
            int vn = (int)shomar_othertech;

            for (int i = 1; i < vn - 1; i++)
            {
                if (Math.Abs(othertech_val3[i] - othertech_val3[i - 1]) > 1000)
                    othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }
            for (int i = vn - 1; i >= 1; i--)
            {
                othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i]) / 2;
            }
            for (int i = 1; i < vn - 1; i++)
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

            tecnic_row_for_label_text_ocp = row;//mn
            Setgrfnodetext(1, 5, 1);
        }

        int tecnic_row_for_label_text_ocp = 0;//mn

        int number_runing_teq = 0, show_number_runing = 0;///mehrdad 
        public int cycle_number_in_file = 1;//mn
        unsafe void Run_key()
        {
                //Read_Lock();//بررسی قفل در گذشته بود

                Setgrfnodetext(0, 3, 1);

                //   timer1.Start();

                show_number_runing = 0;

                for (int cnnnn = 0; cnnnn < dschart1.chartlist1_run.Rows.Count; cnnnn++)
                {
                    cycle_number_in_file = 1;

                    int cycle = Convert.ToInt32(dschart1.chartlist1_run.Rows[cnnnn]["CY"].ToString());
                    if (cycle == 0) cycle = 1;//برای تکنیک هایی که سیکل ندارند

                    remove_graph_last();//مخفی گرافهایی که با سیکل قبل ایجاد شدند

                    for (int i = 0; i < cycle; i++)
                    {
                        number_runing_teq++;

                        Set_Command(Teq, Convert.ToInt32(dschart1.chartlist1_run.Rows[cnnnn]["techno"].ToString()));
                        Set_Command(Run, 2);
                        Set_Command(Multirun_Delay, Convert.ToInt32("0" + dschart1.chartlist1_run.Rows[cnnnn]["QT"].ToString()));
                        Set_Command(Norma_Seq, 0);

                        Setgrfnodetext(cnnnn, 1, 1);

                        fillparam(treeView3.Nodes[0].Nodes[cnnnn].Index, 1);

                        Setbtnenabled(false, 1, 1);

                        Set_Graph(false, cnnnn);

                        Set_Command(Teq, Convert.ToInt32(dschart1.chartlist1_run.Rows[cnnnn]["techno"].ToString()));
                        Set_value(cnnnn);

                        if (detect_Error)
                        {
                            Setgrfnodetext(1, 4, 1);
                            //this.mainf.stf.runToolStripMenuItem1.Enabled = true;
                            //this.mainf.stf.tsbRun.Enabled = true;

                            return;
                        }
                        else
                            Set_Command(Run, 1);

                        shomar_othertech = 0;

                        Array.Clear(othertech_val0, 0, othertech_val0.Length);
                        Array.Clear(othertech_val1, 0, othertech_val1.Length);
                        Array.Clear(othertech_val2, 0, othertech_val2.Length);
                        Array.Clear(othertech_val3, 0, othertech_val3.Length);

                        doRun(cnnnn);//رسم گراف

                        if (shomar_othertech > 0)
                        {
                            transferdata(cnnnn);

                            smootht4(cnnnn);
                            // shomartime = 0;
                            Setgrfnodetext(cnnnn, 2, 1);
                        }
                    }

                    number_runing_teq = 0;//نمایش  تعداد سیکل
                    show_number_runing++;//نمایش تعداد تکنیک
                }

                Setbtnenabled(true, 2, 1);

                timer1.Stop();


                try { sp1.Close(); } catch { }
        }

        delegate void removegraphb();
        private void remove_graph_last()
        {
            if (tChart1.InvokeRequired)
            {
                removegraphb d = new removegraphb(remove_graph_last);
                this.Invoke(d, new object[] { });
            }
            else
            {
                for (int k = 0; k < tChart1.Series.Count; k++)
                    tChart1.Series[k].Enabled = false;
            }
        }
        private void transferdata(int rownum)
        {
            string strpath = "";// fName = "";
            for (long i = 100001; i < 1000000; i++)
            {
                if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile2080\\" + clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat"))
                {
                    if (cycle_number_in_file <= 1)// without cycle
                    {
                        strpath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile2080\\" + clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        //fName = clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        break;
                    }
                    else
                    {
                        strpath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile2080\\" + clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + (i - 1).ToString().Trim() + ".dat";
                        //fName = clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())].Trim() + i.ToString().Trim() + ".dat";
                        break;
                    }
                }
            }

            if (cycle_number_in_file <= 1)
                WriteToFile_2080(dschart1.chartlist1_run.Rows[rownum], clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())], strpath, rownum, 2);//header in file
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
                sa1.WriteLine("Cycle Number: " + cycle_number_in_file.ToString());
                sa1.WriteLine();
            }

            cycle_number_in_file++;

            string Ln = "";

            for (int i = 0; i < shomar_othertech; i++)
            {
                Ln = othertech_val0[i].ToString();
                Ln = Ln + "\t" + othertech_val3[i].ToString();
                Ln = Ln + "\t" + othertech_val1[i].ToString();
                Ln = Ln + "\t" + othertech_val2[i].ToString();
                sa1.WriteLine(Ln);
            }

            sa1.Close();
            dschart1.chartlist1_run.Rows[rownum][23] = strpath;

        }

        public static void WriteToFile_2080(DataRow dr, string tchName, string fileName, int row, int typedevice)
        {
            StreamWriter Fl = new StreamWriter(fileName, false, Encoding.ASCII);

            if (tchName != "APP" && tchName != "SEQ")
            {
                Fl.WriteLine("Settings for Parameters:");
                Fl.WriteLine("Date:" + DateTime.Now.ToString());

                if (tchName == "NPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: NPV");
                    //Fl.WriteLine("TchNo: " + NPV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("Cycles:" + dr[13].ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Width:" + dr[10].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "DPV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: DPV");
                    //Fl.WriteLine("TchNo: " + DPV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("Cycles:" + dr[13].ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Height:" + dr[11].ToString());
                    Fl.WriteLine("Pulse Width:" + dr[10].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "SWV" || tchName == "All")
                {

                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: SWV");
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("Cycles:" + dr[13].ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("Frequency:" + dr[12].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Pulse Height:" + dr[11].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "CV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CV");
                    //Fl.WriteLine("TchNo: " + CV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("Cycles:" + dr[13].ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("E3:" + dr[5].ToString());
                    Fl.WriteLine("Hold Time:0");
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("ScanRate_R:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "LSV" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: LSV");
                    //Fl.WriteLine("TchNo: " + LSV.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("Cycles:" + dr[13].ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("HStep:" + dr[6].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("Scan Rate:" + dr[8].ToString());
                    Fl.WriteLine("TStep:" + dr[9].ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }

                }

                if (tchName == "CPC" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CPC");
                    //Fl.WriteLine("TchNo: " + CPC.ToString());
                    Fl.WriteLine("Current Range:0");
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    Fl.WriteLine("T1:" + dr[15].ToString());
                    Fl.WriteLine("Cycles:1");
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());
                    }
                }

                if (tchName == "OCP" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: OCP");
                    //Fl.WriteLine("TchNo: " + OCP.ToString());
                    Fl.WriteLine("Time:" + clasglobal.ocpprms_ocp.Time.ToString());
                }

                if (tchName == "CHA" || tchName == "All")
                {
                    Fl.WriteLine("\n");
                    Fl.WriteLine("Technic: CHA");
                    //Fl.WriteLine("TchNo: " + CA.ToString());
                    Fl.WriteLine("E1:" + dr[3].ToString());
                    if (typedevice == 1)
                        Fl.WriteLine("E2:" + dr[4].ToString());
                    Fl.WriteLine("Equilibrium Time:" + dr[7].ToString());
                    if (typedevice == 1)
                        Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()) * 1000);
                    else
                        Fl.WriteLine("T1:" + double.Parse(dr[15].ToString()));

                    if (typedevice == 1)
                        Fl.WriteLine("T2:" + double.Parse(dr[16].ToString()) * 1000);
                    Fl.WriteLine("Cycles:" + "1"); //caprms.Cycles.ToString());
                    if (typedevice == 2)
                    {
                        Fl.WriteLine("vs_OCP:" + dr[19].ToString());
                        Fl.WriteLine("OCP Measurment:" + dr[20].ToString());

                    }

                }
                Fl.Close();
            }

            Fl.Close();
        }
        private int HexToDecimal(string hex)
        {
            int sum = 0;
            for (int i = hex.Length - 1; i >= 0; i--)
                if ((int)hex[i] >= 48 && (int)hex[i] <= 57)
                    sum += ((int)hex[i] - 48) * (int)Math.Pow(16, hex.Length - i - 1);
                else
                    sum += ((int)hex[i] - 55) * (int)Math.Pow(16, hex.Length - i - 1);
            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
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
                    panel1.BackColor = cl.Color;
                    panel4.BackColor = cl.Color;
                  
                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[4] = "2080 show:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void tChart1_Click(object sender, EventArgs e)
        {

        }

        public void Calc_V_I1_I2(string VI_endstr, bool show_online,int row)
        {
            Boolean I_sin = true;
            try
            {
                string SS = "";
                //double[] stepCurr = { 0, 0, 10000000, 1100000, 100100, 9990, 990, 90.1 };
                //double[] stepCurr = { 0, 0, 8887901.3443, c, 66733, 6660, 660, 60 };
                //double[] stepCurr = { 0, 0, 10000000, 999900, 101490, 10180, 1021, 103 };  //modares Tehran
                //double[] stepCurr = { 0, 0, 10000000, 1029999, 104500, 10500, 1055, 105 }; //DR bagherzade
                // double[] stepCurr = { 0, 0, 10000000, 1000000, 101000, 10100, 1040, 104 }; //Elmi-Ahvaz
                double[] stepCurr = { 0, 10000000, 1000000, 100000, 10000, 1000, 100 };////man0, 0,
                // double[] stepCurr = {0, 10000000, 1000000, 101000, 10100, 10400, 104 }; //Elmi-Ahvaz
               // int chsum = 0;
                double V = 0, I1 = 0, I2 = 0;
                int StepCurNum1 = 0, StepCurNum2 = 0;
                byte CS = 0;
                while (VI_endstr[CS] != '$') { CS++; if (CS > 16) break; }
                if (CS > 0) VI_endstr = VI_endstr.Substring(CS);
                string s = VI_endstr.Substring(CS + 1, 4);// endstr[1] + endstr[2] + endstr[3] + endstr[4];
                //if (new_numSeries != this.grf.num_Series)
                //{
                //    Row_Data = 0;
                //    new_numSeries = this.grf.num_Series;
                //}
                SS = VI_endstr;
                //V = (double)HexToDecimal(s);
                V = (int)HexToDecimal(s);
                s = VI_endstr.Substring(CS + 5, 4); //(endstr[5] + endstr[6] + endstr[7] + endstr[8]).ToString();
                // I1 = (double)HexToDecimal(s);
                I1 = (int)HexToDecimal(s);
                s = VI_endstr.Substring(CS + 9, 4);  //] + endstr[10] + endstr[11] + endstr[12]).ToString();
                // I2 = (double)HexToDecimal(s);
                I2 = (int)HexToDecimal(s);
                CS = 0;
                StepCurNum1 = Convert.ToInt16(VI_endstr[13]) - 48;
                StepCurNum2 = Convert.ToInt16(VI_endstr[14]) - 48;
                /////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////
                /*******************************************************************
                grdTech.Rows.Insert(rrrr + 1, 1);
                grdTech.Rows[rrrr].Cells["Row"].Value = VI_endstr;
                grdTech.Rows[rrrr].Cells["TCH"].Value = StepCurNum1.ToString();
                grdTech.Rows[rrrr].Cells["Dly"].Value = StepCurNum2.ToString();
                rrrr++;
                /*******************************************************************/
                //if (grf.tech != CPC && grf.tech != CA && grf.tech != SC)
                //    I2 += StepCurNum2 * 65536; 
                CS = (byte)(VI_endstr[15]);
                /*
                chsum = 0;
                try
                {
                    chsum = (int)V + (int)I1 + (int)I2 + StepCurNum1 + StepCurNum2;
                    ////////////////////man/////////////
                    if ((byte)chsum != (byte)CS)
                    {
                        Num_Error++;


                    }// this.grf.Rawdata(this.grf.num_Series, Row_Data) = "Error";
                }
                catch { }
                */
               
                if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CPC2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CHA2080))    // CPC return (0,I,T,StepCurr,0)   || SC return (0,I,T,0,0);;   
                {
                    if (I2 < Prior_I2) Num_OverFlow_CPC_CA++;
                    Prior_I2 = I2;
                    I2 += (Num_OverFlow_CPC_CA * 65535);
                    V = I2;
                    I2 = 0;
                }
                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != 99 && (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != clasglobal.OCP2080 && OCP_Timer == 0 && !OCP_Ok))
                {

                    ////////////// calculat I1
                    if (I1 > 0x1FFF)
                    {
                        I1 = 0x4000 - I1;
                        I1 = I1 * -1;
                        I_sin = true;              //bit i_sin
                    }
                    else I_sin = false;
                    if (I1 > 0x8000) I1 -= 65536;
                    I1 = I1 * 0.61035; ////0.61035
                    if (I_sin) I1 *= -1;
                    I1 /= 1000;                  //  if (I1 >0) I1*=-1;
                    ////////////////calculat I2
                    if (I2 > 0x1FFF)
                    {
                        I2 = 0x4000 - I2;
                        I2 = I2 * -1;
                        I_sin = true;              //bit i_sin
                    }
                    else 
                        I_sin = false;
                    //   if (I1 > 0x8000) I1 -= 65536; 
                    I2 = I2 * 0.61035; ////0.61035
                    if (I_sin) I2 *= -1;

                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != clasglobal.CPC2080 && int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != clasglobal.CHA2080)
                    {
                        if (V > 0x8000) V -= 65536;
                        //  if (I2 > 0x8000) I2 -= 65536;
                        V /= 1000;
                        I2 /= 1000;

                    }
                  
                    ///////////////////////
                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != clasglobal.OCP2080 && OCP_Timer == 0 && !OCP_Ok)
                    {
                      
                        if (StepCurNum1 > 0 && StepCurNum1 < 7)
                            if (stepCurr[StepCurNum1] != 0)
                                I1 = I1 / stepCurr[StepCurNum1];   //in Amper
                            else 
                                I1 = 0;
                       
                    }
                
                    if (StepCurNum2 >= 0 && StepCurNum2 < 8)
                        if (stepCurr[StepCurNum2] != 0)
                            I2 = I2 / stepCurr[StepCurNum2];  // in Amper
                        else I2 = 0;
                  
                }
             
                double temp = 0;
                if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.OCP2080 || OCP_Ok))     // OCP return (V,T,0,0,0);&&(CP_Ok == false)
                {
                    //I1 *= 1000;
                    if (V > 0x1FFF)
                    {
                        V = 0x4000 - V;
                        V = V * -1;
                        I_sin = true;              //bit i_sin
                    }
                    else I_sin = false;

                   
                    V = V * 0.6104;
                    V *= -1;
                    V /= 1000;



                    if (OCP_Ok && OCP_Timer > 0)
                    {
                        show_online = false;
                        Setfilllable(Environment.NewLine + "Time(S): " + ((int)I1).ToString() + "        OCP(mV): " + (V).ToString("000e-0"), 1, 1);
                        System.Threading.Thread.Sleep(1000);
                        OCP_Timer--;

                        //tsocpTime.Text = "Time(S): " + ((int)I1).ToString();
                        //toolStripStatusLabel3.Text = "OCP(mV): " + (V).ToString("000e-0");

                        //return;
                    }
                    //else
                    //    OCP_Timer = 0;




                    temp = V;
                    V = I1;
                    I1 = temp;
                    I2 = 0;

                    if (OCP_Timer <= 0 && OCP_Ok)
                    {
                        show_online = false;
                        OCP_Ok = false; 
                        V = 0;
                        I2 = 0;
                        I1 = 0;
                    }
                }
                //////////////////////////////////////////////////////////
                if ((int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CPC2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CHA2080))    // CPC return (0,I,T,StepCurr,0)   || SC return (0,I,T,0,0);;   
                {
                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CHA2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CPC2080) 
                        V *= 10;
                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CPC2080)
                        I1 *= V;
                    
                }

                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) ==clasglobal.OCP2080)
                {
                    V = Math.Round(V, 2);
                    I1 = Math.Round(I1, 2);
                    I2 = Math.Round(I2, 2);
                }
                number++;
                if (I1 != 0 && show_online)
                {
                    Runnig(V, I1, I2, row);//گراف                             
                }
                if (OCP_Timer == 0)
                    show_online = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   
        public void Runnig( double V, double I1, double I2, int row)
        {
            if (this.InvokeRequired)
            {
                showOnlineData1 d = new showOnlineData1(Runnig);
                this.Invoke(d, new object[] { V, I1, I2, row });
            }
            else
            {
               
                if (!is_set_graph)
                {
                    Set_Graph(true, row);
                    is_set_graph = true;
                }
              
                //grf.textBox1.Text = V.ToString("00e-0");

                if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) != clasglobal.DPV2080)
                {
                    if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.NPV2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.LSV2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.CV2080 || int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.SWV2080)
                    {
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                        {
                            if (V >= double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) - 0.5 && V <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + 0.5)    //this.grf.num_Series
                            {
                               
                                this.tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(V, I1 - I2);
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
                               
                            }
                        }
                        else
                            if (V <= double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + 0.5 && V >= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) - 0.5)
                            {
                                this.tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(V, I1 - I2);
                                                                                

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
                              
                            }
                    }
                    else
                    {
                      
                        this.tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(V / 1000, I1 - I2);   

                        if (int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == clasglobal.OCP2080)//mn
                        {
                            //OnlineChart.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                            //this.OnlineChart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                           
                            int time_text = this.tChart1.Series[tChart1.Series.Count - 1].Points.Count - 1;
                            this.tChart1.Series[tChart1.Series.Count - 1].Points[time_text].AxisLabel = time_text.ToString();
                        }

                       

                        if (shomar_othertech >= othertech_val0.Length)
                        {
                            Array.Resize(ref othertech_val0, othertech_val0.Length + 200);
                            Array.Resize(ref othertech_val1, othertech_val1.Length + 200);
                            Array.Resize(ref othertech_val2, othertech_val2.Length + 200);
                            Array.Resize(ref othertech_val3, othertech_val3.Length + 200);

                        }

                        othertech_val0[shomar_othertech] = V/1000;
                        othertech_val1[shomar_othertech] = I1;
                        othertech_val2[shomar_othertech] = I2;
                        othertech_val3[shomar_othertech] = I1 - I2;

                        shomar_othertech++;

                       
                    }
                }
                else
                {
                    if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                    {
                        if (V >= double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) && V <= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                        {
                            this.tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(V, I1 - I2);//mn i2-i1 bood graph ra makos mizad
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
                            othertech_val3[shomar_othertech] = I1 - I2;//mn

                            shomar_othertech++;
                         
                        }
                    }
                    else
                        if (V <= double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) && V >= double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()))
                        {
                        this.tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(V, I1 - I2);//mn i2-i1 bood graph ra makos mizad
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
                            othertech_val3[shomar_othertech] = I1 - I2;//mn

                            shomar_othertech++;
                         
                        }
                }

                mainf.toolStripProgressBar1.Value++;
                if (mainf.toolStripProgressBar1.Value == 50)
                    mainf.toolStripProgressBar1.Value = 0;
              
                if (!flag_running)
                {
                    mainf.toolStripProgressBar1.Visible = false;
                  
                    mainf.xyLabel.Visible = true;
                    flag_running = false;
                   
                   
                }
                tChart1.Update();
            }
        }
        public void doRun(int row)
        {
            try
            {
                flag_running = true;

                // ShowOnline = true;
                string S = " ", endstr = "";
                int i_h = 0;

                int num_Buffer_Bytes = 0;
                char[] bf = new char[sp1.ReadBufferSize];

                int hh = 0;
                if (sp1.IsOpen)
                {
                    while (sp1.BytesToRead < 19 && hh < 20)//////20
                    {
                        System.Threading.Thread.Sleep(1000);
                        hh++;

                        if (sp1.BytesToRead >= 19) break;///////20
                    }
                   
                    if (hh >= 20)
                    {
                        MessageBox.Show("device not answer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sp1.Close();
                        return;
                    }
                    sp1.Read(bf, 0, num_Buffer_Bytes = sp1.BytesToRead);


                    //while (bf[i_h] != '$' && i_h < num_Buffer_Bytes)
                    //    i_h++;
                    i_h = Array.IndexOf(bf, '$', i_h);

                    while (flag_running)   // || Seq_Run_Num < grdTech_RowCount - 1)    //&& num_Buffer_Bytes > 0
                    {
                        while (i_h < num_Buffer_Bytes)
                        {
                            S = bf[i_h].ToString();
                            endstr += S;
                            i_h++;
                            while (bf[i_h] != '$' && i_h < num_Buffer_Bytes)
                            {
                                S = bf[i_h].ToString();
                                endstr += S;
                                i_h++;
                            }
                            if (i_h < num_Buffer_Bytes - 2 && bf[i_h + 2] == '$' && endstr.Length > 13)
                            {
                                endstr = endstr + "$#";
                                i_h += 2;
                            }

                            if (endstr.IndexOf("END") < 0 && endstr.IndexOf("DEN") < 0)  // && endstr.length < 15
                            {
                                if (endstr.Length > 16)
                                {
                                    Calc_V_I1_I2(endstr, true, row);//گراف و دیگر
                                    endstr = "";
                                }
                            }
                            else
                            {
                                if (endstr.IndexOf("END") >= 0 || endstr.IndexOf("DEN") >= 0)
                                {
                                    if (endstr.Length > 16)
                                    {
                                        Calc_V_I1_I2(endstr.Substring(0, endstr.Length - 6), true, row);
                                        endstr = "";
                                    }
                                    flag_running = false;

                                }
                            }


                        }

                        if (num_Buffer_Bytes >= 0 && flag_running)
                        {
                            hh = 0;
                            int br = 0;
                            if (OCP_Ok || byte.Parse(dschart1.chartlist1_run.Rows[row]["techno"].ToString()) == clasglobal.OCP2080)
                            {
                                br = 1;
                            }
                            else br = 5;
                          
                            while (sp1.BytesToRead < br && hh < 5000000) hh++;
                          
                            sp1.Read(bf, 0, num_Buffer_Bytes = sp1.BytesToRead);
                            if (num_Buffer_Bytes == 0)
                            {
                                System.Threading.Thread.Sleep(Convert.ToInt16(dschart1.chartlist1_run.Rows[row]["QT"].ToString()) * 1000);
                                hh = 1000;
                                if (num_Buffer_Bytes == 0)
                                {
                                    OCP_Ok = false;

                                    flag_running = false;
                                    Chart2 = tChart1;
                                    if (thread_grf != null)
                                        if (thread_grf.ThreadState == ThreadState.Running)
                                            thread_grf.Abort();

                                }

                            }
                            i_h = 0;
                        }
                    }
                                     
                }
                else
                    MessageBox.Show("Port not open.");
            }
            catch 
            {
               
            }
        }
    
      
        public void runf(byte rng, double cp11, double ct11, double cp12, double ct12, double cp13, double ct13, double cp14, double ct14, double cp15, double ct15)
        {
                this.treeView3.Nodes[0].Nodes.Clear();
                Analiz_CP1 = cp11 * 1000;
                Analiz_CP2 = cp12 * 1000;
                Analiz_CP3 = cp13 * 1000;
                Analiz_CP4 = cp14 * 1000;
                Analiz_CP5 = cp15 * 1000;

                Analiz_CT1 = ct11;
                Analiz_CT2 = ct12;
                Analiz_CT3 = ct13;
                Analiz_CT4 = ct14;
                Analiz_CT5 = ct15;

                Range1 = rng;

                // timer1.Enabled = true;
                //  Init_Port();
                if (detect_Error)
                {
                    Setgrfnodetext(1, 4, 1);
                    return;
                }
                if (thread_grf.ThreadState == ThreadState.Aborted || thread_grf.ThreadState == ThreadState.Unstarted)
                {
                    thread_grf = new Thread(Run_key);//mn

                    thread_grf.Start();
                }
                else if (thread_grf.ThreadState == ThreadState.Stopped)
                {
                    thread_grf = new Thread(Run_key);

                    thread_grf.Start();
                }
        }
        delegate void Setgrftchart1Callback(bool overlaymain, int row1,int code, int state);
        private void Setgrftchart1(bool overlaymain, int row1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback d = new Setgrftchart1Callback(Setgrftchart1);
                        this.Invoke(d, new object[] {overlaymain, row1,  code, 2 });
                    }
                    else
                    {
                        tChart1.Titles.Clear();
                        tChart1.Titles.Add("t1");
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

                        //int num_Series = 0;//mn
                        int num_Series = tChart1.Series.Count - 1;


                        Random Random1 = new Random();
                        int Random_Color_Index = 0;
                        Random_Color_Index = (byte)(Random1.NextDouble() * 90);
                        //OnlineChart.Series[num_Series].Color = clasglobal.color_Graph[Random_Color_Index];
                        tChart1.Series[num_Series].Color = Color.Red;


                        if (!overlaymain)
                        {
                            tChart1.Series[num_Series].Points.Clear();
                            //// OnlineChart.Series.Remove(OnlineChart.Series[0]);
                            //if (OnlineChart.Series.Count > 1)                      ////mn
                            //{
                            //    OnlineChart.Series[1].Points.Clear();
                            //    OnlineChart.Series.Remove(OnlineChart.Series[1]);
                            //}
                        }

                        tChart1.Titles[0].Text = clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        tChart1.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle2080[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        tChart1.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle2080[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];




                        //if (double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) == clasglobal.CV2080)
                        //{
                        //    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Max(Math.Max(double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()), double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString())), double.Parse(dschart1.chartlist1_run.Rows[row1][5].ToString()));
                        //    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Min(Math.Min(double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()), double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString())), double.Parse(dschart1.chartlist1_run.Rows[row1][5].ToString()));
                        //    OnlineChart.ChartAreas[0].AxisX.Interval = Math.Round((OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum) / 10, 1);

                        //}
                        //else if (double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) <= 8 || double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) == clasglobal.LSV2080)
                        //{
                        //    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Max(double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()), double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString()));
                        //    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Min(double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()), double.Parse(dschart1.chartlist1_run.Rows[row1][4].ToString()));
                        //    OnlineChart.ChartAreas[0].AxisX.Interval = Math.Round((OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum) / 10, 1);

                        //}
                        //else if (double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) == clasglobal.CPC2080)
                        //{
                        //    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Max(double.Parse(dschart1.chartlist1_run.Rows[row1][3].ToString()), double.Parse(dschart1.chartlist1_run.Rows[row1][15].ToString()));
                        //    OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                        //    OnlineChart.ChartAreas[0].AxisX.Interval = Math.Round((OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum) / 10, 1);


                        //}
                        //else if (double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) == clasglobal.CHA2080 || double.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString()) == clasglobal.CHP2080 )
                        //{
                        //    OnlineChart.ChartAreas[0].AxisX.Maximum = double.Parse(dschart1.chartlist1_run.Rows[row1][15].ToString())  ;
                        //    OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                        //    OnlineChart.ChartAreas[0].AxisX.Interval = Math.Round((OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum) / 10, 1);


                        //}

                    }
                    break;
                case 3 :
                    if (state == 1)
                    {
                        Setgrftchart1Callback d = new Setgrftchart1Callback(Setgrftchart1);
                        this.Invoke(d, new object[] { overlaymain, row1, code, 2 });
                    }
                    else
                    {
                        tChart1.Series[row1].Points.Clear();
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
                        //if (OnlineChart.Series.Count == 0)
                        tChart1.Series.Add(ser1);

                        is_set_graph = true;//mn
                    }
                    break;
                
            }
        }
        delegate void Setgrftchart1Callback2(int ser1,double V1,double I1, int code, int state);
        private void Setgrftchart12(int ser1, double V1, double I1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback2 d = new Setgrftchart1Callback2(Setgrftchart12);
                        this.Invoke(d, new object[] { ser1,V1,I1, code, 2 });
                    }
                    else
                    {
                        tChart1.Series[ser1].Points.AddXY(V1, I1);
                    }
                    break;

            }
        }
        delegate void Setgrftchart1Callback3(int row1, Color color1,int code, int state);
        private void Setgrftchart13(int row1, Color color1,int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback3 d = new Setgrftchart1Callback3(Setgrftchart13);
                        this.Invoke(d, new object[] { row1,color1,code, 2 });
                    }
                    else
                    {
                        tChart1.Series[0].Color = color1;
                       // OnlineChart.Series[0].Points.Clear();

                        for (int i = 0; i < shomar_othertech; i++)
                        {
                            if (i < tChart1.Series[0].Points.Count)
                            {
                                tChart1.Series[0].Points[i].XValue =othertech_val0[i];
                                tChart1.Series[0].Points[i].YValues[0] = othertech_val3[i];

                            }
                            else {
                                tChart1.Series[0].Points.AddXY(othertech_val0[i], othertech_val3[i]);
                            }
                        }
                    }
                    break;

            }
        }
       
        delegate void SetgrfnodeCallback(int txt, int code, int state);
        private void Setgrfnodetext(int txt, int code, int state)
        {
            try
            {
                switch (code)
                {
                    case 1:

                        if (state == 1)
                        {
                            SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                            this.Invoke(d, new object[] { txt, code, 2 });
                        }
                        else
                        {

                            this.treeView3.Nodes[0].Nodes[txt].ForeColor = Color.Red;
                            //this.dschart1.graphlistdata.Clear();
                            //this.dschart1.graphlistdatashow.Clear();
                            // mainf.toolStripStatusLabel5.Text = "";
                            mainf.toolStripStatusLabel5.ForeColor = Color.Green;

                            if (number_runing_teq > 1)
                                mainf.toolStripStatusLabel5.Text = (show_number_runing + 1).ToString().Trim() + "_" + number_runing_teq.ToString() + " - Running technique " + treeView3.Nodes[0].Nodes[show_number_runing].Text;
                            else
                                mainf.toolStripStatusLabel5.Text = (show_number_runing + 1).ToString().Trim() + " - Running technique " + treeView3.Nodes[0].Nodes[show_number_runing].Text;

                            //mainf.toolStripStatusLabel5.Text = (txt + 1).ToString().Trim() + "- Running technique " + treeView3.Nodes[0].Nodes[txt].Text;

                        }
                        break;
                    case 2:
                        if (state == 1)
                        {
                            try
                            {
                                SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                                this.Invoke(d, new object[] { txt, code, 2 });
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }
                        }
                        else
                        {

                            this.treeView3.Nodes[0].Nodes[txt].ForeColor = Color.Green;
                            label4.Text = label4.Text + "\t\t*******************\nE1:" + tChart1.Series[tChart1.Series.Count - 1].Points.FindMinByValue("X").XValue.ToString("000e-0") +
                                "\nE2:" + tChart1.Series[tChart1.Series.Count - 1].Points.FindMaxByValue("X").XValue.ToString("000e-0") + "\nMin. I:" + tChart1.Series[tChart1.Series.Count - 1].Points.FindMinByValue("Y").YValues[0].ToString("000e-0") +
                                "\nMax. I:" + tChart1.Series[tChart1.Series.Count - 1].Points.FindMaxByValue("Y").YValues[0].ToString("000e-0");

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
                            dr[11] = double.Parse(dschart1.chartlist1_run.Rows[txt][13].ToString());
                            dr[12] = double.Parse(dschart1.chartlist1_run.Rows[txt][14].ToString());
                            dr[13] = double.Parse(dschart1.chartlist1_run.Rows[txt][15].ToString());
                            dr[14] = double.Parse(dschart1.chartlist1_run.Rows[txt][16].ToString());
                            dr[15] = double.Parse(dschart1.chartlist1_run.Rows[txt][17].ToString());
                            dr[16] = double.Parse(dschart1.chartlist1_run.Rows[txt][18].ToString());
                            dr[17] = double.Parse(dschart1.chartlist1_run.Rows[txt][2].ToString());
                            dr[18] = 0;
                            dr[19] = dschart1.chartlist1_run.Rows[txt][19].ToString();
                            dr[20] = double.Parse(dschart1.chartlist1_run.Rows[txt][20].ToString());
                            dr[21] = dschart1.chartlist1_run.Rows[txt][23].ToString();
                            dr[22] = dschart1.chartlist1_run.Rows[txt][24].ToString();
                            this.mainf.grf.dschart2.analyselist.Rows.Add(dr);
                            this.mainf.grf.dschart2.analyselist.AcceptChanges();
                            int maxrana = this.mainf.grf.dschart2.analyselist.Rows.Count;
                            this.mainf.grf.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName2080[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())]);

                            //this.mainf.grf.treeView3.Nodes[0].LastNode.ForeColor = Color.Blue;
                            double[] xval = new double[shomar_othertech];
                            double[] yval = new double[shomar_othertech];

                            //for (int countr = 0; countr < shomar_othertech; countr++)
                            //{
                            //    dr = this.mainf.grf.dschart2.analyselistdata.NewRow();
                            //    dr[0] = maxrana-1;
                            //    dr[1] = double.Parse(dschart1.graphlistdata.Rows[countr][1].ToString());
                            //    xval[countr]=double.Parse(dr[1].ToString());
                            //    dr[2] = double.Parse(dschart1.graphlistdata.Rows[countr][2].ToString());
                            //    yval[countr]=double.Parse(dr[2].ToString());
                            //    dr[3] = double.Parse(dschart1.graphlistdata.Rows[countr][3].ToString());
                            //    dr[4] = double.Parse(dschart1.graphlistdata.Rows[countr][4].ToString());

                            //    this.mainf.grf.dschart2.analyselistdata.Rows.Add(dr);
                            //    this.mainf.grf.dschart2.analyselistdata.AcceptChanges();
                            //}
                            this.mainf.grf.tech = byte.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());
                            //// this.mainf.grf.Set_Graph(0, false, true);
                            this.mainf.grf.show_Graph(maxrana - 1, this.mainf.grf, "MainChart", othertech_val0, othertech_val3, true, (int)shomar_othertech);
                            this.mainf.grf.treeView3.SelectedNode = this.mainf.grf.treeView3.Nodes[0].Nodes[maxrana - 1];
                            this.mainf.grf.tChart1.Series[maxrana - 1].Enabled = false;
                            Num_OverFlow_CPC_CA = 0;
                            Prior_I2 = 0;
                        }
                        break;
                    case 3:
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
                                this.treeView3.Nodes[0].Nodes.Add(clasglobal.TechName2080[int.Parse(this.dschart1.chartlist1_run.Rows[iii1][1].ToString())]);
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
                            this.mainf.stf.runToolStripMenuItem1.Enabled = true;
                            this.mainf.stf.tsbRun.Enabled = true;
                        }
                        break;

                    case 5:
                        if (state == 1)
                        {
                            SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                            this.Invoke(d, new object[] { txt, code, 2 });
                        }
                        else
                        {
                            tChart1.Series[tChart1.Series.Count - 1].Points.Clear();
                            for (int i = 0; i < shomar_othertech; i++)
                            {
                                tChart1.Series[tChart1.Series.Count - 1].Points.AddXY(othertech_val0[i], othertech_val3[i]);

                                if (int.Parse(dschart1.chartlist1_run.Rows[tecnic_row_for_label_text_ocp][1].ToString()) == clasglobal.OCP2080)//mn
                                {
                                    int time_text = i;
                                    this.tChart1.Series[tChart1.Series.Count - 1].Points[time_text].AxisLabel = (time_text + 1).ToString();
                                }
                            }
                        }
                        break;
                }
            }
            catch { }
        }
        delegate void Setfilllablecallback(string txt, int code, int state);
        private void Setfilllable(string txt, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setfilllablecallback d = new Setfilllablecallback(Setfilllable);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {

                        label1.Text = txt + Environment.NewLine + label1.Text.Trim();
                       
                    }
                    break;

            }
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
                        tChart1.Refresh();
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
                        //tsbhold.Enabled = true;
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
                        mainf.toolStripStatusLabel5.Text = "Stop";
                        this.mainf.stf.tsbNew.Enabled = true;
                        
                        mainf.toolStripProgressBar1.Visible = false;
                      
                        //grdTech.Enabled = true;
                        //btnDeleteAll.Enabled = true;
                        tsbstop.Enabled = false;
                        //tsbhold.Enabled = false;
                        //tsbcont.Enabled = false;                        
                        //runToolStripMenuItem.Enabled = true;
                        //setupToolStripMenuItem1.Enabled = true;
                        mainf.stf.runToolStripMenuItem1.Enabled = true;
                        mainf.stf.tsbRun.Enabled = true;
                        timer3.Enabled = false;

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
                        //this.mainf.stf.tsbRun.Enabled = false;
                        //this.mainf.stf.tsbNew.Enabled = false;
                        ////tsmOpen.Enabled = false;
                        ////tsmNew.Enabled = false;
                        ////runToolStripMenuItem.Enabled = false;
                        //pausePlayToolStripMenuItem.Enabled = true;
                        //stopToolStripMenuItem.Enabled = true;
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
        private void Set_value(int row)
        {
            //EquilibriumTime = 0;
            //OCP_Timer = 0;
            detect_Error = false;
            try
            {
                if (!sp1.IsOpen)
                {
                    sp1.Open();
                }
                switch (int.Parse(dschart1.chartlist1_run.Rows[row]["techno"].ToString()))
                {

                    case clasglobal.NPV2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E2"].ToString()), -5, 5, E2, 1000, "E2(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["SR"].ToString()), 0.0001, 0.25, SR, 1, "Scan Rate(V/S)", "V/S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["HS"].ToString()), 0.001, 0.025, HS, 1000, "HStep(mV)", "mV");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()), 0.1, 10, TS, 10000, "TStep(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["PW"].ToString()), 0.05, double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()) - 0.05, PulsWidth, 1000, "Pulse Width(S)", "S");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }
                        break;
                    case clasglobal.DPV2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E2"].ToString()), -5, 5, E2, 1000, "E2(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["SR"].ToString()), 0.0001, 0.25, SR, 1, "Scan Rate(V/S)", "V/S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["HS"].ToString()), 0.001, 0.025, HS, 1000, "HStep(mV)", "mV");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()), 0.1, 10, TS, 10000, "TStep(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["PW"].ToString()), 0.05, double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()) - 0.05, PulsWidth, 1000, "Pulse Width(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["PH"].ToString()), 0.001, 0.25, PulsHeight, 1000, "Pulse Height(mV)", "mV");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }

                        break;
                    case clasglobal.SWV2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E2"].ToString()), -5, 5, E2, 1000, "E2(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["SR"].ToString()), 0.001, 25, SR, 1, "Scan Rate(V/S)", "V/S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["HS"].ToString()), 0.001, 0.025, HS, 1000, "HStep(mV)", "mV");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["FR"].ToString()), 1, 1000, frequency, 1, "Frequency(Hz)", "Hz");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["PH"].ToString()), 0.001, 0.25, PulsHeight, 1000, "Pulse Height(mV)", "mV");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }
                        break;
                    case clasglobal.CV2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E2"].ToString()), -5, 5, E2, 1000, "E2(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E3"].ToString()), -5, 5, E3, 1000, "E3(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["SR"].ToString()), 0.0001, 250, SR, 1, "Scan Rate(V/S)", "V/S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["HS"].ToString()), 0.001, 0.025, HS, 1000, "HStep(mV)", "mV");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()), 0.0001, 10, TS, 10000, "TStep(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }

                        break;
                    case clasglobal.LSV2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E2"].ToString()), -5, 5, E2, 1000, "E2(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["SR"].ToString()), 0.0001, 250, SR, 1, "Scan Rate(V/S)", "V/S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["HS"].ToString()), 0.001, 0.025, HS, 1000, "HStep(mV)", "mV");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["TS"].ToString()), 0.0001, 10, TS, 10000, "TStep(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }
                        break;

                    case clasglobal.OCP2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["T1"].ToString()), 1, 65000, TS, 1, "T1(S)", "S");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle(s)", "");

                        break;
                    case clasglobal.CPC2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["T1"].ToString()), 0, 65000, TS, 1, "Time(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle", "");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }
                        break;

                    case clasglobal.CHA2080:
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["E1"].ToString()), -5, 5, E1, 1000, "E1(V)", "V");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["T1"].ToString()), 1, 65000, TS, 1, "Time(S)", "S");
                        set_complete_Comm(double.Parse(dschart1.chartlist1_run.Rows[row]["QT"].ToString()), 0, 2000, QT, 1, "Equilibrium Time(S)", "S");
                        set_complete_Comm((double)Range1, 0, 6, Current_Range, 1, "Current Range", "");
                        set_complete_Comm(1, 1, 1000, CY, 1, "Cycle", "");
                        if (dschart1.chartlist1_run.Rows[row]["vs_OCP"].ToString() == "True")
                        {
                            Set_Command(vs_OCP, 1);
                            if (double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) >= 1 && double.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()) <= 65000)
                            {
                                Set_Command(OCP_Meas, int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString()));
                                OCP_Timer = (int)int.Parse(dschart1.chartlist1_run.Rows[row]["OCPmeas"].ToString());
                                OCP_Ok = true;
                            }
                            else
                                showError("OCP measurment(S)", "0", "65000");
                        }
                        else
                        {
                            Set_Command(vs_OCP, 0);
                            Set_Command(OCP_Meas, 0);
                            OCP_Timer = 0;
                            OCP_Ok = false;
                        }
                        break;


                    default: break;
                }
                if (row == 0)
                {
                    Set_Command(CP1, (int)Analiz_CP1);
                    Set_Command(CP2, (int)Analiz_CP2);
                    Set_Command(CP3, (int)Analiz_CP3);
                    Set_Command(CP4, (int)Analiz_CP4);
                    Set_Command(CP5, (int)Analiz_CP5);
                    Set_Command(CT1, (int)Analiz_CT1);
                    Set_Command(CT2, (int)Analiz_CT2);
                    Set_Command(CT3, (int)Analiz_CT3);
                    Set_Command(CT4, (int)Analiz_CT4);
                    Set_Command(CT5, (int)Analiz_CT5);
                    //Sum_CTn = Analiz_CT1 + Analiz_CT2 + Analiz_CT3 + Analiz_CT4 + Analiz_CT5;
                    //Sum_CTn = Math.Round(Sum_CTn);
                    //CP_Ok = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                detect_Error = true;

            }
        }
        private void showError(string err, string val1, string val2)
        {
            if (showMessageError)
                MessageBox.Show("Invalid Value of '" + err + "'" + (char)13 + "Value Must between '" + val1 + "' And '" + val2 + "'", "Parameter error");
            detect_Error = true;
        }
        private double set_complete_Comm(double X, double Min, double Max, byte Comm, int Zarib, string strVal, string Unit_)
        {
            if (X >= Min && X <= Max)
            {
                Set_Command(Comm, (int)(X * Zarib));
                //if (Comm == 3) E1_Input = X;
                //if (Comm == 4) E2_Input = X;
            }
            else
            {
                showError(strVal, Min.ToString() + Unit_, Max.ToString() + Unit_);
                X = Min;
            }
            return X;
        }


        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Set_Command(Run, 2);
                Set_Command(Multirun_Delay, -2);
                System.Threading.Thread.Sleep(500);
                sp1.DiscardInBuffer();
                sp1.DiscardOutBuffer();
                flag_running = false;


                for (int count1 = 0; count1 < treeView3.Nodes[0].Nodes.Count; count1++)
                {
                    if (treeView3.Nodes[0].Nodes[count1].ForeColor == Color.Red)
                    {
                        transferdata(count1);
                        smootht4(tecnic_row_for_label_text_ocp);
                        // shomartime = 0;
                        Setgrfnodetext(count1, 2, 1);
                        Setbtnenabled(true, 2, 1);
                        //shomar_othertech = 0;
                        //Array.Clear(othertech_val0, 0, othertech_val0.Length);
                        //Array.Clear(othertech_val1, 0, othertech_val1.Length);
                        //Array.Clear(othertech_val2, 0, othertech_val2.Length);
                        //Array.Clear(othertech_val3, 0, othertech_val3.Length);
                        //timer1.Stop();
                    }

                }

                thread_grf.Abort();
            }
            catch { }
        }

       

        private void tChart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                var results11 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                foreach (var result in results11)
                {
                    if (result.ChartElementType == ChartElementType.PlottingArea)
                    {

                        switch (statebutton)
                        {
                            case "selectview":
                                tChart1.Cursor = System.Windows.Forms.Cursors.Cross;

                                break;
                            case "selectonly":
                                tChart1.Cursor = System.Windows.Forms.Cursors.Cross;
                                break;
                            case "showvalue":
                                tChart1.Cursor = System.Windows.Forms.Cursors.SizeAll;

                                break;
                            case "zoomin":

                                tChart1.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.zoomin);

                                break;
                            case "zoomout":
                                tChart1.Cursor = LoadCursorFromResource(Skydat.Properties.Resources.Zoomout);

                                break;
                            default:
                                tChart1.Cursor = System.Windows.Forms.Cursors.Default;
                                break;
                        }

                    }
                    else
                        tChart1.Cursor = System.Windows.Forms.Cursors.Default;


                }
                switch (statebutton)
                {
                    //case "selectview":
                    //    if (e.Button == MouseButtons.Left)
                    //    {
                    //    var results1 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    //    foreach (var result in results1)
                    //    {

                    //        if (result.ChartElementType == ChartElementType.PlottingArea)
                    //        {

                    //            if (startpoint_selx == -1)
                    //            {
                    //                startpoint_selx = Math.Min(Math.Max(e.X, (int)OnlineChart.Left), OnlineChart.Left + OnlineChart.Width);
                    //                startpoint_sely = Math.Min(Math.Max(e.Y, (int)OnlineChart.Top), OnlineChart.Top + OnlineChart.Height);
                    //            }
                    //            else
                    //            {
                    //                OnlineChart.Refresh();
                    //                this.Invalidate(rectsel);
                    //                endpoint_selx = Math.Min(Math.Max(e.X, (int)OnlineChart.Left), OnlineChart.Left + OnlineChart.Width);
                    //                endpoint_sely = Math.Min(Math.Max(e.Y, (int)OnlineChart.Top), OnlineChart.Top + OnlineChart.Height);
                    //                rectsel = new Rectangle(Math.Min(startpoint_selx, endpoint_selx), startpoint_sely, endpoint_selx - startpoint_selx, endpoint_sely - startpoint_sely);
                    //                OnlineChart.CreateGraphics().DrawRectangle(new Pen(Color.Red), rectsel);
                    //            }

                    //        }
                    //    }

                    //    }
                    //    break;
                    case "selectonly":
                        Point mousePoint = new Point(e.X, e.Y);
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
            catch (Exception ex) { errors_cls err = new errors_cls(ex,label4 , 2); }
            

        }
      
        private void Set_Command(byte comm, int val)
        {
            byte[] buf = new byte[18];
            string s = "";
            try
            {
                if (!sp1.IsOpen)
                {
                    sp1.Open();
                }

                if (sp1.IsOpen)
                {
                    if (val >= 0)
                    {
                        buf[3] = 43;
                        s = val.ToString();
                    }
                    else
                    {
                        buf[3] = 45;
                        s = Math.Abs(val).ToString();
                    }

                    buf[0] = 35; buf[1] = comm; buf[2] = 36;  //0.7
                    for (int i = 0; i < s.Length; i++) buf[4 + i] = (byte)(Convert.ToByte(s[i]) - 48);
                    for (int i = 0; i < 6 - s.Length; i++) buf[4 + s.Length + i] = 61;
                    // buf[3 + s.Length] = 37;
                    // sp1.Write(buf, 0, 4 + s.Length);
                    buf[10] = 37;
                    sp1.Write(buf, 0, 11);



                    //string result = System.Text.Encoding.UTF8.GetString(buf);
                    //MessageBox.Show(result);

                     
                    try 
                    {
                        int ii = 0;
                        while (sp1.BytesToRead < 1 && ii < 100000) ii++;
                        int num_r = 2;
                        if (comm == 33 || comm == 38 || comm == 36) num_r = 2;
                        else num_r = 3;
                        if (sp1.BytesToRead <= num_r)
                            sp1.Read(buf, 0, sp1.BytesToRead);
                        else
                            sp1.Read(buf, 0, num_r);
                    }
                    catch { }

                    /*
                    }
                    else
                    {
                        buf[0] = 35; buf[1] = comm; buf[2] = 36; buf[3] = 45;
                        s = Math.Abs(val).ToString();
                        for (int i = 0; i < s.Length; i++)
                            buf[4 + i] = (byte)(Convert.ToByte(s[i]) - 48);
                        buf[4 + s.Length] = 37;
                        sp1.Write(buf, 0, 5 + s.Length);
                    }
                   */
                }
            }
            catch { }
        }
        //private int shomartime=0;
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    shomartime++;
        //    if(!(this.mainf==null))
        //        switch (shomartime % 3)
        //        {
        //            case 0:
        //                this.mainf.toolStripStatusLabel1.ForeColor = Color.Red;
        //                break;
        //            case 1:
        //                this.mainf.toolStripStatusLabel1.ForeColor = Color.Blue;
        //                break;
        //            case 2 :
        //                this.mainf.toolStripStatusLabel1.ForeColor = Color.Green;
        //                break;
        //        }
        //    this.mainf.toolStripStatusLabel1.Text ="Elapsed Time(s)= "+ shomartime;
           

        //}

        private void tsbsave_Click(object sender, EventArgs e)
        {
        //    try
        //    {          
        //            if (shomar_othertech == 0)
        //            {

        //                MessageBox.Show("data not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //            SaveFileDialog sfd1 = new SaveFileDialog();
        //            sfd1.AddExtension = true;
        //            sfd1.DefaultExt = "txt";
        //            sfd1.Filter = "Text document(*.txt)|*.txt";
        //            if (sfd1.ShowDialog() == DialogResult.OK)
        //            {

        //                StreamWriter Fl = new StreamWriter(sfd1.FileName, false, Encoding.ASCII);

        //                Fl.WriteLine("---------------------------------------------------");
        //                Fl.WriteLine("row number      X value         Y value");
        //                Fl.WriteLine("---------------------------------------------------");

        //                for (int cc1 = 0; cc1 < shomar_othertech; cc1++)
        //                {
        //                    Fl.WriteLine((cc1 + 1).ToString().Trim() + "\t\t" + othertech_val0[cc1].ToString().Trim() + "\t\t" + othertech_val1[cc1].ToString().Trim());
        //                }
        //                Fl.Close();
        //            }
        //}

        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        }

        private void tsbshowd_Click(object sender, EventArgs e)
        {
            forms2065.frmshowdata frm = new forms2065.frmshowdata();
            frm.listView1.Items.Clear();

            int r = tChart1.Series.Count - 1;

            for (int i = 0; i < tChart1.Series[r].Points.Count; i++)
            {
                frm.listView1.Items.Add(tChart1.Series[r].Points[i].XValue + "          " + tChart1.Series[r].Points[i].YValues[0].ToString());
            }
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
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

            tChart1.ChartAreas[0].CursorX.IsUserEnabled = state;
            tChart1.ChartAreas[0].CursorY.IsUserEnabled = state;
            tChart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = state;
            tChart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = state;
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
            this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled = !this.tChart1.ChartAreas[0].AxisX.MajorGrid.Enabled;
            this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled = !this.tChart1.ChartAreas[0].AxisY.MajorGrid.Enabled;
        }

        private void btnshowaxes_Click(object sender, EventArgs e)
        {
            statebutton = "";
            selectpchart_func(false);
            tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisX.LabelStyle.Enabled;
            tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisX.MajorTickMark.Enabled;
            tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled = !tChart1.ChartAreas[0].AxisY.LabelStyle.Enabled;
            tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = !tChart1.ChartAreas[0].AxisY.MajorTickMark.Enabled;
        }

        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (statebutton)
            {
                case "zoomin":
                    var results11 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results11)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            this.tChart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            double xMin = tChart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                            double xMax = tChart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                            double yMin = tChart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                            double yMax = tChart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum;
                            double posXStart = (tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - xMin) / 1.1;
                            double posXFinish = (tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + xMax) / 1.1;
                            double posYStart = (tChart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - yMin) / 1.1;
                            double posYFinish = (tChart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + yMax) / 1.1;

                            tChart1.ChartAreas[0].AxisX.ScaleView.Zoom(tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - posXStart, tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + posXFinish, DateTimeIntervalType.Auto, true);
                            tChart1.ChartAreas[0].AxisY.ScaleView.Zoom(tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) - posYStart, tChart1.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) + posYFinish, DateTimeIntervalType.Auto, true);

                            //OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(xMin, 0.95 * xMax, DateTimeIntervalType.Auto, true);
                            //OnlineChart.ChartAreas[0].AxisY.ScaleView.Zoom(yMin, 0.95 * yMax, DateTimeIntervalType.Auto, true);
                        }
                    }
                    break;
                case "zoomout":
                    var results12 = tChart1.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results12)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            this.tChart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            tChart1.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                            tChart1.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);

                        }
                    }
                    break;
            }
        }

      

        //private void sp1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    char[] buff = new char[sp1.ReadBufferSize];
        //    sp1.Read(buff, 0, sp1.ReadBufferSize);
        //    string str=new string(buff);
        //    MessageBox.Show(str);
        //}

        //private void sp1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    //byte[] buff=new byte[sp1.ReadBufferSize];
        //    //sp1.Read(buff,0,sp1.ReadBufferSize);
        //}

      


       

     

    
    }

}