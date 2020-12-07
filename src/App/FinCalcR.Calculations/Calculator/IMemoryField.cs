using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IMemoryField<T>
    {
        string Id { get; }

        string Category { get; }

        T DefaultValue { get; }

        T Value { get; set; }

        void Reset();
    }
}
