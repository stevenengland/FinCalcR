using System;
using FluentAssertions;
using StEn.FinCalcR.Calculations.Calculator.Display;
using Xunit;

namespace FinCalcR.Calculations.Tests.CalculatorTests
{
    public class SingleNumberOutputShould
    {
        [Fact]
        public void ActivateOverlayFlag_WhenOverlayIsSet()
        {
            // Arrange
            var output = new SingleNumberOutput();

            // Act
            output.SetOverlay("test");

            // Assert
            output.IsTemporaryOverlay.Should().Be(true);
        }

        [Fact]
        public void SetOverlayToFalse_WhenFormulaIsUpdated()
        {
            // Arrange
            var output = new SingleNumberOutput();

            // Act
            output.SetOverlay("test");
            output.SetFormula("test");

            // Assert
            output.IsTemporaryOverlay.Should().Be(false);
        }

        [Fact]
        public void SetOverlayToFalse_WhenResultIsSet()
        {
            // Arrange
            var output = new SingleNumberOutput();

            // Act
            output.SetOverlay("test");
            output.SetResult("test");

            // Assert
            output.IsTemporaryOverlay.Should().Be(false);
        }
    }
}
