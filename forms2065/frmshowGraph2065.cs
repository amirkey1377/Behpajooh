using Microsoft.Win32;
using Skydat.classes2065;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Skydat.forms2080;

namespace Skydat.forms2065
{
    public partial class frmshowGraph2065 : Form
    {
        private static readonly object serialLock = new object();
        public static int ChartScaleMultiplier = 2;
        public static int SampleRateMultiplier = 0;


        //Declare your global variable here
        private int shomar_cha_chc = 0, countcv = 0, shomar_othertech = 0, shomarchpa = 0, num_Series = 0, num_cycles = -1;
        private bool cv_stop;
        private bool flag_running = false, S_et = false, fst = false, pulsy = false, ngtv, N_gtv, runerror = false;
        private double cycles = 1, Analiz_St1 = 0, Analiz_CT1 = 0, Analiz_CT2 = 0, Analiz_CT3 = 0, Analiz_CP1 = 0, Analiz_CP2 = 0, Analiz_CP3 = 0, Analiz_T3 = 0, ti = 0;
        double[] time_cha_chc = new double[200];
        double[] volt_cha_chc = new double[200];
        List<double> VoltageStepArray = new List<double>();
        List<double> othertech_val3 = new List<double>();
        //double[] othertech_val3 = new double[200];
        private byte StepRange, SerialState = 0x20, outc_b = 0x20;
        byte[] datas2_0 = new byte[100];
        byte[] datas2_1 = new byte[100];
        byte[] datas2_2 = new byte[100];
        byte[] datas2_state = new byte[100];
        private string staterun = "";
        private Thread thread_grf;
        private delegate void showOnlineData(double v, double i1, double i2, int row);
        private delegate void showDataon(int row);
        private volatile bool _shouldStop = false;//متغیری برای استاپ کردن برنامه در حالتی که فرم به صورت نگهانی بسته شود

        int show_number_runing = 0;///mehrdad   
        public int cycle_number_in_file = 1;//mn

        private double xMinValue, xMaxValue;

        private bool statehc = false;
        private string bpausetxt = "";

        private Series currentSeries;

        private clasmpusbapi usb_fun = new clasmpusbapi();
        private const byte P_C = 0xC8;

        private bool isScrolling = false;

        private drawline_chart drawline1;
        public frmMain2065 mainf;
        public frmChartSetting chartSetting;

        private string statebutton = "";

        //دیباگ مود برای وقتی است که میخواهیم یرای تعمیر دستگاه استپ های ولتاژی را ببینیم
        private bool DebugMode = false;
        private bool Maintenance = false;

        //فعال یا غیرفعال کردن فیلتر آنلاین CV
        public static bool FilterOnline = false;

        //متغیرهای برای تعویض رنگ های استفاده شده در نرم افزار
        Color ChartColor = Color.Black;
        string[] color = File.ReadAllLines(Application.StartupPath + "\\Color.dat");
        Color cl = Color.White;

        //نام گذاری تایتل های نمودار
        string yTitle = "Coulomb(C)";
        string xTitle = "Time(s)";

