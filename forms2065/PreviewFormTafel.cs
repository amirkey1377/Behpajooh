using Skydat.classes2065;
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
    public partial class PreviewFormTafel : Form
    {
        // نگهداری داده‌ها (ویرایش‌ شده)
        public List<ChartData> EditedData { get; private set; }

        public PreviewFormTafel(List<ChartData> data)
        {
            InitializeComponent();
            dataGridView1.DataSource = data;

            // داده‌ها را در متغیر محلی نگه داریم تا بعداً برگردانده شوند
            EditedData = data;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // تغییرات کاربر را از DataGridView به لیست منتقل کن
            dataGridView1.EndEdit(); // اطمینان از ذخیره تغییرات در بایندینگ
            EditedData = new List<ChartData>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;


                // سعی در خواندن و تبدیل مقادیر
                double.TryParse(row.Cells["Time"].Value?.ToString(), out double time);
                double.TryParse(row.Cells["Potential"].Value?.ToString(), out double potential);

                EditedData.Add(new ChartData { Time = time, Potential = potential });
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
