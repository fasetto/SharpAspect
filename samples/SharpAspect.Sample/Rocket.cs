namespace SharpAspect.Sample
{
    public class Rocket: IRocket
    {
        [Log]
        public void Launch()
        {
            System.Console.WriteLine("Launching rocket in 3...2.....1 🚀");
        }
    }
}
