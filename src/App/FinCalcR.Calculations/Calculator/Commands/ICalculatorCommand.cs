namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public interface ICalculatorCommand
    {
        CommandWord CommandWord { get; }

        CommandWord PreviousCommandWord { get; set; }

        bool ShouldExecute(CommandWord commandWord);

        void Execute(params object[] parameter);
    }
}
