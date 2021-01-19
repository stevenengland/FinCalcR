using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StEn.FinCalcR.WinUi.Events;

namespace StEn.FinCalcR.WinUi.Services
{
    public class KeyboardEventDistributionService : IKeyboardEventDistributionService, INotificationHandler<KeyboardKeyDownEvent>
    {
        private readonly ISubscriptionService subscriptionService;

        public KeyboardEventDistributionService(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        public Task Handle(KeyboardKeyDownEvent notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            var handleKeyboardPresses = this.subscriptionService.GetSubscriptionsFor<IHandleKeyboardPress>();
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
