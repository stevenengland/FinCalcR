﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StEn.FinCalcR.WinUi.Commanding
{
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private readonly IErrorHandler errorHandler;
        private bool isExecuting;

        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute() => !this.isExecuting && (this.canExecute?.Invoke() ?? true);

        public async Task ExecuteAsync()
        {
            if (this.CanExecute())
            {
                try
                {
                    this.isExecuting = true;
                    await this.execute().ConfigureAwait(false);
                }
                finally
                {
                    this.isExecuting = false;
                }
            }

            this.TriggerCanExecuteChanged();
        }

        public void TriggerCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        void ICommand.Execute(object parameter) => this.ExecuteAsync().FireAndForgetSafeAsync(this.errorHandler);
        #endregion
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class AsyncCommand<T> : IAsyncCommand<T>
#pragma warning restore SA1402 // File may only contain a single class
    {
        private readonly Func<T, Task> execute;
        private readonly Func<T, bool> canExecute;
        private readonly IErrorHandler errorHandler;
        private bool isExecuting;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(T parameter) => !this.isExecuting && (this.canExecute?.Invoke(parameter) ?? true);

        public async Task ExecuteAsync(T parameter)
        {
            if (this.CanExecute(parameter))
            {
                try
                {
                    this.isExecuting = true;
                    await this.execute(parameter).ConfigureAwait(false);
                }
                finally
                {
                    this.isExecuting = false;
                }
            }

            this.TriggerCanExecuteChanged();
        }

        public void TriggerCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter) => this.CanExecute((T)parameter);

        void ICommand.Execute(object parameter) => this.ExecuteAsync((T)parameter).FireAndForgetSafeAsync(this.errorHandler);
        #endregion
    }
}
