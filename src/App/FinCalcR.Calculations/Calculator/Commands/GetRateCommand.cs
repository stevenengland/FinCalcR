using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class GetRateCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public GetRateCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.GetRate;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            this.calculator.PressLoadMemoryValue(MemoryFieldNames.RateNumber);
        }
    }
}
