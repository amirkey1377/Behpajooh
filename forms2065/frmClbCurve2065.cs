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
    public partial class frmClbCurve2065 : Form
    {
        public frmClbCurve2065()
        {
            InitializeComponent();
        }
        public frmMain2065 mf;

        private void button5_Click(object sender, EventArgs e)
        {
            byte i = 0;
            double A0 = 0, A1 = 0, A2 = 0, B0 = 0, B1 = 0, AB = 0;
            bool erclb = false;
            int n = 0, Cnc = 0;
            double[] Cn = { 0, 0, 0, 0, 0, 0, 0 };
            double[] Ip = { 0, 0, 0, 0, 0, 0, 0 };
            double b = 0, ai = 0, Cr = 0;
            Cn[0] = Convert.ToDouble(txtCon1.Text);
            Ip[0] = Convert.ToDouble(txtPH1.Text);
            Cn[1] = Convert.ToDouble(txtCon2.Text);
            Ip[1] = Convert.ToDouble(txtPH2.Text);
            Cn[2] = Convert.ToDouble(txtCon3.Text);
            Ip[2] = Convert.ToDouble(txtPH3.Text);
            Cn[3] = Convert.ToDouble(txtCon4.Text);
            Ip[3] = Convert.ToDouble(txtPH4.Text);
            Cn[4] = Convert.ToDouble(txtCon5.Text);
            Ip[4] = Convert.ToDouble(txtPH5.Text);
            /*  n:=7;*/

            double Ipx = Convert.ToDouble("0" + txtPH6.Text);
            i = 0;
            while (i < 5)
            {
                if (Cn[i] == 0 && Ip[i] == 0) break;
                i++;
            }
            n = i - 1;
            if (i < 3)
            {
                erclb = true;
                MessageBox.Show("At Least 3 data points must be entered", "Error", MessageBoxButtons.OK);
            }
            else
            {
                A0 = 0; A1 = 0; B0 = 0; B1 = 0; AB = 0;
                for (i = 0; i <= n-1; i++)
                {
                    A0 = A0 + Cn[i];
                    A1 = A1 + Cn[i] * Cn[i];
                    B0 = B0 + Ip[i];
                    B1 = B1 + Ip[i] * Ip[i];
                    AB = AB + Cn[i] * Ip[i];
                }
                AB = n * AB - A0 * B0; // AB:=AB-(A0*B0)/n;
                A2 = n * A1 - A0 * A0; // A2:=A1-(A0*A0)/n;
                b = AB / A2;
                ai = (B0 - b * A0) / n; // a:=(B0/n-b*(A0/n));
                Cr = AB / Math.Sqrt(A2 * (n * B1 - B0 * B0));//Cr:=n*AB/sqrt(n*A2*(n*B1-B0*B0));
                if (Ipx != 0) Cnc = (int)((Ipx - ai) / b);
            }
            if (!erclb)
            {
                txtSlop.Text = b.ToString();
                txtIntercept.Text = ai.ToString();
                txtCorre.Text = Cr.ToString();
                txtCon6.Text = Cnc.ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "clb";
            sfd.Filter = "CLB Files(*.clb)|*.clb";
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
                sw.WriteLine("Correlation:" + txtCorre.Text);
                sw.WriteLine("Intercept:" + txtIntercept.Text);
                sw.WriteLine("Std Concent1:" + txtCon1.Text);
                sw.WriteLine("Std PHeight1:" + txtPH1.Text);
                sw.WriteLine("Std Concent2:" + txtCon2.Text);
                sw.WriteLine("Std PHeight2:" + txtPH2.Text);
                sw.WriteLine("Std Concent3:" + txtCon3.Text);
                sw.WriteLine("Std PHeight3:" + txtPH3.Text);
                sw.WriteLine("Std Concent4:" + txtCon4.Text);
                sw.WriteLine("Std PHeight4:" + txtPH4.Text);
                sw.WriteLine("Std Concent5:" + txtCon5.Text);
                sw.WriteLine("Std PHeight5:" + txtPH5.Text);
                sw.WriteLine("Std Concent6:" + txtCon6.Text);
                sw.WriteLine("Std PHeight6:" + txtPH6.Text);
                sw.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "clb";
            ofd.Filter = "CLB Files(*.clb)|*.clb";
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
                    if (ln.IndexOf("Correlation") >= 0) txtCorre.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Intercept") >= 0) txtIntercept.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent1") >= 0) txtCon1.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight1") >= 0) txtPH1.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent2") >= 0) txtCon2.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight2") >= 0) txtPH2.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent3") >= 0) txtCon3.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight3") >= 0) txtPH3.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent4") >= 0) txtCon4.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight4") >= 0) txtPH4.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent5") >= 0) txtCon5.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight5") >= 0) txtPH5.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std Concent6") >= 0) txtCon6.Text = ln.Substring(ln.IndexOf(':') + 1);
                    if (ln.IndexOf("Std PHeight6") >= 0) txtPH6.Text = ln.Substring(ln.IndexOf(':') + 1);
                }
                sr.Close();
            }
        }
    }
}
