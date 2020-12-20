using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
