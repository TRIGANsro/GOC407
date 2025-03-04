using CacheDemoLib;

namespace TestCache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            ConcurrentCache cache = new(5);

            Task t1 = Task.Run(() =>
                { 
                    cache.Add("key1", () => "value1");
                    cache.Add("key2", () => "value2");
                });

            Task t2 = Task.Run(() =>
                {
                    cache.Add("key1", () => "value3");
                    cache.Add("key2", () => "value4");
                });

            Task t3 = Task.Run(() =>
                {
                    cache.Add("key1", () => "value5");
                    cache.Add("key2", () => "value6");
                });

            Task t4 = Task.Run(() =>
                {
                    var data1 = cache.Get("key1");
                    var data2 = cache.Get("key2");
                    Console.WriteLine($"Data1: {data1}");
                    Console.WriteLine($"Data2: {data2}");
                    Console.WriteLine("Cekam 6 sekund");
                    Thread.Sleep(6000);
                    data1 = cache.Get("key1");
                    data2 = cache.Get("key2");
                    Console.WriteLine($"Data1: {data1}");
                    Console.WriteLine($"Data2: {data2}");
                    Console.WriteLine("Cekam 6 sekund");
                    Thread.Sleep(6000);
                    data1 = cache.Get("key1");
                    data2 = cache.Get("key2");
                    Console.WriteLine($"Data1: {data1}");
                    Console.WriteLine($"Data2: {data2}");

                });

            Task.WaitAll(t1, t2, t3, t4);
        }
    }
}
