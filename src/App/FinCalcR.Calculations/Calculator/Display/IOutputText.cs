using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IOutputText
    {
        event EventHandler TextChanged;

        string TextValue { get; }

        bool IsTemporaryOverlay { get; }

        void Set(string inputText);

        double GetTextValueAsNumber();

        bool IsTextValueAFiniteNumber();
    }
}
