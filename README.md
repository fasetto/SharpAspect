
## Getting Started
![NuGet](https://img.shields.io/nuget/vpre/SharpAspect?label=SharpAspect%20%7C%20NuGet) ![downloads](https://img.shields.io/nuget/dt/SharpAspect)

```sh
dotnet add package SharpAspect
```

### Defining Interceptors

All attributes must derive from *MethodInterceptionAttribute* class.

```cs
public class LogAttribute: MethodInterceptorAttribute
{

}

public class CacheAttribute: MethodInterceptorAttribute
{

}
```
<br>

All interceptors also must implement the *IMethodInterceptor* interface.

```cs
public class LogInterceptor : IMethodInterceptor
{
    public void AfterInvoke(IInvocation invocation)
    {
        // throw new System.NotImplementedException();
    }

    public void BeforeInvoke(IInvocation invocation)
    {
        System.Console.WriteLine($"[Logging] {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}");
    }

    public void OnError(IInvocation invocation, System.Exception e)
    {
        throw new System.NotImplementedException();
        // System.Console.WriteLine(e.Message);
    }
}

public class CacheInterceptor : IMethodInterceptor
{
    public void AfterInvoke(IInvocation invocation)
    {
        // throw new System.NotImplementedException();
    }

    public void BeforeInvoke(IInvocation invocation)
    {
        System.Console.WriteLine($"[Caching] {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}");
    }

    public void OnError(IInvocation invocation, System.Exception e)
    {
        throw new System.NotImplementedException();
    }
}
```

### Registering your interceptors

```cs
private static IServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .ConfigureDynamicProxy(c =>
        {
            // Attribute & Interceptor mapping
            c.AddInterceptor<CacheAttribute, CacheInterceptor>();
            c.AddInterceptor<LogAttribute, LogInterceptor>();
        })

        // Transient service
        .AddTransientProxy<IRocket, Rocket>()

        .BuildServiceProvider();
}
```

```cs
public interface IRocket
{
    void Launch();
    string SetRoute(string route);
}

public class Rocket: IRocket
{
    [Log]
    public void Launch()
    {
        System.Console.WriteLine("Launching rocket in 3...2.....1 🚀");
    }

    [Log]
    [Cache]
    public string SetRoute(string route)
    {
        System.Console.WriteLine($"Route: {route}");
        return route;
    }
}
```

```cs
static void Main(string[] args)
{
    var services = ConfigureServices();

    var rocket = services.GetRequiredService<IRocket>();

    rocket.SetRoute("Moon");
    rocket.Launch();
}
```

### Sample Output

```sh
[Logging] SharpAspect.Sample.IRocket.SetRoute
[Caching] SharpAspect.Sample.IRocket.SetRoute
Route: Moon

[Logging] SharpAspect.Sample.IRocket.Launch
Launching rocket in 3...2.....1 🚀
```
