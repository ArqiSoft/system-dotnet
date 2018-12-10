#if NET47
using Autofac;
using CQRSlite.Commands;
using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sds.CqrsLite
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterAllHadnlers(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies).Where(t =>
            {
                var interfaces = t.GetInterfaces();

                return
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableCommandHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableEventHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
            });
        }

        public static void RegisterHadnlers(this ContainerBuilder builder, params Type[] handlers)
        {
            builder.RegisterTypes(handlers).Where(t =>
            {
                var interfaces = t.GetInterfaces();

                return
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableCommandHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableEventHandler<>)) ||
                    interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
            });
        }
    }
}
#endif
