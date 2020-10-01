using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    public static class InvocationExtensions
    {
        public static IEnumerable<IMethodInterceptor> FindInterceptors(this Castle.DynamicProxy.IInvocation invocation, DynamicProxyConfiguration proxyConfig, IServiceProvider serviceProvider)
        {
            var interceptors = new List<IMethodInterceptor>();

            var attributes = invocation.MethodInvocationTarget.GetCustomAttributes()
                .Where(x => x.GetType().IsSubclassOf(typeof(MethodInterceptorAttribute)))
                .Cast<MethodInterceptorAttribute>();

            foreach (var attribute in attributes)
            {
                var interceptorType = proxyConfig.Interceptors.FirstOrDefault(x => x.AttributeType == attribute.GetType())?.InterceptorType;

                if (interceptorType == null)
                    throw new InvalidOperationException($"No suitable interceptor found for type {attribute}");

               var interceptorInstance = (IMethodInterceptor) ActivatorUtilities.CreateInstance(serviceProvider, interceptorType);

               interceptors.Add(interceptorInstance);
            }

            return interceptors;
        }
    }
}
