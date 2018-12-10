using MassTransit.RabbitMqTransport.Contexts;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MassTransit
{
    public class RabbitMqReceiveObserver : IReceiveObserver
    {
        public async Task PreReceive(ReceiveContext context)
        {
            var rabbitContext = context as RabbitMqReceiveContext;

            if (rabbitContext != null)
            {
                var message = await new StreamReader(System.Text.Encoding.Default.GetString(context.GetBody())).ReadToEndAsync();

                Log.Information($"CorrelationId - {rabbitContext.Properties.CorrelationId}: Receiving message {rabbitContext.Properties.MessageId} for {rabbitContext.InputAddress}");
            }
        }

        public async Task PostReceive(ReceiveContext context)
        {
            var rabbitContext = context as RabbitMqReceiveContext;

            if (rabbitContext != null)
            {
                var message = await new StreamReader(System.Text.Encoding.Default.GetString(context.GetBody())).ReadToEndAsync();

                Log.Information($"CorrelationId - {rabbitContext.Properties.CorrelationId}: Message {rabbitContext.Properties.MessageId} for {rabbitContext.InputAddress} successfully recived. Message: {message}");
            }
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
            where T : class
        {
            Log.Information($"CorrelationId - {context.CorrelationId}: Message {typeof(T)} with Id {context.MessageId} for consumer {consumerType} successfully consumed");

            return Task.FromResult(true);
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan elapsed, string consumerType, Exception exception) where T : class
        {
            Log.Error($"CorrelationId - {context.CorrelationId}: Message {typeof(T)} with Id {context.MessageId} for consumer {consumerType} has failt. Error: {exception.ToString()}");

            return Task.FromResult(true);
        }

        public async Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            var rabbitContext = context as RabbitMqReceiveContext;

            if (rabbitContext != null)
            {
                var message = await new StreamReader(System.Text.Encoding.Default.GetString(context.GetBody())).ReadToEndAsync();

                Log.Error($"CorrelationId - {rabbitContext.Properties.CorrelationId}: Message receive {rabbitContext.Properties.MessageId} for {rabbitContext.InputAddress} has fault. Error: {exception.ToString()}");
            }
        }
    }
}
