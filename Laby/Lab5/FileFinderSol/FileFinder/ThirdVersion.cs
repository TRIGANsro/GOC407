using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FileFinder
{
    internal class ThirdVersion
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
                //BoundedCapacity = 500 // backpressure!
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

            var fileBuffer = CreateFileProducerBlock(token);
            var batchBlock = CreateBatchBlock(batchSize: 10, token);
            var fileAgeBlock = CreateFileAgeBlock(token);
            var collector = CreateCollectorBlock(results, token);

            fileBuffer.LinkTo(batchBlock, new DataflowLinkOptions { PropagateCompletion = true });
            batchBlock.LinkTo(fileAgeBlock, new DataflowLinkOptions { PropagateCompletion = true });
            fileAgeBlock.LinkTo(collector, new DataflowLinkOptions { PropagateCompletion = true });

            // ⏩ PRODUKCE SOUBORŮ ZE SLOŽEK VÍCEVLÁKNOVĚ
            var folderScanner = CreateFolderScannerBlock(pattern, fileBuffer, token);

            // Root + top-level adresáře
            var allFolders = Directory.EnumerateDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
            
            folderScanner.LinkTo(fileBuffer, new DataflowLinkOptions { PropagateCompletion = true });
            
            foreach (var dir in allFolders)
            {
                await folderScanner.SendAsync(dir, token);
            }
            Console.WriteLine("[main] všechny složky odeslány do folderScanner");
            folderScanner.Complete(); // řekni scanneru, že má hotovo
            await folderScanner.Completion; // čekej na dokončení skenování
            Console.WriteLine( "Folder scan hotov");
            //fileBuffer.Complete(); // řekni bufferu, že má hotovo

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
        private static TransformBlock<string, string> CreateFolderScannerBlock(
            string pattern,
            ITargetBlock<string> targetBlock,
            CancellationToken token)
        {
            var block = new TransformBlock<string, string>(async folder =>
                {
                    Console.WriteLine($"[scanner] začínám procházet {folder}");

                    try
                    {
                        EnumerationOptions options = new EnumerationOptions()
                            { IgnoreInaccessible = true, RecurseSubdirectories = true, ReturnSpecialDirectories = false };

                        foreach (var file in Directory.EnumerateFiles(folder, pattern, options))
                        {
                            token.ThrowIfCancellationRequested();

                            if (targetBlock.Completion.IsCompleted)
                            {
                                Console.WriteLine($"❗️POZOR: targetBlock už je Complete(), nelze poslat {file}");
                                break;
                            }

                            await targetBlock.SendAsync(file, token);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"[Scanner] {folder}: {ex.Message}");
                    }

                    return folder; // návratová hodnota se ignoruje
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    //BoundedCapacity = 10,
                    CancellationToken = token
                });

            return block;
        }

    }
}
