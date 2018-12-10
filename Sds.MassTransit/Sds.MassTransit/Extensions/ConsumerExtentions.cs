using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Scoping;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Sds.MassTransit.Extensions
{
    public static class ConsumerExtentions
    {
        public static void ScopedConsumer<TConsumer>(this IReceiveEndpointConfigurator configurator, IServiceProvider container, Action<IConsumerConfigurator<TConsumer>> configure = null) where TConsumer : class, IConsumer
        {
            configurator.Consumer(new ScopeConsumerFactory<TConsumer>(container.GetRequiredService<IConsumerScopeProvider>()), configure);
        }
    }
}
