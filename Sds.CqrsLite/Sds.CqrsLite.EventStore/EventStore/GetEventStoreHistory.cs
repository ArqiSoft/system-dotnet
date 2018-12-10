using Sds.CqrsLite.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.ClientAPI;
using System.Net;
using Serilog;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Sds.CqrsLite.EventStore
{
    public class GetEventStoreHistory : IHistory
    {
        private const string EventClrTypeHeader = "EventClrTypeName";

        const int DEFAULTPORT = 1113;

        private IEventStoreConnection connection;

        private readonly IEventPublisher _publisher;

        public GetEventStoreHistory(IEventPublisher publisher)
        {
            _publisher = publisher;

            var settings = ConnectionSettings.Create();
            //.UseDebugLogger()
            //.EnableVerboseLogging()
            //.UseConsoleLogger();

            connection = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, DEFAULTPORT));
            connection.Connected += (sender, args) => OnConnected(sender, args);
            connection.Closed += (sender, args) => OnClosed(sender, args);
            connection.ErrorOccurred += (sender, args) => OnError(sender, args);
            connection.Disconnected += (sender, args) => OnDisconnected(sender, args);
            connection.ConnectAsync().Wait();
        }

        private void OnConnected(object sender, ClientConnectionEventArgs args)
        {
            Log.Information("Connected to event store");
        }

        private void OnClosed(object sender, ClientClosedEventArgs args)
        {
            Log.Information("Connection to event store has closed");
        }

        private void OnError(object sender, ClientErrorEventArgs args)
        {
            Log.Error($"Event store error: {args.Exception}");
        }

        private void OnDisconnected(object sender, ClientConnectionEventArgs args)
        {
            Log.Information("Disconnection from event store");
        }

        public async Task<IEnumerable<IEvent>> GetHistory(string streamName)
        {
            var fromVersion = 0;
            //throw new NotImplementedException();
            //  by default we try to read next 500 events
            var amount = 100;

            //if (toVersion != null)
            //{
            //    amount = (int)toVersion - fromVersion + 1;
            //}
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

            //var t = Task.Run(task);

            //try
            //{
            //    t.Wait();
            //}
            //catch { }
            //if (t.Status == TaskStatus.RanToCompletion)
            //{
            //    Log.Information("Getting history for {0} is succeeded.", streamName);
            //}
            //else 
            //    if (t.Status == TaskStatus.Faulted)
            //        Log.Error("Getting history for {0} is failed", streamName);
            //return null;


        

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
