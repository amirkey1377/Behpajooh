using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skydat.classes2065
{
    public class Tech
    {
        private string _name;
        private string _type;

        private static readonly string[] ValidNames = { "NPV", "DPV", "SWV", "CV" };

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                //if (Array.Exists(ValidNames, element => element == value))
                //{
                //    _name = value;
                //}
                //else
                //{
                //    throw new ArgumentException($"Invalid name. Valid names are: {string.Join(", ", ValidNames)}");
                //}
            }
        }

        public string Type
        {
            get => _type;
            private set
            {
                if (Array.Exists(ValidNames, element => element == value))
                {
                    _type = value;
                }
                else
                {
                    throw new ArgumentException($"Invalid Type. Valid Types are: {string.Join(", ", ValidNames)}");
                }
            }
        }

        public Tech(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    class NPV : Tech
    {
        public double Intial_E;
        public double Final_E;
        public NPV(string name, string type, double intial_E, double final_E) : base(name, type)
        {
            Intial_E = intial_E;
            Final_E = final_E;
        }
    }
}
