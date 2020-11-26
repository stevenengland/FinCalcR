using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Calculator.Display
{
    public class OutputTextProcessor : IOutputTextProcessor
    {
        public string ApplySpecialNumberFormatting(int decimalCount)
        {
            throw new NotImplementedException();
        }

        public string BuildDecimalRepresentation(string wholeNumberPart, string fractionalPart)
        {
            throw new NotImplementedException();
        }

        public string InsertThousandSeparator(string wholeNumberPart)
        {
            throw new NotImplementedException();
        }

        public bool IsDecimal(string outputText)
        {
            throw new NotImplementedException();
        }
    }
}
