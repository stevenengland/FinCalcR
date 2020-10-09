using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.Common.Services.Localization;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Types;
using StEn.FinCalcR.WinUi.ViewModels;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
	public class FinCalcViewModelTests
	{
		private const double Tolerance = 0.00000001;

		public FinCalcViewModelTests()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
		}

		[Fact]
		public async Task LastPressedOperationIsSetAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
			vm.LastPressedOperation = LastPressedOperation.None;
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.LastPressedOperation == LastPressedOperation.Interest);
			vm.LastPressedOperation = LastPressedOperation.None;
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
		}

		[Fact]
		public async Task StatusBarTextsAreSetAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			Assert.True(vm.YearsStatusBarText == string.Empty);
			vm.YearsPressedCommand.Execute(true);
			Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);
			vm.YearsStatusBarText = string.Empty;
			vm.YearsPressedCommand.Execute(false);
			Assert.True(vm.YearsStatusBarText == Resources.FinCalcFunctionYears);

			Assert.True(vm.InterestStatusBarText == string.Empty);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);
			vm.InterestStatusBarText = string.Empty;
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.InterestStatusBarText == Resources.FinCalcFunctionInterest);

			Assert.True(vm.StartStatusBarText == string.Empty);
			vm.StartPressedCommand.Execute(true);
			Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);
			vm.StartStatusBarText = string.Empty;
			vm.StartPressedCommand.Execute(false);
			Assert.True(vm.StartStatusBarText == Resources.FinCalcFunctionStart);

			Assert.True(vm.RateStatusBarText == string.Empty);
			vm.RatePressedCommand.Execute(true);
			Assert.True(vm.RateStatusBarText == Resources.FinCalcFunctionRate);
			vm.RateStatusBarText = string.Empty;
			vm.RatePressedCommand.Execute(false);
			Assert.True(vm.RateStatusBarText == Resources.FinCalcFunctionRate);

			Assert.True(vm.EndStatusBarText == string.Empty);
			vm.EndPressedCommand.Execute(true);
			Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
			vm.EndStatusBarText = string.Empty;
			vm.EndPressedCommand.Execute(false);
			Assert.True(vm.EndStatusBarText == Resources.FinCalcFunctionEnd);
		}

		#region Initialization Tests

		[Fact]
		public void CorrectSeparatorsAreUsedForEachLanguage()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			Assert.True(vm.DisplayText == "0,");
		}

		#endregion

		#region Decimal Separator Tests

		[Fact]
		public void PressingDecimalSeparatorResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void RightSideGetsActivated()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute("4");

			Assert.True(vm.DisplayText == "0,4");
			Assert.True(Math.Abs(vm.DisplayNumber - 0.4) < Tolerance);
		}

		#endregion

		#region Math Operator Tests

		[Fact]
		public void PressingMathOperatorResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.OperatorPressedCommand.Execute("+");
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void OperatorIsSet()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			Assert.True(vm.ActiveMathOperator == string.Empty);
			vm.OperatorPressedCommand.Execute("-");
			Assert.True(vm.ActiveMathOperator == "-");
		}

		[Fact]
		public void FirstNumberIsAssumedToBeZeroIfOperatorIsFirstInput()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.DigitPressedCommand.Execute("1");
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void PressingZeroWhenLeftSideIsZeroDoesNotAddAnotherZero()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(9);

			Assert.True(vm.DisplayText == "9,");
			Assert.True(Math.Abs(vm.DisplayNumber - 9) < Tolerance);
		}

		[Fact]
		public void LeftSideDoesNotExceedInputLimit()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			for (var i = 1; i < 20; i++)
			{
				vm.DigitPressedCommand.Execute(1);
			}

			Assert.True(vm.DisplayText == "1111111111,");
			Assert.True(Math.Abs(vm.DisplayNumber - 1111111111) < Tolerance);
		}

		[Fact]
		public void RightSideDoesNotExceedInputLimit()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
		public void PressingCalculateResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.CalculatePressedCommand.Execute(null);
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void PressingCalculateAfterNumberWithOrWithoutActiveOperatorResetsStandardNumbers()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);
			Assert.True(vm.DisplayText == "0,");
			Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
		}

		[Fact]
		public void PressingCalculateAfterNumberWithActiveOperatorAssumesSecondNumberToBeZero()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.ClearPressedCommand.Execute(false);
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void ClearingLongTouchResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.ClearPressedCommand.Execute(true);
			this.AssertVmStatusLabelsAreEmpty(vm, true);
		}

		[Fact]
		public void ClearingResetsStandardValues()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute("3");
			vm.YearsPressedCommand.Execute(false);
			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute("3");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,074");
			Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

			vm.ClearPressedCommand.Execute(false);
			vm.OperatorPressedCommand.Execute("*");
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
		}

		[Fact]
		public async Task ClearingLongTouchResetsSpecialFunctionMemoryAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute("3");
			vm.YearsPressedCommand.Execute(false);
			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute("3");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			vm.ClearPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(4);
			vm.OperatorPressedCommand.Execute("*");
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,074");
			Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

			vm.ClearPressedCommand.Execute(true);
			vm.OperatorPressedCommand.Execute("*");
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "0,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance);
		}

		[Fact]
		public void ClearingLongTouchShowsResetHint()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.ClearPressedCommand.Execute(true);

			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<HintEvent>(), It.IsAny<Action<System.Action>>()), Times.Once);
		}

		#endregion

		#region Algeb Sign Tests

		[Fact]
		public void AlgebSignResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			this.SetVmStatusLabelTexts(vm);
			vm.AlgebSignCommand.Execute(null);
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void AlgebSignIsShownAndUnShown()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.AlgebSignCommand.Execute(null);
			vm.DigitPressedCommand.Execute(9);

			Assert.True(vm.DisplayText == "-9,");
			Assert.True(Math.Abs(vm.DisplayNumber - -9) < Tolerance);
		}

		[Fact]
		public void AlgebSignAfterSpecialFunctionIsCorrectlyFormatted()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(2);
			vm.YearsPressedCommand.Execute(false);
			vm.AlgebSignCommand.Execute(null);
			Assert.True(vm.DisplayText == "-2,00"); // not -2,
			Assert.True(Math.Abs(vm.DisplayNumber - -2) < Tolerance);
		}

		[Fact]
		public async Task AlgebSignPressedAfterSpecialFunctionDoesNotResetAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(3);
			vm.YearsPressedCommand.Execute(false); // put a valid value to the memory

			vm.ClearPressedCommand.Execute(false);

			// negative values
			vm.AlgebSignCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			Assert.True(vm.DisplayText == "-1,");
			Assert.True(Math.Abs(vm.DisplayNumber - -1) < Tolerance);

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

		#endregion

		#region Interest Tests

		[Fact]
		public async Task PressingInterestResetsStatusLabelTextsAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			this.SetVmStatusLabelTexts(vm);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
			this.SetVmStatusLabelTexts(vm);
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(string.IsNullOrWhiteSpace(vm.StartStatusBarText)); // An other label than the one belonging to the command.
		}

		[Fact]
		public async Task PressingInterestAlsoCalculatesNominalInterestRateAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(4);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "3,928");
			Assert.True(Math.Abs(vm.DisplayNumber - 3.928487739) < Tolerance);
		}

		[Fact]
		public async Task PressingNominalInterestAlsoCalculatesEffectiveInterestRateButShowsEffectiveInterestRateAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(4);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,074");
			Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);

			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
		}

		[Fact]
		public async Task InterestDoesNotExceedLimitsAndResetsAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var eventAggregatorMock = Mock.Get((IEventAggregator)mockObjects[nameof(IEventAggregator)]);
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(3);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object); // put a valid value to the memory

			vm.ClearPressedCommand.Execute(false);

			vm.AlgebSignCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			Assert.True(vm.DisplayText == "-100,1");
			Assert.True(Math.Abs(vm.DisplayNumber - -100.1) < Tolerance);

			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Once); // error expected
			Assert.True(vm.DisplayText == "0,");
			Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "3,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance); // sides are reset

			vm.ClearPressedCommand.Execute(true); // reset for tests with second function

			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);
			vm.DigitPressedCommand.Execute(4);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object); // put a valid value to the memory

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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			eventAggregatorMock.Verify(x => x.Publish(It.IsAny<ErrorEvent>(), It.IsAny<Action<System.Action>>()), Times.Exactly(2)); // 1 + 1 from above
			Assert.True(vm.DisplayText == "0,");
			Assert.True(Math.Abs(vm.DisplayNumber - 0) < Tolerance); // sides are reset
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance); // sides are reset
		}

		#endregion

		#region Start Tests

		[Fact]
		public void PressingStartResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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

		#endregion

		#region Rate Tests

		[Fact]
		public void PressingRateResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(2);
			vm.RatePressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "2,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
			vm.ClearPressedCommand.Execute(false);
			vm.RatePressedCommand.Execute(true);
			Assert.True(vm.DisplayText == "2,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
		}

		#endregion

		#region End Tests

		[Fact]
		public void PressingEndResetsStatusLabelTexts()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(2);
			vm.EndPressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "2,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
			vm.ClearPressedCommand.Execute(false);
			vm.EndPressedCommand.Execute(true);
			Assert.True(vm.DisplayText == "2,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
		}

		#endregion

		private void SetVmStatusLabelTexts(FinCalcViewModel vm)
		{
			vm.AdvanceStatusBarText = "test";
			vm.YearsStatusBarText = "test";
			vm.InterestStatusBarText = "test";
			vm.StartStatusBarText = "test";
			vm.RateStatusBarText = "test";
			vm.EndStatusBarText = "test";
		}

		private void AssertVmStatusLabelsAreEmpty(FinCalcViewModel vm, bool checkAdvanceStatusBarTextToo = false)
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
