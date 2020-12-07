using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;
using StEn.FinCalcR.Common.Extensions;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Events.EventArgs;
using StEn.FinCalcR.WinUi.Types;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
    public class FinCalcViewModelTests2
    {
        private const double Tolerance = 0.00000001;

        public FinCalcViewModelTests2()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Fact]
        public void KeyboardEventsAreHandled()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            eventAggregatorMock.Verify(x => x.Subscribe(It.IsAny<object>()), Times.Once);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1")));
            Assert.True(vm.DisplayText == "2,00");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2")));
            Assert.True(vm.DisplayText == "2,000");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,000");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3")));
            Assert.True(vm.DisplayText == "2,00");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F5")));
            Assert.True(vm.DisplayText == "2,00");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F5") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F6")));
            Assert.True(vm.DisplayText == "2 p.a.");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F6") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2 p.a.");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F7")));
            Assert.True(vm.DisplayText == "2,018");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F7") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,000");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F8")));
            Assert.True(vm.AdvanceStatusBarText != string.Empty);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F8") { IsShiftPressed = true }));
            Assert.True(vm.AdvanceStatusBarText == string.Empty);
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F9")));
            Assert.True(vm.DisplayText == "-0,01");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F9") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1")));
            Assert.True(vm.DisplayText == "2,00");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = false }));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete")));
            Assert.True(vm.DisplayText == "0,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1")));
            Assert.True(vm.DisplayText == "2,00");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete") { IsShiftPressed = true }));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1") { IsShiftPressed = true }));
            Assert.True(vm.DisplayText == "0,00");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Multiply")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return")));
            Assert.True(vm.DisplayText == "6,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad4")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Divide")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return")));
            Assert.True(vm.DisplayText == "2,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Subtract")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return")));
            Assert.True(vm.DisplayText == "-2,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Add")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return")));
            Assert.True(vm.DisplayText == "4,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemQuestion")));
            Assert.True(vm.DisplayText == "-1,");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Decimal")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemComma")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemPeriod")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1")));
            Assert.True(vm.DisplayText == "1,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D1")));
            Assert.True(vm.DisplayText == "11,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2")));
            Assert.True(vm.DisplayText == "2,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D2")));
            Assert.True(vm.DisplayText == "22,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3")));
            Assert.True(vm.DisplayText == "3,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D3")));
            Assert.True(vm.DisplayText == "33,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad4")));
            Assert.True(vm.DisplayText == "4,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D4")));
            Assert.True(vm.DisplayText == "44,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad5")));
            Assert.True(vm.DisplayText == "5,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D5")));
            Assert.True(vm.DisplayText == "55,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad6")));
            Assert.True(vm.DisplayText == "6,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D6")));
            Assert.True(vm.DisplayText == "66,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad7")));
            Assert.True(vm.DisplayText == "7,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D7")));
            Assert.True(vm.DisplayText == "77,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad8")));
            Assert.True(vm.DisplayText == "8,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D8")));
            Assert.True(vm.DisplayText == "88,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad9")));
            Assert.True(vm.DisplayText == "9,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D9")));
            Assert.True(vm.DisplayText == "99,");
            vm.ClearPressedCommand.Execute(true);
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad0")));
            Assert.True(vm.DisplayText == "0,");
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D1")));
            vm.Handle(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D0")));
            Assert.True(vm.DisplayText == "10,");
            vm.ClearPressedCommand.Execute(true);
        }

        [Fact]
        public async Task LastPressedOperationIsSetAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);

            Assert.True(vm.LastPressedOperation == LastPressedOperation.Clear);
            vm.DigitPressedCommand.Execute("1");
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Digit);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.AlgebSign);
            vm.OperatorPressedCommand.Execute("-"); // needed to be set before calculation
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Calculate);
            vm.OperatorPressedCommand.Execute("+");
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Operator);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Decimal);
            vm.ClearPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Clear);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.ClearPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Clear);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Years);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Years);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
            vm.LastPressedOperation = LastPressedOperation.None;
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Start);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Start);
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Rate);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Rate);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.End);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.End);

            // second function
            vm.ClearPressedCommand.Execute(true);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Clear);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.RatesPerAnnum);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.RatesPerAnnum);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
            vm.LastPressedOperation = LastPressedOperation.None;
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Start);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Start);
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Rate);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.Rate);
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.End);
            vm.LastPressedOperation = LastPressedOperation.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == LastPressedOperation.End);

            vm.ClearPressedCommand.Execute(true);

            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "0,10");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.1) < Tolerance);
        }

        [Fact]
        public void StatusBarTextsAreSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.YearsStatusBarText == string.Empty);
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);
            vm.ClearPressedCommand.Execute(true);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);

            Assert.True(vm.InterestStatusBarText == string.Empty);
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);
            vm.ClearPressedCommand.Execute(true);
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);

            Assert.True(vm.StartStatusBarText == string.Empty);
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);
            vm.ClearPressedCommand.Execute(true);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);

            Assert.True(vm.RateStatusBarText == string.Empty);
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.RateStatusBarText == Resources.FinCalcFunctionRate);
            vm.ClearPressedCommand.Execute(true);

            // To avoid throw because of NaN:
            vm.DigitPressedCommand.Execute(1);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.InterestPressedCommand.Execute(true);
            vm.DigitPressedCommand.Execute(1);
            vm.StartPressedCommand.Execute(false);
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.RateStatusBarText == Resources.FinCalcFunctionRate);

            Assert.True(vm.EndStatusBarText == string.Empty);
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
            vm.ClearPressedCommand.Execute(true);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
        }

        [Fact]
        public void RoundingDisplayValueWorks()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(4);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(9);
            vm.DigitPressedCommand.Execute(9);
            vm.DigitPressedCommand.Execute(9);
            vm.DigitPressedCommand.Execute(9);
            vm.InterestPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.9999) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(1);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(5);
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "1,001");
            Assert.True(Math.Abs(vm.DisplayNumber - 1.0005) < Tolerance);
        }

        [Fact]
        public void ThousandSeparatorIsSetCorrectly()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(3);
            Assert.True(vm.DisplayText == "123,");
            Assert.True(Math.Abs(vm.DisplayNumber - 123) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-123,");
            Assert.True(Math.Abs(vm.DisplayNumber - -123) < Tolerance);
            vm.DigitPressedCommand.Execute(4);
            Assert.True(vm.DisplayText == "-1.234,");
            Assert.True(Math.Abs(vm.DisplayNumber - -1234) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "1.234,");
            Assert.True(Math.Abs(vm.DisplayNumber - 1234) < Tolerance);
            vm.DigitPressedCommand.Execute(5);
            vm.DigitPressedCommand.Execute(6);
            vm.DigitPressedCommand.Execute(7);
            Assert.True(vm.DisplayText == "1.234.567,");
            Assert.True(Math.Abs(vm.DisplayNumber - 1234567) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-1.234.567,");
            Assert.True(Math.Abs(vm.DisplayNumber - -1234567) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            vm.DecimalSeparatorPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(3);
            vm.DigitPressedCommand.Execute(4);
            Assert.True(vm.DisplayText == "1.234.567,1234");
            Assert.True(Math.Abs(vm.DisplayNumber - 1234567.1234) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-1.234.567,1234");
            Assert.True(Math.Abs(vm.DisplayNumber - -1234567.1234) < Tolerance);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "-1.234.567,12");
            Assert.True(Math.Abs(vm.DisplayNumber - -1234567.1234) < Tolerance);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "1.234.567,12");
            Assert.True(Math.Abs(vm.DisplayNumber - 1234567.1234) < Tolerance);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "1.234.567,12");
            Assert.True(Math.Abs(vm.DisplayNumber - 1234567.1234) < Tolerance);
        }

        #region Initialization Tests

        [Fact]
        public void CorrectSeparatorsAreUsedForEachLanguage()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            Assert.True(vm.ThousandsSeparator == ".");
            Assert.True(vm.DecimalSeparator == ",");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            Assert.True(vm.ThousandsSeparator == ",");
            Assert.True(vm.DecimalSeparator == ".");
        }

        [Fact]
        public void CorrectStartupResultTextIsSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.DisplayText == "0,");
        }

        [Fact]
        public void NoSpecialFunctionPressedFlagsAreSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Years));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Interest));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Start));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Rate));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.End));
        }

        #endregion

        #region General Tests

        [Fact]
        public void FlagsAreSetByEachSpecialFunction()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Initial function
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsOnlyFlagSet(PressedSpecialFunctions.Years));
            vm.ClearPressedCommand.Execute(true);
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsOnlyFlagSet(PressedSpecialFunctions.Interest));
            vm.ClearPressedCommand.Execute(true);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsOnlyFlagSet(PressedSpecialFunctions.Start));
            vm.ClearPressedCommand.Execute(true);
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsOnlyFlagSet(PressedSpecialFunctions.Rate));
            vm.ClearPressedCommand.Execute(true);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsOnlyFlagSet(PressedSpecialFunctions.End));
            vm.ClearPressedCommand.Execute(true);
        }

        /// <summary>
        /// Normally I would expect a few functions to not set the flag (especially if there is an error). But the physical calculator does, so am I.
        /// </summary>
        [Fact]
        public void FlagsAreSetBySpecificSpecialSecondFunction()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Initial function
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Years));
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Interest));
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Start));
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Rate));
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.End));
        }

        /// <summary>
        /// Normally I would expect a few functions to not set the flag (especially if there is an error). But the physical calculator does, so am I.
        /// </summary>
        [Fact]
        public void FlagsAreSetBySpecificLongPressedSpecialSecondFunction()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Initial function
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Years));
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Interest));
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Start));
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Rate));
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.End));
        }

        #endregion

        #region Decimal Separator Tests

        [Fact]
        public void PressingDecimalSeparatorResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void InputAfterDecimalWasPressedIsHandledCorrectly()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Decimal separator as first input
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.0) < Tolerance);
            vm.DigitPressedCommand.Execute("4");
            Assert.True(vm.DisplayText == "0,4");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.4) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // Decimal separator after special function
            vm.DigitPressedCommand.Execute(3);
            vm.StartPressedCommand.Execute(false);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "0,1");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.1) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // Decimal separator after percentage
            vm.DigitPressedCommand.Execute(3);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(1);
            vm.EndPressedCommand.Execute(false);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,03");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.03) < Tolerance);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "0,1");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.1) < Tolerance);
        }

        #endregion

        #region Math Operator Tests

        [Fact]
        public void PressingMathOperatorResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.OperatorPressedCommand.Execute("+");
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void OperatorIsSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.ActiveMathOperator == MathOperator.None);
            vm.OperatorPressedCommand.Execute("-");
            Assert.True(vm.ActiveMathOperator == MathOperator.Subtract);
        }

        [Fact]
        public void FirstNumberIsAssumedToBeZeroIfOperatorIsFirstInput()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);

            Assert.True(vm.DisplayText == "-2,");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);
        }

        [Fact]
        public async Task OperatorPressedAfterSpecialFunctionDoesNotResetAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            vm.DigitPressedCommand.Execute(2);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.StartPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.RatePressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            // long presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(true);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            vm.DigitPressedCommand.Execute(2);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.StartPressedCommand.Execute(true);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.RatePressedCommand.Execute(true);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(true);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        #endregion

        #region Digit Input Tests

        [Fact]
        public void PressingDigitsResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.DigitPressedCommand.Execute("1");
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void PressingZeroWhenLeftSideIsZeroDoesNotAddAnotherZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            for (var i = 1; i < 20; i++)
            {
                vm.DigitPressedCommand.Execute(0);
            }

            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingGtZeroWhenFirstInputWasZeroTrimsLeadingZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(9);

            Assert.True(vm.DisplayText == "9,");
            Assert.True(Math.Abs(vm.DisplayNumber - 9) < Tolerance);
        }

        [Fact]
        public void LeftSideDoesNotExceedInputLimit()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            for (var i = 1; i < 20; i++)
            {
                vm.DigitPressedCommand.Execute(1);
            }

            Assert.True(vm.DisplayText == "1.111.111.111,");
            Assert.True(Math.Abs(vm.DisplayNumber - 1111111111) < Tolerance);
        }

        [Fact]
        public void RightSideDoesNotExceedInputLimit()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DecimalSeparatorPressedCommand.Execute(null);
            for (var i = 1; i < 20; i++)
            {
                vm.DigitPressedCommand.Execute(1);
            }

            Assert.True(vm.DisplayText == "0,111111111");
            Assert.True(Math.Abs(vm.DisplayNumber - 0.111111111) < Tolerance);
        }

        #endregion

        #region Calculate Tests

        [Fact]
        public void PressingCalculationAsFirstButtonDisplaysZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingCalculateResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.CalculatePressedCommand.Execute(null);
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void PressingCalculateAfterNumberWithOrWithoutActiveOperatorResetsStandardNumbers()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingCalculateAfterNumberWithActiveOperatorAssumesSecondNumberToBeZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-");
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "2,"); // Second number is assumed to be 0 if there is no digit input after an operator.
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public async Task PressingCalculateAfterSpecialFunctionResetsStandardNumbersAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            vm.DigitPressedCommand.Execute(3);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(4);
            vm.StartPressedCommand.Execute(false);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(5);
            vm.RatePressedCommand.Execute(false);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(6);
            vm.EndPressedCommand.Execute(false);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            // long presses
            vm.YearsPressedCommand.Execute(true);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.StartPressedCommand.Execute(true);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.RatePressedCommand.Execute(true);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.EndPressedCommand.Execute(true);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingCalculateMultipleTimesHasNoEffect()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("+");
            vm.DigitPressedCommand.Execute(1);

            vm.CalculatePressedCommand.Execute(null);

            Assert.True(vm.DisplayText == "11,");
            Assert.True(Math.Abs(vm.DisplayNumber - 11) < Tolerance);

            vm.CalculatePressedCommand.Execute(null);

            Assert.True(vm.DisplayText == "11,");
            Assert.True(Math.Abs(vm.DisplayNumber - 11) < Tolerance);
        }

        [Fact]
        public void DivisionByZeroThrowsAndResets()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(1);
            vm.OperatorPressedCommand.Execute("/");
            vm.DigitPressedCommand.Execute(0);
            vm.CalculatePressedCommand.Execute(null);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once);

            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        #endregion

        #region Clear Tests

        [Fact]
        public void ClearingResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.ClearPressedCommand.Execute(false);
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void ClearingLongTouchResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.ClearPressedCommand.Execute(true);
            this.AssertVmStatusLabelsAreEmpty(vm, true);
        }

        [Fact]
        public void ClearingResetsStandardValues()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute("3");
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute("1");
            vm.CalculatePressedCommand.Execute(null);

            Assert.True(vm.DisplayText == "2,");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public async Task ClearingDoesNotResetSpecialFunctionMemoryAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute("3");
            vm.YearsPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.StartPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.RatePressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.EndPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "3,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);

            // second functions
            vm.ClearPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,074");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(6);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "6 p.a.");
            Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "6 p.a.");
            Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);

            // Repayment rate is already set above. But it gets recalculated when [Rate] is long pressed
            Assert.True(vm.DisplayText == "-604,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -604) < Tolerance);
        }

        [Fact]
        public async Task ClearingLongTouchResetsSpecialFunctionMemoryAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute("3");
            vm.YearsPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.StartPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.RatePressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.EndPressedCommand.Execute(false);

            vm.ClearPressedCommand.Execute(true);

            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "0,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            // second functions
            vm.DigitPressedCommand.Execute(6);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "6 p.a.");
            Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,067");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.06726223) < Tolerance);

            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.StartPressedCommand.Execute(false); // sets repayment rate also

            vm.ClearPressedCommand.Execute(true); // completely reset

            // To avoid NaN set a few values
            vm.DigitPressedCommand.Execute(1);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.StartPressedCommand.Execute(false);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "12 p.a.");
            Assert.True(Math.Abs(vm.DisplayNumber - 12) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "0,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void ClearingLongTouchShowsResetHint()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.ClearPressedCommand.Execute(true);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<HintEvent>(), It.IsAny<Action<System.Action>>()), Times.Once);
        }

        [Fact]
        public void ClearingShortTouchDoesNotResetsFlags()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.YearsPressedCommand.Execute(false);
            vm.InterestPressedCommand.Execute(false);
            vm.StartPressedCommand.Execute(false);
            vm.RatePressedCommand.Execute(false);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsEveryFlagSet());

            vm.ClearPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsEveryFlagSet());
        }

        [Fact]
        public void ClearingLongTouchResetsFlags()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.YearsPressedCommand.Execute(false);
            vm.InterestPressedCommand.Execute(false);
            vm.StartPressedCommand.Execute(false);
            vm.RatePressedCommand.Execute(false);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.PressedSpecialFunctions.IsEveryFlagSet());

            vm.ClearPressedCommand.Execute(true);
            Assert.True(vm.PressedSpecialFunctions.IsNoFlagSet());
        }

        #endregion

        #region Algeb Sign Tests

        [Fact]
        public void AlgebSignResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.AlgebSignCommand.Execute(null);
            this.AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void AlgebSignIsShownAndUnShown()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.AlgebSignCommand.Execute(null);

            Assert.True(vm.DisplayText == "-0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.AlgebSignCommand.Execute(null);

            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.DigitPressedCommand.Execute("3");
            vm.AlgebSignCommand.Execute(null);

            Assert.True(vm.DisplayText == "-3,");
            Assert.True(Math.Abs(vm.DisplayNumber - -3) < Tolerance);
        }

        [Fact]
        public void AlgebSignAsFirstInputFollowedByDigitOutputsDigit()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(9);

            Assert.True(vm.DisplayText == "-9,");
            Assert.True(Math.Abs(vm.DisplayNumber - -9) < Tolerance);
        }

        [Fact]
        public void AlgebSignAfterSpecialFunctionIsCorrectlyFormatted()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-2,00"); // not -2,
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);
        }

        [Fact]
        public void InputsAfterAlgebSignAreHandledCorrectly()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Digit, AlgebSign, Decimal Separator, Digit
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.AlgebSignCommand.Execute(null);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-10,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -10.1) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // Digit, AlgebSign, Digit
            vm.DigitPressedCommand.Execute(1);
            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-11,");
            Assert.True(Math.Abs(vm.DisplayNumber - -11) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            //// Digit, Special Function, AlgebSign, Decimal Separator, Digit
            vm.DigitPressedCommand.Execute(1);
            vm.EndPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "-1,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -1) < Tolerance);
            vm.DigitPressedCommand.Execute(2);
            Assert.True(vm.DisplayText == "-1,2"); // ToDo: physical calc expects 0,2
            Assert.True(Math.Abs(vm.DisplayNumber - -1.2) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // Digit, Special Function, AlgebSign, Decimal Separator, SpecialFunction
            vm.DigitPressedCommand.Execute(1);
            vm.EndPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "-1,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -1) < Tolerance);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "0,"); // ToDo: physical calc expects -1.00
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(true);

            // Special function, AlgebSign, Digit
            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-21,"); // ToDo: physical calc expects -1,
            Assert.True(Math.Abs(vm.DisplayNumber - -21) < Tolerance);
        }

        [Fact]
        public async Task AlgebSignPressedAfterSpecialFunctionDoesNotResetAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            vm.DigitPressedCommand.Execute(3);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-3,000");
            Assert.True(Math.Abs(vm.DisplayNumber - -3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(4);
            vm.StartPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-4,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -4) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(5);
            vm.RatePressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -5) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(6);
            vm.EndPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-6,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -6) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            // long presses
            vm.YearsPressedCommand.Execute(true);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-3,000");
            Assert.True(Math.Abs(vm.DisplayNumber - -3) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.StartPressedCommand.Execute(true);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-4,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -4) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.RatePressedCommand.Execute(true);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -5) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.EndPressedCommand.Execute(true);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-6,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -6) < Tolerance);
        }

        #endregion

        #region Years Tests

        [Fact]
        public void PressingYearsResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.YearsPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            this.SetVmStatusLabelTexts(vm);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingYearsSetsYearsValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
            vm.ClearPressedCommand.Execute(false);
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void PressingYearsSecondFunctionSetsRatesPerAnnum()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "2 " + Resources.FinCalcRatesPerAnnumPostfix);
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
            vm.ClearPressedCommand.Execute(false);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);

            Assert.True(vm.DisplayText == "2 " + Resources.FinCalcRatesPerAnnumPostfix);
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void YearsDoNotExceedLimitsAndReset()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(3);
            vm.YearsPressedCommand.Execute(false); // put a valid value to the memory

            vm.ClearPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-0,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -0.1) < Tolerance);

            vm.YearsPressedCommand.Execute(false);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset
        }

        [Fact]
        public void RatesPerAnnumDoNotExceedLimitsButThrow()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(3);
            vm.YearsPressedCommand.Execute(false); // put a valid value to the memory

            vm.ClearPressedCommand.Execute(false);

            // zero or negative values
            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(0);
            Assert.True(vm.DisplayText == "-0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset

            // positive values gt limit
            vm.DigitPressedCommand.Execute(3);
            vm.DigitPressedCommand.Execute(6);
            vm.DigitPressedCommand.Execute(6);
            Assert.True(vm.DisplayText == "366,");
            Assert.True(Math.Abs(vm.DisplayNumber - 366) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Exactly(2)); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset

            // positive values gt limit
            vm.DigitPressedCommand.Execute(3);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "30,1");
            Assert.True(Math.Abs(vm.DisplayNumber - 30.1) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Exactly(3)); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset
        }

        [Fact]
        public void PressingYearsShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.YearsPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingYearsShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.YearsPressedCommand.Execute(false);

            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);
        }

        #endregion

        #region Interest Tests

        [Fact]
        public async Task PressingInterestResetsStatusLabelTextsAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            this.SetVmStatusLabelTexts(vm);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            this.SetVmStatusLabelTexts(vm);
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public async Task PressingInterestAlsoCalculatesNominalInterestRateAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute(4);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "3,928");
            Assert.True(Math.Abs(vm.DisplayNumber - 3.928487739) < Tolerance);
        }

        [Fact]
        public async Task PressingNominalInterestAlsoCalculatesEffectiveInterestRateButShowsEffectiveInterestRateAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,074");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
        }

        [Fact]
        public async Task InterestDoesNotExceedLimitsAndResetsAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute(3);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object); // put a valid value to the memory

            vm.ClearPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-100,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -100.1) < Tolerance);

            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "3,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset

            vm.ClearPressedCommand.Execute(true); // reset for tests with second function

            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object); // put a valid value to the memory

            vm.ClearPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-100,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -100.1) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);

            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Exactly(2)); // 1 + 1 from above
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
            Assert.True(vm.DisplayText == "4,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance); // sides are reset
        }

        [Fact]
        public void PressingInterestShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.InterestPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingInterestShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.InterestPressedCommand.Execute(false);

            Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);
        }

        #endregion

        #region Start Tests

        [Fact]
        public void PressingStartResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.StartPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.EndStatusBarText)); // An other label than the one belonging to the command.
            this.SetVmStatusLabelTexts(vm);
            vm.StartPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.EndStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingStartSetsStartValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
            vm.ClearPressedCommand.Execute(false);
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void PressingStartDeAndActivatesAdvanceFlagAndStatusLabel()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            Assert.False(vm.IsAdvance());
            Assert.True(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.IsAdvance());
            Assert.False(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));

            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.False(vm.IsAdvance());
            Assert.True(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));
        }

        [Fact]
        public void PressingStartShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.StartPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingStartShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.StartPressedCommand.Execute(false);

            Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);
        }

        #endregion

        #region Rate Tests

        [Fact]
        public void PressingRateResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.RatePressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            this.SetVmStatusLabelTexts(vm);
            vm.RatePressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingRateSetsRateValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // To avoid throw because of NaN when automatically calculating repayment rate:
            vm.DigitPressedCommand.Execute(1);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(1);
            vm.StartPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
            vm.ClearPressedCommand.Execute(false);
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public async Task PressingRateAlsoCalculatesRepaymentRateAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
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
        }

        [Fact]
        public async Task PressingRepaymentRateAlsoCalculatesRepaymentButShowsRepaymentRateAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);
            var gestureHandlerMock = new Mock<IGestureHandler>();
            gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.YearsPressedCommand.Execute(false);
            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            await vm.InterestPressedCommandAsync.ExecuteAsync(gestureHandlerMock.Object);
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
            Assert.True(vm.DisplayText == "-750,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -750) < Tolerance);

            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "-750,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -750) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void PressingRateDoesNotThrowWhenRepaymentRateIsCalculatedAutomaticallyAndIsInvalidNumber()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(0);
            vm.RatePressedCommand.Execute(false); // Would throw div by zero, because repayment rate is calculated in the background with invalid result.
            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Never);
        }

        [Fact]
        public void PressingRateSecondFunctionLongTouchThrowsWhenRepaymentRateIsRecalculatedAndIsInvalidNumber()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once);

            // Assert display is set back to zero and not NaN or something
            Assert.True(double.IsNaN(vm.RepaymentRateNumber));
            Assert.True(vm.DisplayText == "0,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingRateShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.RatePressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingRateShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.RatePressedCommand.Execute(false);

            Assert.True(vm.RateStatusBarText == Resources.FinCalcFunctionRate);
        }

        #endregion

        #region End Tests

        [Fact]
        public void PressingEndResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            this.SetVmStatusLabelTexts(vm);
            vm.EndPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            this.SetVmStatusLabelTexts(vm);
            vm.EndPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingEndSetsEndValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
            vm.ClearPressedCommand.Execute(false);
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void PercentCalculationIsTriggeredOnlyIfConditionsAreMet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // No operator set but last input is a digit
            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(false);

            Assert.False(vm.LastPressedOperation == LastPressedOperation.PercentCalculation);
            vm.ClearPressedCommand.Execute(true);

            // Operator set but last input is a not a digit
            vm.OperatorPressedCommand.Execute("-");
            vm.EndPressedCommand.Execute(false);

            Assert.False(vm.LastPressedOperation == LastPressedOperation.PercentCalculation);
            vm.ClearPressedCommand.Execute(true);
        }

        [Fact]
        public void PercentCalculationDoesNotSetEndFlag()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // 200 * 5
            vm.DigitPressedCommand.Execute(2);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.End));
        }

        [Fact]
        public void PressingEndShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingEndShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
        }

        #endregion

        private void SetVmStatusLabelTexts(FinCalcViewModel2 vm)
        {
            vm.AdvanceStatusBarText = "test";
            vm.YearsStatusBarText = "test";
            vm.InterestStatusBarText = "test";
            vm.StartStatusBarText = "test";
            vm.RateStatusBarText = "test";
            vm.EndStatusBarText = "test";
        }

        private void AssertVmStatusLabelsAreEmpty(FinCalcViewModel2 vm, bool checkAdvanceStatusBarTextToo = false)
        {
            if (checkAdvanceStatusBarTextToo)
            {
                Assert.True(vm.AdvanceStatusBarText == string.Empty);
            }
            else
            {
                Assert.True(vm.AdvanceStatusBarText == "test");
            }

            Assert.True(vm.YearsStatusBarText == string.Empty);
            Assert.True(vm.InterestStatusBarText == string.Empty);
            Assert.True(vm.StartStatusBarText == string.Empty);
            Assert.True(vm.RateStatusBarText == string.Empty);
            Assert.True(vm.EndStatusBarText == string.Empty);
        }
    }
}
