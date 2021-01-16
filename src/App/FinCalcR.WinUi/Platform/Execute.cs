using System;
using System.Threading.Tasks;

namespace StEn.FinCalcR.WinUi.Platform
{
    public static class Execute
    {
        public static void OnUiThread(this Action action)
        {
            PlatformProvider.Current.OnUiThread(action);
        }

        public static Task OnUiThreadAsync(this Action action) => PlatformProvider.Current.OnUiThreadAsync(action);
    }
}
