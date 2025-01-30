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
        sbyte pituusCm;
        SulkaMateriaali Sulka;
        KärkiMateriaali Kärki;

        // tee konstruktori
        public Nuoli(KärkiMateriaali kärki, SulkaMateriaali sulka, float pituusCm)
        {

            this.Kärki = kärki;
            this.Sulka = sulka;
            this.pituusCm = (sbyte)pituusCm;
        }

        public float AnnaHinta()
        {
            float hinta = 0;
            hinta += Kärki switch
            {
                KärkiMateriaali.Puu => 3,
                KärkiMateriaali.Teräs => 5,
                KärkiMateriaali.Timantti => 50,
            };
            hinta += Sulka switch
            {
                SulkaMateriaali.Kanansulka => 1,
                SulkaMateriaali.Kotkansulka => 5,
                SulkaMateriaali.Lehti => 0
            };
            hinta += pituusCm * 0.05f;

            return hinta;
        }


    }
}
