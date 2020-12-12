using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Display;
using StEn.FinCalcR.Calculations.Commands;

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

        void PressOperator(MathOperator mathOperator);

        void PressCalculate();

        void PressClear(IList<string> memoryFieldCategories);
    }
}
