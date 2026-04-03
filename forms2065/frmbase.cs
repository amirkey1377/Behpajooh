using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Skydat;
using System.Diagnostics;
using Skydat.formEIS;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO.Ports;
using System.Management;
using Skydat.forms2080;

namespace Skydat.forms2065
{
    public partial class frmbase : Form
    {
        //public string Version_No = "Version 8.2";

        private ManagementEventWatcher connectWatcher;//ایونت وصل شدن پورت COM
        private ManagementEventWatcher disconnectWatcher;//ایونت قطع شدن پورت COM

        frmPortOption2080 frm = new frmPortOption2080();
        public static SerialPort serial = null;

        bool Idata = true;
        public frmbase()
        {
            InitializeComponent();
        }

        private void frmbase_Load(object sender, EventArgs e)
        {
            StartMonitoring();//تعریف کردن ایونت برای قطع و وصل شدن USB
            //if (DeviceIsConnected() != null)
            //btn2066State();
            GetBackground();
            frm.RunLoadEvent(); // اجرای رویداد Load برای تنظیم Baud Rate
        }

        private void GetBackground()
        {
            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\back.jpg");
            }
            catch
            {
                DialogResult result = MessageBox.Show("تصویر زمینه در دایرکتوری مدنظر یافت نشد", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Retry)
                {
                    GetBackground();
                }
                else return;
            }
        }

