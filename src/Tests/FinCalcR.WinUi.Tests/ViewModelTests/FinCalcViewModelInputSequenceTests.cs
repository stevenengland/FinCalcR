using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
	public class FinCalcViewModelInputSequenceTests
	{
		private const double Tolerance = 0.00000001;

		public FinCalcViewModelInputSequenceTests()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
		}

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

			vm.ClearPressedCommand.Execute(null);

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
	}
}
