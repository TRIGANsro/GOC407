using System;
using System.Threading;

class Program
{
    static AutoResetEvent autoEvent = new AutoResetEvent(false);

    static void Main()
    {
        Thread worker1 = new Thread(() => WorkerMethod(1));
        worker1.Start();

        Thread worker2 = new Thread(() => WorkerMethod(2));
        worker2.Start();

        Console.WriteLine("Hlavní vlákno: Čekám 3 sekundy...");
        Thread.Sleep(3000);

        Console.WriteLine("Hlavní vlákno: Signalizuji, že můžeš pokračovat poprvé!");
        autoEvent.Set(); // Nastaví signál a probudí pracovní vlákno.

        worker1.Join(); // Počká na ukončení pracovního vlákna.
        
        Console.WriteLine("Hlavní vlákno: Signalizuji, že můžeš pokračovat podruhé!");
        autoEvent.Set(); // Nastaví signál a probudí pracovní vlákno.
        worker2.Join(); // Počká na ukončení pracovního vlákna.
    }

    static void WorkerMethod(int id)
    {
        Console.WriteLine($"Pracovní vlákno {id}: Čekám na signál...");
        autoEvent.WaitOne(); // Blokuje, dokud hlavní vlákno nepošle signál.

        Console.WriteLine($"Pracovní vlákno {id}: Dostal jsem signál, pokračuji!");
    }
}
