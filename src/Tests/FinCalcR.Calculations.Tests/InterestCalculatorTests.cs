using System;
using StEn.FinCalcR.Calculations;
using Xunit;

namespace FinCalcR.Calculations.Tests
{
	public class InterestCalculatorTests
	{
		[Theory]
		[InlineData(12, 4, 4.074)]
		public void EffectiveInterestRateIsCalculatedCorrectly(double ratesPerAnnum, double yearlyNominalInterest, double expectedEffectiveInterestRate)
		{
			var effectiveRate = InterestCalculator.GetEffectiveInterestRate(ratesPerAnnum, yearlyNominalInterest);
			Assert.Equal(Math.Round(effectiveRate, 3), expectedEffectiveInterestRate);
		}

		[Theory]
		[InlineData(12, 4, 3.928)]
		public void YearlyNominalInterestRateIsCalculatedCorrectly(double ratesPerAnnum, double effectiveInterestRate, double expectedNominalInterestRate)
		{
			var effectiveRate = InterestCalculator.GetYearlyNominalInterestRate(ratesPerAnnum, effectiveInterestRate);
			Assert.Equal(Math.Round(effectiveRate, 3), expectedNominalInterestRate);
		}
	}
}
