using System;
using System.Collections.Generic;
using System.Text;

namespace Adreslezen
{
    class AdresLocatie
    {
        private int _id = 0;
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public AdresLocatie(double x, double y)
        {
            X = x;
            Y = y;
            ID = IdGenerator();
        }
        public AdresLocatie(int id, double x, double y)
        {
            X = x;
            Y = y;
            ID = id;
        }

        public int IdGenerator()
        {
            return _id += 1;
        }
    }
}
