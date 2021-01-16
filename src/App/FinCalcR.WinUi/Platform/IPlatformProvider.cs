using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Platform
{
    public interface IPlatformProvider
    {
        void OnUiThread(Action action);

        Task OnUiThreadAsync(Action action);
    }
}
