using System.Globalization;
using System.Threading;
using FinCalcR.WinUi.Tests.Mocks;
using Moq;
using StEn.FinCalcR.Common.Services.Localization;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
	public class FinCalcViewModelTests
	{
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
		public void RightSideGetsActivated()
		{

		}

		#endregion

		#region Math Operator Tests

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
		public void PressingZeroWhenLeftSideIsZeroDoesNotAddAnotherZero()
		{

		}

		#endregion
	}
}
