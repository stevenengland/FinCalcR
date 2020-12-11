using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberInput : IInputText
    {
        private readonly string thousandSeparator;
        private readonly int maxArithmeticPrecision;

        private bool isDecimalSeparatorActive = false;
        private string wholeNumberPart = "0";
        private string fractionalNumberPart = string.Empty;

        public SingleNumberInput(string thousandSeparator, string decimalSeparator, int maxArithmeticPrecision)
        {
            this.thousandSeparator = thousandSeparator;
            this.maxArithmeticPrecision = maxArithmeticPrecision;

            this.BuildInputTextFromInternalState();
        }

        // TODO: Remove this Property
        public bool IsDecimalSeparatorActive
        {
            get { return this.isDecimalSeparatorActive; }
            set { this.isDecimalSeparatorActive = value; }
        }

        // TODO: Remove this Property
        public string WholeNumberPart
        {
            get { return this.wholeNumberPart; }
            set { this.wholeNumberPart = value; }
        }

        // TODO: Remove this Property
        public string FractionalNumberPart
        {
            get { return this.fractionalNumberPart; }
            set { this.fractionalNumberPart = value; }
        }

        /// <inheritdoc />
        /// The formula will only consist of a typed number in this implementation.
        public string CurrentInputFormula { get; private set; }

        /// <inheritdoc />
        /// The evaluated result will be the same as the <see cref="CurrentInputFormula"/> in this implementation.
        public string EvaluatedResult { get; private set; }

        public void Set(double number)
        {
            this.BuildInternalStateFromNumber(number);
            this.BuildInputTextFromInternalState();
        }

        public void DecimalSeparator()
        {
            this.isDecimalSeparatorActive = true;
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

        private void BuildInputTextFromInternalState()
        {
            this.CurrentInputFormula = this.wholeNumberPart
                                       + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                                       + this.fractionalNumberPart;
            this.EvaluatedResult = this.CurrentInputFormula; // In a single Number input scenario the evaluated result is also the current input formula.
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

        private string InsertThousandSeparator(string numberText)
        {
            var hasAlgebSign = false;
            if (numberText.StartsWith("-"))
            {
                hasAlgebSign = true;
                numberText = numberText.Substring(1, numberText.Length - 1);
            }

            var len = numberText.Length;
            if (len < 4)
            {
                return hasAlgebSign ? "-" + numberText : numberText;
            }

            var result = string.Empty;
            var lastIndex = 1;
            for (var i = numberText.Length - 3; i > 0; i -= 3)
            {
                lastIndex = i;
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                result = $"{this.thousandSeparator}{numberText.Substring(i, 3)}" + result;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }

            result = numberText.Substring(0, lastIndex) + result;

            return hasAlgebSign ? "-" + result : result;
        }
    }
}
