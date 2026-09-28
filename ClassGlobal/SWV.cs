using Newtonsoft.Json;
using Skydat.classes2065;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    class SWV : ITechniques
    {
        public const string fullName = "Square Wave Voltammetry";

        public const string name = "SWV";

        public const byte number = 3;

        public static string discription = "In this technique , the current at a working electrode is measured while the " +
            "potential between the working electrode and a reference electrode is swept linearly in time. " +
            "The current is sampled at two times - once at the end of the forward potential pulse and again at the end of the reverse potential pulse" +
            "this technique has following properties:\n" +
            "E1\n" +
            "E2\n" +
            "Hstep\n" +
            "Frequency\n" +
            "Scan Rate\n" +
            "Pulse Width\n" +
            "Pulse Height\n" +
            "Equilibrium Time";

        public string Name { get; } = name;

        [JsonIgnore]
        public byte Number { get; } = number;

        [JsonIgnore]
        public string FullName { get; } = fullName;

        [JsonIgnore]
        public string Discription { get; } = discription;

        public string Comment { get; set; }

        public int ColorCode { get; set; }

        public string[] XY_Axis_Title { get; } = new string[2] { "Current(uA)", "Voltage(mV)" };

        public SWV(double E1, double E2, double ScanRate, double HStep, double PulseHeight, int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            SetFrequency();
            _PulseHeight = PulseHeight;
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        public double E1
        {
            get
            {
                return _E1;
            }
            set
            {
                if (value >= -8 && value <= 8)
                {
                    _E1 = clasglobal.swvprms_ocp.E1 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double E2
        {
            get
            {
                return _E2;
            }
            set
            {
                if (value >= -8 && value <= 8)
                {
                    _E2 = clasglobal.swvprms_ocp.E2 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double ScanRate
        {
            get
            {
                return _ScanRate;
            }
            set
            {
                if (value >= 0.001 && value <= 25)
                {
                    _ScanRate = clasglobal.swvprms_ocp.ScanRate = value;
                    SetFrequency();
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double HStep
        {
            get
            {
                return _HStep;
            }
            set
            {
                if (value >= 0.001 && value <= 0.025)
                {
                    _HStep = clasglobal.swvprms_ocp.HStep = value;
                    SetFrequency();

                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double Frequency
        {
            get
            {
                return _Frequency;
            }
        }

        private void SetFrequency()
        {
            if (_HStep == 0)
                return;

            double divide = Math.Round(_ScanRate / _HStep, 3);
            if (divide >= 1 && divide <= 1000)
            {
                _Frequency = clasglobal.swvprms_ocp.Frequency = divide;
            }
            else
            {
                MessageBox.Show("Frequency در بازه مجاز نیست");
            }
        }

        public double PulseHeight
        {
            get
            {
                return _PulseHeight;
            }
            set
            {
                if (value >= 0.05 && (_PulseHeight - 0.5) <= 0.025)
                {
                    _PulseHeight = clasglobal.swvprms_ocp.PulseHeight = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public int EquilibriumTime
        {
            get
            {
                return _EquilibriumTime;
            }
            set
            {
                if (value >= 0 && _EquilibriumTime <= 2000)
                {
                    _EquilibriumTime = value;
                    clasglobal.swvprms_ocp.EquilibriumTime = (double)value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _Frequency;
        private double _PulseHeight;
        private int _EquilibriumTime;

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new SWV(_E1, _E2, _ScanRate, _HStep, _PulseHeight, _EquilibriumTime, Comment);
            ClonedTech.SeriesType = SeriesType;
            ClonedTech.ColorCode = ColorCode;
            if (SeriesPath != null)
            {
                ClonedTech.SeriesPath = SeriesPath;
            }
            return ClonedTech;
        }

        public Series previewChart()
        {
            Series series = new Series($"{name}")
            {
                ChartType = SeriesChartType.StepLine,
                BorderWidth = 2,
                Color = System.Drawing.Color.Blue
            };

            // SWV parameters
            double squareAmplitude = 2 * _PulseHeight;
            double halfCycleDuration = (1 / _Frequency) / 2; // مدت زمان نیم سیکل
            double currentTime = 0;

            if (_E2 == 0 && _E1 == 0)//جلوگیری از خطا در کشیدن چارت
                return null;

            if (_E1 < _E2)
            {
                for (double baseV = _E1; baseV <= _E2 + 1e-6; baseV += _HStep)
                {
                    // Positive half-cycle
                    series.Points.AddXY(currentTime, baseV + _PulseHeight);
                    currentTime += halfCycleDuration;
                    series.Points.AddXY(currentTime, baseV + _PulseHeight);

                    // Negative half-cycle
                    series.Points.AddXY(currentTime, baseV - squareAmplitude);
                    currentTime += halfCycleDuration;
                    series.Points.AddXY(currentTime, baseV - squareAmplitude);
                }
            }
            else
            {
                for (double baseV = _E1; baseV >= _E2 + 1e-6; baseV -= _HStep)
                {

                    // Positive half-cycle
                    series.Points.AddXY(currentTime, baseV + _PulseHeight);
                    currentTime += halfCycleDuration;
                    series.Points.AddXY(currentTime, baseV + _PulseHeight);

                    // Negative half-cycle
                    series.Points.AddXY(currentTime, baseV - squareAmplitude);
                    currentTime += halfCycleDuration;
                    series.Points.AddXY(currentTime, baseV - squareAmplitude);      
                }
            }

            return series;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            int[] VoltageBuffer;

            Array.Resize(ref buffer, 17);

            buffer[1] = 14;
            int E1 = Convert.ToInt16(_E1 * 1000);
            buffer[2] = (byte)(E1 >> 8);   // بایت بالا
            buffer[3] = (byte)(E1 & 0xFF); // بایت پایین

            int E2 = Convert.ToInt16(_E2 * 1000);
            buffer[4] = (byte)(E2 >> 8);   // بایت بالا
            buffer[5] = (byte)(E2 & 0xFF); // بایت پایین

            byte hs = Convert.ToByte(_HStep * 1000);
            buffer[6] = hs;   // بایت بالا

            if (E1 > E2)
            {
                TotalByteExpectedToRead = (((E1 - E2) / hs) + 1) * 4;

                VoltageBuffer = new int[TotalByteExpectedToRead / 4];
                VoltageBuffer[0] = E1;
                for (int i = 1; i < VoltageBuffer.Length; i++)
                {
                    VoltageBuffer[i] = (VoltageBuffer[i - 1] - hs);

                    //if (VoltageBuffer[i] == E2)
                    //{

                    //}
                }
            }
            else
            {
                TotalByteExpectedToRead = (((E2 - E1) / hs) + 1) * 4;

                VoltageBuffer = new int[TotalByteExpectedToRead / 4];
                VoltageBuffer[0] = E1;
                for (int i = 1; i < VoltageBuffer.Length; i++)
                {
                    VoltageBuffer[i] = (VoltageBuffer[i - 1] + hs);
                }
            }

            int TS = Convert.ToInt32((1 / _Frequency) * 1000);
            buffer[7] = (byte)(TS >> 24);   // بایت بالا
            buffer[8] = (byte)(TS >> 16); // بایت پایین
            buffer[9] = (byte)(TS >> 8);   // بایت بالا
            buffer[10] = (byte)(TS & 0xFF); // بایت پایین

            int Delay = Convert.ToInt16(_EquilibriumTime);
            buffer[11] = (byte)(Delay >> 8);   // بایت بالا
            buffer[12] = (byte)(Delay & 0xFF); // بایت پایین

            //این کد برای جور درامدن استراکچر کد میکرو است و از لحاظ نرم افزاری نوجیهی ندارد
            int PW = 0;
            buffer[13] = (byte)(PW >> 8);   // بایت بالا
            buffer[14] = (byte)(PW & 0xFF); // بایت پایین

            byte PH = Convert.ToByte(_PulseHeight * 1000);
            buffer[15] = PH;


            for (int i = 2; i < 16; i++)
            {
                buffer[16] ^= buffer[i];
            }

            return VoltageBuffer;
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            throw new NotImplementedException();
        }

        public string SeriesPath { get; set; }

        public SeriesChartType SeriesType { get; set; }

    }
}
