namespace SharpAspect.Sample
{
    [Intercept(typeof(IRocket))]
    public class Rocket: IRocket
    {
        [CheckFuel]
        public double Fuel { get; set; }
        public string Name { get; set; }

        [Log]
        public void Launch()
        {
            System.Console.WriteLine("Launching rocket in 3...2.....1 🚀");
        }
    }
}
