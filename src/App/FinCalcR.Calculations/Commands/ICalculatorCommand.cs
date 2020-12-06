using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Commands
{
    public interface ICalculatorCommand
    {
        CommandWord CommandWord { get; }

        CommandWord PreviousCommandWord { get; set; }

        bool ShouldExecute(CommandWord commandWord);

        void Execute(params object[] parameter);
    }
}
