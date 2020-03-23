using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Adres
    {
        public string Appartementnummer { get; set; }
        public string Busnummer { get; set; }
        public string Huisnummer { get; set; }
        public string Huisnummerlabel{ get; set; }
        public int ID { get; set; }
        public AdresLocatie Locatie { get; set; }

        public int Postcode { get; set; }
        public Straatnaam Straat { get; set; }

        public Adres(int id, Straatnaam straat, string appartementnummer, string busnummer, string huisnummer, string huisnummerlabel, Gemeente gemeente, int postcode, double x, double y)
        {
            ID = id;
            Straat = straat;
            Appartementnummer = appartementnummer;
            Busnummer = busnummer;
            Huisnummer = huisnummer;
            Huisnummerlabel = huisnummerlabel;
            //Gemeente = gemeente;
            Postcode = postcode;
            Locatie = new AdresLocatie(x, y);

        }
        public override string ToString()
        {
            return $"{ID}, {Straat.straatnaam}, {Postcode}";
        }
    }
}
