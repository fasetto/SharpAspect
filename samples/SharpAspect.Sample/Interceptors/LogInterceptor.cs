using System.Threading.Tasks;

namespace SharpAspect.Sample
{
    [InterceptFor(typeof(LogAttribute))]
    public class LogInterceptor: MethodInterceptor
    {
        private readonly Logger logger;

        // The Logger dependency will be resolved using DI container
        public LogInterceptor(Logger logger)
        {
            this.logger = logger;
        }

        public override Task OnBefore(IInvocation invocation)
        {
            logger.LogInfo($"[Log] Executing method: {invocation.TargetType.FullName}.{invocation.Method.Name}");

            return Task.FromResult(Task.CompletedTask);
        }
    }
}
