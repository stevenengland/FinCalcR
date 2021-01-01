using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public static class NumberFormatting
    {
        public static string InsertGroupSeparator(string inputNumber)
        {
            if (!double.TryParse(inputNumber, NumberStyles.Number, Thread.CurrentThread.CurrentUICulture, out _))
            {
                throw new ArgumentException($"{nameof(inputNumber)} is cannot be parsed to a number.");
            }

            var hasAlgebSign = false;
            var numberParts = inputNumber.Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
            var wholeNumberPart = numberParts[0];
            var fractionalPart = numberParts.Length < 2 ? string.Empty : numberParts[1];

            // Keep the AlgebSign
            if (wholeNumberPart.StartsWith("-"))
            {
                hasAlgebSign = true;
                wholeNumberPart = wholeNumberPart.Substring(1, wholeNumberPart.Length - 1);
            }

            // Get only digits from whole number part (could have group characters)
            wholeNumberPart = new string(wholeNumberPart.Where(char.IsDigit).ToArray());

            var len = wholeNumberPart.Length;
            if (len < 4)
            {
                var groupLessResult = wholeNumberPart
                                      + (string.IsNullOrWhiteSpace(fractionalPart) ? string.Empty : Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator + fractionalPart);
                return hasAlgebSign ? "-" + groupLessResult : groupLessResult;
            }

            // Group after every third char
            var result = string.Empty;
            var lastIndex = 1;
            for (var i = wholeNumberPart.Length - 3; i > 0; i -= 3)
            {
                lastIndex = i;
                result = $"{Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator}{wholeNumberPart.Substring(i, 3)}" + result;
            }

            result = wholeNumberPart.Substring(0, lastIndex) + result
                     + (string.IsNullOrWhiteSpace(fractionalPart) ? string.Empty : Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator + fractionalPart);

            return hasAlgebSign ? "-" + result : result;
        }

        public static string ConvertToNumberFormat(string inputNumber, int precisionLimit)
        {
            if (precisionLimit < 0)
            {
                precisionLimit = 0;
            }

            double.TryParse(inputNumber, NumberStyles.Number, Thread.CurrentThread.CurrentUICulture, out var number);
            return number.ToString($"n{precisionLimit}", Thread.CurrentThread.CurrentUICulture);
        }

        public static string ConvertToNumberFormat(double inputNumber, int precisionLimit)
        {
            if (precisionLimit < 0)
            {
                precisionLimit = 0;
            }

            return inputNumber.ToString($"n{precisionLimit}", Thread.CurrentThread.CurrentUICulture);
        }

        public static string RoundToMaxArithmeticPrecision(double inputNumber, int precisionLimit = -1) => RoundToMaxArithmeticPrecision(inputNumber.ToString(Thread.CurrentThread.CurrentUICulture), precisionLimit);

        public static string RoundToMaxArithmeticPrecision(string inputNumber, int precisionLimit = -1)
        {
            const NumberStyles parsingStyles = NumberStyles.Number;
            var numberParts = inputNumber.Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
            var wholeNumberPart = numberParts[0];
            var fractionalPart = numberParts.Length < 2 ? string.Empty : numberParts[1];

            if (precisionLimit > 0)
            {
                if (string.IsNullOrWhiteSpace(fractionalPart))
                {
                    fractionalPart = "0";
                }

                if (fractionalPart.Length > precisionLimit)
                {
                    var concatenatedSides = wholeNumberPart
                                            + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                                            + fractionalPart;
                    if (double.TryParse(concatenatedSides, parsingStyles, Thread.CurrentThread.CurrentUICulture, out var parsedNumber))
                    {
                        var numberToRound = Math.Round(parsedNumber, precisionLimit, MidpointRounding.AwayFromZero);
                        var sidesArray = numberToRound.ToString(Thread.CurrentThread.CurrentUICulture).Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator.ToCharArray());
                        wholeNumberPart = sidesArray[0];
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

            return wholeNumberPart
                   + Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                   + fractionalPart;
        }
    }
}
