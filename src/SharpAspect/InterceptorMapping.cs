using System;

namespace SharpAspect
{
    public class InterceptorMapping
    {
        public Type AttributeType { get; }
        public Type InterceptorType { get; }

        public InterceptorMapping(Type attributeType, Type interceptorType)
        {
            AttributeType   = attributeType;
            InterceptorType = interceptorType;
        }
    }

    public class InterceptorMapping<TAttribute, TInterceptor>: InterceptorMapping
    {
        public InterceptorMapping()
            : base(typeof(TAttribute), typeof(TInterceptor))
        {

        }
    }
}
