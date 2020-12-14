namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class DecimalSeparatorCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public DecimalSeparatorCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.DecimalSeparator;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressDecimalSeparator();
        }
    }
}
