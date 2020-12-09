using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Commands
{
    public enum CommandWord
    {
        None = 0,
        DecimalSeparator = 1,
        Years,
        Interest,
        Start,
        Rate,
        End,
        RatesPerAnnum,
        PercentCalculation,
        Clear,
        Operator,
        Digit,
        AlgebSign,
        Calculate,
    }
}
