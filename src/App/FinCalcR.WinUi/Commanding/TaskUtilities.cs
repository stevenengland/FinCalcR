using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Commanding
{
    public static class TaskUtilities
    {
#pragma warning disable AsyncFixer03 // Avoid fire & forget async void methods
#pragma warning disable VSTHRD100 // Avoid async void methods
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
#pragma warning disable S3168 // "async" methods should not return "void"
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore S3168 // "async" methods should not return "void"
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
#pragma warning restore VSTHRD100 // Avoid async void methods
#pragma warning restore AsyncFixer03 // Avoid fire & forget async void methods
        {
            try
            {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                await task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
}
