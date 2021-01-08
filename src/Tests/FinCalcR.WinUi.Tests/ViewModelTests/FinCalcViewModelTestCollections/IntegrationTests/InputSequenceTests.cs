using System;
using System.Globalization;
using System.Threading;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    // TODO: Refactor the whole class. It is kind of a relict from the beginning to explore the way the vm and long sequences of input can be tested.
    public class InputSequenceTests
    {
        private const double Tolerance = 0.00000001;
        private const int RatesPerAnnum = 12;

        public InputSequenceTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        #region Focus Rate

        [Fact]
        public void PressingRateLongTouchAlwaysUpdatesRepaymentRateButNotRepayment()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            vm.DigitPressedCommand.Execute(7);
            vm.DigitPressedCommand.Execute(5);
            vm.DigitPressedCommand.Execute(0);
            vm.AlgebSignCommand.Execute(null);
            vm.RatePressedCommand.Execute(false);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);

            // Changing a dependent parameter: Start
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.StartPressedCommand.Execute(false);

            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "-750,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -750) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void CalculationOfRateLeadsToNaNAndUpcomingOperationsAreNotHarmed()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Produce NaN for repayment rate number
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once);

            // Assert display is set back to zero and not NaN or something
            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            // Show rate number that was calculated with NaN of repayment rate number
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);

            // Assert values are set back to zero and not NaN or something
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
            Assert.True(Math.Abs(vm.RateNumber - 0) < Tolerance);
        }

        #endregion

        #region Focus End

        [Fact]
        public void PercentageCalculationIsTriggered()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // AlgebSign
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.AlgebSignCommand.Execute(null);
            vm.AlgebSignCommand.Execute(null);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "10,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);

            // AlgebSign
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "10,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);
        }

        [Fact]
        public void AfterPercentageCalculationTheResultIsUsedAsFirstNumber()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Operator
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "10,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);

            vm.OperatorPressedCommand.Execute("-");
            Assert.True(vm.DisplayText == "10,");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);
            vm.DigitPressedCommand.Execute(5);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "5,");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);

            // AlgebSign
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(false);
            Assert.True(vm.DisplayText == "-10,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -10) < Tolerance);

            // Calculate
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            vm.CalculatePressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "10,00"); // Physical calculator cuts the decimals but is one key for += so the operator logic might get called.
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);

            // Decimal separator
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            vm.DecimalSeparatorPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "10,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "0,1");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.1) < Tolerance);
        }

        #endregion

        #region Focus Rates per Annum

        [Fact]
        public void DisplayTextAfterFollowingOperationsToRatesPerAnnumIsCorrect()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // after rpa is set
            vm.DigitPressedCommand.Execute(7);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            Assert.True(vm.DisplayText == "7,");
            Assert.True(Math.Abs(vm.DisplayNumber - 7) < RatesPerAnnum);
            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "5,");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.DigitPressedCommand.Execute(7);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "7,");
            Assert.True(Math.Abs(vm.DisplayNumber - 7) < Tolerance);
            vm.DigitPressedCommand.Execute(3);
            Assert.True(vm.DisplayText == "0,3");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.3) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.DigitPressedCommand.Execute(7);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-7,");
            Assert.True(Math.Abs(vm.DisplayNumber - -7) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.DigitPressedCommand.Execute(7);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(3);
            Assert.True(vm.DisplayText == "3,");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);
            vm.OperatorPressedCommand.Execute("+");
            vm.DigitPressedCommand.Execute(3);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "6,");
            Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // after rpa is called from memory
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            vm.OperatorPressedCommand.Execute("-");
            Assert.True(vm.DisplayText == "12,");
            Assert.True(Math.Abs(vm.DisplayNumber - 12) < RatesPerAnnum);
            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "10,");
            Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "12,");
            Assert.True(Math.Abs(vm.DisplayNumber - 12) < Tolerance);
            vm.DigitPressedCommand.Execute(3);
            Assert.True(vm.DisplayText == "0,3");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.3) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-12,");
            Assert.True(Math.Abs(vm.DisplayNumber - -12) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            vm.DigitPressedCommand.Execute(3);
            Assert.True(vm.DisplayText == "3,");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);
            vm.OperatorPressedCommand.Execute("+");
            vm.DigitPressedCommand.Execute(3);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "6,");
            Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);
        }

        #endregion

        private static void PerformBasicEndCapitalCalculation(FinCalcViewModel vm)
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
            Assert.True(Math.Abs(vm.DisplayNumber - -113187.5488186329) < Tolerance);
            Assert.True(Math.Abs(vm.EndNumber - -113187.5488186329) < Tolerance);
        }
    }
}
