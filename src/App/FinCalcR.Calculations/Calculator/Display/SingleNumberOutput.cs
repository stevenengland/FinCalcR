using System;
using System.Threading;
using StEn.FinCalcR.Calculations.Calculator.Events;

namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public class SingleNumberOutput : IOutputText
    {
        private string textValue;

        public event EventHandler<OutputTextChangedEventArgs> TextChanged;

        public string TextValue
        {
            get => this.textValue;
            private set
            {
                this.textValue = value;
                this.TextChanged?.Invoke(this, new OutputTextChangedEventArgs(value));
            }
        }

        public bool IsTemporaryOverlay { get; private set; }

        public void SetOverlay(string overlayText)
        {
            this.TextValue = overlayText;
            this.IsTemporaryOverlay = true;
        }

        public void SetFormula(string formula)
        {
            this.SetResult(formula);
        }

        public void SetResult(string result, int precisionLimit = -1)
        {
            this.IsTemporaryOverlay = false;
            var textToDisplay = NumberFormatting.InsertGroupSeparator(
                NumberFormatting.RoundToMaxArithmeticPrecision(result, precisionLimit));

            if (textToDisplay.Split(Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator
                    .ToCharArray()).Length < 2)
            {
                textToDisplay += Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            }

            this.TextValue = textToDisplay;
        }
    }
}
