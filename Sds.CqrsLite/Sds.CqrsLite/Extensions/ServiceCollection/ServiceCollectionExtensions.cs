#if !NET47
using CQRSlite.Commands;
using CQRSlite.Events;
using Microsoft.Extensions.DependencyInjection;
//using Sds.CqrsLite.Saga;
using System.Linq;
using System.Reflection;

namespace Sds.CqrsLite
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllHadnlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                    .AddClasses(classes => classes.Where(x =>
                    {
                        var allInterfaces = x.GetInterfaces();
                        return
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableCommandHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableEventHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
            );
        }

        //public static void RegisterAllSagas(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.Scan(scan => scan
        //        .FromAssemblies(assemblies)
        //            .AddClasses(classes => classes.Where(x =>
        //            {
        //                return x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ISaga<>));
        //            }))
        //            .AsSelf()
        //            .WithTransientLifetime()
        //    );
        //}
    }
}
#endif