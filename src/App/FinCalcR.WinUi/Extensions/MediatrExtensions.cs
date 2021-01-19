using System.Threading.Tasks;
using MediatR;

namespace StEn.FinCalcR.WinUi.Extensions
{
    public static class MediatrExtensions
    {
        public static void PublishOnUiThread(this IMediator mediator, INotification notification) =>
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            Platform.Execute.OnUiThread(async () => await mediator.Publish(notification).ConfigureAwait(true));
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

    }
}
