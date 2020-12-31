namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetStartCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetStartCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetStart;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter) => this.calculator.PressLoadMemoryValue(MemoryFieldNames.StartNumber);
    }
}
