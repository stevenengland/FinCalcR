using System;
using MediatR;

namespace StEn.FinCalcR.WinUi.Events
{
    public class ErrorEvent : INotification
    {
        public ErrorEvent(Exception exception, string errorMessage, bool shutdown = false, bool showAsSnackbarItem = false)
        {
            this.Exception = exception;
            this.ErrorMessage = errorMessage;
            this.ApplicationMustShutdown = shutdown;
            this.ShowAsSnackbarItem = showAsSnackbarItem;
        }

        public ErrorEvent(string errorMessage, bool shutdown = false, bool showAsSnackbarItem = false)
        {
            this.ErrorMessage = errorMessage;
            this.ApplicationMustShutdown = shutdown;
            this.ShowAsSnackbarItem = showAsSnackbarItem;
        }

        public Exception Exception { get; }

        public string ErrorMessage { get; }

        public bool ApplicationMustShutdown { get; } = false;

        public bool ShowAsSnackbarItem { get; } = false;
    }
}
