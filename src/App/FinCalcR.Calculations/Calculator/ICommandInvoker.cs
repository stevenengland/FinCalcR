using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface ICommandInvoker
    {
        void InvokeCommand(CommandWord commandWord, params object[] parameter);
    }
}
