namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetEffectiveInterestCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetEffectiveInterestCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetEffectiveInterest;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressLoadMemoryValue(MemoryFieldNames.EffectiveInterestNumber);
        }
    }
}
