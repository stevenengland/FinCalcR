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

        bool IsCalcCommandLock { get; set; }

        // TODO: MAKE PRIVATE OR REMOVE
        CommandWord LastCommand { get; set; }

        void PressDecimalSeparator();
    }
}
