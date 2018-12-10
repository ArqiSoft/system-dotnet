using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MassTransit
{
    public class RabbitMqPublishObserver : IPublishObserver
    {
        public Task PrePublish<T>(PublishContext<T> context)
            where T : class
        {
            Log.Information($"Publishing message {typeof(T).FullName} with Id {context.MessageId}");

            return Task.FromResult(true);
        }

        public Task PostPublish<T>(PublishContext<T> context)
            where T : class
        {
            Log.Information($"CorrelationId - {context.CorrelationId}: Message {typeof(T).FullName} with Id {context.MessageId} successfully published. Message: {JsonConvert.SerializeObject(context.Message)}");

            return Task.FromResult(true);
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception)
            where T : class
        {
            Log.Error($"CorrelationId - {context.CorrelationId}: Message publish {typeof(T).FullName} with Id {context.MessageId} has fault. Error: {exception.ToString()}");

            return Task.FromResult(true);
        }
    }
}
