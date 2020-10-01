using System;

namespace SharpAspect
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodInterceptorAttribute: Attribute
    {

    }
}
