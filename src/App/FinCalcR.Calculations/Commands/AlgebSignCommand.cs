using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator;

namespace StEn.FinCalcR.Calculations.Commands
{
    public class AlgebSignCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public AlgebSignCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.AlgebSign;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressAlgebSign();
        }
    }
}
