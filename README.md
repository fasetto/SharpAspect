
## Getting Started

<a href="https://www.nuget.org/packages/SharpAspect/">
    <img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/SharpAspect?label=SharpAspect%20%7C%20NuGet">

</a>

<a href="https://www.nuget.org/packages/SharpAspect/">
    <img src="https://img.shields.io/nuget/dt/SharpAspect">
</a>


```sh
dotnet add package SharpAspect
```

### Defining & mapping your Interceptors

All attributes must derive from *MethodInterceptorAttribute* class.

```cs
public class LogAttribute: MethodInterceptorAttribute
{

}
```
<br>

All interceptors also must implement the *IMethodInterceptor* interface and should be marked with `[Interceptor(typeof(TAttribute))]`.

```cs
[Interceptor(typeof(LogAttribute))]
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

        // Order is important here,
        // you must enable the dynamic proxy first before adding your proxied services
        .EnableDynamicProxy()
        .AddTransientProxy<IRocket, Rocket>()

        .BuildServiceProvider();
}
```

```cs
public interface IRocket
{
    void Launch();
}

public class Rocket: IRocket
{
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
    rocket.Launch();
}
```

### Sample Output

```sh
[+] Executing method: SharpAspect.Sample.Rocket.Launch
Launching rocket in 3...2.....1 🚀
```
