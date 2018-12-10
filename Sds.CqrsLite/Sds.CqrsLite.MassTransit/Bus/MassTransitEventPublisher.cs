using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using MassTransit;

namespace Sds.CqrsLite.MassTransit
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _endpoint;

        public MassTransitEventPublisher(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IEvent
        {
            await _endpoint.Publish(@event, @event.GetType(), cancellationToken);
        }
    }
}
