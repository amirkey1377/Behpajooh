using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skydat.classes2065
{
    class Tech
    {
        private string _name;
        private static readonly string[] ValidNames = { "NPV", "DPV", "SWV", "CV" };

        public string Name
        {
            get => _name;
            private set
            {
                if (Array.Exists(ValidNames, element => element == value))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException($"Invalid name. Valid names are: {string.Join(", ", ValidNames)}");
                }
            }
        }

        public Tech(string name)
        {
            Name = name;
        }
    }
}