        private void StartMonitoring()
        {
            try
            {
                WqlEventQuery connectQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
                connectWatcher = new ManagementEventWatcher(connectQuery);
                connectWatcher.EventArrived += new EventArrivedEventHandler(DeviceConnectedEvent);
                connectWatcher.Start();

                WqlEventQuery disconnectQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
                disconnectWatcher = new ManagementEventWatcher(disconnectQuery);
                disconnectWatcher.EventArrived += new EventArrivedEventHandler(DeviceDisconnectedEvent);
                disconnectWatcher.Start();
            }
            catch (Exception EX)
            {
                MessageBox.Show($"Error on monitoring COM event section: {EX.Message}","COM Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
        }

        private void DeviceConnectedEvent(object sender, EventArrivedEventArgs e)
        {
            btn2066State();
        }

        private void DeviceDisconnectedEvent(object sender, EventArrivedEventArgs e)
        {
            btn2066State();
        }

        private void btn2066State()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                try
                {
                    bool resualt = DeviceIsConnected();
                    if (resualt)
                    {
                        Btn2066.Enabled = true;
                    }
                    else
                        Btn2066.Enabled = false;
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("دستگاه به نرم افزار به درستی متصل نشده", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show($"btn2066 State Error:{Ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            
        }  

        public bool DeviceIsConnected()
        {
            List<string> names = ComPortNames("1a86", "7523");
            string[] x = SerialPort.GetPortNames();
            foreach (String c in x)
            {
                if (names.Contains(c))
                {
                    try
                    {
                        using (SerialPort serialPort = new SerialPort(c, 1000000))
                        {
                            serial = serialPort;
                            return true; // بازگشت نام پورت
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("به دلیل وجود مشکل در ارتباط دستگاه به نرم افزار متصل نشده، لطفا از اتصال صحیح کابل اطمینان حاصل فرمایید"
                            ,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        throw;
                    }
                }
            }
            return false;
        }

        List<string> ComPortNames(String VID, String PID)//این متد برای پیدا کردن رجیستری متناظر با پورتی هست که دستگاه به آن وصل شده برای اتوماتیک وصل کردن دستگاه
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

        private bool IsFrmOpen()
        {
            this.Cursor = Cursors.WaitCursor;
            if (classes2065.movedata.open_form == 1)// برای کارایی بهتر نرم افزار اجازه باز شدن چند فرم را نمی دهد
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            return false;
        }

        private void Btn2066_Click(object sender, EventArgs e)
        {
            Idata = false;
            if (IsFrmOpen())
                return;

            frmMain2065 frm = new frmMain2065(Idata)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frm.Show();
            classes2065.movedata.open_form = 1;
            this.Cursor = Cursors.Default;
        }

        private void Btn2090_Click(object sender, EventArgs e)
        {
            if (IsFrmOpen())
                return;

            frmMain12080 frm = new frmMain12080
            {
                StartPosition = FormStartPosition.CenterParent,
            };
            frm.Show();
            classes2065.movedata.open_form = 1;
            this.Cursor = Cursors.Default;
        }

        private void EIS_Click(object sender, EventArgs e)
        {
            IsFrmOpen();
            LandingEIS frm = new LandingEIS
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            classes2065.movedata.open_form = 1;
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            Idata = true;

            frmMain2065 frm = new frmMain2065(Idata)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void CustomizeBtn_Click(object sender, EventArgs e)
        {
            frmbackgroundset frm = new frmbackgroundset(this, 5)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            frm.ShowDialog();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Help.pdf",
                UseShellExecute = true,    // باز کردن با برنامهٔ پیش‌فرض ویندوز
                Verb = "open"              
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception s)
            {
                MessageBox.Show(s.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PortBtn_Click(object sender, EventArgs e)
        {
            forms2080.frmPortOption2080 prtopt = new forms2080.frmPortOption2080
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            prtopt.ShowDialog();
        }

        private void TechnicalBtn_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.behpajooh.net/") { UseShellExecute = true });
        }
    }
    public class TafelResult
    {
        public int startIndex, endIndex;
        public double slope_V_per_dec;   // b in V/dec
        public double slope_mV_per_dec;  // b in mV/dec
        public double intercept;         // a in eta = a + b*log10(i)
        public double r2;
        public double i0;                // exchange current (same units as input current density)
        public double alpha;             // transfer coefficient (dimensionless)
    }

    public static class Tafel
    {
        const double R = 8.314462618;     // J/(mol K)
        const double F = 96485.33212;     // C/mol

        // Main method: E (V), I (A or A/cm2), Ru (Ohm) optional, T(K) optional, n electron count
        public static List<TafelResult> Analyze(double[] E, double[] I,
            double area = 1.0, double Ru = 0.0, double T = 298.15, int n = 1,
            int minWindowPoints = 7, double minLogRange = 0.5, double minCurrentDensity = 1e-9)
        {
            if (E.Length != I.Length) throw new ArgumentException("E and I must same length");

            // 1) convert to current density (A/cm2) if area given (area in cm2)
            double[] j = I.Select(ii => ii / area).ToArray();

            // 2) IR correction if Ru provided: Ecorr = E - i * Ru
            double[] Ecorr = new double[E.Length];
            for (int i = 0; i < E.Length; i++)
                Ecorr[i] = E[i] - j[i] * Ru;

            // We'll produce results for anodic (j>0) and cathodic (j<0)
            var results = new List<TafelResult>();
            results.AddRange(AnalyzeBranch(Ecorr, j, positive: true, T: T, n: n, minWindowPoints: minWindowPoints, minLogRange: minLogRange, minCurrentDensity: minCurrentDensity));
            results.AddRange(AnalyzeBranch(Ecorr, j, positive: false, T: T, n: n, minWindowPoints: minWindowPoints, minLogRange: minLogRange, minCurrentDensity: minCurrentDensity));

            return results;
        }

        // Analyze one polarity branch
        static List<TafelResult> AnalyzeBranch(double[] E, double[] j, bool positive, double T, int n, int minWindowPoints, double minLogRange, double minCurrentDensity)
        {
            int N = E.Length;
            // gather indices that satisfy polarity and minimum magnitude
            var idx = new List<int>();
            for (int i = 0; i < N; i++)
            {
                if (positive)
                {
                    if (j[i] > minCurrentDensity) idx.Add(i);
                }
                else
                {
                    if (j[i] < -minCurrentDensity) idx.Add(i);
                }
            }

            var results = new List<TafelResult>();
            if (idx.Count < minWindowPoints) return results;

            // prepare arrays: x = log10(|j|), y = Ecorr (or eta)
            double[] xAll = idx.Select(i => Math.Log10(Math.Abs(j[i]))).ToArray();
            double[] yAll = idx.Select(i => E[i]).ToArray();
            int M = xAll.Length;

            // sliding window scan to find best linear fit (maximize R2 and require log range)
            double bestR2 = -1.0;
            int bestStart = -1, bestEnd = -1;
            double bestSlope = 0, bestIntercept = 0;

            for (int start = 0; start <= M - minWindowPoints; start++)
            {
                for (int end = start + minWindowPoints - 1; end < M; end++)
                {
                    int len = end - start + 1;
                    // check log span
                    double logMin = xAll.Skip(start).Take(len).Min();
                    double logMax = xAll.Skip(start).Take(len).Max();
                    if (logMax - logMin < minLogRange) continue;

                    var (slope, intercept, r2) = LinearFit(xAll, yAll, start, end);
                    if (r2 > bestR2)
                    {
                        bestR2 = r2;
                        bestStart = start;
                        bestEnd = end;
                        bestSlope = slope;
                        bestIntercept = intercept;
                    }
                }
            }

            if (bestStart >= 0)
            {
                // produce result
                var res = new TafelResult
                {
                    startIndex = idx[bestStart],
                    endIndex = idx[bestEnd],
                    slope_V_per_dec = bestSlope,
                    slope_mV_per_dec = bestSlope * 1000.0,
                    intercept = bestIntercept,
                    r2 = bestR2
                };

                // i0 from intercept: intercept = a, a + b*log10(i0) = 0 => log10(i0) = -a/b
                // be careful if b ~= 0
                if (Math.Abs(bestSlope) > 1e-12)
                {
                    double log10_i0 = -bestIntercept / bestSlope;
                    res.i0 = Math.Pow(10.0, log10_i0);
                }
                else res.i0 = double.NaN;

                // alpha from b = 2.303 R T / (alpha n F)
                res.alpha = (2.303 * R * T) / (Math.Abs(bestSlope) * n * F); // use abs(slope) to get positive alpha

                results.Add(res);
            }

            return results;
        }

        // simple linear fit y = slope * x + intercept on xAll[start..end], yAll...
        static (double slope, double intercept, double r2) LinearFit(double[] xAll, double[] yAll, int start, int end)
        {
            int n = end - start + 1;
            double sx = 0, sy = 0, sxx = 0, sxy = 0, syy = 0;
            for (int k = start; k <= end; k++)
            {
                double x = xAll[k], y = yAll[k];
                sx += x; sy += y;
                sxx += x * x;
                sxy += x * y;
                syy += y * y;
            }
            double xm = sx / n, ym = sy / n;
            double ssxx = sxx - n * xm * xm;
            double ssxy = sxy - n * xm * ym;
            double ssyy = syy - n * ym * ym;

            double slope = ssxx == 0 ? 0.0 : ssxy / ssxx;
            double intercept = ym - slope * xm;
            double r2 = (ssyy == 0) ? 1.0 : (ssxy * ssxy) / (ssxx * ssyy);
            return (slope, intercept, r2);
        }
    }

}
