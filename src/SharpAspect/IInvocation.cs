using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace SharpAspect
{
    public interface IInvocation
    {
        object[] Arguments { get; }
        Type[] GenericArguments { get; }
        object InvocationTarget { get; }
        MethodInfo Method { get; }
        MethodInfo MethodInvocationTarget { get; }
        object Proxy { get; }
        object ReturnValue { get; set; }
        Type TargetType { get; }

        IInvocationProceedInfo CaptureProceedInfo();
        object GetArgumentValue(int index);
        MethodInfo GetConcreteMethod();
        MethodInfo GetConcreteMethodInvocationTarget();
        void SetArgumentValue(int index, object value);
        Exception Exception { get; }
    }
}
