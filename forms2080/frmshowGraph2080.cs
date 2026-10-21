using Microsoft.Win32;
using Skydat.classes2065;
using System;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Skydat.forms2080
{
    public partial class frmshowGraph2080 : Form
    {

        public static int ChartScaleMultiplier = 2;
        public static int SampleRateMultiplier = 1;

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

            OnlineChart.ChartAreas[0].Area3DStyle.Enable3D = false;

            OnlineChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            OnlineChart.ChartAreas[0].AxisX.LineColor = Color.Black;
            OnlineChart.ChartAreas[0].AxisY.LineColor = Color.Black;

            OnlineChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            //OnlineChart.ChartAreas[0].AxisY.Title = "coulomb(c)";
            //OnlineChart.ChartAreas[0].AxisX.Title = "Time(s)";
            OnlineChart.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            OnlineChart.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

            OnlineChart.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
            this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            this.OnlineChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            this.OnlineChart.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            this.OnlineChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Black;
            this.OnlineChart.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.Black;
            OnlineChart.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.Black;
            OnlineChart.ChartAreas[0].AxisY.ScrollBar.BackColor = Color.Black;
            this.OnlineChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            this.tvTechniqueQueue.ExpandAll();

            thread_grf = new Thread(Run_key);

            //therad_incomebuffer = new Thread(ReadIncomBuffer);
            //therad_incomebuffer.Name = "readSerial";
        }

        private double Analiz_CT1 = 0, Analiz_CT2 = 0, Analiz_CT3 = 0, Analiz_CP1 = 0, Analiz_CP2 = 0, Analiz_CP3 = 0, Analiz_CP4 = 0, Analiz_CP5 = 0, Analiz_CT4 = 0, Analiz_CT5 = 0;
        private byte Range1;
        private string statebutton = "";
        private Thread thread_grf;
        public delegate void showOnlineData1(double v, double i1, double i2, int row);

        public bool OCP_Ok = false;
        public int OCP_Timer = 0;
        public drawline_chart drawline1;
        public frmMain12080 mainf;
        public frmOption12080 opForm;

        // int shomar_cha_chc = 0;

        bool smothState = true;

        public string SerialState = null;

        private void GraphForm_Load(object sender, EventArgs e)
        {
            try
            {
                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[4].ToString().Substring(color[4].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                OnlineChart.BackColor = cl;
                OnlineChart.BorderlineColor = cl;
                OnlineChart.ChartAreas[0].BackColor = cl;
                panel1.BackColor = cl;
                pnlChartTools.BackColor = cl;

                drawline1 = new drawline_chart();
                if (opForm.cbDrawLine.Checked)
                    Set_line_Setting();
                else
                    drawline1.Active = false;

                int[] Xvalue = { 0, 1, 2, 3, 4, 5 };
                int[] Yvalue = { 0, 1, 2, 3, 4, 5 };

                for (int i = 0; i < Yvalue.Length; i++)
                {
                    OnlineChart.Series[0].Points.AddXY(Xvalue[i], Yvalue[i]);
                }

                try
                {
                    DeviceIsConnected();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }

            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

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
                            using (sp1)
                            {
                                sp1.PortName = c;
                                //frmPortOption2065 frm = new frmPortOption2065();
                                sp1.BaudRate = 57600;
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
            for (int i = tvTechniqueQueue.Nodes[0].Nodes.Count - 1; i >= 0; i--)
                tvTechniqueQueue.Nodes[0].Nodes[i].Remove();
            rtbParameters.Text = "";
            for (int i = OnlineChart.Series.Count - 1; i >= 0; i--)
                OnlineChart.Series.RemoveAt(i);
            OnlineChart.Refresh();

        }



        delegate void SetfillparamCallback(ITechniques tech, int state);

        private void fillparam(ITechniques tech, int state)
        {
            if (state == 1)
            {
                SetfillparamCallback d = new SetfillparamCallback(fillparam);
                this.Invoke(d, new object[] { tech, 2 });
            }
            else
            {
                rtbParameters.Text = "";
                switch (tech.Name)
                {
                    case "NPV":
                        NPV techNPV = (NPV)tech;
                        rtbParameters.Text += "E1 =" + techNPV.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techNPV.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techNPV.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Pulse Width =" + techNPV.PulseWidth.Value.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techNPV.ScanRate.Value.ToString() + "\n";
                        rtbParameters.Text += "TStep =" + techNPV.TStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techNPV.EquilibriumTime.ToString() + "\n";
                        break;

                    case "DPV":
                        DPV techDPV = (DPV)tech;
                        rtbParameters.Text += "E1 =" + techDPV.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techDPV.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techDPV.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Pulse Width =" + techDPV.PulseWidth.Value.ToString() + "\n";
                        rtbParameters.Text += "Pulse Height =" + techDPV.PulseHeight.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techDPV.EquilibriumTime.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techDPV.ScanRate.ToString() + "\n";
                        rtbParameters.Text += "TStep =" + techDPV.TStep.Value.ToString() + "\n";
                        break;

                    case "SWV":
                        SWV techSWV = (SWV)tech;
                        rtbParameters.Text += "E1 =" + techSWV.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techSWV.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techSWV.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Pulse Height =" + techSWV.PulseHeight.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techSWV.EquilibriumTime.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techSWV.ScanRate.Value.ToString() + "\n";
                        rtbParameters.Text += "Frequency =" + techSWV.Frequency.Value.ToString() + "\n";
                        break;

                    case "CV":
                        CV techCV = (CV)tech;
                        rtbParameters.Text += "E1 =" + techCV.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techCV.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "E3 =" + techCV.E3.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techCV.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techCV.EquilibriumTime.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techCV.ScanRate.Value.ToString() + "\n";
                        rtbParameters.Text += "TStep =" + techCV.TStep.Value.ToString() + "\n";
                        break;

                    case "LSV":
                        LSV techLSV = (LSV)tech;
                        rtbParameters.Text += "E1 =" + techLSV.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techLSV.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techLSV.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techLSV.EquilibriumTime.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techLSV.ScanRate.Value.ToString() + "\n";
                        rtbParameters.Text += "TStep =" + techLSV.TStep.Value.ToString() + "\n";
                        break;

                    case "Tafel":
                        Tafel techTafel = (Tafel)tech;
                        rtbParameters.Text += "E1 =" + techTafel.E1.Value.ToString() + "\n";
                        rtbParameters.Text += "E2 =" + techTafel.E2.Value.ToString() + "\n";
                        rtbParameters.Text += "HStep =" + techTafel.HStep.Value.ToString() + "\n";
                        rtbParameters.Text += "Equilibrium Time =" + techTafel.EquilibriumTime.ToString() + "\n";
                        rtbParameters.Text += "Scan Rate =" + techTafel.ScanRate.Value.ToString() + "\n";
                        rtbParameters.Text += "TStep =" + techTafel.TStep.Value.ToString() + "\n";
                        break;

                }
            }
        }



        public int cycle_number_in_file = 1;//mn

        SeriesChartType CurrentGraphChartType = SeriesChartType.FastLine;
        bool FlagSmothEnded = true;

        unsafe void Run_key()
        {
            SerialState = "Run";
            AddTechniqueQueue(TechLine);

            for (int k = 0; k < TechLine.Count; k++)
            {
                StartingOfTech(k);//UI Changes in the Starting of Technique

                fillparam(TechLine[k], 1);//Write Technique Parameter

                ButtonStates(true);

                ChartStartingTechPreparation(TechLine[k]);

                RecivedPacketCounter = 0;

                try
                {
                    if(sp1.IsOpen)
                        sp1.DiscardInBuffer();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Run_key_Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StopFlag = false;
                EndTechFlag = false;
                StartFlag = false;
                techname = -1;
                ReceivedBytesThreshold = 4;
                ocpCount = 0;

                SetDelay(TechLine[k].EquilibriumTime);

                DeviceSpecification device = frmMain12080.Device;

                SendToUsb(TechLine[k], device);

                this.Invoke(new Action(() =>
                {
                    mainf.pbRunnigTech.Value = 0;
                    mainf.pbRunnigTech.Maximum = VoltageBuffer.Length;
                }));

                CurrentGraphChartType = TechLine[k].SeriesType;


                if (TechLine[k].Name == "OCP")
                {
                    OCP ocp = TechLine[k] as OCP;
                    this.Invoke(new Action(() =>
                    {
                        OnlineChart.ChartAreas[0].AxisX.Maximum = ocp.time;
                        OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                    }));

                    while (!EndTechFlag)
                    {
                        readOCP();
                        if (ocpCount - 1 == ocp.time)
                        {
                            Setfilllable($"تکنیک تمام شده", "sucses");
                            RecivedPacketCounter = 0;
                            EndTechFlag = true;
                            StartFlag = false;
                            break;
                        }
                    }
                }
                else if (TechLine[k].Name == "CV")
                {
                    CV cv = TechLine[k] as CV;
                    this.Invoke(new Action(() =>
                    {
                        graphPreSetup(cv.E1, cv.E2, cv.HStep);
                    }));

                    for (int i = 1; i <= cv.Cycles; i++)
                    {
                        this.Invoke(new Action(() =>
                        {
                            mainf.toolStripStatusLabel5.Text = (k + 1).ToString().Trim() + " - Running " + cv.Name + $" _ Cycle: {i}";
                        }));

                        CheckBuffer();
                        this.Invoke(new Action(() =>
                        {
                            //error control
                            if (!OnlineChart.Series.Any(s => s.Name == "live"))
                                return;

                            Series historySeries = new Series()
                            {
                                ChartType = OnlineChart.Series["live"].ChartType,
                                BorderWidth = 1,
                                Color = OnlineChart.Series["live"].Color
                            };

                            historySeries.Name = $"CV-{i + 1}";

                            // کپی نقاط
                            foreach (var p in OnlineChart.Series["live"].Points)
                            {
                                historySeries.Points.AddXY(p.XValue, p.YValues[0]);
                            }
                            OnlineChart.Series.Add(historySeries);

                            if (smothState)
                            {
                                Task.Run(() =>
                                {
                                    newSmoth(OnlineChart.Series.Count - 1);
                                });
                            }

                            OnlineChart.Series["live"].Points.Clear();
                            UpdateSeriesOpacity(OnlineChart);
                            OnlineChart.Series[0].Color = Color.FromArgb(255,
                            OnlineChart.Series[0].Color.R,
                            OnlineChart.Series[0].Color.G,
                            OnlineChart.Series[0].Color.B);
                            mainf.pbRunnigTech.Value = 0;


                        }));

                        if (StopFlag)
                        {
                            CV tech = TechLine[k] as CV;
                            tech.Cycles = OnlineChart.Series.Count - 1;
                            TechLine[k] = tech;

                            break;
                        }

                        if (i != cv.Cycles)
                        {
                            EndTechFlag = false;
                            StartFlag = true;
                            //techname = -1;
                            ReceivedBytesThreshold = 46;
                        }
                    }

                    this.Invoke(new Action(() =>
                    {
                        mainf.pbRunnigTech.Value = mainf.pbRunnigTech.Maximum;
                    }));

                }
                else if (TechLine[k].Name == "NPV")
                {
                    NPV npv = TechLine[k] as NPV;
                    this.Invoke(new Action(() =>
                    {
                        graphPreSetup(npv.E1, npv.E2, npv.HStep);
                    }));

                    if (device.Info.Model == "2066")
                    {
                        DoRun2(npv, npv.E1, npv.E2, npv.HStep, sp1);
                    }
                    else if (device.Info.Model == "2190")
                    {
                        CheckBuffer();
                    }

                    if (smothState)
                        newSmoth(0);
                }
                else if (TechLine[k].Name == "DPV")
                {
                    DPV dpv = TechLine[k] as DPV;
                    this.Invoke(new Action(() =>
                    {
                        graphPreSetup(dpv.E1, dpv.E2, dpv.HStep);
                    }));

                    CheckBuffer();

                    if (smothState)
                        newSmoth(0);
                }
                else if (TechLine[k].Name == "SWV")
                {
                    SWV swv = TechLine[k] as SWV;
                    this.Invoke(new Action(() =>
                    {
                        graphPreSetup(swv.E1, swv.E2, swv.HStep);
                    }));

                    CheckBuffer();

                    if (smothState)
                        newSmoth(0);
                }
                else if (TechLine[k].Name == "LSV")
                {
                    LSV lsv = TechLine[k] as LSV;
                    this.Invoke(new Action(() =>
                    {
                        graphPreSetup(lsv.E1, lsv.E2, lsv.HStep);
                    }));

                    CheckBuffer();

                    if (smothState)
                        newSmoth(0);
                }
                else if (TechLine[k].Name == "CA")
                {
                    CA ca = TechLine[k] as CA;

                    int totalPoint = (ca.T1 + ca.T2) * ca.Cycles;

                    if (ca.ColometryMode)
                    {
                        this.Invoke(new Action(() =>
                        {
                            OnlineChart.ChartAreas[0].AxisY.Title = "Q(t)";
                        }));

                    }
                    LastI = 0;
                    LastT = 0;
                    TotalQ[0] = 0;
                    AvgI = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    bool OverFlow = false;
                    double pos = 0;
                    int PeriodTime = ca.T1 + ca.T2;
                    double size = PeriodTime * ChartScaleMultiplier;

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        OnlineChart.ChartAreas[0].AxisX.Minimum = 0;
                        OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    });

                    if (PeriodTime * ca.Cycles > 7200)
                    {
                        OverFlow = true;
                    }

                    while (!EndTechFlag)
                    {
                        if (readCA(ca))
                            if (size <= (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum))
                                switch (OverFlow)
                                {
                                    case false:
                                        this.Invoke((MethodInvoker)delegate ()
                                        {
                                            double sample = 1;
                                            if (SampleRateMultiplier != 0)
                                            {
                                                sample = SampleRateMultiplier;
                                            }

                                            pos += sample;
                                            size += sample;
                                            OnlineChart.ChartAreas[0].AxisX.ScaleView.Zoom(pos, size);
                                        });
                                        break;

                                    case true:
                                        this.Invoke((MethodInvoker)delegate ()
                                        {
                                            if (OnlineChart.ChartAreas[0].AxisX.Maximum - OnlineChart.ChartAreas[0].AxisX.Minimum >= (PeriodTime * ChartScaleMultiplier))
                                            {
                                                OnlineChart.Series[0].Points.RemoveAt(0);
                                            }
                                            OnlineChart.ChartAreas[0].AxisX.Minimum = OnlineChart.Series[0].Points[0].XValue - 1;
                                            OnlineChart.ChartAreas[0].AxisX.Maximum = OnlineChart.Series[0].Points[OnlineChart.Series[0].Points.Count - 1].XValue;
                                        });
                                        break;
                                }


                        if (ocpCount > (totalPoint) * 10)
                            break;
                    }

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        mainf.pbRunnigTech.Value = mainf.pbRunnigTech.Maximum;
                       
                    });
                }
                else
                {
                    CheckBuffer();
                    if (smothState)
                        newSmoth(0);
                }

                Thread.Sleep(2000);
                if (smothState)
                {
                    while (!FlagSmothEnded) ;
                }

                ITechniques technique = TechLine[k].Clone();

                TechLine[k].SeriesPath = BackupData(technique);

                Setgrfnodetext(k, 2, 1);

                Setbtnenabled(true, 1, 1);

                this.Invoke(new Action(() =>
                {
                    mainf.toolStripStatusLabel5.ForeColor = Color.DarkCyan;
                    mainf.pbRunnigTech.Value = mainf.pbRunnigTech.Maximum;
                    mainf.pbRunnigTech.ForeColor = Color.DarkCyan;
                    mainf.toolStripStatusLabel5.Text = "Finshed";
                    sp1.Close();
                }));
                Thread.Sleep(500);


                if (StopFlag)
                    break;
            }

        }

        private void AddTechniqueQueue(List<ITechniques> TechniqueQueue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<List<ITechniques>>(AddTechniqueQueue), TechniqueQueue);
                return;
            }

            this.tvTechniqueQueue.Nodes[0].Nodes.Clear();
            for (int i = 0; i < TechLine.Count; i++)
            {
                this.tvTechniqueQueue.Nodes[0].Nodes.Add(TechniqueQueue[i].Name);
            }
            tvTechniqueQueue.ExpandAll();
        }

        private void StartingOfTech(int NumberOfTechInQueueList)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<int>(StartingOfTech), NumberOfTechInQueueList);
                return;
            }

            tvTechniqueQueue.Nodes[0].Nodes[NumberOfTechInQueueList].ForeColor = Color.Red;

            mainf.toolStripStatusLabel5.Text = "";
            mainf.toolStripStatusLabel5.ForeColor = Color.Green;

            mainf.toolStripStatusLabel5.Text = (NumberOfTechInQueueList + 1).ToString().Trim() + " - Running technique " + TechLine[NumberOfTechInQueueList].Name;

        }

        private void ButtonStates(bool StateRun)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(ButtonStates), StateRun);
                return;
            }

            mainf.stf.tsbRun.Enabled = !StateRun;
            mainf.stf.tsbNew.Enabled = !StateRun;
            mainf.stf.runToolStripMenuItem1.Enabled = !StateRun;

            tsbhold.Enabled = StateRun;
            tsbstop.Enabled = StateRun;
        }

        private void ChartStartingTechPreparation(ITechniques RuningTech)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<ITechniques>(ChartStartingTechPreparation), RuningTech);
                return;
            }

            if (OnlineChart.Series.Count > 0)
            {
                OnlineChart.Series.Clear();
            }

            OnlineChart.ChartAreas[0].AxisX.Maximum = double.NaN;
            OnlineChart.ChartAreas[0].AxisX.Minimum = double.NaN;
            OnlineChart.ChartAreas[0].AxisY.Maximum = double.NaN;
            OnlineChart.ChartAreas[0].AxisY.Minimum = double.NaN;

            OnlineChart.ChartAreas[0].AxisX.Title = RuningTech.XY_Axis_Title[1];
            OnlineChart.ChartAreas[0].AxisY.Title = RuningTech.XY_Axis_Title[0];

        }


        private void SetDelay(int Delay)
        {

            if (Delay > 0)
            {
                string txt = mainf.toolStripStatusLabel5.Text;

                this.Invoke(new Action(() =>
                {
                    mainf.toolStripStatusLabel5.ForeColor = Color.DarkRed;
                    mainf.toolStripStatusLabel5.Text = "Equilibrium Time";
                    mainf.pbRunnigTech.ForeColor = Color.DarkRed;
                    mainf.pbRunnigTech.Value = 0;
                    mainf.pbRunnigTech.Maximum = Delay;
                }));

                for (int i = 0; i < Delay; i++)
                {
                    Thread.Sleep(1000);
                    this.Invoke(new Action(() =>
                    {
                        mainf.pbRunnigTech.Value += 1;
                    }));
                    if (StopFlag)
                    {
                        //Setbtnenabled(false, 1, 1);
                        EndTechFlag = false;
                        StopFlag = false;
                        break;
                    }
                    if (tsbhold.Text == "Countinue")
                    {
                        while (tsbhold.Text == "Countinue") ;
                    }

                }

                this.Invoke(new Action(() =>
                {
                    mainf.pbRunnigTech.ForeColor = Color.DarkCyan;
                    mainf.toolStripStatusLabel5.ForeColor = Color.Green;
                    mainf.toolStripStatusLabel5.Text = txt;
                }));

            }


        }

        private string BackupData(ITechniques tech)
        {
            string targetDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile2190\\";

            // چک کردن وجود دایرکتوری و ساختن آن
            if (!Directory.Exists(targetDirectory))
            {
                // این متد تمام سلسله مراتب پوشه‌ها را در صورت نیاز می‌سازد
                Directory.CreateDirectory(targetDirectory);
            }

            // 3. تبدیل داده به فرمت CSV
            string csvContent = ConvertSeriesToCsv(tech);

            // 4. ذخیره فایل با پسوند .csv
            string fileName = $"{tech.Name}_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}" +
                $"_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.csv";
            string fullPath = Path.Combine(targetDirectory, fileName);

            try
            {
                File.WriteAllText(fullPath, csvContent, Encoding.UTF8);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "BackupData_Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return fullPath;
        }

        public string ConvertSeriesToCsv(ITechniques Tech, string delimiter = ",", bool includeHeader = true)
        {
            var sb = new StringBuilder();

            if (Tech.Number == 1)//NPV
            {
                NPV npvTech = Tech as NPV;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "PulseWidth",
                "EquilibriumTime",
                "Comment"
                }));
                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{npvTech.Name}",
                $"{npvTech.ColorCode}",
                $"{npvTech.E1.Value}",
                $"{npvTech.E2.Value}",
                $"{npvTech.ScanRate.Value}",
                $"{npvTech.HStep.Value}",
                $"{npvTech.PulseWidth.Value}",
                $"{npvTech.EquilibriumTime}",
                $"{npvTech.Comment}",
                }));
            }
            else if (Tech.Number == 2)//DPV
            {
                DPV dpvTech = Tech as DPV;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "Pulse Width",
                "Pulse Height",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{dpvTech.Name}",
                $"{dpvTech.ColorCode}",
                $"{dpvTech.E1.Value}",
                $"{dpvTech.E2.Value}",
                $"{dpvTech.ScanRate.Value}",
                $"{dpvTech.HStep.Value}",
                $"{dpvTech.PulseWidth.Value}",
                $"{dpvTech.PulseHeight.Value}",
                $"{dpvTech.EquilibriumTime}",
                $"{dpvTech.Comment}",
                }));
            }
            else if (Tech.Number == 3)//SWV
            {
                SWV swvTech = Tech as SWV;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "Pulse Height",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{swvTech.Name}",
                $"{swvTech.ColorCode}",
                $"{swvTech.E1.Value}",
                $"{swvTech.E2.Value}",
                $"{swvTech.ScanRate.Value}",
                $"{swvTech.HStep.Value}",
                $"{swvTech.PulseHeight.Value}",
                $"{swvTech.EquilibriumTime}",
                $"{swvTech.Comment}",
                }));
            }
            else if (Tech.Number == 4)//CV
            {
                CV cvTech = Tech as CV;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "Cycles",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{cvTech.Name}",
                $"{cvTech.ColorCode}",
                $"{cvTech.E1.Value}",
                $"{cvTech.E2.Value}",
                $"{cvTech.ScanRate.Value}",
                $"{cvTech.HStep.Value}",
                $"{cvTech.Cycles}",
                $"{cvTech.EquilibriumTime}",
                $"{cvTech.Comment}",
                }));
            }
            else if (Tech.Number == 5)//LSV
            {
                LSV lsvTech = Tech as LSV;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{lsvTech.Name}",
                $"{lsvTech.ColorCode}",
                $"{lsvTech.E1.Value}",
                $"{lsvTech.E2.Value}",
                $"{lsvTech.ScanRate.Value}",
                $"{lsvTech.HStep.Value}",
                $"{lsvTech.EquilibriumTime}",
                $"{lsvTech.Comment}",
                }));
            }
            else if (Tech.Number == 8)//Tafel
            {
                Tafel tafelTech = Tech as Tafel;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "Scan Rate",
                "HStep",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{tafelTech.Name}",
                $"{tafelTech.ColorCode}",
                $"{tafelTech.E1.Value}",
                $"{tafelTech.E2.Value}",
                $"{tafelTech.ScanRate.Value}",
                $"{tafelTech.HStep.Value}",
                $"{tafelTech.EquilibriumTime}",
                $"{tafelTech.Comment}",
                }));
            }
            else if (Tech.Number == 6)//OCP
            {
                OCP ocpTech = Tech as OCP;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "Time",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{ocpTech.Name}",
                $"{ocpTech.ColorCode}",
                $"{ocpTech.time.Value}",
                $"{ocpTech.EquilibriumTime}",
                $"{ocpTech.Comment}",
                }));
            }
            else if (Tech.Number == 7)//CA
            {
                CA caTech = Tech as CA;
                //// 📌 نوشتن پارامتر های تکنیک
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "TechName",
                "Color",
                "E1",
                "E2",
                "T1",
                "T2",
                "Cycle",
                "EquilibriumTime",
                "Comment"
                }));

                sb.AppendLine(string.Join(delimiter, new[]
                {
                $"{caTech.Name}",
                $"{caTech.ColorCode}",
                $"{caTech.E1}",
                $"{caTech.E2}",
                $"{caTech.T1}",
                $"{caTech.T2}",
                $"{caTech.Cycles}",
                $"{caTech.EquilibriumTime}",
                $"{caTech.Comment}",
                }));
            }

            // 📌 نوشتن هدر (نام ستون‌ها)
            if (includeHeader)
            {
                sb.AppendLine(string.Join(delimiter, new[]
                {
                "PointIndex",
                "XValue",
                "YValues",
                "Cycle",
                "IsEmpty"
                }));
            }


            foreach (Series series in OnlineChart.Series)
            {
                if (series == null)
                    throw new ArgumentNullException(nameof(series));

                int Cycle = OnlineChart.Series.IndexOf(series);

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
                    i.ToString(),
                    xValue,
                    yValues,
                    Cycle.ToString(),
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
            }

            return sb.ToString();
        }

        private void graphPreSetup(double E1, double E2, double Hstep)
        {
            double min = Math.Min(E1, E2) * 1000;
            double max = Math.Max(E1, E2) * 1000;

            OnlineChart.ChartAreas[0].AxisX.Maximum = max;
            OnlineChart.ChartAreas[0].AxisX.Minimum = min;
            double intervalBase = Hstep * 1000; // مضرب دلخواه

            double range = max - min;

            // حدود 12 برچسب
            double desiredStep = range / 15.0;

            // نزدیک‌ترین مضرب intervalBase
            double axisInterval =
                Math.Ceiling(desiredStep / intervalBase) * intervalBase;

            OnlineChart.ChartAreas[0].AxisX.Interval = axisInterval;

        }

        private void UpdateSeriesOpacity(Chart chart)
        {
            int count = chart.Series.Count;

            for (int i = 0; i < count; i++)
            {
                Series s = chart.Series[i];

                int opacity = 100 - ((count - 1 - i) * 10);

                if (opacity < 10)
                    opacity = 5;

                Color c = s.Color;

                s.Color = Color.FromArgb(
                    (int)(255 * opacity / 100.0),
                    c.R,
                    c.G,
                    c.B);
            }
        }

        public void newSmoth(int ChartSeriesIndex)
        {
            try
            {
                FlagSmothEnded = false;

                List<double> xValues = new List<double>();
                List<double> yValues = new List<double>();

                if (OnlineChart.Series.Count == 0)
                {
                    return;
                }
                Series series = OnlineChart.Series[ChartSeriesIndex];

                foreach (DataPoint point in series.Points)
                {
                    xValues.Add(point.XValue);
                    yValues.Add(point.YValues[0]); // چون ممکنه DataPoint چند مقدار Y داشته باشه (مثل نمودار stack یا bubble)، YValues[0] می‌گیریم
                }



                // 1. اعمال Median Filter (برای حذف اعوجاج تیز)
                var medianFiltered = MedianFilter(yValues, windowSize: 5);

                List<double> smothedX = SavitzkyGolaySmooth(medianFiltered, windowSize: 9);

                var area = OnlineChart.ChartAreas[0];

                double ymin = area.AxisY.Minimum;
                double ymax = area.AxisY.Maximum;

                this.Invoke(new Action(() =>
                {
                    //OnlineChart.SuspendLayout();
                    //series.Points.SuspendUpdates();
                    series.Points.Clear();

                    area.AxisY.Minimum = ymin;
                    area.AxisY.Maximum = ymax;

                }));

                //Thread.Sleep(10);


                var points = xValues
                .Select((x, i) => new { X = x, Y = smothedX[i] })
                .ToList();

                int batchSize = 10;

                for (int start = 0; start < xValues.Count; start += batchSize)
                {
                    int end = Math.Min(start + batchSize, xValues.Count);

                    this.Invoke(new Action(() =>
                    {
                        for (int i = start; i < end; i++)
                        {
                            series.Points.AddXY(xValues[i], smothedX[i]);
                        }

                        //series.Points.ResumeUpdates();
                        //OnlineChart.ResumeLayout();
                    }));

                    Thread.Sleep(20); // حس انیمیشن
                }

                FlagSmothEnded = true;

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "newSmoth_Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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


        private int TotalByteExpectedToRead;
        private int[] VoltageBuffer;
        private void SendToUsb(ITechniques tech, DeviceSpecification device)
        {
            try
            {
                if (!sp1.IsOpen)
                {
                    sp1.Open();
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "SendToUsb_Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (device.Info.Model == "2066")
            {
                string CommondRun = $"@{tech.Number}@";
                long Temp = (long)Analiz_CP1;
                CommondRun += Temp.ToString() + "@";
                Temp = (long)Analiz_CP2;
                CommondRun += Temp.ToString() + "@";
                Temp = (long)Analiz_CP3;
                CommondRun += Temp.ToString() + "@";
                Temp = (long)Analiz_CT1;
                CommondRun += Temp.ToString() + "@";
                Temp = (long)Analiz_CT2;
                CommondRun += Temp.ToString() + "@";
                Temp = (long)Analiz_CT3;
                CommondRun += Temp.ToString() + "@";
                Temp = tech.EquilibriumTime;//quesent time
                CommondRun += Temp.ToString() + "@";

                tech.SendToUsb_For_2066(ref CommondRun);

                sp1.BaudRate = 1000000;
                sp1.Write(CommondRun);

                VoltageBuffer = new int[100];
            }
            else if (device.Info.Model == "2190")
            {
                byte[] buffer = new byte[1];


                VoltageBuffer = tech.SendToUsb(ref TotalByteExpectedToRead, ref buffer);


                sp1.Write(buffer, 0, buffer.Length);

            }

        }

        int ReceivedBytesThreshold = 4;

        int ocpCount = 0;

        private void readOCP()
        {

            int bytesAvailable = sp1.BytesToRead;
            int Erorr = 0;
            if (bytesAvailable > 0)
            {
                Erorr = 0;
                while (bytesAvailable < 6)
                {
                    bytesAvailable = sp1.BytesToRead;
                    Thread.Sleep(10);
                    Erorr++;
                    if (Erorr >= 1000 || !sp1.IsOpen)
                    {
                        //ارور
                        Setfilllable($"bytesAvailable <= ReceivedBytesThreshold error", "error");
                        //return;
                    }
                    if (EndTechFlag)
                    {
                        return;
                    }
                }

                if (!StartFlag)
                {
                    //Thread.Sleep(10);
                    byte[] buff = new byte[4];
                    sp1.Read(buff, 0, 4);
                    if (buff[0] == 31 && buff[1] == 1 && buff[2] == 1 && buff[3] == 1)
                    {
                        StartFlag = true;
                        Setfilllable("Start", "sucses");
                        return;
                    }
                    else
                    {
                        //دوباره بفرست
                        Setfilllable("دوباره بفرست", "error");
                        return;
                    }
                }

                if (bytesAvailable > 6) bytesAvailable = 6;
                byte[] buffer = new byte[bytesAvailable];


                sp1.Read(buffer, 0, bytesAvailable);

                int TechNum = buffer[0];

                if (techname != -1)
                {
                    if (techname != TechNum)
                    {
                        Setfilllable("بافر به درستی خوانده نمیشود!", "error");
                        return;
                    }
                }
                else
                {
                    techname = TechNum;
                }

                byte CheckSum = buffer[1];
                for (int i = 0; i < 3; i++)
                {
                    CheckSum ^= buffer[i + 2];
                }

                double[] DataY = new double[1];
                int[] DataX = new int[1];


                if (CheckSum == buffer[5])
                {
                    DataY[0] = ((Int32)((buffer[1] << 24) | (buffer[2] << 16) | (buffer[3] << 8) | buffer[4])) * (-1 / 10.0);
                    DataX[0] = ocpCount;

                    PlotGraph(OnlineChart, DataX, DataY);

                    ocpCount++;

                }
                else
                {
                    Setfilllable("چکسام درست نیست", "error");
                    return;
                }

            }

        }

        private double[] TotalQ = new double[1] { 0 };

        private double[] AvgI = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private double LastI { get; set; } = 0;

        private double LastT { get; set; } = 0;

        private bool readCA(CA RunTech)
        {
            int bytesAvailable = sp1.BytesToRead;
            int Erorr = 0;
            if (bytesAvailable > 0)
            {
                Erorr = 0;
                while (bytesAvailable < 7)
                {
                    bytesAvailable = sp1.BytesToRead;
                    Thread.Sleep(10);
                    Erorr++;
                    if (Erorr >= 1000 || !sp1.IsOpen)
                    {
                        //ارور
                        Setfilllable($"bytesAvailable <= ReceivedBytesThreshold error", "error");
                        //return;
                    }
                    if (EndTechFlag)
                    {
                        return false;
                    }
                }

                if (!StartFlag)
                {
                    //Thread.Sleep(10);
                    byte[] buff = new byte[4];
                    sp1.Read(buff, 0, 4);
                    if (buff[0] == 31 && buff[1] == 1 && buff[2] == 1 && buff[3] == 1)
                    {
                        StartFlag = true;
                        Setfilllable("Start", "sucses");
                        return false;
                    }
                    else
                    {
                        //دوباره بفرست
                        Setfilllable("دوباره بفرست", "error");
                        return false;
                    }
                }

                if (bytesAvailable > 7) bytesAvailable = 7;
                byte[] buffer = new byte[bytesAvailable];


                int test = sp1.Read(buffer, 0, bytesAvailable);

                int TechNum = buffer[0];

                if (techname != -1)
                {
                    if (techname != TechNum)
                    {
                        Setfilllable("بافر به درستی خوانده نمیشود!", "error");
                        return false;
                    }
                }
                else
                {
                    techname = TechNum;
                }

                byte CheckSum = buffer[1];
                for (int i = 0; i < 4; i++)
                {
                    CheckSum ^= buffer[i + 2];
                }

                double[] DataY = new double[1];
                double[] DataX = new double[1];
                double[] Avg = new double[1];

                if (CheckSum == buffer[6])
                {
                    DataY[0] = ((Int32)((buffer[2] << 24) | (buffer[3] << 16) | (buffer[4] << 8) | buffer[5])) * (-1 / 10.0);
                    DataX[0] = Math.Round(ocpCount / 10.0, 1);
                    ocpCount++;


                    int sample = 1;
                    if (SampleRateMultiplier != 0)
                    {
                        sample = SampleRateMultiplier;
                    }

                    int i = (int)(DataX[0] * 10) % 10;
                    AvgI[i] = DataY[0];

                    if (i == 0)
                    {
                        int validDataCount = AvgI.Count(x => x != 0);

                        if (validDataCount >= 6)
                        {
                            if (RunTech.ColometryMode)
                            {
                                double[] sortedAvgI = (double[])AvgI.Clone();
                                Array.Sort(sortedAvgI);
                                Array.Reverse(sortedAvgI);

                                double sum = 0;
                                for (int k = 2; k < sortedAvgI.Length - 2; k++)
                                {
                                    sum += sortedAvgI[k];
                                }
                                double currentI = sum / (sortedAvgI.Length - 4);
                                double currentT = DataX[0];

                                double DeltaI = (currentI + LastI);
                                if (currentI - LastI == 0)
                                    DeltaI = 0;
                                else
                                    DeltaI /= 2.0;

                                double deltaT = currentT - LastT;

                                TotalQ[0] = TotalQ[0] + (DeltaI * deltaT);
                                TotalQ[0] = Math.Round(TotalQ[0], 4);

                                LastI = currentI;
                                LastT = currentT;

                                double[] abs = new double[1] { TotalQ[0] };

                                if (DataX[0] % sample == 0)
                                {
                                    PlotGraph(OnlineChart, DataX, abs, sample);
                                    return true;
                                }

                            }
                            else
                            {
                                double[] sortedAvgI = (double[])AvgI.Clone();
                                Array.Sort(sortedAvgI);
                                Array.Reverse(sortedAvgI);

                                double sum = 0;
                                for (int k = 2; k < sortedAvgI.Length - 2; k++)
                                {
                                    sum += sortedAvgI[k];
                                }
                                Avg[0] = sum / (sortedAvgI.Length - 4);
                                Avg[0] = Math.Round(Avg[0], 4);
                                if (DataX[0] % sample == 0)
                                {
                                    PlotGraph(OnlineChart, DataX, Avg, sample);
                                    return true;
                                }
                                LastT = DataX[0];
                            }
                        }
                    }




                }
                else
                {
                    Setfilllable("چکسام درست نیست", "error");
                    return false;
                }


            }

            return false;


        }

        void CheckBuffer()
        {
            try
            {
                while (!EndTechFlag)
                {
                    int bytesAvailable = sp1.BytesToRead;
                    int Erorr = 0;
                    while (bytesAvailable == 0)
                    {
                        Thread.Sleep(10);
                        Erorr++;
                        if (Erorr >= 1000 || !sp1.IsOpen)
                        {
                            //ارور
                            Setfilllable($"bytesAvailable == 0 error", "error");
                            Erorr = 0;
                            //return;
                        }
                        bytesAvailable = sp1.BytesToRead;
                        if (EndTechFlag)
                        {
                            return;
                        }
                    }

                    if (bytesAvailable > 0)
                    {
                        Erorr = 0;
                        while (bytesAvailable < ReceivedBytesThreshold)
                        {
                            bytesAvailable = sp1.BytesToRead;
                            Thread.Sleep(10);
                            Erorr++;
                            if (Erorr >= 1000 || !sp1.IsOpen)
                            {
                                //ارور
                                Setfilllable($"bytesAvailable <= ReceivedBytesThreshold error", "error");
                                //return;
                            }
                            if (EndTechFlag)
                            {
                                return;
                            }
                        }

                        ReadIncomBuffer(bytesAvailable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int RecivedPacketCounter = 0;
        bool StopFlag = false, EndTechFlag = false, StartFlag = false;
        int techname = -1;

        void ReadIncomBuffer(int ByteToRead)
        {

            try
            {
                // برای مواقعی که تکنیک تمام شده ولی هنوز پس از ان ایونت انفاق میوفتد
                if (EndTechFlag == true)
                {
                    Setfilllable($"DataReceived Happen but EndFlag true", "warning");
                    return;
                }

                Setfilllable($"ReadIncomBuffer", "log");
                if (ByteToRead <= 40)
                {
                    Setfilllable($"بافر زیر ۴۰ خوانده شده {ByteToRead}", "log");
                }
                int ExpectedByteToRead = TotalByteExpectedToRead - (RecivedPacketCounter * 40);
                if (ExpectedByteToRead <= 40)
                {
                    //تکنیک تمام شده
                    Setfilllable($"زیر ۴۰{ExpectedByteToRead}", "log");
                }

                //int errorCount = 0;
                //while (ByteToRead == 0 && sp1.IsOpen)
                //{
                //    ByteToRead = sp1.BytesToRead;
                //    errorCount++;
                //    if (errorCount >= 2000)
                //    {
                //        if (errorCount / 1000 == 0)
                //        {
                //            Setfilllable($"{errorCount}", "warning", 1);
                //        }
                //        if (errorCount == 5000)
                //            return;
                //    }
                //    //Thread.Sleep(1);
                //}
                if (!sp1.IsOpen)
                {
                    MessageBox.Show("پورت سریال باز نیست", "sp1_DataReceived_Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!StartFlag)
                {
                    //Thread.Sleep(10);
                    byte[] buff = new byte[4];
                    sp1.Read(buff, 0, 4);
                    if (buff[0] == 31 && buff[1] == 1 && buff[2] == 1 && buff[3] == 1)
                    {
                        StartFlag = true;
                        Setfilllable("Start", "sucses");
                        //تنظیم میکنیم که چه زمانی اینتراپت دیتا بخورد
                        if (ExpectedByteToRead <= 40)
                            ReceivedBytesThreshold = ExpectedByteToRead + 6;
                        else
                            ReceivedBytesThreshold = 46;

                        return;
                    }
                    else
                    {
                        //دوباره بفرست
                        Setfilllable("دوباره بفرست", "error");
                        return;
                    }
                }

                //Thread.Sleep(10);
                //ByteToRead = sp1.BytesToRead;
                if (ByteToRead < 2)
                {
                    Setfilllable("پکت زیر دو بایت است!", "warning");
                    ByteToRead = sp1.BytesToRead;
                }

                if (ByteToRead > ReceivedBytesThreshold) ByteToRead = ReceivedBytesThreshold;
                byte[] buffer = new byte[ReceivedBytesThreshold];


                int test = sp1.Read(buffer, 0, ReceivedBytesThreshold);

                int TechNum = buffer[0];

                if (techname != -1)
                {
                    if (techname != TechNum)
                    {
                        Setfilllable("بافر به درستی خوانده نمیشود!", "error");
                        return;
                    }
                }
                else
                {
                    techname = TechNum;
                }

                uint ByteNum = (uint)((buffer[1] << 24) | (buffer[2] << 16) | (buffer[3] << 8) | buffer[4]);
                if (ByteNum == ExpectedByteToRead)
                    RecivedPacketCounter++;
                else
                {
                    Setfilllable($"پکت به درستی گرفته نشد {RecivedPacketCounter}", "error");
                    return;
                }



                if (ByteNum >= 40)
                    ByteNum = 40;

                //حالت خطایابی
                if ((ByteNum + 6) > ByteToRead)
                {

                    Setfilllable($"حالت خطایابی {ByteNum},{ByteToRead}", "warning");

                    int diff = (int)ByteNum + 6 - ByteToRead;
                    while (ByteNum + 6 >= ByteToRead)
                    {
                        ByteToRead = sp1.BytesToRead;

                    }
                    int i = buffer.Length - 1;
                    Array.Resize(ref buffer, (int)ByteNum + 6);

                    byte[] diffBuffer = new byte[diff];
                    sp1.Read(diffBuffer, 0, diff);
                    foreach (byte data in diffBuffer)
                    {
                        i++;
                        buffer[i] = data;
                    }
                }

                byte CheckSum = buffer[5];
                for (int i = 0; i < ByteNum - 1; i++)
                {
                    CheckSum ^= buffer[i + 6];
                }

                double[] DataY = new double[ByteNum / 4];
                int[] DataX = new int[ByteNum / 4];

                if (CheckSum == buffer[ByteNum + 5])
                {
                    //MessageBox.Show("اطلاعات به درستی ذخیره شده");
                    for (int i = 0; i < ByteNum / 4; i++)
                    {
                        DataY[i] = ((Int32)((buffer[i * 4 + 5] << 24) | (buffer[i * 4 + 6] << 16) | (buffer[i * 4 + 7] << 8) | buffer[i * 4 + 8])) * -1 / 10.00;
                        DataX[i] = VoltageBuffer[i + ((RecivedPacketCounter - 1) * 10)];
                    }
                    //if (TechNum == 2)//DPV
                    //{
                    //    double[] PulsyDataY = new double[DataY.Length / 2];
                    //    DataX = new int[PulsyDataY.Length];
                    //    for (int i = 0,j = 0; i < DataY.Length; i += 2,j++)
                    //    {
                    //        PulsyDataY[j] = DataY[i + 1] - DataY[i];
                    //        DataX[j] = VoltageBuffer[j + ((RecivedPacketCounter - 1) * 5)];
                    //    }
                    //    PlotGraph(OnlineChart, DataX, PulsyDataY);
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < ByteNum / 4; i++)
                    //    {
                    //        DataX[i] = VoltageBuffer[i + ((RecivedPacketCounter - 1) * 10)];
                    //    }
                    //    PlotGraph(OnlineChart, DataX, DataY);
                    //}

                    PlotGraph(OnlineChart, DataX, DataY);

                    if (ExpectedByteToRead <= 40)
                    {
                        //تکنیک تمام شده
                        Setfilllable($"تکنیک تمام شده", "sucses");
                        RecivedPacketCounter = 0;
                        EndTechFlag = true;
                        StartFlag = false;

                        this.Invoke(new Action(() =>
                        {
                            mainf.pbRunnigTech.Value = mainf.pbRunnigTech.Maximum;
                        }));
                    }

                }
                else
                {
                    Setfilllable("چکسام درست نیست", "error");
                    return;
                }

                ExpectedByteToRead = TotalByteExpectedToRead - (RecivedPacketCounter * 40);
                if (ExpectedByteToRead <= 40)
                {
                    //تکنیک تمام شده
                    ReceivedBytesThreshold = ExpectedByteToRead + 6;
                    Setfilllable($"پکت بعدی زیر ۴۰{ExpectedByteToRead}", "log");
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "DataRecive_Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //throw;
            }

        }

        public void PlotGraph(Chart chart, double[] xValues, double[] yValues, int sample)
        {
            if (chart == null || xValues == null || yValues == null)
                return;

            if (chart.InvokeRequired)
            {
                chart.Invoke(new Action(() => PlotGraph(chart, xValues, yValues, sample)));
                return;
            }

            Series series;

            if (chart.Series.IndexOf("live") == -1)
            {
                series = new Series("live")
                {
                    ChartType = CurrentGraphChartType,
                    BorderWidth = 3,
                    Color = Color.Red
                };

                chart.Series.Add(series);
            }
            else
            {
                series = chart.Series["live"];
            }

            chart.SuspendLayout();

            try
            {
                series.Points.SuspendUpdates();

                // 1. اعمال Median Filter (برای حذف اعوجاج تیز)
                var medianFiltered = MedianFilter(yValues.ToList(), windowSize: 3);

                List<double> smothedX = SavitzkyGolaySmooth(medianFiltered, windowSize: 5);


                for (int i = 0; i < yValues.Length; i++)
                {
                    series.Points.AddXY(xValues[i], smothedX[i]);
                }
                mainf.pbRunnigTech.Value += sample;

                series.Points.ResumeUpdates();
            }
            finally
            {
                chart.ResumeLayout();
                chart.Invalidate();
            }
        }

        public void PlotGraph(Chart chart, int xValue, double yValue)
        {
            if (chart == null)
                return;

            if (chart.InvokeRequired)
            {
                chart.Invoke(new Action(() => PlotGraph(chart, xValue, yValue)));
                return;
            }

            Series series;

            if (chart.Series.IndexOf("live") == -1)
            {
                series = new Series("live")
                {
                    ChartType = CurrentGraphChartType,
                    BorderWidth = 3,
                    Color = Color.Red
                };

                chart.Series.Add(series);
            }
            else
            {
                series = chart.Series["live"];
            }

            chart.SuspendLayout();

            try
            {
                series.Points.SuspendUpdates();

                series.Points.AddXY(xValue, yValue);

                mainf.pbRunnigTech.Value++;
                if (mainf.pbRunnigTech.Value == 100)
                    mainf.pbRunnigTech.Value = 0;

                if (!flag_running)
                {
                    mainf.pbRunnigTech.Visible = false;
                    mainf.xyLabel.Visible = true;
                    flag_running = false;

                    OnlineChart.Update();
                }

                series.Points.ResumeUpdates();
            }
            finally
            {
                chart.ResumeLayout();
                chart.Invalidate();
            }

        }

        public void PlotGraph(Chart chart, int[] xValues, double[] yValues)
        {
            if (chart == null || xValues == null || yValues == null)
                return;

            if (chart.InvokeRequired)
            {
                chart.Invoke(new Action(() => PlotGraph(chart, xValues, yValues)));
                return;
            }

            Series series;

            if (chart.Series.IndexOf("live") == -1)
            {
                series = new Series("live")
                {
                    ChartType = CurrentGraphChartType,
                    BorderWidth = 3,
                    Color = Color.Red
                };

                chart.Series.Add(series);
            }
            else
            {
                series = chart.Series["live"];
            }

            chart.SuspendLayout();

            try
            {
                series.Points.SuspendUpdates();

                // 1. اعمال Median Filter (برای حذف اعوجاج تیز)
                var medianFiltered = MedianFilter(yValues.ToList(), windowSize: 3);

                List<double> smothedX = SavitzkyGolaySmooth(medianFiltered, windowSize: 5);


                for (int i = 0; i < yValues.Length; i++)
                {
                    series.Points.AddXY(xValues[i], smothedX[i]);
                }
                mainf.pbRunnigTech.Value += yValues.Length;

                series.Points.ResumeUpdates();
            }
            finally
            {
                chart.ResumeLayout();
                chart.Invalidate();
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
                    pnlChartTools.BackColor = cl.Color;

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

        private void frmshowGraph2080_Resize(object sender, EventArgs e)
        {
            timerResizeFrm.Stop();
            timerResizeFrm.Start();
        }

        private void timerResizeFrm_Tick(object sender, EventArgs e)
        {
            timerResizeFrm.Stop();
            toolStrip1.Height = (int)(this.Height * 0.1);
            tvTechniqueQueue.Height = (int)(this.Height * 0.3);
            pnlParam.Height = (int)(this.Height * 0.4);
            pnlChartTools.Width = (int)(OnlineChart.Width * 0.03);
            foreach (Button item in pnlChartTools.Controls)
            {
                item.Height = item.Width;
            }
            //rtbLogger.Height = (int)(this.Height * 0.2);
        }

        List<ITechniques> TechLine;

        public void runf(List<ITechniques> techLine, byte rng, double cp11, double ct11, double cp12, double ct12, double cp13, double ct13, double cp14, double ct14, double cp15, double ct15)
        {
            TechLine = techLine;
            this.tvTechniqueQueue.Nodes[0].Nodes.Clear();

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


            thread_grf = new Thread(Run_key);//mn
            thread_grf.Start();
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


        delegate void SetgrfnodeCallback(int txt, int code, int state);
        private void Setgrfnodetext(int txt, int code, int state)
        {
            try
            {
                switch (code)
                {

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

                            this.tvTechniqueQueue.Nodes[0].Nodes[txt].ForeColor = Color.Green;
                            rtbParameters.Text += "\t\t*******************\nE1:" + OnlineChart.Series[OnlineChart.Series.Count - 1].Points.FindMinByValue("X").XValue.ToString("000e-0") +
                                "\nE2:" + OnlineChart.Series[OnlineChart.Series.Count - 1].Points.FindMaxByValue("X").XValue.ToString("000e-0") + "\nMin. I:" + OnlineChart.Series[OnlineChart.Series.Count - 1].Points.FindMinByValue("Y").YValues[0].ToString("000e-0") +
                                "\nMax. I:" + OnlineChart.Series[OnlineChart.Series.Count - 1].Points.FindMaxByValue("Y").YValues[0].ToString("000e-0");

                            //ITechniques techPass = TechLine[txt].Clone();
                            //techPass.Series = TechLine[txt].Series;
                            //ITechniques tech = TechLine[txt].Clone();
                            mainf.grf.AddTechnique(TechLine[txt]);

                        }
                        break;
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            
            }
        }

        private void Setfilllable(string txt, string title = "log")
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string, string>(Setfilllable), txt, title);
                return;
            }

            Color color;

            switch (title.ToLowerInvariant())
            {
                case "warning":
                    color = Color.Yellow;
                    break;

                case "error":
                    color = Color.Red;
                    break;

                case "sucses":
                    color = Color.Green;
                    break;

                case "log":
                    color = Color.White;
                    break;

                default:
                    color = Color.Gray;
                    break;
            }

            clasglobal.AppendText(
                rtbLogger,
                txt + Environment.NewLine,
                color);
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
                        this.tvTechniqueQueue.SelectedNode = this.tvTechniqueQueue.Nodes[0].Nodes[row];
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
                        OnlineChart.Refresh();
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
                        mainf.stf.tsbRun.Enabled = bl;
                        mainf.stf.tsbNew.Enabled = bl;
                        mainf.stf.runToolStripMenuItem1.Enabled = bl;
                        tsbhold.Enabled = !bl;
                        tsbstop.Enabled = !bl;

                        //mainf.pbRunnigTech.Visible = !bl;
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

                        mainf.pbRunnigTech.Visible = false;

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
                        mainf.pbRunnigTech.Visible = true;
                    }
                    break;
            }
        }

        private void itemSmothState_Click(object sender, EventArgs e)
        {
            if (itemSmothState.Checked)
            {
                smothState = true;
            }
            else
            {
                smothState = false;
            }
        }

        private void tsbhold_Click(object sender, EventArgs e)
        {
            //ToolBarButton btn = sender as ToolBarButton;
            if (tsbhold.Text == "Pause")// حالت Pause
            {
                byte[] buufer = new byte[] { 32, 1, 2, 2 };
                sp1.Write(buufer, 0, 4);
                tsbhold.Text = "Countinue";
                tsbhold.Image = Properties.Resources.Play_icon;
                tsbstop.Enabled = false;
                mainf.toolStripStatusLabel5.Text = "Pause";

                SerialState = "Pause";
            }
            else if (tsbhold.Text == "Countinue")//حالت continue
            {
                byte[] buufer = new byte[] { 32, 1, 1, 1 };
                sp1.Write(buufer, 0, 4);
                tsbhold.Text = "Pause";
                tsbhold.Image = Properties.Resources.Pause_icon;
                tsbstop.Enabled = true;
                mainf.toolStripStatusLabel5.Text = "Run";

                SerialState = "Run";
            }

        }


        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buufer = new byte[] { 32, 1, 3, 3 };
                if (sp1.IsOpen)
                    sp1.Write(buufer, 0, 4);
                Setbtnenabled(true, 1, 1);
                EndTechFlag = true;
                StopFlag = true;
                mainf.toolStripStatusLabel5.Text = "Stop";

                SerialState = "Stop";

            }
            catch (Exception)
            {
                throw;
            }
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
                        mainf.xyLabel.Text = x111.ToString("#.##") + " | " + y111.ToString("#.#e-0");

                        break;
                }

            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, rtbParameters, 2); }


        }

        private void tsbshowd_Click(object sender, EventArgs e)
        {
            forms2065.frmshowdata frm = new forms2065.frmshowdata();
            frm.listView1.Items.Clear();

            for (int i = 0; i < OnlineChart.Series["live"].Points.Count; i++)
            {
                frm.listView1.Items.Add((i + 1) + "          " + OnlineChart.Series["live"].Points[i].XValue + "          " + OnlineChart.Series["live"].Points[i].YValues[0].ToString());
            }

            //int r = tChart1.Series.Count -1;

            //for (int i = 0; i < tChart1.Series[r].Points.Count; i++)
            //{
            //    frm.listView1.Items.Add(tChart1.Series[r].Points[i].XValue + "          " + tChart1.Series[r].Points[i].YValues[0].ToString());
            //}
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

            OnlineChart.ChartAreas[0].CursorX.IsUserEnabled = state;
            OnlineChart.ChartAreas[0].CursorY.IsUserEnabled = state;
            OnlineChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = state;
            OnlineChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = state;
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

        private void DoRun2(ITechniques tech, double E1, double E2, double Hstep, SerialPort mainSerialPort, byte StepRange=0)
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
            int TechNumber = tech.Number;


            double ti = 0;
            bool S_et = true, pulsy = false, fst = false;
            bool ngtv = false, N_gtv;
            if (E1 < E2)
            {
                N_gtv = false;
            }
            else
            {
                N_gtv = true;
            }


            if (tech.Name == "NPV")
            {
                S_et = false;
            }
            else if (tech.Name == "DPV")
            {
                pulsy = true;
                ngtv =! N_gtv;
                S_et = false;
            }
            else if(tech.Name == "SWV")
            {
                pulsy = true;
                ngtv = N_gtv;
            }


            // اینوکر برای اینکه تابعی که در ترد(اصلی) دیگری اجرا می‌شود را به صورت متغییر فراخوانی کنیم
            this.Invoke(new Action(() =>
            {
                OnlineChart.ChartAreas[0].AxisX.ScaleView.ZoomReset(0);
                mainSerialPort.ReadTimeout = Timeout.Infinite;
                mainSerialPort.DiscardInBuffer();
                OnlineChart.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
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


            try
            {
                while (!EndTechFlag)
                {
                    data = ReadSerialData(out TechniqueEnded, ref sign, mainSerialPort);
                    if (data == null)
                    {
                        Setfilllable("داده های دریافتی از دستگاه خالی هستند و احتمالا دستگاه دچار مشکل شده", "log");

                        SerialReadError++;
                    }
                    else if (data.SequenceEqual(new byte[] { 000, 222, 111 }))
                    {
                        if (!string.Equals(SerialState, "Stop", StringComparison.OrdinalIgnoreCase))
                            MessageBox.Show("تکنیک به درستی اجرا نمیشود");
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
                                            I1 = C[j - 1];
                                            I2 = C[j];
                                        }
                                        else  ////if ngtv == false
                                        {
                                            I2 = C[j];
                                            I1 = C[j - 1];
                                        }
                                    }
                                }

                            }
                            else//if pulsy == false
                            {
                                I1 = C[j];
                                I2 = 0;
                            }

                            int numdigit = Hstep.ToString("0.############################")
                                                .Split('.')[1]
                                                .Length;// calculate digit number in of heght step

                            if (!(pulsy && evn))
                                if (N_gtv)
                                {
                                    Vold = V;
                                    V = Math.Round(V - Hstep, numdigit);
                                }
                                else
                                {
                                    Vold = V;
                                    V = Math.Round(V + Hstep, numdigit);
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

                                Runnig2(Vold, I1, I2);///// رسم گراف اصلی

                                if (DebugMode || Maintenance)//for monitoring purpse
                                {
                                    Setfilllable($"{Vold}:{I1}:{I2} ", "log");
                                    Setfilllable($"vold1{Vold} ", "log");
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

                                Runnig2(Vold, logI, I2);///// رسم گراف اصلی
                            }
                            else
                                Runnig2(Vold, I1, I2);///// رسم گراف اصلی

                            if (DebugMode || Maintenance)
                            {
                                Setfilllable($"{Vold}:{I1}:{I2} ", "log");
                                Setfilllable($"vold1{Vold} ", "log");

                            }
                        }

                    }

                    if (TechniqueEnded)  //if portc=0x09
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            if (mainSerialPort.IsOpen)
                            {
                                mainSerialPort.Write("end1");
                                Thread.Sleep(10);
                            }
                        }
                        TechniqueEnded = false;
                        EndTechFlag = true;
                        mainSerialPort.Close();
                        Thread.Sleep(100);
                        Setbtnenabled(true, 2, 1);
                        return;
                    }

                    if (string.Equals(SerialState, "Stop", StringComparison.OrdinalIgnoreCase))//if software == stoped 
                    {
                        HandleStopCondition(sp1);
                        Setfilllable("finish running DoRun2", "sucses");
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void HandleStopCondition(SerialPort mainSerialPort)
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
                Setfilllable($"HandleStopCondition UnauthorizedAccessException: {ex.Message}", "error");

            }
            catch (IOException ex)
            {
                MessageBox.Show("خطایی در ارتباط با پورت سریال رخ داده است. لطفاً اتصال سخت‌افزاری را بررسی کنید.",
                                "خطای I/O", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Setfilllable($"HandleStopCondition IOException: {ex.Message}", "error");

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("پورت سریال در وضعیت نامعتبری قرار دارد. لطفاً تنظیمات پورت را بررسی کنید.",
                                "خطای عملیات نامعتبر", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Setfilllable($"HandleStopCondition InvalidOperationException: {ex.Message}", "error");


            }
            catch (Exception ex)
            {
                MessageBox.Show("یک خطای ناشناخته رخ داده است. لطفاً برنامه را مجدداً اجرا کنید.",
                                "خطای نامشخص", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Setfilllable($"HandleStopCondition Exception: {ex.Message}", "error");

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
                        Setfilllable($"HandleStopCondition Failed to close the serial port: {ex.Message}", "error");
                    }
                }
            }
        }



        //دیباگ مود برای وقتی است که میخواهیم یرای تعمیر دستگاه استپ های ولتاژی را ببینیم
        private readonly bool DebugMode = false;
        private readonly bool Maintenance = false;
        private bool flag_running = false;

        public void Runnig2(double V, double I1, double I2)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<double, double, double>(Runnig2), V, I1, I2);
                return;
            }

            if (OnlineChart.Series.Count == 0)
            {
                OnlineChart.Series.Add("tst");
            }

            V *= 1000;
            PlotGraph(OnlineChart, (int)V, I1 - I2);

            
        }

        private static readonly object serialLock = new object();

        public byte[] ReadSerialData(out bool TechniqueEnded, ref int sign, SerialPort mainSerialPort)
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
                            {
                                if (DebugMode)
                                    Setfilllable($"مقدار بافر سریال اورفلو شده است = {bytesAvailable}", "log");

                            }
                            else
                            {
                                if (DebugMode)
                                    Setfilllable($"مقدار بافر سریال نزدیک به اورفلو شدن است = {bytesAvailable}", "log");
                            }
                        }

                        string receivedData = sp1.ReadLine();
                        if (string.IsNullOrWhiteSpace(receivedData))
                            return null;

                        if (receivedData.Contains("end"))
                        {
                            TechniqueEnded = true;
                            if (DebugMode) Setfilllable("TechniqueEnded", "sucses");
                        }
                        else if (receivedData.Contains("@"))
                        {
                            string[] parsedData = receivedData.Split('@');
                            if (parsedData.Length < 4)
                                throw new FormatException("Received data is incomplete or malformed.");

                            if (DebugMode) Setfilllable("TechniqueEnded", "log");

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
                                        Setfilllable($"c:{item}", "log");

                                }
                            }
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    if (DebugMode) Setfilllable($"ReadSerialData: Timeout error: {ex.Message}", "log");
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }
                catch (FormatException ex)
                {
                    if (DebugMode) Setfilllable($"ReadSerialData: Format error: {ex.Message}", "log");
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }
                catch (Exception ex)
                {
                    if (DebugMode) Setfilllable($"ReadSerialData: Unexpected error: {ex.Message}", "log");
                    data[0] = 000;
                    data[1] = 222;
                    data[2] = 111;
                }

                return data;
            }

        }

    }

}