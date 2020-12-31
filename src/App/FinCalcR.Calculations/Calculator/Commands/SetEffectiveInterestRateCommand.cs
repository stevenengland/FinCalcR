namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetEffectiveInterestRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetEffectiveInterestRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetEffectiveInterest;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.SetEffectiveInterestRate();
    }
}
