namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetNominalInterestRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetNominalInterestRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetNominalInterestRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.GetNominalInterestRate();
    }
}
