using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Calculator.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface ICommandInvoker
    {
        // TODO: Remove Event
        event EventHandler<CommandWord> CommandExecuted;

        event EventHandler<Exception> CommandFailed;

        void InvokeCommand(CommandWord commandWord, params object[] parameter);

        // TODO: REMOVE HERE AND MAKE PRIVATE IN IMPL.
        void AddCommandToJournal(CommandWord commandWord);
    }
}
