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


            //foreach(KeyValuePair<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeente in gemeentes)
            //{
            //    Console.WriteLine(gemeente.Key);
            //}
            //Console.WriteLine(gemeentes.Count);

            Gemeente testgemeente = new Gemeente(44021, "Gent");
            Straatnaam teststraat = new Straatnaam(69702, "Beukenlaan", testgemeente);
            Adres testadres = new Adres(2000000034, teststraat,null, null, "15","15",testgemeente,9051, 101301.62, 190958.69);

            Console.WriteLine(testadres.Busnummer == null);

            ab.voegAdresToe(testadres);

        }
    }
}
