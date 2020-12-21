using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.Calculations.Calculator.Display;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface ICalculationCommandReceiver
    {
        IMemoryFieldContainer MemoryFields { get; }

        IOutputText OutputText { get; }

        IInputText InputText { get; }

        MathOperator ActiveMathOperator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether calculation is locked.
        /// That is the case if calculation was just triggered and another calculation without further input should be avoided.
        /// </summary>
        bool IsCalcCommandLock { get; set; }

        // TODO: MAKE PRIVATE OR REMOVE
        CommandWord LastCommand { get; set; }

        void PressDecimalSeparator();

        void PressAlgebSign();

        void PressOperator(string mathOperatorToken);

        void PressCalculate();

        void PressClear(IList<string> memoryFieldCategories);

        void PressDigit(string digit);

        void PressLoadMemoryValue(string memoryFieldId);

        void CalculateYears();

        void SetYears();

        void SetRatesPerAnnum();

        void SetEnd();

        void CalculateEnd();

        void CalculatePercent();
        void CalculateRepaymentRate();
    }
}
