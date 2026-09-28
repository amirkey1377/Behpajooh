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
    class DPV : ITechniques
    {
        public const string fullName = "Differential Pulse Voltammetry";

        public const string name = "DPV";

        public const byte number = 2;


        public static string discription = "this technique can be considered as a derivative of linear sweep voltammetry" +
            " , with a series of regular voltage pulses superimposed on the potential linear sweep or stairsteps. " +
            "The current is measured immediately before each potential change, and the current difference is plotted as a function of potential.\n" +
            "this technique has following properties:\n" +
            "E1\n" +
            "E2\n" +
            "Hstep\n" +
            "Tstep\n" +
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


        public DPV(double E1, double E2, double ScanRate, double HStep, double PulseWidth, double PulseHeight, int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            SetTstep();
            _PulseWidth = PulseWidth;
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
                    _E1 = clasglobal.dpvprms_ocp.E1 = value;
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
                    _E2 = clasglobal.dpvprms_ocp.E2 = value;
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
                if (value >= 0.0001 && value <= 0.25)
                {
                    _ScanRate = clasglobal.dpvprms_ocp.ScanRate = value;
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
                    _HStep = clasglobal.dpvprms_ocp.HStep = value;
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

        public double PulseWidth
        {
            get
            {
                return _PulseWidth;
            }
            set
            {
                if (value >= 0.05 && value <= (_TStep - 0.05))
                {
                    _PulseWidth = clasglobal.dpvprms_ocp.PulseWidth = value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
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
                if (value >= 0.001 && value <= 0.25)
                {
                    _PulseHeight = clasglobal.dpvprms_ocp.PulseHeight = value;
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
                    clasglobal.dpvprms_ocp.EquilibriumTime = (double)value;
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        private void SetTstep()
        {
            double divide = Math.Round(_HStep / _ScanRate, 3);
            if (divide >= 0.1 && divide <= 10)
            {
                _TStep = clasglobal.dpvprms_ocp.TStep = divide;
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
        private double _PulseWidth;
        private double _PulseHeight;
        private int _EquilibriumTime;


        public ITechniques Clone()
        {
            ITechniques ClonedTech = new DPV(_E1, _E2, _ScanRate, _HStep, _PulseWidth, _PulseHeight, _EquilibriumTime, Comment);
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

            // DPV parameters
            double pulseAmp = _PulseHeight;
            double pulseDuration = _PulseWidth;
            double baselineDuration = _TStep - _PulseWidth;
            double currentTime = 0;

            if (_E1 == 0 && _E2 == 0)//جلوگیری از خطا در کشیدن چارت
                return null;

            if (_E2 > _E1)
            {
                for (double v = _E1; v <= _E2 + 1e-6; v += _HStep)
                {
                    // Baseline (before pulse)
                    series.Points.AddXY(currentTime, v);
                    currentTime += baselineDuration;
                    series.Points.AddXY(currentTime, v);

                    // Pulse
                    series.Points.AddXY(currentTime, v + pulseAmp);
                    currentTime += pulseDuration;
                    series.Points.AddXY(currentTime, v + pulseAmp);
                }
            }
            else
            {
                for (double v = _E1; v >= _E2 - 1e-6; v -= _HStep)
                {
                    // Baseline (before pulse)
                    series.Points.AddXY(currentTime, v);
                    currentTime += baselineDuration;
                    series.Points.AddXY(currentTime, v);

                    // Pulse
                    series.Points.AddXY(currentTime, v + pulseAmp);
                    currentTime += pulseDuration;
                    series.Points.AddXY(currentTime, v + pulseAmp);
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
            buffer[6] = hs;


            if (E1 > E2)
            {
                TotalByteExpectedToRead = (((E1 - E2) / hs) + 1) * 4;
                //TotalByteExpectedToRead *= 2;


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
                //TotalByteExpectedToRead *= 2; 

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

            int PW = Convert.ToInt16(_PulseWidth * 1000);
            buffer[13] = (byte)(PW >> 8);   // بایت بالا
            buffer[14] = (byte)(PW & 0xFF); // بایت پایین

            var PH = Convert.ToByte(_PulseHeight * 1000);
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

        public SeriesChartType SeriesType { get; set; }

    }
}
