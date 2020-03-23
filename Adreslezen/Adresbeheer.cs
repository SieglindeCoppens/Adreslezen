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
using System.Linq;

namespace Adreslezen
{
    class AdresBeheer
    {

        public Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>> LeesAdressen()
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
                        input = sr.ReadLine();
                        if (input == null)
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
                        else if (teller == 9)
                        {
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                nis = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 10)
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
            //DbConnection connection = getConnection();

            foreach (KeyValuePair<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeente in gemeentes)
            {
                voegGemeenteToe(gemeente.Key);
                foreach (KeyValuePair<Straatnaam, List<Adres>> straat in gemeente.Value)
                {
                    voegStraatToe(straat.Key);

                    foreach (Adres adres in straat.Value)
                    {
                        voegAdresToe(adres);
                    }
                }

            }
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

        public void voegGemeenteToe(Gemeente gemeente)
        {
            DbConnection connection = getConnection();
            string queryGemeente = "INSERT INTO dbo.gemeente(NIScode, gemeentenaam) VALUES(@NIScode,@gemeentenaam)";

            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();

                try
                {

                    DbParameter parNIScode = sqlFactory.CreateParameter();
                    parNIScode.ParameterName = "@NIScode";
                    parNIScode.DbType = DbType.Int32;
                    command.Parameters.Add(parNIScode);
                    DbParameter parGemeentenaam = sqlFactory.CreateParameter();
                    parGemeentenaam.ParameterName = "@gemeentenaam";
                    parGemeentenaam.DbType = DbType.String;
                    command.Parameters.Add(parGemeentenaam);

                    command.CommandText = queryGemeente;
                    command.Parameters["@NIScode"].Value = gemeente.NIScode;
                    command.Parameters["@gemeentenaam"].Value = gemeente.GemeenteNaam;
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

        }
        public void voegStraatToe(Straatnaam straat)
        {
            DbConnection connection = getConnection();
            string queryStraat = "INSERT INTO dbo.straatnaam(Id, straatnaam, NIScode) VALUES(@Id, @straatnaam, @NIScode)";

            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();

                try
                {

                    DbParameter paId = sqlFactory.CreateParameter();
                    paId.ParameterName = "@Id";
                    paId.DbType = DbType.Int32;
                    command.Parameters.Add(paId);
                    DbParameter parStraatnaam = sqlFactory.CreateParameter();
                    parStraatnaam.ParameterName = "@straatnaam";
                    parStraatnaam.DbType = DbType.String;
                    command.Parameters.Add(parStraatnaam);
                    DbParameter parNIScode = sqlFactory.CreateParameter();
                    parNIScode.ParameterName = "@NIScode";
                    parNIScode.DbType = DbType.Int32;
                    command.Parameters.Add(parNIScode);


                    command.CommandText = queryStraat;
                    command.Parameters["@Id"].Value = straat.ID;
                    command.Parameters["@NIScode"].Value = straat.Gemeente.NIScode;
                    command.Parameters["@straatnaam"].Value = straat.straatnaam;


                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }

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

                    if (adres.Huisnummerlabel == null)
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
        public Adres geefAdres(int id)
        {
            DbConnection connection = getConnection();
            string query = "SELECT * FROM dbo.adres WHERE Id=@Id";
            //string queryAS = "SELECT * FROM dbo.gemeente t1"
            //    + "where t1.NIScode = @NIScode";
            //string queryAB = "SELECT * FROM dbo.straatnaam t2" + "WHERE t2.Id=@straatnaamID";
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                DbParameter paramId = sqlFactory.CreateParameter();
                paramId.ParameterName = "@Id";
                paramId.DbType = DbType.Int32;
                paramId.Value = id;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    reader.Read();
                    int adresId = (int)reader["Id"];
                    int straatnaamID = (int)reader["straatnaamID"];
                    string huisnummer = (string)reader["huisnummer"];
                    string appartementnummer = (string)reader["appartementnummer"];
                    string busnummer = (string)reader["busnummer"];
                    string huisnummerlabel = (string)reader["huisnummerlabel"];
                    int adreslocatieID = (int)reader["adreslocatieID"];

                    reader.Close();

                    Straatnaam straat = GeefStraat(straatnaamID);

                    Adres adres = new Adres(adresId, straat, appartementnummer, busnummer, huisnummer, huisnummerlabel, straat.Gemeente);

                    Console.WriteLine($"Gemeente: {straat.Gemeente.GemeenteNaam} {straat.Gemeente.NIScode} \nStraat: {straat.straatnaam} {straat.ID}\nAdres: {adresId} {huisnummer} {appartementnummer} {busnummer} ");


                    return adres;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        //public AdresLocatie GetAdresLocatie()
        //{

        //}

        public Straatnaam GeefStraat(int id)
        {
            DbConnection connection = getConnection();
            string query = "SELECT * FROM dbo.straatnaam WHERE id=@id";
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                DbParameter paramId = sqlFactory.CreateParameter();
                paramId.ParameterName = "@Id";
                paramId.DbType = DbType.Int32;
                paramId.Value = id;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    reader.Read();
                    int straatID = (int)reader["Id"];
                    string straatnaam = (string)reader["straatnaam"];
                    int NIScode = (int)reader["NIScode"];
                    reader.Close();

                    Gemeente gemeente = GeefGemeente(NIScode);
                    Straatnaam straat = new Straatnaam(straatID, straatnaam, gemeente);

                    return straat;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public Gemeente GeefGemeente(int NIScode)
        {
            {
                DbConnection connection = getConnection();
                string query = "SELECT * FROM dbo.gemeente WHERE NIScode=@NIScode";
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    DbParameter paramNIScode = sqlFactory.CreateParameter();
                    paramNIScode.ParameterName = "@NIScode";
                    paramNIScode.DbType = DbType.Int32;
                    paramNIScode.Value = NIScode;
                    command.Parameters.Add(paramNIScode);
                    connection.Open();
                    try
                    {
                        DbDataReader reader = command.ExecuteReader();
                        reader.Read();

                        Gemeente gemeente = new Gemeente((int)reader["NIScode"], (string)reader["gemeentenaam"]);
                        reader.Close();
                        return gemeente;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public List<string> geefStraten(string gemeentenaam)
        {
            DbConnection connection = getConnection();


            //Stap  : de NIScode van de gegeven gemeentenaam zoeken

            string query = "SELECT * FROM dbo.gemeente WHERE gemeentenaam=@gemeentenaam";
            int NIScode = 0;
            List<String> straten = new List<string>();

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                DbParameter paramGemeentenaam = sqlFactory.CreateParameter();
                paramGemeentenaam.ParameterName = "@gemeentenaam";
                paramGemeentenaam.DbType = DbType.String;
                paramGemeentenaam.Value = gemeentenaam;
                command.Parameters.Add(paramGemeentenaam);
                connection.Open();
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    reader.Read();
                    NIScode = (int)reader["NIScode"];
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }

            string querystraten = "SELECT * FROM dbo.straatnaam WHERE NIScode=@NIScode";

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = querystraten;
                DbParameter paramNIScode= sqlFactory.CreateParameter();
                paramNIScode.ParameterName = "@NIScode";
                paramNIScode.DbType = DbType.Int32;
                paramNIScode.Value = NIScode;
                command.Parameters.Add(paramNIScode);
                connection.Open();
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        straten.Add(reader.GetString(1));
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }

            }
            straten.Sort();
            return straten;

        }

        public List<Adres> geefAdressen(int straatID)
        {
            DbConnection connection = getConnection();

            string query = "SELECT * FROM dbo.adres WHERE straatnaamID=@Id";
            List<Adres> adressen = new List<Adres>();

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                DbParameter paramId = sqlFactory.CreateParameter();
                paramId.ParameterName = "@Id";
                paramId.DbType = DbType.Int32;
                paramId.Value = straatID;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int adresID = (int)reader["Id"];
                        Adres adres = geefAdres(adresID);
                        adressen.Add(adres);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
            return adressen;
        }


    }
}
 
