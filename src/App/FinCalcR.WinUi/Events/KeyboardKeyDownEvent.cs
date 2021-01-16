using MediatR;
using StEn.FinCalcR.WinUi.Events.EventArgs;

namespace StEn.FinCalcR.WinUi.Events
{
    public class KeyboardKeyDownEvent : INotification
    {
        public KeyboardKeyDownEvent(MappedKeyEventArgs e)
        {
            this.KeyEventArgs = e;
        }

        public MappedKeyEventArgs KeyEventArgs { get; }
    }
}
