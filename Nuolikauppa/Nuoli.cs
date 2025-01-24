namespace Nuolikauppa
{
    internal enum KärkiMateriaali
    {
        Puu,
        Teräs,
        Timantti
    }

    internal enum SulkaMateriaali
    {
        Lehti,
        Kanansulka,
        Kotkansulka
    }
    
    internal class Nuoli
    { 
        public int AnnaHinta()
        {

        }
        // TODO : Tee konstruktori
        private float puu = 3;
        private float teräs = 5;
        private float timantti = 50;
        private float Lehti = 0;
        private float kanansulka = 1;
        private float kotkansulka = 5;
        // TODO : Tee PalautaHinta metodi
    }
}
