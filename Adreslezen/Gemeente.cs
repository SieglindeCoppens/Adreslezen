using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Gemeente
    {
        public string GemeenteNaam { get; set; }
        public int NIScode { get; set; }

        public Gemeente(int nisCode, string gemeentenaam)
        {
            NIScode = nisCode;
            GemeenteNaam = gemeentenaam;
        }
        public override string ToString()
        {
            return $"{NIScode}: {GemeenteNaam}";
        }
        public override bool Equals(object obj)
        {
            if (obj is Gemeente)
            {
                Gemeente other = obj as Gemeente;
                return (this.NIScode == other.NIScode);
            }
            else return false;
        }
    }
}
