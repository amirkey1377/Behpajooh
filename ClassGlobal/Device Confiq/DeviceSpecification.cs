using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Skydat
{
    public class DeviceSpecification
    {
        public DeviceInfo Info { get; set; }

        public Dictionary<string, TechniqueSpecification> Techniques { get; set; }


        public static DeviceSpecification Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<DeviceSpecification>(json);
        }
    }

    public class TechniqueSpecification
    {
        public bool Enabled { get; set; }

        // مقداردهی اولیه برای جلوگیری از خطای Null در زمان ساخت آبجکت
        public Dictionary<string, ParameterSpecification> Parameters { get; set; } = new Dictionary<string, ParameterSpecification>();

        public void InitializeParameterNames()
        {
            // بررسی مجدد برای زمانی که از JSON خوانده می‌شود و ممکن است null باشد
            if (Parameters == null)
            {
                Parameters = new Dictionary<string, ParameterSpecification>();
            }

            foreach (var parameter in Parameters)
            {
                parameter.Value.Name = parameter.Key;
            }
        }
    }

    public class DeviceInfo
    {
        public string Model { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class ParameterSpecification
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Default { get; set; }
        public string Unit { get; set; }
        public double Resolution { get; set; }
    }
}