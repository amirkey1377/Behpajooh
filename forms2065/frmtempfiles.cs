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
using System.Linq;
using System.Threading;

namespace Skydat.forms2065
{
    public partial class frmtempfiles : Form
    {
       public frmtempfiles()
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
        bool smooth_state = false;
        double[] othertech_val0 = new double[200];
        double[] othertech_val1 = new double[200];       
        private int shomar_val = 0;
         KeyEventArgs ee = new KeyEventArgs(Keys.Alt);
        private void GraphForm_Load(object sender, EventArgs e)
        {                     
            try
            {
                string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                string s = color[2].ToString().Substring(color[2].ToString().IndexOf(":") + 1);
                Color cl = Color.FromArgb(Convert.ToInt32(s));
                tChart1.BackColor = cl;
                tChart1.BorderlineColor = cl;
                tChart1.ChartAreas[0].BackColor = cl;
                panel2.BackColor = cl;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

            filltreeview3(4);
        }

        private void filltreeview3(int typecall)
        {
            treeView3.Nodes[0].Nodes.Clear();

            DirectoryInfo info = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile");
            FileInfo[] files=null;
            switch(typecall){
                case 1:
                    files = info.GetFiles().OrderBy(p => p.Name).ToArray();
                    break;
                case 2:
                    files = info.GetFiles().OrderByDescending(p => p.Name).ToArray();
                    break;
                case 3:
                    files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                    break;
                case 4:
                    files = info.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                    break;
        }
            foreach (FileInfo file in files)
            {

                treeView3.Nodes[0].Nodes.Add(file.Name);
            }
        

            //  treeView3.Nodes[0].Nodes.Add(filenames[i].Trim().Substring(len1, filenames[i].Trim().Length - len1));

            treeView3.ExpandAll();
            tChart1.Series[0].Points.Clear();
            label4.Text = "";
            label41.Text = "";
        }


        string tech = "";
        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level > 0)
            {
                if (e.Node.IsSelected)
                {
                    restore_Data(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\"+e.Node.Text.Trim());
                    if (smooth_state)
                    smootht4();
                    else
                    {
                        if (tech == "CHP" || tech == "CHA" || tech == "CHC" || tech == "CD")
                            tChart1.Series[0].Points.AddXY(0, othertech_val1[0]);//رسم در نقطه صفر زمان // technics chp cha chc

                        tech = "";

                        for (int i = 0; i < shomar_val; i++)
                        {
                            tChart1.Series[0].Points.AddXY(othertech_val0[i], othertech_val1[i]);
                           
                        }
                    }

                }

            }

        }
        private void restore_Data(string fileName)
        {

            string s = "", fs = "";
            bool isData = false;
            if (fileName != "")
                fs = fileName;
            StreamReader Fl;

            EventArgs e = new EventArgs();
            if (fs != "")
            {
                Fl = new StreamReader(fs, Encoding.ASCII);

                try
                {
                    int cycle = 0;

                    while (!Fl.EndOfStream)
                    {
                        s = Fl.ReadLine();
                        tChart1.Series[0].Points.Clear();
                       
                        if (s.Trim().Length == 0)//خط خالی
                            s = Fl.ReadLine();
                       
                        if (s.LastIndexOf("=====") > 0)
                        {
                            cycle++;

                            if (cycle > 1)//اگر دارای سیکل بود فقط یک سیکل را نمایش دهد
                                break;
                        }

                        if (!isData)
                        {
                            if (s.IndexOf("Tech") >= 0)
                            {
                                if (s.IndexOf(':') >= 0)
                                {
                                    tech = s.Substring(s.IndexOf(':') + 1).Trim();

                                    for (int i = 0; i < clasglobal.TechName.Length; i++)
                                        if (clasglobal.TechName[i] == s.Substring(s.IndexOf(':') + 1).Trim())
                                        {
                                            tChart1.ChartAreas[0].AxisY.Title = clasglobal.VAxisTitle[i];
                                            tChart1.ChartAreas[0].AxisX.Title = clasglobal.HAxisTitle[i];
                                            label41.Text = "Technique name :" + clasglobal.TechName[i];
                                            label4.Text = "";
                                            if (clasglobal.TechName[i].ToLower() == "cha" || clasglobal.TechName[i].ToLower() == "chp" || clasglobal.TechName[i].ToLower() == "chc" || clasglobal.TechName[i].ToLower() == "cd")
                                                smooth_state = false;
                                            else
                                                smooth_state = true;
                                            break;
                                        }
                                }
                            }
                            else if (s.IndexOf("Current Range") >= 0 || s.IndexOf("E1") >= 0 || s.IndexOf("E2") >= 0 || s.IndexOf("E3") >= 0 || s.IndexOf("HStep") >= 0 || s.IndexOf("Equilibrium Time") >= 0 || s.IndexOf("Hold Time") >= 0 || s.IndexOf("Scan Rate") >= 0 || s.IndexOf("ScanRate_R") >= 0
                                || s.IndexOf("TStep") >= 0 || s.IndexOf("Pulse Width") >= 0 || s.IndexOf("pulse ") >= 0 || s.IndexOf("Frequency") >= 0 || s.IndexOf("Cycle") >= 0 || s.IndexOf("T1") >= 0 || s.IndexOf("T2") >= 0 || s.IndexOf("I1") >= 0 || s.IndexOf("I2") >= 0)
                                label4.Text = label4.Text.Trim() + Environment.NewLine + s.Trim();

                            if (s.LastIndexOf("Potential") >= 0 || s.LastIndexOf("========") >= 0)
                            {
                                isData = true;
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

                                    try { if (num == 0) x = Convert.ToDouble(sss); } catch { x = 0; }//this.grf.dataE[num_data + num_ser, row] //890220
                                    try { if (num == 1) y = Convert.ToDouble(sss); } catch { y = 0; } //this.grf.dataI1[num_data + num_ser, row] = Convert.ToDouble(sss);
                                    try { if (num == 2) z = Convert.ToDouble(sss); } catch { z = 0; } //this.grf.dataI2[num_data + num_ser, row] = Convert.ToDouble(sss);
                                    try { if (num == 3) w = Convert.ToDouble(sss); } catch { w = 0; } //this.grf.dataI2[num_data + num_ser, row] = Convert.ToDouble(sss);

                                    sss = "";
                                    num++;
                                }
                                l++;
                            }

                            try { if (num == 0) x = Convert.ToDouble(sss); } catch { x = 0; }
                            try { if (num == 1) y = Convert.ToDouble(sss); } catch { y = 0; }
                            try { if (num == 2) z = Convert.ToDouble(sss); } catch { z = 0; }
                            try { if (num == 3) w = Convert.ToDouble(sss); } catch { w = 0; }

                            if (shomar_val >= othertech_val0.Length)
                            {
                                Array.Resize(ref othertech_val0, othertech_val0.Length + 10);
                                Array.Resize(ref othertech_val1, othertech_val1.Length + 10);
                            }
                            othertech_val0[shomar_val] = x;
                            othertech_val1[shomar_val] = y;
                            shomar_val++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "selected File is not in valid Format...", "Warning");
                }
                Fl.Close();
            }

        }
        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView3.SelectedNode != null && treeView3.SelectedNode.Level > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.AddExtension = true;
                    sfd.DefaultExt = "dat";
                    sfd.Filter = "Data Files(*.dat)|*.dat";
                    int nn = this.treeView3.Nodes[0].Nodes.Count;

