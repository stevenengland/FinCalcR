using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.WinUi.Types;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1651:Do not use placeholder elements", Justification = "Tests do not care")]
	public class InputSequenceTests
	{
		private const double Tolerance = 0.00000001;

		public InputSequenceTests()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
		}

		#region Basic Arithmetics

		[Fact]
		public void AddingNumbersSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("+");
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "22,");
			Assert.True(Math.Abs(vm.DisplayNumber - 22) < Tolerance);
		}

		[Fact]
		public void SubstractingNumbersSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "18,");
			Assert.True(Math.Abs(vm.DisplayNumber - 18) < Tolerance);
		}

		[Fact]
		public void DividingNumbersSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("/");
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "10,");
			Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);
		}

		[Fact]
		public void MultiplyingNumbersSucceeds()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("*");
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "40,");
			Assert.True(Math.Abs(vm.DisplayNumber - 40) < Tolerance);
		}

		#endregion

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

		/// <summary>
		/// 3 - 1 = 2
		/// Clear
		/// 5 + 3 = 8.
		/// </summary>
		[Fact]
		public void Calculation_Clearing_Calculation()
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

			vm.DigitPressedCommand.Execute("5");
			vm.OperatorPressedCommand.Execute("+");
			vm.DigitPressedCommand.Execute("3");
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "8,");
			Assert.True(Math.Abs(vm.DisplayNumber - 8) < Tolerance);
		}

		/// <summary>
		/// A Calculation is performed.
		/// A Digit is entered.
		/// The result text is the digit.
		/// 2 * 6 = 12
		/// 1.
		/// </summary>
		[Fact]
		public void Digit_Operator_Digit_Calculate_Digit()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute("2");
			vm.OperatorPressedCommand.Execute("*");
			vm.DigitPressedCommand.Execute("6");
			vm.CalculatePressedCommand.Execute(null);

			vm.DigitPressedCommand.Execute("1");

			Assert.True(vm.DisplayText == "1,");
			Assert.True(Math.Abs(vm.DisplayNumber - 1) < Tolerance);
		}

		/// <summary>
		/// A Digit, an operator and a digit are pressed. Instead of pressing Calculation a further operator is pressed.
		/// A Digit and then the calculation command is pressed.
		/// 2 * 6 - // here the result of the calculation should be visible because of the second operator that was pressed.
		/// 4 *     // here the result of the calculation should be visible because of the second operator that was pressed.
		/// 2 = 16.
		/// </summary>
		[Fact]
		public void Digit_Operator_Digit_Operator_Digit_Calculate()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute("2");
			vm.OperatorPressedCommand.Execute("*");
			vm.DigitPressedCommand.Execute("6");
			vm.OperatorPressedCommand.Execute("-");

			Assert.True(vm.DisplayText == "12,");
			Assert.True(Math.Abs(vm.DisplayNumber - 12) < Tolerance);

			vm.DigitPressedCommand.Execute("4");
			vm.OperatorPressedCommand.Execute("*");

			Assert.True(vm.DisplayText == "8,");
			Assert.True(Math.Abs(vm.DisplayNumber - 8) < Tolerance);

			vm.DigitPressedCommand.Execute("2");
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "16,");
			Assert.True(Math.Abs(vm.DisplayNumber - 16) < Tolerance);
		}

		/// <summary>
		/// A calculation is triggered that throws.
		/// Another calculation should not be harmed by this because of a reset after a throw.
		/// </summary>
		[Fact]
		public void Calculation_Throw_Calculation()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.DigitPressedCommand.Execute(1);
			vm.OperatorPressedCommand.Execute("/");
			vm.DigitPressedCommand.Execute(0);
			vm.CalculatePressedCommand.Execute(null);

			vm.DigitPressedCommand.Execute(2);
			vm.OperatorPressedCommand.Execute("*");
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "4,");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
		}

		/// <summary>
		/// First input is a operator. So the first Number is 0.
		/// Next input is a operator followed by a digit.
		/// </summary>
		[Fact]
		public void Tc0001()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute("3");
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "-3,");
			Assert.True(Math.Abs(vm.DisplayNumber - (-3)) < Tolerance);
		}

		/// <summary>
		/// Produces a fraction with more fractional digits than allowed.
		/// Fractional digits are rounded.
		/// </summary>
		[Fact]
		public void Tc0002()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

			// 123,12 / 22 = 5,5963636363636363636363636363636
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(3);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(2);
			vm.OperatorPressedCommand.Execute("/");
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(2);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "5,596363636");
			Assert.True(Math.Abs(vm.DisplayNumber - 5.596363636) < Tolerance);
		}

		#region Focus Interest

		[Fact]
		public async Task DecimalPlacesAreFilledCorrectlyAfterInterestButtonWasPressedAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(1);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 1) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,100");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.1) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(1);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,010");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.01) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(1);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,001");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.001) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(1);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.0001) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(5);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,001");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.0005) < Tolerance);

			vm.ClearPressedCommand.Execute(false);

			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(9);
			vm.DigitPressedCommand.Execute(5);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "1,010");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.0095) < Tolerance);
		}

		/// <summary>
		/// A subtraction is performed. Instead of = the interest button is pressed. The last number goes into memory without performing the calculation.
		/// After that another subtraction is performed. The interest value is used as first number.
		/// 2 - 9 Interest // 9.000
		/// - 6 =          // 3.
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0001Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(2);
			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute(9);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			Assert.True(vm.DisplayText == "9,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 9) < Tolerance);

			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute(6);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "3,");
			Assert.True(Math.Abs(vm.DisplayNumber - 3) < Tolerance);
		}

		/// <summary>
		/// Input sequence contains more decimal digits than the number that would be displayed. Internally it is saved completely.
		/// A calculation uses the last displayed number (interest) as first number for the calculation.
		/// 0,123456 Interest   // 0,123
		/// + 1 =				// 1,123456
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0002Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(0);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(3);
			vm.DigitPressedCommand.Execute(4);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(6);

			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			Assert.True(vm.DisplayText == "0,123");
			Assert.True(Math.Abs(vm.DisplayNumber - 0.123456) < Tolerance);

			vm.OperatorPressedCommand.Execute("+");
			vm.DigitPressedCommand.Execute(1);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "1,123456");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.123456) < Tolerance);
		}

		/// <summary>
		/// The interest value is loaded from the memory (5).
		/// A value is subtracted. The result is a calculation with interest as first number.
		/// 5 Interest
		/// Interest^ - 3 = // 2,
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0003Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(5);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute(3);
			vm.CalculatePressedCommand.Execute(null);

			Assert.True(vm.DisplayText == "2,");
			Assert.True(Math.Abs(vm.DisplayNumber - 2) < Tolerance);
		}

		/// <summary>
		/// A digit is entered followed by an operator. Then the interest button is pressed.
		/// 6 - Interest // 6.000 is taken as interest number.
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0004Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(6);
			vm.OperatorPressedCommand.Execute("-");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			Assert.True(vm.DisplayText == "6,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 6) < Tolerance);
		}

		/// <summary>
		/// A digit is added to another followed by an operator. Then the interest button is pressed.
		/// 2 + 2 - Interest // 4.000 is taken as interest number.
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0005Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(2);
			vm.OperatorPressedCommand.Execute("+");
			vm.DigitPressedCommand.Execute(2);
			vm.OperatorPressedCommand.Execute("-");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);

			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
		}

		/// <summary>
		/// A digit is set as interest.
		/// A decimal and a number follows.
		/// 5 Interest // 5,000
		/// ,9         // 0,9
		/// </summary>
		/// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
		[Fact]
		public async Task Tc_Interest_0006Async()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			var gestureHandlerMock = new Mock<IGestureHandler>();
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute(5);
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(9);

			Assert.True(vm.DisplayText == "0,9");
			Assert.True(Math.Abs(vm.DisplayNumber - 0.9) < Tolerance);
		}

		#endregion

	}
}
