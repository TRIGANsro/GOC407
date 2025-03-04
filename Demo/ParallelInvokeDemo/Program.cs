namespace ParallelInvokeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Parallel.Invoke(
                () => Akce("Akce 1"),
                () => Akce("Akce 2"),
                () => Akce("Akce 3"),
                () => Akce("Akce 4"),
                () => Akce("Akce 5")
            );

            Console.WriteLine("Hotovo");

            Parallel.For(1,11, i => Console.WriteLine(i));

            Console.WriteLine("**************************");

            var pole = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Parallel.ForEach(pole, i => Console.WriteLine(i));
        }

        private static void Akce(string message)
        {
            Console.WriteLine(message);
        }
    }
}
