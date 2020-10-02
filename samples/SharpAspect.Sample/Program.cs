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

            rocket.Name = "Falcon 9";
            rocket.Fuel = 90.21d;
            rocket.Launch();

            System.Console.WriteLine($"{rocket.Name} launched successfully. (:");
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<Logger>()
                .AddTransient<IRocket, Rocket>()

                .EnableDynamicProxy()

                .BuildServiceProvider();
        }
    }
}
