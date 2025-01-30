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
        public sbyte pituusCm { get; set; }
        public SulkaMateriaali sulka { get; set; }
        public KärkiMateriaali kärki { get; set; }
        

        // tee konstruktori
        public Nuoli(KärkiMateriaali kärki, SulkaMateriaali sulka, float pituusCm)
        {

            this.kärki = kärki;
            this.sulka = sulka;
            this.pituusCm = (sbyte)pituusCm;
        }

        public sbyte GetPituus() => pituusCm;
        public SulkaMateriaali GetSulka() => sulka;
        public KärkiMateriaali GetKärki() => kärki;
        public float AnnaHinta()
        {
            float hinta = 0;
            hinta += kärki switch
            {
                KärkiMateriaali.Puu => 3,
                KärkiMateriaali.Teräs => 5,
                KärkiMateriaali.Timantti => 50,
            };
            hinta += sulka switch
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
