#if NET47
using Autofac;
using System.Security;
#else
using Microsoft.Extensions.DependencyInjection;
#endif
using CQRSlite.Config;
using System;
using System.Collections.Generic;

namespace Sds.CqrsLite
{
#if NET47
    public class CqrsLiteDependencyResolver : IServiceLocator
    {
        private readonly ILifetimeScope container;

        public CqrsLiteDependencyResolver(ILifetimeScope container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        [SecurityCritical]
        public object GetService(Type serviceType)
        {
            return this.container.Resolve(serviceType);
        }

        [SecurityCritical]
        public T GetService<T>()
        {
            return this.container.Resolve<T>();
        }

        public ILifetimeScope ApplicationContainer
        {
            get
            {
                return this.container;
            }
        }
    }
#else
    public class CqrsLiteDependencyResolver : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public CqrsLiteDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
#endif
}
