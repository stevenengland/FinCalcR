﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Attributes;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Exceptions;
using StEn.FinCalcR.Calculations.Messages;
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
                case CommandWord.SetYears:
                case CommandWord.GetYears:
                case CommandWord.CalculateYears:
                case CommandWord.GetStart:
                case CommandWord.SetStart:
                case CommandWord.CalculateStart:
                case CommandWord.SetAdvance:
                case CommandWord.SetRate:
                case CommandWord.GetRate:
                case CommandWord.CalculateRate:
                case CommandWord.GetRepaymentRate:
                case CommandWord.SetRepaymentRate:
                case CommandWord.GetEnd:
                case CommandWord.SetEnd:
                case CommandWord.CalculateEnd:
                case CommandWord.PercentCalculation:
                    this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 2);
                    break;
                case CommandWord.SetRatesPerAnnum:
                case CommandWord.GetRatesPerAnnum:
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

            double result = 0;

            if (this.ActiveMathOperator != MathOperator.None)
            {
                var (isValidResult, calculatedResult) = CalculationProxy.CalculateAndCheckResult(
                    true,
                    new Func<double, double, string, double>(SimpleCalculator.Calculate),
                    this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                    this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value,
                    this.TranslateMathOperator(this.ActiveMathOperator));

                result = calculatedResult;
            }

            this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = result;
            this.InputText.SetInternalState(result);
            this.ActiveMathOperator = MathOperator.None;
            this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
            this.IsCalcCommandLock = true;
        }

        public void PressClear(IList<string> memoryFieldCategories)
        {
            this.MemoryFields.Reset(memoryFieldCategories);
            this.InputText.ResetInternalState();
            this.ActiveMathOperator = MathOperator.None;
            this.IsCalcCommandLock = false;
            this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
        }

        public void PressDigit(string digit)
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            if (this.LastCommand.IsSpecialCommandWord())
            {
                this.ActiveMathOperator = MathOperator.None;
            }

            if (this.IsCalcCommandLock)
            {
                this.InputText.ResetInternalState();
                this.IsCalcCommandLock = false;
            }

            this.InputText.Digit(digit);

            this.OutputText.SetResult(this.InputText.GetEvaluatedResult());
        }

        public void PressLoadMemoryValue(string memoryFieldId)
        {
            var memoryFieldDescriptor = this.MemoryFields.Get(memoryFieldId);
            if (memoryFieldDescriptor.Id == MemoryFieldNames.RatesPerAnnumNumber)
            {
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value;
                this.InputText.SetInternalState(this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value);
                this.ActiveMathOperator = MathOperator.None;
                this.OutputText.SetOverlay(this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value + " p.a.");
            }
            else if (memoryFieldDescriptor.Id == MemoryFieldNames.InterestNumber
                     || memoryFieldDescriptor.Id == MemoryFieldNames.NominalInterestRateNumber)
            {
                var memoryField = this.MemoryFields.Get<double>(memoryFieldId);
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = memoryField.Value;
                this.InputText.SetInternalState(memoryField.Value);
                this.ActiveMathOperator = MathOperator.None;
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 3);
            }
            else if (memoryFieldDescriptor.Id == MemoryFieldNames.YearsNumber
                     || memoryFieldDescriptor.Id == MemoryFieldNames.StartNumber
                     || memoryFieldDescriptor.Id == MemoryFieldNames.RateNumber
                     || memoryFieldDescriptor.Id == MemoryFieldNames.RepaymentRateNumber
                     || memoryFieldDescriptor.Id == MemoryFieldNames.EndNumber)
            {
                var memoryField = this.MemoryFields.Get<double>(memoryFieldId);
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
                this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = memoryField.Value;
                this.InputText.SetInternalState(memoryField.Value);
                this.ActiveMathOperator = MathOperator.None;
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 2);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void CalculateYears()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            var (isValidResult, calculatedResult) = CalculationProxy.CalculateAndCheckResult(
                true,
                new Func<double, double, double, double, double, bool, double>(FinancialCalculator.N),
                this.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value,
                this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value,
                this.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

            this.InputText.SetInternalState(calculatedResult);
            this.CommonSpecialFunctionWriteToMemoryOperations(this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber), 2);
        }

        public void SetYears()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            var tmpYearsNumber = this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value;
            this.CommonSpecialFunctionWriteToMemoryOperations(this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber), 2);
            if (this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value < 0)
            {
                this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value = tmpYearsNumber;
                throw new ValidationException(ErrorMessages.Instance.YearsMustNotBeNegative());
            }
        }

        public void SetRatesPerAnnum()
        {
            // Special - if the last pressed operation was a special function this current special function should not work with old values.
            if (this.LastCommand.IsSpecialCommandWord())
            {
                this.InputText.ResetInternalState();
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
            }

            // Write the value to the memory
            var backupRpaNumber = this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value;
            var tmpRpaNumberField = new SimpleMemoryField<double>("tmpRpaNumberField", default(double), "temp");
            this.CommonSpecialFunctionWriteToMemoryOperations(tmpRpaNumberField, 0, false);
            if (tmpRpaNumberField.Value < 1
                || tmpRpaNumberField.Value > 365
                || tmpRpaNumberField.Value != Math.Truncate(tmpRpaNumberField.Value))
            {
                this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value = backupRpaNumber;
                throw new ValidationException(ErrorMessages.Instance.RatesPerAnnumExceedsRange());
            }
            else
            {
                this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value = (int)tmpRpaNumberField.Value;
                this.OutputText.SetOverlay(this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value + " p.a.");
            }
        }

        public void SetEnd()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            this.CommonSpecialFunctionWriteToMemoryOperations(this.MemoryFields.Get<double>(MemoryFieldNames.EndNumber), 2);
        }

        public void CalculateEnd()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            var (isValidResult, calculatedResult) = CalculationProxy.CalculateAndCheckResult(
                                       true,
                                       new Func<double, double, double, double, double, bool, double>(
                                           FinancialCalculator.Kn),
                                       this.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value,
                                       this.MemoryFields.Get<double>(MemoryFieldNames.RateNumber).Value,
                                       this.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value,
                                       this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value,
                                       this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value,
                                       this.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

            calculatedResult *= -1;

            this.InputText.SetInternalState(calculatedResult);
            this.CommonSpecialFunctionWriteToMemoryOperations(this.MemoryFields.Get<double>(MemoryFieldNames.EndNumber), 2);
        }

        public void CalculatePercent()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            this.SetMemoryFieldValue(this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber));
            var calculation = double.NaN;
            switch (this.ActiveMathOperator)
            {
                case MathOperator.Multiply:
                    calculation = CalculationProxy.CalculateAndCheckResult(
                            true,
                            new Func<double, double, double>(SimpleCalculator.GetPartValue),
                            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                            this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value)
                        .calculatedResult;
                    break;
                case MathOperator.Add:
                    calculation = CalculationProxy.CalculateAndCheckResult(
                            true,
                            new Func<double, double, double>(SimpleCalculator.AddPartValueToBaseValue),
                            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                            this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value)
                        .calculatedResult;
                    break;
                case MathOperator.Subtract:
                    calculation = CalculationProxy.CalculateAndCheckResult(
                            true,
                            new Func<double, double, double>(SimpleCalculator.SubPartValueFromBaseValue),
                            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                            this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value)
                        .calculatedResult;
                    break;
                case MathOperator.Divide: // ToDo: function is not documented and calculates like below - but makes not much sense...
                    calculation = CalculationProxy.CalculateAndCheckResult(
                            true,
                            new Func<double, double, double>((baseValue, rate) => baseValue / rate * 100),
                            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value,
                            this.MemoryFields.Get<double>(MemoryFieldNames.PostOperatorNumber).Value)
                        .calculatedResult;
                    break;
                default:
                    throw new NotSupportedException();
            }

            this.MemoryFields.Reset(new List<string>() {MemoryFieldNames.Categories.Standard});
            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = calculation;
            this.ActiveMathOperator = MathOperator.None;
            this.IsCalcCommandLock = true;
            this.InputText.SetInternalState(calculation);
            this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), 2);
        }

        public void CalculateRepaymentRate()
        {
            throw new NotImplementedException();
        }

        public void CalculateRate()
        {
            this.HandleCommonTasksIfLastCommandWasSpecial();

            var (isValidResult, calculatedResult) = CalculationProxy.CalculateAndCheckResult(
                true,
                new Func<double, double, double, double, double, bool, double>(FinancialCalculator.E),
                this.MemoryFields.Get<double>(MemoryFieldNames.EndNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.StartNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.NominalInterestRateNumber).Value,
                this.MemoryFields.Get<double>(MemoryFieldNames.YearsNumber).Value,
                this.MemoryFields.Get<int>(MemoryFieldNames.RatesPerAnnumNumber).Value,
                this.MemoryFields.Get<bool>(MemoryFieldNames.IsAdvance).Value);

            calculatedResult *= -1;

            this.InputText.SetInternalState(calculatedResult);
            this.CommonSpecialFunctionWriteToMemoryOperations(this.MemoryFields.Get<double>(MemoryFieldNames.RateNumber), 2);
        }

        private void HandleCommonTasksIfLastCommandWasSpecial()
        {
            // Special - if the last pressed operation was a special function this current special function should not work with old values.
            if (this.LastCommand.IsSpecialCommandWord())
            {
                this.InputText.ResetInternalState();
                this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
            }
        }

        private void CommonSpecialFunctionWriteToMemoryOperations(IMemoryFieldValue<double> memoryField, int specialNumberDecimalCount, bool setDisplayText = true)
        {
            // If last input was an operator restore the firstNumber for upcoming operations
            if (this.LastCommand == CommandWord.Operator)
            {
                this.InputText.SetInternalState(this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value);
            }

            this.SetMemoryFieldValue(memoryField);
            this.MemoryFields.Reset(new List<string>() { MemoryFieldNames.Categories.Standard });
            this.MemoryFields.Get<double>(MemoryFieldNames.PreOperatorNumber).Value = memoryField.Value;
            this.InputText.SetInternalState(memoryField.Value); // So that the display text can be set.
            this.ActiveMathOperator = MathOperator.None;
            if (setDisplayText)
            {
                this.OutputText.SetResult(this.InputText.GetEvaluatedResult(), specialNumberDecimalCount);
            }
        }

        private void SetMemoryFieldValue<T>(IMemoryFieldValue<T> memoryField)
        {
            var value = double.Parse(this.InputText.GetEvaluatedResult());
            switch (memoryField)
            {
                case IMemoryField<int> intField:
                    intField.Value = (int)value;
                    break;
                case IMemoryField<double> doubleField:
                    doubleField.Value = value;
                    break;
                default:
                    throw new NotSupportedException();
            }

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
