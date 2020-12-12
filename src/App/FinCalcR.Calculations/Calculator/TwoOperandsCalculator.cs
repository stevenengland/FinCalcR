using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Attributes;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;
using StEn.FinCalcR.Calculations.Validation;
using StEn.FinCalcR.Common.Converter;
using StEn.FinCalcR.Common.Extensions;

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

        /// <inheritdoc />
        public bool IsCalcCommandLock { get; set; }

        // TODO: MAKE PRIVATE OR REMOVE
        public CommandWord LastCommand { get; set; }

        public void PressDecimalSeparator()
        {
            this.IsCalcCommandLock = false;

            this.HandleTemporaryOverlay();

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

        public void PressAlgebSign()
        {
            this.InputText.AlgebSign();

            switch (this.LastCommand)
            {
                case CommandWord.Interest:
                    this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 3);
                    break;
                case CommandWord.Years:
                case CommandWord.Start:
                case CommandWord.Rate:
                case CommandWord.End:
                case CommandWord.PercentCalculation:
                    this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 2);
                    break;
                case CommandWord.RatesPerAnnum:
                    this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
                    break;
                default:
                    this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
                    break;
            }
        }

        public void PressOperator(string mathOperatorToken)
        {
            var mathOperator = EnumConverter.ParseToEnum<MathOperator, TokenAttribute>(mathOperatorToken);

            this.HandleTemporaryOverlay();

            if (this.LastCommand == CommandWord.PercentCalculation)
            {
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
            }

            if (this.ActiveMathOperator != MathOperator.None)
            {
                this.PressCalculate();
                this.ActiveMathOperator = mathOperator;
            }
            else
            {
                this.ActiveMathOperator = mathOperator;
                this.SetMemoryFieldValue(this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber));
            }
        }

        public void PressCalculate()
        {
            if (this.IsCalcCommandLock)
            {
                return;
            }

            this.SetMemoryFieldValue(this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber));

            double calculatedResult = 0;

            if (this.ActiveMathOperator != MathOperator.None)
            {
                calculatedResult = CalculationProxy.CalculateAndCheckResult(
                    true,
                    new Func<double, double, string, double>(SimpleCalculator.Calculate),
                    this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                    this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value,
                    this.TranslateMathOperator(this.ActiveMathOperator));
            }

            if (NumberValidations.IsValidNumber(calculatedResult))
            {
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = calculatedResult;
                this.InputText.SetInternalState(calculatedResult);
                this.ActiveMathOperator = MathOperator.None;
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
                this.IsCalcCommandLock = true;
            }
        }

        public void PressClear(IList<string> memoryFieldCategories)
        {
            this.MemoryFields.Reset(memoryFieldCategories);
            this.InputText.ResetInternalState();
            this.ActiveMathOperator = MathOperator.None;
            this.IsCalcCommandLock = false;
            this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
        }

        private void SetMemoryFieldValue(IMemoryFieldValue<double> memoryField)
        {
            var value = double.Parse(this.InputText.GetEvaluatedResult());
            memoryField.Value = value;

            this.InputText.ResetInternalState();
        }

        private void HandleTemporaryOverlay()
        {
            // Since the output text can be set independent from the input text a refresh of the output text might be needed.
            // I.e.: After pressing rates per annum a different text is shown to the user than the input text was before.
            if (this.OutputText.IsTemporaryOverlay)
            {
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
            }
        }

        // TODO: REMOVE
        private string TranslateMathOperator(MathOperator activeMathOperator)
        {
            // TODO: Remove whole function as soon as VMv2 is finished so the old VM does not rely on SimpleCalculator anymore.
            switch (activeMathOperator)
            {
                case MathOperator.None:
                    return string.Empty;
                case MathOperator.Add:
                    return "+";
                case MathOperator.Subtract:
                    return "-";
                case MathOperator.Divide:
                    return "/";
                case MathOperator.Multiply:
                    return "*";
                default:
                    throw new ArgumentOutOfRangeException(nameof(activeMathOperator), activeMathOperator, null);
            }
        }
    }
}
