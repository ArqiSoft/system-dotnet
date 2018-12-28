using MassTransit;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.MassTransit.Observers
{
    public class PublishObserver : IPublishObserver
    {
        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            Log.Information($"[Publish] Message {typeof(T)} with Id: {context.MessageId} is about to be published. Message: {JsonConvert.SerializeObject(context.Message)}");

            return Task.CompletedTask;
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            Log.Information($"[Publish] Message {typeof(T)} with Id: {context.MessageId} successdully published. Message: {JsonConvert.SerializeObject(context.Message)}");

            return Task.CompletedTask;
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            Log.Error($"[Publish] Cannot publish message {typeof(T)}  with Id: {context.MessageId}. Message: {JsonConvert.SerializeObject(context.Message)}. Exception: {exception.ToString()}");

            return Task.CompletedTask;
        }
    }
}
