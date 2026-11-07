using Newtonsoft.Json;
using Skydat.classes2065;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public const string parentcategory = "Measurement - voltammetric analysis";

        public string Name { get; } = name;

        [JsonIgnore]
        public byte Number { get; } = number;

        [JsonIgnore]
        public string FullName { get; } = fullName;

        [JsonIgnore]
        public string Discription { get; } = discription;

        [JsonIgnore, Browsable(false)]
        public string ParentCategory { get; } = parentcategory;

        public string Comment { get; set; }

        public int ColorCode { get; set; }

        public string[] XY_Axis_Title { get; } = new string[2] { "Current(uA)", "Voltage(mV)" };

        public SWV(TechniqueSpecification specific, int EquilibriumTime = 0, string comment = "")
        {
            specific.InitializeParameterNames();

            ParameterSpecification e1 = specific.Parameters["E1"];
            _E1 = new Parameter(e1.Name, e1.Description, e1.Default, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            ParameterSpecification e2 = specific.Parameters["E2"];
            _E2 = new Parameter(e2.Name, e2.Description, e2.Default, e2.Max, e2.Min, e2.Unit, e2.Resolution);

            ParameterSpecification ScanRate = specific.Parameters["ScanRate"];
            _ScanRate = new Parameter(ScanRate.Name, ScanRate.Description, ScanRate.Default, ScanRate.Max, ScanRate.Min, ScanRate.Unit, ScanRate.Resolution);

            ParameterSpecification Hstep = specific.Parameters["Hstep"];
            _HStep = new Parameter(Hstep.Name, Hstep.Description, Hstep.Default, Hstep.Max, Hstep.Min, Hstep.Unit, Hstep.Resolution);

            ParameterSpecification PulseHeight = specific.Parameters["PulseHeight"];
            _PulseHeight = new Parameter(PulseHeight.Name, PulseHeight.Description, PulseHeight.Default, PulseHeight.Max, PulseHeight.Min, PulseHeight.Unit, PulseHeight.Resolution);

            ParameterSpecification Frequency = specific.Parameters["Frequency"];
            _Frequency = new Parameter(Frequency.Name, Frequency.Description, Frequency.Default, Frequency.Max, Frequency.Min, Frequency.Unit, Frequency.Resolution);

            SetFrequency();

            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();

            _HStep.ValueChanged += HStep_ValueChanged;
            _ScanRate.ValueChanged += HStep_ValueChanged;

        }

        private SWV(Parameter E1, Parameter E2, Parameter ScanRate, Parameter HStep, Parameter PulseHeight, Parameter Frequency, int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            _Frequency = Frequency;
            _PulseHeight = PulseHeight;
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        public SWV(TechniqueSpecification specific, double E1, double E2, double ScanRate, double HStep, double PulseWidth, int EquilibriumTime = 0, string comment = "")
        {
            ParameterSpecification e1 = specific.Parameters["E1"];
            _E1 = new Parameter(e1.Name, e1.Description, E1, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            ParameterSpecification e2 = specific.Parameters["E2"];
            _E2 = new Parameter(e2.Name, e2.Description, E2, e2.Max, e2.Min, e2.Unit, e2.Resolution);

            ParameterSpecification ScanRateSpecific = specific.Parameters["ScanRate"];
            _ScanRate = new Parameter(ScanRateSpecific.Name, ScanRateSpecific.Description, ScanRate, ScanRateSpecific.Max, ScanRateSpecific.Min, ScanRateSpecific.Unit, ScanRateSpecific.Resolution);

            ParameterSpecification Hstep = specific.Parameters["Hstep"];
            _HStep = new Parameter(Hstep.Name, Hstep.Description, HStep, Hstep.Max, Hstep.Min, Hstep.Unit, Hstep.Resolution);

            ParameterSpecification PulseHeight = specific.Parameters["PulseHeight"];
            _PulseHeight = new Parameter(PulseHeight.Name, PulseHeight.Description, PulseHeight.Default, PulseHeight.Max, PulseHeight.Min, PulseHeight.Unit, PulseHeight.Resolution);

            ParameterSpecification Frequency = specific.Parameters["Frequency"];
            _Frequency = new Parameter(Frequency.Name, Frequency.Description, Frequency.Default, Frequency.Max, Frequency.Min, Frequency.Unit, Frequency.Resolution);

            SetFrequency();
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        public Parameter E1
        {
            get
            {
                return _E1;
            }
        }

        public Parameter E2
        {
            get
            {
                return _E2;
            }
        }

        public Parameter ScanRate
        {
            get
            {
                return _ScanRate;
            }
        }

        public Parameter HStep
        {
            get
            {
                return _HStep;
            }
        }

        public Parameter Frequency
        {
            get
            {
                return _Frequency;
            }
        }

        private void HStep_ValueChanged(object sender, EventArgs e)
        {
            SetFrequency();
        }

        private void SetFrequency()
        {
            if (_HStep == 0)
                return;

            double divide = Math.Round(_ScanRate / _HStep, 3);
            if (divide >= _Frequency.LowerLimit && divide <= _Frequency.UpperLimit)
            {
                _Frequency.Value = divide;
            }
            else
            {
                MessageBox.Show("Frequency در بازه مجاز نیست");
            }
        }

        public Parameter PulseHeight
        {
            get
            {
                return _PulseHeight;
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

        private Parameter _E1;
        private Parameter _E2;
        private Parameter _ScanRate;
        private Parameter _HStep;
        private Parameter _Frequency;
        private Parameter _PulseHeight;
        private int _EquilibriumTime;

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new SWV(_E1, _E2, _ScanRate, _HStep, _PulseHeight, _Frequency, _EquilibriumTime, Comment);
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
            byte Technum = number;
            buffer[0] = Technum;

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
