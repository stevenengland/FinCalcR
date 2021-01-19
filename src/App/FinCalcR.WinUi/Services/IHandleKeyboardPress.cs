using System.Threading;
using System.Threading.Tasks;
using StEn.FinCalcR.WinUi.Events;

namespace StEn.FinCalcR.WinUi.Services
{
    public interface IHandleKeyboardPress
    {
        Task HandleAsync(KeyboardKeyDownEvent notification, CancellationToken cancellationToken);
    }
}
