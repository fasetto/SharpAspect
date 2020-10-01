using System;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    public static class ServiceCollectionExtensions
    {
        private static DynamicProxyConfiguration proxyConfig;
        public static IServiceCollection ConfigureDynamicProxy(this IServiceCollection services, Action<DynamicProxyConfiguration> action)
        {
            proxyConfig = new DynamicProxyConfiguration(services);

            action(proxyConfig);

            return services;
        }

        public static IServiceCollection AddTransientProxy<TService, TImplementation>(this IServiceCollection services)
            where TImplementation: TService
        {
            return AddProxyWithLifeTime<TService, TImplementation>(services, ServiceLifetime.Transient);
        }

        public static IServiceCollection AddSingletonProxy<TService, TImplementation>(this IServiceCollection services)
            where TImplementation: TService
        {
            return AddProxyWithLifeTime<TService, TImplementation>(services, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddScopedProxy<TService, TImplementation>(this IServiceCollection services)
            where TImplementation: TService
        {
            return AddProxyWithLifeTime<TService, TImplementation>(services, ServiceLifetime.Scoped);
        }

        private static IServiceCollection AddProxyWithLifeTime<TService, TImplementation>(IServiceCollection services, ServiceLifetime lifetime)
            where TImplementation: TService
        {
            var serviceProvider = services.BuildServiceProvider();

            var targetObj = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var proxyObj  = new ProxyFactory<TService>(serviceProvider, proxyConfig).CreateProxy(targetObj);

            services.Add(new ServiceDescriptor(typeof(TService), sp => proxyObj, lifetime));

            return services;
        }
    }
}
