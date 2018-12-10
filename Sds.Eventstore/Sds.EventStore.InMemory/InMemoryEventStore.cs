using CQRSlite.Domain.Exception;
using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sds.EventStore
{
    public class InMemoryEventStore : IEventStore, CQRSlite.Events.IEventStore
    {
        private IDictionary<Guid, IList<IEvent>> _streams = new Dictionary<Guid, IList<IEvent>>();

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken token = default(CancellationToken))
        {
            lock (_streams)
            {
                if (_streams.ContainsKey(aggregateId))
                {
                    var events = _streams[aggregateId].Skip(fromVersion);

                    return Task.FromResult<IEnumerable<IEvent>>(events);
                }

                return Task.FromResult<IEnumerable<IEvent>>(new List<IEvent>());
            }
        }

        public Task Save(IEnumerable<IEvent> events, CancellationToken token = default(CancellationToken))
        {
            lock (_streams)
            {
                var groupedEvents = events.GroupBy(e => e.Id, (id, evnts) => new
                {
                    Id = id,
                    Events = evnts
                });

                foreach (var g in groupedEvents)
                {
                    AppendEventsToStream(g.Id, g.Events, g.Events.First().Version - 1);
                }

                return Task.CompletedTask;
            }
        }

        public Task<IEnumerable<IEvent>> ReadEventsBackwardAsync(Guid stream, long start = -1, int count = -1)
        {
            lock (_streams)
            {
                if (_streams.ContainsKey(stream))
                {
                    var length = _streams[stream].Count();
                    var events = _streams[stream].Reverse();

                    if (start >= 0)
                    {
                        events = events.Skip(length - (int)start - 1);
                    }

                    if (count > 0)
                    {
                        events = events.Take(count);
                    }

                    return Task.FromResult(events);
                }

                return Task.FromResult((IEnumerable<IEvent>)new List<IEvent>());
            }
        }

        public Task<IEnumerable<IEvent>> ReadEventsForwardAsync(Guid stream, long start = 0, int count = -1)
        {
            lock (_streams)
            {
                if (_streams.ContainsKey(stream))
                {
                    var events = _streams[stream].Skip((int)start);

                    if (count > 0)
                    {
                        events = events.Take(count);
                    }

                    return Task.FromResult(events);
                }

                return Task.FromResult((IEnumerable<IEvent>)new List<IEvent>());
            }
        }

        private Task AppendEventsToStream(Guid id, IEnumerable<IEvent> domainEvents, int expectedVersion)
        {
            lock (_streams)
            {
                if (!_streams.ContainsKey(id))
                {
                    _streams[id] = new List<IEvent>();
                }

                var stream = _streams[id];

                var lastEvent = stream.LastOrDefault();

                if (lastEvent == null && expectedVersion != 0 || lastEvent != null && lastEvent.Version != expectedVersion)
                {
                    throw new ConcurrencyException(id);
                }

                foreach (var e in domainEvents)
                {
                    stream.Add(e);
                }

                return Task.CompletedTask;
            }
        }
    }
}
