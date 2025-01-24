namespace Nuolikauppa
{

    
    internal class Program
    {


        // TODO : Tee konstruktori
        private float puu = 3;
        private float teräs = 5;
        private float timantti = 50;
        private float Lehti = 0;
        private float kanansulka = 1;
        private float kotkansulka = 5;
        // TODO : Tee PalautaHinta metodi


        static void Main(string[] args)
        {

            Console.WriteLine("Tervetuloa nuolikauppaan, millaisen nuolen haluat");
            KärkiMateriaali valittuKärki;
            while (true)
            {
                Console.WriteLine("Minkälainen kärki");
                string? vastaus = Console.ReadLine();
                if (vastaus.ToLower() == "puu")
                {
                    valittuKärki= KärkiMateriaali.Puu;
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
                Console.WriteLine("Minkälaiset sulat");
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
            // Kysy nuolen k'rki, sulka ja pituus
            // Kysy pituutta uudestaan jos se on < 60 || > 100
            // Tulosta nuolen hinta
        }
    }
}
