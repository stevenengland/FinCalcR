using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator;

namespace StEn.FinCalcR.Calculations.Commands
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
