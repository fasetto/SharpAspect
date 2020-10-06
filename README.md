
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

**SharpAspect** is an AOP *(Aspect-Oriented Programming)* package for .Net <br>
It depends on *Castle.Core* DynamicProxy. <br>
Currently only supports method and property interception.

Take advantage of run-time interception for your next project.

Check the [wiki](https://github.com/fasetto/SharpAspect/wiki) page for more samples and documentation.

### Defining & Mapping your Interceptors

#### Method Interception

```cs
public class LogAttribute: MethodInterceptorAttribute
{
}

[InterceptFor(typeof(LogAttribute))]
public class LogInterceptor: MethodInterceptor
{
    private readonly Logger logger;

    // The Logger dependency will be resolved using Microsoft's DI container
    public LogInterceptor(Logger logger)
    {
        this.logger = logger;
    }

    // MethodInterceptor class provides OnBefore, OnAfter and OnError methods.
    // You can override these methods to seperate the logic you don't want in your actual method.
    public override Task OnBefore(IInvocation invocation)
    {
        logger.LogInfo($"[Log] Executing method: {invocation.TargetType.FullName}.{invocation.Method.Name}");

        return Task.FromResult(Task.CompletedTask);
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

        // Call this, after you registered your services.
        .EnableDynamicProxy()

        .BuildServiceProvider();
}
```

```cs
public interface IRocket
{
    string Name { get; set; }

    void Launch();
}

// Enabled interception for service type IRocket

[Intercept(typeof(IRocket))]
public class Rocket: IRocket
{
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
    rocket.Launch();

    System.Console.WriteLine($"{rocket.Name} launched successfully. (:");
}
```

### Sample Output

```sh
[+] [Log] Executing method: SharpAspect.Sample.Rocket.Launch
Launching rocket in 3...2.....1 🚀
Falcon 9 launched successfully. (:
```
