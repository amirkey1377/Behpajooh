using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Skydat
{
    class Tafel : ITechniques
    {
        public const string fullName = "Tafel Plot";

        public const string name = "Tafel";

        public const byte number = 8;

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

        private double _E1;
        private double _E2;
        private double _ScanRate;
        private double _HStep;
        private double _TStep;
        private int _EquilibriumTime;

        public Tafel(double E1, double E2, double ScanRate, double HStep, int EquilibriumTime = 0, string comment = "")
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

        private void SetTstep()
        {
            double divide = Math.Round(_HStep / _ScanRate, 4);
            if (divide >= 0.001 && divide <= 10)
            {
                _TStep = divide;
            }
            else
            {
                MessageBox.Show("TStep در بازه مجاز نیست");
            }
        }

        public int EquilibriumTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SeriesPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SeriesChartType SeriesType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ITechniques Clone()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            throw new NotImplementedException();
        }
    }
}
