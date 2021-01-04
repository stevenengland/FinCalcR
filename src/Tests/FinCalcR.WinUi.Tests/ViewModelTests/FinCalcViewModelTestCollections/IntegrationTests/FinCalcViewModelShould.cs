using System.Globalization;
using System.Threading;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinCalcViewModelShould
    {
        private const double Tolerance = 0.00000001;

        public FinCalcViewModelShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Theory]
        [InlineData("1,2", new[] { Ca.Nr1, Ca.Dec, Ca.Dec, Ca.Nr2 })]
        [InlineData("3,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc, Ca.Calc })]
        public void ShowExpectedOutputText_WhenCertainActionsArePerformedTwice(string expectedOutputTextAfterAllOperations, Ca[] actions)
        {
            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);
        }

        [Theory]
        [InlineData("3,", 3, new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc })]
        [InlineData("32,", 32, new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("68,", 68, new[] { Ca.Nr34, Ca.OpM, Ca.Nr2, Ca.Calc })]
        [InlineData("17,", 17, new[] { Ca.Nr34, Ca.OpD, Ca.Nr2, Ca.Calc })]
        public void ShowExpectedOutputText_WhenBasicArithmeticIsPerformed(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions)
        {
            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, Tolerance);
        }
    }
}
