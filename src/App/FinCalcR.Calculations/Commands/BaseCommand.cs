using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Commands
{
    public abstract class BaseCommand : ICalculatorCommand
    {
        public abstract CommandWord CommandWord { get; }

        public CommandWord PreviousCommandWord { get; set; }

        public bool ShouldExecute(CommandWord commandWord)
        {
            return this.CommandWord == commandWord;
        }

        public abstract void Execute(params object[] parameter);
    }
}
