using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat.formEIS
{
    public partial class LandingEIS : Form
    {
        //Global Variable
        public string Input;
        string[] StringData;
        float[] numb = new float[3];

        public LandingEIS()
        {
            InitializeComponent();
            chart1.MouseWheel += chart_MouseWheel;
        }

        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XFinish { get; set; }
            public double YStart { get; set; }
            public double YFinish { get; set; }
        }

        private readonly Stack<ZoomFrame> _zoomFrames = new Stack<ZoomFrame>();
        private void chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0)
                {
                    if (0 < _zoomFrames.Count)
                    {
                        var frame = _zoomFrames.Pop();
                        if (_zoomFrames.Count == 0)
                        {
                            xAxis.ScaleView.ZoomReset();
                            //yAxis.ScaleView.ZoomReset();
                        }
                        else
                        {
                            xAxis.ScaleView.Zoom(frame.XStart, frame.XFinish);
                            //yAxis.ScaleView.Zoom(frame.YStart, frame.YFinish);
                        }
                    }
                }
                else if (e.Delta > 0)
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    //var yMin = yAxis.ScaleView.ViewMinimum;
                    //var yMax = yAxis.ScaleView.ViewMaximum;

                    _zoomFrames.Push(new ZoomFrame { XStart = xMin, XFinish = xMax });

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    //var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    //var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    //yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch { }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void LandingEIS_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.#e-0";
            //chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#0.##";
        }

        private void SerialPort_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            Input = SerialPort.ReadLine();
            StringData = Input.Split('@');
            //MessageBox.Show(StringData[0] + ' ' + StringData[1] + ' ' + StringData[2]);
            try
            {
                numb[0] = float.Parse(StringData[0]);
                numb[1] = (float.Parse(StringData[1]));
                numb[2] = (float.Parse(StringData[2]) / 10);
            }

            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                chart1.Series["Series1"].Points.AddXY(numb[1], numb[2]);
            });

            return;
        }

        private void Open_port_Click(object sender, EventArgs e)
        {
            if (SerialPort.IsOpen == false)
            {
                SerialPort.PortName = comboBox1.Text;
                SerialPort.Open();
                Open_port.Text = "close";
            }
            else
            {
                SerialPort.Close();
                Open_port.Text = "open";
            }
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            try
            {
                string send = null;

                send = '@' + Convert.ToString(10) + '@';
                send = send + Vmin.Text + '@';
                send = send + Vmax.Text + '@';
                send = send + Hs.Text + '@';
                send = send + Ts.Text + '@';
                send = send + '@';

                SerialPort.Write(send);
                MessageBox.Show(send);
            }

            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
        }

        private void CLR_Click_1(object sender, EventArgs e)
        {
            //chart1.Series["Series1"].Points.Clear();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int value = random.Next(1, 20);
            Input = Convert.ToString(value);
            Input = Input + '@';

            value = random.Next(1, 20);
            Input = Input + Convert.ToString(value);
            Input = Input + '@';

            value = random.Next(1, 20);
            Input = Input + Convert.ToString(value);

            StringData = Input.Split('@');
            MessageBox.Show(StringData[0] + ' ' + StringData[1] + ' ' + StringData[2]);

            try
            {
                numb[0] = int.Parse(StringData[0]);
                numb[1] = int.Parse(StringData[1]);
                numb[2] = int.Parse(StringData[2]);
            }

            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                chart1.Series["Series1"].Points.AddXY(numb[1], numb[2]);
            });
        }

        private void LandingEIS_FormClosing(object sender, FormClosingEventArgs e)
        {
            classes2065.movedata.open_form =0;
        }
    }
}
