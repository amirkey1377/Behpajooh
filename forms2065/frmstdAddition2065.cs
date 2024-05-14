using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Skydat.forms2065
{
    public partial class frmstdAddition2065 : Form
    {
        public frmstdAddition2065()
        {
            InitializeComponent();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "stn";
            sfd.Filter = "STN Files(*.stn)|*.stn";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                sw.WriteLine("Header:" + txtHeader.Text);
                sw.WriteLine("Note:" + txtNote.Text);
                sw.WriteLine("X Axis Title:" + txtXTitle.Text);
                sw.WriteLine("Y Axis Title:" + txtYTitle.Text);
                sw.WriteLine("X Axis Unit:" + txtXUnit.Text);
                sw.WriteLine("Y Axis Unit:" + txtYUnit.Text);
                sw.WriteLine("Slop:" + txtSlop.Text);
                sw.WriteLine("Correlation:" + txtCore.Text);
                sw.WriteLine("Add Concent0:" + txtCon0.Text);
                sw.WriteLine("Add PHeight0:" + txtPH0.Text);
                sw.WriteLine("Add Concent1:" + txtCon1.Text);
                sw.WriteLine("Add PHeight1:" + txtPH1.Text);
                sw.WriteLine("Add Concent2:" + txtCon2.Text);
                sw.WriteLine("Add PHeight2:" + txtPH2.Text);
                sw.WriteLine("Add Concent3:" + txtCon3.Text);
                sw.WriteLine("Add PHeight3:" + txtPH3.Text);
                sw.WriteLine("Add Concent4:" + txtCon4.Text);
                sw.WriteLine("Add PHeight4:" + txtPH4.Text);
                sw.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "stn";
            ofd.Filter = "STN Files(*.stn)|*.stn";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                string ln = "";
                while (!sr.EndOfStream)
                {
                    ln = sr.ReadLine();
                    if (ln.IndexOf("Header") >= 0) txtHeader.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Note") >= 0) txtNote.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("X Axis Title") >= 0) txtXTitle.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Y Axis Title") >= 0) txtYTitle.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("X Axis Unit") >= 0) txtXUnit.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Y Axis Unit") >= 0) txtYUnit.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Slop") >= 0) txtSlop.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Correlation") >= 0) txtCore.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add Concent0") >= 0) txtCon0.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add PHeight0") >= 0) txtPH0.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add Concent1") >= 0) txtCon1.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add PHeight1") >= 0) txtPH1.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add Concent2") >= 0) txtCon2.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add PHeight2") >= 0) txtPH2.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add Concent3") >= 0) txtCon3.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add PHeight3") >= 0) txtPH3.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add Concent4") >= 0) txtCon4.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Add PHeight4") >= 0) txtPH4.Text = ln.Substring(ln.IndexOf(':') + 1);
                }
                sr.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int i = 0, n = 0;
            double A0 = 0, A1 = 0, B0 = 0, B1 = 0, AB = 0;

            bool erclb = false;
            double Cnc = 0;
            double[] Cn = { 0, 0, 0, 0, 0, 0, 0 };
            double[] Ip = { 0, 0, 0, 0, 0, 0, 0 };
            double b = 0, ai = 0, Cr = 0;
            //  Cn[0]:=strtofloat(Edit1.text);
            Ip[0] = Convert.ToDouble(txtPH0.Text);
            Cn[1] = Convert.ToDouble(txtCon1.Text);
            Ip[1] = Convert.ToDouble(txtPH1.Text);
            Cn[2] = Convert.ToDouble(txtCon2.Text);
            Ip[2] = Convert.ToDouble(txtPH2.Text);
            Cn[3] = Convert.ToDouble(txtCon3.Text);
            Ip[3] = Convert.ToDouble(txtPH3.Text);
            Cn[4] = Convert.ToDouble(txtCon4.Text);
            Ip[4] = Convert.ToDouble(txtPH4.Text);

            i = 1;
            while (i < 5)
            {
                if ((Cn[i] == 0) || (Ip[i] == 0)) break;
                i++;
            }
            n = i - 1;
            if ((n < 1) || (Ip[0] == 0))
            {
                erclb = true;
                if (Ip[0] == 0)
                    MessageBox.Show("PeakHeight for Uknown must be entered", "Error", MessageBoxButtons.OK);
                else
                    MessageBox.Show("At Least 2 data points must be entered", "Error", MessageBoxButtons.OK);
            }
            else
            {
                A0 = 0; A1 = 0; B0 = 0; B1 = 0; AB = 0;
                for (i = 1; i < n - 1; i++)
                {
                    A0 = A0 + Cn[i];
                    A1 = A1 + Cn[i] * Cn[i];
                    B0 = B0 + Ip[i];
                    B1 = B1 + Ip[i] * Ip[i];
                    AB = AB + Cn[i] * Ip[i];
                }
                b = (AB - Ip[0] * A0) / A1;
                ai = Ip[0];
                textBox1.Text = ai.ToString() + "  " + b.ToString();
                Cnc = ai / b;
                B0 = B0 + Ip[0];
                B1 = B1 + Ip[0] * Ip[0];
                Cr = (n * AB - A0 * B0) / Math.Sqrt((n * A1 - A0 * A0) * (n * B1 - B0 * B0));
            }
            if (!erclb)
            {
                txtSlop.Text = b.ToString();
                txtCore.Text = Cr.ToString();
                txtCon0.Text = Cnc.ToString();
            }
}

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
