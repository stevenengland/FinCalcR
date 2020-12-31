namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetYearsCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetYearsCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetYears;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.PressLoadMemoryValue(MemoryFieldNames.YearsNumber);
    }
}
