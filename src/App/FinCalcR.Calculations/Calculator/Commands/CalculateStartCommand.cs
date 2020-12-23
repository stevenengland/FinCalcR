namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateStartCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculateStartCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateStart;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.CalculateStart();
        }
    }
}
