using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests
{
    public class AboutViewModelShould
    {
        [Fact]
        public void HaveSetVersionText_WhenVmIsLoaded()
        {
            // Arrange
            var vm = MockFactories.AboutViewModelFactory(out _);

            // Act
            // Assert
            vm.AppVersionText.Should().NotBeNullOrWhiteSpace();
        }
    }
}
