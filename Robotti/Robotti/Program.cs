using System;

namespace Robotti
{
    public interface IRobottiKäsky
    {
        void Suorita(Robotti robotti);
    }
    public class Robotti
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool OnKäynnissä { get; set; }
        public IRobottiKäsky?[] Käskyt { get; } = new IRobottiKäsky?[3];

        public void Suorita()
        {
            foreach (IRobottiKäsky? käsky in Käskyt)
            {
                käsky?.Suorita(this);
                Console.WriteLine($"Robotti: [{X} {Y} {OnKäynnissä}]");
            }
        }
    }

    

    
    public class Käynnistä : IRobottiKäsky
    {
        public  void Suorita(Robotti robotti)
        {
            robotti.OnKäynnissä = true;
        }
    }

    
    public class Sammuta : IRobottiKäsky
    {
        public void Suorita(Robotti robotti)
        {
            robotti.OnKäynnissä = false;
        }
    }

    
    public class YlösKäsky : IRobottiKäsky
    {
        public void Suorita(Robotti robotti)
        {
            if (robotti.OnKäynnissä)
            {
                robotti.Y++;
            }
        }
    }

    
    public class AlasKäsky : IRobottiKäsky
    {
        public void Suorita(Robotti robotti)
        {
            if (robotti.OnKäynnissä)
            {
                robotti.Y--;
            }
        }
    }

    public class VasenKäsky : IRobottiKäsky
    {
        public void Suorita(Robotti robotti)
        {
            if (robotti.OnKäynnissä)
            {
                robotti.X--;
            }
        }
    }

    public class OikeaKäsky : IRobottiKäsky
    {
        public void Suorita(Robotti robotti)
        {
            if (robotti.OnKäynnissä)
            {
                robotti.X++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Robotti robotti = new Robotti();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Mitä komentoja syötetään robotille? Vaihtoehdot: Käynnistä, Sammuta, Ylös, Alas, Oikea, Vasen.");
                string? command = Console.ReadLine()?.ToLower();

                robotti.Käskyt[i] = command switch
                {
                    "käynnistä" => new Käynnistä(),
                    "sammuta" => new Sammuta(),
                    "ylös" => new YlösKäsky(),
                    "alas" => new AlasKäsky(),
                    "vasen" => new VasenKäsky(),
                    "oikea" => new OikeaKäsky(),
                    _ => null
                };
            }

            robotti.Suorita();
        }
    }
}
