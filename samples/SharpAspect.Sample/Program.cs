using System;
using Microsoft.Extensions.DependencyInjection;

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
