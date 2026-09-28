using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat.forms2080
{
    public partial class frmChartSetting2190 : Form
    {
        public static SeriesChartType ChartType = SeriesChartType.FastLine;
        public static Color ChartColor = Color.DarkCyan;


        public ITechniques UpdateTech = null;
        public frmChartSetting2190(ITechniques Tech)
        {
            InitializeComponent();
            this.Name = Tech.Name;
            UpdateTech = Tech;

            btnColor.BackColor = Color.FromArgb(Tech.ColorCode);

            switch (Tech.SeriesType)
            {
                case SeriesChartType.FastPoint:
                    rbPoint.Checked = true;
                    break;
                case SeriesChartType.Point:
                    rbPoint.Checked = true;
                    break;
                case SeriesChartType.Spline:
                    rbCurve.Checked = true;
                    break;
                case SeriesChartType.FastLine:
                    rbLine.Checked = true;
                    break;
            }

            if (Tech.Name == "CA")
            {
                gbAdvance.Enabled = true;
            }

            this.rbPoint.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            this.rbCurve.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            this.rbLine.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);    
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
            
        }

        private void txtChartScale_Validated(object sender, EventArgs e)
        {
            
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

            UpdateTech.ColorCode = ChartColor.ToArgb();
        }

        private void ChartSetting_Load(object sender, EventArgs e)
        {
            
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e)
        {
            var radiobtn = sender as RadioButton;
            switch (radiobtn.Name)
            {
                case "rbLine":
                    ChartType = SeriesChartType.FastLine;
                        break;
                case "rbCurve":
                    ChartType = SeriesChartType.Spline;
                    break;
                case "rbPoint":
                    ChartType = SeriesChartType.FastPoint;
                    break;
                default:
                    break;
            }

            UpdateTech.SeriesType = ChartType;
        }
    }
}
