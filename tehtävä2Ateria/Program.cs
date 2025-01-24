namespace tehtävä2Ateria
{
    enum Materiaali
    {
        Muovi,
        Hiilikuitu,
        Teräs
    }
    class Auto
    {
        public int renkaita;
        string mallinimi;
        string valimistaja;
        public float huippunopeus;
        Color väri;
        Materiaali korinMateriaali;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //Luo olio luokkamäärituksen perusteella
            Auto toyota = new Auto();

            toyota.huippunopeus = 50.0f;
            Console.WriteLine("Toyotassa on" + toyota.renkaita + "rengasta");
            Auto mersu = new Auto();
        }
    }
}
