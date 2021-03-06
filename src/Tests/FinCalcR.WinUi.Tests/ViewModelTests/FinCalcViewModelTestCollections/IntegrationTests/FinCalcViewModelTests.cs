﻿using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using MediatR;
using Moq;
using StEn.FinCalcR.Calculations.Calculator;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Messages;
using StEn.FinCalcR.Common.Extensions;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Events.EventArgs;
using StEn.FinCalcR.WinUi.Messages;
using StEn.FinCalcR.WinUi.Types;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class FinCalcViewModelTests
    {
        private const double Tolerance = 0.00000001;

        public FinCalcViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
            ErrorMessages.Instance = new LocalizedErrorMessages();
        }

        [Fact]
        public async Task KeyboardEventsAreIgnoredIfCallerIsNotOfTheSameTypeAsTheHandlingVmAsync()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Act
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", "test")), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", "test")), default).ConfigureAwait(false);

            // Assert
            vm.DisplayText.Should().Be("0,");
        }

        [Fact]
        public async Task KeyboardEventsAreHandledAsync()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,000");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,000");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F5", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F5", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F6", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2 p.a.");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F6", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2 p.a.");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F7", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,018");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F7", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,000");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F8", vm)), default).ConfigureAwait(false);
            Assert.True(vm.AdvanceStatusBarText != string.Empty);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F8", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.AdvanceStatusBarText?.Length == 0);
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F3", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F9", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "-0,01");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F9", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = false }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,00");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Delete", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("F1", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,00");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Multiply", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "6,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad4", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Divide", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Subtract", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "-2,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Add", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Return", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "4,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemQuestion", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "-1,");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Decimal", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemComma", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("OemPeriod", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "1,1");
            vm.ClearPressedCommand.Execute(true);

            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "1,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D1", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "11,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad2", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "2,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D2", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "22,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad3", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "3,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D3", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "33,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad4", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "4,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D4", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "44,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad5", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "5,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D5", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "55,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad6", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "6,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D6", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "66,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad7", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "7,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D7", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "77,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad8", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "8,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D8", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "88,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad9", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "9,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D9", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "99,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("NumPad0", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D0", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "10,");
            vm.ClearPressedCommand.Execute(true);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("D1", vm)), default).ConfigureAwait(false);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Escape", vm)), default).ConfigureAwait(false);
            Assert.True(vm.DisplayText == "0,");

            Assert.False(vm.IsMemoryPaneExpanded);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Down", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.True(vm.IsMemoryPaneExpanded);
            await vm.HandleAsync(new KeyboardKeyDownEvent(new MappedKeyEventArgs("Up", vm) { IsShiftPressed = true }), default).ConfigureAwait(false);
            Assert.False(vm.IsMemoryPaneExpanded);
        }

        [Fact]
        public void LastPressedOperationIsSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            vm.DigitPressedCommand.Execute("1");
            Assert.True(vm.LastPressedOperation == CommandWord.Digit);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == CommandWord.AlgebSign);
            vm.OperatorPressedCommand.Execute("-"); // needed to be set before calculation
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == CommandWord.Calculate);
            vm.OperatorPressedCommand.Execute("+");
            Assert.True(vm.LastPressedOperation == CommandWord.Operator);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            Assert.True(vm.LastPressedOperation == CommandWord.DecimalSeparator);
            vm.ClearPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            vm.LastPressedOperation = CommandWord.None;
            vm.ClearPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetYears);
            vm.LastPressedOperation = CommandWord.None;
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetYears);
            vm.LastPressedOperation = CommandWord.None;
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetEffectiveInterest);
            vm.LastPressedOperation = CommandWord.None;
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetEffectiveInterest);
            vm.LastPressedOperation = CommandWord.None;
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetStart);
            vm.LastPressedOperation = CommandWord.None;
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetStart);
            vm.LastPressedOperation = CommandWord.None;
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetRate);
            vm.LastPressedOperation = CommandWord.None;
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetRate);
            vm.LastPressedOperation = CommandWord.None;
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetEnd);
            vm.LastPressedOperation = CommandWord.None;
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetEnd);

            // second function
            vm.ClearPressedCommand.Execute(true);

            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            vm.DigitPressedCommand.Execute(1); // So that rpa will be in range when set in the next command
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetRatesPerAnnum);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetRatesPerAnnum);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetNominalInterestRate);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetNominalInterestRate);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetAdvance);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.SetAdvance);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetRepaymentRate);
            vm.LastPressedOperation = CommandWord.None;

            // start must be != 0 for the repayment rate.
            vm.DigitPressedCommand.Execute(1);
            vm.StartPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetRepaymentRate);

            // End command does not have a second function
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(false);
            Assert.True(vm.LastPressedOperation == CommandWord.SetEnd);
            vm.LastPressedOperation = CommandWord.None;
            vm.OperatorPressedCommand.Execute("*");
            vm.EndPressedCommand.Execute(true);
            Assert.True(vm.LastPressedOperation == CommandWord.GetEnd);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.YearsStatusBarText?.Length == 0);
            vm.YearsPressedCommand.Execute(true);
            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);
            vm.ClearPressedCommand.Execute(true);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);

            Assert.True(vm.InterestStatusBarText?.Length == 0);
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);
            vm.ClearPressedCommand.Execute(true);
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);

            Assert.True(vm.StartStatusBarText?.Length == 0);
            vm.StartPressedCommand.Execute(true);
            Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);
            vm.ClearPressedCommand.Execute(true);
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);

            Assert.True(vm.RateStatusBarText?.Length == 0);
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

            Assert.True(vm.EndStatusBarText?.Length == 0);
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            Assert.True(FinCalcViewModel.ThousandsSeparator == ".");
            Assert.True(FinCalcViewModel.DecimalSeparator == ",");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            Assert.True(FinCalcViewModel.ThousandsSeparator == ",");
            Assert.True(FinCalcViewModel.DecimalSeparator == ".");
        }

        [Fact]
        public void CorrectStartupResultTextIsSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.DisplayText == "0,");
        }

        [Fact]
        public void NoSpecialFunctionPressedFlagsAreSetInFreshVm()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Years));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Interest));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Start));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Rate));
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.End));
        }

        #endregion

        #region General Tests

        [Fact]
        public void CommandsResetSecondFunctionTrigger_WhenExecuted()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Act
            vm.OperatorPressedCommand.Execute("*");
            vm.DigitPressedCommand.Execute(1);

            // Assert
            vm.SecondFunctionTrigger.Should().BeFalse();
        }

        [Fact]
        public void FlagsAreSetByEachSpecialFunction()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Initial function
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Initial function
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void InputAfterDecimalWasPressedIsHandledCorrectly()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.OperatorPressedCommand.Execute("+");
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void OperatorIsSet()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.True(vm.ActiveMathOperator == MathOperator.None);
            vm.OperatorPressedCommand.Execute("-");
            Assert.True(vm.ActiveMathOperator == MathOperator.Subtract);
        }

        [Fact]
        public void FirstNumberIsAssumedToBeZeroIfOperatorIsFirstInput()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);

            Assert.True(vm.DisplayText == "-2,");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);
        }

        [Fact]
        public void OperatorPressedAfterSpecialFunctionDoesNotReset()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("-");
            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-"); // same as calculate
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(2);
            vm.InterestPressedCommand.Execute(false);
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

            vm.DigitPressedCommand.Execute(2);
            vm.InterestPressedCommand.Execute(true);
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.DigitPressedCommand.Execute("1");
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void PressingZeroWhenLeftSideIsZeroDoesNotAddAnotherZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(9);

            Assert.True(vm.DisplayText == "9,");
            Assert.True(Math.Abs(vm.DisplayNumber - 9) < Tolerance);
        }

        [Fact]
        public void LeftSideDoesNotExceedInputLimit()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingCalculateResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.CalculatePressedCommand.Execute(null);
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void PressingCalculateAfterNumberWithOrWithoutActiveOperatorResetsStandardNumbers()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingCalculateAfterNumberWithActiveOperatorAssumesSecondNumberToBeZero()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(2);
            vm.OperatorPressedCommand.Execute("-");
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "2,"); // Second number is assumed to be 0 if there is no digit input after an operator.
            Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
        }

        [Fact]
        public void PressingCalculateAfterSpecialFunctionResetsStandardNumbers()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.CalculatePressedCommand.Execute(null);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(3);
            vm.InterestPressedCommand.Execute(false);
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

            vm.InterestPressedCommand.Execute(true);
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(1);
            vm.OperatorPressedCommand.Execute("/");
            vm.DigitPressedCommand.Execute(0);
            vm.CalculatePressedCommand.Execute(null);

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(n => n.GetType() == typeof(ErrorEvent)), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        #endregion

        #region Clear Tests

        [Fact]
        public void ClearingResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.ClearPressedCommand.Execute(false);
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void ClearingLongTouchResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.ClearPressedCommand.Execute(true);
            AssertVmStatusLabelsAreEmpty(vm, true);
        }

        [Fact]
        public void ClearingLongTouchResetsAdvance()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            vm.UseAnticipativeInterestYield.Should().BeTrue();
            vm.ClearPressedCommand.Execute(true);
            vm.UseAnticipativeInterestYield.Should().BeFalse();
        }

        [Fact]
        public void ClearingResetsStandardValues()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
        public void ClearingDoesNotResetSpecialFunctionMemory()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute("3");
            vm.YearsPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.InterestPressedCommand.Execute(false);
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
            vm.InterestPressedCommand.Execute(true);
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
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "4,074");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

            vm.ClearPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
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
        public void ClearingLongTouchResetsSpecialFunctionMemory()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute("3");
            vm.YearsPressedCommand.Execute(false);
            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute("3");
            vm.InterestPressedCommand.Execute(false);
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

            vm.InterestPressedCommand.Execute(true);
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
            vm.InterestPressedCommand.Execute(false);
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
            vm.InterestPressedCommand.Execute(true);
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
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.ClearPressedCommand.Execute(true);

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(n => n.GetType() == typeof(HintEvent)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void ClearingShortTouchDoesNotResetsFlags()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.AlgebSignCommand.Execute(null);
            AssertVmStatusLabelsAreEmpty(vm);
        }

        [Fact]
        public void AlgebSignIsShownAndUnShown()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(9);

            Assert.True(vm.DisplayText == "-9,");
            Assert.True(Math.Abs(vm.DisplayNumber - -9) < Tolerance);
        }

        [Fact]
        public void AlgebSignAfterSpecialFunctionIsCorrectlyFormatted()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "-1,000");
            Assert.True(Math.Abs(vm.DisplayNumber - -1) < Tolerance);

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
        public void AlgebSignPressedAfterSpecialFunctionDoesNotReset()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Short presses
            vm.DigitPressedCommand.Execute(2);
            vm.YearsPressedCommand.Execute(false);
            vm.AlgebSignCommand.Execute(null);
            Assert.True(vm.DisplayText == "-2,00");
            Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);

            vm.ClearPressedCommand.Execute(false);

            vm.DigitPressedCommand.Execute(3);
            vm.InterestPressedCommand.Execute(false);
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

            vm.InterestPressedCommand.Execute(true);
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.YearsPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            SetVmStatusLabelTexts(vm);
            vm.YearsPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingYearsSetsYearsValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(3);
            vm.YearsPressedCommand.Execute(false); // put a valid value to the memory that will be used to reset to

            vm.ClearPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-0,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -0.1) < Tolerance);

            vm.YearsPressedCommand.Execute(false);

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(m => ((ErrorEvent)m).ErrorMessage == LocalizedErrorMessages.Instance.YearsMustNotBeNegative()), It.IsAny<CancellationToken>()), Times.Once); // error expected
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
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(m => ((ErrorEvent)m).ErrorMessage == LocalizedErrorMessages.Instance.RatesPerAnnumExceedsRange()), It.IsAny<CancellationToken>()), Times.Once); // error expected
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

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(m => ((ErrorEvent)m).ErrorMessage == LocalizedErrorMessages.Instance.RatesPerAnnumExceedsRange()), It.IsAny<CancellationToken>()), Times.Exactly(2)); // error expected
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

            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(m => ((ErrorEvent)m).ErrorMessage == LocalizedErrorMessages.Instance.RatesPerAnnumExceedsRange()), It.IsAny<CancellationToken>()), Times.Exactly(3)); // error expected
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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.YearsPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingYearsShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.YearsPressedCommand.Execute(false);

            Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);
        }

        [Fact]
        public void YearsSecondFunctionDoesNotSetYearsFlag()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Act
            vm.DigitPressedCommand.Execute(1);
            vm.OperatorPressedCommand.Execute("*");
            vm.YearsPressedCommand.Execute(true);

            // Assert
            Assert.False(vm.PressedSpecialFunctions.HasFlag(PressedSpecialFunctions.Years));
        }

        #endregion

        #region Interest Tests

        [Fact]
        public void PressingInterestResetsStatusLabelTexts()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.InterestPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            SetVmStatusLabelTexts(vm);
            vm.InterestPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingInterestAlsoCalculatesNominalInterestRate()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(4);
            vm.InterestPressedCommand.Execute(false);
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,928");
            Assert.True(Math.Abs(vm.DisplayNumber - 3.928487739) < Tolerance);
        }

        [Fact]
        public void PressingNominalInterestAlsoCalculatesEffectiveInterestRateButShowsEffectiveInterestRate()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(false);
            Assert.True(vm.DisplayText == "4,074");
            Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "4,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
        }

        [Fact]
        public void EffectiveAndNominalInterestDoNotExceedLimitsAndResets()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(3);
            vm.InterestPressedCommand.Execute(false); // put a valid value to the memory

            vm.ClearPressedCommand.Execute(false);

            vm.AlgebSignCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            vm.DigitPressedCommand.Execute(0);
            vm.DigitPressedCommand.Execute(0);
            vm.DecimalSeparatorPressedCommand.Execute(null);
            vm.DigitPressedCommand.Execute(1);
            Assert.True(vm.DisplayText == "-100,1");
            Assert.True(Math.Abs(vm.DisplayNumber - -100.1) < Tolerance);

            vm.InterestPressedCommand.Execute(false);

            mediatorMock.Verify(x => x.Publish<INotification>(It.Is<ErrorEvent>(m => m.ErrorMessage == LocalizedErrorMessages.Instance.EffectiveInterestExceedsRange()), It.IsAny<CancellationToken>()), Times.Once); // error expected
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "3,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset

            vm.ClearPressedCommand.Execute(true); // reset for tests with second function

            vm.DigitPressedCommand.Execute(4);
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(false); // put a valid value to the memory

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
            vm.InterestPressedCommand.Execute(false);

            mediatorMock.Verify(x => x.Publish<INotification>(It.Is<ErrorEvent>(m => m.ErrorMessage == LocalizedErrorMessages.Instance.NominalInterestExceedsRange()), It.IsAny<CancellationToken>()), Times.Exactly(1)); // Not 2 because it is another error message
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
            vm.OperatorPressedCommand.Execute("*");
            vm.InterestPressedCommand.Execute(true);
            Assert.True(vm.DisplayText == "4,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance); // sides are reset
        }

        [Fact]
        public void PressingInterestShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.InterestPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,000");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingInterestShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.StartPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.EndStatusBarText)); // An other label than the one belonging to the command.
            SetVmStatusLabelTexts(vm);
            vm.StartPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.EndStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingStartSetsStartValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            Assert.False(vm.UseAnticipativeInterestYield);
            Assert.True(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));
            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.True(vm.UseAnticipativeInterestYield);
            Assert.False(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));

            vm.OperatorPressedCommand.Execute("*");
            vm.StartPressedCommand.Execute(false);
            Assert.False(vm.UseAnticipativeInterestYield);
            Assert.True(string.IsNullOrWhiteSpace(vm.AdvanceStatusBarText));
        }

        [Fact]
        public void PressingStartShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.StartPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingStartShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.RatePressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            SetVmStatusLabelTexts(vm);
            vm.RatePressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingRateSetsRateValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
        public void PressingRateAlsoCalculatesRepaymentRate()
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
        }

        [Fact]
        public void PressingRepaymentRateAlsoCalculatesRepaymentButShowsRepaymentRate()
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
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(0);
            vm.RatePressedCommand.Execute(false); // Would throw div by zero, because repayment rate is calculated in the background with invalid result.
            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(n => n.GetType() == typeof(ErrorEvent)), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void PressingRateSecondFunctionLongTouchThrowsWhenRepaymentRateIsRecalculatedAndIsInvalidNumber()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var mediatorMock = Mock.Get((IMediator)mockObjects[nameof(IMediator)]);
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(n => n.GetType() == typeof(ErrorEvent)), It.IsAny<CancellationToken>()), Times.Once);

            // Assert display is set back to zero and not NaN or something
            Assert.True(vm.LastPressedOperation == CommandWord.Clear);
            Assert.True(vm.DisplayText == "0,");
            Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
        }

        [Fact]
        public void PressingRateShouldDisplaySpecialOutputText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.RatePressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingRateShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            SetVmStatusLabelTexts(vm);
            vm.EndPressedCommand.Execute(true);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
            SetVmStatusLabelTexts(vm);
            vm.EndPressedCommand.Execute(false);
            Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
        }

        [Fact]
        public void PressingEndSetsEndValue()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // No operator set but last input is a digit
            vm.DigitPressedCommand.Execute(2);
            vm.EndPressedCommand.Execute(false);

            Assert.False(vm.LastPressedOperation == CommandWord.PercentCalculation);
            vm.ClearPressedCommand.Execute(true);

            // Operator set but last input is a not a digit
            vm.OperatorPressedCommand.Execute("-");
            vm.EndPressedCommand.Execute(false);

            Assert.False(vm.LastPressedOperation == CommandWord.PercentCalculation);
            vm.ClearPressedCommand.Execute(true);
        }

        [Fact]
        public void PercentCalculationDoesNotSetEndFlag()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            Assert.True(vm.DisplayText == "5,00");
            Assert.True(Math.Abs(vm.DisplayNumber - 5) < Tolerance);
        }

        [Fact]
        public void PressingEndShouldSetRateLabelText()
        {
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            vm.DigitPressedCommand.Execute(5);
            vm.EndPressedCommand.Execute(false);

            Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
        }

        #endregion

        private static void SetVmStatusLabelTexts(FinCalcViewModel vm)
        {
            vm.AdvanceStatusBarText = "test";
            vm.YearsStatusBarText = "test";
            vm.InterestStatusBarText = "test";
            vm.StartStatusBarText = "test";
            vm.RateStatusBarText = "test";
            vm.EndStatusBarText = "test";
        }

        private static void AssertVmStatusLabelsAreEmpty(FinCalcViewModel vm, bool checkAdvanceStatusBarTextToo = false)
        {
            if (checkAdvanceStatusBarTextToo)
            {
                Assert.True(vm.AdvanceStatusBarText?.Length == 0);
            }
            else
            {
                Assert.True(vm.AdvanceStatusBarText == "test");
            }

            Assert.True(vm.YearsStatusBarText?.Length == 0);
            Assert.True(vm.InterestStatusBarText?.Length == 0);
            Assert.True(vm.StartStatusBarText?.Length == 0);
            Assert.True(vm.RateStatusBarText?.Length == 0);
            Assert.True(vm.EndStatusBarText?.Length == 0);
        }
    }
}
