using System;
using System.Globalization;
using System.Threading;
using FinCalcR.WinUi.Tests.Mocks;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
	public class CalculatorManualExampleTests
	{
		private const double Tolerance = 0.01;

		public CalculatorManualExampleTests()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
		}

		#region Percentage Calculation

		[Fact]
		public void PercentageCalculationExamples()
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
			Assert.True(vm.DisplayText == "10,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 10) < Tolerance);

			// 200 + 5
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("+");
			vm.DigitPressedCommand.Execute(5);
			vm.EndPressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "210,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 210) < Tolerance);

			// 200 - 5
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("-");
			vm.DigitPressedCommand.Execute(5);
			vm.EndPressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "190,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 190) < Tolerance);

			// 200 / 5 <-- not documented and not very useful
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.OperatorPressedCommand.Execute("/");
			vm.DigitPressedCommand.Execute(5);
			vm.EndPressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "4.000,00");
			Assert.True(Math.Abs(vm.DisplayNumber - 4000) < Tolerance);
		}

		#endregion

		#region Second Function Examples

		[Fact]
		public void InterestSecondFunctionExample()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.DigitPressedCommand.Execute("4");
			vm.OperatorPressedCommand.Execute("*");
			vm.InterestPressedCommand.Execute(false);
			Assert.True(vm.DisplayText == "4,074");
			Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);
			Assert.True(Math.Abs(vm.InterestNumber - 4.074154292) < Tolerance);

			vm.OperatorPressedCommand.Execute("*");
			vm.InterestPressedCommand.Execute(true);
			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
			Assert.True(Math.Abs(vm.NominalInterestRateNumber - 4) < Tolerance);
		}

		#endregion

		#region N Examples

		[Fact]
		public void N_BookP32()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.DigitPressedCommand.Execute(5);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(5);
			vm.InterestPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.AlgebSignCommand.Execute(null);
			vm.StartPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(0);
			vm.AlgebSignCommand.Execute(null);
			vm.RatePressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.EndPressedCommand.Execute(false);
			vm.YearsPressedCommand.Execute(false);

			Assert.True(vm.DisplayText == "39,85");
			Assert.True(Math.Abs(vm.DisplayNumber - 39.85) < Tolerance);
			Assert.True(Math.Abs(vm.YearsNumber - 39.85) < Tolerance);
		}

		#endregion

		#region Kn Examples

		[Fact]
		public void Kn_Manual_LoanQuestion1()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

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

		#endregion

		#region K0 Examples

		[Fact]
		public void K0_BookP31()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.DigitPressedCommand.Execute(3);
			vm.DigitPressedCommand.Execute(5);
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(5);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(5);
			vm.InterestPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(0);
			vm.AlgebSignCommand.Execute(null);
			vm.RatePressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.EndPressedCommand.Execute(false);
			vm.StartPressedCommand.Execute(false);

			Assert.True(vm.DisplayText == "-49.406,13");
			Assert.True(Math.Abs(vm.DisplayNumber - -49406.13) < Tolerance);
			Assert.True(Math.Abs(vm.StartNumber - -49406.13) < Tolerance);
		}

		#endregion

		#region E Examples

		[Fact]
		public void E_Manual_IncomeDrawDownQuestion2()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.OperatorPressedCommand.Execute("*");
			vm.StartPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.OperatorPressedCommand.Execute("*");
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(5);
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.DecimalSeparatorPressedCommand.Execute(null);
			vm.DigitPressedCommand.Execute(4);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(4);
			vm.DigitPressedCommand.Execute(6);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(7);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(9);
			vm.InterestPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.AlgebSignCommand.Execute(null);
			vm.StartPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(0);
			vm.EndPressedCommand.Execute(false);
			vm.RatePressedCommand.Execute(false);

			Assert.True(vm.DisplayText == "11.827,95");
			Assert.True(Math.Abs(vm.DisplayNumber - 11827.95) < Tolerance);
			Assert.True(Math.Abs(vm.RateNumber - 11827.95) < Tolerance);
		}

		[Fact]
		public void E_BookP17()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(5);
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(7);
			vm.InterestPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(0);
			vm.StartPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.EndPressedCommand.Execute(false);
			vm.RatePressedCommand.Execute(false);

			Assert.True(vm.DisplayText == "-255,41");
			Assert.True(Math.Abs(vm.DisplayNumber - -255.41) < Tolerance);
			Assert.True(Math.Abs(vm.RateNumber - -255.41) < Tolerance);
		}

		#endregion

		#region P Examples

		[Fact]
		public void P_BookP21()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

			vm.DigitPressedCommand.Execute(1);
			vm.OperatorPressedCommand.Execute("*");
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(5);
			vm.YearsPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(5);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.AlgebSignCommand.Execute(null);
			vm.StartPressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(1);
			vm.DigitPressedCommand.Execute(2);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.DigitPressedCommand.Execute(0);
			vm.RatePressedCommand.Execute(false);
			vm.DigitPressedCommand.Execute(0);
			vm.EndPressedCommand.Execute(false);
			vm.InterestPressedCommand.Execute(false);

			Assert.True(vm.DisplayText == "1,455");
			Assert.True(Math.Abs(vm.DisplayNumber - 1.455) < Tolerance);
			Assert.True(Math.Abs(vm.InterestNumber - 1.455) < Tolerance);
			Assert.True(Math.Abs(vm.NominalInterestRateNumber - 1.455) < Tolerance);
		}

		#endregion

	}
}
