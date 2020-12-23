using System;
using StEn.FinCalcR.Calculations.Calculator.Events;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IOutputText
    {
        event EventHandler<OutputTextChangedEventArgs> TextChanged;

        string TextValue { get; }

        bool IsTemporaryOverlay { get; }

        void SetOverlay(string overlayText);

        void SetFormula(string formula);

        void SetResult(string result, int precisionLimit = -1);
    }
}
