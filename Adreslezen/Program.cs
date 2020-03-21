using System;
using System.Collections;
using System.Collections.Generic;

namespace Adreslezen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //string test = "<agiv:APPTNR />";
            //char[] splitsers = { '<', '>' };
            //string[] tests = test.Split(splitsers);
            //Console.WriteLine(tests.Length);
            //foreach(string st in tests)
            //{
            //    Console.WriteLine(st);
            //}

            AdresBeheer ab = new AdresBeheer();
            var gemeentes = ab.LeesAdressen();


            foreach(KeyValuePair<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeente in gemeentes)
            {
                Console.WriteLine(gemeente.Key);
            }

        }
    }
}
