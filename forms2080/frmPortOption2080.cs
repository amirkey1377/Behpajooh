using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Skydat;


namespace Skydat.forms2080
{
    public partial class frmPortOption2080 : Form
    {
        public static int Global_baudRate = 115200;
        private string configFilePath;

        public frmPortOption2080()
        {
            InitializeComponent();
            // تنظیم مسیر فایل در دایرکتوری نصب نرم‌افزار
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            configFilePath = Path.Combine(appPath, "Config.txt");

        }

        public void RunLoadEvent()
        {
            OnLoad(EventArgs.Empty); // اجرای رویداد Load به‌صورت دستی
        }

        //        MainForm mform = MainForm();// = MainForm();
        public bool isOK = false;

        private void button1_Click(object sender, EventArgs e)
        {
            cmbPrtName.Text = "";// "COM5";
            txtBRate.Text = "1000000";
            txtRT.Text = "1000";
            txtWT.Text = "1000";       
        }

         private void button2_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
      
        private void cmbPrtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                button1_Click(button1, e);
            if (e.KeyValue == 27)
                button2_Click(button2, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                sp1.PortName = cmbPrtName.Text;
                sp1.BaudRate = int.Parse(txtBRate.Text);
                Global_baudRate = int.Parse(txtBRate.Text);
                sp1.ReadTimeout = int.Parse(txtRT.Text);
                sp1.WriteTimeout = int.Parse(txtWT.Text);
                if (!sp1.IsOpen)
                    sp1.Open();
                Properties.Settings.Default.portname = sp1.PortName;
                Properties.Settings.Default.baudrate = txtBRate.Text.Trim();
                Properties.Settings.Default.rtport = txtRT.Text.Trim();
                Properties.Settings.Default.wtport = txtWT.Text.Trim();
                Properties.Settings.Default.Save();
                isOK = true;
                sp1.DiscardInBuffer();
                sp1.DiscardOutBuffer();
                sp1.Close();
                lblpn.Text = Properties.Settings.Default.portname;
                lblbr.Text = Properties.Settings.Default.baudrate;
                lblrt.Text = Properties.Settings.Default.rtport;
                lblwt.Text = Properties.Settings.Default.wtport;
                MessageBox.Show("parameters setting was done successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Port '" + sp1.PortName + "' Can not be opened..." + Environment.NewLine + ex.Message, "Warning");

            }
        }

        private void frmPortOption2080_Load(object sender, EventArgs e)
        {
            cmbPrtName.Text = sp1.PortName;
            txtBRate.Text = sp1.BaudRate.ToString();
            txtRT.Text = sp1.ReadTimeout.ToString();
            txtWT.Text = sp1.WriteTimeout.ToString();
           
            foreach (string str in System.IO.Ports.SerialPort.GetPortNames())
            {
                ((ComboBox)cmbPrtName).Items.Add(str);
            }
            lblpn.Text = Properties.Settings.Default.portname;
            lblbr.Text = Properties.Settings.Default.baudrate;
            lblrt.Text = Properties.Settings.Default.rtport;
            lblwt.Text = Properties.Settings.Default.wtport;

            LoadBaudRate();
        }

        private void LoadBaudRate()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    string content = File.ReadAllText(configFilePath);
                    if (int.TryParse(content, out int baudRate))
                    {
                        Global_baudRate = baudRate;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در خواندن فایل تنظیمات: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // نمایش مقدار در فرم (در صورت نیاز)
            txtBRate.Text = Global_baudRate.ToString();
        }

        private void SaveBaudRate()
        {
            try
            {
                File.WriteAllText(configFilePath, Global_baudRate.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ذخیره فایل تنظیمات: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBRate_Validated(object sender, EventArgs e)
        {
            if (int.TryParse(txtBRate.Text, out int newBaudRate) && newBaudRate != Global_baudRate)
            {
                Global_baudRate = newBaudRate;
                SaveBaudRate(); // ذخیره مقدار جدید
            }
        }
    }
}