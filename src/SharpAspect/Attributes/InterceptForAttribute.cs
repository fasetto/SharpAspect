using System;

namespace SharpAspect
{
    /// <summary>
    /// Creates interceptor mapping between your attribute and the interceptor.
    /// </summary>
    /// <remarks>
    /// Methods, properties or any other interceptable members will be intercepted if they have the attribute that is mapped for.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InterceptForAttribute: Attribute
    {
        public Type AttributeType { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="attributeType">Attribute type for the interceptor.</param>
        public InterceptForAttribute(Type attributeType)
        {
            AttributeType = attributeType;
        }
    }
}
