using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using StEn.FinCalcR.Calculations.Calculator.Display;
using Xunit;

namespace FinCalcR.Calculations.Tests.CalculatorTests
{
    public class NumberFormattingShould
    {
        [Theory]
        [InlineData("1.234", 2, "1.23")]
        [InlineData("-1,234.123", 2, "-1234.12")]
        public void ReturnReducedFractionalPart_WhenFractionalPartIsLongerThanMaxPrecision_GivenATextualNumber(string inputNumber, int maxPrecision, string expectedOutput)
        {
            var result = NumberFormatting.RoundToMaxArithmeticPrecision(inputNumber, maxPrecision);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory]
        [InlineData(1.234, 2, "1.23")]
        public void ReturnReducedFractionalPart_WhenFractionalPartIsLongerThanMaxPrecision_GivenANumber(double inputNumber, int maxPrecision, string expectedOutput)
        {
            var result = NumberFormatting.RoundToMaxArithmeticPrecision(inputNumber, maxPrecision);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory]
        [InlineData("4.9999", "5.000")]
        [InlineData("4.9989", "4.999")]
        public void ReturnRoundedResults_WhenLastFractionalDigitIsGreaterEqualsRoundingBarrier(string inputNumber, string expectedOutput)
        {
            var result = NumberFormatting.RoundToMaxArithmeticPrecision(inputNumber, 3);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory]
        [InlineData(2000.12, 3, "2,000.120")]
        [InlineData(2000.12, -1, "2,000")]
        public void ReturnFormattedResult_GivenANumber(double inputNumber, int precision, string expectedOutput)
        {
            var result = NumberFormatting.ConvertToNumberFormat(inputNumber, precision);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory]
        [InlineData("2000.12", 3, "2,000.120")]
        [InlineData("2,000.12", 3, "2,000.120")]
        [InlineData("2,000.12", -1, "2,000")]
        public void ReturnFormattedResult_GivenATextualNumber(string inputNumber, int precision, string expectedOutput)
        {
            var result = NumberFormatting.ConvertToNumberFormat(inputNumber, precision);
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory]
        [InlineData("-2000.120", "-2,000.120")]
        [InlineData("2000.120", "2,000.120")]
        [InlineData("200.120", "200.120")]
        [InlineData("-200.120", "-200.120")]
        [InlineData("2000.12", "2,000.12")]
        [InlineData("2,000,000.12", "2,000,000.12")]
        [InlineData("2000", "2,000")]
        [InlineData("-2000", "-2,000")]
        [InlineData("2000.", "2,000")]
        public void ReturnGroupedNumberString(string input, string expectedOutput)
        {
            var result = NumberFormatting.InsertGroupSeparator(input);
            result.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
