using System.Threading.Tasks;

namespace SharpAspect.Sample
{
    [InterceptFor(typeof(CheckFuelAttribute))]
    public class CheckFuelInterceptor: PropertyInterceptor
    {
        private readonly Logger logger;
        public CheckFuelInterceptor(Logger logger)
        {
            this.logger = logger;
        }

        public override Task OnSet(IInvocation invocation, object value)
        {
            var hasEnoughFuel = (double) value > 70d;

            if (!hasEnoughFuel)
                throw new System.Exception("Fuel is not enough to launch.");

            logger.LogInfo($"[CheckFuel] Fuel: %{value}, enough to launch.");

            return Task.FromResult(Task.CompletedTask);
        }
    }
}
