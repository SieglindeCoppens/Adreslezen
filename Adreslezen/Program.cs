using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Adreslezen
{
    class Program
    {
        static void Main(string[] args)
        {
            DbProviderFactories.RegisterFactory("sqlserver", SqlClientFactory.Instance);
            string connectionString = "Data Source=DESKTOP-HT91N8R\\SQLEXPRESS;Initial Catalog=db_Straatlezen;Integrated Security=True";
            DbProviderFactory sqlFactory = DbProviderFactories.GetFactory("sqlserver");

            AdresBeheer ab = new AdresBeheer(sqlFactory, connectionString);


            //string test = "<agiv:APPTNR />";
            //char[] splitsers = { '<', '>' };
            //string[] tests = test.Split(splitsers);
            //Console.WriteLine(tests.Length);
            //foreach(string st in tests)
            //{
            //    Console.WriteLine(st);
            //}

            //var gemeentes = ab.LeesAdressen();
            //ab.VulDatabankOp(gemeentes);

            ab.geefAdres(1000322692);

            var straten = ab.geefStraten("Gavere");
            foreach(string straat in straten)
            {
                Console.WriteLine(straat);
            }

            var adressen = ab.geefAdressen(69428);

            //int aantalStraten = 0;
            //foreach (KeyValuePair<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeente in gemeentes)
            //{
            //    aantalStraten += gemeente.Value.Count;
            //}
            //Console.WriteLine(aantalStraten);

            //Gemeente testgemeente = new Gemeente(44021, "Gent");
            //Straatnaam teststraat = new Straatnaam(69702, "Beukenlaan", testgemeente);
            //Adres testadres = new Adres(2000000034, teststraat,null, null, "15","15",testgemeente,9051, 101301.62, 190958.69);
            // ab.voegAdresToe(testadres);



        }
    }
}
