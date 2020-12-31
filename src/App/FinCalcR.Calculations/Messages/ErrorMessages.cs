namespace StEn.FinCalcR.Calculations.Messages
{
    public class ErrorMessages
    {
        protected ErrorMessages()
        {
        }

        public static ErrorMessages Instance { get; set; } = new ErrorMessages();

        public virtual string CalculationOfPercentIsNotPossible(int iterations) => $"Calculation of p was not possible. Try to increase {nameof(iterations)}.";

        public virtual string YearsMustNotBeNegative() => $"Years must not be negative!";

        public virtual string RatesPerAnnumExceedsRange() => $"The rates per annum must be between 1 and 365 and must be an integer.";

        public virtual string EffectiveInterestExceedsRange() => $"The value for the effective interest must be >= -100.";

        public virtual string NominalInterestExceedsRange() => $"The value for the nominal interest must be >= -100.";
    }
}
