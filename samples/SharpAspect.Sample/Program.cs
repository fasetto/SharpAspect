using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

// https://blog.zhaytam.com/2020/08/18/aspnetcore-dynamic-proxies-for-aop/
// https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-leaking-this.md

namespace SharpAspect.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();

            var rocket = services.GetRequiredService<IRocket>();

            rocket.SetRoute("Moon");
            rocket.Launch();
        }

        private static IServiceProvider ConfigureServices()
        {
            var proxyGenerator = new ProxyGenerator();

            return new ServiceCollection()
                .ConfigureDynamicProxy(c =>
                {
                    c.AddInterceptor<CacheAttribute, CacheInterceptor>();
                    c.AddInterceptor<LogAttribute, LogInterceptor>();
                })

                .AddTransientProxy<IRocket, Rocket>()

                .BuildServiceProvider();
        }
    }
}
