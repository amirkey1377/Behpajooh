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
    public partial class frmOption12065 : Form
    {
        public frmOption12065()
        {
            InitializeComponent();
            cmbStyleDraw.SelectedIndex = 0;
        }
        public frmMain2065 mf;
        public clasMain mc = new clasMain();
        public frmanalyseGraph2065 gf;


             

        private void BtnColorDraw_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            mf.drawColor = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // gf.do_More();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // gf.do_More();
        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbTypeSmooth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                button3_Click(button3, e);
            if (e.KeyValue == 27)
                button1_Click(button1, e);
        }

        
    }
}
