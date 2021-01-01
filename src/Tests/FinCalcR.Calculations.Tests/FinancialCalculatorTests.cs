using System;
using Xunit;
using FinancialCalculation = StEn.FinCalcR.Calculations.FinancialCalculation;

namespace FinCalcR.Calculations.Tests
{
    public class FinancialCalculatorTests
    {
        [Theory]
        [InlineData(12, 4, 4.074)]
        public void EffectiveInterestRateIsCalculatedCorrectly(double m, double p, double expectedEffectiveInterestRate)
        {
            var effectiveInterestRate = FinancialCalculation.GetEffectiveInterestRate(p, m);
            Assert.Equal(Math.Round(effectiveInterestRate, 3), expectedEffectiveInterestRate);
        }

        [Theory]
        [InlineData(12, 4, 3.928)]
        public void YearlyNominalInterestRateIsCalculatedCorrectly(double m, double effectiveInterestRate, double expectedP)
        {
            var p = FinancialCalculation.GetYearlyNominalInterestRate(m, effectiveInterestRate);
            Assert.Equal(Math.Round(p, 3), expectedP);
        }

        [Theory]
        [InlineData(12, 150000, 4, 2, 750)]
        public void AnnuityIsCalculatedCorrectly(double m, double k0, double p, double repaymentRate, double expectedE)
        {
            var annuity = FinancialCalculation.GetAnnuity(k0, repaymentRate, p, m);
            Assert.Equal(annuity, expectedE);
        }

        [Theory]
        [InlineData(12, 150000, 4, 750, 2)]
        public void RepaymentRateIsCalculatedCorrectly(double m, double k0, double p, double e, double expectedRepaymentRate)
        {
            var repaymentRate = FinancialCalculation.GetRepaymentRate(k0, p, m, e);
            Assert.Equal(repaymentRate, expectedRepaymentRate);
        }

        [Theory]
        [MemberData(nameof(FinCalcR.Tests.Shared.TestData.FinancialCalculation.KnWanted), MemberType = typeof(FinCalcR.Tests.Shared.TestData.FinancialCalculation))]
        public void Kn_IsCalculatedCorrectly(double m, double n, double p, double k0, double e, double expectedKn)
        {
            const double localTolerance = 0.01;
            var finalCapital = FinancialCalculation.Kn(k0, e, p, n, m);
            Assert.True(Math.Abs(finalCapital - expectedKn) < localTolerance);
        }

        [Theory]
        [MemberData(nameof(FinCalcR.Tests.Shared.TestData.FinancialCalculation.K0Wanted), MemberType = typeof(FinCalcR.Tests.Shared.TestData.FinancialCalculation))]
        public void K0_IsCalculatedCorrectly(double m, double n, double p, double e, double kn, double expectedK0)
        {
            const double localTolerance = 0.01;
            var k0 = FinancialCalculation.K0(kn, e, p, n, m);
            Assert.True(Math.Abs(k0 - expectedK0) < localTolerance);
        }

        [Theory]
        [MemberData(nameof(FinCalcR.Tests.Shared.TestData.FinancialCalculation.EWanted), MemberType = typeof(FinCalcR.Tests.Shared.TestData.FinancialCalculation))]
        public void E_IsCalculatedCorrectly(bool advance, double m, double n, double p, double k0, double kn, double expectedE)
        {
            const double localTolerance = 0.01;
            var e = FinancialCalculation.E(kn, k0, p, n, m, advance);
            Assert.True(Math.Abs(e - expectedE) < localTolerance);
        }

        [Theory]
        [MemberData(nameof(FinCalcR.Tests.Shared.TestData.FinancialCalculation.NWanted), MemberType = typeof(FinCalcR.Tests.Shared.TestData.FinancialCalculation))]
        public void N_IsCalculatedCorrectly(double m, double p, double k0, double e, double kn, double expectedN)
        {
            const double localTolerance = 0.01;
            var n = FinancialCalculation.N(kn, k0, e, p, m);
            Assert.True(Math.Abs(n - expectedN) < localTolerance);
        }

