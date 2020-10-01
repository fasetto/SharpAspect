using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace SharpAspect
{
    public class DynamicProxyConfiguration
    {
        public List<InterceptorMapping> Interceptors { get; set; }

        private readonly IServiceCollection services;
        public DynamicProxyConfiguration(IServiceCollection services)
        {
            this.services = services;
            this.Interceptors = new List<InterceptorMapping>();
        }

        public void AddInterceptor<TAttribute, TInterceptor>()
            where TInterceptor: IMethodInterceptor
            where TAttribute: MethodInterceptorAttribute
        {
            this.Interceptors.Add(new InterceptorMapping<TAttribute, TInterceptor>());
        }

        public void AddInterceptor(Type attributeType, Type interceptorType)
        {
            this.Interceptors.Add(new InterceptorMapping(attributeType, interceptorType));
        }
    }
}
