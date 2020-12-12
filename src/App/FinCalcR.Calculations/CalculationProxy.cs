using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations
{
    public static class CalculationProxy
    {
        public static double CalculateAndCheckResult(bool notifyIfResultIsNotValid, Delegate method, params object[] args)
        {
            double calculatedResult = 0;
            try
            {
                calculatedResult = (double) method.DynamicInvoke(args);
                if (!Validation.NumberValidations.IsValidNumber(calculatedResult))
                {
                    throw new NotFiniteNumberException();
                }
            }
            catch
            {
                if (notifyIfResultIsNotValid)
                {
                    throw;
                }
            }

            return calculatedResult;
        }
	}
}
