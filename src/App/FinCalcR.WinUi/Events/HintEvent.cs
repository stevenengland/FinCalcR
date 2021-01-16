using MediatR;

namespace StEn.FinCalcR.WinUi.Events
{
    public class HintEvent : INotification
    {
        public HintEvent(string message)
        {
            this.Message = message;
        }

        public string Message { get; }
    }
}
