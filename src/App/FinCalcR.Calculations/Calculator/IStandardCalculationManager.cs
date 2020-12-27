using System.Collections.Generic;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface IStandardCalculationManager
    {
        IMemoryFieldContainer MemoryFields { get; }

        /// <summary>
        /// Gets or sets a value indicating whether calculation is locked.
        /// That is the case if calculation was just triggered and another calculation without further input should be avoided.
        /// </summary>
        bool IsCalcCommandLock { get; set; }

        void PressDecimalSeparator();

        void PressAlgebSign();

        void PressOperator(string mathOperatorToken);

        void PressCalculate();

        void PressClear(IList<string> memoryFieldCategories);

        void PressDigit(string digit);

        void PressLoadMemoryValue(string memoryFieldId);
    }
}
