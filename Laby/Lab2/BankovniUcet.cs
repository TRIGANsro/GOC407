namespace Lab2;

public class BankovniUcet
{
    private decimal _zustatek;
    private readonly Lock _zamek = new ();

    public BankovniUcet(decimal pocatecniZustatek)
    {
        _zustatek = pocatecniZustatek;
    }

    public void VlozPenize(decimal castka)
    {
        Assert.IsTrue(castka > 0, "Vkládaná částka musí být kladná");

        lock (_zamek)
        {
            _zustatek += castka;
        }
    }

    public void VyberPenize(decimal castka)
    {
        Assert.IsTrue(castka > 0, "Vybíraná částka musí být kladná");
        
        lock (_zamek)
        {
            if (castka > _zustatek)
            {
                throw new ArgumentException("Nedostatečný zůstatek");
            }

            _zustatek -= castka;
        }
    }

    public decimal ZjistiZustatek()
    {
        lock (_zamek)
        {
            return _zustatek;
        }
    }    
}