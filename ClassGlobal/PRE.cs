using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using Skydat.classes2065;
using System.ComponentModel;

namespace Skydat
{
    class PRE : ITechniques, ICustomTypeDescriptor
    {
        public const string fullName = "Pre-condition";
        public const string name = "PRE";
        public const byte number = 7;

        public static string discription = "this technique measures the current that develops " +
            "in an electrochemical cell under conditions where voltage is in excess of that predicted " +
            "by the Nernst equation. CV is performed by cycling the potential of a working electrode, and " +
            "measuring the resulting current.";

        public const string parentcategory = "Measurement - general";

        [Browsable(false)]
        public string Name { get; } = name;

        [JsonIgnore, Browsable(false)]
        public byte Number { get; } = number;

        [JsonIgnore, Browsable(false)]
        public string FullName { get; } = fullName;

        [JsonIgnore, Browsable(false)]
        public string Discription { get; } = discription;

        [JsonIgnore, Browsable(false)]
        public string ParentCategory { get; } = parentcategory;

        public string Comment { get; set; }

        [Browsable(false)]
        public int ColorCode { get; set; }

        [Browsable(false)]
        private string[] _xyAxisTitle = new string[2] { "Current(uA)", "Time(S)" };

        [Browsable(false)]
        public string[] XY_Axis_Title
        {
            get { return _xyAxisTitle; }
            private set { _xyAxisTitle = value; }
        }

        private bool _colometryMode;

        [Browsable(false)]
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


