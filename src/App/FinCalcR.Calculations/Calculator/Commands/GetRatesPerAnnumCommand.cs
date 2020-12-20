using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetRatesPerAnnumCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetRatesPerAnnumCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetRatesPerAnnum;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressLoadMemoryValue(MemoryFieldNames.RatesPerAnnumNumber);
        }
    }
}
