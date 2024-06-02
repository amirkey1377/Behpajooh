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

namespace Skydat.forms2065
{
    public partial class frmbase : Form
    {
        //public string Version_No = "Version 8.2";
        bool Idata = true;
        public frmbase()
        {
            InitializeComponent();
        }

        private void frmbase_Load(object sender, EventArgs e)
        {
            try  
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\back.jpg"); 
            }
            catch { }
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

        private void Btn2066_Click(object sender, EventArgs e)
        {
            Idata = false;
            this.Cursor = Cursors.WaitCursor;
            if (classes2065.movedata.open_form == 1)// برای کارایی بهتر نرم افزار اجازه باز شدن چند فرم را نمی دهد
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> names = ComPortNames("1a86", "7523");
            if (names.Count > 0)// نرم افزار به صورت اتوماتیک اتصال دستگاه را تشخیص میدهد
            {
                string[] x = SerialPort.GetPortNames();
                int match = 0;
                foreach (String c in x)
                {
                    if (names.Contains(c))
                    {
                        try
                        {
                            serialPort2.PortName = c;
                            serialPort2.BaudRate = 1000000;
                            match = 1;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("مشکل در ارتباط");
                        }
                    }
                }

                if (match != 1)
                {

                    MessageBox.Show("Please make sure the cable connected to device properly and then try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    return;
                }

                frmMain2065 frm = new frmMain2065(Idata);
                frm.Show();
                frm.StartPosition = FormStartPosition.CenterScreen;
                classes2065.movedata.open_form = 1;
                this.Cursor = Cursors.Default;
            }
        }

        private void btnFRA_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if(classes2065.movedata.open_form == 1)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            forms2080.frmMain12080 frm = new forms2080.frmMain12080();
            frm.StartPosition = FormStartPosition.CenterScreen;
            classes2065.movedata.open_form = 1;
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void EIS_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (classes2065.movedata.open_form == 1)// برای کارایی بهتر نرم افزار اجازه باز شدن چند فرم را نمی دهد
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LandingEIS frm = new LandingEIS();
            frm.StartPosition = FormStartPosition.CenterScreen;
            classes2065.movedata.open_form = 1;
            frm.Show();
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            Idata = true;
            this.Cursor = Cursors.WaitCursor;
            if (classes2065.movedata.open_form == 1)// برای کارایی بهتر نرم افزار اجازه باز شدن چند فرم را نمی دهد
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("You have already opened a window. Close it to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmMain2065 frm = new frmMain2065(Idata);
            frm.Show();
            frm.StartPosition = FormStartPosition.CenterScreen;
            classes2065.movedata.open_form = 1;
            this.Cursor = Cursors.Default;
        }

        private void CustomizeBtn_Click(object sender, EventArgs e)
        {
            frmbackgroundset frm = new frmbackgroundset(this, 5);
            frm.StartPosition = FormStartPosition.CenterScreen;
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
            forms2080.frmPortOption2080 prtopt = new forms2080.frmPortOption2080();
            prtopt.StartPosition = FormStartPosition.CenterScreen;
            prtopt.ShowDialog();
        }

        private void TechnicalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("chrome.exe", "https://behpajooh.net/");
        }


    }
}
