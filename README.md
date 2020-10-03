
## Getting Started

<a href="https://www.nuget.org/packages/SharpAspect/">
    <img alt="Nuget" src="https://img.shields.io/nuget/vpre/SharpAspect">

</a>

<a href="https://www.nuget.org/packages/SharpAspect/">
    <img src="https://img.shields.io/nuget/dt/SharpAspect">
</a>


```sh
dotnet add package SharpAspect
```

### Defining & mapping your Interceptors


#### Method Interception

```cs
public class LogAttribute: MethodInterceptorAttribute
{
}

[InterceptFor(typeof(LogAttribute))]
public class LogInterceptor : IMethodInterceptor
{
    private readonly Logger logger;

    // The Logger dependency will be resolved using Microsoft's DI container
    public LogInterceptor(Logger logger)
    {
        this.logger = logger;
    }

    public void AfterInvoke(IInvocation invocation)
    {
        // throw new System.NotImplementedException();
    }

    public void BeforeInvoke(IInvocation invocation)
    {
        logger.LogInfo($"Executing method: {invocation.TargetType.FullName}.{invocation.Method.Name}");
    }

    public void OnError(IInvocation invocation, System.Exception e)
    {
        throw new System.NotImplementedException();
        // System.Console.WriteLine(e.Message);
    }
}
```

#### Property Interception

```cs
public class CheckFuelAttribute: PropertyInterceptorAttribute
{
}

[InterceptFor(typeof(CheckFuelAttribute))]
public class CheckFuelInterceptor : IPropertyInterceptor
{
    private readonly Logger logger;
    public CheckFuelInterceptor(Logger logger)
    {
        this.logger = logger;
    }

    public void OnGet(IInvocation invocation)
    {

    }

    public void OnSet(IInvocation invocation, object value)
    {
        var hasEnoughFuel = (double) value > 70d;

        if (!hasEnoughFuel)
            throw new System.Exception("Fuel is not enough to launch.");

        logger.LogInfo($"[OnSet] Fuel: %{value}, enough to launch.");
    }
}
```

Simple logger.

```cs
public class Logger
{
    public void LogInfo(string message)
    {
        System.Console.WriteLine($"[+] {message}");
    }
}
```

### Registering your services

```cs
private static IServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddSingleton<Logger>()
        .AddTransient<IRocket, Rocket>()

        // Call this, after you registerd your services.
        .EnableDynamicProxy()

        .BuildServiceProvider();
}
```

```cs
public interface IRocket
{
    double Fuel { get; set; }
    string Name { get; set; }

    void Launch();
}

// Enabled interception for service type IRocket

[Intercept(typeof(IRocket))]
public class Rocket: IRocket
{
    [CheckFuel]
    public double Fuel { get; set; }
    public string Name { get; set; }

    [Log]
    public void Launch()
    {
        System.Console.WriteLine("Launching rocket in 3...2.....1 🚀");
    }
}
```

```cs
static void Main(string[] args)
{
    var services = ConfigureServices();

    var rocket = services.GetRequiredService<IRocket>();

    rocket.Name = "Falcon 9";
    rocket.Fuel = 90.21d;
    rocket.Launch();

    System.Console.WriteLine($"{rocket.Name} launched successfully. (:");
}
```

### Sample Output

```sh
[+] [OnSet] Fuel: %90.21, enough to launch.
[+] [BeforeInvoke] Executing method: SharpAspect.Sample.Rocket.Launch
Launching rocket in 3...2.....1 🚀
Falcon 9 launched successfully. (:
```
