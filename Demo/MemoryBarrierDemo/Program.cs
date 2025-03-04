using System;
using System.Threading;

class Program
{
    private static int sharedValue = 0;
    private static bool isValueSet = false;

    static void Main()
    {
        Thread writerThread = new Thread(() =>
        {
            Thread.Sleep(1000);     // Čekání na spuštění čtecího vlákna
            sharedValue = 42;         // Nastavení hodnoty
            Thread.MemoryBarrier();   // Zajištění viditelnosti pro ostatní vlákna
            isValueSet = true;        // Signalizace, že hodnota je připravena
        });

        Thread readerThread = new Thread(() =>
        {
            while (!isValueSet)
            {
                Console.WriteLine("Čtení hodnoty: čekám na nastavení hodnoty...");
                Thread.Sleep(10); // Čekání na nastavení hodnoty
            }

            Thread.MemoryBarrier();   // Zajištění, že čtení sharedValue proběhne až po přečtení isValueSet
            Console.WriteLine($"Čtená hodnota: {sharedValue}");
        });

        writerThread.Start();
        readerThread.Start();

        writerThread.Join();
        readerThread.Join();
    }
}
