using System;
using System.Net.Http.Headers;

namespace StEn.FinCalcR.Calculations
{
	public static class FinancialCalculator
	{
		public static double GetEffectiveInterestRate(double p, double m)
		{
			var decimalNominalInterest = p / 100;
			return (Math.Pow(1 + (decimalNominalInterest / m), m) - 1) * 100;
		}

		public static double GetYearlyNominalInterestRate(double m, double effectiveInterestRate)
		{
			var decimalEffectiveInterestRate = effectiveInterestRate / 100;
			return ((Math.Pow(decimalEffectiveInterestRate + 1, 1 / m) - 1) * m) * 100;
		}

		public static double GetAnnuity(double k0, double e, double p, double m)
		{
			var annuity = ((k0 * p / 100) + (k0 * e / 100)) / m;
			return annuity;
		}

		public static double GetRepaymentRate(double k0, double p, double m, double annuity)
		{
			var repaymentRate = ((annuity * m) - (k0 * p / 100)) / k0 * 100;
			return repaymentRate;
		}

		public static double Kn(double k0, double e, double p, double n, double m, bool advance = false)
		{
			if (Math.Abs(p) < 0.00000001)
			{
				return k0 + (m * n * e); // Same as the calculator does but it is not correct. k0 must be multiplied by -1 in advance.
			}

			var bracketValue = 1 + (p / (100 * m));

			if (advance)
			{
				e *= bracketValue;
			}

			var initialSummand = k0 * Math.Pow(bracketValue, n * m);
			var regularSummand = e * ((Math.Pow(bracketValue, n * m) - 1) / (bracketValue - 1));
			var kn = initialSummand + regularSummand;
			return kn;
		}

		public static double E(double kn, double k0, double p, double n, double m)
		{
			var bracketValue = 1 + (p / (100 * m));
			var e = (kn - (((-1) * k0) * Math.Pow(bracketValue, n * m))) / ((Math.Pow(bracketValue, n * m) - 1) / (bracketValue - 1));
			return e;
		}

		public static double K0(double kn, double e, double p, double n, double m)
		{
			kn *= -1;
			var bracketValue = 1 + (p / (100 * m));
			var k0 = (kn - (e * ((Math.Pow(bracketValue, n * m) - 1) / (bracketValue - 1)))) / Math.Pow(bracketValue, n * m);
			return k0;
		}

		public static double N(double kn, double k0, double e, double p, double m)
		{
			if ((k0 > 0 && e > 0) || (k0 < 0 && e < 0)
				|| (e > 0 && kn > 0) || (e < 0 && kn < 0)
				|| (k0 > 0 && kn > 0) || (k0 < 0 && kn < 0))
			{
				kn *= -1;
			}

			return Math.Log10(((100 * e * m) + (p * kn)) / ((100 * e * m) + (p * k0))) / (m * Math.Log10((p / (100 * m)) + 1));
		}
	}
}
