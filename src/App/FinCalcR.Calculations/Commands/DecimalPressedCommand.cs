using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator;

namespace StEn.FinCalcR.Calculations.Commands
{
    public class DecimalPressedCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public DecimalPressedCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.DecimalSeparator;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressDecimalSeparator();
        }
    }
}
