using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    public static class InvocationExtensions
    {
        public static IEnumerable<IPropertyInterceptor> FindPropertyInterceptors(this Castle.DynamicProxy.IInvocation invocation, DynamicProxyConfiguration proxyConfig, IServiceProvider serviceProvider)
        {
            var interceptors = new List<IPropertyInterceptor>();

            var methodName = invocation.Method.Name.Replace("set_", "").Replace("get_", "");
            var methodInfo = invocation.MethodInvocationTarget.DeclaringType.GetProperty(methodName);

            var attributes = methodInfo.GetCustomAttributes()
                .Where(x => x.GetType().IsSubclassOf(typeof(PropertyInterceptorAttribute)))
                .Cast<PropertyInterceptorAttribute>();

            foreach (var attribute in attributes)
            {
                var interceptorType = proxyConfig.Interceptors.FirstOrDefault(x => x.AttributeType == attribute.GetType())?.InterceptorType;

                if (interceptorType == null)
                    throw new InvalidOperationException($"No suitable interceptor found for type {attribute}");

               var interceptorInstance = (IPropertyInterceptor) ActivatorUtilities.CreateInstance(serviceProvider, interceptorType);

               interceptors.Add(interceptorInstance);
            }

            return interceptors;
        }

        public static IEnumerable<IMethodInterceptor> FindMethodInterceptors(this Castle.DynamicProxy.IInvocation invocation, DynamicProxyConfiguration proxyConfig, IServiceProvider serviceProvider)
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

        internal static bool IsGetter(this Castle.DynamicProxy.IInvocation invocation)
        {
            return invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("get_");
        }

        internal static bool IsSetter(this Castle.DynamicProxy.IInvocation invocation)
        {
            return invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_");
        }
    }
}
