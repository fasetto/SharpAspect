namespace SharpAspect
{
    /// <summary>
    /// Interceptor classes must implement this interface for property interception.
    /// </summary>
    public interface IPropertyInterceptor
    {
        void OnGet(IInvocation invocation);
        void OnSet(IInvocation invocation, object value);
    }
}
