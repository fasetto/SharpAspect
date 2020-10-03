using System;

namespace SharpAspect
{
    /// <summary>
    /// For method interception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodInterceptorAttribute: Attribute
    {

    }
}
