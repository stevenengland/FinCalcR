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
        [InlineData("0,", new[] { Ca.OpA })] // First and last action is operator -> Output is untouched
        [InlineData("1,", new[] { Ca.Nr1, Ca.OpA })]
        [InlineData("2,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr1, Ca.OpS })]
        public void ShowExpectedOutputText_WhenLastActionWasOperator(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("-2,", new[] { Ca.OpS, Ca.Nr2, Ca.Calc })] // First action was operator so calculate so the first number is zero.
        [InlineData("3,", new[] { Ca.Nr34, Ca.OpS, Ca.Nr1, Ca.Calc, Ca.ClearP, Ca.Nr2, Ca.OpA, Ca.Nr1, Ca.Calc })] // Calculation, Clear, Calculation
        [InlineData("3,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc, Ca.Calc })] // Calculation twice at the end
        [InlineData("0,", new[] { Ca.Nr1, Ca.OpD, Ca.Nr0, Ca.Calc })] // Calculation that throws
        [InlineData("4,", new[] { Ca.Nr1, Ca.OpD, Ca.Nr0, Ca.Calc, Ca.Nr2, Ca.OpM, Ca.Nr2, Ca.Calc })] // A Calculation that throws followed by another calculation -> should not be harmed by this because of a reset after a throw.
        public void ShowExpectedOutputText_WhenLastActionWasCalculate(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("0,", new[] { Ca.Nr1, Ca.ClearP })]
        public void ShowExpectedOutputText_WhenLastActionWasClear(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("0,000", new[] { Ca.Nr1, Ca.SetYears, Ca.SetInt })] // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetInt, Ca.SetStart })] // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetStart, Ca.SetRate })] // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetRate, Ca.SetEnd })] // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetEnd, Ca.SetYears })] // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        public void ShowExpectedOutputText_WhenLastActionWasSettingSpecialValue(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("3,", 3, new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc })]
        [InlineData("32,", 32, new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("68,", 68, new[] { Ca.Nr34, Ca.OpM, Ca.Nr2, Ca.Calc })]
        [InlineData("17,", 17, new[] { Ca.Nr34, Ca.OpD, Ca.Nr2, Ca.Calc })]
        public void ShowExpectedOutputText_WhenBasicArithmeticIsPerformed(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, Tolerance);

        [Theory]
        [InlineData("0,323529412", 0.323529412, new[] { Ca.Nr1, Ca.Nr1, Ca.OpD, Ca.Nr34, Ca.Calc})] // 0,3235294117647058823 Produces a fraction with more fractional digits than allowed. Fractional digits are rounded.
        public void ShowExpectedOutputText_WhenCalculationProducesMoreFractionalDigitsThanDisplayable(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, Tolerance);
    }
}
