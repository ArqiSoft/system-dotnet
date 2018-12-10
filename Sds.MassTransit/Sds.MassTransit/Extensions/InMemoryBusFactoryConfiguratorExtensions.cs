using Automatonymous;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sds.MassTransit.Extensions
{
    public static class InMemoryBusFactoryConfiguratorExtensions
    {
        public static Task RegisterConsumers(this IBusFactoryConfigurator configurator, IServiceProvider provider, params Assembly[] assemblies)
        {
            var consumers = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))).ToList();

            foreach (var t in consumers)
            {
                configurator.RegisterConsumer(t, provider);
            }

            return Task.CompletedTask;
        }

        public static void RegisterConsumer<TConsumer>(this IBusFactoryConfigurator bus, IServiceProvider provider, Action<IReceiveEndpointConfigurator> endpointConfigurator = null, Action<IConsumerConfigurator<TConsumer>> consumerConfigurator = null) where TConsumer : class, IConsumer
        {
            bus.ReceiveEndpoint(typeof(TConsumer).FullName, e =>
            {
                e.Consumer(() => provider.GetRequiredService<TConsumer>(), consumerConfigurator);

                endpointConfigurator?.Invoke(e);
            });
        }

        public static void RegisterScopedConsumer<TConsumer>(this IBusFactoryConfigurator configurator, IServiceProvider provider, Action<IReceiveEndpointConfigurator> endpointConfigurator = null, Action<IConsumerConfigurator<TConsumer>> consumerConfigurator = null) where TConsumer : class, IConsumer
        {
            configurator.ReceiveEndpoint(typeof(TConsumer).FullName, e =>
            {
                e.ScopedConsumer(provider, consumerConfigurator);

                endpointConfigurator?.Invoke(e);
            });
        }

        public static Task RegisterConsumer(this IBusFactoryConfigurator configurator, Type consumerType, IServiceProvider provider)
        {
            configurator.ReceiveEndpoint(consumerType.FullName, e =>
            {
                e.Consumer(consumerType, t => provider.GetRequiredService(t));
                e.UseInMemoryOutbox();
            });

            return Task.CompletedTask;
        }

        public static Task RegisterStateMachines(this IBusFactoryConfigurator configurator, IServiceProvider provider, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var stateMachines = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(MassTransitStateMachine<>));

            foreach (var t in stateMachines)
            {
                configurator.RegisterStateMachine(t, provider);
            }

            return Task.CompletedTask;
        }

        public static Task RegisterStateMachine(this IBusFactoryConfigurator configurator, Type stateMachineType, IServiceProvider provider)
        {
            configurator.ReceiveEndpoint(stateMachineType.FullName, e =>
            {
                var instanceType = stateMachineType
                    .GetTypeInfo()
                    .BaseType
                    .GetGenericArguments()[0];

                var automatonymousIntegrationAssembly = Assembly.Load(new AssemblyName("MassTransit.AutomatonymousExtensions.DependencyInjectionIntegration"));

                var stateMachineSagaMethod = automatonymousIntegrationAssembly
                    .GetTypes()
                    .Where(t => t.GetTypeInfo().IsSealed && !t.GetTypeInfo().IsGenericType && !t.IsNested)
                    .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                    .Where(m => m.IsDefined(typeof(ExtensionAttribute), false) && m.GetParameters()[0].ParameterType == typeof(IReceiveEndpointConfigurator) && m.GetParameters()[2].ParameterType == typeof(IServiceProvider))
                    .Single(m => m.Name == "StateMachineSaga")
                    .MakeGenericMethod(instanceType);

                var stateMachine = provider.GetService(stateMachineType);

                stateMachineSagaMethod.Invoke(e, new object[] { e, stateMachine, provider, null });
            });

            return Task.CompletedTask;
        }

        public static Task RegisterStateMachine<TStateMachine>(this IBusFactoryConfigurator configurator, IServiceProvider provider) where TStateMachine : class, StateMachine
        {
            return configurator.RegisterStateMachine(typeof(TStateMachine), provider);
        }
    }
}
