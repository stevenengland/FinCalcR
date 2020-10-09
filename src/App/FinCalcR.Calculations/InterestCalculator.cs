using System;

namespace StEn.FinCalcR.Calculations
{
	public static class InterestCalculator
	{
		public static double GetEffectiveInterestRate(double ratesPerAnnum, double yearlyNominalInterest)
		{
			var decimalNominalInterest = yearlyNominalInterest / 100;
			return (Math.Pow(1 + (decimalNominalInterest / ratesPerAnnum), ratesPerAnnum) - 1) * 100;
		}

		public static double GetYearlyNominalInterestRate(double ratesPerAnnum, double effectiveInterestRate)
		{
			var decimalEffectiveInterestRate = effectiveInterestRate / 100;
			return ((Math.Pow(decimalEffectiveInterestRate + 1, 1 / ratesPerAnnum) - 1) * ratesPerAnnum) * 100;
		}
	}
}
