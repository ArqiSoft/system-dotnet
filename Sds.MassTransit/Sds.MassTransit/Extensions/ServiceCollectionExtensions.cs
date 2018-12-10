using Automatonymous;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sds.MassTransit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllConsumers(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var consumers = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.GetInterfaces().Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)));
            foreach (var consumer in consumers)
            {
                services.AddTransient(consumer);
            }
        }

        public static void AddAllStateMachines(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var stateMachines = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(MassTransitStateMachine<>));

            foreach (var machine in stateMachines)
            {
                services.AddTransient(machine);
            }
        }

        public static void AddAllStateMachinesInMemorySagaRepositories(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
            {
                assemblies = new Assembly[] { Assembly.GetEntryAssembly() };
            }

            var stateMachines = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(MassTransitStateMachine<>));

            foreach (var machine in stateMachines)
            {
                var stateType = machine.BaseType.GenericTypeArguments[0];

                var repository = typeof(InMemorySagaRepository<>);
                var inMemoryRepository = Activator.CreateInstance(repository.MakeGenericType(new Type[] { stateType }));

                var dependencyInjectionAssembly = Assembly.Load(new AssemblyName("Microsoft.Extensions.DependencyInjection.Abstractions"));

                var addSingltonMethod = dependencyInjectionAssembly
                    .GetTypes()
                    .Where(t => t.GetTypeInfo().IsSealed && !t.GetTypeInfo().IsGenericType && !t.IsNested)
                    .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                    .Where(m => m.IsDefined(typeof(ExtensionAttribute), false) && m.GetParameters()[0].ParameterType == typeof(IServiceCollection))
                    .Where(m => m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType == m.GetGenericArguments()[0])
                    .Single(m => m.Name == "AddSingleton")
                    .MakeGenericMethod(typeof(ISagaRepository<>).MakeGenericType(new Type[] { stateType }));

                addSingltonMethod.Invoke(services, new object[] { services, inMemoryRepository });
            }
        }
    }
}
