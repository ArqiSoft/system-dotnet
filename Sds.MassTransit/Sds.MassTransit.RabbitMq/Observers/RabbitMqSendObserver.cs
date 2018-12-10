using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MassTransit
{
    public class RabbitMqSendObserver : ISendObserver
    {
        public Task PreSend<T>(SendContext<T> context)
            where T : class
        {
            Log.Information($"Sending message {typeof(T).FullName} with Id {context.MessageId}");

            return Task.FromResult(true);
        }

        public Task PostSend<T>(SendContext<T> context)
            where T : class
        {
            Log.Information($"CorrelationId - {context.CorrelationId}: Message {typeof(T).FullName} with Id {context.MessageId} successfully sent. Message: {JsonConvert.SerializeObject(context.Message)}");

            return Task.FromResult(true);
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception)
            where T : class
        {
            Log.Error($"CorrelationId - {context.CorrelationId}: Message send {typeof(T).FullName} with Id {context.MessageId} has fault. Error: {exception.ToString()}");

            return Task.FromResult(true);
        }
    }
}
