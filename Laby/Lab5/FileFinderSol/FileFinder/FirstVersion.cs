using System.Threading.Tasks.Dataflow;

namespace FileFinder;

public class FirstVersion
{
    private static TransformManyBlock<string, string> CreateFileFinderBlock(string searchPattern, CancellationToken token)
    {
        return new TransformManyBlock<string, string>(path =>
        {
            try
            {
                EnumerationOptions options = new EnumerationOptions()
                    { IgnoreInaccessible = true, RecurseSubdirectories = true, ReturnSpecialDirectories = false };
                return Directory.EnumerateFiles(path, searchPattern, options);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Chyba při prohledávání složky: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        },
        new ExecutionDataflowBlockOptions { CancellationToken = token });
    }

    private static BatchBlock<string> CreateBatchBlock(int batchSize, CancellationToken token)
    {
        return new BatchBlock<string>(batchSize, new GroupingDataflowBlockOptions
        {
            CancellationToken = token
        });
    }

    private static TransformManyBlock<string[], (string file, int daysOld)> CreateFileAgeBlock(CancellationToken token)
    {
        return new TransformManyBlock<string[], (string file, int daysOld)>(paths =>
        {
            var results = new List<(string file, int daysOld)>();

            foreach (var path in paths)
            {
                try
                {
                    var lastWrite = File.GetLastWriteTimeUtc(path);
                    var days = (int)(DateTime.UtcNow - lastWrite).TotalDays;
                    results.Add((path, days));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Chyba při načítání info o souboru: {ex.Message}");
                    results.Add((path, int.MaxValue));
                }
            }

            return results;
        },
        new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = token
        });
    }

    private static ActionBlock<(string file, int daysOld)> CreateCollectorBlock(List<(string file, int daysOld)> resultList, CancellationToken token)
    {
        return new ActionBlock<(string file, int daysOld)>(item =>
        {
            resultList.Add(item);
        },
        new ExecutionDataflowBlockOptions
        {
            CancellationToken = token
        });
    }

    public static async Task RunFileAgeAnalyzerAsync(string rootPath, string pattern, CancellationToken token)
    {
        var results = new List<(string file, int daysOld)>();

        var fileFinder = CreateFileFinderBlock(pattern, token);
        var batchBlock = CreateBatchBlock(batchSize: 10, token);
        var fileAgeBlock = CreateFileAgeBlock(token);
        var collector = CreateCollectorBlock(results, token);

        fileFinder.LinkTo(batchBlock, new DataflowLinkOptions { PropagateCompletion = true });
        batchBlock.LinkTo(fileAgeBlock, new DataflowLinkOptions { PropagateCompletion = true });
        fileAgeBlock.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        fileFinder.Post(rootPath);
        fileFinder.Complete();

        await collector.Completion;

        PrintSortedResults(results);
    }

    private static void PrintSortedResults(List<(string file, int daysOld)> results)
    {
        using var outputFile = File.CreateText("vystup.txt");
        foreach (var (file, days) in results.OrderBy(x => x.daysOld))
        {
            outputFile.WriteLine($"{file} – {days} dní");
        }
    }
}