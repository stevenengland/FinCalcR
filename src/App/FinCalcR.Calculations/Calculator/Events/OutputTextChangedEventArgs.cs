using System;
using System.Collections.Generic;
using System.Text;

namespace StEn.FinCalcR.Calculations.Calculator.Events
{
    public class OutputTextChangedEventArgs : EventArgs
    {
        public OutputTextChangedEventArgs(string newText)
        {
            this.NewText = newText;
        }

        private string NewText { get; }
    }
}
