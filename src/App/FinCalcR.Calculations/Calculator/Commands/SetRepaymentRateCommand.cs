namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetRepaymentRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetRepaymentRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetRepaymentRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.SetRepaymentRate();
        }
    }
}
