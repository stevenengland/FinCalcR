using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryField
    {
        string Id { get; }

        string Category { get; }

        double DefaultValue { get; }

        double Value { get; set; }

        void Reset();
    }
}
