using CQRSlite.Events;
using Newtonsoft.Json;
using Sds.CqrsLite.Events;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public static class EventExtentions
    {
        public static string ToStringFormat(this IEvent e)
        {
            var correlationId = e is ICorrelatedEvent ? $", CorrelationId: {(e as ICorrelatedEvent).CorrelationId}" : "";
            var userId = e is IUserEvent ? $", UserId: {(e as IUserEvent).UserId}" : "";

            return $"{e.GetType().FullName}(Id: {e.Id}{correlationId}{userId})";
        }
    }

    public class ConsumeEventObserver : IConsumeEventObserver
    {
        public Task ConsumeFault<T>(T @event, Exception exception) where T : IEvent
        {
            Log.Error($"Error occured during handling event {@event.ToStringFormat()}");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }

        public Task PostConsume<T>(T @event) where T : IEvent
        {
            Log.Information($"Event {@event.ToStringFormat()} successfully handled");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }

        public Task PreConsume<T>(T @event) where T : IEvent
        {
            Log.Information($"Event {@event.ToStringFormat()} recived");
            Log.Debug($"Event: { JsonConvert.SerializeObject(@event)}");

            return Task.FromResult(true);
        }
    }
}
