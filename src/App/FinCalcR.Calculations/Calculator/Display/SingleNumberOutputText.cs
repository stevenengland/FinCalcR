using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberOutputText : IOutputText
    {
        private IResultTextFormatter formatter;

        public SingleNumberOutputText(IResultTextFormatter formatter)
        {
            this.formatter = formatter;
        }

        public event EventHandler TextChanged;

        public string TextValue { get; private set; }

        public bool IsTemporaryOverlay { get; }

        public void Set(string inputText)
        {
            this.TextValue = inputText;
        }

        public double GetTextValueAsNumber()
        {
            return double.TryParse(this.TextValue, out var value) ? value : double.NaN;
        }

        public string GetWholeNumberPart()
        {
            throw new NotImplementedException();
        }

        public string GetFractionalPart()
        {
            throw new NotImplementedException();
        }

        public bool IsTextValueAFiniteNumber()
        {
            if (!double.TryParse(this.TextValue, out var value))
            {
                return false;
            }

            return !double.IsInfinity(value) && !double.IsNaN(value);
        }
    }
}
