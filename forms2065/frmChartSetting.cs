using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skydat.forms2065
{
    public partial class frmChartSetting : Form
    {
        public static char ChartType = 'N';
        public static Color ChartColor = Color.DarkCyan;
        public frmChartSetting()
        {
            InitializeComponent();
            chartColor.Color = Color.DarkCyan;
        }

        public frmChartSetting(string node)
        {
            InitializeComponent();
            if (node == "CA" || node == "CHP" || node == "CD")
            {
                gbAdvance.Enabled = true;
            }
            else
                gbAdvance.Enabled = false;

        }

        private void txtSampleDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a control key (e.g., backspace), digit, or decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSampleDelay_Validated(object sender, EventArgs e)
        {
            int sample;
            if (int.TryParse(txtSampleDelay.Text, out sample))
                if (sample >= 0)
                {
                    frmshowGraph2065.SampleRateMultiplier = sample;
                }
        }

        private void txtChartScale_Validated(object sender, EventArgs e)
        {
            int Scale;
            if (int.TryParse(txtChartScale.Text, out Scale))
                if (Scale >= 1)
                {
                    frmshowGraph2065.ChartScaleMultiplier = Scale;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            chartColor.ShowDialog();
            btnColor.BackColor = chartColor.Color;
            ChartColor = chartColor.Color;
        }

        private void ChartSetting_Load(object sender, EventArgs e)
        {
            chartColor.Color = ChartColor;
            btnColor.BackColor = chartColor.Color;
            txtSampleDelay.Text = frmshowGraph2065.SampleRateMultiplier.ToString();
            txtChartScale.Text = frmshowGraph2065.ChartScaleMultiplier.ToString();
            switch (ChartType)
            {
                case 'L':
                    rbLine.Checked = true;
                    break;
                case 'P':
                    rbPoint.Checked = true;
                    break;
                case 'C':
                    rbCurve.Checked = true;
                    break;
                default:
                    rbLine.Checked = true;
                    break;
            }
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e)
        {
            var radiobtn = sender as RadioButton;
            switch (radiobtn.Name)
            {
                case "rbLine":
                    ChartType = 'L';
                        break;
                case "rbCurve":
                    ChartType = 'C';
                    break;
                case "rbPoint":
                    ChartType = 'P';
                    break;
                default:
                    break;
            }
        }
    }
}
