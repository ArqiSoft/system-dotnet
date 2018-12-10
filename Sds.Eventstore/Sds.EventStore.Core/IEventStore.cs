using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sds.EventStore
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> ReadEventsForwardAsync(Guid streamId, long start = 0, int count = -1);
        Task<IEnumerable<IEvent>> ReadEventsBackwardAsync(Guid streamId, long start = -1, int count = -1);
    }
}
