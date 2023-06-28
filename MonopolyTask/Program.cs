using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MonopolyTask
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneratePallet();
            var list = FileManager.ReadFromFile<List<Pallet>>(ConfigurationManager.AppSettings["FileName"]);
            FirstOutput(list);
            SecondOutput(list);
            Console.ReadKey();
        }

        static DateTime GenerateDate(Random rnd)
        {
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }

        static float GenerateFloat(Random rnd, float min, float max)
        {
            double val = (rnd.NextDouble() * (max - min) + min);
            return (float)val;
        }

        static void GeneratePallet()
        {
            var gen = new Random();
            var lst = new List<Pallet>();

            for (int i = 0; i < gen.Next(20, 30); i++)
            {
                var pTmp = new byte[sizeof(float)];
                int pId;
                float pWidth, pHeight, pDepth;

                pId = i;
                pWidth = GenerateFloat(gen, 1, 20);
                pHeight = GenerateFloat(gen, 1, 20);
                pDepth = GenerateFloat(gen, 1, 20);
                var pallet = new Pallet(pId, pWidth, pHeight, pDepth);

                for (int j = 0; j < gen.Next(5, 10); j++)
                {
                    var bTmp = new byte[sizeof(float)];
                    int bId;
                    float bWidth = -1f, bHeight = -1f, bDepth, bWeight;
                    DateTime? prodDate = null, expDate = null;

                    bId = j;
                    do
                    {
                        var wTmp = GenerateFloat(gen, 1, 20);
                        if (pWidth >= wTmp)
                            bWidth = wTmp;
                    } while (bWidth < 0);
                    do
                    {
                        var hTmp = GenerateFloat(gen, 1, 20);
                        if (pHeight >= hTmp)
                            bHeight = hTmp;
                    } while (bHeight < 0);
                    bDepth = GenerateFloat(gen, 1, 20);
                    bWeight = GenerateFloat(gen, 1, 20);
                    var solve = gen.Next(0, 100);
                    if (solve < 40)
                    {
                        prodDate = GenerateDate(gen);
                    }
                    else if (solve > 65)
                    {
                        expDate = GenerateDate(gen);
                    }
                    else
                    {
                        prodDate = GenerateDate(gen);
                        expDate = GenerateDate(gen);
                    }

                    pallet.AddBox(new Box(bId, bWidth, bHeight, bDepth, bWeight, prodDate, expDate));
                }
                lst.Add(pallet);
            }
            FileManager.SaveToFile(ConfigurationManager.AppSettings["FileName"], lst);
        }

        static void FirstOutput(List<Pallet> list)
        {
            var groupedQuery = list.GroupBy(e => e.ExpirationDate).OrderBy(e => e.Key);
            Console.WriteLine("\n" + new string('=', 55) + " ВЫВОД 1 " + new string('=', 55) + "\n");
            foreach (var group in groupedQuery)
            {
                Console.WriteLine(new string('*', 50) + $" {group.Key} " + new string('*', 49));
                var orderedQuery = group.OrderBy(e => e.CalculateWeight());
                foreach (var pallet in orderedQuery)
                {
                    Console.WriteLine(new string('_', 55) + " ПАЛЛЕТА " + new string('_', 55));
                    Console.WriteLine(String.Format($"ID:\t\t{pallet.Id}"));
                    Console.WriteLine(String.Format("Ширина:\t\t{0:000.0}", pallet.Width));
                    Console.WriteLine(String.Format("Высота:\t\t{0:000.0}", pallet.Height));
                    Console.WriteLine(String.Format("Глубина:\t{0:000.0}", pallet.Depth));
                    Console.WriteLine(String.Format("Общий вес:\t{0:000.0}", pallet.CalculateWeight()));
                    Console.WriteLine(String.Format("Общий объём:\t{0:000.0}", pallet.CalculateVolume()));
                    Console.WriteLine(String.Format($"Срок годности:\t{pallet.CalculateExpirationDate()}"));
                    Console.WriteLine("Содержимое паллеты:");
                    Console.WriteLine(new string('-', 119));
                    var str = string.Format("| {0, 4} | {1, 10} | {2, 10} | {3, 10} | {4, 10} | {5, 10} | {6, 20} | {7, 20} |",
                                                "ID", "Ширина", "Высота", "Глубина", "Вес", "Объём", "Дата производства", "Срок годности");
                    Console.WriteLine(str);
                    Console.WriteLine(new string('-', 119));
                    for (int i = 0; i < pallet.BoxesCount(); i++)
                    {
                        str = string.Format("| {0, 4} | {1, 10} | {2, 10} | {3, 10} | {4, 10} | {5, 10} | {6, 20} | {7, 20} |",
                            pallet[i].Id,
                            String.Format("{0:000.0}", pallet[i].Width),
                            String.Format("{0:000.0}", pallet[i].Height),
                            String.Format("{0:000.0}", pallet[i].Depth),
                            String.Format("{0:000.0}", pallet[i].Weight),
                            String.Format("{0:000.0}", pallet[i].CalculateVolume()),
                            pallet[i].ProductionDate,
                            pallet[i].ExpirationDate);
                        Console.WriteLine(str);
                    }
                    Console.WriteLine(new string('-', 119));
                    Console.WriteLine("");
                }
            }
        }

        static void SecondOutput(List<Pallet> list)
        {
            var query = list.OrderByDescending(e => e.CalculateMaxExpirationDate()).Take(3).OrderBy(e => e.CalculateVolume());
            Console.WriteLine("\n" + new string('=', 55) + " ВЫВОД 2 " + new string('=', 55) + "\n");
            foreach (var pallet in query)
            {
                Console.WriteLine(new string('_', 55) + " ПАЛЛЕТА " + new string('_', 55));
                Console.WriteLine(String.Format($"ID:\t\t{pallet.Id}"));
                Console.WriteLine(String.Format("Ширина:\t\t{0:000.0}", pallet.Width));
                Console.WriteLine(String.Format("Высота:\t\t{0:000.0}", pallet.Height));
                Console.WriteLine(String.Format("Глубина:\t{0:000.0}", pallet.Depth));
                Console.WriteLine(String.Format("Общий вес:\t{0:000.0}", pallet.CalculateWeight()));
                Console.WriteLine(String.Format("Общий объём:\t{0:000.0}", pallet.CalculateVolume()));
                Console.WriteLine(String.Format($"Срок годности:\t{pallet.CalculateExpirationDate()}"));
                Console.WriteLine("Содержимое паллеты:");
                Console.WriteLine(new string('-', 119));
                var str = string.Format("| {0, 4} | {1, 10} | {2, 10} | {3, 10} | {4, 10} | {5, 10} | {6, 20} | {7, 20} |",
                                            "ID", "Ширина", "Высота", "Глубина", "Вес", "Объём", "Дата производства", "Срок годности");
                Console.WriteLine(str);
                Console.WriteLine(new string('-', 119));
                for (int i = 0; i < pallet.BoxesCount(); i++)
                {
                    str = string.Format("| {0, 4} | {1, 10} | {2, 10} | {3, 10} | {4, 10} | {5, 10} | {6, 20} | {7, 20} |",
                        pallet[i].Id,
                        String.Format("{0:000.0}", pallet[i].Width),
                        String.Format("{0:000.0}", pallet[i].Height),
                        String.Format("{0:000.0}", pallet[i].Depth),
                        String.Format("{0:000.0}", pallet[i].Weight),
                        String.Format("{0:000.0}", pallet[i].CalculateVolume()),
                        pallet[i].ProductionDate,
                        pallet[i].ExpirationDate);
                    Console.WriteLine(str);
                }
                Console.WriteLine(new string('-', 119));
                Console.WriteLine("");
            }
        }
    }
}
