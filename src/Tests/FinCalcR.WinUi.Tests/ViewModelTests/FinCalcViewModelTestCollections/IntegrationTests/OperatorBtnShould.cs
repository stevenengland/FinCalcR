using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class OperatorBtnShould : TestBase
    {
        [Fact]
        public void ActivateSecondFunctionTrigger_WhenActiveOperatorIsAdd()
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Act
            vm.OperatorPressedCommand.Execute("*");

            // Assert
            vm.SecondFunctionTrigger.Should().BeTrue();
        }
    }
}
