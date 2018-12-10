using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sds.EventStore
{
    public class EventStore : IEventStore
    {
        private const string EventClrTypeHeader = "EventClrTypeName";

        private IEventStoreConnection connection;

        public EventStore(string connectionString)
        {
            var settings = ConnectionSettings.Create()
                .KeepReconnecting();

            connection = EventStoreConnection.Create(connectionString, settings);
            connection.ConnectAsync().Wait();
        }

        public async Task<IEnumerable<IEvent>> ReadEventsForwardAsync(Guid streamId, long start = 0, int count = -1)
        {
            var sliceSize = 200;
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = start;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync($"aggregator-{streamId}", nextSliceStart, count == StreamPosition.End ? sliceSize : nextSliceStart + sliceSize > count ? count : sliceSize, false);

                nextSliceStart = currentSlice.NextEventNumber;

                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream && (count == StreamPosition.End || count != StreamPosition.End && nextSliceStart < start + count));

            return streamEvents.Select(e => RebuildEvent(e));
        }

        public async Task<IEnumerable<IEvent>> ReadEventsBackwardAsync(Guid streamId, long start = -1, int count = -1)
        {
            var sliceSize = 200;
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = start;
            do
            {
                currentSlice = await connection.ReadStreamEventsBackwardAsync($"aggregator-{streamId}", nextSliceStart, count == StreamPosition.End ? sliceSize : nextSliceStart + sliceSize > count ? count : sliceSize, false);

                nextSliceStart = currentSlice.NextEventNumber;

                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream && (count == StreamPosition.End || count != StreamPosition.End && nextSliceStart < start + count));

            return streamEvents.Select(e => RebuildEvent(e));
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
