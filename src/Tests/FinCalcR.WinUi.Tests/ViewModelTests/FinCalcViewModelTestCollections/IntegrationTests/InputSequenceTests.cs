using System;
using System.Globalization;
using System.Threading;
using FinCalcR.WinUi.Tests.Mocks;
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
    }
}
