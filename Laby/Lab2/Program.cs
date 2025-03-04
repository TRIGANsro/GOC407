namespace Lab2;

public class Program
{
    public static void Main()
    {
        BankovniUcet ucet = new BankovniUcet(1000);

        Task vklad1 = new Task(() =>
        {
            System.Console.WriteLine($"Vklad na ucet: 500");
            ucet.VlozPenize(500);
            System.Console.WriteLine($"Zůstatek na účtu: {ucet.ZjistiZustatek()}");
        });

        Task vklad2 = new Task(() =>
        {
            System.Console.WriteLine($"Vklad na ucet: 200");
            ucet.VlozPenize(200);
            System.Console.WriteLine($"Zůstatek na účtu: {ucet.ZjistiZustatek()}");
        });

        Task vyber1 = new Task(() =>
        {
            System.Console.WriteLine($"Vyber uctu: 200");
            ucet.VyberPenize(200);
            System.Console.WriteLine($"Zůstatek na účtu: {ucet.ZjistiZustatek()}");
        });

        Task vyber2 = new Task(() =>
        {
            System.Console.WriteLine($"Vyber uctu: 500");
            ucet.VyberPenize(500);
            System.Console.WriteLine($"Zůstatek na účtu: {ucet.ZjistiZustatek()}");
        });

        vklad1.Start();
        vklad2.Start();
        vyber1.Start();
        vyber2.Start();

        Task.WaitAll(vklad1, vklad2, vyber1, vyber2);

        System.Console.WriteLine($"Zůstatek na účtu: {ucet.ZjistiZustatek()}");
    }
}