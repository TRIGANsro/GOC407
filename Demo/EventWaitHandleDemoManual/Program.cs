using System;
using System.Threading;

class Program
{
    static ManualResetEvent manualEvent = new ManualResetEvent(false);

    static void Main()
    {
        Thread worker1 = new Thread(WorkerMethod);
        Thread worker2 = new Thread(WorkerMethod);

        worker1.Start();
        worker2.Start();

        Console.WriteLine("Hlavní vlákno: Čekám 3 sekundy...");
        Thread.Sleep(3000);

        Console.WriteLine("Hlavní vlákno: Signalizuji, že mohou pokračovat!");
        manualEvent.Set(); // Umožní oběma vláknům pokračovat.

        worker1.Join();
        worker2.Join();
    }

    static void WorkerMethod()
    {
        Console.WriteLine($"Pracovní vlákno {Thread.CurrentThread.ManagedThreadId}: Čekám na signál...");
        manualEvent.WaitOne(); // Blokuje, dokud hlavní vlákno nepošle signál.

        Console.WriteLine($"Pracovní vlákno {Thread.CurrentThread.ManagedThreadId}: Dostal jsem signál, pokračuji!");
    }
}
