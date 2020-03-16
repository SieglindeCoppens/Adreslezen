using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Straat
    {
        public Gemeente Gemeente { get; set; }
        public int ID { get; set; }
        public string Straatnaam { get; set; }

        public Straat(int id, string straatnaam, Gemeente gemeente)
        {
            ID = id;
            Straatnaam = straatnaam;
            Gemeente = gemeente;
        }
        public override string ToString()
        {
            return $"{ID}: {Gemeente}, {Straatnaam}";
        }
    }
}
