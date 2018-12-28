using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using MassTransit;

namespace Sds.CqrsLite.MassTransit
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private ConsumeContext _context;

        public void SetContext(ConsumeContext context)
        {
            _context = context;
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IEvent
        {
            if (_context == null)
                throw new NullReferenceException(nameof(_context));

            await _context.Publish(@event, @event.GetType(), c =>
            {
                if (_context.CorrelationId != null)
                {
                    c.Headers.Set("CorrelationId", _context.CorrelationId.ToString());
                }
            }, cancellationToken);
        }
    }
}
