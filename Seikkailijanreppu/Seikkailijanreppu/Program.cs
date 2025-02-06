namespace Seikkailijanreppu
{
    public abstract class Tavara
    {
        
        public double Paino { get; private set; }
        public double Tilavuus { get; private set; }

        
        public Tavara(double paino, double tilavuus)
        {
            Paino = paino;
            Tilavuus = tilavuus;
        }
    }
    public class Nuoli : Tavara { public Nuoli() : base(0.1, 0.05) { } }
    public class Jousi : Tavara { public Jousi() : base(1, 4) { } }
    public class Köysi : Tavara { public Köysi() : base(1, 1.5) { } }
    public class Vesi : Tavara { public Vesi() : base(2, 2) { } }
    public class RuokaAnnos : Tavara { public RuokaAnnos() : base(1, 0.5) { } }
    public class Miekka : Tavara { public Miekka() : base(5, 3) { } }

    public class Reppu
    {
        
        private List<Tavara> Tavarat { get; set; }
        public int MaksimiTavaroidenMaara { get; private set; }
        public double MaksimiKantoPaino { get; private set; }
        public double MaksimiTilavuus { get; private set; }

        
        public Reppu(int maksimiTavaroidenMaara, double maksimiKantoPaino, double maksimiTilavuus)
        {
            Tavarat = new List<Tavara>();
            MaksimiTavaroidenMaara = maksimiTavaroidenMaara;
            MaksimiKantoPaino = maksimiKantoPaino;
            MaksimiTilavuus = maksimiTilavuus;
        }

        
        public bool Lisää(Tavara tavara)
        {
            // Check item count
            if (Tavarat.Count >= MaksimiTavaroidenMaara)
                return false;

            // Calculate total weight
            double kokonaispPaino = LaskeKokonaisPaino() + tavara.Paino;
            if (kokonaispPaino > MaksimiKantoPaino)
                return false;

            // Calculate total volume
            double kokonaispTilavuus = LaskeKokonaisTilavuus() + tavara.Tilavuus;
            if (kokonaispTilavuus > MaksimiTilavuus)
                return false;

            
            Tavarat.Add(tavara);
            return true;
        }

        
        public double LaskeKokonaisPaino()
        {
            double kokonaisPaino = 0;
            foreach (var tavara in Tavarat)
            {
                kokonaisPaino += tavara.Paino;
            }
            return kokonaisPaino;
        }

        
        public double LaskeKokonaisTilavuus()
        {
            double kokonaisTilavuus = 0;
            foreach (var tavara in Tavarat)
            {
                kokonaisTilavuus += tavara.Tilavuus;
            }
            return kokonaisTilavuus;
        }

        
        public void NäytäTiedot()
        {
            Console.WriteLine($"Repussa on tällä hetkellä {Tavarat.Count}/{MaksimiTavaroidenMaara} tavaraa, {LaskeKokonaisPaino()}/{MaksimiKantoPaino} painoa ja {LaskeKokonaisTilavuus()}/{MaksimiTilavuus} tilavuus");

        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            
            Reppu pelaajanReppu = new Reppu(
                maksimiTavaroidenMaara: 10,
                maksimiKantoPaino: 30,   
                maksimiTilavuus: 20      
            );

            while (true)
            {
                pelaajanReppu.NäytäTiedot();

                Console.WriteLine("\nMitä haluat lisätä?");
                Console.WriteLine("1 - Nuoli");
                Console.WriteLine("2 - Jousi");
                Console.WriteLine("3 - Köysi");
                Console.WriteLine("4 - Vesi");
                Console.WriteLine("5 - RuokaAnnos");
                Console.WriteLine("6 - Miekka");

                string valinta = Console.ReadLine();

                Tavara uusiTavara = null;
                switch (valinta)
                {
                    case "1": uusiTavara = new Nuoli(); break;
                    case "2": uusiTavara = new Jousi(); break;
                    case "3": uusiTavara = new Köysi(); break;
                    case "4": uusiTavara = new Vesi(); break;
                    case "5": uusiTavara = new RuokaAnnos(); break;
                    case "6": uusiTavara = new Miekka(); break;
                    default:
                        Console.WriteLine("Virheellinen valinta!");
                        continue;
                }

                if (uusiTavara != null)
                {

                    if (!pelaajanReppu.Lisää(uusiTavara))
                    {
                        Console.WriteLine("Tavaraa ei voitu lisätä reppuun!");
                    }
                }
            }
        }
    }
}
