namespace SharpAspect.Sample
{
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
            logger.LogInfo($"[BeforeInvoke] Executing method: {invocation.TargetType.FullName}.{invocation.Method.Name}");
        }

        public void OnError(IInvocation invocation, System.Exception e)
        {
            throw new System.NotImplementedException();
            // System.Console.WriteLine(e.Message);
        }
    }
}
