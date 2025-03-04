class Singleton
{
    private static Singleton? instance;
    private static object lockObject = new object();

    public static Singleton Instance
    {
        get
        {
            if (instance == null) // Kontrola mimo zámek
            {
                lock (lockObject)
                {
                    if (instance == null) // Kontrola uvnitř zámku
                    {
                        Singleton temp = new Singleton();
                        Thread.MemoryBarrier(); // Zajištění správného pořadí zápisu
                        instance = temp;
                    }
                }
            }
            return instance;
        }
    }

    private Singleton()
    {
        // Dlouho běžící soukromý konstruktor
        System.Console.WriteLine("Singleton vytvořen");
    }
}
