using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations;
using Xunit;

namespace FinCalcR.Calculations.Tests
{
	public class InterestCalculatorTests
	{
		[Theory]
		[InlineData(12, 4, 4.074)]
		public void EffectiveInterestRateIsCalculatedCorrectly(double ratesPerAnnum, double yearlyNominalInterest, double expectedEffectiveRate)
		{
			var effectiveRate = InterestCalculator.GetEffectiveInterestRate(ratesPerAnnum, yearlyNominalInterest);
			Assert.Equal(Math.Round(effectiveRate, 3), expectedEffectiveRate);
		}
	}
}
