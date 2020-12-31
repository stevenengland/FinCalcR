using System;

namespace StEn.FinCalcR.Calculations
{
    public static class BasicCalculations
    {
        public static double Calculate(double value1, double value2, string mathOperator)
        {
            double result;
            switch (mathOperator)
            {
                case "/":
                    result = value1 / value2;
                    break;
                case "*":
                    result = value1 * value2;
                    break;
                case "+":
                    result = value1 + value2;
                    break;
                case "-":
                    result = value1 - value2;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        public static double GetPartValue(double baseValue, double rate) => baseValue * rate / 100;

        public static double AddPartValueToBaseValue(double baseValue, double rate)
        {
            var partValue = baseValue * rate / 100;
            return baseValue + partValue;
        }

        public static double SubPartValueFromBaseValue(double baseValue, double rate)
        {
            var partValue = baseValue * rate / 100;
            return baseValue - partValue;
        }
    }
}
