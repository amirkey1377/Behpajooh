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
    class OCP : ITechniques
    {
        public const string fullName = "Open Circuit Potential";

        public const string name = "OCP";

        public const byte number = 6;

        public static string discription = "Open Circuit Potential" +
            "this technique has following properties:\n" +
            "Tstep\n";

        public string Name { get; } = name;

        [JsonIgnore]
        public byte Number { get; } = number;

        [JsonIgnore]
        public string FullName { get; } = fullName;

        [JsonIgnore]
        public string Discription { get; } = discription;

        public string Comment { get; set; }

        public int ColorCode { get; set; }

        public string[] XY_Axis_Title { get; } = new string[2] { "Voltage(mV)", "Time(S)" };

        public OCP(TechniqueSpecification specific, int EquilibriumTime = 0, string comment = "")
        {
            specific.InitializeParameterNames();

            ParameterSpecification e1 = specific.Parameters["Time"];
            _time = new Parameter(e1.Name, e1.Description, e1.Default, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }


        private OCP(Parameter Time, int EquilibriumTime = 0, string comment = "")
        {
            _time = Time;
            Comment = comment;
            _EquilibriumTime = EquilibriumTime;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        public OCP(TechniqueSpecification specific, int time, int EquilibriumTime = 0, string comment = "")
        {
            ParameterSpecification timeSpecific = specific.Parameters["Time"];
            _time = new Parameter(timeSpecific.Name, timeSpecific.Description, time, timeSpecific.Max, timeSpecific.Min, timeSpecific.Unit, timeSpecific.Resolution);

         
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            //برای اینکه هر تکنیک کد رنگی مختص به خود را داشته باشد
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }


        private Parameter _time;
        private int _EquilibriumTime;


        public Parameter time
        {
            get
            {
                return _time;
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

        public ITechniques Clone()
        {
            ITechniques ClonedTech = new OCP(_time ,EquilibriumTime ,Comment);
            ClonedTech.SeriesType = SeriesType;
            if (SeriesPath != null)
            {
                ClonedTech.SeriesPath = SeriesPath;
            }
            return ClonedTech;
        }

        public string SeriesPath { get; set; }

        public Series previewChart()
        {
            return null;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            byte Technum = number;
            buffer[0] = Technum;

            int[] TimeBuffer = new int[(int)time.Value + 1];

            Array.Resize(ref buffer, 14);

            buffer[1] = 14;
            int E1 = 0;
            buffer[2] = (byte)(E1 >> 8);   // بایت بالا
            buffer[3] = (byte)(E1 & 0xFF); // بایت پایین

            int E2 = 0;
            buffer[4] = (byte)(E2 >> 8);   // بایت بالا
            buffer[5] = (byte)(E2 & 0xFF); // بایت پایین

            byte hs = 0;
            buffer[6] = hs;   // بایت بالا

            int TS = (int)_time + 1;
            buffer[7] = (byte)(TS >> 24);   // بایت بالا
            buffer[8] = (byte)(TS >> 16); // بایت پایین
            buffer[9] = (byte)(TS >> 8);   // بایت بالا
            buffer[10] = (byte)(TS & 0xFF); // بایت پایین

            int Delay = 0;
            buffer[11] = (byte)(Delay >> 8);   // بایت بالا
            buffer[12] = (byte)(Delay & 0xFF); // بایت پایین


            for (int i = 2; i < 11; i++)
            {
                buffer[13] ^= buffer[i];
            }

            TimeBuffer[0] = 0;
            for (int i = 1; i <= time; i++)
            {
                TimeBuffer[i] = TimeBuffer[i - 1] + 1;
            }

            return TimeBuffer;
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            throw new NotImplementedException();
        }

        public SeriesChartType SeriesType { get; set; }


    }
}
