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
            this.calculator.IsCalcCommandLock = false;

            this.calculator.InputText.DecimalSeparator();

            // Since output text can be set independent from input text a refresh is needed although separator might not chance input text itself.
            // I.e.: After pressing rates per annum a different text is shown to the user than input text. The next command must refresh the display text.
            this.calculator.OutputText.Set(this.calculator.InputText.CurrentInputText);

            //// Special - if the last pressed operation was a special function this operation should not work with old values.
            if (this.PreviousCommandWord.IsSpecialCommandWord()

                // Percent calculation -> is not considered a special function yet but also needs cleanup.
               || this.PreviousCommandWord == CommandWord.PercentCalculation)
            {
                this.calculator.ResetMemoryFields(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.calculator.InputText.ResetInternalState();
                this.calculator.ActiveMathOperator = MathOperator.None;
            }
        }
    }
}
