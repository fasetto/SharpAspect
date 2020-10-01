using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SharpAspect
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection EnableDynamicProxy(this IServiceCollection services)
        {
            var proxyConfig = new DynamicProxyConfiguration(services);

            var interceptors = Assembly.GetEntryAssembly().GetTypes()
                .Where(x => x.GetCustomAttribute<InterceptorAttribute>() != null);

            foreach (var interceptor in interceptors)
            {
                var attribute = interceptor.GetCustomAttribute<InterceptorAttribute>().AttributeType;
                proxyConfig.AddInterceptor(attribute, interceptor);
            }

            services.TryAddSingleton(proxyConfig);
            services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

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

            var proxyConfig = serviceProvider.GetRequiredService<DynamicProxyConfiguration>();
            var targetObj   = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var proxyObj    = new ProxyFactory<TService>(serviceProvider, proxyConfig).CreateProxy(targetObj);

            services.Add(new ServiceDescriptor(typeof(TService), sp => proxyObj, lifetime));

            return services;
        }
    }
}
