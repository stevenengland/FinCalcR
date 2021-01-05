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
        [InlineData("1,", new[] { Ca.Nr34, Ca.OpM, Ca.Nr2, Ca.Calc, Ca.Nr1 })] // Calculate, Digit
        [InlineData("1,2", new[] { Ca.Nr1, Ca.Dec, Ca.Dec, Ca.Nr2 })] // Decimal twice before digit
        [InlineData("34,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr1, Ca.OpS, Ca.Nr34 })]
        public void ShowExpectedOutputText_WhenLastActionWasDigit(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("-1,", new[] { Ca.Nr1, Ca.Alg })]
        public void ShowExpectedOutputText_WhenLastActionWasAlgebSign(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("1,", new[] { Ca.Nr1, Ca.Dec })]
        public void ShowExpectedOutputText_WhenLastActionWasDecimalSeparator(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("1,", new[] { Ca.Nr1, Ca.OpA })]
        [InlineData("2,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr1, Ca.OpS })]
        public void ShowExpectedOutputText_WhenLastActionWasOperator(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("3,", new[] { Ca.Nr34, Ca.OpS, Ca.Nr1, Ca.Calc, Ca.ClearP, Ca.Nr2, Ca.OpA, Ca.Nr1, Ca.Calc })] // Calculation, Clear, Calculation
        [InlineData("3,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc, Ca.Calc })] // Calculation twice at the end
        public void ShowExpectedOutputText_WhenLastActionWasCalculate(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("0,", new[] { Ca.Nr1, Ca.ClearP })]
        public void ShowExpectedOutputText_WhenLastActionWasClear(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("3,", 3, new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc })]
        [InlineData("32,", 32, new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("68,", 68, new[] { Ca.Nr34, Ca.OpM, Ca.Nr2, Ca.Calc })]
        [InlineData("17,", 17, new[] { Ca.Nr34, Ca.OpD, Ca.Nr2, Ca.Calc })]
        public void ShowExpectedOutputText_WhenBasicArithmeticIsPerformed(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, Tolerance);
    }
}
