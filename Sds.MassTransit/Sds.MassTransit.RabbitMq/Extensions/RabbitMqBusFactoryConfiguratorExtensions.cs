using Automatonymous;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Sds.MassTransit.Extensions;
using Sds.MassTransit.Saga;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sds.MassTransit.RabbitMq
{
    public static class RabbitMqBusFactoryConfiguratorExtensions
    {
        public static Task RegisterConsumers(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider provider, Action<IRabbitMqReceiveEndpointConfigurator> endpointConfigurator, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var consumers = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))).ToList();

            foreach (var t in consumers)
            {
                bus.RegisterConsumer(host, t, provider, endpointConfigurator);
            }

            return Task.CompletedTask;
        }

        public static Task RegisterConsumer(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, Type consumerType, IServiceProvider provider, Action<IRabbitMqReceiveEndpointConfigurator> endpointConfigurator = null)
        {
            bus.ReceiveEndpoint(host, consumerType.FullName, e =>
            {
                e.Consumer(consumerType, t => provider.GetRequiredService(t));

                endpointConfigurator?.Invoke(e);
            });

            return Task.CompletedTask;
        }

        public static void RegisterConsumer<TConsumer>(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider provider, Action<IRabbitMqReceiveEndpointConfigurator> endpointConfigurator = null, Action<IConsumerConfigurator<TConsumer>> consumerConfigurator = null) where TConsumer : class, IConsumer
        {
            bus.ReceiveEndpoint(host, typeof(TConsumer).FullName, e =>
            {
                e.Consumer(() => provider.GetRequiredService<TConsumer>(), consumerConfigurator);

                endpointConfigurator?.Invoke(e);
            });
        }

        public static void RegisterScopedConsumer<TConsumer>(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider provider, Action<IRabbitMqReceiveEndpointConfigurator> endpointConfigurator = null, Action<IConsumerConfigurator<TConsumer>> consumerConfigurator = null) where TConsumer : class, IConsumer
        {
            bus.ReceiveEndpoint(host, typeof(TConsumer).FullName, e =>
            {
                e.ScopedConsumer(provider, consumerConfigurator);

                endpointConfigurator?.Invoke(e);
            });
        }

        public static Task RegisterStateMachines(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider container, ISagaRepositoryFactory repositoryFactory, Action<IRabbitMqReceiveEndpointConfigurator> configure, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var stateMachines = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(StateMachine<>)));

            foreach (var t in stateMachines)
            {
                bus.RegisterStateMachine(host, container, t, repositoryFactory, configure);
            }

            return Task.CompletedTask;
        }

        public static Task RegisterStateMachine(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider container, Type stateMachineType, ISagaRepositoryFactory repositoryFactory, Action<IRabbitMqReceiveEndpointConfigurator> configure)
        {
            bus.ReceiveEndpoint(host, stateMachineType.FullName, e =>
            {
                var instanceType = stateMachineType
                    .GetTypeInfo()
                    .BaseType
                    .GetGenericArguments()[0];

                var createMethod = repositoryFactory
                    .GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(mi => mi.Name == "Create")
                    .Where(mi => mi.IsGenericMethod)
                    .Where(mi => mi.GetGenericArguments().Length == 1)
                    .Single()
                    .MakeGenericMethod(instanceType);

                var automatonymousIntegrationAssembly = Assembly.Load(new AssemblyName("MassTransit.AutomatonymousIntegration"));

                var stateMachineSagaMethod = automatonymousIntegrationAssembly
                    .GetTypes()
                    .Where(t => t.GetTypeInfo().IsSealed && !t.GetTypeInfo().IsGenericType && !t.IsNested)
                    .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                    .Where(m => m.IsDefined(typeof(ExtensionAttribute), false) && m.GetParameters()[0].ParameterType == typeof(IReceiveEndpointConfigurator))
                    .Single(m => m.Name == "StateMachineSaga")
                    .MakeGenericMethod(instanceType);

                var stateMachine = container.GetService(stateMachineType);
                var repository = createMethod.Invoke(repositoryFactory, new object[] { });

                stateMachineSagaMethod.Invoke(e, new object[] { e, stateMachine, repository, null });

                configure(e);
            });

            return Task.CompletedTask;
        }

        public static Task RegisterStateMachine<TStateMachine>(this IRabbitMqBusFactoryConfigurator bus, IRabbitMqHost host, IServiceProvider container, ISagaRepositoryFactory repositoryFactory, Action<IRabbitMqReceiveEndpointConfigurator> configure) where TStateMachine : class, StateMachine
        {
            return bus.RegisterStateMachine(host, container, typeof(TStateMachine), repositoryFactory, configure);
        }
    }
}
