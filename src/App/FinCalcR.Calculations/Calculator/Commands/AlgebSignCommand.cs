namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class AlgebSignCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public AlgebSignCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.AlgebSign;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressAlgebSign();
        }
    }
}
