using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "MemberData needs public property")]
    public class InputSequencesShould
    {
        public InputSequencesShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
        }

        public static TheoryData<Ca[], string> InputSequences =>
            new TheoryData<Ca[], string>()
            {
                { new[] { Ca.Nr1, Ca.Dec, Ca.Dec, Ca.Nr2 }, "1,2" },
            };

        [Theory]
        [MemberData(nameof(InputSequences))]
        public void ShowExpectedOutputText_GivenVariousInputSequences(Ca[] operations, string expectedOutputTextAfterAllOperations)
        {
            // Arrange
            var mockObjects = MockFactories.GetMockObjects();
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(mockObjects);

            // Act
            FinCalcViewModelHelper.ExecuteDummyOperations(vm, operations);

            // Assert
            vm.DisplayText.Should().Be(expectedOutputTextAfterAllOperations);
        }
    }
}
