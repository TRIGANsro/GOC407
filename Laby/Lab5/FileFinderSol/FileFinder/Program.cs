
using System.Threading.Tasks.Dataflow;

namespace FileFinder;
class Program
{
    public static async Task Main()
    {
        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
            Console.WriteLine("\nZrušeno uživatelem...");
        };

        try
        {
            await FirstVersion.RunFileAgeAnalyzerAsync(@"C:\", "*.*", cts.Token);
            //await SecondVersion.RunFileAgeAnalyzerAsync(@"C:\", "*.*", cts.Token);
            //await ThirdVersion.RunFileAgeAnalyzerAsync(@"C:\", "*.*", cts.Token);
            //await FourthVersion.RunFileAgeAnalyzerAsync(@"C:\", "*.*", cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operace byla zrušena.");
        }
    }

    
}
