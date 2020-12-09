using System;
using System.Collections.Generic;
using System.Text;
using StEn.FinCalcR.Calculations.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public interface ICommandInvoker
    {
        // TODO: Remove Event
        event EventHandler<CommandWord> CommandExecuted;

        void InvokeCommand(CommandWord commandWord, params object[] parameter);

        // TODO: REMOVE HERE AND MAKE PRIVATE IN IMPL.
        void AddCommandToJournal(CommandWord commandWord);
    }
}
