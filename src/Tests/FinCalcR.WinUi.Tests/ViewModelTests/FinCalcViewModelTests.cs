using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
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
			vm.ClearPressedCommand.Execute(null);
			this.AssertVmStatusLabelsAreEmpty(vm);
		}

		[Fact]
		public void ClearingResetsValuesExceptSpecialFunctionMemory()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute("3");
			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute("1");
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "2,");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);

			vm.ClearPressedCommand.Execute(null);

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
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			vm.ClearPressedCommand.Execute(null);

			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			Assert.True(vm.DisplayText == "3,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);
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

			Assert.True(vm.YearsStatusBarText == string.Empty);
			Assert.True(vm.InterestStatusBarText == string.Empty);
			Assert.True(vm.StartStatusBarText == string.Empty);
			Assert.True(vm.RateStatusBarText == string.Empty);
			Assert.True(vm.EndStatusBarText == string.Empty);
		}
	}
}
