namespace SharpAspect.Sample
{
    public class Rocket: IRocket
    {
        [Log]
        // [Cache]
        public void Launch()
        {
            System.Console.WriteLine("Launching rocket in 3...2.....1 🚀");
        }

        [Log]
        [Cache]
        public string SetRoute(string route)
        {
            System.Console.WriteLine($"Route: {route}");
            return route;
        }
    }
}
