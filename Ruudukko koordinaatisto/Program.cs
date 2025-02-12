using System;

namespace Ruudukko_koordinaatisto
{
    

    public readonly struct Koordinaatti
    {
        
        public readonly int X { get; init; }
        public readonly int Y { get; init; }

        
        public Koordinaatti(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool OnVieressa(Koordinaatti toinen)
        {
            int xEro = Math.Abs(X - toinen.X);
            int yEro = Math.Abs(Y - toinen.Y);

            return (xEro == 1 && yEro == 0) || (xEro == 0 && yEro == 1);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Koordinaatti keskipiste = new Koordinaatti(0, 0);
            Koordinaatti[] testKoordinaatit =
            {
            new Koordinaatti(-1, -1),
            new Koordinaatti(-1, 0),
            new Koordinaatti(-1, 1),
            new Koordinaatti(0, -1),
            new Koordinaatti(0, 0),
            new Koordinaatti(0, 1),
            new Koordinaatti(1, -1),
            new Koordinaatti(1, 0),
            new Koordinaatti(1, 1)
            };

            foreach (var koordinaatti in testKoordinaatit)
            {
                Console.WriteLine($"Annettu koordinaatti {koordinaatti} on koordinaatin {keskipiste} vieressä.");
            }
        }
    }
}
