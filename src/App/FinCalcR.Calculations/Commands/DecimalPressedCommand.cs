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

            // Since the output text can be set independent from the input text a refresh of the output text might be needed.
            // I.e.: After pressing rates per annum a different text is shown to the user than the input text was before.
            if (this.calculator.OutputText.IsTemporaryOverlay)
            {
                this.calculator.OutputText.Set(this.calculator.InputText.CurrentInputText);
            }

            // Special - if the last pressed operation was a special function a few things have to be reset.
            // I.e. if the last value is 3.0 from Start command the next input like digit 1 should create 0.1 instead of 3.1 or 31.0
            if (this.PreviousCommandWord.IsSpecialCommandWord()

                // Percent calculation -> is not considered a special function yet but also needs cleanup.
               || this.PreviousCommandWord == CommandWord.PercentCalculation)
            {
                this.calculator.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.calculator.InputText.ResetInternalState();
                this.calculator.ActiveMathOperator = MathOperator.None;
            }

            this.calculator.InputText.DecimalSeparator();
        }
    }
}
