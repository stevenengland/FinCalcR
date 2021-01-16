using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace StEn.FinCalcR.WinUi.Platform
{
    public class XamlPlatformProvider : IPlatformProvider
    {
        private readonly Dispatcher dispatcher;

        public XamlPlatformProvider()
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void OnUiThread(Action action)
        {
            if (this.CheckAccess())
            {
                action();
            }
            else
            {
                var waitHandle = new System.Threading.ManualResetEvent(false);
                Exception exception = null;
                Action method = () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    waitHandle.Set();
                };

                // TODO: Switch from legacy to JoinableTask like https://github.com/microsoft/vs-threading/blob/master/doc/analyzers/VSTHRD001.md suggests
                // Currently it is impossible due to bugs by libraries: Microsoft.VisualStudio.Shell.Framework and Microsoft.VisualStudio.Shell.Interop in v16.7.xxx
#pragma warning disable VSTHRD001 // Avoid legacy thread switching APIs
                _ = this.dispatcher.BeginInvoke(method);
#pragma warning restore VSTHRD001 // Avoid legacy thread switching APIs
                waitHandle.WaitOne();
                if (exception != null)
                {
                    throw new System.Reflection.TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                }
            }
        }

        public virtual Task OnUiThreadAsync(Action action)
        {
            this.ValidateDispatcher();

            // TODO: Switch from legacy to JoinableTask like https://github.com/microsoft/vs-threading/blob/master/doc/analyzers/VSTHRD001.md suggests
            // Currently it is impossible due to bugs by libraries: Microsoft.VisualStudio.Shell.Framework and Microsoft.VisualStudio.Shell.Interop in v16.7.xxx
#pragma warning disable VSTHRD001 // Avoid legacy thread switching APIs
            return this.dispatcher.InvokeAsync(action).Task;
#pragma warning restore VSTHRD001 // Avoid legacy thread switching APIs
        }

        private bool CheckAccess() => this.dispatcher?.CheckAccess() != false;

        private void ValidateDispatcher()
        {
            if (this.dispatcher == null)
            {
                throw new InvalidOperationException("Not initialized with dispatcher.");
            }
        }
    }
}
