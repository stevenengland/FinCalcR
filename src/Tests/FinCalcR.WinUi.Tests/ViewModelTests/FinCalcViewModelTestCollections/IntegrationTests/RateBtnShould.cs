using System.Collections.Generic;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class RateBtnShould : TestBase
    {
        [Fact]
        public void UpdateRepaymentRate_WhenPressedLong()
        {
            var testSequences = new List<(string, Ca[])>
            {
                // Set dependent numbers so that repayment rate can be calculated and get repayment rate from memory.
                ("0,74", new[] { Ca.Nr1, Ca.Nr0, Ca.SetYears,Ca.Nr2, Ca.SetInt, Ca.Nr1, Ca.Nr5, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.Nr34, Ca.Nr0, Ca.Alg, Ca.SetRate, Ca.GetRep }),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testSequences);
        }

        [Fact]
        public void NotUpdateRepayment_WhenLongPressed()
        {
            var testSequences = new List<(string, Ca[])>
            {
                // Set dependent numbers so that repayment rate can be calculated and get repayment rate from memory.
                ("0,74", new[] { Ca.Nr1, Ca.Nr0, Ca.SetYears,Ca.Nr2, Ca.SetInt, Ca.Nr1, Ca.Nr5, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.Nr34, Ca.Nr0, Ca.Alg, Ca.SetRate, Ca.GetRep }),

                // Change a dependent value and see that rate did not change.
                ("-340,00", new[] { Ca.Nr1, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.GetRate }),

                // See that repayment rate changed.
                ("2,10", new[] { Ca.GetRep }),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testSequences);
        }
    }
}
