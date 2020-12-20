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
                   || commandWord == CommandWord.Rate
                   || commandWord == CommandWord.End
                   || commandWord == CommandWord.RatesPerAnnum;
        }
    }
}
