namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetRepaymentRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetRepaymentRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetRepaymentRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.GetRepaymentRate();
    }
}
