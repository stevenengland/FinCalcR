namespace StEn.FinCalcR.Calculations.Validation
{
    public static class NumberValidations
    {
        public static bool IsValidNumber(double number)
        {
            return !double.IsNaN(number) && !double.IsInfinity(number);
        }
    }
}
