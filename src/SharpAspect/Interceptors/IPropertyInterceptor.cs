namespace SharpAspect
{
    public interface IPropertyInterceptor
    {
        void OnGet(IInvocation invocation);
        void OnSet(IInvocation invocation, object value);
    }
}
