using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    internal class ProxyFactory<T>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DynamicProxyConfiguration proxyConfig;
        private readonly IProxyGenerator proxyGenerator;
        public ProxyFactory(IServiceProvider serviceProvider, DynamicProxyConfiguration proxyConfig)
        {
            this.serviceProvider = serviceProvider;
            this.proxyConfig     = proxyConfig;
            this.proxyGenerator  = serviceProvider.GetRequiredService<IProxyGenerator>();
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
