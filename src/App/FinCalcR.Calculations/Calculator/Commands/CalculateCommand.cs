namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public CalculateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.Calculate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressCalculate();
        }
    }
}
