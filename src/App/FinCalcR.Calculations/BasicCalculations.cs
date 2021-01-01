using System;

namespace StEn.FinCalcR.Calculations
{
    public static class BasicCalculations
    {
        public static double Calculate(double value1, double value2, string mathOperator)
        {
            switch (mathOperator)
            {
                case "/":
                    return value1 / value2;
                case "*":
                    return value1 * value2;
                case "+":
                    return value1 + value2;
                case "-":
                    return value1 - value2;
                default:
                    throw new NotSupportedException();
            }
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
