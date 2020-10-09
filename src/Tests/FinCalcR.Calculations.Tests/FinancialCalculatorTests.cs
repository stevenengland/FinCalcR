using System;
using StEn.FinCalcR.Calculations;
using Xunit;

namespace FinCalcR.Calculations.Tests
{
	public class FinancialCalculatorTests
	{
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
		public void RepaymentIsCalculatedCorrectly(double ratesPerAnnum, double loan, double nominalInterestRate, double annuity, double expectedRepayment)
		{
			var repayment = FinancialCalculator.GetRepayment(ratesPerAnnum, loan, nominalInterestRate, annuity);
			Assert.Equal(repayment, expectedRepayment);
		}
	}
}
