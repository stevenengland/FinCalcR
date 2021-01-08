using System;
using FinCalcR.WinUi.Tests.Mocks;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinCalcViewModelShould : TestBase
    {
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

        // Call all the special setters
        [InlineData("1,00", new[] { Ca.Nr1, Ca.SetYears })]
        [InlineData("1,000", new[] { Ca.Nr1, Ca.SetInt })]
        [InlineData("1,00", new[] { Ca.Nr1, Ca.SetStart })]
        [InlineData("1,00", new[] { Ca.Nr1, Ca.SetRate })]
        [InlineData("1,00", new[] { Ca.Nr1, Ca.SetEnd })]
        [InlineData("1,005", new[] { Ca.Nr1, Ca.SetNom })]
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetRep })]
        [InlineData("1 p.a.", new[] { Ca.Nr1, Ca.SetRpa })]

        // If a special function is invoked right after another the display shall be zero (unless there is a reason to calculate the special value).
        [InlineData("0,000", new[] { Ca.Nr1, Ca.SetYears, Ca.SetInt })]
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetInt, Ca.SetStart })]
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetStart, Ca.SetRate })]
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetRate, Ca.SetEnd })]
        [InlineData("0,00", new[] { Ca.Nr1, Ca.SetEnd, Ca.SetYears })]

        // Round trip with a number before the special function. Intent is to NOT invoke the calculation of the the fifth and following special numbers because of the number set before.
        [InlineData("1,00", new[] { Ca.Nr1, Ca.SetYears, Ca.Nr1, Ca.SetInt, Ca.Nr1, Ca.SetStart, Ca.Nr1, Ca.SetRate, Ca.Nr1, Ca.SetEnd, Ca.Nr1, Ca.SetYears })]

        // A subtraction is performed. Instead of = the interest button is pressed. The last number goes into memory without performing the calculation.
        [InlineData("2,000", new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.SetInt })]
        public void ShowExpectedOutputText_WhenLastActionWasSettingSpecialValue(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("3,", 3, new[] { Ca.Nr1, Ca.OpA, Ca.Nr2, Ca.Calc })]
        [InlineData("32,", 32, new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("68,", 68, new[] { Ca.Nr34, Ca.OpM, Ca.Nr2, Ca.Calc })]
        [InlineData("17,", 17, new[] { Ca.Nr34, Ca.OpD, Ca.Nr2, Ca.Calc })]
        public void ShowExpectedOutputText_WhenBasicArithmeticIsPerformed(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, this.Tolerance);

        [Theory]
        [InlineData("0,323529412", 0.323529412, new[] { Ca.Nr1, Ca.Nr1, Ca.OpD, Ca.Nr34, Ca.Calc })] // 0,3235294117647058823 Produces a fraction with more fractional digits than allowed. Fractional digits are rounded.
        [InlineData("1,001", 1.0005, new[] { Ca.Nr1, Ca.Dec, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr5, Ca.SetInt })]
        [InlineData("1,010", 1.0095, new[] { Ca.Nr1, Ca.Dec, Ca.Nr0, Ca.Nr0, Ca.Nr9, Ca.Nr5, Ca.SetInt })]
        public void ShowExpectedOutputText_WhenOutputIsRoundedBecauseInputOrCalculationProducesMoreFractionalDigitsThanDisplayable(string expectedOutputTextAfterAllOperations, double expectedNumberAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, expectedNumberAfterAllOperations, this.Tolerance);

        [Theory]
        [InlineData("1,000", new[] { Ca.Nr1, Ca.SetInt })]
        [InlineData("1,100", new[] { Ca.Nr1, Ca.Dec, Ca.Nr1, Ca.SetInt })]
        [InlineData("1,010", new[] { Ca.Nr1, Ca.Dec, Ca.Nr0, Ca.Nr1, Ca.SetInt })]
        [InlineData("1,001", new[] { Ca.Nr1, Ca.Dec, Ca.Nr0, Ca.Nr0, Ca.Nr1, Ca.SetInt })]
        [InlineData("1,000", new[] { Ca.Nr1, Ca.Dec, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr1, Ca.SetInt })]
        public void ShowExpectedOutputText_WhenFractionalPartNeedsFilling(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]

        // A subtraction is performed. Instead of = the interest button is pressed. The last number goes into memory without performing the calculation.
        // After that another subtraction is performed. The interest value is used as first number.
        [InlineData("-32,", new[] { Ca.Nr34, Ca.OpS, Ca.Nr2, Ca.SetInt, Ca.OpS, Ca.Nr34, Ca.Calc })]

        // Also uses the full number instead of the displayed number when fractional part exceeds displayable number
        [InlineData("1,123412", new[] { Ca.Nr0, Ca.Dec, Ca.Nr1, Ca.Nr2, Ca.Nr34, Ca.Nr1, Ca.Nr2, Ca.SetInt, Ca.OpA, Ca.Nr1, Ca.Calc })]

        // The interest value is loaded from the memory. A value is subtracted. The result is a calculation with interest as first number.
        [InlineData("-32,", new[] { Ca.Nr2, Ca.SetInt, Ca.GetInt, Ca.OpS, Ca.Nr34, Ca.Calc })]

        // A digit is entered followed by an operator. Then the interest button is pressed. Digit - Interest -> Digit is taken as interest number.
        [InlineData("2,000", new[] { Ca.Nr2, Ca.OpS, Ca.SetInt })]

        // A digit is added to another followed by an operator. Then the interest button is pressed. 2 + 2 - Interest -> 4.000 is taken as interest number.
        [InlineData("4,000", new[] { Ca.Nr2, Ca.OpA, Ca.Nr2, Ca.OpS, Ca.SetInt })]
        [InlineData("2,074154292", new[] { Ca.Nr2, Ca.OpA, Ca.Nr2, Ca.OpM, Ca.SetInt, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("2,074154292", new[] { Ca.Nr2, Ca.OpA, Ca.Nr2, Ca.Calc, Ca.OpM, Ca.SetInt, Ca.OpS, Ca.Nr2, Ca.Calc })]

        // After percentage calculation the result is used as first number
        [InlineData("5,", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.SetEnd, Ca.OpS, Ca.Nr5, Ca.Calc })] // Operator
        [InlineData("-10,00", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.SetEnd, Ca.Alg })] // AlgebSign
        [InlineData("10,00", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.SetEnd, Ca.Calc })] // Calculate
        [InlineData("0,1", new[] { Ca.Nr2, Ca.Nr0, Ca.Nr0, Ca.OpM, Ca.Nr5, Ca.SetEnd, Ca.Dec, Ca.Nr1 })] // AlgebSign
        public void UseCurrentOutputAsFirstNumber(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        [Theory]
        [InlineData("-113.186,548818633", new[] { Ca.OpA, Ca.Nr1, Ca.Calc })] // Operator
        [InlineData("2,", new[] { Ca.Nr1, Ca.OpA, Ca.Nr1, Ca.Calc })] // Digit
        [InlineData("113.187,55", new[] { Ca.Alg })] // AlgebSign
        [InlineData("0,1", new[] { Ca.Dec, Ca.Nr1 })] // Decimal
        public void ShowExpectedOutputText_WhenActionsArePerformedAfterCalculationOfEndNumber(string expectedOutputTextAfterAllOperations, Ca[] actions)
        {
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(out _);
            this.PerformBasicEndCapitalCalculation(vm);
            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations, vm);
        }

        [Theory]
        [InlineData("5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.OpS })]
        [InlineData("3,", new[] { Ca.Nr5, Ca.SetRpa, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.Dec })]
        [InlineData("0,2", new[] { Ca.Nr5, Ca.SetRpa, Ca.Dec, Ca.Nr2 })]
        [InlineData("-5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.Alg })]
        [InlineData("2,", new[] { Ca.Nr5, Ca.SetRpa, Ca.Nr2 })]
        [InlineData("4,", new[] { Ca.Nr5, Ca.SetRpa, Ca.Nr2, Ca.OpA, Ca.Nr2, Ca.Calc })]

        // When called from memory
        [InlineData("5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.OpS })]
        [InlineData("3,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.OpS, Ca.Nr2, Ca.Calc })]
        [InlineData("5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.Dec })]
        [InlineData("0,2", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.Dec, Ca.Nr2 })]
        [InlineData("-5,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.Alg })]
        [InlineData("2,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.Nr2 })]
        [InlineData("4,", new[] { Ca.Nr5, Ca.SetRpa, Ca.GetRpa, Ca.Nr2, Ca.OpA, Ca.Nr2, Ca.Calc })]
        public void ShowExpectedOutputText_AfterRatesPerAnnumAreSet(string expectedOutputTextAfterAllOperations, Ca[] actions) => FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(actions, expectedOutputTextAfterAllOperations);

        private void PerformBasicEndCapitalCalculation(FinCalcViewModel vm)
        {
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(5);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.StartPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);
            vm.EndPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "-113.187,55");
            Assert.True(Math.Abs(vm.DisplayNumber - -113187.5488186329) < this.Tolerance);
            Assert.True(Math.Abs(vm.EndNumber - -113187.5488186329) < this.Tolerance);
        }
    }
}
