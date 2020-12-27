namespace StEn.FinCalcR.Calculations.Calculator.Display
{
    public interface IInputText
    {
        /// <summary>
        /// Gets the typed formula like "(4 + 5) + 1".
        /// </summary>
        /// <returns>A textual representation of the current input in terms of a formula.</returns>
        string GetCurrentInputFormula();

        /// <summary>
        /// Gets the evaluated result of the current currently typed formula.
        /// </summary>
        /// <returns>A number that is the result of the evaluated formula.</returns>
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
