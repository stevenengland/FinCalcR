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
		[InlineData(0, 10, 9.5689685146845171, 10, 12, 1998.63856714)] // With zero start
		[InlineData(150000, -750, 4, 10, 12, 113187.5488186329)] // From manual
		[InlineData(150000, -750, 4, 10.123, 12, 112636.9953862361)] // With decimal as years
		public void FinalCapitalIsCalculatedCorrectly(double initialCapital, double regularPayment, double nominalInterestRate, double paymentPeriod, double ratesPerAnnum, double expectedFinalCapital)
		{
			var finalCapital = FinancialCalculator.GetFinalCapital(initialCapital, regularPayment, nominalInterestRate, paymentPeriod, ratesPerAnnum);
			Assert.True(Math.Abs(finalCapital - expectedFinalCapital) < Tolerance);
		}

		[Theory]
		[InlineData(1000, 10, 10, 10, 12, 13.26389109)]
		[InlineData(0, 200000, 6.784974465, 25, 12, 255.41418004033653)] // From manual
		public void RegularPaymentIsCalculatedCorrectly(double initialCapital, double finalCapital, double nominalInterestRate, double paymentPeriod, double ratesPerAnnum, double expectedRegularPayment)
		{
			var regularPayment = FinancialCalculator.GetRegularPayment(initialCapital, finalCapital, nominalInterestRate, paymentPeriod, ratesPerAnnum);
			Assert.True(Math.Abs(regularPayment - expectedRegularPayment) < Tolerance);
		}

		[Theory]
		[InlineData(0, 1000, 2.471803524, 25, 12, -539.39058942391114)] // From manual
		[InlineData(100, 1000, 2.471803524, 25, 12, -22900.848016139695)] // Non zero regular payment
		public void InitialCapitalIsCalculatedCorrectly(double E, double Kn, double p, double n, double m, double expectedK0)
		{
			var initialCapital = FinancialCalculator.GetInitialCapital(E, Kn, p, n, m);
			Assert.True(Math.Abs(initialCapital - expectedK0) < Tolerance);
		}

		[Theory]
		[InlineData(-750, 0, 4, 150000, 12, 27.511057340185396)]
		public void PaymentPeriodIsCalculatedCorrectly(double e, double kn, double p, double k0, double m, double expectedN)
		{
			var n = FinancialCalculator.N(e, kn, p, k0, m);
			Assert.True(Math.Abs(n - expectedN) < Tolerance);
		}
	}
}
