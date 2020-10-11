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
	}
}
