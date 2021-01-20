using System.Collections.Generic;
using System.Threading;
using FinCalcR.WinUi.Tests.Mocks;
using FluentAssertions;
using MediatR;
using Moq;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.WinUi.Events;
using Xunit;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public class RateBtnShould : TestBase
    {
        [Fact]
        public void UpdateRepaymentRate_WhenPressedLong()
        {
            var testSequences = new List<(string, Ca[])>
            {
                // Set dependent numbers so that repayment rate can be calculated and get repayment rate from memory.
                ("0,74", new[] { Ca.Nr1, Ca.Nr0, Ca.SetYears, Ca.Nr2, Ca.SetInt, Ca.Nr1, Ca.Nr5, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.Nr34, Ca.Nr0, Ca.Alg, Ca.SetRate, Ca.GetRep }),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testSequences);
        }

        [Fact]
        public void NotUpdateRepayment_WhenLongPressed()
        {
            var testSequences = new List<(string, Ca[])>
            {
                // Set dependent numbers so that repayment rate can be calculated and get repayment rate from memory.
                ("0,74", new[] { Ca.Nr1, Ca.Nr0, Ca.SetYears, Ca.Nr2, Ca.SetInt, Ca.Nr1, Ca.Nr5, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.Nr34, Ca.Nr0, Ca.Alg, Ca.SetRate, Ca.GetRep }),

                // Change a dependent value and see that rate did not change.
                ("-340,00", new[] { Ca.Nr1, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.Nr0, Ca.SetStart, Ca.GetRate }),

                // See that repayment rate changed.
                ("2,10", new[] { Ca.GetRep }),
            };

            FinCalcViewModelHelper.ExecuteDummyActionsAndCheckOutput(testSequences);
        }

        [Fact]
        public void TriggerCalculationOfRepaymentRate_ButFailsAndResets_WhenCalculationIsNotPossible()
        {
            // Arrange
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(out var autoMocker);
            var mediatorMock = autoMocker.GetMock<IMediator>();

            // Act
            // Produce NaN for repayment rate number
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);

            // Assert
            mediatorMock.Verify(x => x.Publish(It.Is<INotification>(n => n.GetType() == typeof(ErrorEvent)), It.IsAny<CancellationToken>()), Times.Once);

            // Assert display is set back to zero and not NaN or something
            vm.LastPressedOperation.Should().Be(CommandWord.Clear);
            vm.DisplayText.Should().Be("0,");
            vm.DisplayNumber.Should().BeApproximately(0, this.Tolerance);
        }

        [Fact]
        public void AlsoCalculateRepaymentRate_AndHandleCalculationErrors()
        {
            // Arrange
            var vm = MockFactories.FinCalcViewModelWithCalculatorImplementationFactory(out var autoMocker);
            var mediatorMock = autoMocker.GetMock<IMediator>();

            // Act
            // Produce NaN for repayment rate number
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(true);
            mediatorMock.Verify(x => x.Publish<INotification>(It.IsAny<ErrorEvent>(), It.IsAny<CancellationToken>()), Times.Once);

            // Assert
            // Assert display is set back to zero and not NaN or something
            vm.LastPressedOperation.Should().Be(CommandWord.Clear);
            vm.DisplayText.Should().Be("0,");
            vm.DisplayNumber.Should().BeApproximately(0, this.Tolerance);

            // Show rate number that was calculated with NaN of repayment rate number
            vm.OperatorPressedCommand.Execute("*");
            vm.RatePressedCommand.Execute(false);

            // Assert values are set back to zero and not NaN or something
            vm.DisplayText.Should().Be("0,00");
            vm.DisplayNumber.Should().BeApproximately(0, this.Tolerance);
            vm.RateNumber.Should().BeApproximately(0, this.Tolerance);
        }
    }
}
