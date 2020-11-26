using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Calculator.Display
{
    public interface IOutputText
    {
        string TextValue { get; }

        double NumberValue { get; }

        IOutputTextProcessor Processor { get; }
    }
}
