using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IResultTextFormatter
    {
        string GetFormattedResult(string input);
    }
}
