using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    internal class ProxyFactory
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly DynamicProxyConfiguration proxyConfig;
        protected readonly IProxyGenerator proxyGenerator;
        public ProxyFactory(IServiceProvider serviceProvider, DynamicProxyConfiguration proxyConfig)
        {
            this.serviceProvider = serviceProvider;
            this.proxyConfig     = proxyConfig;
            this.proxyGenerator  = serviceProvider.GetRequiredService<IProxyGenerator>();
        }

        public object CreateProxy(Type serviceType, object target)
        {
            var interceptor = new CoreInterceptor(serviceProvider, proxyConfig);
            var proxy       = proxyGenerator.CreateInterfaceProxyWithTarget(serviceType, target, interceptor);

            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy));

            return proxy;
        }
    }

    internal class ProxyFactory<T> : ProxyFactory
    {
        public ProxyFactory(IServiceProvider serviceProvider, DynamicProxyConfiguration proxyConfig)
            : base(serviceProvider, proxyConfig)
        {
        }

        public T CreateProxy(T target)
        {
            var interceptor = new CoreInterceptor(serviceProvider, proxyConfig);
            var proxy       = proxyGenerator.CreateInterfaceProxyWithTarget(typeof(T), target, interceptor);

            if (proxy == null)
                throw new ArgumentNullException(nameof(proxy));

            return (T) proxy;
        }
    }
}
