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
		public async Task InterestSecondFunctionExampleAsync()
		{
			var mockObjects = MockFactories.GetMockObjects();
			var gestureHandlerMock = new Mock<IGestureHandler>();
			var vm = MockFactories.FinCalcViewModelFactory(mockObjects);
			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(false);

			vm.DigitPressedCommand.Execute("4");
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,074");
			Assert.True(Math.Abs(vm.DisplayNumber - 4.074154292) < Tolerance);
			Assert.True(Math.Abs(vm.InterestNumber - 4.074154292) < Tolerance);

			gestureHandlerMock.Setup(x => x.IsLongTouchAsync(It.IsAny<TimeSpan>())).ReturnsAsync(true);
			vm.OperatorPressedCommand.Execute("*");
			await vm.InterestPressedCommand.ExecuteAsync(gestureHandlerMock.Object);
			Assert.True(vm.DisplayText == "4,000");
			Assert.True(Math.Abs(vm.DisplayNumber - 4) < Tolerance);
			Assert.True(Math.Abs(vm.NominalInterestRateNumber - 4) < Tolerance);
		}

		#endregion
	}
}
