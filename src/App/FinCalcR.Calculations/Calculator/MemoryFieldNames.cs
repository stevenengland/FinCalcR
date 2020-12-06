using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public static class MemoryFieldNames
    {
        public static string YearsNumber => nameof(YearsNumber);

        public static string InterestNumber => nameof(InterestNumber);

        public static string StartNumber => nameof(StartNumber);

        public static string RateNumber => nameof(RateNumber);

        public static string EndNumber => nameof(EndNumber);

        public static string RatesPerAnnumNumber => nameof(RatesPerAnnumNumber);

        public static string NominalInterestRateNumber => nameof(NominalInterestRateNumber);

        public static string RepaymentRateNumber => nameof(RepaymentRateNumber);

        public static string PreOperatorNumber => nameof(PreOperatorNumber);

        public static string PostOperatorNumber => nameof(PostOperatorNumber);

        public static class Categories
        {
            public static string Special => nameof(Special);

            public static string Standard => nameof(Standard);
        }
    }
}
