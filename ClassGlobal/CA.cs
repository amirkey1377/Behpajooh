using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using Skydat.classes2065;

namespace Skydat
{
    class CA : ITechniques
    {
        public const string fullName = "Chrono Amperometry";

        public const string name = "CA";

        public const byte number = 7;

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

        private string[] _xyAxisTitle = new string[2] { "Current(uA)", "Time(S)" };
        public string[] XY_Axis_Title
        {
            get { return _xyAxisTitle; }
            private set { _xyAxisTitle = value; }
        }

        private bool _colometryMode;
        public bool ColometryMode
        {
            get { return _colometryMode; }
            set
            {
                if (_colometryMode == value) return;
                _colometryMode = value;

                if (value == true)
                    XY_Axis_Title = new string[2] { "Q(I)", "Time(S)" };
                else
                    XY_Axis_Title = new string[2] { "Current(uA)", "Time(S)" };
            }
        }


        public CA(double E1, double E2, int T1, int T2, int Cycles, int EquilibriumTime = 0, string comment = "", bool ColometryMode = false)
        {
            _E1 = E1;
            _E2 = E2;
            _T1 = T1;
            _T2 = T2;
            _Cycles = Cycles;
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;

            this.ColometryMode = ColometryMode;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();

        }

        private double _E1;
        private double _E2;
        private int _T1;
        private int _T2;
        private int _Cycles;
        private int _EquilibriumTime;

        public double E1
        {
            get
            {
                return _E1;
            }
            set
            {
                if (value >= -5 && value <= 5)
                {
                    _E1 = clasglobal.caprms_ocp.E1 = value;
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
                if (value >= -5 && value <= 5)
                {
                    _E2 = clasglobal.caprms_ocp.E2 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public int T1
        {
            get
            {
                return _T1;
            }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    _T1 = clasglobal.caprms_ocp.T1 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public int T2
        {
            get
            {
                return _T2;
            }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    _T2 = clasglobal.caprms_ocp.T2 = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
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
                if (value >= 1 && value <= 10000)
                {
                    _Cycles = clasglobal.caprms_ocp.Cycles = value;
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
                    clasglobal.lsvprms_ocp.EquilibriumTime = (double)value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new CA(_E1, _E2, _T1, _T2, _Cycles, _EquilibriumTime, Comment, ColometryMode);
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

            if (_E1 == 0 && _E2 == 0)//جلوگیری از خطا در کشیدن چارت
                return null;


            int fullCycle = _T1 + _T2;
            //بیشتر از دن سیکل نمایش نمیدهد
            for (int i = 0; i < _Cycles && i < 10; i++)
            {
                if (_T1 != 0)
                {
                    series.Points.AddXY(0 + (i * fullCycle), _E1);
                    series.Points.AddXY(_T1 + (i * fullCycle), _E1);
                }

                if (_T2 != 0)
                {
                    series.Points.AddXY(_T1 + (i * fullCycle), _E2);
                    series.Points.AddXY(fullCycle + (i * fullCycle), _E2);
                }
            }

            return series;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            byte Technum = number;
            buffer[0] = Technum;

            int[] VoltageBuffer = new int[(_T1 + _T2) * _Cycles];

            Array.Resize(ref buffer, 23);

            buffer[1] = 20;
            int E1 = Convert.ToInt16(_E1 * 1000);
            buffer[2] = (byte)(E1 >> 8);   // بایت بالا
            buffer[3] = (byte)(E1 & 0xFF); // بایت پایین

            int E2 = Convert.ToInt16(_E2 * 1000);
            buffer[4] = (byte)(E2 >> 8);   // بایت بالا
            buffer[5] = (byte)(E2 & 0xFF); // بایت پایین

            //Hstep
            buffer[6] = 0;   // بایت بالا

            int T1 = Convert.ToInt32(_T1);
            buffer[7] = (byte)(T1 >> 24);   // بایت بالا
            buffer[8] = (byte)(T1 >> 16); // بایت پایین
            buffer[9] = (byte)(T1 >> 8);   // بایت بالا
            buffer[10] = (byte)(T1 & 0xFF); // بایت پایین

            int Delay = Convert.ToInt16(_EquilibriumTime);
            buffer[11] = 0;
            buffer[12] = 0;

            //int PW = 0;
            buffer[13] = 0;
            buffer[14] = 0;

            //int PH = 0;
            buffer[15] = 0;

            int T2 = Convert.ToInt32(_T2);
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

            for (int i = 0; i < (_T1 + _T2) * _Cycles; i++)
            {
                VoltageBuffer[i] = i + 1;
            }

            return VoltageBuffer;
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            CommondRun += 0.ToString() + "@";

            CommondRun += ".";
        }


        public SeriesChartType SeriesType { get; set; }

    }
}
