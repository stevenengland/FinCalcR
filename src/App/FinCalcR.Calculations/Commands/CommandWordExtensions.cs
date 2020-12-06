using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Commands
{
    public static class CommandWordExtensions
    {
        public static bool IsSpecialCommandWord(this CommandWord commandWord)
        {
            return commandWord == CommandWord.Years
                   || commandWord == CommandWord.Interest
                   || commandWord == CommandWord.Start
                   || commandWord == CommandWord.Rate
                   || commandWord == CommandWord.End
                   || commandWord == CommandWord.RatesPerAnnum;
        }
    }
}
