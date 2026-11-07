
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows;
using Skydat.classes2065;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;

namespace Skydat.forms2065
{
    public partial class frmofflinestate : Form
    {
        byte ENQ0 = 5, ENQ1 = 7, ENQ2 = 9, ACK = 6, NAK = 21, TRANS_BUFF = 96;
        //byte ENQ0 = 5, ENQ1 = 7, ENQ2 = 9, ACK = 6, NAK = 15, DATA_READY = 1, TRANS_BUFF = 60;//delphi

        private Thread thread_grf;
        double[] othertech_val0 = new double[200];
        double[] othertech_val1 = new double[200];
        byte[] x_rec = new byte[4];
        private int shomar_val = 0, sh_state = 1,shomar_array=0;
        private byte[] buf = new byte[1];
        private byte[] data=new byte[1];
        int shomar_data=0;
        byte x1_r;
   
        public frmofflinestate()
        {
            InitializeComponent();
          
            tChart1.ChartAreas[0].BackColor = Color.Black;
            tChart1.ChartAreas[0].Area3DStyle.Enable3D = false;

            tChart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            tChart1.ChartAreas[0].AxisX.LineColor = Color.Black;
            tChart1.ChartAreas[0].AxisY.LineColor = Color.Black;

            tChart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
            tChart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;
            tChart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;

            tChart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            tChart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##";
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
        
             }
       
        private void GraphForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (sp1.IsOpen)
                    sp1.Close();

                cmbPrtName.Items.Clear();
                foreach (string s in SerialPort.GetPortNames())
                    cmbPrtName.Items.Add(s);


                //SerialPort port = new SerialPort(s);
                //port.Open();
                //if (port.IsOpen)
                //{
                //    cmbPrtName.Items.Add(s);
                //}
                //port.Close();
                cmbPrtName.Text = cmbPrtName.Items[0].ToString();
            }
            catch (Exception err) { MessageBox.Show(err.Message); }
             
            filltreeview3();

