using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class CalculateRepaymentRateCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public CalculateRepaymentRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.CalculateRepaymentRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.CalculateRepaymentRate();
        }
    }
}
