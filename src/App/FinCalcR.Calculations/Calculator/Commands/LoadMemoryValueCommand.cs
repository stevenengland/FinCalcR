using System;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class LoadMemoryValueCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public LoadMemoryValueCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.LoadMemoryValue;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            if (parameter == null || parameter.Length == 0)
            {
                throw new ArgumentException($"{nameof(parameter)} in {nameof(this.GetType)}");
            }

            var memoryFieldId = (string)parameter[0];

            this.calculator.PressLoadMemoryValue(memoryFieldId);
        }
    }
}
