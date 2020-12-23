namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateEffectiveInterestRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculateEffectiveInterestRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateEffectiveInterest;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.CalculateEffectiveInterest();
        }
    }
}
