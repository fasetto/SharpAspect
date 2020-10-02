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

            var typesInAssembly = Assembly.GetEntryAssembly().GetTypes();

            AddInterceptorMappings(proxyConfig, typesInAssembly);

            services.TryAddSingleton(proxyConfig);
            services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

            var serviceTypes = typesInAssembly
                .Where(x => x.GetCustomAttribute<InterceptAttribute>() != null);

            var serviceProvider = services.BuildServiceProvider();

            foreach (var type in serviceTypes)
            {
                var serviceType = type.GetCustomAttribute<InterceptAttribute>().ServiceType;

                var targetObj = serviceProvider.GetRequiredService(serviceType);
                var proxyObj = new ProxyFactory(serviceProvider, proxyConfig).CreateProxy(serviceType, targetObj);

                var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == serviceType);

                // Remove previous implementation and add the decorated service.
                services.Remove(serviceDescriptor);
                services.Add(new ServiceDescriptor(serviceType, sp => proxyObj, serviceDescriptor.Lifetime));
            }

            return services;
        }

        private static void AddInterceptorMappings(DynamicProxyConfiguration proxyConfig, System.Type[] typesInAssembly)
        {
            var interceptors = typesInAssembly
                .Where(x => x.GetCustomAttribute<InterceptorAttribute>() != null);

            foreach (var interceptor in interceptors)
            {
                var attribute = interceptor.GetCustomAttribute<InterceptorAttribute>().AttributeType;
                proxyConfig.AddInterceptor(attribute, interceptor);
            }
        }
    }
}
