using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public class TwoOperandsCalculator : ICalculationCommandReceiver
    {
        public TwoOperandsCalculator(IOutputText outputText, IInputText inputText)
        {
            this.OutputText = outputText;
            this.InputText = inputText;

            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.PreOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.PostOperatorNumber, 0, MemoryFieldNames.Categories.Standard));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.YearsNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.InterestNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.StartNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.RateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.EndNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<int>(MemoryFieldNames.RatesPerAnnumNumber, 12, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.NominalInterestRateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<double>(MemoryFieldNames.RepaymentRateNumber, 0, MemoryFieldNames.Categories.Special));
            this.MemoryFields.Add(new SimpleMemoryField<bool>(MemoryFieldNames.IsAdvance, false, MemoryFieldNames.Categories.Special));
        }

        public IMemoryFieldContainer MemoryFields { get; } = new MemoryFieldContainer();

        public IOutputText OutputText { get; }

        public IInputText InputText { get; }

        public MathOperator ActiveMathOperator { get; set; }

        public bool IsCalcCommandLock { get; set; }

        // TODO: MAKE PRIVATE OR REMOVE
        public CommandWord LastCommand { get; set; }

        public void PressDecimalSeparator()
        {
            this.IsCalcCommandLock = false;

            // Since the output text can be set independent from the input text a refresh of the output text might be needed.
            // I.e.: After pressing rates per annum a different text is shown to the user than the input text was before.
            if (this.OutputText.IsTemporaryOverlay)
            {
                this.OutputText.SetResult(this.InputText.EvaluatedResult);
            }

            // Special - if the last pressed operation was a special function a few things have to be reset.
            // I.e. if the last value is 3.0 from Start command the next input like digit 1 should create 0.1 instead of 3.1 or 31.0
            if (this.LastCommand.IsSpecialCommandWord()

                // Percent calculation -> is not considered a special function yet but also needs cleanup.
                || this.LastCommand == CommandWord.PercentCalculation)
            {
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.InputText.ResetInternalState();
                this.ActiveMathOperator = MathOperator.None;
            }

            this.InputText.DecimalSeparator();
        }
    }
}
