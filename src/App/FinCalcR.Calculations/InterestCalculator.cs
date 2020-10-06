using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations
{
	public static class InterestCalculator
	{
		public static double GetEffectiveInterestRate(double ratesPerAnnum, double yearlyNominalInterest)
		{
			var decimalNominalInterest = yearlyNominalInterest / 100;
			return (Math.Pow(1 + (decimalNominalInterest / ratesPerAnnum), ratesPerAnnum) - 1) * 100;
		}
	}
}
