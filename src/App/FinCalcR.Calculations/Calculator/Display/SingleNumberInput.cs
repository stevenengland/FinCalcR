using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberInput : IInputText
    {
        private readonly string thousandSeparator;
        private readonly string decimalSeparator;
        private readonly int maxArithmeticPrecision;

        private bool isDecimalSeparatorActive = false;
        private string wholeNumberPart = "0";
        private string fractionalNumberPart = string.Empty;

        public SingleNumberInput(string thousandSeparator, string decimalSeparator, int maxArithmeticPrecision)
        {
            this.thousandSeparator = thousandSeparator;
            this.decimalSeparator = decimalSeparator;
            this.maxArithmeticPrecision = maxArithmeticPrecision;
        }

        public string CurrentInputText { get; private set; }

        public void Set(double number, int precisionLimit = 0)
        {
            this.BuildInputText(precisionLimit);
        }

        public void DecimalSeparator()
        {
            this.isDecimalSeparatorActive = !this.isDecimalSeparatorActive;
        }

        public void ResetInternalState(bool updateCurrentInputText = false)
        {
            this.fractionalNumberPart = string.Empty;
            this.wholeNumberPart = "0";
            this.isDecimalSeparatorActive = false;

            if (updateCurrentInputText)
            {
                this.BuildInputText();
            }
        }

        private void BuildInputText(int precisionLimit = 0)
        {
            var formattedWholeNumberPart = this.InsertThousandSeparator(this.wholeNumberPart);
            var formattedFractionalNumberPart = this.fractionalNumberPart;
            if (precisionLimit > 0)
            {
                if (string.IsNullOrWhiteSpace(formattedFractionalNumberPart))
                {
                    formattedFractionalNumberPart = "0";
                }

                if (formattedFractionalNumberPart.Length > precisionLimit)
                {
                    var concatenatedSides = formattedWholeNumberPart + this.decimalSeparator + formattedFractionalNumberPart;
                    if (double.TryParse(concatenatedSides, out var parsedNumber))
                    {
                        var numberToRound = Math.Round(parsedNumber, precisionLimit, MidpointRounding.AwayFromZero);
                        var sidesArray = numberToRound.ToString(CultureInfo.CurrentCulture).Split(this.decimalSeparator.ToCharArray());
                        formattedWholeNumberPart = this.InsertThousandSeparator(sidesArray[0]);
                        formattedFractionalNumberPart = sidesArray.Length > 1 ? sidesArray[1] : "0";
                    }
                }

                for (var i = formattedFractionalNumberPart.Length; i < precisionLimit; i++)
                {
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                    formattedFractionalNumberPart += "0";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
                }
            }

            this.CurrentInputText = formattedWholeNumberPart + this.decimalSeparator + formattedFractionalNumberPart;
        }

        private void BuildSidesFromNumber(double number)
        {
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
