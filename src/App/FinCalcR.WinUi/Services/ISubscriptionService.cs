using System.Collections.Generic;

namespace StEn.FinCalcR.WinUi.Services
{
    public interface ISubscriptionService
    {
        void Subscribe(object subscriber);

        IEnumerable<T> GetSubscriptionsFor<T>();
    }
}
