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
    class CV : ITechniques
    {
        public const string fullName = "Cyclic Voltammetry";

        public const string name = "CV";

        public const byte number = 4;

        public static string discription = "this technique measures the current that develops " +
            "in an electrochemical cell under conditions where voltage is in excess of that predicted " +
            "by the Nernst equation. CV is performed by cycling the potential of a working electrode, and " +
            "measuring the resulting current." +
            "this technique has following properties:\n" +
            "E1\n" +
            "E2\n" +
            "E3\n" +
            "Hstep\n" +
            "Tstep\n" +
            "Scan Rate\n" +
            "Cycle\n" +
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


        public CV(double E1, double E2, double ScanRate, double HStep, int Cycles, int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E3 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            _Cycles = Cycles;
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
                    _E1 = _E3 = clasglobal.cvprms_ocp.E1 = value;
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
                    _E2 = clasglobal.cvprms_ocp.E2 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public double E3
        {
            get
            {
                return _E3;
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
                    _ScanRate = clasglobal.cvprms_ocp.ScanRate = value;
                    SetTstep();
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
                    _HStep = clasglobal.cvprms_ocp.HStep = value;
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

        public int Cycles
        {
            get
            {
                return _Cycles;
            }
            set
            {
                if (value >= 0 && _EquilibriumTime <= 1000)
                {
                    _Cycles = value;
                    clasglobal.cvprms_ocp.Cycles = (double)value;
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
                    clasglobal.cvprms_ocp.EquilibriumTime = (double)value;
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
                _TStep = clasglobal.cvprms_ocp.TStep = divide;
            }
            else
            {
                MessageBox.Show("TStep در بازه مجاز نیست");
            }
        }


        private double _E1;
        private double _E2;
        private double _E3;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private int _Cycles;
        private int _EquilibriumTime;

        public Series Series { get; set; }

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new CV(_E1, _E2, _ScanRate, _HStep, _Cycles, _EquilibriumTime, Comment);
            ClonedTech.SeriesType = SeriesType;
            ClonedTech.ColorCode = ColorCode;
            if (SeriesPath != null)
            {
                ClonedTech.SeriesPath = SeriesPath;
            }
            return ClonedTech;
        }

        public string SeriesPath { get; set; }

        public Series previewChart()
        {

            Series series = new Series($"{name}")
            {
                ChartType = SeriesChartType.StepLine,
                BorderWidth = 2,
                Color = System.Drawing.Color.Blue
            };

            double currentTime = 0;
            double scanRate = _ScanRate; // ولت بر ثانیه
            double sampleTime = _TStep; // مدت زمان نمونه‌گیری (ثانیه)
            double currentV = _E1;

            // CV parameters
            bool forward = true;

            if (_E1 == 0 && _E2 == 0)//جلوگیری از خطا در کشیدن چارت
                return null;

            while (true)
            {
                series.Points.AddXY(currentTime, currentV);
                currentTime += sampleTime;

                if (forward)
                {
                    currentV += scanRate * sampleTime;
                    if (currentV >= _E2)
                    {
                        forward = false; // رسیدیم به قله، باید برگردیم
                    }
                }
                else
                {
                    currentV -= scanRate * sampleTime;
                    if (currentV <= _E1)
                    {
                        series.Points.AddXY(currentTime, currentV);
                        break; // به ولتاژ شروع برگشتیم
                    }
                }
            }

            return series;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            int[] VoltageBuffer;

            Array.Resize(ref buffer, 23);

            buffer[1] = 20;

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
                TotalByteExpectedToRead = ((((E1 - E2) / hs) + 1) * 2) - 1;
                TotalByteExpectedToRead = TotalByteExpectedToRead * 4;

                VoltageBuffer = new int[TotalByteExpectedToRead / 4];
                VoltageBuffer[0] = E1;
                for (int i = 1; i < VoltageBuffer.Length; i++)
                {
                    if (VoltageBuffer[i - 1] > E2)
                        VoltageBuffer[i] = (VoltageBuffer[i - 1] - hs);
                    else
                        VoltageBuffer[i] = (VoltageBuffer[i - 1] + hs);
                }
            }
            else
            {
                TotalByteExpectedToRead = ((((E2 - E1) / hs) + 1) * 2) - 1;
                TotalByteExpectedToRead = TotalByteExpectedToRead * 4;

                VoltageBuffer = new int[TotalByteExpectedToRead / 4];
                bool BackDirction = false;
                VoltageBuffer[0] = E1;
                for (int i = 1; i < VoltageBuffer.Length; i++)
                {
                    if (!BackDirction)
                    {
                        VoltageBuffer[i] = (VoltageBuffer[i - 1] + hs);
                        if (VoltageBuffer[i] >= E2)
                        {
                            BackDirction = true;
                        }
                    }
                    else
                        VoltageBuffer[i] = (VoltageBuffer[i - 1] - hs);
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

            int T2 = 0; //Convert.ToInt32(_T2 * 1000);
            buffer[16] = (byte)(T2 >> 24);   // بایت بالا
            buffer[17] = (byte)(T2 >> 16); // بایت پایین
            buffer[18] = (byte)(T2 >> 8);   // بایت بالا
            buffer[19] = (byte)(T2 & 0xFF); // بایت پایین

            int Cycle = Convert.ToInt16(_Cycles);
            buffer[20] = (byte)(Cycle >> 8);   // بایت بالا
            buffer[21] = (byte)(Cycle & 0xFF); // بایت پایین

            for (int i = 2; i < 22; i++)
            {
                buffer[22] ^= buffer[i];
            }

            return VoltageBuffer;
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            CommondRun += 0.ToString() + "@";

            long Temp = (int)(_E1 * 1000);//e1
            CommondRun += Temp.ToString() + "@";

            Temp = (int)(_E2 * 1000);//e2
            CommondRun += Temp.ToString() + "@";

            Temp = (int)(_HStep * 1000);//hs
            CommondRun += Temp.ToString() + "@";

            Temp = (long)(_TStep * 1000);// t1 
            CommondRun += Temp.ToString() + "@";


            CommondRun += ".";
        }


        public SeriesChartType SeriesType { get; set; }

    }
}
