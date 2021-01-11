using System.Globalization;
using System.Threading;
using FluentAssertions;
using StEn.FinCalcR.WinUi.Converter;
using Xunit;

namespace FinCalcR.WinUi.Tests.ConverterTests
{
    public class NumberToFormattedStringConverterShould
    {
        public NumberToFormattedStringConverterShould()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Theory]
        [InlineData(20.333, 2, "20.33")]
        [InlineData(20.335, 2, "20.34")]
        [InlineData(20.333, 0, "20")]
        [InlineData(20000.333, 2, "20,000.33")]
        public void ReturnCorrectText_WhenConvertingFromNumber(double input, double precision, string output)
        {
            var converter = new NumberToFormattedStringConverter();
            var result = (string)converter.Convert(input, null, precision, null);
            result.Should().Be(output);
        }
    }
}
