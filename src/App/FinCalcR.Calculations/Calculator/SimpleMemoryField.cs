using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public class SimpleMemoryField : IMemoryField
    {
        public SimpleMemoryField(string id, double defaultValue, string category)
        {
            this.Id = id;
            this.DefaultValue = defaultValue;
            this.Category = category;
        }

        public string Id { get; }

        public string Category { get; }

        public double DefaultValue { get; }

        public double Value { get; set; }

        public void Reset()
        {
            this.Value = this.DefaultValue;
        }
    }
}
