namespace SharpAspect.Sample
{
    [InterceptFor(typeof(CheckFuelAttribute))]
    public class CheckFuelInterceptor : IPropertyInterceptor
    {
        private readonly Logger logger;
        public CheckFuelInterceptor(Logger logger)
        {
            this.logger = logger;
        }

        public void OnGet(IInvocation invocation)
        {

        }

        public void OnSet(IInvocation invocation, object value)
        {
            var hasEnoughFuel = (double) value > 70d;

            if (!hasEnoughFuel)
                throw new System.Exception("Fuel is not enough to launch.");

            logger.LogInfo($"[OnSet] Fuel: %{value}, enough to launch.");
        }
    }
}
