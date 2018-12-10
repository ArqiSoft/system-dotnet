using CQRSlite.Bus;
using CQRSlite.Commands;
using CQRSlite.Config;
using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Sds.CqrsLite
{
    public class BusRegistrar
    {
        private readonly IServiceLocator _serviceLocator;

        public BusRegistrar(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }

        public void Register(params Type[] typesFromAssemblyContainingMessages)
        {
            foreach (var typesFromAssemblyContainingMessage in typesFromAssemblyContainingMessages)
            {
                var executorsAssembly = typesFromAssemblyContainingMessage.GetTypeInfo().Assembly;

                RegisterTypes(executorsAssembly.GetTypes());
            }
        }

        public void RegisterTypes(IEnumerable<Type> executorTypes)
        {
            var registrar = _serviceLocator.GetService<IHandlerRegistrar>();

            var handlers = executorTypes.Select(t => new { Type = t, Interfaces = ResolveMessageHandlerInterface(t) })
                    .Where(e => e.Interfaces != null && e.Interfaces.Any());

            foreach (var handler in handlers)
            {
                foreach (var @interface in handler.Interfaces)
                {
                    InvokeHandler(@interface, registrar, handler.Type);
                }
            }
        }

        private void InvokeHandler(Type @interface, IHandlerRegistrar registrar, Type executorType)
        {
            var commandType = @interface.GetGenericArguments()[0];

            var registerExecutorMethod = registrar
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.Name == "RegisterHandler")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Length == 1)
                .Single(mi => mi.GetParameters().Length == 1)
                .MakeGenericMethod(commandType);

            Func<dynamic, CancellationToken, Task> del;
            if (IsCancellable(@interface))
            {
                del = (x, token) =>
                {
                    dynamic handler = _serviceLocator.GetService(executorType);
                    return handler.Handle(x, token);
                };
            }
            else
            {
                del = (x, token) =>
                {
                    dynamic handler = _serviceLocator.GetService(executorType);
                    return handler.Handle(x);
                };
            }

            registerExecutorMethod.Invoke(registrar, new object[] { del });
        }

        private static bool IsCancellable(Type @interface)
        {
            return @interface.GetGenericTypeDefinition() == typeof(ICancellableCommandHandler<>)
                   || @interface.GetGenericTypeDefinition() == typeof(ICancellableEventHandler<>);
        }

        private static IEnumerable<Type> ResolveMessageHandlerInterface(Type type)
        {
            return type
                .GetInterfaces()
                .Where(i => i.GetTypeInfo().IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                             || i.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                             || i.GetGenericTypeDefinition() == typeof(ICancellableCommandHandler<>)
                             || i.GetGenericTypeDefinition() == typeof(ICancellableEventHandler<>)
                            ));
        }
    }
}
