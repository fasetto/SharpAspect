using System;

namespace SharpAspect
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptAttribute: Attribute
    {
        public Type ServiceType { get; set; }

        public InterceptAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
