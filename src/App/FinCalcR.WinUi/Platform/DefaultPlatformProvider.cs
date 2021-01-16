using System;
using System.Threading;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Platform
{
    public class DefaultPlatformProvider : IPlatformProvider
    {
        public void OnUiThread(Action action) => action();

        public Task OnUiThreadAsync(Action action)
        {
            var uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew(
                action,
                CancellationToken.None,
                TaskCreationOptions.None,
                uiScheduler);
        }
    }
}
