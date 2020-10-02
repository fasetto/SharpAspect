namespace SharpAspect.Sample
{
    public interface IRocket
    {
        string Name { get; set; }
        double Fuel { get; set; }

        void Launch();
    }
}
