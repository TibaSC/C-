namespace Ovi
{
    internal class Program
    {
        public enum OvenTila { Auki, Kiinni, Lukossa };

        static void Main(string[] args)
        {
            string avaaLukko = "Avaa Lukko";
            string avaa = "Avaa";
            string sulje = "Sulje";
            string lukitse = "Lukitse";
            OvenTila ovenTila = OvenTila.Auki;
            Console.WriteLine($"Hello,");
            Console.WriteLine($"Ovi on Auki. Mitä haluat tehdä? \n1. {avaaLukko}, 2. {avaa},3. {sulje}, 4. {lukitse}\nNyt aloitetaan");

            while (true)
            {
                Console.WriteLine($"Ovi on {ovenTila}. Mitä haluat tehdä?");
                string? vastaus = Console.ReadLine();
                if (ovenTila == OvenTila.Auki && vastaus == sulje)
                {
                    ovenTila = OvenTila.Kiinni;
                }
                else if (ovenTila == OvenTila.Kiinni)
                {
                    if (vastaus == avaa)
                    {
                        ovenTila = OvenTila.Auki;
                    }
                    else if(vastaus == lukitse)
                    {
                        ovenTila = OvenTila.Lukossa;
                    }
                }
                else if (ovenTila == OvenTila.Lukossa && vastaus == avaaLukko)
                {
                    ovenTila = OvenTila.Kiinni;
                }
            }




        }
    }
}
