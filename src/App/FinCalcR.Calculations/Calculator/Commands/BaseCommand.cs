namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public abstract class BaseCommand : ICalculatorCommand
    {
        public abstract CommandWord CommandWord { get; }

        public CommandWord PreviousCommandWord { get; set; }

        public bool ShouldExecute(CommandWord commandWord) => this.CommandWord == commandWord;

        public abstract void Execute(params object[] parameter);
    }
}
