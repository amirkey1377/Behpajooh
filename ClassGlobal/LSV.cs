using Newtonsoft.Json;
using Skydat.classes2065;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    class LSV : ITechniques
    {
        public const string fullName = "Linear Sweep Voltammetry";

        public const string name = "LSV";

        public const byte number = 5;

        public static string discription = "in this technique, the current at a working " +
            "electrode is measured while the potential between the working electrode and " +
            "a reference electrode is swept linearly in time. Oxidation or reduction of species " +
            "is registered as a peak or trough in the current signal at the potential at which the " +
            "species begins to be oxidized or reduced." +
            "this technique has following properties:\n" +
            "E1\n" +
            "E2\n" +
            "Hstep\n" +
            "Tstep\n" +
            "Scan Rate\n" +
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


        public LSV(double E1, double E2, double ScanRate, double HStep, int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            SetTstep();
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
                    _E1 = clasglobal.lsvprms_ocp.E1 = value;
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
                    _E2 = clasglobal.lsvprms_ocp.E2 = value;
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
                    _HStep = clasglobal.lsvprms_ocp.HStep = value;
                    SetTstep();
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
                if (value >= 0.0001 && value <= 250)
                {
                    _ScanRate = clasglobal.lsvprms_ocp.ScanRate = value;
                    SetTstep();

                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double TStep
        {
            get
            {
                return _TStep;
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
                    clasglobal.lsvprms_ocp.EquilibriumTime = (double)value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        private void SetTstep()
        {
            double divide = Math.Round(_HStep / _ScanRate, 4);
            if (divide >= 0.001 && divide <= 10)
            {
                _TStep = clasglobal.lsvprms_ocp.TStep = divide;
            }
            else
            {
                MessageBox.Show("TStep در بازه مجاز نیست");
            }
        }

        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private int _EquilibriumTime;

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new LSV(_E1, _E2, _ScanRate, _HStep, _EquilibriumTime, Comment);
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


            // LSV parameters
            var scanRate = _ScanRate; // ولت بر ثانیه
            var sampleTime = _TStep; // مدت زمان نمونه‌گیری (ثانیه)
            double currentTime = 0;
            double currentV_LSV = _E1;

            if (_E1 == 0 && _E2 == 0)//جلوگیری از خطا در کشیدن چارت
                return null;

            if (_E1 < _E2)
            {
                while (currentV_LSV <= _E2)
                {
                    series.Points.AddXY(currentTime, currentV_LSV);
                    currentTime += sampleTime;
                    currentV_LSV += scanRate * sampleTime;
                }
            }
            else
            {
                while (currentV_LSV >= _E2)
                {
                    series.Points.AddXY(currentTime, currentV_LSV);
                    currentTime += sampleTime;
                    currentV_LSV -= scanRate * sampleTime;
                }
            }

            return series;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            int[] VoltageBuffer;
            Array.Resize(ref buffer, 14);
            buffer[1] = 11;
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

            int TS = Convert.ToInt32(_TStep * 1000);
            buffer[7] = (byte)(TS >> 24);   // بایت بالا
            buffer[8] = (byte)(TS >> 16); // بایت پایین
            buffer[9] = (byte)(TS >> 8);   // بایت بالا
            buffer[10] = (byte)(TS & 0xFF); // بایت پایین

            int Delay = Convert.ToInt16(_EquilibriumTime);
            buffer[11] = (byte)(Delay >> 8);   // بایت بالا
            buffer[12] = (byte)(Delay & 0xFF); // بایت پایین

            for (int i = 2; i < 13; i++)
            {
                buffer[13] ^= buffer[i];
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
