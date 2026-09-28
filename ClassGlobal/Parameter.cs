using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skydat
{
    public class Parameter
    {
        public string Name { get; }
        public string Description { get; }

        private double _value;

        public event EventHandler ValueChanged;

        public double Value
        {
            get => _value;
            set
            {
                if (value < LowerLimit || value > UpperLimit)
                {
                    throw new ArgumentOutOfRangeException(
                                        nameof(value),
                                        $"Value must be between {LowerLimit} and {UpperLimit} {Unit}");
                }

                if (_value == value)
                    return;

                _value = value;

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }

        public string Unit { get; }
        public double Resolution { get; }

        public Parameter(
            string name,
            string description,
            double value,
            double upperLimit,
            double lowerLimit,
            string unit = null,
            double resolution = 0)
        {
            Name = name;
            Description = description;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Unit = unit;
            Resolution = resolution;

            Value = value;
        }

 

        public static implicit operator double(Parameter parameter)
            => parameter.Value;

    }
}
