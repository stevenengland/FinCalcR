using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinCalcR.WinUi.Tests.CalculatorTests
{
    public class OutputTextShould
    {
        [Theory]
        [InlineData("1000", "1.000")]
        public void InsertThousandSeparatorAtAllPositions_GivenDfferentLenthInput(string given, string expected)
        {

        }
    }
}
