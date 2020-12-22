using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class DigitCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public DigitCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.Digit;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            if (parameter == null || parameter.Length == 0)
            {
                throw new ArgumentException();
            }

            var digit = parameter[0].ToString();

            this.calculator.PressDigit(digit);
        }
    }
}
