using System;

namespace SharpAspect
{
    /// <summary>
    /// For property interception.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyInterceptorAttribute: Attribute
    {

    }
}
