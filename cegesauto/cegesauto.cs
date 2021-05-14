using System;
using System.Collections.Generic;
using System.IO;

namespace cegesauto
{
    class cegesauto
    {
        class Adatok
        {
            public int nap;
            public string oraperc;
            public string rendszam;
            public int szemelyaz;
            public int km;
            public bool bejott;
            public Adatok(int nap, string oraperc, string rendszam, int szemelyaz, int km, bool kibe)
            {
                this.nap = nap;
                this.oraperc = oraperc;
                this.rendszam = rendszam;
                this.szemelyaz = szemelyaz;
                this.km = km;
                this.bejott = kibe;
            }
            public string Kibeiras()
            {
                if (bejott)
                {
                    return "be";
                }
                else
                {
                    return "ki";
                }
            }
        }
        class Kmstat
        {
            public int vegkm = 0;
            public int kezdokm = 0;
            public Kmstat(Adatok ad)
            {
                this.vegkm = ad.km;
                this.kezdokm = ad.km;
            }
            public void Ujadat(Adatok ad)
            {
                this.vegkm = ad.km;
            }
            public int Km()
            {
                return vegkm - kezdokm;
            }
        }
        static void Main(string[] args)
        {
            #region 1.feladat
            StreamReader file = new StreamReader("../../../autok.txt");
            List<Adatok> autoadatok = new List<Adatok>();
            bool readlinesuccesful = true;
            while (readlinesuccesful)
            {
                string line = file.ReadLine();
                if (line == null)
                {
                    readlinesuccesful = false;
                    break;
                }
                string[] tordeltsor = line.Split(' ');
                autoadatok.Add(new Adatok(Int32.Parse(tordeltsor[0]), tordeltsor[1], tordeltsor[2], Int32.Parse(tordeltsor[3]), Int32.Parse(tordeltsor[4]), Int32.Parse(tordeltsor[5]) != 0 ? true : false));
            }
            file.Close();
            #endregion
            #region 2.feladat
            Console.WriteLine("2.feladat");
            for (int i = autoadatok.Count - 1; i >= 0; i--)
            {
                if (!autoadatok[i].bejott)
                {
                    Console.WriteLine($"{autoadatok[i].nap}. nap rendszám: {autoadatok[i].rendszam}");
                    break;
                }
            }
            Console.Write('\n');
            #endregion
            #region 3.feladat
            Console.WriteLine("3.feladat");
            Console.Write("Nap: ");
            int adottnap = Int32.Parse(Console.ReadLine());
            Console.WriteLine($"Forgalom a(z) {adottnap}. napon:");
            for (int i = 0; i < autoadatok.Count; i++)
            {
                if (adottnap == autoadatok[i].nap)
                {
                    Console.WriteLine($"{autoadatok[i].oraperc} {autoadatok[i].rendszam} {autoadatok[i].szemelyaz} {autoadatok[i].Kibeiras()}");
                }
            }
            Console.Write('\n');
            #endregion
            #region 4.feladat
            Console.WriteLine("4.feladat");
            HashSet<string> nemvisszahozott = new HashSet<string>();

            for (int i = 0; i < autoadatok.Count; i++)
            {
                if (!autoadatok[i].bejott)
                {
                    nemvisszahozott.Add(autoadatok[i].rendszam);
                }
                else
                {
                    nemvisszahozott.Remove(autoadatok[i].rendszam);
                }
            }
            Console.WriteLine($"A hónap végén {nemvisszahozott.Count} autót nem hoztak vissza.");
            Console.Write('\n');
            #endregion
            #region 5.feladat
            Console.WriteLine("5.feladat");
            Dictionary<string, Kmstat> statisztika = new Dictionary<string, Kmstat>();
            foreach (var ad in autoadatok)
            {
                if (!statisztika.ContainsKey(ad.rendszam))
                {
                    statisztika.Add(ad.rendszam, new Kmstat(ad));
                }
                else
                {
                    statisztika[ad.rendszam].Ujadat(ad);
                }
            }
            foreach (var kv in statisztika)
            {
                Console.WriteLine($"{kv.Key} {kv.Value.Km()} km");
            }
            Console.Write('\n');
            #endregion
            #region 6.feladat
            Console.WriteLine("6.feladat");
            Dictionary<string, int> utikm = new Dictionary<string, int>();
            int leghossz = 0;
            int legszem = 0;
            foreach (var ad in autoadatok)
            {
                if (!ad.bejott)
                {
                    utikm.Add(ad.rendszam, ad.km);
                }
                else
                {
                    int hossz = ad.km - utikm[ad.rendszam];
                    utikm.Remove(ad.rendszam);
                    if (leghossz < hossz)
                    {
                        leghossz = hossz;
                        legszem = ad.szemelyaz;
                    }
                }
            }
            Console.WriteLine($"Leghosszabb út: {leghossz} km, személy: {legszem}");
            Console.Write('\n');
            #endregion
            #region 7.feladat
            Console.WriteLine("7.feladat");
            Console.Write("Rendszám: ");
            string adottrendsz = Console.ReadLine();
            StreamWriter keszintendo = new StreamWriter($"../../../{adottrendsz}_menetlevel.txt");
            bool visszahoztae = true;
            foreach (var ad in autoadatok)
            {
                if (ad.rendszam == adottrendsz)
                {
                    //if (!ad.bejott)
                    //{
                    //    keszintendo.Write($"{ad.szemelyaz}\t{ad.nap}.\t{ad.oraperc}\t{ad.km} km");//588 21. 16:58 13452 km 23. 20:28 14335 km
                    //}
                    //if (ad.bejott)
                    //{
                    //    keszintendo.Write($"\t{ad.nap}.\t{ad.oraperc}\t{ad.km} km\n");
                    //}
                    if (!ad.bejott)
                    {
                        keszintendo.Write($"{ad.szemelyaz}");//588 21. 16:58 13452 km 23. 20:28 14335 km
                    }
                    keszintendo.Write($"\t{ad.nap}. {ad.oraperc}\t{ad.km} km");
                    if (ad.bejott)
                    {
                        keszintendo.Write($"\n");
                    }
                    visszahoztae = ad.bejott;
                }
            }
            if (!visszahoztae)
            {
                keszintendo.Write('\n');
            }
            keszintendo.Close();
            Console.WriteLine("Menetlevél kész.");
            #endregion
        }
    }
}
