using System;
using System.ComponentModel.Design.Serialization;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using StEn.FinCalcR.Calculations.Messages;

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

			var q = 1 + (p / (100 * m));

			if (advance)
			{
				e *= q;
			}

			var initialSummand = k0 * Math.Pow(q, n * m);
			var regularSummand = e * ((Math.Pow(q, n * m) - 1) / (q - 1));
			var kn = initialSummand + regularSummand;
			return kn;
		}

		public static double K0(double kn, double e, double p, double n, double m, bool advance = false)
		{
			var q = 1 + (p / (100 * m));

			if (advance)
			{
				e *= q;
			}

			kn *= -1;
			var k0 = (kn - (e * ((Math.Pow(q, n * m) - 1) / (q - 1)))) / Math.Pow(q, n * m);
			return k0;
		}

		public static double E(double kn, double k0, double p, double n, double m, bool advance = false)
		{
			var q = 1 + (p / (100 * m));
			var e = (kn - (((-1) * k0) * Math.Pow(q, n * m))) / ((Math.Pow(q, n * m) - 1) / (q - 1));
			if (advance)
			{
				return e / q;
			}

			return e;
		}

		public static double N(double kn, double k0, double e, double p, double m, bool advance = false)
		{
			var q = 1 + (p / (100 * m));

			if (advance)
			{
				e *= q;
			}

			if ((k0 > 0 && e > 0) || (k0 < 0 && e < 0)
				|| (e > 0 && kn > 0) || (e < 0 && kn < 0)
				|| (k0 > 0 && kn > 0) || (k0 < 0 && kn < 0))
			{
				kn *= -1;
			}

			var n = Math.Log10(((100 * e * m) + (p * kn)) / ((100 * e * m) + (p * k0))) / (m * Math.Log10((p / (100 * m)) + 1));
			return n;
		}

		public static double P(double kn, double k0, double e, double n, double m, bool advance = false, int iterations = 50)
		{
			// Many thanks to Thomas (https://github.com/tbuder) for helping me to deal with this calculation/formula :)
			double p0 = 0;

			for (var i = 1; i <= iterations; i++)
			{
				p0++;
				var pN = p0;

				// Adjust x in Math.Pow(10, -x) to a lower value if more precision is needed (but be careful with the calculation time)
				while (Math.Abs(advance ? PolynomialAdvance(kn, k0, e, n, m, pN) : Polynomial(kn, k0, e, n, m, pN)) > 1 * Math.Pow(10, -8))
				{
					pN -= (advance ? PolynomialAdvance(kn, k0, e, n, m, pN) : Polynomial(kn, k0, e, n, m, pN)) / (advance ? DiffPolynomialAdvance(kn, k0, e, n, m, pN) : DiffPolynomial(kn, k0, e, n, m, pN));
				}

				if (advance)
				{
					e *= 1 + (pN / 100 * m);
				}

				if (Math.Abs(Kn(k0, e, pN, n, m) - kn) < 1 * Math.Pow(10, -1))
				{
					return pN;
				}
			}

			throw new CalculationException(ErrorMessages.Instance.CalculationOfPercentIsNotPossible(iterations));

			double Polynomial(double dKn, double dK0, double dE, double dN, double dM, double dP)
			{
				var q = 1 + (dP / (100 * dM));
				var result = (dK0 * Math.Pow(q, (dN * dM) + 1)) + ((dE - dK0) * Math.Pow(q, dN * dM)) - (dKn * q) + dKn - dE;
				return result;
			}

			double DiffPolynomial(double dKn, double dK0, double dE, double dN, double dM, double dP)
			{
				var q = 1 + (dP / (100 * dM));
				var result = (((dN * dM) + 1) * dK0 * Math.Pow(q, dN * dM)) + (dN * dM * (dE - dK0) * Math.Pow(q, (dN * dM) - 1)) - dKn;
				return result;
			}

			double PolynomialAdvance(double dKn, double dK0, double dE, double dN, double dM, double dP)
			{
				var q = 1 + (dP / (100 * dM));
				var result = ((dK0 + dE) * Math.Pow(q, (dN * dM) + 1)) - (dK0 * Math.Pow(q, dN * dM)) - ((dE - dKn) * q) + dKn;
				return result;
			}

			double DiffPolynomialAdvance(double dKn, double dK0, double dE, double dN, double dM, double dP)
			{
				var q = 1 + (dP / (100 * dM));
				var result = (((dN * dM) + 1) * (dK0 + dE) * Math.Pow(q, dN * dM)) - (dN * dM * dK0 * Math.Pow(q, (dN * dM) - 1)) - dKn - dE;
				return result;
			}
		}
	}
}
