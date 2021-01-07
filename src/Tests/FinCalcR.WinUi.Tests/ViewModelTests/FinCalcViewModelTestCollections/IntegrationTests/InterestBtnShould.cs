using System.Collections.Generic;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class InterestBtnShould : TestBase
    {
        [Theory]
        [InlineData("0,995", new[] { Ca.Nr1, Ca.SetInt, Ca.GetNom })]
        public void UpdateNominalInterestRate_WhenPressedShort(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Fact]
        public void DoesNotUpdateEffectiveInterestRate_WhenPressedLong()
        {
            var testSequences = new List<(string, Ca[])>
            {
                // Set interest of 1 and RPA of 2
                (null, new[] { Ca.Nr1, Ca.SetInt, Ca.Nr2, Ca.SetRpa }),

                // Nominal interest rate gets recalculated
                ("0,998", new[] { Ca.GetNom }),

                // Check that effective interest was not changed
                ("1,000", new[] { Ca.GetInt }),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testSequences);
        }
    }
}
