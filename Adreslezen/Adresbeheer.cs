//1. gegevens lezen v/anuit het bestand en importeren in dbopppppp
//2. methode die nieuw adres aan databank toevoegt (enkel adres en adreslocatie opvullen!! Straatnaam en Gemeente bestaan zogezegd reeds)
//public void voegAdresToe(Adres adres){...}
//dit moet een transactie zijn! Adres en adreslocatie worden allebei aangepast of allebei niet! 



using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Adreslezen
{
    class AdresBeheer
    {

        public Dictionary<Gemeente,Dictionary<Straatnaam, List<Adres>>> LeesAdressen()
        {
            //Dictionary<Straatnaam, List<Adres>> straatnamen = new Dictionary<Straatnaam, List<Adres>>();
            Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeentes = new Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>>();
            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\adresbeheer\CrabAdr.gml"))
            {
                string input = null;
                char[] splitsers = { '<', '>' };

                int id = 0;
                int straatnaamId = 0;
                string straatnaam = "";
                string huisnummer = null;
                string appartementnummer = null;
                string busnummer = null;
                string huisnrLabel = null;
                int nis = 0;
                string gemeentenaam = null;
                double x = 0;
                double y = 0;
                int postcode = 0;


                for (int t = 0; t < 7; t++)
                    sr.ReadLine();

                while ((sr.ReadLine()) != null)
                {
                    for (int teller = 1; teller <= 22; teller++)
                    {
                        input= sr.ReadLine();
                        if(input == null)
                        {
                            break;
                        }
                        else if (teller == 1 || (teller >= 12 && teller <= 15) || teller >= 18)
                        {
                        }
                        else if (teller == 2)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                id = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 3)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                straatnaamId = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 4)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                straatnaam = inputs[2];
                            }
                        }
                        else if (teller == 5)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                huisnummer = inputs[2];
                            }
                        }
                        else if (teller == 6)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                appartementnummer = inputs[2];
                            }
                        }
                        else if (teller == 7)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                busnummer = inputs[2];
                            }
                        }
                        else if (teller == 8)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                huisnrLabel = inputs[2];
                            }
                        }
                        else if(teller == 9)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                nis = int.Parse(inputs[2]);
                            }
                        }
                        else if(teller == 10)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                gemeentenaam = inputs[2];
                            }
                        }
                        else if (teller == 11)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                postcode = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 16)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                x = double.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 17)
                        { 
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                y = double.Parse(inputs[2]);
                            }
                        }
                    }
                    //kijken of de gemeente al in de dictionary zit, zo niet: gemeente maken
                    Gemeente gemeente = new Gemeente(nis, gemeentenaam);
                    if (!gemeentes.ContainsKey(gemeente))
                    {
                        gemeentes.Add(gemeente, new Dictionary<Straatnaam, List<Adres>>());
                    }

                    //kijken of de straat al in de dictionary zit, zo niet: straat maken
                    Straatnaam straat = new Straatnaam(straatnaamId, straatnaam, gemeente);
                    if (!gemeentes[gemeente].ContainsKey(straat))
                    {

                        gemeentes[gemeente].Add(straat, new List<Adres>());
                    }

                    //adres aanmaken
                    Adres adres = new Adres(id, straat, appartementnummer, busnummer, huisnummer, huisnrLabel, gemeente, postcode, x, y);

                    //adres toevoegen aan dictionary
                    gemeentes[gemeente][straat].Add(adres);
                }
            }
            return gemeentes;
        }

        // ---------------------------------------------- INGELEZEN ADRESSEN OPVULLEN IN DE DATABANK ------------------------------------------------------------- //

        public void VulDatabankOp(Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeentes)
        {






        }









        /// DEEL 2 ------------------------------------------------------------------------------------------------///
       
        private DbProviderFactory sqlFactory;
        private string connectionString;

        public AdresBeheer(DbProviderFactory sqlFactory, string connectionString)
        {
            this.sqlFactory = sqlFactory;
            this.connectionString = connectionString;
        }
        private DbConnection getConnection()
        {
            DbConnection connection = sqlFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        public void voegAdresToe(Adres adres)
        {
            DbConnection connection = getConnection();
            string query = "INSERT INTO dbo.adres(Id,straatnaamID,huisnummer,appartementnummer,busnummer,huisnummerlabel,adreslocatieID) VALUES(@Id,@straatnaamID,@huisnummer,@appartementnummer,@busnummer,@huisnummerlabel,@adreslocatieID)";
            string query2 = "INSERT INTO dbo.adresLocatie(Id,X,Y) VALUES(@AdreslocatieId,@X,@Y)";

            using (DbCommand command = connection.CreateCommand())
            using (DbCommand command2 = connection.CreateCommand())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command2.Transaction = transaction;

                try
                {
                    DbParameter parId = sqlFactory.CreateParameter();
                    parId.ParameterName = "@Id";
                    parId.DbType = DbType.Int32;
                    command.Parameters.Add(parId);
                    DbParameter parStraatnaamID = sqlFactory.CreateParameter();
                    parStraatnaamID.ParameterName = "@straatnaamID";
                    parStraatnaamID.DbType = DbType.Int32;
                    command.Parameters.Add(parStraatnaamID);
                    DbParameter parHuisnummer = sqlFactory.CreateParameter();
                    parHuisnummer.ParameterName = "@huisnummer";
                    parHuisnummer.DbType = DbType.String;
                    command.Parameters.Add(parHuisnummer);
                    DbParameter parAppartementnummer = sqlFactory.CreateParameter();
                    parAppartementnummer.ParameterName = "@appartementnummer";
                    parAppartementnummer.DbType = DbType.String;
                    command.Parameters.Add(parAppartementnummer);
                    DbParameter parBusnummer = sqlFactory.CreateParameter();
                    parBusnummer.ParameterName = "@busnummer";
                    parBusnummer.DbType = DbType.String;
                    command.Parameters.Add(parBusnummer);
                    DbParameter parHuisnummerlabel = sqlFactory.CreateParameter();
                    parHuisnummerlabel.ParameterName = "@huisnummerlabel";
                    parHuisnummerlabel.DbType = DbType.String;
                    command.Parameters.Add(parHuisnummerlabel);
                    DbParameter parAdreslocatieID = sqlFactory.CreateParameter();
                    parAdreslocatieID.ParameterName = "@adreslocatieID";
                    parAdreslocatieID.DbType = DbType.Int32;
                    command.Parameters.Add(parAdreslocatieID);

                    command.CommandText = query;
                    command.Parameters["@Id"].Value = adres.ID;
                    command.Parameters["@straatnaamID"].Value = adres.Straat.ID;
                    command.Parameters["@huisnummer"].Value = adres.Huisnummer;

                    //probleem met null waardes toevoegen aan de db!!  gebruik van DBNull.Value?

                    if (adres.Appartementnummer == null)
                        command.Parameters["@appartementnummer"].Value = DBNull.Value;
                    else
                        command.Parameters["@appartementnummer"].Value = adres.Appartementnummer;

                    if (adres.Busnummer == null)
                        command.Parameters["@busnummer"].Value = DBNull.Value;
                    else
                        command.Parameters["@busnummer"].Value = adres.Busnummer;

                    if (adres.Huisnummerlabel== null)
                        command.Parameters["@huisnummerlabel"].Value = DBNull.Value;
                    else
                        command.Parameters["@huisnummerlabel"].Value = adres.Huisnummerlabel;

                    if (adres.Locatie.ID == 0)
                        command.Parameters["@adreslocatieID"].Value = DBNull.Value;
                    else
                        command.Parameters["@adreslocatieID"].Value = adres.Locatie.ID;

                    command.ExecuteNonQuery();

                    DbParameter parAdreslocatieId = sqlFactory.CreateParameter();
                    parAdreslocatieId.ParameterName = "@AdreslocatieId";
                    parAdreslocatieId.DbType = DbType.Int32;
                    command2.Parameters.Add(parAdreslocatieId);
                    DbParameter parX = sqlFactory.CreateParameter();
                    parX.ParameterName = "@X";
                    parX.DbType = DbType.Double;
                    command2.Parameters.Add(parX);
                    DbParameter parY = sqlFactory.CreateParameter();
                    parY.ParameterName = "@Y";
                    parY.DbType = DbType.Double;
                    command2.Parameters.Add(parY);

                    command2.CommandText = query2;
                    command2.Parameters["@AdreslocatieId"].Value = adres.Locatie.ID;
                    command2.Parameters["@X"].Value = adres.Locatie.X;
                    command2.Parameters["@Y"].Value = adres.Locatie.Y;

                    command2.ExecuteNonQuery();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}