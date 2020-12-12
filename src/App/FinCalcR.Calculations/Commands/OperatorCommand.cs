using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator;

namespace StEn.FinCalcR.Calculations.Commands
{
    public class OperatorCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public OperatorCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.Operator;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            var mathOperatorToken = (string)parameter[0];
            this.calculator.PressOperator(mathOperatorToken);
        }
    }
}
