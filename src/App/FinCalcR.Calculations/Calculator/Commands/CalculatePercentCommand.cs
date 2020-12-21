using System;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculatePercentCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public CalculatePercentCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.PercentCalculation;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.CalculatePercent();
        }
    }
}
