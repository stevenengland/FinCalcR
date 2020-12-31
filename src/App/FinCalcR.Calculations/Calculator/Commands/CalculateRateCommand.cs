namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculateRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.CalculateRate();
    }
}
