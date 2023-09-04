using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skydat.classes2065;

namespace Skydat.forms2065
{
    public partial class frmShowPeaks12065 : Form
    {
        public frmShowPeaks12065()
        {
            InitializeComponent();
        }
        public frmOption12065 Foption;
        public frmMain2065 fm;
        clasPeak cPeak = new clasPeak();
        clasMain mc = new clasMain();

        private void frmShowPeaks_FormClosing(object sender, FormClosingEventArgs e)
        {
            fm.Detect_Peak = false;
            Foption.cbDrawLine.Checked = false;
            fm.grf.drawline1.EnableDraw = Foption.cbDrawLine.Checked;
            fm.grf.drawline1.Active = false;
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            int Selected_Item = 0;
            //richTextBox1.Clear();
            cPeak.SR_Peak = fm.ScanRate;
            if (fm.Detect_Peak && rbAutoPeak.Checked)
            {
                if (fm.grf.treeView3.Nodes[0].Nodes.Count > 1)
                {
                    MessageBox.Show("Select one graph for peak search");
                    return;
                }
                double[] BBR = mc.BR;
                for (int i = 0; i < fm.grf.treeView3.Nodes[0].Nodes.Count; i++)
                    if (fm.grf.treeView3.Nodes[0].Nodes[i].Checked)
                    {
                        Selected_Item = i;
                        break;
                    }
                double[] xvale=new double[fm.grf.tChart1.Series[Selected_Item].Points.Count];
                double[] yvale=new double[fm.grf.tChart1.Series[Selected_Item].Points.Count];
                for(int c=0;c<fm.grf.tChart1.Series[Selected_Item].Points.Count;c++)
                {
                    xvale[c]=fm.grf.tChart1.Series[Selected_Item].Points[c].XValue;
                    yvale[c]=fm.grf.tChart1.Series[Selected_Item].Points[c].YValues[0];

                }
                mc.smoothAverage(2, xvale, yvale, false, 0);
                richTextBox1.Text += cPeak.findp1(xvale, yvale, fm.grf.tChart1.Series[Selected_Item].Points.Count);
                if (richTextBox1.Text.Length < 5)
                    return;
                for (int j = 0; j < 20; j++)
                    if (cPeak.PeakX11_n[j] != 0 && cPeak.PeakY11_n[j] != 0)
                    {
                        
                        try
                        {
                             }
                        catch { }
                    }
                return;
              
            }
            if (fm.Detect_Peak && rbManualPeak.Checked)
            {
                fm.grf.isSelectedLine = true;
                if (fm.grf.drawline1.Lines.Count() > 0)
                    fm.grf.DrawLineSelectLine(fm.grf.drawline1);
                fm.grf.isSelectedLine = false;
            }
            if (!fm.Detect_Peak)
            {
                Foption.cbDrawLine.Checked = false;
               fm.grf.drawline1.Active = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Foption.cbDrawLine.Checked = false;
            fm.grf.drawline1.Active = false;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void rbAutoPeak_CheckedChanged(object sender, EventArgs e)
        {
            if (fm.Detect_Peak)
            {
                if (rbManualPeak.Checked)
                {
                    if (!Foption.cbDrawLine.Checked)
                        MessageBox.Show("Please Draw Line(s) with Mouse and select them to see your PEAK!!!");
                    Foption.cbDrawLine.Checked = true;
                    fm.grf.drawline1.EnableDraw = Foption.cbDrawLine.Checked;
                    fm.grf.drawline1.Active = true;
                    fm.grf.Set_line_Setting();
                }
                else
                {
                    Foption.cbDrawLine.Checked = false;
                    fm.grf.drawline1.EnableDraw = Foption.cbDrawLine.Checked;
                    fm.grf.drawline1.Active = false;
                }
            }
            else
                try
                {
                    for (int i = 0; i < fm.grf.drawline1.Lines.Count(); i++)
                        fm.grf.drawline1.Lines.RemoveAt(i);
                }
                catch { }
        }

        private void frmShowPeaks_Load(object sender, EventArgs e)
        {
            fm.grf.fPeak = this;
            Width = 260;
            Height = 580;
            Top = 0;
            Left = 0;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (richTextBox1.Text != "");
        }
    }
}
