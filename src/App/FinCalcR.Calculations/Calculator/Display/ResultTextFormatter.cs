using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class ResultTextFormatter : IResultTextFormatter
    {
        public string GetFormattedResult(string input)
        {
            // Check if the input format is valid
            throw new NotImplementedException();
        }

        private string ApplySpecialNumberFormatting(int decimalCount)
        {
            throw new NotImplementedException();
        }

        private string BuildDecimalRepresentation(string wholeNumberPart, string fractionalPart)
        {
            throw new NotImplementedException();
        }

        private string InsertThousandSeparator(string wholeNumberPart)
        {
            throw new NotImplementedException();
        }

        private bool IsDecimal(string outputText)
        {
            throw new NotImplementedException();
        }
    }
}
