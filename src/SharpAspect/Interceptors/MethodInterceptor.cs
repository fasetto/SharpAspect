using System;
using System.Threading.Tasks;

namespace SharpAspect
{
    /// <summary>
    /// Interceptor classes must implement this interface for method interception.
    /// </summary>
    public interface IMethodInterceptor
    {
        Task OnBefore(IInvocation invocation);
        Task OnAfter(IInvocation invocation);
        Task OnError(IInvocation invocation, Exception e);
    }

    ///<inheritdoc cref="IMethodInterceptor"/>
    public abstract class MethodInterceptor: IMethodInterceptor
    {
        public virtual Task OnAfter(IInvocation invocation)
        {
            return Task.FromResult(Task.CompletedTask);
        }

        public virtual Task OnBefore(IInvocation invocation)
        {
            return Task.FromResult(Task.CompletedTask);
        }

        public virtual Task OnError(IInvocation invocation, Exception e)
        {
            throw e;
        }
    }
}
