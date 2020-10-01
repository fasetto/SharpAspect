using System;

namespace SharpAspect
{
    public interface IMethodInterceptor
    {
        void BeforeInvoke(IInvocation invocation);
        void AfterInvoke(IInvocation invocation);
        void OnError(IInvocation invocation, Exception e);
    }
}
