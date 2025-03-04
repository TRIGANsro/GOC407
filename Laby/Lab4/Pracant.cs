namespace Lab4;

public class Pracant
{
    //Dlouha akce podporuje info o postupu a požadavek na Cancel
    public string DlouhaAkce(int data, IProgress<(int stav, string stavText)>? progress = null, CancellationToken? cancellation = null)
    {
        //Simulace dlouheho vypoctu
        for (int i = 1; i < 11; i++)
        {
            Thread.Sleep(500); //zablokujeme vlákno na 0,5 vteriny

            //Reportuji aktualni stav prace
            progress?.Report(((i*10),$"Je hotovo {i}/10"));

            //Testuji zda neni požadavek na Cancel
            if (cancellation is not null && cancellation.Value.IsCancellationRequested)
            {
                //Uklidim rozdelanou praci
                cancellation.Value.ThrowIfCancellationRequested();
            }
        }

        return "HOTOVO " + data;
    }
}