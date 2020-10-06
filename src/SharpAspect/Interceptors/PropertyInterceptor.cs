using System.Threading.Tasks;

namespace SharpAspect
{
    /// <summary>
    /// Interceptor classes must implement this interface for property interception.
    /// </summary>
    /// <remarks><see cref="OnGet"/> and <see cref="OnSet"/> methods will be called before getter or setter called.</remarks>
    public interface IPropertyInterceptor
    {
        Task OnGet(IInvocation invocation);
        Task OnSet(IInvocation invocation, object value);
    }

    /// <inheritdoc cref="IPropertyInterceptor"/>
    public abstract class PropertyInterceptor: IPropertyInterceptor
    {
        public virtual Task OnGet(IInvocation invocation)
        {
            return Task.FromResult(Task.CompletedTask);
        }
        public virtual Task OnSet(IInvocation invocation, object value)
        {
            return Task.FromResult(Task.CompletedTask);
        }
    }
}