            try
            {
                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[3].ToString().Substring(color[3].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                tChart1.BackColor = cl;
                tChart1.BorderlineColor = cl;
                tChart1.ChartAreas[0].BackColor = cl;
                panel4.BackColor = cl;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void filltreeview3() {

            tChart1.Series[0].Points.Clear();
            label4.Text = "";           
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                sp1.Dispose();
                sp1.PortName = cmbPrtName.Text.Trim();
                if (!sp1.IsOpen)
                    sp1.Open();

                sh_state = 1;
                shomar_array = 0;
                Array.Clear(othertech_val0, 0, othertech_val0.Length);
                Array.Clear(othertech_val1, 0, othertech_val1.Length);
                Array.Clear(x_rec, 0, x_rec.Length);
                thread_grf = new Thread(Run_key);
                thread_grf.Start();

                tsbReceive.Enabled = false;
                tsbrun.Enabled = false;
                tsbshowdata.Enabled = true;
                tChart1.Series[0].Points.Clear();
                label2.Text = "";
                label4.Text = "";
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
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
                    panel4.BackColor = cl.Color;
                    
                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[3] = "2065 offline:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void tsbshowdata_Click(object sender, EventArgs e)
        {
            forms2065.frmshowdata frm = new forms2065.frmshowdata();
            frm.listView1.Items.Clear();
            for (int i = 0; i < tChart1.Series[0].Points.Count; i++)
            {
                frm.listView1.Items.Add(tChart1.Series[0].Points[i].XValue + "          " + tChart1.Series[0].Points[i].YValues[0]);
            }
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }

        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (tChart1.Series[0].Points.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "dat";
                    string dir_sfd = "";
                    sfd.Filter = "Data Files(*.dat)|*.dat";
                    if (dir_sfd != "")
                        sfd.InitialDirectory = dir_sfd;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ReadWrite.WriteToFile(dschart11.chartlist1_run.Rows[0], clasglobal.TechName[int.Parse(dschart11.chartlist1_run.Rows[0][1].ToString())], sfd.FileName, 0, 1);//header in file

                        StreamWriter sa1 = new StreamWriter(sfd.FileName, true);
                        sa1.WriteLine("============================================================");

                        string Ln = "";

                        for (int i = 0; i < tChart1.Series[0].Points.Count; i++)
                        {
                            Ln = tChart1.Series[0].Points[i].XValue.ToString();
                            Ln = Ln + "\t" + tChart1.Series[0].Points[i].YValues[0].ToString();
                            Ln = Ln + "\t" + tChart1.Series[0].Points[i].YValues[0].ToString();
                            Ln = Ln + "\t" + "0";
                            Ln = Ln + "\t";
                            sa1.WriteLine(Ln);
                        }

                        sa1.Close();
                        //dschart11.chartlist1_run.Rows[0][23] = sfd.FileName;

                        MessageBox.Show("Data saved.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show("There is not any graph for saving !!!");
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        unsafe void Run_key()
        {
            try
            {
                Int32 ii1 = 0;//در زبان دلفی که به اینجا منتقل شده است از نوع علامت دار بود. به همین علت در تکنیک سی اچ پی اینجا مقدار نهایی اختلاف داشت
                int t1, t2, t3, Pnum = 0, j = 0, /*ii1 = 0,*/ i = 0;
                int Elo = 0, Stp = 0;
                byte check = 0, Teq = 0;
                short[] Powr = new short[1];
                short[] Crnt = new short[1];

                bool pulsy = false, N_gtv = false, evn = false;
                string outstr = "";
                double SR0, i1 = 0, i2 = 0, ss = 0;
                if (!sp1.IsOpen)
                    sp1.Open();
                t1 = Environment.TickCount;
                buf[0] = 37;
                sp1.Write(buf, 0, 1);
                t3 = t1;
                while (sh_state == 1 && (Environment.TickCount <= (t1 + 15000)))
                {
                    t2 = Environment.TickCount;
                    if ((t2 - t3) >= 1000)
                    {
                        sp1.Write(buf, 0, 1);
                        t3 = t2;
                    }
                }
                if (sh_state == 1)
                {
                    Settxt("=> Error in receiving data_1" + Environment.NewLine, 2, 1);
                    Settxt("1" + Environment.NewLine, 4, 1);
                    Setgraphpoint(0, 0, 110, 0);//فعال کردن دکمه جرا

                    return;
                }
                Settxt("=> running & get receiving data" + Environment.NewLine, 2, 1);
                buf[0] = ACK;                                              //ACK
                sp1.Write(buf, 0, 1);
                while (sh_state != 3) ;//در حال دریافت

                if ((x_rec[0] == ENQ1) && ((x_rec[0] + x_rec[1] + x_rec[2]) % 256 == x_rec[3]))
                {
                    Pnum = x_rec[2] * 256 + x_rec[1];
                    buf[0] = ACK;
                    sp1.Write(buf, 0, 1);
                }
                else
                {
                    buf[0] = NAK;
                    sp1.Write(buf, 0, 1);
                    Settxt("=> Error in receiving data" + Environment.NewLine, 2, 1);//mn

                    return;
                }

                t1 = Environment.TickCount;
                while (sh_state != 4 || Environment.TickCount <= (t1 + 6000)) ;//در حال دریافت

                if (sh_state != 4)
                {
                    Settxt("=> Error in receiving data_2" + Environment.NewLine, 2, 1);
                    Settxt("1" + Environment.NewLine, 4, 1);
                    Setgraphpoint(0, 0, 110, 0);//فعال کردن دکمه جرا
                                       
                    return;
                }

                while (j < (Pnum + 2))
                {
                    if (j < (Pnum + 1))
                        check += data[j];

                    j++;
                }

                if ((data[0] == ENQ2) && (check == data[(Pnum + 1)]))
                {
                    buf[0] = ACK;
                    sp1.Write(buf, 0, 1);
                }
                else
                {
                    buf[0] = NAK;
                    sp1.Write(buf, 0, 1);
                    Settxt("=> Error in receiving data" + Environment.NewLine, 2, 1);//mn

                    return;
                }

                Settxt("=> receiving & get showing data" + Environment.NewLine, 2, 1);

                int Vry = 0;

                dschart11.chartlist1_run.Rows.Clear();//هنگام ذخیره فقط یک تکنیک را دارا باشدmn
                DataRow dr = dschart11.chartlist1_run.NewRow();


                if (x1_r != 27)  //Sarcheshma2//  
                {
                    Teq = data[3];
                    outstr = "Technic: " + clasglobal.TechName[Teq].Trim() + Environment.NewLine;
                    outstr = outstr + "===============================" + Environment.NewLine;
                    Settxt(Teq.ToString(), 3, 1);
                    dr[0] = 0;
                    dr[1] = Teq;
                    dr[21] = clasglobal.TechName[Teq].Trim();
                    Stp = (sbyte)data[4];
                    if (Teq < 8)
                    {
                        Vry = 0;
                        if (Math.Abs(Stp) > 25)
                        {
                            Vry = 1;
                            if (Stp < 0)
                                Stp |= 64;
                            else
                                Stp &= 31;
                        }
                        if (Stp < 0)
                            Stp = Stp * (-1);
                        //Stp = Math.Abs(Stp);
                    }

                    if (Stp > 0)
                        Vry = 1;

                    if (Teq == 9)
                        Vry = 0;
                }
                int IE, FE, E3 = 0, vv = 0;


                IE = BitConverter.ToInt16(new byte[] { data[5], data[6] }, 0);
                FE = BitConverter.ToInt16(new byte[] { data[7], data[8] }, 0);
                ii1 = BitConverter.ToInt16(new byte[] { data[9], data[10] }, 0);
              
                if (Teq < 8)
                {
                    i = 11;
                    if (Teq == 4)
                    {
                        E3 = BitConverter.ToInt16(new byte[] { data[11], data[12] }, 0);
                        i = 13;
                        Elo = IE;
                        if (E3 != IE)
                        {
                            
                            Elo = E3;
                        }
                    }

                    if ((Teq == 2) || (Teq == 3) || (Teq == 7))
                        pulsy = true;
                    else
                        pulsy = false;

                    if (IE < FE)
                    {
                        N_gtv = false;
                    }
                    else
                    {
                        N_gtv = true;
                    }
                    evn = true;
                }
                else if (Teq == 9)
                    i = 7;
                else
                    i = 11;

                if (Teq != 10)
                {
                    outstr = outstr + "E1(v)=" + ((double)IE / 1000).ToString() + Environment.NewLine;
                    dr[3] = ((double)IE / 1000);
                }
                else
                {
                    outstr = outstr + "I1(mA)=" + ((double)IE / 1000).ToString() + Environment.NewLine;
                    dr[17] = ((double)IE / 1000);

                }

                if (Teq != 9)
                {
                    if (Teq != 10)
                    {
                        outstr = outstr + "E2(v)=" + ((double)FE / 1000).ToString() + Environment.NewLine;
                        dr[4] = ((double)FE / 1000);

                    }
                    else
                    {
                        outstr = outstr + "I2(mA)=" + ((double)FE / 1000).ToString() + Environment.NewLine;
                        dr[18] = ((double)FE / 1000);
                    }

                }

                if (Teq == 4)
                {
                    outstr = outstr + "E3(v)=" + ((double)E3 / 1000).ToString() + Environment.NewLine;
                    dr[5] = ((double)E3 / 1000);

                    //  outstr="Cycles="+inttostr(Cycl)+"  Hold Time(s)="+inttostr(Hld_t)+"  SR2(v/s)="+floattostrf(SR2,ffgeneral,5,3);
                    //  
                }
                else if ((Teq == 2) || (Teq == 3) || (Teq == 7))
                {
                    outstr = outstr + "Hpuls(v)=";//+floattostrf(Hpuls/1000.,ffnumber,4,3);

                }

                if (Teq < 8)
                {
                    if (Stp != 0)
                    {
                        IE = IE + (FE - IE) % Stp;
                        E3 = E3 + (FE - E3) % Stp;
                    }
                    vv = IE;
                    if (Teq == 3)
                        ii1 = 1000 / ii1;
                    SR0 = (double)Stp / ii1;
                    if (ii1 == 10000)
                        SR0 = Stp / 0.5;

                    outstr = outstr + "Hstep(mv)=" + Stp.ToString() + Environment.NewLine;
                    outstr = outstr + "Tstep(s)=" + ((double)ii1 / 1000).ToString() + Environment.NewLine;
                    outstr = outstr + "Scan rate(mv/s)=" + (SR0 * 1000).ToString() + Environment.NewLine;
                    dr[6] = Stp;
                    dr[7] = 0;
                    dr[8] = SR0 * 1000;
                    dr[9] = ((double)ii1 / 1000).ToString();
                    dr[10] = ii1 / 1000;
                }
                else
                {
                    i2 = 0;
                    t1 = BitConverter.ToInt16(new byte[] { data[Pnum - 1], data[Pnum] }, 0);
                   
                    Pnum--;
                    if (Teq == 9)
                    {
                        ss = 0.02;
                        if (t1 > 100)
                            ss = ss * (t1 / 100.0);
                        outstr = outstr + "T1(s)=" + t1.ToString() + Environment.NewLine;

                        dr[15] = t1.ToString();
                    }
                    else
                    {
                        ii1 = ii1 / 10;
                        t1 = t1 / 10;
                        ss = 0.005;
                        if (t1 > 25) ss = ss * (t1 / 25.0);
                        outstr = outstr + "T1(s)=" + ii1.ToString() + Environment.NewLine + "T2(s)=" + (t1 - ii1).ToString() + Environment.NewLine;

                        dr[15] = ii1.ToString();
                        dr[16] = (t1 - ii1).ToString();
                    }


                    if (Teq == 10)
                    {
                        Vry = 1;
                        outstr = outstr + "Potential /mV";
                    }
                    else
                    {
                        if (Teq == 12)
                            outstr = outstr + "Charge /uC";
                        else
                            outstr = outstr + "Current /uA";
                    }

                }
                Settxt(outstr, 1, 1);

                j = i;
                Settxt("", 5, 1);

                dschart11.chartlist1_run.Rows.Add(dr);
                               
                while (j < Pnum)
                {                   
                    if (Teq < 8)
                    {
                        if ((pulsy) && (!evn))
                        {

                            x1_r = (byte)((int)data[j] & 15);
                            ii1 = (data[j + 1] << 4) | (data[j] >> 4);
                            if ((x1_r % 2) == 1)
                                ii1 = ii1 * -1; //invers curve
                            x1_r = (byte)((int)x1_r >> 1);
                            if (x1_r == 0)
                                x1_r = 8;

                            i2 = ii1 * Math.Pow(10, x1_r - 6);
                            Setgraphpoint((vv * 0.001), (i1 - i2), 1, 1);
                        }
                        else
                        {
                            x1_r = (byte)((int)data[j] & 15);
                            ii1 = ((data[j + 1] << 4) | (data[j] >> 4));
                            if ((x1_r % 2) == 1)
                                ii1 = ii1 * -1;
                            x1_r = (byte)((int)x1_r >> 1);
                            if (x1_r == 0)
                                x1_r = 8;

                            //yazdani
                            if ((Teq == 0) || (Teq == 4) || (Teq == 5) || (Teq == 6))  // shaemi
                                i1 = ii1 * Math.Pow(10, x1_r - 5);                             // shaemi
                            else                                                   // shaemi
                                i1 = ii1 * Math.Pow(10, x1_r - 6);

                            //Memo2.Text=Memo2.Text+"_"+IntToStr(ii1);

                            if (!pulsy)
                            {
                                if (Teq == 1)   // shaemi
                                    Setgraphpoint((vv * 0.001), (i1 * 10), 1, 1);
                                //outstr = (vv * 0.001).ToString("0.000") + "  " + (i1 * 10).ToString("0.000"); // shaemi
                                else
                                    Setgraphpoint((vv * 0.001), (i1), 1, 1);

                                //outstr = (vv * 0.001).ToString("0.000") + "  " + (i1).ToString("0.000");
                                //      outstr=floattostrf(vv*0.001,ffNumber,4,3)+"  "+inttostr(ii1)+"E"+inttostr(x1);

                            }
                        }
                        if (!(pulsy && evn))
                            if (N_gtv)
                                vv = vv - Stp;
                            else
                                vv = vv + Stp;
                        j = j + 2;
                        evn = !evn;
                        if (Teq == 4)
                        {
                            if ((vv == FE) || (vv == Elo))
                                N_gtv = !N_gtv;
                        }
                    }
                    else
                    {
                        if (((data[j + 1] == 255) && (data[j] == 255)) || ((data[j + 1] == 0) && (data[j] == 0)))
                        {
                            if (data[j] == 0)
                                Vry = 0;
                            else
                                Vry = 1;               
                        }
                        else
                        {
                            //i2 = i2 + ss;//قبلا این بود اما مقادیر زمان را در بخشهایی از زمان بسیار ریز میکرد
                            try { i2 = Math.Round(i2, 4) + ss; } catch { i2 = i2 + ss; }

                            if (Vry == 1)
                            {
                                ii1 = (Int16)(data[j + 1] * 256 + data[j]);//برای تکنیک سی اچ پی مقدار باید از نوع علامت دار باشد
                              
                                if (Teq == 9)
                                    i1 = ii1 * Math.Pow(10, 2);
                                else
                                    i1 = ii1;
                            }
                            else
                            {
                                x1_r = (byte)((int)data[j] & 15);
                               
                                ii1 = (Int32)((data[j + 1] << 4) | (data[j] >> 4));

                                if ((x1_r % 2) == 1)
                                    ii1 = ii1 * -1;

                                x1_r = (byte)((int)x1_r >> 1);

                                if (x1_r == 0)
                                    x1_r = 8;
                                i1 = ii1 * Math.Pow(10, x1_r - 6);
                            }

                            if (Teq == 10)  // shaemi
                                i1 = i1 * (-1);

                            Setgraphpoint(i2, i1, 1, 1);
                        }

                        j = j + 2;
                    }
                } //while
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }

            Setgraphpoint(0, 0, 110, 0);//فعال کردن دکمه جرا
        }
               
        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }
              
        private void btnselimage_Click(object sender, EventArgs e)//تست باز بودن پورت
        {
            try
            {
                try { thread_grf.Abort(); } catch { }
                tsbrun.Enabled = true;
                tsbReceive.Enabled = true;
                tsbshowdata.Enabled = false;
                label2.Text = "";
                label4.Text = "";
                tChart1.Series[0].Points.Clear();

                sp1.Dispose();

                byte[] buf = new byte[10];
                if (cmbPrtName.Text != "")
                {
                    sp1.PortName = cmbPrtName.Text.Trim();
                    sp1.Open();

                    if (sp1.IsOpen)
                    {
                        MessageBox.Show("port settings were done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sp1.Close();
                    }
                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex, "", 1); }
        }

        private void tsbRun_Click(object sender, EventArgs e)
        {
            try
            {
                buf[0] = 33;

                sp1.Dispose();
                sp1.PortName = cmbPrtName.Text.Trim();
                if (!sp1.IsOpen)
                    sp1.Open();
                
                label2.Text = "=> Run technique  (waiting...)" + Environment.NewLine;
                label4.Text = "";
                sp1.Write(buf, 0, 1);
                tsbrun.Enabled = false;
                tsbshowdata.Enabled = false;
                tChart1.Series[0].Points.Clear();
            }
            catch (Exception ex)
            {
                errors_cls err = new errors_cls(ex, 1, 1);
            }
        }

        private void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                switch (sh_state)
                {
                    case 1:
                        sp1.Read(buf, 0, 1);
                        x1_r = buf[0];
                        sh_state++;
                        break;
                    case 2:
                        sp1.Read(buf, 0, 1);
                        if (buf[0] != ENQ0)
                        {
                            for (int mm = 0; mm < 4; mm++)
                            {
                                if (mm != 0)
                                    sp1.Read(buf, 0, 1);
                                x_rec[shomar_array++] = buf[0];
                            }
                        }
                        sh_state++;
                        break;
                    case 3:
                        int n1 = x_rec[2] * 256 + x_rec[1];
                        for (int mm = 0; mm < n1 + 2; mm++)
                        {
                            sp1.Read(buf, 0, 1);
                            if (shomar_data >= data.Length)
                            {
                                Array.Resize(ref data, data.Length + 10);
                            }
                            data[shomar_data++] = buf[0];
                        }
                        sh_state++;
                        break;

                    case 4:
                        break;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        delegate void SetgraphpointCallback(double h1,double h2, int code, int state);
        private void Setgraphpoint(double h1, double h2, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SetgraphpointCallback d = new SetgraphpointCallback(Setgraphpoint);
                        this.Invoke(d, new object[] { h1,h2, code, 2 });
                    }
                    else
                    {
                        tChart1.Series[0].Points.AddXY(h1, h2);
                    }
                    break;

                case 110:
                    if (this.InvokeRequired)//فعال کردن دکمه اجرا و دریافت
                    {
                        SetgraphpointCallback d = new SetgraphpointCallback(Setgraphpoint);
                        this.Invoke(d, new object[] { h1, h2, code, 0 });
                    }
                    else
                    {
                        tsbrun.Enabled = true;
                        tsbReceive.Enabled = true;

                        try { sp1.Dispose(); } catch { }
                    }
                    break;
            }
        }

        delegate void SettxtCallback(string txt, int code, int state);
        private void Settxt(string txt, int code, int state)
        {
            switch (code)
            {
                case 1:

                    if (state == 1)
                    {
                        SettxtCallback d = new SettxtCallback(Settxt);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        label4.Text= txt;
                    }
                    break;
                    case 2:

                    if (state == 1)
                    {
                        SettxtCallback d = new SettxtCallback(Settxt);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        label2.Text += txt;
                    }
                    break;

                    case 3:

                    if (state == 1)
                    {
                        SettxtCallback d = new SettxtCallback(Settxt);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        tChart1.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle[int.Parse(txt)].Trim();
                        tChart1.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle[int.Parse(txt)].Trim();
                    }
                    break;
                case 4:
                    if (state == 1)
                    {
                        SettxtCallback d = new SettxtCallback(Settxt);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else//فعال یا عدم فعال کردن دکمه های نمایش دیتا و ذخیره اگر دیتا دریافت کرده بشد
                    {
                        if (txt.Trim() == "1") {
                            tsbReceive.Enabled = false;
                            tsbrun.Enabled = true;
                            tsbSaveAll.Enabled = false;
                            tsbshowdata.Enabled = false;                        
                        }
                        else if (txt.Trim() == "2")
                        {
                            tsbReceive.Enabled = false;
                            tsbrun.Enabled = true;
                            tsbSaveAll.Enabled = true;
                            tsbshowdata.Enabled = true ;
                        }
                    }
                    break;
                case 5:

                    if (state == 1)
                    {
                        SettxtCallback d = new SettxtCallback(Settxt);
                        this.Invoke(d, new object[] { txt, code, 2 });
                    }
                    else
                    {
                        tChart1.Series[0].Points.Clear();
                    }
                    break;
            }
        }

        private void sp1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            label4.Text += "error";
        }

       
    }
}


