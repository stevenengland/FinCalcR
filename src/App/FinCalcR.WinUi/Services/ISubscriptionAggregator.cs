using System.Collections.Generic;

namespace StEn.FinCalcR.WinUi.Services
{
    public interface ISubscriptionAggregator
    {
        void Subscribe(object subscriber);

        IEnumerable<T> GetSubscriptionsFor<T>();
    }
}
