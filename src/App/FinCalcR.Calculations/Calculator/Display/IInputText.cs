namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IInputText
    {
        // ToDo: Remove function
        bool IsDecimalSeparatorActive { get; set; }

        // ToDo: Remove function
        string WholeNumberPart { get; set; }

        // ToDo: Remove function
        string FractionalNumberPart { get; set; }

        /// <summary>
        /// Gets the typed formula like "(4 + 5) + 1".
        /// </summary>
        string GetCurrentInputFormula();

        /// <summary>
        /// Gets the evaluated result of the current currently typed formula.
        /// </summary>
        string GetEvaluatedResult();

        void ResetInternalState(bool updateCurrentInputText = false);

        void SetInternalState(double number);

        void DecimalSeparator();

        void AlgebSign();

        void Operator();

        void Calculate();

        void Digit(string digit);
    }
}
