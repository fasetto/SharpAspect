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

            // Property interception handled here
            if (invocation.IsSetter())
            {
                var propertyInterceptors = invocation.FindPropertyInterceptors(proxyConfig, serviceProvider);

                foreach (var interceptor in propertyInterceptors)
                    await interceptor.OnSet(invocationContext, invocation.Arguments[0]);

                await proceed(invocation).ConfigureAwait(false);
                return;
            }

            var methodInterceptors = invocation.FindMethodInterceptors(proxyConfig, serviceProvider);

            foreach (var interceptor in methodInterceptors)
                await interceptor.OnBefore(invocationContext);

            try
            {
                await proceed(invocation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                foreach (var interceptor in methodInterceptors)
                    await interceptor.OnError(invocationContext, ex);
            }
            finally
            {
                foreach (var interceptor in methodInterceptors)
                    await interceptor.OnAfter(invocationContext);
            }

        }

        protected override async Task<TResult> InterceptAsync<TResult>(Castle.DynamicProxy.IInvocation invocation, Func<Castle.DynamicProxy.IInvocation, Task<TResult>> proceed)
        {
            var invocationContext = new InvocationContext(invocation);

            // Property interception handled here
            if (invocation.IsGetter())
            {
                var propertyInterceptors = invocation.FindPropertyInterceptors(proxyConfig, serviceProvider);

                foreach (var interceptor in propertyInterceptors)
                    await interceptor.OnGet(invocationContext);

                return await proceed(invocation).ConfigureAwait(false);
            }

            var methodInterceptors = invocation.FindMethodInterceptors(proxyConfig, serviceProvider);

            foreach (var interceptor in methodInterceptors)
                await interceptor.OnBefore(invocationContext);

            TResult result = default(TResult);

            try
            {
                result = await proceed(invocation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                foreach (var interceptor in methodInterceptors)
                    await interceptor.OnError(invocationContext, ex);
            }
            finally
            {
                foreach (var interceptor in methodInterceptors)
                    await interceptor.OnAfter(invocationContext);
            }

            return result;
        }
    }
}
