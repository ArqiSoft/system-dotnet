using CQRSlite.Commands;
using CQRSlite.Events;
using CQRSlite.Messages;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public class ConsumeSagaObserver : IConsumeSagaObserver
    {
        public Task ConsumeFault<T>(T message, Exception exception) where T : IMessage
        {
            if (message is IEvent)
            {
                var @event = message as IEvent;

                Log.Error($"Error occured during handling event {message.GetType().FullName} with Id {@event.Id}. Error: {exception}");
                Log.Debug($"Event: { JsonConvert.SerializeObject(message)}");
            }
            else
            {
                Log.Error($"Cannot process command {message.GetType().FullName}. Error: {exception}");
                Log.Debug($"Command: { JsonConvert.SerializeObject(message)}");
            }

            return Task.FromResult(true);
        }

        public Task PostConsume<T>(T message) where T : IMessage
        {
            if (message is IEvent)
            {
                var @event = message as IEvent;

                Log.Information($"Event {message.GetType().FullName} with Id {@event.Id} successfully handled");
                Log.Debug($"Event: { JsonConvert.SerializeObject(message)}");
            }
            else
            {
                Log.Information($"Command {message.GetType().FullName} processed successfully");
                Log.Debug($"Command: { JsonConvert.SerializeObject(message)}");
            }

            return Task.FromResult(true);
        }

        public Task PreConsume<T>(T message) where T : IMessage
        {
            if (message is IEvent)
            {
                var @event = message as IEvent;

                Log.Information($"Event {message.GetType().FullName} with Id {@event.Id} recived");
                Log.Debug($"Event: { JsonConvert.SerializeObject(message)}");
            }
            else
            {
                Log.Information($"Command {message.GetType().FullName} processing started");
                Log.Debug($"Command: { JsonConvert.SerializeObject(message)}");
            }

            return Task.FromResult(true);
        }
    }
}
