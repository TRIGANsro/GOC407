namespace LOHBuffers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BufferPool pool = new BufferPool(4);

            for (int i = 0; i < 10; i++)
            {
                Task.Run(() => Akce(pool));
                
            }

            Console.ReadKey();
            

            for (int i = 0; i < 10; i++)
            {
                Task.Run(() => Akce(pool));

            }

            Console.ReadKey();

        }

        static void Akce(BufferPool pool)
        {
            Console.WriteLine("Start vlakna - " + Thread.CurrentThread.ManagedThreadId);
            using (IBufferRegistration bufferRegistration = pool.GetBuffer())
            {
                var data = bufferRegistration.Buffer;
                // use buffer
                Thread.Sleep(1000);
                Console.WriteLine("Vlakno konci - " + Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
