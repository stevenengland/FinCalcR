using System;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetAdvanceCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetAdvanceCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetAdvance;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            if (parameter == null || parameter.Length == 0)
            {
                throw new ArgumentException();
            }

            this.calculator.SetAdvance((bool)parameter[0]);
        }
    }
}
