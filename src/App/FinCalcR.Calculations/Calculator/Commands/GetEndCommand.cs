namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetEndCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetEndCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetEnd;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressLoadMemoryValue(MemoryFieldNames.EndNumber);
        }
    }
}
