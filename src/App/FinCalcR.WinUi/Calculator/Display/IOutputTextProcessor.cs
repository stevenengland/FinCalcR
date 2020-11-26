using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Calculator.Display
{
    public interface IOutputTextProcessor
    {
        string BuildDecimalRepresentation(string wholeNumberPart, string fractionalPart);

        string ApplySpecialNumberFormatting(int decimalCount);

        string InsertThousandSeparator(string wholeNumberPart);

        bool IsDecimal(string outputText);
    }
}
