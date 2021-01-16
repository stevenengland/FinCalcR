using System.Threading.Tasks;
using MediatR;

namespace StEn.FinCalcR.WinUi.Extensions
{
    public static class MediatrExtensions
    {
        public static void PublishOnUiThread(this IMediator mediator, INotification notification) => Platform.Execute.OnUiThread(() => _ = Task.Run(() => mediator.Publish(notification)));
    }
}
