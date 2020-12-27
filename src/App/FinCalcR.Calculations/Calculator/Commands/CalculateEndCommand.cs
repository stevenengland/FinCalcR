namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateEndCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculateEndCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateEnd;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.CalculateEnd();
        }
    }
}
