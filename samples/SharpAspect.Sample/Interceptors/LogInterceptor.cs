namespace SharpAspect.Sample
{
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
}
