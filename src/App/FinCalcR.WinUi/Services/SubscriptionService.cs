using System;
using System.Collections.Generic;
using System.Linq;

namespace StEn.FinCalcR.WinUi.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly List<object> handlers = new List<object>();

        public void Subscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            lock (this.handlers)
            {
                if (this.handlers.Any(x => x.Equals(subscriber)))
                {
                    return;
                }

                this.handlers.Add(subscriber);
            }
        }

        public IEnumerable<T> GetSubscriptionsFor<T>()
        {
            lock (this.handlers)
            {
                var resultList = new List<T>();
                foreach (var handler in this.handlers)
                {
                    if (handler is T castHandler)
                    {
                        resultList.Add(castHandler);
                    }
                }

                return resultList;
            }
        }
    }
}
