//1. gegevens lezen vanuit het bestand en importeren in db
//2. methode die nieuw adres aan databank toevoegt (enkel adres en adreslocatie opvullen!! Straatnaam en Gemeente bestaan zogezegd reeds)
//public void voegAdresToe(Adres adres){...}
//dit moet een transactie zijn! Adres en adreslocatie worden allebei aangepast of allebei niet! 


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adreslezen
{
    class AdresBeheer
    {
        public Dictionary<Gemeente,Dictionary<Straatnaam, List<Adres>>> LeesAdressen()
        {
            Dictionary<Straatnaam, List<Adres>> straatnamen = new Dictionary<Straatnaam, List<Adres>>();
            Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>> gemeentes = new Dictionary<Gemeente, Dictionary<Straatnaam, List<Adres>>>();
            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\source\repos\Adreslezen\CrabAdr.gml"))
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


                for (int t = 0; t < 9; t++)
                    sr.ReadLine();

                while ((sr.ReadLine()) != null)
                {
                    for (int teller = 1; teller <= 22; teller++)
                    {
                        if (teller == 1 || (teller >= 12 && teller <= 15) || teller >= 18)
                        {
                            sr.ReadLine();
                        }
                        else if (teller == 2)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                id = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 3)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                straatnaamId = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 4)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                straatnaam = inputs[2];
                            }
                        }
                        else if (teller == 5)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                huisnummer = inputs[2];
                            }
                        }
                        else if (teller == 6)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                appartementnummer = inputs[2];
                            }
                        }
                        else if (teller == 7)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                busnummer = inputs[2];
                            }
                        }
                        else if (teller == 8)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                huisnrLabel = inputs[2];
                            }
                        }
                        else if(teller == 9)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                nis = int.Parse(inputs[2]);
                            }
                        }
                        else if(teller == 10)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                gemeentenaam = inputs[2];
                            }
                        }
                        else if (teller == 11)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                postcode = int.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 16)
                        {
                            input = sr.ReadLine();
                            String[] inputs = input.Split(splitsers);
                            if (inputs.Length == 5)
                            {
                                x = double.Parse(inputs[2]);
                            }
                        }
                        else if (teller == 17)
                        { 
                            input = sr.ReadLine();
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
                        gemeentes[gemeente].Add(straat, null);
                    }

                    //adres aanmaken
                    Adres adres = new Adres(id, straat, appartementnummer, busnummer, huisnummer, huisnrLabel, gemeente, postcode, x, y);

                    //adres toevoegen aan dictionary
                    gemeentes[gemeente][straat].Add(adres);
                }
            }
            return gemeentes;
        }
        public void voegAdresToe(Adres adres)
        {

        }
    }
}
