using CQRSlite.Events;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public class EventPublisherObserver : IEventPublisherObserver
    {
        public Task PublishFault<T>(T @event, Exception exception) where T : IEvent
        {
            Log.Error($"Error occured during publishing event {@event.ToStringFormat()}. Error: {exception}");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }

        public Task PostPublish<T>(T @event) where T : IEvent
        {
            Log.Information($"Event {@event.ToStringFormat()} successfully published");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }

        public Task PrePublish<T>(T @event) where T : IEvent
        {
            Log.Information($"Publishing event {@event.ToStringFormat()}");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }
    }
}
