using System;
using System.Globalization;
using System.Threading;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberInput : IInputText
    {
        private readonly int maxArithmeticPrecision;

        private string currentInputFormula;
        private string wholeNumberPart = "0";
        private string fractionalNumberPart = string.Empty;
        private bool isDecimalSeparatorActive;

        public SingleNumberInput(int maxArithmeticPrecision)
        {
            this.maxArithmeticPrecision = maxArithmeticPrecision;

            this.BuildInputTextFromInternalState();
        }

        /// <inheritdoc />
        /// The formula will only consist of a typed number in this implementation.
        public string GetCurrentInputFormula()
        {
            this.BuildInputTextFromInternalState();
            return this.currentInputFormula;
        }

        /// <inheritdoc />
        /// The evaluated result will be the same as the <see cref="GetCurrentInputFormula"/> in this implementation.
        public string GetEvaluatedResult()
        {
            return this.GetCurrentInputFormula();
        }

        public void ResetInternalState(bool updateCurrentInputText = false)
        {
            this.fractionalNumberPart = string.Empty;
            this.wholeNumberPart = "0";
            this.isDecimalSeparatorActive = false;

            if (updateCurrentInputText)
            {
                this.BuildInputTextFromInternalState();
            }
        }

        public void SetInternalState(double number)
        {
            this.BuildInternalStateFromNumber(number);
            this.BuildInputTextFromInternalState(); // TODO REMOVE BECAUSE IT IS NOT NECCESSARY
        }

        public void DecimalSeparator()
        {
            this.isDecimalSeparatorActive = true;
        }

        public void AlgebSign()
        {
            if (this.wholeNumberPart.StartsWith("-"))
            {
                this.wholeNumberPart = this.wholeNumberPart.Substring(1);
            }
            else
            {
                this.wholeNumberPart = "-" + this.wholeNumberPart;
            }
        }

        public void Operator()
        {
            // Nothing to do in the case of an number-only-input.
        }

        public void Calculate()
        {
            // Nothing to do in the case of an number-only-input.
        }

        public void Digit(string digit)
        {
            if (this.isDecimalSeparatorActive)
            {
                if (this.fractionalNumberPart.Length < this.maxArithmeticPrecision)
                {
                    this.fractionalNumberPart += digit;
                }
            }
            else
            {
                if (this.wholeNumberPart.Length < 10)
                {
                    if (!this.wholeNumberPart.StartsWith("0") && !this.wholeNumberPart.StartsWith("-0"))
                    {
                        this.wholeNumberPart += digit;
                    }
                    else
                    {
                        if (this.wholeNumberPart == "-0")
                        {
                            this.wholeNumberPart = "-" + digit;
                        }
                        else
                        {
                            this.wholeNumberPart = digit;
                        }
                    }
                }
            }
        }

        private void BuildInputTextFromInternalState()
        {
            this.currentInputFormula = this.wholeNumberPart
                                       + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                                       + this.fractionalNumberPart;
        }

        private void BuildInternalStateFromNumber(double number)
        {
            this.ResetInternalState();

            var roundedResult = Math.Round(number, this.maxArithmeticPrecision);
            var s = roundedResult.ToString(CultureInfo.InvariantCulture);
            var parts = s.Split('.');
            this.wholeNumberPart = parts[0];
            this.fractionalNumberPart = parts.Length < 2 ? string.Empty : parts[1];
        }
    }
}
