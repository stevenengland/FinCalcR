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
