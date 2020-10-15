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
		public void EffectiveInterestRateIsCalculatedCorrectly(double m, double p, double expectedEffectiveInterestRate)
		{
			var effectiveInterestRate = FinancialCalculator.GetEffectiveInterestRate(p, m);
			Assert.Equal(Math.Round(effectiveInterestRate, 3), expectedEffectiveInterestRate);
		}

		[Theory]
		[InlineData(12, 4, 3.928)]
		public void YearlyNominalInterestRateIsCalculatedCorrectly(double m, double effectiveInterestRate, double expectedP)
		{
			var p = FinancialCalculator.GetYearlyNominalInterestRate(m, effectiveInterestRate);
			Assert.Equal(Math.Round(p, 3), expectedP);
		}

		[Theory]
		[InlineData(12, 150000, 4, 2, 750)]
		public void AnnuityIsCalculatedCorrectly(double m, double k0, double p, double repaymentRate, double expectedE)
		{
			var annuity = FinancialCalculator.GetAnnuity(k0, repaymentRate, p, m);
			Assert.Equal(annuity, expectedE);
		}

		[Theory]
		[InlineData(12, 150000, 4, 750, 2)]
		public void RepaymentRateIsCalculatedCorrectly(double m, double k0, double p, double e, double expectedRepaymentRate)
		{
			var repaymentRate = FinancialCalculator.GetRepaymentRate(k0, p, m, e);
			Assert.Equal(repaymentRate, expectedRepaymentRate);
		}

		[Theory]
		[InlineData(0, 10, 9.5689685146845171, 10, 12, 1998.63856714)] // With zero start
		[InlineData(150000, -750, 4, 10, 12, 113187.5488186329)] // From manual
		[InlineData(150000, -750, 4, 10.123, 12, 112636.9953862361)] // With decimal as years
		public void Kn_IsCalculatedCorrectly(double k0, double e, double p, double n, double m, double expectedKn)
		{
			var finalCapital = FinancialCalculator.Kn(k0, e, p, n, m);
			Assert.True(Math.Abs(finalCapital - expectedKn) < Tolerance);
		}

		[Theory]
		[InlineData(1000, 10, 10, 10, 12, 13.26389109)]
		[InlineData(0, 200000, 6.784974465, 25, 12, 255.41418004033653)] // From manual
		public void E_IsCalculatedCorrectly(double k0, double kn, double p, double n, double m, double expectedE)
		{
			var e = FinancialCalculator.E(kn, k0, p, n, m);
			Assert.True(Math.Abs(e - expectedE) < Tolerance);
		}

		[Theory]
		[InlineData(0, 1000, 2.471803524, 25, 12, -539.39058942391114)] // From manual
		[InlineData(100, 1000, 2.471803524, 25, 12, -22900.848016139695)] // Non zero regular payment
		public void K0_IsCalculatedCorrectly(double e, double kn, double p, double n, double m, double expectedK0)
		{
			var k0 = FinancialCalculator.K0(kn, e, p, n, m);
			Assert.True(Math.Abs(k0 - expectedK0) < Tolerance);
		}

		[Theory]
		[InlineData(-750, 0, 4, 150000, 12, 27.511057340185396)]
		public void N_IsCalculatedCorrectly(double e, double kn, double p, double k0, double m, double expectedN)
		{
			var n = FinancialCalculator.N(kn, k0, e, p, m);
			Assert.True(Math.Abs(n - expectedN) < Tolerance);
		}
	}
}
