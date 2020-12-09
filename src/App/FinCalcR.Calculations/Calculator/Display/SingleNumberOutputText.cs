using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StEn.FinCalcR.Calculations.Calculator.Events;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberOutputText : IOutputText
    {
        public event EventHandler<OutputTextChangedEventArgs> TextChanged;

        public string TextValue { get; private set; }

        public bool IsTemporaryOverlay { get; private set; }

        public void Set(string inputText, bool isTemporaryOverlay = false)
        {
            this.TextValue = inputText;
            this.TextChanged?.Invoke(this, new OutputTextChangedEventArgs(this.TextValue));

            this.IsTemporaryOverlay = isTemporaryOverlay;
        }

        public double GetTextValueAsNumber()
        {
            return double.TryParse(this.TextValue, out var value) ? value : double.NaN;
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
