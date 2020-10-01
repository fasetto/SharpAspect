using System;
using System.Reflection;

namespace SharpAspect
{
    internal class InvocationContext : IInvocation
    {
        private readonly Castle.DynamicProxy.IInvocation invocation;
        public InvocationContext(Castle.DynamicProxy.IInvocation invocation)
        {
            this.invocation = invocation;
        }

        public Exception Exception { get; set; }

        public object[] Arguments => invocation.Arguments;

        public Type[] GenericArguments => invocation.GenericArguments;

        public object InvocationTarget => invocation.InvocationTarget;

        public MethodInfo Method => invocation.Method;

        public MethodInfo MethodInvocationTarget => invocation.MethodInvocationTarget;

        public object Proxy => invocation.Proxy;

        public object ReturnValue
        {
            get => invocation.ReturnValue;
            set => invocation.ReturnValue = value;
        }

        public Type TargetType => invocation.TargetType;

        public Castle.DynamicProxy.IInvocationProceedInfo CaptureProceedInfo() => invocation.CaptureProceedInfo();

        public object GetArgumentValue(int index) => invocation.GetArgumentValue(index);

        public MethodInfo GetConcreteMethod() => invocation.GetConcreteMethod();

        public MethodInfo GetConcreteMethodInvocationTarget() => invocation.GetConcreteMethodInvocationTarget();

        public void SetArgumentValue(int index, object value) => invocation.SetArgumentValue(index, value);
    }
}
