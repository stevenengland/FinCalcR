using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations
{
    public static class CalculationProxy
    {
        /// <summary>
        /// Takes in a function that calculates a result. The result is than checked to be a finite number.
        /// </summary>
        /// <param name="throwIfResultIsNotValid">If true and the result is not a finite number the method throws instead of reporting false as a result.</param>
        /// <param name="calculationMethod">The method to invoke that returns a calculation result.</param>
        /// <param name="args">Arguments for the <paramref name="calculationMethod"/>.</param>
        /// <returns>Returns a tuple indicating whether the calculation was successful and the result itself.</returns>
        public static (bool isValidResult, double calculatedResult) CalculateAndCheckResult(bool throwIfResultIsNotValid, Delegate calculationMethod, params object[] args)
        {
            double calculatedResult = 0;
            bool isValidResult;

            try
            {
                calculatedResult = (double)calculationMethod.DynamicInvoke(args);
                if (!Validation.NumberValidations.IsValidNumber(calculatedResult))
                {
                    throw new NotFiniteNumberException();
                }

                isValidResult = true;
            }
            catch
            {
                isValidResult = false;
                if (throwIfResultIsNotValid)
                {
                    throw;
                }
            }

            return (isValidResult, calculatedResult);
        }
    }
}
