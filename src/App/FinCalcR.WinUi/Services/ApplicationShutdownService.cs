using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MediatR;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Platform;

namespace StEn.FinCalcR.WinUi.Services
{
    public class ApplicationShutdownService : INotificationHandler<ApplicationShutdownEvent>
    {
        // Not very sophisticated at the moment but ok...
        public Task Handle(ApplicationShutdownEvent notification, CancellationToken cancellationToken) => Execute.OnUiThreadAsync(Application.Current.Shutdown);
    }
}
