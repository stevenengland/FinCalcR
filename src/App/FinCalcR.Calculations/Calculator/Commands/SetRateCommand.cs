namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.SetRate();
    }
}
