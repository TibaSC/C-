namespace Nuolikauppa
{


    internal class Program
    {


        public static void Main(string[] args)
        {

            Console.WriteLine("Tervetuloa nuolikauppaan, millaisen nuolen haluat");
            KärkiMateriaali valittuKärki;
            while (true)
            {
                Console.WriteLine("Minkälainen kärki (puu, teräs vai timantti)? ");
                string? vastaus = Console.ReadLine();
                if (vastaus.ToLower() == "puu")
                {
                    valittuKärki = KärkiMateriaali.Puu;
                    break;
                }
                else if (vastaus.ToLower() == "teräs")
                {
                    valittuKärki = KärkiMateriaali.Teräs;
                    break;
                }
                else if (vastaus.ToLower() == "timantti")
                {
                    valittuKärki = KärkiMateriaali.Timantti;
                    break;
                }
            }

            SulkaMateriaali valittuSulka;
            while (true)
            {
                Console.WriteLine("Minkälaiset sulat (lehti, kanansulka vai kotkansulka)?");
                string? vastaus = Console.ReadLine();
                if (vastaus.ToLower() == "lehti")
                {
                    valittuSulka = SulkaMateriaali.Lehti;
                    break;
                }
                else if (vastaus.ToLower() == "kanansulka")
                {
                    valittuSulka = SulkaMateriaali.Kanansulka;
                    break;
                }
                else if (vastaus.ToLower() == "kotkansulka")
                {
                    valittuSulka = SulkaMateriaali.Kotkansulka;
                    break;
                }
            }

            sbyte pituusCm;
            while (true)
            {
                Console.WriteLine("Nuolen pituus (60-100cm): ");

                pituusCm = sbyte.Parse(Console.ReadLine());
                if (pituusCm >= 60 && pituusCm <= 100)
                {
                    break;
                }
                
            }
            Nuoli nuoli = new Nuoli(valittuKärki, valittuSulka, pituusCm);
            float hinta = nuoli.AnnaHinta();

            Console.WriteLine($"Nuoli hinta on: {hinta} kultaa.");
        }
    }
}


