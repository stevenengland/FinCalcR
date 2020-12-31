namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetNominalInterestRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetNominalInterestRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetNominalInterestRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.SetNominalInterestRate();
    }
}
