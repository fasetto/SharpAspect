namespace SharpAspect.Sample
{
    public class Logger
    {
        public void LogInfo(string message)
        {
            System.Console.WriteLine($"[+] {message}");
        }
    }
}
