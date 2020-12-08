using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IInputText
    {
        string CurrentInputText { get; }

        void Set(double number, int fractionalDigitCount = -1);

        void DecimalSeparator();

        void ResetInternalState(bool updateCurrentInputText = false);

        // ToDo: Remove function
        bool IsDecimalSeparatorActive { get; set; }
    }
}
