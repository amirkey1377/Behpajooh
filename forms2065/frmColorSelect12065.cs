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
    public partial class frmColorSelect12065 : Form
    {
        public frmColorSelect12065()
        {
            InitializeComponent();
        }
        public frmMain2065 mf;

        private void frmColorSelect_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = mf.cpar;
        }
    }
}
