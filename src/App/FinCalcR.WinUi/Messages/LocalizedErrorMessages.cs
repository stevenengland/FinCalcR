using StEn.FinCalcR.Calculations.Messages;
using StEn.FinCalcR.Common.LanguageResources;

namespace StEn.FinCalcR.WinUi.Messages
{
    public class LocalizedErrorMessages : ErrorMessages
    {
        public override string CalculationOfPercentIsNotPossible(int iterations)
        {
            return string.Format(Resources.EXC_CALCULATION_OF_P_IS_NOT_POSSIBLE, nameof(iterations));
        }

        public override string YearsMustNotBeNegative() => Resources.EXC_YEARS_EXCEEDED_LIMIT;

        public override string RatesPerAnnumExceedsRange() => Resources.EXC_RATES_PER_ANNUM_OUT_OF_RANGE;

        public override string EffectiveInterestExceedsRange() => Resources.EXC_EFFINTEREST_EXCEEDS_LIMIT;

        public override string NominalInterestExceedsRange() => Resources.EXC_NOMINTEREST_EXCEEDS_LIMIT;
    }
}
