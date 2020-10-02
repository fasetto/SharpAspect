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
            rocket.Launch();
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<Logger>()

                // Order is important here,
                // you must enable the dynamic proxy first before adding your proxied services
                .EnableDynamicProxy()
                .AddTransientProxy<IRocket, Rocket>()

                .BuildServiceProvider();
        }
    }
}
