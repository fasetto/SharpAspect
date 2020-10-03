using System;

namespace SharpAspect
{
    /// <summary>
    /// Interceptor classes must implement this interface for method interception.
    /// </summary>
    public interface IMethodInterceptor
    {
        void BeforeInvoke(IInvocation invocation);
        void AfterInvoke(IInvocation invocation);
        void OnError(IInvocation invocation, Exception e);
    }
}
