using System.Diagnostics;

namespace PLINQDemo;

internal class Program
{
    static void Main(string[] args)
    {
        //Random random = new Random();

        //// Create a data source
        //List<int> data = new List<int>(10_000_000);

        //for (int i = 0; i < 10_000_000; i++)
        //{
        //    data.Add(random.Next(1,10));
        //}

        //int sum = 0;
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();
        //foreach (var cislo in data)
        //{
        //    sum += cislo;
        //}
        //stopwatch.Stop();
        //Console.WriteLine(sum + " " + stopwatch.ElapsedTicks);

        //sum = 0;
        //stopwatch.Restart();
        //foreach (var cislo in data)
        //{
        //    sum += cislo;
        //}
        //stopwatch.Stop();
        //Console.WriteLine(sum + " " + stopwatch.ElapsedTicks);


        //sum = 0;
        //stopwatch.Restart();

        //sum = data.AsParallel().Sum();

        //stopwatch.Stop();

        //Console.WriteLine(sum + " " + stopwatch.ElapsedTicks);

        //sum = 0;

        //stopwatch.Restart();

        //Parallel.ForEach(data, (cislo) =>
        //{
        //    Interlocked.Add(ref sum, cislo);
        //});

        //stopwatch.Stop();

        //Console.WriteLine(sum + " " + stopwatch.ElapsedTicks);

        //sum = 0;

        //stopwatch.Restart();

        //Parallel.ForEach(data, (cislo) =>
        //{
        //    sum += cislo;
        //});

        //stopwatch.Stop();

        //Console.WriteLine(sum + " " + stopwatch.ElapsedTicks);

        Random random = new Random();

        int[] data = new int[10_000];
        for (int i = 0; i < 10_000; i++)
        {
            data[i] = random.Next(1,21);
        }

        var query = from cislo in data.AsParallel()
                    orderby cislo
                    select cislo;

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }
}