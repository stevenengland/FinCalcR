using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryFieldValue<T>
    {
        T DefaultValue { get; }

        T Value { get; set; }
    }
}