        //Adding library
        [DllImport("User32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        private static extern IntPtr LoadCursorFromFile(String str);



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

        List<string> ComPortNames(String VID, String PID)//COM این متد وظیفه پیدا کردن و لیست کردن پورت‌های
                                                         //مربوط به یک دستگاه خاص را دارد
                                                         //که با استفاده از شناسه سخت‌افزاری آن دستگاه، یعنی
                                                         //VID (Vendor ID) و PID (Product ID) مشخص می‌شود.
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }

        public bool DeviceIsConnected()
        {
            try
            {
                List<string> names = ComPortNames("1a86", "7523");
                string[] x = SerialPort.GetPortNames();
                foreach (String c in x)
                {
                    if (names.Contains(c))
                    {
                        try
                        {
                            using (mainSerialPort)
                            {
                                mainSerialPort.PortName = c;
                                //frmPortOption2065 frm = new frmPortOption2065();
                                mainSerialPort.BaudRate = 1000000;
                            }
                            return true;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("به دلیل وجود مشکل در ارتباط دستگاه به نرم افزار متصل نشده، لطفا از اتصال صحیح کابل اطمینان حاصل فرمایید"
                                , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("مشکل در ارتباط\n" + e.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
        }

        public frmshowGraph2065()
        {
            InitializeComponent();

            //Set parameter like color & ... for graph
            OnlineChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisX.LineColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisY.LineColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisX.TitleForeColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisY.TitleForeColor = ChartColor;

            OnlineChart.ChartAreas[0].AxisY.Title = yTitle;
            OnlineChart.ChartAreas[0].AxisX.Title = xTitle;

            OnlineChart.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";

            OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            OnlineChart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            OnlineChart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.LineColor = ChartColor;
            this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.LineColor = ChartColor;
            this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.LineColor = ChartColor;
            this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.LineColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisX.ScrollBar.BackColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisY.ScrollBar.BackColor = ChartColor;
            OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);

            panel1.BackColor = cl;
            OnlineChart.BackColor = Color.Transparent;
            OnlineChart.BorderlineColor = Color.Transparent;
            OnlineChart.ChartAreas[0].BackColor = Color.Transparent;
            toolsPanel.BackColor = Color.Transparent;

            try
            {
                // تنظیمات اولیه محورهای چارت
                //OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                //OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

                this.RunnigTechTreeview.ExpandAll();
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
                DeviceIsConnected();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void GraphForm_Enter(object sender, EventArgs e)
        {
            try
            {
                if (mainf != null)
                    mainf.grf1 = sender as frmshowGraph2065;
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtLoger, 2); }
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
                RunnigTechTreeview.SelectedNode = RunnigTechTreeview.Nodes[0].Nodes[row];
                Parameters.Text = "";
                switch (byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()))
                {
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
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) != 9999) Parameters.Text = Parameters.Text + "Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) + "\n";
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

                    case clasglobal.Tafel:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) != 9999) Parameters.Text = Parameters.Text + "HStep =" + double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) + "\n";
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

                    case clasglobal.Battery:
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

                    case clasglobal.CA:
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) != 9999) Parameters.Text = Parameters.Text + "E1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) != 9999) Parameters.Text = Parameters.Text + "T1 =" + double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) != 9999) Parameters.Text = Parameters.Text + "E2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) != 9999) Parameters.Text = Parameters.Text + "T2 =" + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) != 9999) Parameters.Text = Parameters.Text + "Equilibrium Time =" + double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString()) + "\n";
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) != 9999) Parameters.Text = Parameters.Text + " Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) + "\n";
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
                        if (double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) != 9999) Parameters.Text = Parameters.Text + " Cycles =" + double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) + "\n";
                        break;
                }
                Parameters.Text = Parameters.Text + "comment =" + dschart1.chartlist1_run.Rows[row]["comment"].ToString();
            }
        }

        public void Set_Graph(bool overlaymain, int row1)
        {
            Setgrftchart1(overlaymain, 0, 1, 1);//پاک کردن نمودار و افزودن تایتل خالی به نمودار

            Invoke((MethodInvoker)delegate ()
            {
                OnlineChart.Series.Clear();
            });

            Series ChartSeries = new Series();
            ChartSeries.LabelFormat = "#,##0.###########";
            ChartSeries.ChartType = SeriesChartType.Point;
            if (frmChartSetting.ChartType == 'L')
            {
                ChartSeries.ChartType = SeriesChartType.Line;
            }
            else if (frmChartSetting.ChartType == 'C')
            {
                ChartSeries.ChartType = SeriesChartType.Spline;
            }
            else if (frmChartSetting.ChartType == 'P')
            {
                ChartSeries.ChartType = SeriesChartType.Point;
            }

            ChartSeries.Color = frmChartSetting.ChartColor;
            if (ChartSeries.Color == Color.FromArgb(0, 0, 0))
            {
                ChartSeries.Color = Color.DarkCyan;
            }

            Setgrftchart1(overlaymain, row1, 2, 1);// افزودن تایتل محور عمودی و افقی و کلی به نمودار

            AddSeriesToChart(ChartSeries, 1, 1);//اد کردن سری به نمودار

        }

        unsafe private void doRun2_1(int row)
        {
            byte[] data = new byte[3];//دیتا گرفته شده از دستگاه به این ارایه منتقل میشود
            double pos = 0, K = 0;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            int SerialReadError = 0;


            int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());
            int OnePeroid_Time = (int)((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000);
            double size = OnePeroid_Time * ChartScaleMultiplier;
            int cycle = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) * 1000);
            if (TechNumber == 10)
            {
                cycle = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));
            }
            bool state = false;
            if (OnePeroid_Time * cycle > 7200) // اگر تکنیک بیش از دو ساعت زمان بر بود
            {
                state = true;
            }

            bool TechniqueEnded = false;
            int sign = 0;
            shomarchpa = 0;

            int EquilibriumTime = Convert.ToInt32(dschart1.chartlist1_run.Rows[row]["qt"].ToString());
            if (EquilibriumTime > 0)
            {
                EquilibriumTimeDelay(EquilibriumTime);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                mainSerialPort.ReadTimeout = Timeout.Infinite;
                mainSerialPort.DiscardInBuffer();
                OnlineChart.ChartAreas[0].AxisX.Minimum = Double.NaN;//به این دلیل است که در تکنیک های کلومتری مقدار مینیمم جدول تغییر میابد
                OnlineChart.ChartAreas[0].AxisX.Maximum = Double.NaN;//به این دلیل است که در تکنیک های کلومتری مقدار مینیمم جدول تغییر میابد
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                Setbtnenabled(true, 1, 2);
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
                OnlineChart.ChartAreas[0].CursorX.AutoScroll = true;
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // سبک دکمه‌های اسکرول
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;
            });

            if (mainSerialPort.IsOpen == false)
                mainSerialPort.Open();

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 10)//cp
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    OnlineChart.ChartAreas[0].AxisY.Title = "Potenial(mV)";
                    OnlineChart.ChartAreas[0].AxisX.Interval = 0; // تنظیم فاصله مقادیر محور افقی به 1 (فقط اعداد صحیح)
                    OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0.0";
                    if (OnePeroid_Time >= 5)
                        OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "##0"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح
                });

                stopwatch.Start();


                while (SerialState != 0xA0 && !TechniqueEnded && !_shouldStop)//((shomarchpa * ti < Analiz_t3 ) && (SerialState != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        runerror = true;
                        if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                        staterun = "";
                        return;
                    }

                    if (stopwatch.ElapsedMilliseconds > 100)
                    {
                        if (DebugMode) Settxtfill_func(2, $"Ram speed is too slow: Delay {stopwatch.ElapsedMilliseconds}ms", 2, 1);
                    }
                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    stopwatch.Restart();

                    if (data == null)
                    {
                        Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (SerialState != 0xA0) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }
                    //    SerialReadError++;

                    //if (SerialReadError == 5)
                    //{
                    //    MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                    //    return;
                    //}

                    try
                    {
                        data[0] = 6;
                        if (DebugMode)
                        {
                            Settxtfill_func(3, $"{data[0]}, {data[1]}, {data[2]}", 2, 1);
                        }

                        if (TechniqueEnded)
                        {
                            if (DebugMode)
                                Settxtfill_func(2, "Cp TechniqueEnded", 2, 1);
                        }

                        if (SerialState == 0x60) //اگر pause بود 
                        {
                            outc_b = 0x60;
                            continue;
                        }
                        if (SerialState == 0x20 && outc_b == 0x60)//
                        {
                            outc_b = 0x20;
                            continue;
                        }

                        if (shomarchpa >= datas2_state.Length)
                        {
                            Array.Resize(ref datas2_2, datas2_state.Length + 100);
                            Array.Resize(ref datas2_0, datas2_state.Length + 100);
                            Array.Resize(ref datas2_1, datas2_state.Length + 100);
                            Array.Resize(ref datas2_state, datas2_state.Length + 100);
                            for (int l = shomarchpa; l < datas2_state.Length; l++)
                                datas2_state[l] = 2;

                        }
                        datas2_2[shomarchpa] = (byte)(data[0] & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                        datas2_0[shomarchpa] = (byte)data[1];//read porta
                        datas2_1[shomarchpa] = (byte)data[2];//read portb
                        datas2_state[shomarchpa] = 0;
                        if (!TechniqueEnded) showCPdata(row);
                        shomarchpa++;
                        if (OnePeroid_Time * ChartScaleMultiplier * 5 < shomarchpa)
                            switch (state)
                            {
                                case false:
                                    this.Invoke((MethodInvoker)delegate ()
                                    {
                                        K++;
                                        if (K == (5 * SampleRateMultiplier) || SampleRateMultiplier == 0)
                                        {
                                            double scope = time_cha_chc[SampleRateMultiplier * 5] - time_cha_chc[0];
                                            if (SampleRateMultiplier == 0) scope = (double)1 / 5;
                                            pos += scope;
                                            size += scope;
                                            OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                            K = 0;
                                        }
                                    });
                                    break;
                                case true:
                                    this.Invoke((MethodInvoker)delegate ()
                                    {
                                        if (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum >= (OnePeroid_Time * ChartScaleMultiplier))
                                        {
                                            OnlineChart.Series[0].Points.RemoveAt(0); // حذف اولین داده (قدیمی‌ترین داده)
                                        }

                                        // به‌روزرسانی محور X برای نمایش فقط 20 داده اخیر
                                        OnlineChart.ChartAreas[0].AxisX.Minimum = OnlineChart.Series[0].Points[0].XValue;
                                        OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.Series[0].Points[OnlineChart.Series[0].Points.Count - 1].XValue;

                                    });
                                    break;
                                default:
                                    break;
                            }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("CP Technique Could not run correctly" + Ex.Message);
                    }


                    if (SerialState == 0xA0 || TechniqueEnded)//((shomarchpa * ti>= Analiz_T3) || SerialState ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        if (mainSerialPort.IsOpen)
                        {
                            mainSerialPort.Write("end1");
                            mainSerialPort.Close();
                        }

                        Thread.Sleep(1000);
                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "Cp TechniqueEnded", 2, 1);
                        return;
                    }
                }
            }

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 19)//Battery
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    OnlineChart.ChartAreas[0].AxisY.Title = "Potenial(mV)";
                    OnlineChart.ChartAreas[0].AxisX.Interval = 0; // تنظیم فاصله مقادیر محور افقی به 1 (فقط اعداد صحیح)
                    OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0.0";
                    if (OnePeroid_Time >= 5)
                        OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "##0"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح
                });

                stopwatch.Start();


                while (SerialState != 0xA0 && !TechniqueEnded && !_shouldStop)//((shomarchpa * ti < Analiz_t3 ) && (SerialState != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        runerror = true;
                        if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                        staterun = "";
                        return;
                    }

                    if (stopwatch.ElapsedMilliseconds > 100)
                    {
                        if (DebugMode) Settxtfill_func(2, $"Ram speed is too slow: Delay {stopwatch.ElapsedMilliseconds}ms", 2, 1);
                    }
                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    stopwatch.Restart();

                    if (data == null)
                    {
                        Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (SerialState != 0xA0) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }

                    try
                    {
                        data[0] = 6;
                        if (DebugMode)
                        {
                            Settxtfill_func(3, $"{data[0]}, {data[1]}, {data[2]}", 2, 1);
                        }

                        if (TechniqueEnded)
                        {
                            if (DebugMode)
                                Settxtfill_func(2, "Cp TechniqueEnded", 2, 1);
                        }

                        if (SerialState == 0x60) //اگر pause بود 
                        {
                            outc_b = 0x60;
                            continue;
                        }
                        if (SerialState == 0x20 && outc_b == 0x60)//
                        {
                            outc_b = 0x20;
                            continue;
                        }

                        if (shomarchpa >= datas2_state.Length)
                        {
                            Array.Resize(ref datas2_2, datas2_state.Length + 100);
                            Array.Resize(ref datas2_0, datas2_state.Length + 100);
                            Array.Resize(ref datas2_1, datas2_state.Length + 100);
                            Array.Resize(ref datas2_state, datas2_state.Length + 100);
                            for (int l = shomarchpa; l < datas2_state.Length; l++)
                                datas2_state[l] = 2;

                        }
                        datas2_2[shomarchpa] = (byte)(data[0] & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                        datas2_0[shomarchpa] = (byte)data[1];//read porta
                        datas2_1[shomarchpa] = (byte)data[2];//read portb
                        datas2_state[shomarchpa] = 0;
                        if (!TechniqueEnded) showCPdata(row);
                        shomarchpa++;
                        if (OnePeroid_Time * ChartScaleMultiplier * 5 < shomarchpa)
                            switch (state)
                            {
                                case false:
                                    this.Invoke((MethodInvoker)delegate ()
                                    {
                                        K++;
                                        if (K == (5 * SampleRateMultiplier) || SampleRateMultiplier == 0)
                                        {
                                            double scope = time_cha_chc[SampleRateMultiplier * 5] - time_cha_chc[0];
                                            if (SampleRateMultiplier == 0) scope = (double)1 / 5;
                                            pos += scope;
                                            size += scope;
                                            OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                            K = 0;
                                        }
                                    });
                                    break;
                                case true:
                                    this.Invoke((MethodInvoker)delegate ()
                                    {
                                        if (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum >= (OnePeroid_Time * ChartScaleMultiplier))
                                        {
                                            OnlineChart.Series[0].Points.RemoveAt(0); // حذف اولین داده (قدیمی‌ترین داده)
                                        }

                                        // به‌روزرسانی محور X برای نمایش فقط 20 داده اخیر
                                        OnlineChart.ChartAreas[0].AxisX.Minimum = OnlineChart.Series[0].Points[0].XValue;
                                        OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.Series[0].Points[OnlineChart.Series[0].Points.Count - 1].XValue;

                                    });
                                    break;
                                default:
                                    break;
                            }
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show("CP Technique Could not run correctly" + Ex.Message);
                    }


                    if (SerialState == 0xA0 || TechniqueEnded)//((shomarchpa * ti>= Analiz_T3) || SerialState ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        if (mainSerialPort.IsOpen)
                        {
                            mainSerialPort.Write("end1");
                            mainSerialPort.Close();
                        }

                        Thread.Sleep(1000);
                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "Cp TechniqueEnded", 2, 1);
                        return;
                    }
                }
            }

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 11)//CA
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                    OnlineChart.ChartAreas[0].AxisY.Title = "Current(uA)";
                    OnlineChart.ChartAreas[0].AxisX.Interval = 0; // تنظیم فاصله مقادیر محور افقی به 1 (فقط اعداد صحیح)
                    OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0.#"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح
                    if (OnePeroid_Time >= 5)
                        OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح

                    OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                });

                while (SerialState != 0xA0 && !TechniqueEnded && !_shouldStop)//((shomarchpa * ti < Analiz_t3 ) && (SerialState != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        runerror = true;
                        if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                        staterun = "";
                        return;
                    }

                    if (DebugMode) Settxtfill_func(2, $"{stopwatch.ElapsedMilliseconds}", 2, 1);
                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    stopwatch.Restart();

                    if (data == null)
                    {
                        Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (SerialState != 0xA0) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }

                    try
                    {
                        if (DebugMode)
                        {
                            if (DebugMode) Settxtfill_func(2, $"{data[0]}, {data[1]}, {data[2]}", 2, 1);
                        }

                        if (TechniqueEnded)
                            if (DebugMode) Settxtfill_func(2, "CA TechniqueEnded", 2, 1);


                        if (SerialState == 0x60) //اگر pause بود 
                        {
                            outc_b = 0x60;
                            continue;
                        }
                        if (SerialState == 0x20 && outc_b == 0x60)//
                        {
                            outc_b = 0x20;
                            continue;
                        }

                        if ((byte)(data[0] & 0x0f) <= 7)
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
                            datas2_2[shomarchpa] = (byte)(data[0] & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                            datas2_0[shomarchpa] = (byte)data[1];//read porta
                            datas2_1[shomarchpa] = (byte)data[2];//read portb
                            datas2_state[shomarchpa] = 0;
                            if (!TechniqueEnded) showCPdata(row);
                            shomarchpa++;
                            if (OnePeroid_Time * ChartScaleMultiplier * 10 < shomarchpa)
                                switch (state)
                                {
                                    case false:
                                        this.Invoke((MethodInvoker)delegate ()
                                        {
                                            K++;
                                            if (K == (10 * SampleRateMultiplier) || SampleRateMultiplier == 0)
                                            {
                                                double scope = time_cha_chc[SampleRateMultiplier * 10] - time_cha_chc[0];
                                                if (SampleRateMultiplier == 0) scope = (double)1 / 10;
                                                pos += scope;
                                                size += scope;
                                                OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                                //Settxtfill_func(2, $"scope:{scope},pos{pos},size{size},K{K}", 2, 1);
                                                K = 0;
                                            }
                                        });
                                        break;
                                    case true:
                                        this.Invoke((MethodInvoker)delegate ()
                                        {
                                            if (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum >= (OnePeroid_Time * ChartScaleMultiplier))
                                            {
                                                OnlineChart.Series[0].Points.RemoveAt(0); // حذف اولین داده (قدیمی‌ترین داده)
                                            }

                                            // به‌روزرسانی محور X برای نمایش فقط 20 داده اخیر
                                            OnlineChart.ChartAreas[0].AxisX.Minimum = OnlineChart.Series[0].Points[0].XValue;
                                            OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.Series[0].Points[OnlineChart.Series[0].Points.Count - 1].XValue;

                                        });
                                        break;
                                    default:
                                        break;
                                }
                        }

                    }
                    catch (Exception Msg)
                    {
                        MessageBox.Show(Msg.Message);
                    }

                    if (SerialState == 0xA0 || TechniqueEnded)//((shomarchpa * ti>= Analiz_T3) || SerialState ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        TechniqueEnded = false;
                        if (mainSerialPort.IsOpen)
                        {
                            mainSerialPort.Write("end1");
                            mainSerialPort.Close();
                        }
                        Thread.Sleep(1000);
                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running CA", 2, 1);
                        return;
                    }
                }
            }

            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) == 12)//CC
            {
                while (SerialState != 0xA0 && !TechniqueEnded && !_shouldStop)//((shomarchpa * ti < Analiz_t3 ) && (SerialState != 0xa0))   ->  if state run 
                {

                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        runerror = true;
                        if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                        staterun = "";
                        return;
                    }

                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    if (data == null)
                    {
                        Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (SerialState != 0xA0) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }

                    try
                    {
                        if (DebugMode)
                        {
                            Settxtfill_func(2, $"{data[0]}, {data[1]}, {data[2]}", 2, 1);
                        }

                        if (TechniqueEnded)
                        {
                            if (DebugMode)
                                Settxtfill_func(2, "Cp TechniqueEnded", 2, 1);
                        }

                        if (SerialState == 0x60) //اگر pause بود 
                        {
                            outc_b = 0x60;
                            continue;
                        }
                        if (SerialState == 0x20 && outc_b == 0x60)//
                        {
                            outc_b = 0x20;
                            continue;
                        }

                        if ((byte)(data[0] & 0x0f) <= 7)
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
                            datas2_2[shomarchpa] = (byte)(data[0] & 0x0F);//(inprt(P_C) and $0F); در اینجا سه تا ارایه 100 تایی پر می شود(خودم)
                            datas2_0[shomarchpa] = (byte)data[1];//read porta
                            datas2_1[shomarchpa] = (byte)data[2];//read portb
                            datas2_state[shomarchpa] = 0;
                            showdata(row);
                            shomarchpa++;
                            // Settxtfill_func(2, $"TEST1:{shomarchpa * ti},i{i}", 2, 1);
                        }

                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }

                    if (SerialState == 0xA0 || TechniqueEnded)//((shomarchpa * ti>= Analiz_T3) || SerialState ==0xa0)  -> (0xa0 means stop  )  for stop device
                    {
                        TechniqueEnded = false;
                        if (mainSerialPort.IsOpen)
                        {
                            mainSerialPort.Write("end1");
                            mainSerialPort.Close();
                        }
                        //transmit 0x80 to portc in ppi    -> i think 0x80 for accept and answer must be 0x80

                        // inpt = 0x0a;                           // Settxtfill_func(1, $"finish running----{inpt}", 2, 1);
                        Settxtfill_func(1, "finish running CC", 2, 1);
                        Thread.Sleep(100);
                        usb_fun.Outprt(P_C, 0x00); //transmit 0x00 to portc for accept stop
                        usb_fun.ClosePipes();
                        return;
                    }
                }
            }

        }

        private void HandleStopCondition()
        {
            try
            {
                if (mainSerialPort == null)
                {
                    MessageBox.Show("پورت سریال مقداردهی نشده است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!mainSerialPort.IsOpen)
                {
                    Thread.Sleep(10);
                    mainSerialPort.Open();
                }

                // کد های مهم ***
                mainSerialPort.Write("stop");
                Setbtnenabled(true, 2, 1);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("دسترسی به پورت سریال مجاز نیست. لطفاً بررسی کنید که برنامه دیگری از آن استفاده نمی‌کند.",
                                "خطای دسترسی", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Settxtfill_func(4, $"HandleStopCondition UnauthorizedAccessException: {ex.Message}", 2, 1);
            }
            catch (IOException ex)
            {
                MessageBox.Show("خطایی در ارتباط با پورت سریال رخ داده است. لطفاً اتصال سخت‌افزاری را بررسی کنید.",
                                "خطای I/O", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settxtfill_func(4, $"HandleStopCondition IOException: {ex.Message}", 2, 1);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("پورت سریال در وضعیت نامعتبری قرار دارد. لطفاً تنظیمات پورت را بررسی کنید.",
                                "خطای عملیات نامعتبر", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settxtfill_func(4, $"HandleStopCondition InvalidOperationException: {ex.Message}", 2, 1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("یک خطای ناشناخته رخ داده است. لطفاً برنامه را مجدداً اجرا کنید.",
                                "خطای نامشخص", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Settxtfill_func(4, $"HandleStopCondition Exception: {ex.Message}", 2, 1);
            }
            finally
            {
                if (mainSerialPort != null && mainSerialPort.IsOpen)
                {
                    try
                    {
                        mainSerialPort.Close();
                    }
                    catch (Exception ex)
                    {
                        Settxtfill_func(4, $"HandleStopCondition Failed to close the serial port: {ex.Message}", 2, 1);
                    }
                }
            }
        }

        private void EquilibriumTimeDelay(int EquilibriumTimeDelay)
        {
            if (DebugMode)
            {
                Settxtfill_func(3, "EquilibriumTime Started", 2, 1);
            }

            for (int delay = 0; delay < EquilibriumTimeDelay; delay++)//EquilibriumTime
            {
                Thread.Sleep(1000);
                if (SerialState == 0xa0)
                {
                    HandleStopCondition();
                    if (DebugMode)
                    {
                        Settxtfill_func(2, "EquilibriumTime stoped", 2, 1);
                    }
                    return;
                }
            }

            if (DebugMode)
            {
                Settxtfill_func(3, "EquilibriumTime Finished", 2, 1);
            }
        }

        public byte[] ReadSerialData(out bool TechniqueEnded, ref int sign)
        {

            TechniqueEnded = false;
            byte[] data = new byte[3];


            lock (serialLock)
            {
                try
                {
                    int bytesAvailable = mainSerialPort.BytesToRead;
                    int Erorr = 0;
                    while (bytesAvailable == 0)
                    {
                        Thread.Sleep(110);
                        Erorr++;
                        if (Erorr >= 100 || !mainSerialPort.IsOpen)
                        {
                            data[0] = 000;
                            data[1] = 222;
                            data[2] = 111;
                            return data;
                        }
                        bytesAvailable = mainSerialPort.BytesToRead;
                    }

                    if (bytesAvailable > 0)
                    {
                        if (bytesAvailable > 16300)
                        {
                            if (bytesAvailable == 16384)
                                if (DebugMode) Settxtfill_func(1, $"مقدار بافر سریال اورفلو شده است = {bytesAvailable}", 2, 1);
                                else
                                if (DebugMode) Settxtfill_func(1, $"مقدار بافر سریال نزدیک به اورفلو شدن است = {bytesAvailable}", 2, 1);
                        }

                        string receivedData = mainSerialPort.ReadLine();
                        if (string.IsNullOrWhiteSpace(receivedData))
                            return null;

                        if (receivedData.Contains("end"))
                        {
                            TechniqueEnded = true;
                            if (DebugMode) Settxtfill_func(3, "TechniqueEnded", 2, 1);
                        }
                        else if (receivedData.Contains("@"))
                        {
                            string[] parsedData = receivedData.Split('@');
                            if (parsedData.Length < 4)
                                throw new FormatException("Received data is incomplete or malformed.");

                            if (DebugMode) Settxtfill_func(1, $"endpacket: {receivedData}", 2, 1);

                            if (!byte.TryParse(parsedData[1], out data[0]) ||
                                !byte.TryParse(parsedData[2], out data[1]) ||
                                !byte.TryParse(parsedData[3], out data[2]))
                            {
                                throw new FormatException("Failed to parse byte values from received data.");
                            }

                            sign = 1;
                            flag_running = true;

                            if (DebugMode || Maintenance)
                            {
                                foreach (var item in parsedData)
                                {
                                    if (!string.IsNullOrWhiteSpace(item))
                                        Settxtfill_func(1, $"c:{item}", 2, 1);
                                }
                            }
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    if (DebugMode) Settxtfill_func(1, $"ReadSerialData: Timeout error: {ex.Message}", 2, 1);
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }
                catch (FormatException ex)
                {
                    if (DebugMode) Settxtfill_func(1, $"ReadSerialData: Format error: {ex.Message}", 2, 1);
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }
                catch (Exception ex)
                {
                    if (DebugMode) Settxtfill_func(1, $"ReadSerialData: Unexpected error: {ex.Message}", 2, 1);
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }

                return data;
            }

        }


        private void DoRun2(int row)
        {
            byte[] data = new byte[3];//دیتا گرفته شده از دستگاه به این ارایه منتقل میشود
            Int64 j = 0, h = 0;
            bool evn = true, TechniqueEnded = false;
            byte[] Powr = new Byte[4000000];
            double[] Crnt = new double[4000000];
            double[] C = new double[4000000];
            List<byte> Po = new List<byte>();
            double[] Crn = new double[4000000];
            Boolean[] signe = new Boolean[4000000];
            int sign = 0;
            int i = 0;
            double Itot = 0;
            double V = 0, I1 = 0, I2 = 0, Vold;
            int SerialReadError = 0;

            int averagelength = 3;
            double[] Online_filter = new double[averagelength];
            double[] Vold_arrey = new double[averagelength];
            double[] I1_arrey = new double[averagelength];
            double[] I2_arrey = new double[averagelength];
            double[] I1_buffer = new double[2];

            //داده های مرتبط با پارمتر های تکنیک
            int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());
            double E1 = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
            double E2 = double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString());
            int EquilibriumTime = Convert.ToInt32(dschart1.chartlist1_run.Rows[row]["qt"].ToString());
            double E3 = double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString());


            // اینوکر برای اینکه تابعی که در ترد(اصلی) دیگری اجرا می‌شود را به صورت متغییر فراخوانی کنیم
            this.Invoke(new Action(() =>
            {
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                mainSerialPort.ReadTimeout = Timeout.Infinite;
                mainSerialPort.DiscardInBuffer();
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
                Holdbtn.Enabled = false;
            }));

            bool tafel_flag = false;
            if (TechNumber == 18)
            {
                TechNumber = 5;
                tafel_flag = true;
            }

            if (TechNumber < 8)   // if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps
                V = E1;// e1 
            else if (TechNumber == 9)//فقط برای تکنیک CPC
                V = ti;
            Vold = V;

            flag_running = true;

            if (!mainSerialPort.IsOpen)
                mainSerialPort.Open();

            Setbtnenabled(true, 1, 1);//غیرفعال کردن دکمه های Run و...


            if (EquilibriumTime > 0)
            {
                EquilibriumTimeDelay(EquilibriumTime);
            }

            try
            {
                while (!_shouldStop)//برای مواقعی که کاربر بد.ن استپ فرم را میبندد و یا نرم افزار ناگهانی بسته میشود
                {
                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        staterun = "";
                        return;
                    }

                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    if (data == null)
                    {
                        Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (SerialState != 0xA0 && !runerror) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }

                    if (fst) //اگر Tstep از 50 میلی ثانیه کمتر شود 
                    {
                        Powr[j] = data[0];//SAVE IN BUFFER 
                        Crnt[j] = ((data[1] << 6) | (data[2] >> 2)); //???

                        if (Crnt[j] > 0x1FFF)//0x1fff = 8191 
                        {
                            Crnt[j] = (0x4000 - Crnt[j]);//16384 - current ->negative current
                            signe[j] = true;
                        }
                        else
                            signe[j] = false;

                        if (StepRange != 0)//ApplyRangeLimitations
                            if ((TechNumber < 8) && (Crnt[j] > 6560))//if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps && current >6560
                                Crnt[j] = 6560;//current 6560
                        if ((TechNumber == 9) && (Powr[j] == 8)) //if tecno cpc or ccc
                            Crnt[j] = Crnt[j] * 2; //current upto 1A
                    }
                    else
                    {
                        Po.Add(data[0]);//save power in arry po
                        Crn[h] = ((data[1] << 6) | (data[2] >> 2));
                        if (Crn[h] > 0x1FFF)
                        {
                            Crn[h] = (0x4000 - Crn[h]);
                            Crn[h] = Crn[h] * -1;
                        }

                        h++;
                        if (h == 5)
                        {
                            i++;

                            double sum = (Crn[0] * Math.Pow(10, Po[0] - Po[4]) + Crn[1] * Math.Pow(10, Po[1] - Po[4]) + Crn[2] * Math.Pow(10, Po[2] - Po[4]) + Crn[3] * Math.Pow(10, Po[3] - Po[4]) + Crn[4]);
                            Itot = sum / 5;

                            Powr[j] = Po[4];
                            Crnt[j] = Math.Round(Itot, 2);
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
                        if ((Powr[j] == 8) && (TechNumber != 9))
                        {//techno <8 and >9

                            C[j] = Crnt[j] * 1.02207 * Math.Pow(10, Powr[j]);/////// - 6
                        }
                        else
                        {
                            C[j] = Crnt[j] * 0.61035 * Math.Pow(10, Powr[j] - 6); ////- 6  -> for cpc
                                                                                  //Settxtfill_func(2, $"flag1={C[j]},range{Powr[j]} ", 2, 1);
                        }
                        if (signe[j] ^ (TechNumber == 10))
                            C[j] = C[j] * -1;

                        if (TechNumber < 8)//if techno == dcv,npv,dpv,swv,cv,lsv,dcs,dps,
                        {
                            //////mn////////////if (Xinv)
                            //////mn////////////    C[j] = C[j] * -1;//در گراف سی وی اگر ولتاژ از بزرگ به کوچک برود جریان را منفی نمی کند

                            if (pulsy) //if pulsy ==true
                            {
                                if ((TechNumber == 2) || (TechNumber == 1) || (TechNumber == 7))
                                {

                                    if (!evn)//
                                    {
                                        if (ngtv) //if ngtv == true
                                        {
                                            I1 = C[j];
                                            I2 = C[j - 1];
                                        }
                                        else  ////if ngtv == false
                                        {
                                            I1 = C[j];
                                            I2 = C[j - 1];
                                        }

                                    }
                                }
                                else
                                {

                                    if (!evn)//
                                    {
                                        if (ngtv) //if ngtv == true
                                        {
                                            //I1 = C[j];
                                            I1 = C[j - 1];
                                            I2 = C[j];
                                            //Settxtfill_func(2, $"negative2", 2, 1);
                                            //I1 = C[j] - C[j - 1];
                                            //I2 = 0;
                                        }
                                        else  ////if ngtv == false
                                        {

                                            I2 = C[j];
                                            I1 = C[j - 1];
                                            //Settxtfill_func(2, $"positive2", 2, 1);
                                            //I1 = C[j - 1] - C[j];
                                            //I2 = 0;
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
                                Po.Clear();
                                S_et = false;
                            }

                            j++;
                            evn = !evn;

                        }
                        else
                        {
                            //techno ccc,cpc,chp,cha,chc

                            if (TechNumber == 12)//if techno ==chc
                                I1 = I1 + C[j] * ti;
                            else//techno ...
                                I1 = C[j]; // Addxy(V1,C[j],'',Colr[cl]);
                            I2 = 0;
                            Vold = V;
                            V += ti;
                            j++;
                        }

                        if (pulsy)
                        {
                            if (evn)
                            {

                                Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی

                                if (DebugMode || Maintenance)//for monitoring purpse
                                {
                                    Settxtfill_func(2, $"{Vold}:{I1}:{I2} ", 2, 1);
                                    Settxtfill_func(2, $"vold1{Vold}", 2, 1);
                                }
                            }
                        }
                        else
                        {
                            if (tafel_flag)
                            {
                                if (I1 == 0)
                                    I1 = 0.0001; // جلوگیری از خطای لگاریتم

                                double logI = Math.Log10(Math.Abs(I1));

                                Runnig2(Vold, logI, I2, row);///// رسم گراف اصلی
                            }
                            else
                                Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی

                            if (DebugMode || Maintenance)
                            {
                                Settxtfill_func(2, $"{Vold}:{I1}:{I2} ", 2, 1);
                                Settxtfill_func(2, $"vold1{Vold}", 2, 1);
                            }
                        }

                    }

                    if (TechniqueEnded)  //if portc=0x09
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            mainSerialPort.Write("end1");
                            Thread.Sleep(10);
                        }
                        TechniqueEnded = false;
                        mainSerialPort.Close();
                        Thread.Sleep(100);
                        Setbtnenabled(true, 2, 1);
                        return;
                    }

                    if (SerialState == 0xA0)//if software == stoped 
                    {
                        HandleStopCondition();
                        Settxtfill_func(3, "finish running DoRun2", 2, 1);
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {  
                MessageBox.Show(Ex.Message);
            }

        }

        private void DoRun2CV(int row)
        {
            byte[] data = new byte[3];//دیتا گرفته شده از دستگاه به این ارایه منتقل میشود
            Int64 j = 0, h = 0;
            bool evn = true, E_nd = false, back_direction = false, TechniqueEnded = false;
            byte[] Powr = new Byte[4000000];
            double[] Crnt = new double[4000000];
            double[] C = new double[4000000];
            List<byte> Po = new List<byte>(5);
            double[] Crn = new double[4000000];
            Boolean[] signe = new Boolean[4000000];
            int sign = 0;
            int SerialReadError = 0;

            double Itot = 0;
            double V = 0, I1 = 0, I2 = 0, Vold;
            int i = 0;


            //داده های مرتبط با پارمتر های تکنیک
            int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());
            double E1 = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
            double E2 = double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString());
            int EquilibriumTime = Convert.ToInt32(dschart1.chartlist1_run.Rows[row]["qt"].ToString());
            double E3 = double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString());
            int Cycle = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString());

            double deltaRange = Math.Abs(E2 - E1);

            // اینوکر برای اینکه تابعی که در ترد(اصلی) دیگری اجرا می‌شود را به صورت متغییر فراخوانی کنیم
            this.Invoke(new Action(() =>
            {
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                mainSerialPort.ReadTimeout = Timeout.Infinite;
                mainSerialPort.DiscardInBuffer();
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
            }));

            if (TechNumber < 8)   // if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps
                V = E1;// e1 
            else if (TechNumber == 9)//فقط برای تکنیک CPC
                V = ti;
            Vold = V;

            flag_running = true;

            I2 = 0;

            bool fst_New = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 < 50) || (Analiz_St1 < 1);
            bool S_et_New = false;


            if (!mainSerialPort.IsOpen)
                mainSerialPort.Open();

            if (EquilibriumTime > 0)
                EquilibriumTimeDelay(EquilibriumTime);


            try
            {
                Setbtnenabled(true, 1, 1);//غیرفعال کردن دکمه های Run و...

                while (!_shouldStop)//تا زمانی که دستور پایان را از دستگاه نگیرد میچرخد
                {
                    if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                    {
                        runerror = true;
                        staterun = "";
                        return;
                    }

                    data = ReadSerialData(out TechniqueEnded, ref sign);
                    while (data == null)
                    {
                        data = ReadSerialData(out TechniqueEnded, ref sign);
                    }
                    //if (data == null)
                    //{
                    //    Settxtfill_func(2, "داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", 2, 1);
                    //    SerialReadError++; 
                    //}
                    if (data.SequenceEqual(new byte[] { 000, 222, 111 }) && SerialState != 0xA0)
                    {
                        if (SerialState != 0xA0) MessageBox.Show("تکنیک به درستی اجرا نمیشود");
                        return;
                    }

                    if (fst_New) //اگر Tstep از 50 میلی ثانیه کمتر شود 
                    {
                        Powr[j] = data[0];//SAVE IN BUFFER 
                        Crnt[j] = ((data[1] << 6) | (data[2] >> 2)); //???

                        if (Crnt[j] > 0x1FFF)//0x1fff = 8191 
                        {
                            Crnt[j] = (0x4000 - Crnt[j]);//16384 - current ->negative current
                            signe[j] = true;
                        }
                        else
                            signe[j] = false;

                        if (StepRange != 0)//ApplyRangeLimitations
                            if ((TechNumber < 8) && (Crnt[j] > 6560))//if techno dcv,npv,dpv,swv,cv,lsv,dcs,dps && current >6560
                                Crnt[j] = 6560;//current 6560
                        if ((TechNumber == 9) && (Powr[j] == 8)) //if tecno cpc or ccc
                            Crnt[j] = Crnt[j] * 2; //current upto 1A

                        S_et_New = true;
                    }
                    else
                    {
                        if (data == null)
                        {
                            continue;
                        }
                        Po.Add(data[0]);//save power in arry po
                        Crn[h] = ((data[1] << 6) | (data[2] >> 2));
                        if (Crn[h] > 0x1FFF)
                        {
                            Crn[h] = (0x4000 - Crn[h]);
                            Crn[h] = Crn[h] * -1;
                        }

                        h++;
                        if (h == 5)
                        {
                            i++;


                            double sum = (Crn[0] * Math.Pow(10, Po[0] - Po[4]) + Crn[1] * Math.Pow(10, Po[1] - Po[4]) + Crn[2] * Math.Pow(10, Po[2] - Po[4]) + Crn[3] * Math.Pow(10, Po[3] - Po[4]) + Crn[4]);
                            Itot = sum / 5;

                            Powr[j] = Po[4];
                            Crnt[j] = Math.Round(Itot, 2);
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

                            S_et_New = true;
                        }
                    }

                    if (S_et_New)
                    {

                        if ((Powr[j] == 8) && (TechNumber != 9))
                        {//techno <8 and >9

                            C[j] = Crnt[j] * 1.02207 * Math.Pow(10, Powr[j]);/////// - 6
                        }
                        else
                        {
                            C[j] = Crnt[j] * 0.61035 * Math.Pow(10, Powr[j] - 6); ////- 6  -> for cpc
                                                                                  //Settxtfill_func(2, $"flag1={C[j]},range{Powr[j]} ", 2, 1);
                        }
                        if (signe[j] ^ (TechNumber == 10))
                            C[j] = C[j] * -1;


                        I1 = C[j];

                        string Hstep = dschart1.chartlist1_run.Rows[row][6].ToString();
                        int numdigit = Hstep.Length - Hstep.IndexOf('.') - 1;// calculate digit number in of heght step
                        if (!(pulsy && evn))
                        {
                            Vold = V;
                            if (N_gtv)
                            {
                                V = Math.Round(V - double.Parse(Hstep), numdigit);
                            }
                            else
                            {
                                V = Math.Round(V + double.Parse(Hstep), numdigit);
                            }
                        }
                        evn = !evn;
                        j++;


                        this.Invoke((MethodInvoker)delegate ()
                        {
                            OnlineChart.Series[countcv].Points.AddXY(Vold, I1);///// رسم گراف اصلی


                            VoltageStepArray.Add(Vold);//حاوی استپ های ولتاژی
                            othertech_val3.Add(I1);

                            shomar_othertech++;

                            //double percent = (Vold * 100) / (deltaRange * 2);  
                            mainf.toolStripProgressBar1.Value++;
                            if (mainf.toolStripProgressBar1.Value == 100)
                                mainf.toolStripProgressBar1.Value = 0;

                        });

                        if (DebugMode || Maintenance)
                        {
                            Settxtfill_func(2, $"{Vold}:{I1}:{I2}", 2, 1);
                        }

                        if (!fst_New)
                        {
                            h = 0;
                            Po.Clear();
                            S_et_New = false;
                        }



                        if (TechNumber == 4)// if techno ==   cv
                        {
                            if (E1 < E2) //if (e1<e2)
                            {
                                if (V >= E2) // if v>e2
                                {
                                    back_direction = true;
                                    N_gtv = !N_gtv;
                                }

                                if (V < E3 && back_direction)
                                {
                                    Cycle = Cycle - 1;// cycle = cycle-1
                                    if (Cycle >= 0) //if cycle >= 0
                                    {
                                        E_nd = true;
                                        countcv++;
                                        N_gtv = !N_gtv;
                                    }
                                }
                            }
                            else//if e1>e2
                            {
                                if (V <= E2)//if v<=e2
                                {
                                    back_direction = true;
                                    N_gtv = !N_gtv;
                                }

                                if (V > E3 && back_direction)
                                {
                                    Cycle = Cycle - 1;
                                    if (Cycle >= 0)
                                    {
                                        E_nd = true;
                                        countcv++;
                                        N_gtv = !N_gtv;
                                    }
                                }
                            }
                        }

                        if (E_nd)
                        {
                            double temp1, temp4;
                            temp1 = VoltageStepArray[shomar_othertech - 1];
                            temp4 = othertech_val3[shomar_othertech - 1];

                            StartNewSeries();
                            newSmoth(countcv - 1);
                            FadeOldSeries();
                            transferdata(row, 1);


                            this.Invoke((MethodInvoker)delegate ()
                            {
                                mainf.TechStatus.Text = (RunnigTechTreeview.SelectedNode.Index + 1).ToString().Trim() + "_" + countcv.ToString() + " - Running technique " + RunnigTechTreeview.Nodes[0].Nodes[RunnigTechTreeview.SelectedNode.Index].Text;

                            });
                            //cv_stop = false;

                            if (Cycle > 0)//cycle >0
                            {
                                shomar_othertech = 1;
                                VoltageStepArray.Clear();
                                othertech_val3.Clear();
                                VoltageStepArray.Add(temp1);
                                othertech_val3.Add(temp4);
                                //Setgrfnodetext(row, 4, 1);
                            }

                            if (N_gtv)
                            {
                                V = E1 - double.Parse(Hstep);//v =e1-hs
                            }
                            else
                            {
                                V = E1 + double.Parse(Hstep);//v=e1+hs
                            }

                            E_nd = false;
                            back_direction = false;
                        }

                    }

                    if (TechniqueEnded)  //if portc=0x09
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            mainSerialPort.Write("end1");
                            Thread.Sleep(10);
                        }
                        TechniqueEnded = false;
                        mainSerialPort.Close();
                        Thread.Sleep(100);
                        Setbtnenabled(true, 2, 1);
                        return;
                    }

                    if (SerialState == 0xA0)//if software == stoped 
                    {
                        HandleStopCondition();
                        Settxtfill_func(3, "finish running DoRun2", 2, 1);
                        return;
                    }
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void StartNewSeries()
        {
            currentSeries = new Series()
            {
                ChartType = OnlineChart.Series[0].ChartType,
                Color = Color.DarkCyan,
                BorderWidth = OnlineChart.Series[0].BorderWidth,
                MarkerStyle = OnlineChart.Series[0].MarkerStyle
            };

            this.Invoke((MethodInvoker)delegate ()
            {
                OnlineChart.Series.Add(currentSeries);
            });
        }

        private void FadeOldSeries()
        {
            foreach (var series in OnlineChart.Series)
            {
                if (series == currentSeries)
                    continue;

                Color oldColor = series.Color;
                int newAlpha = Math.Max(40, oldColor.A - 40); // کم‌کم کمرنگ کن
                this.Invoke((MethodInvoker)delegate ()
                {
                    series.Color = Color.FromArgb(newAlpha, oldColor.R, oldColor.G, oldColor.B);
                });
            }
        }

        unsafe private void dorun3(int row)
        {
            double V = 0, I1 = 0, I2 = 0;
            Int64 j = 0;
            bool evn = true;
            Boolean strt = true;

            byte[] datas = new byte[3];
            byte[] Powr = new Byte[32710];
            double[] Crnt = new double[32710];
            double[] C = new double[32710];
            Boolean[] signe = new Boolean[32710];
            int counter = 0, number = 0, sign = 0, end = 0;
            String string1;
            mainSerialPort.ReadTimeout = Timeout.Infinite;
            mainSerialPort.DiscardInBuffer();
            int EquilibriumTime = Convert.ToInt32(dschart1.chartlist1_run.Rows[row]["qt"].ToString());

            // اینوکر برای اینکه تابعی که در ترد(اصلی) دیگری اجرا می‌شود را به صورت متغییر فراخوانی کنیم
            this.Invoke(new Action(() =>
            {
                OnlineChart.ChartAreas[0].AxisX.Minimum = Double.NaN;//به این دلیل است که در تکنیک های کلومتری مقدار مینیمم جدول تغییر میابد
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                mainSerialPort.ReadTimeout = Timeout.Infinite;
                mainSerialPort.DiscardInBuffer();
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
                Holdbtn.Enabled = false;
            }));

            if (EquilibriumTime > 0)
            {
                EquilibriumTimeDelay(EquilibriumTime);
            }

            strt = true;
            if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) < 8)
                V = double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
            else if (double.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()) >= 9)
                V = ti;
            while (!_shouldStop)
            {
                if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                {
                    runerror = true;
                    if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                    staterun = "";
                    return;
                }

                string1 = null;
                string[] strings = new string[4];
                /////Setbtnenabled(true, 1, 2);
                // serialPort2.WriteLine("start");
                number = mainSerialPort.BytesToRead;

                try
                {

                    if (number > 0)
                    {

                        string1 = mainSerialPort.ReadLine();


                        //DataReceived.Remove(DataReceived.Length-1);

                        if (sign == 0)
                        {
                            if (string1.Contains("end"))
                            {
                                end = 1;
                                string1 = "";
                                Settxtfill_func(2, "TechniqueEnded", 2, 1);
                            }
                            else if (string1.Contains("@"))
                            {
                                strings = string1.Split('@');

                                datas[2] = (byte)Int32.Parse(strings[1]);
                                datas[0] = (byte)Int32.Parse(strings[2]);
                                datas[1] = (byte)Int32.Parse(strings[3]);
                                //counter = (int)Int32.Parse(strings[4]);

                                //Settxtfill_func(2, $"c:{datas[2]},i{i}", 2, 1);
                                //Settxtfill_func(2, $"a:{datas[0]},i{i}", 2, 1);
                                //Settxtfill_func(2, $"b:{datas[1]},i{i}", 2, 1);


                                counter++;
                                sign = 1;
                                number = 0;
                                flag_running = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show($"dorun3: {Ex.Message} ");
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
                                I1 = C[j - 1] - C[j];
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

                }
                else if (end == 1)  //if portc=0x09
                {
                    for (int z = 0; z < 5; z++)
                    {
                        mainSerialPort.Write("end1");
                        Thread.Sleep(10);
                    }
                    mainSerialPort.Close();
                    Thread.Sleep(1000);                    //////////////////////////////////
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
                        // isSet_Grid_Cells = true;
                        //  showMessageError = true;
                        OnlineChart.Series[num_Series].Label = this.dschart1.chartlist1_run.Rows[row]["Tch"].ToString();

                        Setgrfnodeselect(row, 1, 1);

                        long k = 0;
                        while (k < 10) k++;
                        num_cycles = 1;   // (int)cycles;
                        Thread.Sleep(int.Parse(this.dschart1.chartlist1_run.Rows[row][7].ToString()));
                        //EquilibriumTime = 0;
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

                if (SerialState == 0xA0)//if software ==pause 
                {
                    mainSerialPort.Write("stop");
                    mainSerialPort.Close();
                    System.Threading.Thread.Sleep(1000);
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, "finish running-2", 2, 1);
                    return;
                }

                Chart2 = this.OnlineChart;

            } //while true              
        }

        unsafe private void dorun2_costum(int row)//CD
        {
            string ReadLineString;
            byte end = 0;
            double counter = 0, number = 0;
            double pos = 0, K = 0;
            int Cd_Counter = 0;


            double size = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 2000;
            int time_Of_Period_cd = (int)((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000);
            int cycle = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));
            int EquilibriumTime = Convert.ToInt32(dschart1.chartlist1_run.Rows[row]["qt"].ToString());

            bool state = false;
            if (time_Of_Period_cd * cycle > 7200) // اگر تکنیک بیش از دو ساعت زمان بر بود
            {
                state = true;
            }

            int sign = 0;
            double Vold, I1;
            //double[] I1 = new double [2]{ 0, 0 };

            int DataPerSecond = 10;
            if (time_Of_Period_cd * cycle > 500)
                DataPerSecond = 2;

            mainSerialPort.ReadTimeout = Timeout.Infinite;


            if (EquilibriumTime > 0)
            {
                EquilibriumTimeDelay(EquilibriumTime);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                OnlineChart.ChartAreas[0].AxisY.Title = "Potenial(mV)";
                OnlineChart.ChartAreas[0].AxisX.Interval = 0; // تنظیم فاصله مقادیر محور افقی به 1 (فقط اعداد صحیح)
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0.0";
                if (time_Of_Period_cd >= 5)
                    OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "##0"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح

                OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
                OnlineChart.ChartAreas[0].CursorX.AutoScroll = true;
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // سبک دکمه‌های اسکرول
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

                OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                OnlineChart.ChartAreas[0].AxisX.Maximum = Double.NaN;
                Series RaiseTime = new Series("RaiseTime") { Color = Color.Green, ChartType = OnlineChart.Series[0].ChartType };
                OnlineChart.Series.Add(RaiseTime);
                Series FallTime = new Series("FallTime") { Color = Color.Red, ChartType = OnlineChart.Series[0].ChartType };
                OnlineChart.Series.Add(FallTime);
            });

            flag_running = true;
            int volt1 = 0;
            mainSerialPort.DiscardInBuffer();

            while (!_shouldStop)//تا زمانی که دستور پایان را از دستگاه نگیرد میچرخد
            {
                if (staterun.Trim() == "Form Closed")// برای وقتی که فرم یهویی بسته میشه
                {
                    runerror = true;
                    if (DebugMode) Settxtfill_func(2, "Form Closed", 2, 1);
                    staterun = "";
                    return;
                }
                ReadLineString = null;
                string[] strings = new string[4];

                try
                {
                    number = mainSerialPort.BytesToRead;
                    if (number > 0)
                    {
                        ReadLineString = mainSerialPort.ReadLine();

                        if (sign == 0)
                        {

                            if (ReadLineString.Contains("end14"))
                            {
                                end = 1;
                                ReadLineString = "";
                                Settxtfill_func(2, "TechniqueEnded", 2, 1);
                                sign = 1;
                            }
                            else
                            {
                                strings = ReadLineString.Split('@');

                                //counter = Math.Round((Math.Round(double.Parse(strings[1]) * 10) / 10));
                                counter = double.Parse(strings[1]);
                                volt1 = int.Parse(strings[2]);
                                if (DebugMode || Maintenance)
                                    Settxtfill_func(2, $"voltage:{volt1}", 2, 1);

                                sign = 1;
                                number = 0;
                                flag_running = true;
                                ReadLineString = "";
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

                if (sign == 1)
                {
                    Vold = counter;
                    I1 = volt1;

                    if (SampleRateMultiplier != 0)
                    {
                        if (Cd_Counter % (DataPerSecond * SampleRateMultiplier) == 0)
                        {
                            RunnigCD(Vold, I1, 0, row);///// رسم گراف اصلی
                        }
                    }
                    else
                        RunnigCD(Vold, I1, 0, row);///// رسم گراف اصلی


                    if (time_Of_Period_cd * ChartScaleMultiplier * DataPerSecond < Cd_Counter)
                        switch (state)
                        {
                            case false:
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    K++;
                                    if (K == (DataPerSecond * SampleRateMultiplier) || SampleRateMultiplier == 0)
                                    {
                                        double scope = SampleRateMultiplier;
                                        if (SampleRateMultiplier == 0) scope = (double)1 / DataPerSecond;
                                        pos += scope;
                                        size += scope;
                                        OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                        K = 0;
                                    }
                                });

                                break;
                            case true:
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    if (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum >= (time_Of_Period_cd * ChartScaleMultiplier))
                                    {
                                        OnlineChart.Series[1].Points.RemoveAt(0); // حذف اولین داده (قدیمی‌ترین داده)
                                    }

                                    OnlineChart.ChartAreas[0].AxisX.Minimum = OnlineChart.Series[1].Points[0].XValue;
                                    OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.Series[1].Points[OnlineChart.Series[1].Points.Count - 1].XValue;

                                });
                                break;
                            default:
                                break;
                        }

                    Cd_Counter++;
                    sign = 0;

                }

                if (end == 1)  //if portc=0x09
                {
                    end = 0;
                    mainSerialPort.Write("end1");
                    System.Threading.Thread.Sleep(2000);
                    mainSerialPort.Close();
                    // System.Threading.Thread.Sleep(1000 * Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["qt"].ToString()));//delay with equlibirum 
                    // System.Threading.Thread.Sleep(int.Parse(this.dschart1.chartlist.Rows[row][7].ToString()));
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, $"finish running-1 :{end} :{ReadLineString}", 2, 1);
                    return;
                }

                if (SerialState == 0xA0)//if software ==pause 
                {
                    if (mainSerialPort.IsOpen)
                    {
                        mainSerialPort.Write("stop");
                        System.Threading.Thread.Sleep(1000);
                        mainSerialPort.Close();
                    }
                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, "finish running-2", 2, 1);
                    return;
                }
            }
        }

        private void dorun2_costum(int row, int New_way)//CD
        {
            string[] strings = new string[4];
            double I1 = 0, I2 = 0, Vold;
            double counter = 0;
            int volt1 = 0;
            bool endFlag = false;
            double E1 = double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString());
            double E2 = double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString());
            int Cycle = int.Parse(dschart1.chartlist1_run.Rows[row][12].ToString());
            int T1 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);
            double T1Scale = T1 * 100;// T1Scale = T1/10(s) * 1000 = ms
            int T2 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
            double T2Scale = T2 * 100;// T1Scale = T1/10(s) * 1000 = ms
            int time_Of_Period_cd = (int)((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000);
            string response = null;
            string send = null, send1 = null, send2 = null;


            this.BeginInvoke(new Action(() =>//ست آپ اولیه‌ی چارت
            {
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                OnlineChart.ChartAreas[0].AxisY.Title = "Potenial(mV)";
                OnlineChart.ChartAreas[0].AxisX.Interval = 0; // تنظیم فاصله مقادیر محور افقی به 1 (فقط اعداد صحیح)
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "0.0";
                if (time_Of_Period_cd >= 5)
                    OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "##0"; // قالب‌بندی برچسب‌ها به صورت عدد صحیح

                OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                OnlineChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Seconds;
                OnlineChart.ChartAreas[0].CursorX.AutoScroll = true;
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // سبک دکمه‌های اسکرول
                OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;

                OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                OnlineChart.ChartAreas[0].AxisX.Maximum = Double.NaN;
            }));


            //تاخیر بابت ایجاد Equbrilum Time
            for (int delay = 0; delay < Convert.ToInt32(this.dschart1.chartlist1_run.Rows[row]["qt"].ToString()); delay++)
            {
                System.Threading.Thread.Sleep(1000);
                if (SerialState == 0xa0)
                {
                    Settxtfill_func(2, "stop", 2, 1);
                    mainSerialPort.Write("stop");
                    mainSerialPort.Close();
                    Setbtnenabled(true, 2, 1);
                    return;
                }
            }

            while (true)
            {
                if (mainSerialPort.BytesToRead > 0)//اگر دیتایی بر روی پورت سریال مشاهده کرد از حلقه خارج میشود
                {
                    mainSerialPort.DiscardInBuffer();//داده های اولی اشتباه هستن
                    break;
                }
                else
                {
                    Thread.Sleep(100); // توقف کوتاه برای کاهش بار پردازشی
                }
            }

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            // تعریف تایمر با فاصله T1Scale میلی‌ثانیه
            System.Timers.Timer timer = new System.Timers.Timer(T1Scale);

            // فعال کردن تایمر
            timer.AutoReset = true;
            timer.Enabled = true;
            bool isTimerRunning = false;

            double VritualT = T1 * 1000;//Second ==> MSecond
            bool flag = true;
            send1 = "14" + "@" + E1.ToString() + "@" + T1Scale.ToString() + ".";
            send2 = "14" + "@" + E2.ToString() + "@" + T2Scale.ToString() + ".";
            send = send1;
            mainSerialPort.NewLine = "#";

            //timer.AutoReset = false;

            mainSerialPort.Close();
            // تنظیم رویداد برای اجرا در هر بار تکرار تایمر
            timer.Elapsed += async (sender, e) =>
            {
                if (isTimerRunning)
                {
                    Settxtfill_func(2, "conflict", 2, 1);
                    return; // جلوگیری از اجرای همزمان
                }

                isTimerRunning = true;
                stopwatch.Restart();

                try
                {
                    if (mainSerialPort.IsOpen == false)
                    {
                        mainSerialPort.Open();
                        await Task.Delay(5);
                    }

                    mainSerialPort.DiscardOutBuffer();
                    mainSerialPort.Write(send);
                    Settxtfill_func(1, $"{send}", 2, 1);
                    while (mainSerialPort.BytesToRead == 0)
                    {

                    }
                    stopwatch.Stop();
                    Settxtfill_func(2, $"{stopwatch.ElapsedMilliseconds} ms", 2, 1);


                    if (flag)// نیم سیکل مثبت
                    {
                        try
                        {
                            //response = await Task.Run(() => serialPort2.ReadLine());
                            response = mainSerialPort.ReadExisting();
                            Settxtfill_func(1, $"{response}", 2, 1);


                            if (!string.IsNullOrEmpty(response))
                            {
                                strings = response.Split('@');
                                int.TryParse(strings[2], out volt1);
                                Vold = counter;
                                I1 = volt1;
                                Runnig2(Vold, I1, I2, row); // رسم گراف اصلی
                                counter = Math.Round(counter + (T1Scale / 1000), 1);
                                mainSerialPort.DiscardInBuffer();
                                VritualT = VritualT - T1Scale;
                                //Settxtfill_func(2, $"{VritualT}", 2, 1);
                                Settxtfill_func(1, $"{VritualT}f1", 2, 1);

                            }
                            else
                            {
                                Settxtfill_func(3, "nullhapen1", 2, 1);
                                mainSerialPort.DiscardInBuffer();
                            }
                            //serialPort2.Close();

                            //await Task.Run(() =>
                            //{
                            //    // اکشن اول: خواندن اطلاعات از سریال پورت
                            //    response = serialPort2.ReadExisting();
                            //    //Settxtfill_func(2, $"{response}", 2, 1);
                            //    if (!string.IsNullOrEmpty(response))
                            //    {
                            //        strings = response.Split('@');
                            //        int.TryParse(strings[2], out volt1);
                            //        counter = Math.Round(counter + (T1Scale / 1000), 1);
                            //        Vold = counter;
                            //        I1 = volt1;
                            //        Runnig2(Vold, I1, I2, row); // رسم گراف اصلی
                            //        serialPort2.DiscardInBuffer();
                            //        VritualT = VritualT - T1Scale;
                            //        Settxtfill_func(2, $"{VritualT}", 2, 1);
                            //    }
                            //});
                            //Thread.Sleep(20);

                        }
                        catch (TimeoutException)
                        {
                            Vold = counter;
                            Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                            counter = Math.Round(counter + (T1Scale / 1000), 1);
                            Settxtfill_func(3, "state:31", 2, 1);
                            VritualT = VritualT - T1Scale;
                        }

                    }
                    else
                    {
                        try
                        {
                            //response = await Task.Run(() => serialPort2.ReadLine());
                            lock (mainSerialPort)
                            {
                                response = mainSerialPort.ReadExisting();
                                Settxtfill_func(1, $"{response}", 2, 1);
                            }

                            if (!string.IsNullOrEmpty(response))
                            {
                                strings = response.Split('@');
                                int.TryParse(strings[2], out volt1);
                                Vold = counter;
                                I1 = volt1;
                                Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                                counter = Math.Round(counter + (T2Scale / 1000), 1);
                                mainSerialPort.DiscardInBuffer();
                                VritualT = VritualT - T2Scale;
                                Settxtfill_func(1, $"{VritualT}f2", 2, 1);
                            }
                            else
                            {
                                Settxtfill_func(3, "null hapen2", 2, 1);
                                mainSerialPort.DiscardInBuffer();
                            }
                            //serialPort2.Close();

                            //await Task.Run(() =>
                            //{
                            //    response = serialPort2.ReadExisting();
                            //    //Settxtfill_func(2, $"{response}", 2, 1);
                            //    if (!string.IsNullOrEmpty(response))
                            //    {
                            //        strings = response.Split('@');
                            //        int.TryParse(strings[2], out volt1);
                            //        counter = Math.Round(counter + (T2Scale / 1000), 1);
                            //        Vold = counter;
                            //        I1 = volt1;
                            //        Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                            //        serialPort2.DiscardInBuffer();
                            //        VritualT = VritualT - T2Scale;
                            //        Settxtfill_func(2, $"{VritualT}", 2, 1);
                            //    }
                            //    else
                            //        Settxtfill_func(2, "null hapen", 2, 1);
                            //});
                            //Thread.Sleep(20);
                        }
                        catch (TimeoutException)
                        {
                            Vold = counter;
                            Runnig2(Vold, I1, I2, row);///// رسم گراف اصلی
                            counter = Math.Round(counter + (T2Scale / 1000), 1);
                            Settxtfill_func(3, "state:32", 2, 1);

                            VritualT = VritualT - T2Scale;
                        }
                    }

                    //timer.Start(); // راه‌اندازی دوباره تایمر
                    if (VritualT == 0)
                    {
                        flag = !flag;
                        Settxtfill_func(1, "Flag", 2, 1);
                        if (flag)
                        {
                            VritualT = T1 * 1000;// تبدیل به میلی ثانیه
                            send = send1;
                            timer.Interval = T1Scale;
                            Cycle--;
                            if (Cycle == 0)
                            {
                                //serialPort2.Open();
                                mainSerialPort.NewLine = "\n";
                                mainSerialPort.Write("end1");
                                Settxtfill_func(2, "Technique Finished", 2, 1);
                                Setbtnenabled(true, 2, 1);
                                endFlag = true;
                                //serialPort2.Close();
                                timer.Stop();
                                timer.Dispose();
                                return;
                            }
                        }
                        else
                        {
                            VritualT = T2 * 1000;// تبدیل به میلی ثانیه
                            send = send2;
                            timer.Interval = T2Scale;
                        }
                    }
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //}));

                }
                catch (InvalidOperationException)
                {
                    if (mainSerialPort != null)
                    {
                        if (mainSerialPort.IsOpen)
                            mainSerialPort.Close();
                        timer.Stop();
                        timer.Dispose();
                        return;
                    }
                    Settxtfill_func(2, "InvalidOperationException", 2, 1);

                }
                catch (Exception Ex)
                {
                    //MessageBox.Show(Ex.Message);
                    Settxtfill_func(2, $"InvalidOperationException {Ex.Message}", 2, 1);
                    throw;
                }
                finally
                {
                    isTimerRunning = false;
                }

            };




            while (!endFlag)
            {
                if (staterun.Trim() == "Form Closed")//اگر فرم بسته شد
                {
                    runerror = true;
                    staterun = "";
                    break;
                }

                if (SerialState == 0xa0)//اگر دکمه استوپ زده شد
                {
                    Settxtfill_func(2, "stop", 2, 1);
                    timer.Enabled = false;
                    mainSerialPort.Write("stop");
                    mainSerialPort.Close();
                    Setbtnenabled(true, 2, 1);
                    return;
                }
                //await Task.Delay();
                Thread.Sleep(5);


                //if (stopwatch.ElapsedMilliseconds >= 100)
                //{
                //    stopwatch.Restart();

                //    Settxtfill_func(2, "stopwatch:100ms", 2, 1);

                //}
                //Thread.Sleep(1);
            }

        }

        unsafe private void OCP_Reading(int row)//OCP
        {

            string string1;
            byte end = 0;
            int sign = 0;
            float counter = 0;

            flag_running = true;
            mainSerialPort.ReadTimeout = Timeout.Infinite;
            float volt1 = 0;

            mainSerialPort.DiscardInBuffer();

            for (; ; )//تا زمانی که دستور پایان را از دستگاه نگیرد میچرخد
            {
                if (staterun.Trim() == "Form Closed")
                {
                    runerror = true;
                    staterun = "";
                    break;
                }
                string1 = null;
                string[] strings = new string[4];
                float number = mainSerialPort.BytesToRead;

                try
                {
                    if (number > 0)
                    {
                        string1 = mainSerialPort.ReadLine();
                        if (sign == 0)
                        {
                            if (string1.Contains("end15"))
                            {
                                end = 1;
                                Settxtfill_func(2, "TechniqueEnded", 2, 1);
                            }
                            else
                            {
                                strings = string1.Split('@');

                                counter = float.Parse(strings[1]);
                                volt1 = Int32.Parse(strings[2]);

                                Settxtfill_func(2, $"voltage:{volt1}", 2, 1);
                                //serialPort2.DiscardInBuffer();

                                sign = 1;
                                number = 0;
                                flag_running = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

                if (sign == 1)
                {
                    double I1 = volt1;
                    Runnig2(counter, I1, 0, row);///// رسم گراف اصلی
                    sign = 0;
                }
                else if (end == 1)  //if portc=0x09
                {
                    end = 0;
                    mainSerialPort.Write("end1");
                    mainSerialPort.Close();
                    Thread.Sleep(1000);

                    Setbtnenabled(true, 2, 1);
                    Settxtfill_func(1, $"finish running-1 :{end} :{string1}", 2, 1);
                    return;
                }

                if (SerialState == 0xA0)//if software ==pause 
                {
                    mainSerialPort.Write("stop");
                    if (mainSerialPort.IsOpen) mainSerialPort.Close();

                    Thread.Sleep(1000);
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
                if (this.IsHandleCreated && !this.IsDisposed && thread_grf.IsAlive)
                {
                    this.Invoke(d, new object[] { V, I1, I2, row });
                }

            }
            else
            {
                /////////////////////////////////////
                // do_smooth_Run_Time(false, Already_Drawing_Graph);
                // this.OnlineChart.Series[row].Points.AddXY(V, I1 - I2);

                OnlineChart.Series[0].Points.AddXY(V, I1 - I2);///// رسم گراف اصلی


                VoltageStepArray.Add(V);//حاوی استپ های ولتاژی
                othertech_val3.Add(I1 - I2);

                shomar_othertech++;

                mainf.toolStripProgressBar1.Value++;
                if (mainf.toolStripProgressBar1.Value == 100)
                    mainf.toolStripProgressBar1.Value = 0;

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

        public void RunnigCD(double V, double I1, double I2, int row)
        {
            if (this.InvokeRequired)
            {
                showOnlineData d = new showOnlineData(RunnigCD);
                if (this.IsHandleCreated && !this.IsDisposed && thread_grf.IsAlive)
                {
                    this.Invoke(d, new object[] { V, I1, I2, row });
                }

            }
            else
            {
                int T1 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);
                int T2 = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
                int Ttotal = T1 + T2;
                double x = V / Ttotal;
                int Cycle = (int)x * Ttotal;

                if (V <= (T1 + Cycle))
                {
                    this.OnlineChart.Series[2].Points.AddXY(V, double.NaN);
                    this.OnlineChart.Series[1].Points.AddXY(V, I1 - I2);///// رسم گراف اصلی
                }
                else
                {
                    this.OnlineChart.Series[1].Points.AddXY(V, double.NaN);
                    this.OnlineChart.Series[2].Points.AddXY(V, I1 - I2);///// رسم گراف اصلی
                }



                VoltageStepArray.Add(V);//حاوی استپ های ولتاژی
                othertech_val3.Add(I1 - I2);

                shomar_othertech++;

                mainf.toolStripProgressBar1.Value++;
                if (mainf.toolStripProgressBar1.Value == 100)
                    mainf.toolStripProgressBar1.Value = 0;

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


        private void Run_key()
        {
            try
            {

                Setgrfnodetext(0, 3, 1);//add tech3 from chartlist run to tree3  اضافه کردن نام تکنیک در حال اجرا به درخت

                for (int TechSeries = 0; TechSeries < dschart1.chartlist1_run.Rows.Count && SerialState != 0xA0; TechSeries++)//چرخ حلقه به تعداد تکنیک های انتخاب شده
                {
                    //Declare Your Varible Here
                    cycle_number_in_file = 1;//save in file
                    show_number_runing = TechSeries;
                    countcv = 0;
                    cv_stop = false;

                    this.Invoke(new Action(() =>
                    {
                        RunnigTechTreeview.Nodes[0].Nodes[TechSeries].ForeColor = Color.DarkCyan;
                        mainf.TechStatus.ForeColor = Color.DarkCyan;
                        mainf.TechStatus.Text = (TechSeries + 1).ToString().Trim() + "- Running technique" + RunnigTechTreeview.Nodes[0].Nodes[TechSeries].Text;
                    }));

                    fillTechParameter(RunnigTechTreeview.Nodes[0].Nodes[TechSeries].Index, 1);//show status in Parameters  نمایش وضعیت در حال اجرا                                                             //fillparamdata(treeView3.Nodes[0].LastNode.Index.Index);   

                    movedata.main_cycle = dschart1.chartlist1_run.Rows[TechSeries][13].ToString();
                    string s = dschart1.chartlist1_run.Rows[TechSeries][1].ToString();
                    if (dschart1.chartlist1_run.Rows[TechSeries][1].ToString() == "11")
                    {
                        movedata.main_cycle = dschart1.chartlist1_run.Rows[TechSeries][12].ToString();
                    }


                    Set_value2(TechSeries); //ست کردن و پردازش اولیه اطلاعات خام وارد شده توسط کاربر و ست درون دیتاست اجرا و ارسال به دستگاه

                    Setbtnenabled(false, 1, 1);

                    Set_Graph(false, TechSeries);//پاک کردن نمودار و افزودن تایتل محورها و کل و فرمت نمایش به نمودار

                    switch (int.Parse(dschart1.chartlist1_run.Rows[TechSeries][1].ToString()))// techno 
                    {
                        case 1://NPV
                            NPV_Confiq(TechSeries);
                            if (!runerror)
                            {
                                newSmoth(0);
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 2://DPV
                            DPV_Confiq(TechSeries);
                            if (!runerror)
                            {
                                newSmoth(0);
                                transferdata(TechSeries, 1);

                            }
                            break;

                        case 3://SWV
                            SWV_Confiq(TechSeries);
                            if (!runerror)
                            {
                                newSmoth(0);
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 4://CV   
                            CV_Confiq(TechSeries);//به علت داشتن سیکل ذخیره فایل و نرم کردن درون توابع زیرمجموعه انجام می شود
                            //dschart1.chartlist1_run.Rows[TechSeries][13] = movedata.main_cycle;//به علت این که سیکل دارد و هنگام اجرا خانه این فیلد مورد استفاده قرار میگیرد بنابراین با اجرای این تابع دیتای سیکل را از بین نمیبریم
                            if (cv_stop)
                            {
                                newSmoth(countcv);
                                int CycleNum = OnlineChart.Series.Count;
                                countcv++;
                                dschart1.chartlist1_run.Rows[TechSeries][13] = CycleNum;
                                movedata.main_cycle = Convert.ToString(CycleNum);
                                transferdata(TechSeries, 1);
                                fillTechParameter(RunnigTechTreeview.Nodes[0].Nodes[TechSeries].Index, 1);
                                cv_stop = false;
                            }
                            break;

                        case 5://LSV
                            LSV_Confiq(TechSeries);
                            if (!runerror)
                            {
                                newSmoth(0);
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 18://tafel
                            LSV_Confiq(TechSeries);
                            if (!runerror)
                            {
                                newSmoth(0);
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 9://CPC
                            run_key_tch9(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                                smootht9(TechSeries, OnlineChart.Series[0].Color);
                            }
                            break;

                        case 10://CHP
                            run_key_tch10(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                            }
                            break;

                        case 11://CHA
                            run_key_tch11(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                            }
                            break;

                        case 12://CHC
                            run_key_tch12(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                            }
                            break;

                        case 14://CD
                            run_key_tch14(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 15://OCP
                            run_key_tch15(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 16://RT
                            run_key_tch15(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 1);
                            }
                            break;

                        case 19://Battery
                            run_key_tch10(TechSeries);
                            if (!runerror)
                            {
                                transferdata(TechSeries, 2);
                            }
                            break;
                    }

                    if (!runerror)
                    {
                        Setgrfnodetext(TechSeries, 2, 1);// ست کردن دیتا درون دیتاست و نمودار فرم بعدی که آنالیز می باشد                        
                    }
                }
                if (!runerror)
                {
                    Setbtnenabled(true, 2, 1);//نمایش دکمه و متن پایان اجرا
                    var xAxis = OnlineChart.ChartAreas[0].AxisX;
                    var yAxis = OnlineChart.ChartAreas[0].AxisY;
                    xMinValue = xAxis.ScaleView.ViewMinimum;
                    xMaxValue = xAxis.ScaleView.ViewMaximum;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CsvSave(int rownum)
        {
            // تعیین مسیر
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string targetPath = Path.Combine(baseDir, "tempfile", "csvSave");

            // چک کردن وجود دایرکتوری و ساختن آن
            if (!Directory.Exists(targetPath))
            {
                // این متد تمام سلسله مراتب پوشه‌ها را در صورت نیاز می‌سازد
                Directory.CreateDirectory(targetPath);
            }

            string techIdRaw = dschart1.chartlist1_run.Rows[rownum][1].ToString();
            string techName = clasglobal.TechName[int.Parse(techIdRaw)].Trim();

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6); // ۶ کاراکتر اول GUID
            string fileName = $"{techName}_{timestamp}_{uniqueId}.dat";

            string filePath = Path.Combine(targetPath, $"{fileName}.csv");
            // File.WriteAllText(filePath, csvContent);

            var sb = new StringBuilder();

            // افزودن هدر پارامترها
            sb.AppendLine("$# Technique,Cyclic Voltammetry");
            sb.AppendLine("# ScanRate,0.1");

        }

        private void transferdata(int rownum, int typecall)
        {
            string targetDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\";

            // چک کردن وجود دایرکتوری و ساختن آن
            if (!Directory.Exists(targetDirectory))
            {
                // این متد تمام سلسله مراتب پوشه‌ها را در صورت نیاز می‌سازد
                Directory.CreateDirectory(targetDirectory);
            }


            string strpath = "";// fName = "";

            string techIdRaw = dschart1.chartlist1_run.Rows[rownum][1].ToString();
            string techName = clasglobal.TechName[int.Parse(techIdRaw)].Trim();

            var availableNumber = Enumerable.Range(100001, 1000000)
                .FirstOrDefault(num => !File.Exists(Path.Combine(targetDirectory, $"{techName}{num}.dat")));

            

            if (availableNumber != 0)
            {
                int finalNumber = (cycle_number_in_file <= 1) ? availableNumber : availableNumber - 1;
                strpath = Path.Combine(targetDirectory, $"{techName}{finalNumber}.dat");
            }
            else
                MessageBox.Show("پوشه tempfile را خالی کنید");

            if (cycle_number_in_file <= 1)
                ReadWrite.WriteToFile(dschart1.chartlist1_run.Rows[rownum], clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())], strpath, rownum, 1);//header in file
            else
            {
                StreamWriter s = new StreamWriter(strpath, true);
                s.WriteLine();
                s.Close();
            }

            if (cv_stop && Byte.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString()) == clasglobal.CV)
            {
                clasglobal.ChangeCycle(strpath);
            }

            dschart1.chartlist1_run.Rows[rownum][23] = strpath;
            StreamWriter sa1 = new StreamWriter(strpath, true);
            sa1.WriteLine("============================================================");

            if (cycle_number_in_file > 1)
            {
                sa1.WriteLine("Cycle Number: " + cycle_number_in_file.ToString());
                sa1.WriteLine();
            }

            cycle_number_in_file++;

            string Ln = "";

            if (typecall == 1)
            {
                for (int i = 0; i < VoltageStepArray.Count; i++)
                {
                    Ln = VoltageStepArray[i].ToString().Trim();
                    Ln = Ln + "\t" + othertech_val3[i].ToString().Trim() + ';';
                    sa1.WriteLine(Ln.Trim());
                }
            }
            else
            {
                int Factor = 10;
                if (clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[rownum][1].ToString())] == "CP")
                {
                    Factor = 5;
                }
                for (int count1 = 0; count1 < shomar_cha_chc; count1++)
                {
                    if (SampleRateMultiplier != 0)
                    {
                        if (count1 % (Factor * SampleRateMultiplier) == 0)
                        {
                            if (datas2_state[count1] == 1)
                            {
                                Ln = time_cha_chc[count1].ToString();
                                Ln = Ln + "\t" + volt_cha_chc[count1].ToString();
                                sa1.WriteLine(Ln);
                            }
                        }
                    }
                    else
                    {
                        if (datas2_state[count1] == 1)
                        {
                            Ln = time_cha_chc[count1].ToString();
                            Ln = Ln + "\t" + volt_cha_chc[count1].ToString();
                            Ln = Ln + "\t" + volt_cha_chc[count1].ToString();
                            sa1.WriteLine(Ln);
                        }
                    }


                }
            }
            sa1.Close();
        }

        private double GetChartValue(int row, int column)
        {
            try
            {
                // بررسی وجود جدول و تعداد سطرها
                if (dschart1?.chartlist1_run == null)
                    throw new NullReferenceException("جدول chartlist1_run مقداردهی نشده است.");

                if (row < 0 || row >= dschart1.chartlist1_run.Rows.Count)
                    throw new ArgumentOutOfRangeException(nameof(row), "ایندکس سطر خارج از محدوده است.");

                if (column < 0 || column >= dschart1.chartlist1_run.Columns.Count)
                    throw new ArgumentOutOfRangeException(nameof(column), "ایندکس ستون خارج از محدوده است.");

                object cellValue = dschart1.chartlist1_run.Rows[row][column];

                // بررسی مقدار تهی یا خالی
                if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    throw new InvalidOperationException($"مقدار سلول در [{row}, {column}] نامعتبر است (null یا خالی).");

                // تلاش برای تبدیل مقدار به عدد
                if (double.TryParse(cellValue.ToString(), out double result))
                {
                    return result;
                }
                else
                {
                    throw new FormatException($"مقدار '{cellValue}' در [{row}, {column}] عددی نیست.");
                }
            }
            catch (Exception ex)
            {
                // ثبت خطا برای بررسی‌های بعدی
                MessageBox.Show($"خطا در دریافت مقدار از جدول:\n{ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.NaN; // مقدار نامعتبر برای جلوگیری از کرش برنامه
            }
        }

        private void NPV_Confiq(int row)
        {
            fst = false;
            S_et = false;
            pulsy = false;

            double E1 = GetChartValue(row, 3);
            double E2 = GetChartValue(row, 4);

            if (E1 < E2)//برای پیدا کردن جهت تکنیک
            {
                N_gtv = false;
                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E1 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E2 * 10) / 10;
                }));
            }
            else
            {
                N_gtv = true;
                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E2 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E1 * 10) / 10;
                }));
            }

            DoRun2(row);
        }

        private void DPV_Confiq(int row)
        {
            fst = false;
            S_et = false;
            pulsy = true;

            double E1 = GetChartValue(row, 3);
            double E2 = GetChartValue(row, 4);

            if (E1 < E2)
            {
                N_gtv = false;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E1 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E2 * 10) / 10;
                }));
            }
            else
            {
                N_gtv = true;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E2 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E1 * 10) / 10;
                }));
            }
            ngtv = !N_gtv;

            DoRun2(row);
        }

        private void SWV_Confiq(int row)
        {
            if (Analiz_St1 < 100)
            {
                fst = true;
                S_et = true;
            }
            else
            {
                fst = false;
                S_et = false;
            }

            double E1 = GetChartValue(row, 3);
            double E2 = GetChartValue(row, 4);

            if (E1 < E2)
            {
                N_gtv = false;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E1 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E2 * 10) / 10;
                }));
            }
            else
            {
                N_gtv = true;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E2 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E1 * 10) / 10;
                }));
            }

            if (Analiz_St1 <= 2)
                Analiz_St1 = 1;
            pulsy = true;
            ngtv = N_gtv;
            if (!fst)
                DoRun2(row);
            else
                dorun3(row);

        }

        private void CV_Confiq(int row)
        {
            double E1 = GetChartValue(row, 3);
            double E2 = GetChartValue(row, 4);

            if (E1 < E2)
            {
                N_gtv = false;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E1 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E2 * 10) / 10;
                }));
            }
            else
            {
                N_gtv = true;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E2 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E1 * 10) / 10;
                }));
            }

            pulsy = false;

            DoRun2CV(row);
        }

        private void LSV_Confiq(int row)
        {
            if ((double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000 < 50) || (Analiz_St1 < 1))
            {
                fst = true;
                S_et = true;
            }
            else
            {
                fst = false;
                S_et = false;

            }

            double E1 = GetChartValue(row, 3);
            double E2 = GetChartValue(row, 4);

            if (E1 < E2)
            {
                N_gtv = false;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E1 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E2 * 10) / 10;
                }));
            }
            else
            {
                N_gtv = true;

                this.Invoke(new Action(() =>
                {
                    OnlineChart.ChartAreas[0].AxisX.Minimum = Math.Floor(E2 * 10) / 10;
                    OnlineChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(E1 * 10) / 10;
                }));
            }

            pulsy = false;


            if (!fst)
                DoRun2(row);
            else
                dorun3(row);
        }



        private void run_key_tch9(int row)
        {
            N_gtv = false;
            fst = true;
            S_et = true;

            dschart1.chartlist1_run.Rows[row][6] = 1;

            this.Invoke(new Action(() =>
            {
                OnlineChart.ChartAreas[0].AxisX.Minimum = Double.NaN;
                OnlineChart.ChartAreas[0].AxisX.Maximum = Double.NaN;
            }));

            DoRun2(row);
        }

        private void run_key_tch10(int row)
        {
            N_gtv = false;
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
        private void run_key_tch11(int row)
        {
            N_gtv = false;
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

        private void run_key_tch14(int row)
        {
            N_gtv = false;
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
            dorun2_costum(row);
            //doRun2_1(row);
        }
        private void run_key_tch15(int row)
        {
            OCP_Reading(row);
        }

        public void Runf(byte rng, bool precondtion)
        {
            RunnigTechTreeview.Nodes[0].Nodes.Clear();

            if (precondtion)
            {
                Analiz_CP1 = clasglobal.cpctprms.CP1 * 1000;
                Analiz_CP2 = clasglobal.cpctprms.CP2 * 1000;
                Analiz_CP3 = clasglobal.cpctprms.CP3 * 1000;
                Analiz_CT1 = clasglobal.cpctprms.CT1;
                Analiz_CT2 = clasglobal.cpctprms.CT2;
                Analiz_CT3 = clasglobal.cpctprms.CT3;
                StepRange = rng;
            }

            if (SerialState == 0xA0)//if serial is stop 
            {
                SerialState = 0x20;//change mode to run
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
                        OnlineChart.Titles.Add("Clear");
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
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
                        OnlineChart.Titles[0].Text = clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        OnlineChart.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];
                        OnlineChart.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle[int.Parse(dschart1.chartlist1_run.Rows[row1][1].ToString())];


                        OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                        OnlineChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

                        OnlineChart.MouseWheel += OnlineChart_MouseWheel;
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

        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XFinish { get; set; }
            public double YStart { get; set; }
            public double YFinish { get; set; }
        }

        private readonly Stack<ZoomFrame> _zoomFrames = new Stack<ZoomFrame>();
        private void OnlineChart_MouseWheel(object sender, MouseEventArgs e)
        {
            // تنظیمات اولیه محورهای چارت
            OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            //chart.ChartAreas[0].AxisX.ScaleView.Size = 1;

            try
            {
                if (e.Delta < 0)
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    double difrance = xMax - xMin;
                    difrance = Math.Round(difrance * 2, 3);

                    var pixelToValue = Math.Round(xAxis.PixelPositionToValue(e.Location.X), 3);
                    var posXStart = pixelToValue - difrance;
                    if (posXStart < xMinValue)
                    {
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                        return;
                    }
                    var posXFinish = pixelToValue + difrance;
                    if (posXFinish > xMaxValue)
                    {
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
                        return;
                    }

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);

                }
                else if (e.Delta > 0)
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    double difrance = xMax - xMin;
                    difrance = Math.Round(difrance / 2, 3);

                    //_zoomFrames.Push(new ZoomFrame { XStart = xMin, XFinish = xMax });
                    var pixelToValue = Math.Round(xAxis.PixelPositionToValue(e.Location.X), 3);
                    var posXStart = pixelToValue - difrance;
                    if (posXStart < xMinValue)
                    {
                        posXStart = xMinValue;
                    }
                    var posXFinish = pixelToValue + difrance;
                    if (posXFinish > xMaxValue)
                    {
                        posXFinish = xMaxValue;
                    }

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    OnlineChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true; // فعال کردن اسکرول بار
                    OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // سبک دکمه‌های اسکرول
                    OnlineChart.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.White;
                }
            }
            catch (Exception Msg)
            {
                MessageBox.Show(Msg.Message);
            }
        }

        delegate void Setgrftchart1Callback1(Series ser1, int code, int state);
        private void AddSeriesToChart(Series ser1, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        Setgrftchart1Callback1 d = new Setgrftchart1Callback1(AddSeriesToChart);
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

        private void Holdbtn_Click(object sender, EventArgs e)
        {

            SerialState = 0x60;
            if (mainSerialPort.IsOpen)
                mainSerialPort.Write("puase");

            statehc = true;
            bpausetxt = mainf.TechStatus.Text;
            mainf.TechStatus.ForeColor = Color.Red;
            mainf.TechStatus.Text = bpausetxt + "- Pause";
            this.Holdbtn.Enabled = false;
            this.Stopbtn.Enabled = false;
            this.Continuebtn.Enabled = true;

            //this.Invoke((MethodInvoker)delegate ()
            //{

            //    bpausetxt = mainf.TechStatus.Text;
            //    mainf.TechStatus.ForeColor = Color.Brown;
            //    mainf.TechStatus.Text = bpausetxt + "- Pause";
            //    this.Holdbtn.Enabled = false;
            //    this.Stopbtn.Enabled = false;
            //    this.Continuebtn.Enabled = true;

            //});
        }

        private void serialPort2_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            string error = GetSerialErrorMessage(e.EventType); 
            MessageBox.Show(error,"Serial Port Error Flag", MessageBoxButtons.OK,MessageBoxIcon.Error);

        }

        private string GetSerialErrorMessage(SerialError Error)
        {
            switch (Error)
            {
                case SerialError.Frame:
                    return "Frame Error: مشکل در فریم داده (baud rate یا نویز)";
                case SerialError.Overrun:
                    return "Overrun Error: بافر سخت‌افزاری پر شده";
                case SerialError.RXOver:
                    return "RX Buffer Overflow: بافر دریافت پر شده";
                case SerialError.RXParity:
                    return "Parity Error: خطای parity در داده دریافتی";
                case SerialError.TXFull:
                    return "TX Buffer Full: بافر ارسال پر شده";
                default:
                    return "Unknown Serial Port Error";
            }
        }

        private void frmshowGraph2065_Resize(object sender, EventArgs e)
        {
            RunnigTechTreeview.Top = panel2.Bottom + 5;
            panel3.Width = (int)(this.ClientSize.Width * 0.2);
            panel1.Width = (int)(this.ClientSize.Width * 0.8);
            panel1.Left = panel3.Right + 5;
            toolsPanel.Width = (int)(OnlineChart.Width * 0.04);
            if (toolsPanel.Width < 40)
                toolsPanel.Width = 43;

            toolsPanel.Left = OnlineChart.Left;
            toolsPanel.Top = OnlineChart.Top + 2;
            foreach (Control control in toolsPanel.Controls)
            {
                control.Height = toolsPanel.Height / 8;
            }


        }

        private Image ResizeImage(Image image, int width, int height)
        {
            if (height != 0 && width != 0)
            {
                Bitmap resizedImage = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(image, 0, 0, width, height);
                }
                return resizedImage;
            }
            return null;
        }

        private void frmshowGraph2065_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                staterun = "Form Closed";

                runerror = true;

                _shouldStop = true;
                cv_stop = true;

                if (mainSerialPort.PortName == "null")
                    return;

                if (!mainSerialPort.IsOpen)
                    mainSerialPort.Open();

                mainSerialPort.Write("stop");
                mainSerialPort.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void OnlineChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) // وقتی دکمه‌ی وسط ماوس (اسکرول) فشرده شد
            {
                isScrolling = true;
                OnlineChart.Cursor = Cursors.Hand; // تغییر کرسر به حالت اسکرول
            }
        }

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
                        txtLoger.Text = txt.Trim();
                    }
                    break;
                case 2:
                    if (state == 1)
                    {
                        Settxtfill d = new Settxtfill(Settxtfill_func);
                        if (this != null && !this.IsDisposed && this.IsHandleCreated)
                        {
                            this.BeginInvoke(d, new object[] { typetxt, txt, code, 2 });
                        }
                    }
                    else
                    {
                        //  txtcomment.Text= txt + Environment.NewLine + txtcomment.Text.Trim();
                        if (typetxt == 1)
                            clasglobal.AppendText(txtLoger, txt + Environment.NewLine, Color.Black);
                        else if (typetxt == 2)
                            clasglobal.AppendText(txtLoger, txt + Environment.NewLine, Color.Red);
                        else if (typetxt == 3)
                            clasglobal.AppendText(txtLoger, txt + Environment.NewLine, Color.Green);
                        else if (typetxt == 4)
                            clasglobal.AppendText(txtLoger, txt + Environment.NewLine, Color.Yellow);


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
                mainf.TechStatus.Text = (RunnigTechTreeview.SelectedNode.Index + 1).ToString().Trim() + "_" + number.ToString() + " - Running technique " + RunnigTechTreeview.Nodes[0].Nodes[RunnigTechTreeview.SelectedNode.Index].Text;
            else
                mainf.TechStatus.Text = (RunnigTechTreeview.SelectedNode.Index + 1).ToString().Trim() + " - Running technique " + RunnigTechTreeview.Nodes[0].Nodes[RunnigTechTreeview.SelectedNode.Index].Text;
        }


        delegate void SetgrfnodeCallback(int txt, int code, int state);

        private void txtLoger_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuDataLoger.Show(txtLoger, e.Location);
            }
        }

        private void DebugItem_CheckedChanged(object sender, EventArgs e)
        {
            txtLoger.Clear();
            if (MaintenanceItem.Checked)
                Maintenance = true;
            else Maintenance = false;

            if (DebugItem.Checked)
                DebugMode = true;
            else DebugMode = false;
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            toolStrip1.Height = (int)(this.Height * 0.1);

            if (toolStrip1.Height != 0 && toolStrip1.Width != 0)
            {
                int itemWidth = toolStrip1.Width / toolStrip1.Items.Count;// تنظیم اندازه هر آیتم
                int iconSize = (int)(toolStrip1.Height * 0.4);
                float fontSize = toolStrip1.Height / 8.0f;

                if (iconSize <= 30)
                {
                    iconSize = 30;
                }

                foreach (ToolStripItem item in toolStrip1.Items)
                {
                    item.Width = itemWidth;
                    item.Font = new Font(item.Font.FontFamily, fontSize);

                    if (item is ToolStripButton button && button.Image != null)
                    {
                        button.Image = ResizeImage(button.Image, iconSize, iconSize);
                    }
                    item.Height = toolStrip1.Height;

                }
            }
        }

        private void Setgrfnodetext(int txt, int code, int state)
        {
            switch (code)
            {
                case 2:
                    if (state == 1)
                    {
                        SetgrfnodeCallback d = new SetgrfnodeCallback(Setgrfnodetext);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        RunnigTechTreeview.Nodes[0].Nodes[txt].ForeColor = Color.Green;
                        if (OnlineChart.Series[0].Points.Count > 0)// مقادیر پیک جریان یا ولتاژی نمودار را نشان می‌دهد
                            Parameters.Text = Parameters.Text + "\n\t*******************\nE1:" + OnlineChart.Series[0].Points.FindMinByValue("X").XValue.ToString("000e-0") +
                                "\nE2:" + OnlineChart.Series[0].Points.FindMaxByValue("X").XValue.ToString("000e-0") + "\nMin. I:" + OnlineChart.Series[0].Points.FindMinByValue("Y").YValues[0].ToString("000e-0") +
                                "\nMax. I:" + OnlineChart.Series[0].Points.FindMaxByValue("Y").YValues[0].ToString("000e-0");

                        int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());

                        DataRow dr = this.mainf.grf.dschart2.analyselist.NewRow();
                        dr[0] = TechNumber;
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
                        if (TechNumber == 9)
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
                        mainf.grf.dschart2.analyselist.Rows.Add(dr);
                        mainf.grf.dschart2.analyselist.AcceptChanges();
                        int maxrana = mainf.grf.dschart2.analyselist.Rows.Count;

                        Color color = Color.FromArgb(clasglobal.Graph_Color);
                        string ParentNodeName = clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())];
                        if (dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() != "")
                            ParentNodeName += "(" + dschart1.chartlist1_run.Rows[txt][24].ToString().Trim() + ")";

                        TreeNode ParentNode = new TreeNode(ParentNodeName) { ForeColor = color };


                        if (countcv > 0)
                        {
                            for (int i = 0; i < countcv; i++)
                            {
                                TreeNode ChildrenNode = new TreeNode($"Cycle_{i + 1}") { ForeColor = color };
                                ParentNode.Nodes.Add(ChildrenNode);
                            }
                        }
                        mainf.grf.TechTreeview.Nodes[0].Nodes.Add(ParentNode);
                        mainf.grf.TechTreeview.SelectedNode = mainf.grf.TechTreeview.Nodes[0].LastNode;
                        int ParentNodeIndex = mainf.grf.TechTreeview.Nodes[0].LastNode.Index;
                        mainf.grf.tech = byte.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString());

                        //if (!(mainf.tabControl1.SelectedTab == mainf.tabControl1.TabPages[2]))
                        //{
                        //    var lastTab = mainf.tabControl1.SelectedTab;
                        //    mainf.tabControl1.SelectedTab = mainf.tabControl1.TabPages[2];// نمایان کردن فرم آنالیز
                        //    Thread.Sleep(1);
                        //    mainf.tabControl1.SelectedTab = lastTab;// برگشتن به فرم در حال اجرا
                        //}
                        string ParentSeriesName = ParentNodeName + '-' + mainf.grf.TechTreeview.Nodes[0].LastNode.Index;

                        if (TechNumber == 10 || TechNumber == 11 || TechNumber == 12 || TechNumber == 19)
                        {
                            Array.Resize(ref time_cha_chc, shomar_cha_chc);
                            Array.Resize(ref volt_cha_chc, shomar_cha_chc);
                            mainf.grf.SetSeriesForAnalyseChart(color, ParentSeriesName);

                            if (SampleRateMultiplier != 0)
                            {
                                int factor = 10 * SampleRateMultiplier;
                                if (clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())] == ("CP") 
                                    || clasglobal.TechName[int.Parse(dschart1.chartlist1_run.Rows[txt][1].ToString())] == ("Battery"))
                                {
                                    factor = 5 * SampleRateMultiplier;
                                }

                                // محاسبه تعداد خانه‌هایی که با توجه به ضریب samplerate هستند
                                int newSize = time_cha_chc.Length / factor;
                                if (time_cha_chc.Length % factor != 0)
                                    newSize++; // اگر باقی‌مانده وجود داشته باشد، یک خانه بیشتر نیاز داریم
                                Double[] DeceasedArray = new double[newSize];
                                Double[] DeceasedArrayVoltage = new double[newSize];

                                // انتقال خانه‌های با ایندکس ضریب 5 به آرایه کوچک‌تر
                                int index = 0;
                                for (int i = 0; i < time_cha_chc.Length; i += factor)
                                {
                                    DeceasedArray[index] = time_cha_chc[i];
                                    DeceasedArrayVoltage[index] = volt_cha_chc[i];
                                    index++;
                                }

                                mainf.grf.tChart1.Series[ParentSeriesName].Points.DataBindXY(DeceasedArray, DeceasedArrayVoltage);
                            }
                            else
                                mainf.grf.tChart1.Series[ParentSeriesName].Points.DataBindXY(time_cha_chc, volt_cha_chc);

                            mainf.grf.tChart1.Series[ParentSeriesName].Enabled = false;

                        }
                        else if (TechNumber == 4)//cv
                        {
                            mainf.grf.SetSeriesForAnalyseChart(color, ParentSeriesName);

                            for (int i = 0; i < countcv; i++)
                            {
                                string SeriesName = $"Cycle_{(i + 1)}-{ParentNodeIndex}";
                                mainf.grf.SetSeriesForAnalyseChart(color, SeriesName);

                                List<double> xValues = new List<double>();
                                List<double> yValues = new List<double>();

                                Series series = OnlineChart.Series[i];

                                foreach (DataPoint point in series.Points)
                                {
                                    xValues.Add(point.XValue);
                                    yValues.Add(point.YValues[0]); // چون ممکنه DataPoint چند مقدار Y داشته باشه (مثل نمودار stack یا bubble)، YValues[0] می‌گیریم
                                }
                                mainf.grf.tChart1.Series[SeriesName].Points.DataBindXY(xValues, yValues);
                                mainf.grf.smooth_new(SeriesName, xValues, yValues);
                                mainf.grf.tChart1.Series[SeriesName].Enabled = false;
                            }

                        }
                        else
                        {
                            mainf.grf.SetSeriesForAnalyseChart(color, ParentSeriesName);
                            mainf.grf.tChart1.Series[ParentSeriesName].Points.DataBindXY(VoltageStepArray, othertech_val3);
                            if (TechNumber != 14)
                                mainf.grf.smooth_new(ParentSeriesName, VoltageStepArray, othertech_val3);

                            mainf.grf.tChart1.Series[ParentSeriesName].Enabled = false;

                            //mainf.grf.show_Graph(color.ToArgb(), this.mainf.grf, "MainChart", othertech_val0, othertech_val3, true);
                            //mainf.grf.tChart1.Series[maxrana - 1].Enabled = false;
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
                        this.RunnigTechTreeview.Nodes[0].Nodes.Clear();
                        for (int i = 0; i < dschart1.chartlist1_run.Rows.Count; i++)
                        {
                            this.RunnigTechTreeview.Nodes[0].Nodes.Add(clasglobal.TechName[int.Parse(this.dschart1.chartlist1_run.Rows[i][1].ToString())]);
                            this.RunnigTechTreeview.Nodes[0].Nodes[i].ForeColor = Color.Black;
                        }
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
                    toolsPanel.BackColor = cl.Color;

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
                        this.RunnigTechTreeview.SelectedNode = this.RunnigTechTreeview.Nodes[0].Nodes[row];
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

        private void btnSwapArea_Click(object sender, EventArgs e)
        {
            SwapAxes(OnlineChart);
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

            // 4. ریفرش نمودار
            chart.Invalidate();
        }

        private void btnSwapArea_Click_1(object sender, EventArgs e)
        {
            SwapAxes(OnlineChart);
        }

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
                        mainf.stf.tsbRun.Enabled = false;
                        mainf.stf.tsbNew.Enabled = false;
                        mainf.stf.runToolStripMenuItem1.Enabled = false;
                        Holdbtn.Enabled = true;
                        Stopbtn.Enabled = true;
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
                        Holdbtn.Enabled = false;
                        Stopbtn.Enabled = false;
                        mainf.stf.runToolStripMenuItem1.Enabled = true;
                        mainf.stf.tsbRun.Enabled = true;
                        // timer3.Enabled = false;
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
            shomar_othertech = 0;
            VoltageStepArray.Clear();
            othertech_val3.Clear();

            switch (byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString()))//switch(tech)
            {
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
                case clasglobal.Battery:
                    //dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][3] = -1 * double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString());
                    dschart1.chartlist1_run.Rows[row][4] = -1 * double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString());
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;

                    break;
                case clasglobal.CA:

                    dschart1.chartlist1_run.Rows[row][13] = 1;
                    dschart1.chartlist1_run.Rows[row][15] = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][16] = double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) / 1000;
                    dschart1.chartlist1_run.Rows[row][12] = double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) / 1000;

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
                case clasglobal.RT:
                    break;
                case clasglobal.Tafel:
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
                    break;
                default: break;
            }
            ////////////////////////////////////
            Send_usb_Parameter(row);//ارسال پارامتر های تکنیک به ترتیب ول تا آخر به دستگاه

        }

        public void Send_usb_Parameter(int row)
        {
            long Temp = 0;

            if (!mainSerialPort.IsOpen)
            {
                mainSerialPort.Open();
            }

            Settxtfill_func(2, $"start", 2, 1);
            mainSerialPort.Write("@");
            byte Tech_Number = byte.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());

            if (Tech_Number == 18)
            {
                Tech_Number = 5; //تافل را به lsv تبدیل میکنیم
            }
            else if(Tech_Number == 19)
            {
                Tech_Number = 10;
            }

            mainSerialPort.Write(Tech_Number.ToString() + "@");

            if ((int)Tech_Number <= 16)
            {
                Temp = (long)Analiz_CP1;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (long)Analiz_CP2;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (long)Analiz_CP3;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (long)Analiz_CT1;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (long)Analiz_CT2;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (long)Analiz_CT3;
                mainSerialPort.Write(Temp.ToString() + "@");
                Temp = (int)double.Parse(dschart1.chartlist1_run.Rows[row][7].ToString());//quesent time
                mainSerialPort.Write(Temp.ToString() + "@");
                if (Tech_Number == 0)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 1)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);//pw
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 2)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);//pw
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 3)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = long.Parse(dschart1.chartlist1_run.Rows[row][12].ToString());//fr
                    Settxtfill_func(2, $"fr {Temp}", 2, 1);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 4)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//e3
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = int.Parse(dschart1.chartlist1_run.Rows[row][13].ToString());//cycle
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 5)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");

                }
                else if (Tech_Number == 6)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 7)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()) * 1000);//hs
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][11].ToString()) * 1000);//ph
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][10].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");

                }
                else if (Tech_Number == 8)
                {
                    //Tech_Number = Range1;
                    //serialPort2.Write(Tech_Number.ToString() + "@");
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
                else if (Tech_Number == 9)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()));// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()));//for low cpc,and high cpc
                                                                                                //Temp = (int)(double.Parse(ds);//for low cpc,and high cpc
                    Settxtfill_func(2, $"cpc_mode={Temp}", 2, 1);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");

                    Analiz_T3 = double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString());//t1
                    ti = 0.02;
                    if (Analiz_T3 > 100)
                        ti = ((int)(Analiz_T3 / 100)) * ti;

                }
                else if (Tech_Number == 10)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));// CYCLE
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//eup
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);// edown
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");

                    Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                    //if (Analiz_T3 > 5024) //if (t1+t2)*1000  > 5024
                    //    ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                    //else
                    //    ti = 1;

                    ti = 1;
                }
                else if (Tech_Number == 11)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//e1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//e2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);// t1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    //Settxtfill_func(2, $"t1{Temp}", 2, 1);


                    Analiz_T3 = (double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) + double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString())) * 1000;//t1+t2

                    if (Analiz_T3 > 5024) //if (t1+t2)*1000  > 5024
                        ti = ((int)(Analiz_T3 / 25)) * 0.005;//analize-t3 = (t1+t2)*1000
                    else
                        ti = 1;


                    Temp = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()) * 1000);// CYCLE
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 12)
                {
                    mainSerialPort.Write(StepRange.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][3].ToString()) * 1000);//i1 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][4].ToString()) * 1000);//i2 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                    mainSerialPort.Write(Temp.ToString() + "@");

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
                    tmep_t = (Int16)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);
                    //Settxtfill_func(2, $"analiz={tmep_t}", 2, 1);
                    mainSerialPort.Write(tmep_t.ToString() + "@");
                    mainSerialPort.Write(".");
                }
                else if (Tech_Number == 14)
                {
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][5].ToString()) * 1000);//ecut 
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][13].ToString()) * 1000);//ecutoff
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][17].ToString())); //i1 (me)
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][18].ToString()));//i2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][16].ToString()) * 1000);//t2
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][12].ToString()));//cycle
                    mainSerialPort.Write(Temp.ToString() + "@");
                    //Settxtfill_func(2, $"analiz={Temp}", 2, 1);
                    mainSerialPort.Write(".");

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

                }
                else if (Tech_Number == 15)
                {
                    Temp = (long)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()) * 1000);//t1
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");

                }
                else if (Tech_Number == 16)
                {
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][6].ToString()));//for low cpc,and high cpc
                    mainSerialPort.Write(Temp.ToString() + "@");
                    Temp = (int)(double.Parse(dschart1.chartlist1_run.Rows[row][15].ToString()));
                    mainSerialPort.Write(Temp.ToString() + "@");
                    mainSerialPort.Write(".");
                }
            }
        }


        public void newSmoth(int ChartSeriesIndex)
        {
            try
            {
                List<double> xValues = new List<double>();
                List<double> yValues = new List<double>();

                Series series = OnlineChart.Series[ChartSeriesIndex];

                foreach (DataPoint point in series.Points)
                {
                    xValues.Add(point.XValue);
                    yValues.Add(point.YValues[0]); // چون ممکنه DataPoint چند مقدار Y داشته باشه (مثل نمودار stack یا bubble)، YValues[0] می‌گیریم
                }



                // 1. اعمال Median Filter (برای حذف اعوجاج تیز)
                var medianFiltered = MedianFilter(yValues, windowSize: 5);

                List<double> smothedX = SavitzkyGolaySmooth(medianFiltered, windowSize: 9);
                this.Invoke(new Action(() =>
                {
                    series.Points.Clear();
                }));

                for (int i = 0; i < xValues.Count; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        series.Points.AddXY(xValues[i], smothedX[i]);

                        mainf.toolStripProgressBar1.Value++;
                        if (mainf.toolStripProgressBar1.Value == 100)
                            mainf.toolStripProgressBar1.Value = 0;
                    }));

                    Thread.Sleep(10);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static List<double> MedianFilter(List<double> data, int windowSize)
        {
            int half = windowSize / 2;
            int n = data.Count;
            List<double> result = new List<double>();

            for (int i = 0; i < n; i++)
            {
                List<double> window = new List<double>();

                for (int j = i - half; j <= i + half; j++)
                {
                    int index = Math.Max(0, Math.Min(j, n - 1));
                    window.Add(data[index]);
                }

                window.Sort();
                result.Add(window[window.Count / 2]);
            }

            return result;
        }

        public static List<double> SavitzkyGolaySmooth(List<double> data, int windowSize = 7, int polyOrder = 2)
        {
            if (windowSize % 2 == 0 || windowSize < 3)
                throw new ArgumentException("Window size must be odd and >= 3.");

            int half = windowSize / 2;
            int n = data.Count;
            List<double> result = new List<double>(new double[n]);
            double[] coeffs;

            if (windowSize == 5 && polyOrder == 2)
                coeffs = new double[] { -3.0 / 35, 12.0 / 35, 17.0 / 35, 12.0 / 35, -3.0 / 35 };
            else if (windowSize == 7 && polyOrder == 2)
                coeffs = new double[] { -2.0 / 21, 3.0 / 21, 6.0 / 21, 7.0 / 21, 6.0 / 21, 3.0 / 21, -2.0 / 21 };
            else if (windowSize == 9 && polyOrder == 2)
                coeffs = new double[] { -21.0 / 231, 14.0 / 231, 39.0 / 231, 54.0 / 231, 59.0 / 231, 54.0 / 231, 39.0 / 231, 14.0 / 231, -21.0 / 231 };
            else
                throw new NotSupportedException("Only windowSize=5,7,9 and polyOrder=2 are implemented.");

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = -half; j <= half; j++)
                {
                    int k = i + j;
                    k = Math.Max(0, Math.Min(k, n - 1)); // Clamp to edge
                    sum += data[k] * coeffs[j + half];
                }
                result[i] = sum;
            }
            return result;
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
            long Fore = 0, Rev = 0;
            int vn = 0, NUM, j = 0, len;
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

            for (int i = 1; i <= vn - 1; i++)
            {
                if (Math.Abs(othertech_val3[i] - othertech_val3[i - 1]) > 1000)
                    othertech_val3[i] = (othertech_val3[i - 1] + othertech_val3[i + 1]) / 2;
            }

            for (int i = vn; i >= 2; i--)
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

                for (int i = len; i >= 1; i--)
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

        private void hToolStripMenuItem_Click(object sender, EventArgs e)// دکمه hold   cont
        {
            if (statehc)//for again run -> countiue
            {
                mainSerialPort.Write("run");
                Settxtfill_func(3, $"run again", 2, 1);
                SerialState = 0x20;
                statehc = false;
                mainf.TechStatus.ForeColor = Color.Green;
                mainf.TechStatus.Text = bpausetxt;
                this.Holdbtn.Enabled = true;
                this.Continuebtn.Enabled = false;
                this.Stopbtn.Enabled = true;
            }
            else
            {// for pause
                SerialState = 0x60;
                mainSerialPort.Write("puase");
                statehc = true;
                bpausetxt = mainf.TechStatus.Text;
                mainf.TechStatus.ForeColor = Color.Brown;
                mainf.TechStatus.Text = bpausetxt + "- Pause";
                this.Holdbtn.Enabled = false;
                this.Stopbtn.Enabled = false;
                this.Continuebtn.Enabled = true;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)//stop
        {
            cv_stop = true;
            SerialState = 0xA0;

            if (mainSerialPort == null)
                return;

            if (!mainSerialPort.IsOpen)
                mainSerialPort.Open();

            mainSerialPort.Write("stop");
            mainSerialPort.Close();

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
                int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());

                


                for (int i = 0; i < datas2_state.Length; i++)
                {
                    if (SerialState == 0xA0)
                        return;

                    if (datas2_state[i] == 0)
                    {
                        ///////////////////////////////
                        // if (SerialState = 0x60) H_ld = true;

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

                        if (Powr == 8)
                            C = Crnt * 1.02207 * Math.Pow(10, Powr);/////// - 6
                        else
                            C = Crnt * 0.61035 * Math.Pow(10, Powr - 6); //// - 6
                        if (signe ^ (TechNumber == 10))
                            C = C * -1;

                        if (shomar_cha_chc >= time_cha_chc.Length)
                        {
                            Array.Resize(ref time_cha_chc, time_cha_chc.Length + 200);
                            Array.Resize(ref volt_cha_chc, volt_cha_chc.Length + 200);
                        }
                        time_cha_chc[shomar_cha_chc] = Math.Round((i + 1) * ti, 3, MidpointRounding.AwayFromZero);

                        if (TechNumber == 12)//CHC
                            if (shomar_cha_chc == 0)
                                volt_cha_chc[shomar_cha_chc] = C * ti;
                            else
                                volt_cha_chc[shomar_cha_chc] = volt_cha_chc[shomar_cha_chc - 1] + (C * ti);
                        else
                            volt_cha_chc[shomar_cha_chc] = C;

                        //this.OnlineChart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(dschart1.chartlist1_run.Rows[row][3]) - 1;
                        //this.OnlineChart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(dschart1.chartlist1_run.Rows[row][4]) + 1;

                        //رسم برای سه تکنیک  chp cha chc
                        if (shomar_cha_chc == 0 && TechNumber != 12)//برای رسم در نقطه صفر زمان غیر از تکنیک سی اچ سیmn
                            this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc] - 1, volt_cha_chc[shomar_cha_chc]);

                        if (TechNumber == 11)
                        {
                            time_cha_chc[shomar_cha_chc] /= 10;
                        }
                        if (TechNumber == 10)//CP
                        {
                            time_cha_chc[shomar_cha_chc] /= 5;
                        }
                        if (TechNumber == 14)
                        {
                            time_cha_chc[shomar_cha_chc] /= 10;
                        }

                        if (shomar_cha_chc == 0 && TechNumber == 12)//برای رسم در نقطه صفر زمان در  تکنیک سی اچ سیmn
                        {
                            this.OnlineChart.Series[0].Points.AddXY(0, volt_cha_chc[shomar_cha_chc]);
                            shomar_cha_chc++;
                            continue;
                        }

                        datas2_state[i] = 1;

                        if (TechNumber != 12)//mn
                            this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);
                        else//mn
                            this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);//CHC


                        shomar_cha_chc++;
                    }
                }
            }
        }

        private void showCPdata(int row)
        {
            if (this.InvokeRequired)
            {
                showDataon d = new showDataon(showCPdata);
                this.Invoke(d, new object[] { row });
            }
            else
            {
                byte Powr = 0;
                double C = 0, Crnt = 0;
                bool signe = false;
                int TechNumber = int.Parse(dschart1.chartlist1_run.Rows[row][1].ToString());

                Powr = datas2_2[shomar_cha_chc];
                Crnt = ((datas2_0[shomar_cha_chc] << 6) | (datas2_1[shomar_cha_chc] >> 2));
                if (Crnt > 0x1FFF)
                {
                    Crnt = (0x4000 - Crnt);
                    signe = true;
                }
                else
                    signe = false;

                if (Powr == 8)
                    C = Crnt * 1.02207 * Math.Pow(10, Powr);/////// - 6
                else
                    C = Crnt * 0.61035 * Math.Pow(10, Powr - 6); //// - 6
                if (signe ^ (TechNumber == 10 || TechNumber == 19))
                    C = C * -1;

                if (shomar_cha_chc >= time_cha_chc.Length)
                {
                    Array.Resize(ref time_cha_chc, time_cha_chc.Length + 200);
                    Array.Resize(ref volt_cha_chc, volt_cha_chc.Length + 200);
                }
                time_cha_chc[shomar_cha_chc] = shomar_cha_chc + 1;
                //time_cha_chc[shomar_cha_chc] = Math.Round((shomar_cha_chc + 1) * ti, 3, MidpointRounding.AwayFromZero);

                volt_cha_chc[shomar_cha_chc] = C;

                //رسم برای سه تکنیک  chp cha chc
                if (shomar_cha_chc == 0 && TechNumber != 12)//برای رسم در نقطه صفر زمان غیر از تکنیک سی اچ سیmn
                    this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc] - 1, volt_cha_chc[shomar_cha_chc]);

                int Scale = (shomar_cha_chc + 1);

                if (TechNumber == 11)
                {
                    time_cha_chc[shomar_cha_chc] /= 10;
                }
                else if (TechNumber == 10)//CP
                {
                    time_cha_chc[shomar_cha_chc] /= 5;
                }
                else if (TechNumber == 19)//Battery
                {
                    time_cha_chc[shomar_cha_chc] /= 5;
                }
                else if (TechNumber == 14)
                {
                    time_cha_chc[shomar_cha_chc] /= 10;
                }

                datas2_state[shomar_cha_chc] = 1;

                if (SampleRateMultiplier != 0)
                {
                    if (TechNumber == 11)
                        Scale %= (10 * SampleRateMultiplier);
                    else if (TechNumber == 10 || TechNumber == 19)//CP
                        Scale %= (5 * SampleRateMultiplier);

                    if (Scale == 0 && shomar_cha_chc != 0)
                    {
                        this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);
                    }
                }
                else
                {
                    this.OnlineChart.Series[0].Points.AddXY(time_cha_chc[shomar_cha_chc], volt_cha_chc[shomar_cha_chc]);
                }

                shomar_cha_chc++;
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

        private void OnlineChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) // وقتی دکمه وسط ماوس رها شد
            {
                isScrolling = false;
                OnlineChart.Cursor = Cursors.Default;
            }
            switch (statebutton)
            {
                case "zoomin":
                    var results11 = OnlineChart.HitTest(e.X, e.Y, false, ChartElementType.PlottingArea);
                    foreach (var result in results11)
                    {
                        if (result.ChartElementType == ChartElementType.PlottingArea)
                        {
                            //this.OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                            //double xMin = OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                            //double xMax = OnlineChart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                            //double yMin = OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                            //double yMax = OnlineChart.ChartAreas[0].AxisY.ScaleView.ViewMaximum;
                            //double posXStart = (OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - xMin) / 1.1;
                            //double posXFinish = (OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + xMax) / 1.1;
                            //double posYStart = (OnlineChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - yMin) / 1.1;
                            //double posYFinish = (OnlineChart.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + yMax) / 1.1;

                            //OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - posXStart, OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + posXFinish, DateTimeIntervalType.Auto, true);
                            //OnlineChart.ChartAreas[0].AxisY.ScaleView.Zoom(OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) - posYStart, OnlineChart.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.Y) + posYFinish, DateTimeIntervalType.Auto, true);

                            var chart = OnlineChart;
                            var xAxis = chart.ChartAreas[0].AxisX;
                            var yAxis = chart.ChartAreas[0].AxisY;

                            var xMin = xAxis.ScaleView.ViewMinimum;
                            var xMax = xAxis.ScaleView.ViewMaximum;
                            double difrance = xMax - xMin;
                            difrance = Math.Round(difrance / 4, 3);



                            //_zoomFrames.Push(new ZoomFrame { XStart = xMin, XFinish = xMax });
                            var pixelToValue = Math.Round(xAxis.PixelPositionToValue(e.Location.X), 3);
                            var posXStart = pixelToValue - difrance;
                            var posXFinish = pixelToValue + difrance;

                            xAxis.ScaleView.Zoom(posXStart, posXFinish);

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

        private void OnlineChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (isScrolling)
            {
                var chart = OnlineChart.ChartAreas[0];
                double xMin = chart.AxisX.ScaleView.ViewMinimum;
                double xMax = chart.AxisX.ScaleView.ViewMaximum;

                // محاسبه حرکت اسکرول در محور X
                double deltaX = (chart.AxisX.Maximum - chart.AxisX.Minimum) / chart.Position.Width;
                chart.AxisX.ScaleView.Scroll(xMin - deltaX * e.X / 10);
            }
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
            catch (Exception ex) { errors_cls err = new errors_cls(ex, txtLoger, 2); }
        }

        private void Showbtn_Click(object sender, EventArgs e)//این کلید برای نمایش آنلاین داده است
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
    }
}