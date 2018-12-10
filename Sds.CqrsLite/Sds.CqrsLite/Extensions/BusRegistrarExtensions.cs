using CQRSlite.Messages;
using System.Linq;
using System.Reflection;

namespace Sds.CqrsLite
{
    public static class BusRegistrarExtensions
    {
        public static void Register(this BusRegistrar registrar, Assembly assembly)
        {
            var allHandlers = assembly.GetTypes()
                .Where(t =>
                {
                    var interfaces = t.GetInterfaces();

                    return
                        interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>)) ||
                        interfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>));
                });

            if (allHandlers.Any())
            {
                registrar.Register(allHandlers.First());
            }
        }

        public static void RegisterEntryAssembly(this BusRegistrar registrar)
        {
            registrar.Register(Assembly.GetEntryAssembly());
        }
    }
}
