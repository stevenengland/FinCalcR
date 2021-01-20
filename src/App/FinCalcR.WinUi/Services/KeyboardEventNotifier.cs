using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StEn.FinCalcR.WinUi.Events;

namespace StEn.FinCalcR.WinUi.Services
{
    public class KeyboardEventNotifier : IKeyboardEventNotifier, INotificationHandler<KeyboardKeyDownEvent>
    {
        private readonly ISubscriptionAggregator subscriptionAggregator;

        public KeyboardEventNotifier(ISubscriptionAggregator subscriptionAggregator)
        {
            this.subscriptionAggregator = subscriptionAggregator;
        }

        public Task Handle(KeyboardKeyDownEvent notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            var handleKeyboardPresses = this.subscriptionAggregator.GetSubscriptionsFor<IHandleKeyboardPress>();
            if (handleKeyboardPresses != null)
            {
                tasks = handleKeyboardPresses
                    .Select(method => method.HandleAsync(notification, cancellationToken))
                    .ToList();
            }

            return Task.WhenAll(tasks);
        }
    }
}
