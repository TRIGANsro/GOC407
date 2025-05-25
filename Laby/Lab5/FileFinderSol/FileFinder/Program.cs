
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
            //await FirstVersion.RunFileAgeAnalyzerAsync(@"E:\zdroje", "*.cs", cts.Token);
            //await SecondVersion.RunFileAgeAnalyzerAsync(@"E:\zdroje", "*.cs", cts.Token);
            await FourthVersion.RunFileAgeAnalyzerAsync(@"E:\zdroje", "*.cs", cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operace byla zrušena.");
        }
    }

    
}
