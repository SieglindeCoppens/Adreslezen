using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class Straatnaam
    {
        public Gemeente Gemeente { get; set; }
        public int ID { get; set; }
        public string straatnaam { get; set; }

        public Straatnaam(int id, string straatnaam2, Gemeente gemeente)
        {
            ID = id;
            straatnaam = straatnaam2;
            Gemeente = gemeente;
        }
        public override string ToString()
        {
            return $"{ID}: {Gemeente}, {straatnaam}";
        }
        public override bool Equals(object obj)
        {
            if (obj is Straatnaam)
            {
                Straatnaam other = obj as Straatnaam;
                return (this.ID == other.ID);
            }
            else return false;
        }
    }
}
