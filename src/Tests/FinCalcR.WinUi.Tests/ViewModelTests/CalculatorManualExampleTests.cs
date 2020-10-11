using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.Common.Extensions;
using StEn.FinCalcR.WinUi.Types;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
	public class CalculatorManualExampleTests
	{
		private const double Tolerance = 0.00000001;

		public CalculatorManualExampleTests()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
		}

		#region Second Function Examples

		[Fact]
		public void InterestSecondFunctionExample()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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

		#region Loan Examples

		[Fact]
		public void LoanQuestion1Example()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);

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

			Assert.True(vm.DisplayText == "-113187,55");
			Assert.True(Math.Abs(vm.DisplayNumber - -113187.5488186329) < Tolerance);
			Assert.True(Math.Abs(vm.EndNumber - -113187.5488186329) < Tolerance);
		}

		#endregion

	}
}
