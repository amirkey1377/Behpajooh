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

        bool Idata = true;
        public frmbase()
        {
            InitializeComponent();
        }

        private void frmbase_Load(object sender, EventArgs e)
        {
            StartMonitoring();//تعریف کردن ایونت برای قطع و وصل شدن USB
            //if (DeviceIsConnected() != null)
            btn2066State();
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

                            return true; // بازگشت نام پورت
                        }
                    }
                    catch (Exception ex)
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

        private void IsFrmOpen()
        {
            this.Cursor = Cursors.WaitCursor;
            if (classes2065.movedata.open_form == 1)// برای کارایی بهتر نرم افزار اجازه باز شدن چند فرم را نمی دهد
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Btn2066_Click(object sender, EventArgs e)
        {
            Idata = false;
            IsFrmOpen();

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
            IsFrmOpen();
            forms2080.frmMain12080 frm = new forms2080.frmMain12080
            {
                StartPosition = FormStartPosition.CenterScreen
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
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\Help.pdf");
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
            Process.Start("https://behpajooh.net/");
        }
    }
}