        [Theory]
        [MemberData(nameof(FinCalcR.Tests.Shared.TestData.FinancialCalculation.PWanted), MemberType = typeof(FinCalcR.Tests.Shared.TestData.FinancialCalculation))]
        public void P_IsCalculatedCorrectly(double m, double n, double k0, double e, double kn, double expectedP)
        {
            kn *= -1;
            const double localTolerance = 0.001;
            var pNom = FinancialCalculation.P(kn, k0, e, n, m);
            var pEff = FinancialCalculation.GetEffectiveInterestRate(pNom, m);
            Assert.True(Math.Abs(pEff - expectedP) < localTolerance);
        }

#pragma warning disable SA1005 // Single line comments should begin with single space
#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
#pragma warning disable S4144 // Methods should not have identical implementations
        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, 126231.40)] //      +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -126010.48)] //        ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 126010.48)] //     +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, 114305.10)] //     -++
        [InlineData(12, 10, 0.995445737, -100, -1000, -126231.40)] //       +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -114124.22)] //       -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 114124.22)] //        --+
        [InlineData(12, 10, -1.004612831, -100, -1000, -114305.10)] //  ---
        public void Kn_IsCalculatedCorrectly_Permutation(double m, double n, double p, double k0, double e, double expectedKn)
        {
            const double localTolerance = 0.01;
            var finalCapital = FinancialCalculation.Kn(k0, e, p, n, m);
            Assert.True(Math.Abs(finalCapital - expectedKn) < localTolerance);
        }

        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, 126336.02)] //      +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -126115.10)] //        ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 126115.10)] //     +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, 114209.48)] //     -++
        [InlineData(12, 10, 0.995445737, -100, -1000, -126336.02)] //       +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -114028.60)] //       -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 114028.60)] //        --+
        [InlineData(12, 10, -1.004612831, -100, -1000, -114209.48)] //  ---
        public void KnAdvance_IsCalculatedCorrectly_Permutation(double m, double n, double p, double k0, double e, double expectedKn)
        {
            const double localTolerance = 0.01;
            var finalCapital = FinancialCalculation.Kn(k0, e, p, n, m, true);
            Assert.True(Math.Abs(finalCapital - expectedKn) < localTolerance);
        }

        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, -12322.85)] //  +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -10512.28)] // ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 10512.28)] //  +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, -13734.75)] // -++
        [InlineData(12, 10, 0.995445737, -100, -1000, 12322.85)] // +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -11523.30)] //    -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 11523.30)] // --+
        [InlineData(12, 10, -1.004612831, -100, -1000, 13734.75)] //    ---
        public void K0_IsCalculatedCorrectly_Permutation(double m, double n, double p, double e, double kn, double expectedK0)
        {
            const double localTolerance = 0.01;
            var k0 = FinancialCalculation.K0(kn, e, p, n, m);
            Assert.True(Math.Abs(k0 - expectedK0) < localTolerance);
        }

        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, -12332.32)] //  +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -10521.75)] // ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 10521.75)] //  +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, -13724.18)] // -++
        [InlineData(12, 10, 0.995445737, -100, -1000, 12332.32)] // +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -11512.73)] //    -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 11512.73)] // --+
        [InlineData(12, 10, -1.004612831, -100, -1000, 13724.18)] //    ---
        public void K0Advance_IsCalculatedCorrectly_Permutation(double m, double n, double p, double e, double kn, double expectedK0)
        {
            const double localTolerance = 0.01;
            var k0 = FinancialCalculation.K0(kn, e, p, n, m, true);
            Assert.True(Math.Abs(k0 - expectedK0) < localTolerance);
        }

        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, 8.80)]  //  +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -7.05)] // ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 7.05)] //  +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, 9.55)] //  -++
        [InlineData(12, 10, 0.995445737, -100, -1000, -8.80)] //    +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -7.96)] //    -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 7.96)] // --+
        [InlineData(12, 10, -1.004612831, -100, -1000, -9.55)] //   ---
        public void E_IsCalculatedCorrectly_Permutation(double m, double n, double p, double k0, double kn, double expectedE)
        {
            const double localTolerance = 0.01;
            var e = FinancialCalculation.E(kn, k0, p, n, m);
            Assert.True(Math.Abs(e - expectedE) < localTolerance);
        }

        [Theory]
        [InlineData(12, 10, 0.995445737, 100, 1000, 8.80)]  //  +++
        [InlineData(12, 10, 0.995445737, 100, -1000, -7.05)] // ++-
        [InlineData(12, 10, 0.995445737, -100, 1000, 7.05)] //  +-+
        [InlineData(12, 10, -1.004612831, 100, 1000, 9.56)] //  -++
        [InlineData(12, 10, 0.995445737, -100, -1000, -8.80)] //    +--
        [InlineData(12, 10, -1.004612831, 100, -1000, -7.97)] //    -+-
        [InlineData(12, 10, -1.004612831, -100, 1000, 7.97)] // --+
        [InlineData(12, 10, -1.004612831, -100, -1000, -9.56)] //   ---
        public void EAdvance_IsCalculatedCorrectly_Permutation(double m, double n, double p, double k0, double kn, double expectedE)
        {
            const double localTolerance = 0.01;
            var e = FinancialCalculation.E(kn, k0, p, n, m, true);
            Assert.True(Math.Abs(e - expectedE) < localTolerance);
        }

        [Theory]
        [InlineData(12, 5.3660387, 10000, 550, -1000000, 39.85)]    //  ++-
        [InlineData(12, 5.3660387, 10000, -550, 1000000, 42.89)]    //  +-+
        [InlineData(12, 5.3660387, -10000, 550, -1000000, 42.89)]   //  -+-
        [InlineData(12, 5.3660387, -10000, -550, 1000000, 39.85)] //    --+
        public void N_IsCalculatedCorrectly_PermutationOfRealNumbers(double m, double p, double k0, double e, double kn, double expectedN)
        {
            const double localTolerance = 0.01;
            var n = FinancialCalculation.N(kn, k0, e, p, m);
            Assert.True(Math.Abs(n - expectedN) < localTolerance);
        }

        [Theory]
        [InlineData(12, 5.3660387, 10000, 550, 1000000)]      //        +++
        [InlineData(12, 5.3660387, -10000, 550, 1000000)]     //        -++
        [InlineData(12, 5.3660387, 10000, -550, -1000000)]  //      +--
        [InlineData(12, 5.3660387, -10000, -550, -1000000)] //      ---
        public void N_IsCalculatedCorrectly_PermutationOfNan(double m, double p, double k0, double e, double kn)
        {
            var n = FinancialCalculation.N(kn, k0, e, p, m);
            Assert.True(double.IsNaN(n));
        }

        [Theory]
        [InlineData(12, 5.3660387, 10000, 550, -1000000, 39.78)]    //  ++-
        [InlineData(12, 5.3660387, 10000, -550, 1000000, 42.81)]    //  +-+
        [InlineData(12, 5.3660387, -10000, 550, -1000000, 42.81)]   //  -+-
        [InlineData(12, 5.3660387, -10000, -550, 1000000, 39.78)] //        --+
        public void NAdvance_IsCalculatedCorrectly_PermutationOfRealNumbers(double m, double p, double k0, double e, double kn, double expectedN)
        {
            const double localTolerance = 0.01;
            var n = FinancialCalculation.N(kn, k0, e, p, m, true);
            Assert.True(Math.Abs(n - expectedN) < localTolerance);
        }

        [Theory]
        [InlineData(12, 5.3660387, 10000, 550, 1000000)]      //        +++
        [InlineData(12, 5.3660387, -10000, 550, 1000000)]     //        -++
        [InlineData(12, 5.3660387, 10000, -550, -1000000)]  //      +--
        [InlineData(12, 5.3660387, -10000, -550, -1000000)] //      ---
        public void NAdvance_IsCalculatedCorrectly_PermutationOfNan(double m, double p, double k0, double e, double kn)
        {
            var n = FinancialCalculation.N(kn, k0, e, p, m);
            Assert.True(double.IsNaN(n));
        }

        //      [Theory]
        //      [InlineData(12, 5.3660387, 10000, 550, 1000000, 39.85)] //      Error   +++     39.85
        //      [InlineData(12, 5.3660387, 10000, 550, -1000000, 39.85)] //     39,85   ++-     Nan
        //      [InlineData(12, 5.3660387, 10000, -550, 1000000, 42.89)] //     42,89   +-+     nan
        //      [InlineData(12, 5.3660387, -10000, 550, 1000000, 39.85)] //     Errorb  -++     42.89
        //      [InlineData(12, 5.3660387, 10000, -550, -1000000, 39.85)] //        Errorb  +--     42.89
        //      [InlineData(12, 5.3660387, -10000, 550, -1000000, 42.89)] //        42,89   -+-     nan
        //      [InlineData(12, 5.3660387, -10000, -550, 1000000, 41.16)] //        41,16   --+     nan
        //      [InlineData(12, 5.3660387, -10000, -550, -1000000, 39.85)] //       Error   ---     39.85
        //      public void N_IsCalculatedCorrectly2(double m, double p, double k0, double e, double kn, double expectedN)
        //      {
        //          var localTolerance = 0.01;
        //          var n = FinancialCalculation.N(kn, k0, e, p, m);
        //          Assert.True(Math.Abs(n - expectedN) < localTolerance);
        //      }
    }
#pragma warning restore SA1005 // Single line comments should begin with single space
#pragma warning restore S125 // Sections of code should not be commented out
#pragma warning restore SA1025 // Code should not contain multiple whitespace in a row
#pragma warning restore S4144 // Methods should not have identical implementations
}
