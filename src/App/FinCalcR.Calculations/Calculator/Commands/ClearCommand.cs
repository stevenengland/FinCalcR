using System.Collections.Generic;

namespace StEn.FinCalcR.Calculations.Calculator.Commands
{
    public class ClearCommand : BaseCommand
    {
        private readonly ICalculationCommandReceiver calculator;

        public ClearCommand(ICalculationCommandReceiver calculator)
        {
            this.calculator = calculator;
            this.CommandWord = CommandWord.Clear;
        }

        public override CommandWord CommandWord { get; }

        public override void Execute(params object[] parameter)
        {
            var list = new List<string>();
            if (parameter?.Length > 0)
            {
                list = (List<string>)parameter[0];
            }

            this.calculator.PressClear(list);
        }
    }
}
