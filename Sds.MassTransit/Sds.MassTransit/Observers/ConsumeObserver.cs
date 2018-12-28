using MassTransit;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.MassTransit.Observers
{
    public class ConsumeObserver : IConsumeObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            Log.Error($"[Consume] Message {typeof(T)} with Id: {context.MessageId} consumed with error. Message: {JsonConvert.SerializeObject(context.Message)}. Exception: {exception.ToString()}");
            await context.CompleteTask;
        }

        public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            Log.Information($"[Consume] Message {typeof(T)} with Id: {context.MessageId} successfully consumed. Message: {JsonConvert.SerializeObject(context.Message)}");
            await context.CompleteTask;
        }

        public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            Log.Information($"[Consume] Message {typeof(T)} with Id: {context.MessageId} is about to be consumed. Message: {JsonConvert.SerializeObject(context.Message)}");
            await context.CompleteTask;
        }
    }
}
