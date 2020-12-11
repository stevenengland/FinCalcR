using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StEn.FinCalcR.Calculations.Calculator.Events;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberOutput : IOutputText
    {
        private string textValue;

        public event EventHandler<OutputTextChangedEventArgs> TextChanged;

        public string TextValue
        {
            get => this.textValue;
            private set
            {
                this.textValue = value;
                this.TextChanged?.Invoke(this, new OutputTextChangedEventArgs(value));
            }
        }

        public bool IsTemporaryOverlay { get; private set; }

        public void SetOverlay(string overlayText)
        {
            this.TextValue = overlayText;
            this.IsTemporaryOverlay = true;
        }

        public void SetFormula(string formula)
        {
            this.SetResult(formula);
        }

        public void SetResult(string result, int precisionLimit = -1)
        {
            var numberParts = result.Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
            var wholeNumberPart = this.InsertGroupSeparator(numberParts[0]);
            var fractionalNumberPart = this.SetMaxArithmeticPrecision(numberParts.Length < 2 ? string.Empty : numberParts[1], precisionLimit);
            this.TextValue = wholeNumberPart
                             + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                             + fractionalNumberPart;
        }

        private string SetMaxArithmeticPrecision(string fractionalPart, int precisionLimit = -1)
        {
            if (precisionLimit > 0)
            {
                if (string.IsNullOrWhiteSpace(fractionalPart))
                {
                    fractionalPart = "0";
                }

                if (fractionalPart.Length > precisionLimit)
                {
                    var concatenatedSides = "0"
                                            + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                                            + fractionalPart;
                    if (double.TryParse(concatenatedSides, out var parsedNumber))
                    {
                        var numberToRound = Math.Round(parsedNumber, precisionLimit, MidpointRounding.AwayFromZero);
                        var sidesArray = numberToRound.ToString(CultureInfo.CurrentCulture).Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
                        fractionalPart = sidesArray.Length > 1 ? sidesArray[1] : "0";
                    }
                }

                for (var i = fractionalPart.Length; i < precisionLimit; i++)
                {
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                    fractionalPart += "0";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
                }
            }

            return fractionalPart;
        }

        private string InsertGroupSeparator(string wholeNumberPart)
        {
            var hasAlgebSign = false;
            if (wholeNumberPart.StartsWith("-"))
            {
                hasAlgebSign = true;
                wholeNumberPart = wholeNumberPart.Substring(1, wholeNumberPart.Length - 1);
            }

            var len = wholeNumberPart.Length;
            if (len < 4)
            {
                return hasAlgebSign ? "-" + wholeNumberPart : wholeNumberPart;
            }

            var result = string.Empty;
            var lastIndex = 1;
            for (var i = wholeNumberPart.Length - 3; i > 0; i -= 3)
            {
                lastIndex = i;
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                result = $"{Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator}{wholeNumberPart.Substring(i, 3)}" + result;
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
            }

            result = wholeNumberPart.Substring(0, lastIndex) + result;

            return hasAlgebSign ? "-" + result : result;
        }
    }
}
