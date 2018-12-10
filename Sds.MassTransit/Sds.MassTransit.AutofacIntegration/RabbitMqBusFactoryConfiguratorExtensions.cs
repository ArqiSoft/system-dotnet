using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sds.MassTransit.AutofacIntegration
{
    public static class RabbitMqBusFactoryConfiguratorExtensions
    {
        public static Task RegisterConsumers(this IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host, IComponentContext provider, Action<IRabbitMqReceiveEndpointConfigurator> configure, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var consumers = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))).ToList();

            foreach (var t in consumers)
            {
                configurator.RegisterConsumer(host, t, provider, configure);
            }

            return Task.CompletedTask;
        }

        public static Task RegisterConsumer(this IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host, Type consumerType, IComponentContext provider, Action<IRabbitMqReceiveEndpointConfigurator> configure)
        {
            var interfaces = consumerType.GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType && i.GenericTypeArguments.Count() == 1 && (i.GetGenericTypeDefinition() == typeof(IConsumer<>))).ToList();

            configurator.ReceiveEndpoint(host, consumerType.FullName, e =>
            {
                var context = provider.Resolve<IComponentContext>();
                e.Consumer(consumerType, t => context.Resolve(t));
                configure(e);
            });

            return Task.CompletedTask;
        }

        public static void RegisterConsumer<TConsumer>(this IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host, IComponentContext provider, Action<IRabbitMqReceiveEndpointConfigurator> configure) where TConsumer : IConsumer
        {
            configurator.RegisterConsumer(host, typeof(TConsumer), provider, configure);
        }

        public static void RegisterConsumer<TConsumer>(this IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host, IComponentContext provider) where TConsumer : IConsumer
        {
            configurator.RegisterConsumer(host, typeof(TConsumer), provider, e => { });
        }
    }

    public static class ContainerBuilderExtensions
    {
        public static void AddAllConsumers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            builder.RegisterConsumers(assemblies);
        }
    }
}
