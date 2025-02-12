using System;

namespace Väritetyt_tavarat
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

    
    public class Nuoli : Tavara
    {
        public Nuoli() : base(0.1, 0.05) { }
        public override string ToString() => "Nuoli";
    }

    public class Jousi : Tavara
    {
        public Jousi() : base(1, 4) { }
        public override string ToString() => "Jousi";
    }

    public class Köysi : Tavara
    {
        public Köysi() : base(1, 1.5) { }
        public override string ToString() => "Köysi";
    }

    public class Vesi : Tavara
    {
        public Vesi() : base(2, 2) { }
        public override string ToString() => "Vesi";
    }

    public class RuokaAnnos : Tavara
    {
        public RuokaAnnos() : base(1, 0.5) { }
        public override string ToString() => "Ruokaa";
    }

    public class Miekka : Tavara
    {
        public Miekka() : base(5, 3) { }
        public override string ToString() => "Miekka";
    }

    // Generic colored item class
    public class VaritettyTavara<T>
    {
        private T tavara;
        private ConsoleColor vari;

        public VaritettyTavara(T tavara, ConsoleColor vari)
        {
            this.tavara = tavara;
            this.vari = vari;
        }

        public void NaytaTavara()
        {
            ConsoleColor alkuperainenVari = Console.ForegroundColor;
            Console.ForegroundColor = vari;
            Console.WriteLine(tavara.ToString());
            Console.ForegroundColor = alkuperainenVari;
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            // Create colored items
            var punainenMiekka = new VaritettyTavara<Miekka>(new Miekka(), ConsoleColor.Blue);
            var sininenVesi = new VaritettyTavara<Vesi>(new Vesi(), ConsoleColor.Green);
            var keltainenKoysi = new VaritettyTavara<Köysi>(new Köysi(), ConsoleColor.Yellow);
            var vihreaJousi = new VaritettyTavara<Jousi>(new Jousi(), ConsoleColor.Red);
            var magentaNuoli = new VaritettyTavara<Nuoli>(new Nuoli(), ConsoleColor.Magenta);
            var cyanRuoka = new VaritettyTavara<RuokaAnnos>(new RuokaAnnos(), ConsoleColor.Cyan);

            // Display all items with their colors
            Console.WriteLine("\nNäytetään kaikki tavarat väreissä:");
            punainenMiekka.NaytaTavara();
            sininenVesi.NaytaTavara();
            keltainenKoysi.NaytaTavara();
            vihreaJousi.NaytaTavara();
            magentaNuoli.NaytaTavara();
            cyanRuoka.NaytaTavara();

        }
    }
}
