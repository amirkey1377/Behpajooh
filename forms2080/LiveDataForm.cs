
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

public class LiveDataForm : Form
{
    private readonly Chart _chart;
    private readonly Series _liveSeries;

    private readonly DataGridView dgvLiveData;
    private readonly Button btnDelete;

    public LiveDataForm(Chart chart)
    {
        _chart = chart;

        _liveSeries = _chart.Series.FindByName("live");

        if (_liveSeries == null)
            throw new ArgumentException(
                "سری Live در نمودار پیدا نشد.");

        Text = "Live Data";
        StartPosition = FormStartPosition.CenterScreen;
        //Size = new Size(700, 500);
        //MinimumSize = new Size(200, 200);

        dgvLiveData = new DataGridView
        {
            Dock = DockStyle.Fill,

            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false,

            ReadOnly = false,
            MultiSelect = false,

            SelectionMode =
                DataGridViewSelectionMode.FullRowSelect,

            AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill,

            RowHeadersVisible = false,
            ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing,

            RowTemplate = { Height = 30 },

            ScrollBars = ScrollBars.Vertical,

            EditMode =
                DataGridViewEditMode.EditProgrammatically
        };

        dgvLiveData.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Time",
            HeaderText = "Time",
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.NotSortable
        });

        dgvLiveData.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Voltage",
            HeaderText = "Voltage",
            ReadOnly = false,
            SortMode = DataGridViewColumnSortMode.NotSortable
        });

        btnDelete = new Button
        {
            Text = "Delete",
            Dock = DockStyle.Right,
            Width = 110
        };

        Panel bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 45
        };

        bottomPanel.Controls.Add(btnDelete);

        Controls.Add(dgvLiveData);
        Controls.Add(bottomPanel);

        // ارتفاع ثابت هر سطر
        dgvLiveData.RowTemplate.Height = 30;

        // ارتفاع عنوان ستون‌ها
        dgvLiveData.ColumnHeadersHeight = 35;

        // ارتفاع جدول برای نمایش 10 سطر
        int rowsHeight = 10 * 30;

        // ارتفاع کل فرم
        int formHeight =
            rowsHeight +
            dgvLiveData.ColumnHeadersHeight +
            45 +                 // پنل دکمه Delete
            40;                  // حاشیه و نوار عنوان فرم

        ClientSize = new Size(400, formHeight);

        LoadLiveData();

        // ویرایش فقط با دابل‌کلیک روی ستون Voltage
        dgvLiveData.CellDoubleClick +=
            DgvLiveData_CellDoubleClick;

        // ثبت مقدار جدید Voltage در نمودار
        dgvLiveData.CellEndEdit +=
            DgvLiveData_CellEndEdit;

        dgvLiveData.DataError += (s, e) =>
        {
            MessageBox.Show(
                "مقدار واردشده معتبر نیست.",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            e.Cancel = true;
        };

        // حذف سطر انتخاب‌شده
        btnDelete.Click += BtnDelete_Click;

        // انتخاب سطر با راست‌کلیک روی نقطه نمودار
        _chart.MouseDown += Chart_MouseDown;
    }

    private void LoadLiveData()
    {
        dgvLiveData.Rows.Clear();

        foreach (DataPoint point in _liveSeries.Points)
        {
            string timeText;

            if (_liveSeries.XValueType ==
                ChartValueType.DateTime)
            {
                timeText = DateTime
                    .FromOADate(point.XValue)
                    .ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
            {
                timeText = point.XValue.ToString(
                    "G", CultureInfo.CurrentCulture);
            }

            int rowIndex = dgvLiveData.Rows.Add(
                timeText,
                point.YValues[0].ToString(
                    "G", CultureInfo.CurrentCulture));

            // اتصال سطر جدول به نقطه اصلی نمودار
            dgvLiveData.Rows[rowIndex].Tag = point;
        }
    }

    private void DgvLiveData_CellDoubleClick(
        object sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != 1)
            return;

        dgvLiveData.CurrentCell =
            dgvLiveData.Rows[e.RowIndex].Cells[1];

        dgvLiveData.BeginEdit(true);
    }

    private void DgvLiveData_CellEndEdit(
        object sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != 1)
            return;

        DataGridViewRow row =
            dgvLiveData.Rows[e.RowIndex];

        DataPoint point = row.Tag as DataPoint;

        if (point == null)
            return;

        string text = Convert.ToString(
            row.Cells["Voltage"].Value);

        // ابتدا با تنظیمات فعلی ویندوز
        // و سپس با فرمت اعشاری نقطه‌ای بررسی می‌کنیم.
        double voltage;

        bool valid = double.TryParse(
            text,
            NumberStyles.Float,
            CultureInfo.CurrentCulture,
            out voltage);

        if (!valid)
        {
            valid = double.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out voltage);
        }

        if (!valid ||
            double.IsNaN(voltage) ||
            double.IsInfinity(voltage))
        {
            MessageBox.Show(
                "لطفاً یک عدد معتبر برای Voltage وارد کن.");

            row.Cells["Voltage"].Value =
                point.YValues[0].ToString(
                    "G", CultureInfo.CurrentCulture);

            return;
        }

        // فقط مقدار Y تغییر می‌کند؛ X (زمان) ثابت می‌ماند.
        point.YValues[0] = voltage;

        _chart.Invalidate();
    }

    private void Chart_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right)
            return;

        HitTestResult result = _chart.HitTest(
            e.X,
            e.Y,
            ChartElementType.DataPoint);

        if (result.Series == null ||
            result.Series.Name != "Live" ||
            result.PointIndex < 0)
        {
            return;
        }

        if (result.PointIndex >= dgvLiveData.Rows.Count)
            return;

        dgvLiveData.ClearSelection();

        DataGridViewRow row =
            dgvLiveData.Rows[result.PointIndex];

        row.Selected = true;

        dgvLiveData.CurrentCell = row.Cells["Time"];
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (dgvLiveData.SelectedRows.Count == 0)
        {
            MessageBox.Show("ابتدا یک سطر را انتخاب کن.");
            return;
        }

        DataGridViewRow row =
            dgvLiveData.SelectedRows[0];

        DataPoint point = row.Tag as DataPoint;

        if (point == null)
            return;

        // حذف نقطه متناظر از سری نمودار
        _liveSeries.Points.Remove(point);

        // حذف سطر از جدول
        dgvLiveData.Rows.Remove(row);

        _chart.Invalidate();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _chart.MouseDown -= Chart_MouseDown;

        base.OnFormClosed(e);
    }
}
