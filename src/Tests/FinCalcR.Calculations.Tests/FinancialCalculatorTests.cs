using System;
using System.Runtime.InteropServices;
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
		[InlineData(12, 10, 9.5689685146845171, 0, 10, 1998.63856714)] // With zero start
		[InlineData(12, 10.123, 4, 150000, -750, 112636.9953862361)] // With decimal as years
		[InlineData(12, 10, 4, 150000, -750, 113187.5488186329)] // From manual
		[InlineData(12, 25, 2.471803524, -1000, 0, -1853.9440984093485)] // From manual / Book p.19
		[InlineData(12, 25, 5.841060678, 0, -100, -67628.896204469813)] // Book p. 16
		[InlineData(12, 10, 4.121255148, 100000, -510.10, 75301.50)] // Book p. 22
		[InlineData(12, 35, 5.3660387, 0, -550, -678177.09)] // Book p. 30
		[InlineData(12, 18, 3.928487739, 0, -184, -57655.85)] // Book p. 57
		[InlineData(12, 1, -22.10816415, -10000, -184, -9997.45)] // Book p. 58
		[InlineData(12, 17, 7.720836132, -9997.45, -184, -114205.70)] // Book p. 59
		[InlineData(12, 2, 1.981897562, 0, -2000, -48922.81)] // Book p. 62
		[InlineData(12, 45, 6.784974465, -48922.81, -282.67, -2027489.70)] // Book p. 64
		[InlineData(12, 45, 6.266827589, 0, -600, -1798658.87)] // Book p. 65 1
		[InlineData(12, 10, 6.266827589, 0, -600, -99764.53)] // Book p. 65 2
		[InlineData(12, 35, 6.266827589, -87764.53, -600, -1691684.55)] // Book p. 66
		[InlineData(12, 35, 6.266827589, 12000, 0, 106974.32)] // Book p. 67
		[InlineData(12, 10, 5.841060678, 170000, -1100, 125723.32)] // Book p. 82
		[InlineData(12, 25, 3.928487739, -300, 0, -799.75)] // Book p. 84 1
		[InlineData(12, 50, 3.928487739, -300, 0, -2132.01)] // Book p. 84 2
		[InlineData(12, 26, 4.409771281, -0.7, 0, -2.20)] // Book p. 97
		[InlineData(12, 15, 0.995445737, -2041, 0, -2369.54)] // Book p. 115
		[InlineData(12, 15, 1.981897562, -2041, 0, -2746.92)] // Book p. 116
		[InlineData(12, 25, 0, 0, -100, -30000)] // Book p. 122
		[InlineData(12, 25, 0, 100, -100, -29900)] // Same as Book p. 122 but with k0 > 0 -> The same result like from the calculator but it's a bug.
		[InlineData(12, 20, 3.928487739, 0, -150, -54576.26)] // Book p. 125
		[InlineData(12, 45, 2.276104576, -2905, 0, -8082.57)] // Book p. 128
		[InlineData(12, 45, 0.995445737, -1287, 0, -2013.91)] // Book p. 129
		[InlineData(12, 18, 3.928487739, -10000, -184, -77914.01)] // Book p. 142 1 // Error in the book -> expects 79914.01
		[InlineData(12, 18, 7.720836132, -10000, -184, -125640.18)] // Book p. 142 2
		[InlineData(12, 17, 7.720836132, -10000, -184, -114215.13)] // Book p. 143 1
		[InlineData(12, 1, -22.10816415, -114215.13, -184, -93369.56)] // Book p. 143 2
		public void Kn_IsCalculatedCorrectly(double m, double n, double p, double k0, double e, double expectedKn)
		{
			var localTolerance = 0.01;
			var finalCapital = FinancialCalculator.Kn(k0, e, p, n, m);
			Assert.True(Math.Abs(finalCapital - expectedKn) < localTolerance);
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
		[InlineData(12, 25, 2.471803524, 0, 1000,  -539.39)] // From manual // Book p. 19
		[InlineData(12, 25, 2.471803524, 100, 1000, -22900.85)] // Non zero regular payment
		[InlineData(12, 25, 6.784974465, 0, 200000, -36849.84)] // Book p. 18
		[InlineData(12, 35, 5.3660387, -550, 1000000, -49406.13)] // Book p. 31
		[InlineData(12, 5, 3.445078463, 1000, 0, -55044.38)] // Book p. 60
		[InlineData(12, 25, 5.841060678, -800, 0, 126059.52)] // Book p. 80
		[InlineData(12, 25, 5.841060678, 1000, 0, -157574.40)] // Book p. 81 -> Error in the book. Should be -1000 for the rate.
		[InlineData(12, 25, 5.841060678, -1000, 0, 157574.40)] // Book p. 81 -> Error in the book. Should be -157574.40 for the start.
		[InlineData(12, 10, 1.981897562, 0, 100000, -82034.83)] // Book p. 92
		[InlineData(12, 15, 2.256515397, 0, 2740, -1953.84)] // Book p. 118
		[InlineData(12, 15, 2.256515397, 0, 2360, -1682.87)] // Book p. 119
		[InlineData(12, 15, 2.256515397, 0, 2041, -1455.40)] // Book p. 120
		[InlineData(12, 18, 2.667152753, 8083, 0, -1385363.36)] // Book p. 130
		[InlineData(12, 18, 1.292317896, 2014, 0, -387946.76)] // Book p. 131
		public void K0_IsCalculatedCorrectly(double m, double n, double p, double e, double kn, double expectedK0)
		{
			var localTolerance = 0.01;
			var k0 = FinancialCalculator.K0(kn, e, p, n, m);
			Assert.True(Math.Abs(k0 - expectedK0) < localTolerance);
		}

		[Theory]
		[InlineData(12, 4, 150000, -750, 0, 27.51)] // From manual
		[InlineData(12, 4.121255148, 100000, -510.10, 0, 27.19)] // Book p. 23
		[InlineData(12, 5.3660387, -10000, -550, 1000000, 39.85)] // Book p. 32
		[InlineData(12, 5.003639805, 250000, -1250, 0, 35.96)] // Book p. 77
		[InlineData(12, 5.003639805, 200000, -1250, 0, 22.03)] // Book p. 78
		[InlineData(12, 5.003639805, 150000, -1250, 0, 13.90)] // Book p. 79
		public void N_IsCalculatedCorrectly(double m, double p, double k0, double e, double kn, double expectedN)
		{
			var localTolerance = 0.01;
			var n = FinancialCalculator.N(kn, k0, e, p, m);
			Assert.True(Math.Abs(n - expectedN) < localTolerance);
		}

#pragma warning disable SA1005 // Single line comments should begin with single space
#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
#pragma warning disable SA1512 // Single-line comments should not be followed by blank line
#pragma warning disable S4144 // Methods should not have identical implementations
		[Theory]
		[InlineData(12, 5.3660387, 10000, 550, -1000000, 39.85)]			//		39,85	++-		39,85
		[InlineData(12, 5.3660387, 10000, -550, 1000000, 42.89)]			//	n	42,89	+-+		nan
		[InlineData(12, 5.3660387, -10000, 550, -1000000, 42.89)]			//	n	42,89	-+-		nan
		[InlineData(12, 5.3660387, -10000, -550, 1000000, 39.85)]           //		39,85	--+		39,85
		public void N_IsCalculatedCorrectly_PermutationOfRealNumbers(double m, double p, double k0, double e, double kn, double expectedN)
		{
			var localTolerance = 0.01;
			var n = FinancialCalculator.N(kn, k0, e, p, m);
			Assert.True(Math.Abs(n - expectedN) < localTolerance);
		}

		[Theory]
		[InlineData(12, 5.3660387, 10000, 550, 1000000)]      //		Error	+++		nan
		[InlineData(12, 5.3660387, -10000, 550, 1000000)]     //	n	Errorb	-++		42.89
		[InlineData(12, 5.3660387, 10000, -550, -1000000)]	//	n	Errorb	+--		42.89
		[InlineData(12, 5.3660387, -10000, -550, -1000000)]	//		Error	---		nan
		public void N_IsCalculatedCorrectly_PermutationOfNan(double m, double p, double k0, double e, double kn)
		{
			var n = FinancialCalculator.N(kn, k0, e, p, m);
			Assert.True(double.IsNaN(n));
		}

		//		[Theory]
		//		[InlineData(12, 5.3660387, 10000, 550, 1000000, 39.85)] //		Error	+++		39.85
		//		[InlineData(12, 5.3660387, 10000, 550, -1000000, 39.85)] //		39,85	++-		Nan
		//		[InlineData(12, 5.3660387, 10000, -550, 1000000, 42.89)] //		42,89	+-+		nan
		//		[InlineData(12, 5.3660387, -10000, 550, 1000000, 39.85)] //		Errorb	-++		42.89
		//		[InlineData(12, 5.3660387, 10000, -550, -1000000, 39.85)] //		Errorb	+--		42.89
		//		[InlineData(12, 5.3660387, -10000, 550, -1000000, 42.89)] //		42,89	-+-		nan
		//		[InlineData(12, 5.3660387, -10000, -550, 1000000, 41.16)] //		41,16	--+		nan
		//		[InlineData(12, 5.3660387, -10000, -550, -1000000, 39.85)] //		Error	---		39.85
		//		public void N_IsCalculatedCorrectly2(double m, double p, double k0, double e, double kn, double expectedN)
		//		{
		//			var localTolerance = 0.01;
		//			var n = FinancialCalculator.N(kn, k0, e, p, m);
		//			Assert.True(Math.Abs(n - expectedN) < localTolerance);
		//		}

		[Theory]
		[InlineData(12, 10, 0.995445737, 100, 1000, -12322.85)] //	+++	-12322.850978599574
		[InlineData(12, 10, 0.995445737, 100, -1000, -10512.28)] //	++-	-10512.277069174643
		[InlineData(12, 10, 0.995445737, -100, 1000, 10512.28)] //	+-+	10512.277069174643
		[InlineData(12, 10, -1.004612831, 100, 1000, -13734.75)] //	-++	-13727.8035549473
		[InlineData(12, 10, 0.995445737, -100, -1000, 12322.85)] //	+--	12322.850978599574
		[InlineData(12, 10, -1.004612831, 100, -1000, -11523.30)] //	-+-	-11518.376865984397
		[InlineData(12, 10, -1.004612831, -100, 1000, 11523.30)] //	--+	11518.376865984397
		[InlineData(12, 10, -1.004612831, -100, -1000, 13734.75)] //	---	13727.8035549473
		public void K0_IsCalculatedCorrectly_Permutation(double m, double n, double p, double e, double kn, double expectedK0)
		{
			var localTolerance = 0.01;
			var k0 = FinancialCalculator.K0(kn, e, p, n, m);
			Assert.True(Math.Abs(k0 - expectedK0) < localTolerance);
		}
	}
#pragma warning restore SA1005 // Single line comments should begin with single space
#pragma warning restore S125 // Sections of code should not be commented out
#pragma warning restore SA1025 // Code should not contain multiple whitespace in a row
#pragma warning restore SA1512 // Single-line comments should not be followed by blank line
#pragma warning restore S4144 // Methods should not have identical implementations
}
