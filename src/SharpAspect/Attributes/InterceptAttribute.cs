using System;

namespace SharpAspect
{
    /// <summary>
    /// Enables interception for your services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptAttribute: Attribute
    {
        public Type ServiceType { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="serviceType">Registered service type in the <see cref="Microsoft.Extensions.DependencyInjection.ServiceCollection"/>.</param>
        public InterceptAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
