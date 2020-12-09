using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StEn.FinCalcR.Calculations.Calculator.Events;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IOutputText
    {
        event EventHandler<OutputTextChangedEventArgs> TextChanged;

        string TextValue { get; }

        bool IsTemporaryOverlay { get; }

        void Set(string inputText, bool isTemporaryOverlay = false);

        double GetTextValueAsNumber();

        bool IsTextValueAFiniteNumber();
    }
}
