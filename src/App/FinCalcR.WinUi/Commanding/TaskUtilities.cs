using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Commanding
{
    public static class TaskUtilities
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
#pragma warning disable CA1030 // Use events where appropriate
#pragma warning disable AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning disable VSTHRD100 // Avoid async void methods
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
#pragma warning disable S3168 // "async" methods should not return "void"
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore S3168 // "async" methods should not return "void"
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
#pragma warning restore VSTHRD100 // Avoid async void methods
#pragma warning restore AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning restore CA1030 // Use events where appropriate
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
#pragma warning disable CA2007 // Do not directly await a Task
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                await task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
#pragma warning restore CA2007 // Do not directly await a Task
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                handler?.HandleError(ex);
            }
        }
    }
}
