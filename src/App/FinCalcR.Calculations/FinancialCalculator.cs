using System;

namespace StEn.FinCalcR.Calculations
{
	public static class FinancialCalculator
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

		public static double GetAnnuity(double ratesPerAnnum, double loan, double nominalInterestRate, double repaymentRate)
		{
			var annuity = ((loan * nominalInterestRate / 100) + (loan * repaymentRate / 100)) / ratesPerAnnum;
			return annuity;
		}

		public static double GetRepayment(double ratesPerAnnum, double loan, double nominalInterestRate, double annuity)
		{
			var repayment = ((annuity * ratesPerAnnum) - (loan * nominalInterestRate / 100)) / loan * 100;
			return repayment;
		}

		public static double GetFinalCapital(double initialCapital, double regularPayment, double nominalInterestRate, double paymentPeriod, double ratesPerAnnum)
		{
			var bracketValue = 1 + (nominalInterestRate / (100 * ratesPerAnnum));
			var initialSummand = initialCapital * Math.Pow(bracketValue, paymentPeriod * ratesPerAnnum);
			var regularSummand = regularPayment * ((Math.Pow(bracketValue, paymentPeriod * ratesPerAnnum) - 1) / (bracketValue - 1));
			var finalCapital = initialSummand + regularSummand;
			return finalCapital;
		}
	}
}
