using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class AdresLocatie
    {
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public AdresLocatie(double x, double y)
        {
            X = x;
            Y = y;
        }
        public AdresLocatie(int id, double x, double y)
        {
            X = x;
            Y = y;
            ID = id;
        }
    }
}
