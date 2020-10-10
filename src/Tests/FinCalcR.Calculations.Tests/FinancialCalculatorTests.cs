using System;
using StEn.FinCalcR.Calculations;
using Xunit;

namespace FinCalcR.Calculations.Tests
{
	public class FinancialCalculatorTests
	{
		private const double Tolerance = 0.00000001;

		[Theory]
		[InlineData(12, 4, 4.074)]
		public void EffectiveInterestRateIsCalculatedCorrectly(double ratesPerAnnum, double yearlyNominalInterest, double expectedEffectiveInterestRate)
		{
			var effectiveRate = FinancialCalculator.GetEffectiveInterestRate(ratesPerAnnum, yearlyNominalInterest);
			Assert.Equal(Math.Round(effectiveRate, 3), expectedEffectiveInterestRate);
		}

		[Theory]
		[InlineData(12, 4, 3.928)]
		public void YearlyNominalInterestRateIsCalculatedCorrectly(double ratesPerAnnum, double effectiveInterestRate, double expectedNominalInterestRate)
		{
			var effectiveRate = FinancialCalculator.GetYearlyNominalInterestRate(ratesPerAnnum, effectiveInterestRate);
			Assert.Equal(Math.Round(effectiveRate, 3), expectedNominalInterestRate);
		}

		[Theory]
		[InlineData(12, 150000, 4, 2, 750)]
		public void AnnuityIsCalculatedCorrectly(double ratesPerAnnum, double loan, double nominalInterestRate, double repaymentRate, double expectedAnnuity)
		{
			var annuity = FinancialCalculator.GetAnnuity(ratesPerAnnum, loan, nominalInterestRate, repaymentRate);
			Assert.Equal(annuity, expectedAnnuity);
		}

		[Theory]
		[InlineData(12, 150000, 4, 750, 2)]
		public void RepaymentRateIsCalculatedCorrectly(double ratesPerAnnum, double loan, double nominalInterestRate, double annuity, double expectedRepayment)
		{
			var repayment = FinancialCalculator.GetRepaymentRate(ratesPerAnnum, loan, nominalInterestRate, annuity);
			Assert.Equal(repayment, expectedRepayment);
		}

		[Theory]
		[InlineData(0, 10, 9.5689685146845171, 10, 12, 1998.63856714)]
		public void FinalCapitalIsCalculatedCorrectly(double initialCapital, double regularPayment, double nominalInterestRate, double paymentPeriod, double ratesPerAnnum, double expectedFinalCapital)
		{
			var finalCapital = FinancialCalculator.GetFinalCapital(initialCapital, regularPayment, nominalInterestRate, paymentPeriod, ratesPerAnnum);
			Assert.True(Math.Abs(finalCapital - expectedFinalCapital) < Tolerance);
		}
	}
}
