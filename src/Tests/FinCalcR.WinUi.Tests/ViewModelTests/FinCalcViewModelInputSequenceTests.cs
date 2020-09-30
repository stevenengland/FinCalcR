using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
	}
}
