using System.Threading.Tasks.Dataflow;

namespace FileFinder;

public class SecondVersion
{
    private static BatchBlock<string> CreateBatchBlock(int batchSize, CancellationToken token)
    {
        return new BatchBlock<string>(batchSize, new GroupingDataflowBlockOptions
        {
            CancellationToken = token
        });
    }

    private static BufferBlock<string> CreateFileProducerBlock(CancellationToken token)
    {
        return new BufferBlock<string>(new DataflowBlockOptions
        {
            CancellationToken = token,
            BoundedCapacity = 500 // backpressure!
        });
    }

    private static Task ProduceFilesAsync(BufferBlock<string> target, string root, string pattern, CancellationToken token)
    {
        return Task.Run(async () =>
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories))
                {
                    token.ThrowIfCancellationRequested();
                    await target.SendAsync(file, token);
                }
            }
            finally
            {
                target.Complete();
            }
        }, token);
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

        var fileBuffer = CreateFileProducerBlock(token);
        var batchBlock = CreateBatchBlock(batchSize: 10, token);
        var fileAgeBlock = CreateFileAgeBlock(token);
        var collector = CreateCollectorBlock(results, token);

        fileBuffer.LinkTo(batchBlock, new DataflowLinkOptions { PropagateCompletion = true });
        batchBlock.LinkTo(fileAgeBlock, new DataflowLinkOptions { PropagateCompletion = true });
        fileAgeBlock.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

        // spustit hledání jako task (neblokuje hlavní vlákno)
        _ = ProduceFilesAsync(fileBuffer, rootPath, pattern, token);


        await collector.Completion;

        PrintSortedResults(results);
    }

    private static void PrintSortedResults(List<(string file, int daysOld)> results)
    {
        foreach (var (file, days) in results.OrderBy(x => x.daysOld))
        {
            Console.WriteLine($"{file} – {days} dní");
        }
    }
}