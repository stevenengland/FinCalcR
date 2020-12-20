using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class SetRatesPerAnnumCommand: BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public SetRatesPerAnnumCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.SetRatesPerAnnum;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.SetRatesPerAnnum();
        }
    }
}
