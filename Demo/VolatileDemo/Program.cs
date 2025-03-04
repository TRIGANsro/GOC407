using System;
using System.Threading;

class Program
{
    private static volatile bool stop = false; // Sdílená proměnná

    static void Main()
    {
        Thread workerThread = new Thread(() =>
        {
            Console.WriteLine("Vlákno začalo běžet...");
            while (!stop) { } // Nekonečná smyčka, pokud `stop` není synchronizováno
            Console.WriteLine("Vlákno bylo zastaveno.");
        });

        workerThread.Start();

        Thread.Sleep(1000); // Simulace práce
        stop = true;        // Nastavení příznaku (ale změna nemusí být viditelná)

        workerThread.Join();
        Console.WriteLine("Hlavní vlákno skončilo.");
    }
}
