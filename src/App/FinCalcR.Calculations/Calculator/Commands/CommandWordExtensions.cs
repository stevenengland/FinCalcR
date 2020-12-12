﻿namespace StEn.FinCalcR.Calculations.Calculator.Commands
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