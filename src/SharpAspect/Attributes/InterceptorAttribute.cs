using System;

namespace SharpAspect
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptorAttribute: Attribute
    {
        public Type AttributeType { get; set; }

        public InterceptorAttribute(Type attributeType)
        {
            AttributeType = attributeType;
        }
    }
}
