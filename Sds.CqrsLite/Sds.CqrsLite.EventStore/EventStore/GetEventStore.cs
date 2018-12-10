using CQRSlite.Domain.Exception;
using CQRSlite.Events;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sds.CqrsLite.EventStore
{
    public class GetEventStore : IEventStore
    {
        private const string EventClrTypeHeader = "EventClrTypeName";

        private IEventStoreConnection connection;

        public GetEventStore(string connectionString)
        {
            var settings = ConnectionSettings.Create()
                //.EnableVerboseLogging()
                //.LimitReconnectionsTo(100)
                //.LimitRetriesForOperationTo(100)
                //.SetTimeoutCheckPeriodTo(TimeSpan.FromMilliseconds(100))
                //.SetReconnectionDelayTo(TimeSpan.Zero)
                //.FailOnNoServerResponse()
                .KeepReconnecting();
                //.KeepRetrying()
                //.SetOperationTimeoutTo(TimeSpan.FromSeconds(10));

            connection = EventStoreConnection.Create(connectionString, settings);
            connection.Connected += (sender, args) => OnConnected(sender, args);
            connection.Closed += (sender, args) => OnClosed(sender, args);
            connection.ErrorOccurred += (sender, args) => OnError(sender, args);
            connection.Disconnected += (sender, args) => OnDisconnected(sender, args);
            connection.ConnectAsync().Wait();
        }

        private void OnConnected(object sender, ClientConnectionEventArgs args)
        {
            Log.Debug("Connected to event store");
        }

        private void OnClosed(object sender, ClientClosedEventArgs args)
        {
            Log.Information($"Connection to event store has closed: {args.Reason}");
        }

        private void OnError(object sender, ClientErrorEventArgs args)
        {
            Log.Error($"Event store error: {args.Exception}");
        }

        private void OnDisconnected(object sender, ClientConnectionEventArgs args)
        {
            Log.Debug("Disconnection from event store");
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            //  IMPORTANT! Expected version calculation based on the fact that we get Min fromt the events we want to append
            //  and decrement it on "1" in order to get the vertion of last event in the stream and decrement it on "1" again 
            //  in order to get it consistent with EventStore cause it starts vertioning from 0 and IEvent from 1
            var groupedEvents = events.GroupBy(e => e.Id, (id, evnts) => new
            {
                Id = id,
                Events = evnts.ToList()
            }).ToList();

            foreach (var g in groupedEvents)
            {
                try
                {
                    await AppendEventsToStream($"aggregator-{g.Id}", g.Events, g.Events.First().Version - 2);
                }
                catch (WrongExpectedVersionException)
                {
                    throw new ConcurrencyException(g.Id);
                }
            }
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ReadFromStream($"aggregator-{aggregateId}", fromVersion);
        }

        private async Task AppendEventsToStream(string streamName, IEnumerable<IEvent> domainEvents, int expectedVersion)
        {
            await connection.AppendToStreamAsync(streamName, expectedVersion == -1 ? ExpectedVersion.NoStream : expectedVersion, domainEvents.Select(e => new EventData(
                Guid.NewGuid(),
                e.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e)),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
                {
                    EventClrTypeName = e.GetType().AssemblyQualifiedName
                })))));

            Log.Debug($"Events successfully appended to stream {streamName}");
        }

        private async Task<IEnumerable<IEvent>> ReadFromStream(string streamName, int fromVersion)
        {
            if (fromVersion < 0)
            {
                fromVersion = 0;
            }

            //  by default we try to read next 100 events
            var amount = 100;

            var events = new List<IEvent>();

            //  read events page by page...
            while (true)
            {
                var res = await connection.ReadStreamEventsForwardAsync(streamName, fromVersion, amount, false);

                events.AddRange(res.Events.Select(e => RebuildEvent(e)));

                if (res.IsEndOfStream)
                    break;

                fromVersion += amount;
            }

            return events;
        }

        private IEvent RebuildEvent(ResolvedEvent eventStoreEvent)
        {
            var metadata = eventStoreEvent.OriginalEvent.Metadata;
            var data = eventStoreEvent.OriginalEvent.Data;
            var typeOfDomainEvent = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            var rebuiltEvent = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)typeOfDomainEvent));
            return rebuiltEvent as IEvent;
        }
    }
}
