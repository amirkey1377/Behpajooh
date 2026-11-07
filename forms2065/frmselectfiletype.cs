using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skydat.forms2065
{
    
    public partial class frmselectfiletype : Form
    {
        //public clasMain mc = new clasMain();
        //public frmMain1 mf;
        public int filetype_select = 0;

        public frmselectfiletype()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rdbtext.Checked)
                filetype_select = 1;
            else if (rdbxls.Checked)
                filetype_select = 2;
            else
                filetype_select = 3;
            this.DialogResult=DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rdbxls_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
