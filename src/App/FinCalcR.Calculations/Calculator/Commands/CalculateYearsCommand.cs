namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateYearsCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculateYearsCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateYears;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.CalculateYears();
    }
}
