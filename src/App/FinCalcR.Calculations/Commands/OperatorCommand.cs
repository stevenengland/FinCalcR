using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator;

namespace StEn.FinCalcR.Calculations.Commands
{
    public class OperatorCommand : BaseCommand
    {
        private ICalculationCommandReceiver calculator;

        public OperatorCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.Operator;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            var mathOperator = (MathOperator)parameter[0];
            this.calculator.PressOperator(mathOperator);
        }
    }
}