                    sfd.FileName = treeView3.SelectedNode.Text;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (sfd.FileName != "")
                        {
                            File.Copy(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + treeView3.SelectedNode.Text.Trim(), sfd.FileName, true);
                            if (MessageBox.Show("done successfully." + Environment.NewLine + "do you want to delete tempfile? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                tsbDelete_Click(tsbDelete, e);

                            }

                        }
                    }
                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex,1,1); }
        }      
     

        public void smootht4()
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

            tChart1.Series[0].Points.Clear();
            for (int i = 0; i < vn; i++)
            {
                tChart1.Series[0].Points.AddXY(othertech_val0[i], othertech_val1[i]);
            }

        }

         
      

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            filltreeview3(4);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView3.SelectedNode != null && treeView3.SelectedNode.Level>0)
                {
                    if (MessageBox.Show("are you sure?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        File.Delete(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + treeView3.SelectedNode.Text);
                        MessageBox.Show("done successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tsbRefresh_Click(tsbRefresh, e);
                    }
                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex,1,1); }
        }

        private void tsbshowdata_Click(object sender, EventArgs e)
        {
            if (treeView3.SelectedNode!=null && treeView3.SelectedNode.Level == 1)
            {
                frmshowdata frm = new frmshowdata();
                frm.listView1.Items.Clear();
                for (int i = 0; i < tChart1.Series[0].Points.Count; i++)
                {
                    frm.listView1.Items.Add(tChart1.Series[0].Points[i].XValue + "          " + tChart1.Series[0].Points[i].YValues[0]);
                }
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
          
                treeView3.CheckBoxes = !treeView3.CheckBoxes;
            
                treeView3.ExpandAll();
            if(treeView3.CheckBoxes)
                toolStripMenuItem1.Text="deactive multi-selection";
            else
                toolStripMenuItem1.Text = "active multi-selection";

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                int shomarcheckednode = 0;
                if (MessageBox.Show("are you sure?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (treeView3.CheckBoxes)
                    {
                        for (int i = 0; i < treeView3.Nodes[0].Nodes.Count; i++)
                        {
                            if (treeView3.Nodes[0].Nodes[i].Checked)
                            {
                                File.Delete(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\tempfile\\" + treeView3.Nodes[0].Nodes[i].Text);
                                shomarcheckednode++;
                            }
                        }
                        if (shomarcheckednode > 0)
                        {
                            MessageBox.Show("done successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tsbRefresh_Click(tsbRefresh, e);
                        }
                    }
                    else
                    {
                        MessageBox.Show("please active multi-selection!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
            catch (Exception ex) { errors_cls err = new errors_cls(ex,1,1); }
        }

        private void treeView3_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                if (treeView3.Nodes[0].Checked)
                {
                    for (int i = 0; i < treeView3.Nodes[0].Nodes.Count; i++)
                        treeView3.Nodes[0].Nodes[i].Checked = true;

                }
                else {
                    for (int i = 0; i < treeView3.Nodes[0].Nodes.Count; i++)
                        treeView3.Nodes[0].Nodes[i].Checked = false;
                }

            }
        }

        private void nameascToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filltreeview3(1);
        }

        private void namedescToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filltreeview3(2);
        }

        private void dateascToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filltreeview3(3);
        }

        private void datedescToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filltreeview3(4);
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
                    panel2.BackColor = cl.Color;

                    string[] color = File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat");
                    color[2] = "2065 temp:" + cl.Color.ToArgb().ToString();
                    File.WriteAllLines(System.Windows.Forms.Application.StartupPath + "\\Color.dat", color);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void toolstripChart_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tChart1_Click(object sender, EventArgs e)
        {

        }
    }
}