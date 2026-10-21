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

        public DPV(TechniqueSpecification specific, int EquilibriumTime = 0, string comment = "")
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

            ParameterSpecification PulseWidth = specific.Parameters["PulseWidth"];
            _PulseWidth = new Parameter(PulseWidth.Name, PulseWidth.Description, PulseWidth.Default, PulseWidth.Max, PulseWidth.Min, PulseWidth.Unit, PulseWidth.Resolution);

            ParameterSpecification PulseHeight = specific.Parameters["PulseHeight"];
            _PulseHeight = new Parameter(PulseHeight.Name, PulseHeight.Description, PulseHeight.Default, PulseHeight.Max, PulseHeight.Min, PulseHeight.Unit, PulseHeight.Resolution);

            ParameterSpecification Tstep = specific.Parameters["Tstep"];
            _TStep = new Parameter(Tstep.Name, Tstep.Description, Tstep.Default, Tstep.Max, Tstep.Min, Tstep.Unit, Tstep.Resolution);

            SetTstep();

            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();

            _HStep.ValueChanged += HStep_ValueChanged;
            _ScanRate.ValueChanged += HStep_ValueChanged;

        }

        public DPV(Parameter E1, Parameter E2, Parameter ScanRate, Parameter HStep, Parameter PulseWidth
            , Parameter PulseHeight, Parameter TStep,  int EquilibriumTime = 0, string comment = "")
        {
            _E1 = E1;
            _E2 = E2;
            _ScanRate = ScanRate;
            _HStep = HStep;
            _TStep = TStep;
            _PulseWidth = PulseWidth;
            _PulseHeight = PulseHeight;
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        public DPV(TechniqueSpecification specific, double E1, double E2, double ScanRate, double HStep
            , double PulseWidth, double PulseHeight, int EquilibriumTime = 0, string comment = "")
        {
            ParameterSpecification e1 = specific.Parameters["E1"];
            _E1 = new Parameter(e1.Name, e1.Description, E1, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            ParameterSpecification e2 = specific.Parameters["E2"];
            _E2 = new Parameter(e2.Name, e2.Description, E2, e2.Max, e2.Min, e2.Unit, e2.Resolution);

            ParameterSpecification ScanRateSpecific = specific.Parameters["ScanRate"];
            _ScanRate = new Parameter(ScanRateSpecific.Name, ScanRateSpecific.Description, ScanRate, ScanRateSpecific.Max, ScanRateSpecific.Min, ScanRateSpecific.Unit, ScanRateSpecific.Resolution);

            ParameterSpecification Hstep = specific.Parameters["Hstep"];
            _HStep = new Parameter(Hstep.Name, Hstep.Description, HStep, Hstep.Max, Hstep.Min, Hstep.Unit, Hstep.Resolution);

            ParameterSpecification PulseWidthSpecific = specific.Parameters["PulseWidth"];
            _PulseWidth = new Parameter(PulseWidthSpecific.Name, PulseWidthSpecific.Description, PulseWidth, PulseWidthSpecific.Max, PulseWidthSpecific.Min, PulseWidthSpecific.Unit, PulseWidthSpecific.Resolution);

            ParameterSpecification PulseHeightSpecific = specific.Parameters["PulseHeight"];
            _PulseHeight = new Parameter(PulseHeightSpecific.Name, PulseHeightSpecific.Description, PulseHeight, PulseHeightSpecific.Max, PulseHeightSpecific.Min, PulseHeightSpecific.Unit, PulseHeightSpecific.Resolution);

            ParameterSpecification Tstep = specific.Parameters["Tstep"];
            _TStep = new Parameter(Tstep.Name, Tstep.Description, Tstep.Default, Tstep.Max, Tstep.Min, Tstep.Unit, Tstep.Resolution);

            SetTstep();
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        private void HStep_ValueChanged(object sender, EventArgs e)
        {
            SetTstep();
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

        public Parameter TStep
        {
            get
            {
                return _TStep;
            }
        }

        public Parameter PulseWidth
        {
            get
            {
                return _PulseWidth;
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
                }
                else
                    MessageBox.Show("مقدار وارد شده مجاز نیست");
            }
        }

        private void SetTstep()
        {
            double divide = Math.Round(_HStep / _ScanRate, 3);
            if (divide >= _TStep.LowerLimit && divide <= _TStep.UpperLimit)
            {
                _TStep.Value = divide;
                _PulseWidth.UpperLimit = _TStep - 0.05;
            }
            else
            {
                MessageBox.Show("TStep در بازه مجاز نیست");
            }
        }

        private Parameter _E1;
        private Parameter _E2;
        private Parameter _ScanRate;
        private Parameter _HStep;
        private Parameter _TStep;
        private Parameter _PulseWidth;
        private Parameter _PulseHeight;
        private int _EquilibriumTime;


        public ITechniques Clone()
        {
            ITechniques ClonedTech = new DPV(_E1, _E2, _ScanRate, _HStep, _PulseWidth, _PulseHeight, _TStep, _EquilibriumTime, Comment);
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
