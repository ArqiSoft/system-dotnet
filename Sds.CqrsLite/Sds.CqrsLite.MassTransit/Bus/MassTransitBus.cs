using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using MassTransit;

namespace Sds.CqrsLite.MassTransit
{
    public class MassTransitBus : IEventPublisher
    {
        private readonly IBusControl _busControl;

        public MassTransitBus(IBusControl busControl)
        {
            _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IEvent
        {
            await _busControl.Publish(@event, @event.GetType(), cancellationToken);
        }
    }
}
