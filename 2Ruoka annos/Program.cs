namespace _2Ruoka_annos
{
    internal class Program
    {
        enum PääraakaAine
        {
            nautaa, kanaa, kasviksia
        }
        enum Lisuke
        {
            perunaa, riisiä, pastaa
        }
        enum Kastike
        {
            curry, hapanimelä, pippuri, chili
        }
        class Ateria
        {
            public string pääaine;
            public string lisuke;
            public string kastike;
        }
        static void Main(string[] args)
        {
            Ateria ateria = new Ateria();
            //while (true)
            //{
            //    Console.WriteLine("Valitse pääraaka aine: nautaa, kanaa, kasviksia");
            //    string? vastaus=Console.ReadLine();
            //    if (vastaus == PääraakaAine.nautaa.ToString())
            //    {
            //        ateria.pääaine = PääraakaAine.nautaa;
            //        break;
            //    }
            //    else if (vastaus == PääraakaAine.kanaa.ToString())
            //    {
            //        ateria.pääaine = PääraakaAine.kanaa;
            //        break;
            //    }
            //    else if (vastaus == PääraakaAine.kasviksia.ToString())
            //    {
            //        ateria.pääaine = PääraakaAine.kasviksia;
            //        break;
            //    }

            //}
            while (true)
            {
                Console.WriteLine("Valitse pääraaka aine: nautaa, kanaa, kasviksia");
                string? vastaus = Console.ReadLine();
                if (Enum.IsDefined(typeof(PääraakaAine), vastaus))
                {
                    ateria.pääaine = vastaus;
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("Valitse lisuke: perunaa, riisiä, pastaa");
                string? vastaus = Console.ReadLine();
                if (vastaus == Lisuke.perunaa.ToString() || vastaus == Lisuke.riisiä.ToString() || vastaus == Lisuke.pastaa.ToString())
                {
                    ateria.lisuke = vastaus;
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("Valitse pääraaka aine: nautaa, kanaa, kasviksia");
                string? vastaus = Console.ReadLine();
                if (Enum.IsDefined(typeof(PääraakaAine), vastaus))
                {
                    ateria.pääaine = vastaus;
                    break;
                }
            }




            Console.WriteLine("Valitse kastike: curry, hapanimelä, pippuri, chili");
            Console.ReadLine();

            Console.WriteLine($"{ateria.pääaine} ja {ateria.lisuke} {ateria.kastike}-kastikkeella");
        }
    }
}
