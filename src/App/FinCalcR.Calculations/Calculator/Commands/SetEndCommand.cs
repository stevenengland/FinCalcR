using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetEndCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetEndCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetEnd;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.SetEnd();
        }
    }
}