        // ================== سازنده اول (از TechniqueSpecification) ==================
        public PRE(TechniqueSpecification specific, int EquilibriumTime = 0, string comment = "")
        {
            specific.InitializeParameterNames();

            ParameterSpecification e1 = specific.Parameters["E1"];
            _E1 = new Parameter(e1.Name, e1.Description, e1.Default, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            ParameterSpecification t1 = specific.Parameters["T1"];
            _T1 = new Parameter(t1.Name, t1.Description, t1.Default, t1.Max, t1.Min, t1.Unit, t1.Resolution);

            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        // ================== سازنده دوم (از Parameter ها برای Clone) ==================
        public PRE(Parameter E1,  Parameter T1, int EquilibriumTime = 0, string comment = "", bool ColometryMode = false)
        {
            _E1 = E1; _T1 = T1;
            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            this.ColometryMode = ColometryMode;
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        // ================== سازنده سوم (از مقادیر عددی برای فرآیند لود) ==================
        public PRE(TechniqueSpecification specific, double E1, int T1, int EquilibriumTime = 0, string comment = "")
        {
            ParameterSpecification e1 = specific.Parameters["E1"];
            _E1 = new Parameter(e1.Name, e1.Description, E1, e1.Max, e1.Min, e1.Unit, e1.Resolution);

            ParameterSpecification t1 = specific.Parameters["T1"];
            _T1 = new Parameter(t1.Name, t1.Description, T1, t1.Max, t1.Min, t1.Unit, t1.Resolution);


            _EquilibriumTime = EquilibriumTime;
            Comment = comment;
            ColorCode = ColorUtility.GenerateHighContrastColor();
        }

        private Parameter _E1;
        private Parameter _T1;
        private int _EquilibriumTime;

        [Browsable(false)]
        public Parameter E1 { get { return _E1; } }

        [Category("PRE Values"), DisplayName("E1"), Description("")]
        public double E1_Value
        {
            get { return _E1.Value; }
            set { _E1.Value = value; }
        }
        [Browsable(false)]
        public Parameter T1 { get { return _T1; } }

        [Category("PRE Values"), DisplayName("T1"), Description("")]
        public double T1_Value
        {
            get { return _T1.Value; }
            set { _T1.Value = value; }
        }

        [Category("PRE Values"), DisplayName("EquilibriumTime")]
        public int EquilibriumTime
        {
            get { return _EquilibriumTime; }
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
            ITechniques ClonedTech = new PRE(_E1, _T1, _EquilibriumTime, Comment, ColometryMode);
            ClonedTech.SeriesType = SeriesType;
            ClonedTech.ColorCode = ColorCode;
            if (SeriesPath != null) ClonedTech.SeriesPath = SeriesPath;
            return ClonedTech;
        }

        [Browsable(false)]
        public string SeriesPath { get; set; }

        [Browsable(false)]
        public SeriesChartType SeriesType { get; set; }

        public Series previewChart()
        {
            Series series = new Series($"{name}") { ChartType = SeriesChartType.StepLine, BorderWidth = 2, Color = System.Drawing.Color.Blue };
            int fullCycle = (int)(_T1);
            if (_T1 != 0) { series.Points.AddXY(0, _E1); series.Points.AddXY(_T1 , _E1); }
            return series;
        }

        public int[] SendToUsb(ref int TotalByteExpectedToRead, ref byte[] buffer)
        {
            byte Technum = number;
            buffer[0] = Technum;
            int[] VoltageBuffer = new int[(int)(_T1)];
            Array.Resize(ref buffer, 23);
            buffer[1] = 20;
            int E1 = Convert.ToInt16(_E1 * 1000); buffer[2] = (byte)(E1 >> 8); buffer[3] = (byte)(E1 & 0xFF);
            buffer[4] = 0;
            buffer[5] = 0;
            buffer[6] = 0;
            int T1 = Convert.ToInt32(_T1); buffer[7] = (byte)(T1 >> 24); buffer[8] = (byte)(T1 >> 16); buffer[9] = (byte)(T1 >> 8); buffer[10] = (byte)(T1 & 0xFF);
            int Delay = Convert.ToInt16(_EquilibriumTime); buffer[11] = 0; buffer[12] = 0;
            buffer[13] = 0; buffer[14] = 0; buffer[15] = 0; buffer[16] = 0; buffer[17] = 0; buffer[18] = 0; buffer[19] = 0; buffer[20] = 0; buffer[21] = 0;
            for (int i = 2; i < 22; i++) { buffer[22] ^= buffer[i]; }
            for (int i = 0; i < _T1 ; i++) { VoltageBuffer[i] = i + 1; }
            return VoltageBuffer;
        }

        public void SendToUsb_For_2066(ref string CommondRun)
        {
            CommondRun += 0.ToString() + "@";
            CommondRun += ".";
        }

        // ================== تزریق توضیحات داینامیک (ICustomTypeDescriptor) ==================
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
        public string GetClassName() => TypeDescriptor.GetClassName(this, true);
        public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);
        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
        public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);
        public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);
        public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);
        public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);
        public object GetPropertyOwner(PropertyDescriptor pd) => this;
        public PropertyDescriptorCollection GetProperties() => GetProperties(new Attribute[0]);

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var props = TypeDescriptor.GetProperties(this, attributes, true);
            var newProps = new List<PropertyDescriptor>();

            foreach (PropertyDescriptor prop in props)
            {
                string dynamicDescription = "";

                // خواندن توضیحات از روی متغیرهای اصلی _E1 و ...
                switch (prop.Name)
                {
                    case "E1_Value": dynamicDescription = _E1.Description; break;
                    case "T1_Value": dynamicDescription = _T1.Description; break;
                }

                if (!string.IsNullOrEmpty(dynamicDescription))
                {
                    Attribute[] attrs = new Attribute[prop.Attributes.Count + 1];
                    prop.Attributes.CopyTo(attrs, 0);
                    attrs[attrs.Length - 1] = new DescriptionAttribute(dynamicDescription);
                    newProps.Add(TypeDescriptor.CreateProperty(typeof(PRE), prop, attrs));
                }
                else
                {
                    newProps.Add(prop);
                }
            }
            return new PropertyDescriptorCollection(newProps.ToArray());
        }
        // =====================================================================================
    }
}