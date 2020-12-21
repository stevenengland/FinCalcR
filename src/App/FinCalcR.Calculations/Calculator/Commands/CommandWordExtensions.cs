namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public static class CommandWordExtensions
    {
        public static bool IsSpecialCommandWord(this CommandWord commandWord)
        {
            return commandWord == CommandWord.SetYears
                   || commandWord == CommandWord.GetYears
                   || commandWord == CommandWord.CalculateYears
                   || commandWord == CommandWord.Interest
                   || commandWord == CommandWord.Start
                   || commandWord == CommandWord.GetRate
                   || commandWord == CommandWord.SetRate
                   || commandWord == CommandWord.GetRepaymentRate
                   || commandWord == CommandWord.SetRepaymentRate
                   || commandWord == CommandWord.CalculateRate
                   || commandWord == CommandWord.GetEnd
                   || commandWord == CommandWord.SetEnd
                   || commandWord == CommandWord.CalculateEnd
                   || commandWord == CommandWord.SetRatesPerAnnum
                   || commandWord == CommandWord.GetRatesPerAnnum;
        }
    }
}
