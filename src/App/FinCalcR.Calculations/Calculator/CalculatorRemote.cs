using System;
using System.Collections.Generic;
using System.Linq;
using StEn.FinCalcR.Calculations.Commands;

namespace StEn.FinCalcR.Calculations.Calculator
{
    public class CalculatorRemote : ICommandInvoker
    {
        // List of all possible commands
        private readonly CommandList commandList;

        // A stack of command words to enable undo operations at a later time.
        private readonly IList<CommandWord> commandJournal = new List<CommandWord>();

        public CalculatorRemote(CommandList commandList)
        {
            this.commandList = commandList;
        }

        public event EventHandler<CommandWord> CommandExecuted;

        public void InvokeCommand(CommandWord commandWord, params object[] parameter)
        {
            ICalculatorCommand command = this.commandList.FirstOrDefault(c => c.ShouldExecute(commandWord));
            if (command == null)
            {
                return;
            }

            command.PreviousCommandWord = this.commandJournal.LastOrDefault();
            this.CommandExecuted?.Invoke(this, command.PreviousCommandWord);

            this.AddCommandToJournal(commandWord);
            command.Execute(parameter);
        }

        private void AddCommandToJournal(CommandWord commandWord)
        {
            if (this.commandJournal.Count >= 20)
            {
                this.commandJournal.RemoveAt(0);
            }

            this.commandJournal.Add(commandWord);
        }
    }
}
