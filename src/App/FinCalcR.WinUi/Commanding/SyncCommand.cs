using System;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Commanding
{
#pragma warning disable CA2007 // Do not directly await a Task
	public class SyncCommand : ISyncCommand
	{
		private readonly Action execute;
		private readonly Func<bool> canExecute;
		private bool isExecuting;

		public SyncCommand(
			Action execute,
			Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute()
		{
			return !this.isExecuting && (this.canExecute?.Invoke() ?? true);
		}

		public void ExecuteSync()
		{
			if (this.CanExecute())
			{
				try
				{
					this.isExecuting = true;
					this.execute();
				}
				finally
				{
					this.isExecuting = false;
				}
			}
		}

		#region Explicit implementations
		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute();
		}

		void ICommand.Execute(object parameter)
		{
			this.ExecuteSync();
		}
		#endregion
	}

#pragma warning disable SA1402 // File may only contain a single class
	public class SyncCommand<T> : ISyncCommand<T>
#pragma warning restore SA1402 // File may only contain a single class
	{
		private readonly Action<T> execute;
		private readonly Func<T, bool> canExecute;
		private bool isExecuting;

		public SyncCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute(T parameter)
		{
			return !this.isExecuting && (this.canExecute?.Invoke(parameter) ?? true);
		}

		public void ExecuteSync(T parameter)
		{
			if (this.CanExecute(parameter))
			{
				try
				{
					this.isExecuting = true;
					this.execute(parameter);
				}
				finally
				{
					this.isExecuting = false;
				}
			}
		}

		#region Explicit implementations
		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute((T)parameter);
		}

		void ICommand.Execute(object parameter)
		{
			this.ExecuteSync((T)parameter);
		}
		#endregion
	}
#pragma warning restore CA2007 // Do not directly await a Task
}
