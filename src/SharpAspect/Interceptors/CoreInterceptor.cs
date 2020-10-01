using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace SharpAspect
{
    internal class CoreInterceptor: AsyncInterceptorBase
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DynamicProxyConfiguration proxyConfig;
        public CoreInterceptor(IServiceProvider serviceProvider, DynamicProxyConfiguration proxyConfig)
        {
            this.serviceProvider = serviceProvider;
            this.proxyConfig = proxyConfig;
        }

        protected override async Task InterceptAsync(Castle.DynamicProxy.IInvocation invocation, Func<Castle.DynamicProxy.IInvocation, Task> proceed)
        {
            var invocationContext = new InvocationContext(invocation);
            var interceptors = invocation.FindInterceptors(proxyConfig, serviceProvider);

            foreach (var interceptor in interceptors)
                interceptor.BeforeInvoke(invocationContext);

            try
            {
                await proceed(invocation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                foreach (var interceptor in interceptors)
                    interceptor.OnError(invocationContext, ex);

                return;
            }

            foreach (var interceptor in interceptors)
                interceptor.AfterInvoke(invocationContext);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(Castle.DynamicProxy.IInvocation invocation, Func<Castle.DynamicProxy.IInvocation, Task<TResult>> proceed)
        {
            var invocationContext = new InvocationContext(invocation);
            var interceptors = invocation.FindInterceptors(proxyConfig, serviceProvider);

            foreach (var interceptor in interceptors)
                interceptor.BeforeInvoke(invocationContext);

            TResult result;

            try
            {
                result = await proceed(invocation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                foreach (var interceptor in interceptors)
                    interceptor.OnError(invocationContext, ex);

                return default(TResult);

            }

            foreach (var interceptor in interceptors)
                interceptor.AfterInvoke(invocationContext);

            return result;
        }
    }
}
