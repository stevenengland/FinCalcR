using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IInputText
    {
        /// <summary>
        /// Gets the typed formula like "(4 + 5) + 1".
        /// </summary>
        string CurrentInputFormula { get; }

        /// <summary>
        /// Gets the evaluated result of the current currently typed formula.
        /// </summary>
        string EvaluatedResult { get; }

        void Set(double number);

        void DecimalSeparator();

        void ResetInternalState(bool updateCurrentInputText = false);

        // ToDo: Remove function
        bool IsDecimalSeparatorActive { get; set; }

        // ToDo: Remove function
        string WholeNumberPart { get; set; }

        // ToDo: Remove function
        string FractionalNumberPart { get; set; }
    }
}
