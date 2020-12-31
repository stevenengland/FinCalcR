namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetStartCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetStartCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetStart;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.SetStart();
    }
}
