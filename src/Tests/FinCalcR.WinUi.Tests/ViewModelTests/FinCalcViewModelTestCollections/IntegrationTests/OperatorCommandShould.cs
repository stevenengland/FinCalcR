using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class OperatorCommandShould
    {
        public OperatorCommandShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        [Fact]
        public void ActivateSecondFunctionTrigger_WhenOperatorIsAdd()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModel2WithCalculatorImplementationFactory(mockObjects);

            // Act
            vm.OperatorPressedCommand.Execute("*");

            // Assert
            vm.SecondFunctionTrigger.Should().BeTrue();
        }
    }
}
