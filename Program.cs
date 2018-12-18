﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace MMWD_CS
{
    static class Program
    {
        public static List<int> TabooList = new List<int>(); //lista numerów (indeksów) pozycji których nie można zmieniać przez jakiś czas
        public static List<int> LifeTime = new List<int>(); //lista kadencji danych elementow taboo list 
        public static List<Food> Global_Solution = new List<Food>(); // przechowanie najlepszego globalnie rozwiązania
        public static int cadence = 4;


        ///----------------------------------------czytanie z pliku-----------------------------
        public static List<Food> FromFileToList(string source)
        {
            string[] Base_Catcher = File.ReadAllLines(source);
            List<Food> ToList = new List<Food>();
            List<string> Converted_Base = new List<string>();
            if (File.Exists(source))
            {
                string[] Conector = new string[] { };
                for (int i = 0; i < Base_Catcher.Length; i++)
                {
                    Conector = Base_Catcher[i].Split(' ');  ///rozdzial ze wzgledu na spacje
                    Converted_Base.AddRange(Conector); ///przenoszenie do listy converted_base 
                    Conector = new string[] { };
                }
                Converted_Base.Remove("");       /// nie wiem czemu ale widzipuste znaki i dodaje je do tablicy wiec usuwam je
                int counter = Converted_Base.Count();
                for (int i = 0; i < counter; i = i + 7) ///wszucanie z listy do typu produkty kolejnych przedmiotow (ostateczna wersja danych)
                {
                    double id = Convert.ToDouble(Converted_Base[i]);
                    double calories = Convert.ToDouble(Converted_Base[i + 2]);
                    double carbohydrates = Convert.ToDouble(Converted_Base[i + 3]);
                    double protein = Convert.ToDouble(Converted_Base[i + 4]);
                    double fat = Convert.ToDouble(Converted_Base[i + 5]);
                    double cost = Convert.ToDouble(Converted_Base[i + 6]);
                    ToList.Add(new Food(id, Converted_Base[i + 1], calories, carbohydrates, protein, fat, cost));
                }
            }
            return ToList;
        }
        ///----------------------------------------Wpisywanie do pliku-----------------------------
        public static void FromListToFile(List<Food> Source, string destination)
        {
            //string[] List_Catcher = new string[] { };
            List<string> Converted_List = new List<string>();
            int counter = Source.Count();
            for (int i = 0; i < counter; i++) ///wszucanie z listy do typu produkty kolejnych przedmiotow (ostateczna wersja danych)
            {
                Converted_List.Add(Convert.ToString(Source[i].ID) + " " + Source[i].Name + " " + Convert.ToString(Source[i].K_Calories)
                    + " " + Convert.ToString(Source[i].Carbonhydrates) + " " + Convert.ToString(Source[i].Proteins) + " "
                    + Convert.ToString(Source[i].Fats) + " " + Convert.ToString(Source[i].Cost));
            }
            File.WriteAllLines(destination, Converted_List);
        }
        //public static string[] Base_Catcher = File.ReadAllLines("produkty.txt");
        public static List<Food> Produkty = new List<Food>();
        //public static List<string> Converted_Base = new List<string>();
        public static List<Food> Used = new List<Food>();
        public static List<Food> Taboo = new List<Food>();
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]

        ///-------------------------------Obliczanie funkcji celu----------------------------------------------
        public static double Function(List<Food> Solution, double BMR)
        {
            int n = Solution.Count;
            double SumOfCosts = 0, SumOfCarbo = 0, SumOfFats = 0, SumOfProteins = 0, SumOfCalories = 0, Penalty = 0;
            double MaxCalories = BMR + 500, MaxCarbo = 0.65 * n, MaxFats = 0.25 * n, MaxProteins = 0.3 * n, MinCalories = BMR - 500, MinCarbo = 0.5 * n, MinFats = 0.15 * n, MinProteins = 0.2 * n;
            double wsp1 = 1, wsp2 = 2;
            for (int i = 0; i < n; i++)
            {
                SumOfCosts = SumOfCosts + Solution[i].Cost;
                SumOfCarbo = SumOfCarbo + Solution[i].Carbonhydrates;
                SumOfFats = SumOfFats + Solution[i].Fats;
                SumOfProteins = SumOfProteins + Solution[i].Proteins;
                SumOfCalories = SumOfCalories + Solution[i].K_Calories;
            }
            if (SumOfCalories > MaxCalories || SumOfCalories < MinCalories) Penalty++;
            if (SumOfCarbo > MaxCarbo || SumOfCarbo < MinCarbo) Penalty++;
            if (SumOfFats > MaxFats || SumOfFats < MinFats) Penalty++;
            if (SumOfProteins > MaxProteins || SumOfProteins < MinProteins) Penalty++;
            double Function = wsp1 * SumOfCosts + wsp2 * Penalty;
            return Function;
        }
        ///-------------------------------Znajdywanie randomowego rozwiązania-------------------------------------------------

        public static List<Food> RandSolution(int n)
        {
            List<Food> Solution = new List<Food>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                int rInt = r.Next(0, 10);
                Solution.Add(Produkty[rInt]);
            }
            return Solution;
        }
        public static List<Food> FindNextSolution(List<Food> Solution, double BMR)
        {
            List<Food> NextSolution = new List<Food>();
            //List<Food> CopyofSolution = new List<Food>();
            int counter = Produkty.Count;//ilosc wszystkich produktow

            //przepisuje wszystkie produkty od 1 do n - ilosc wybranych produktow
            for (int i = 0; i < Solution.Count; i++)
            {
                NextSolution[i] = Solution[i];
            }
            //sprawdza czy rozwiazanie z listy taboo polepsza rozwiazanie globalnie
            for (int i = 0; i < TabooList.Count; i++)
            {
                CheckTabooSolution(TabooList[i],Solution,BMR);
            }
            CheckTaboo();
            //OTOCZENIE: WYMIENIA LOSOWY ELEMENT Z OBECNEGO ROZW. NA LOSOWY - 10RAZY
            Random r = new Random();

        Rand:
            int r1 = r.Next(0, Solution.Count);
            for (int j = 0; j < TabooList.Count; j++)
            {
                if (TabooList[j] == r1) goto Rand;
            }
            for (int j = 0; j < 10; j++)
            {
                int r2 = r.Next(0, counter);
                Solution[r1] = Produkty[r2];
                if (Function(Solution, BMR) < Function(NextSolution, BMR))
                {
                    NextSolution = Solution;
                    TabooList.Add(r1);
                    LifeTime.Add(cadence);
                }
            }
            if (Function(NextSolution, BMR) < Function(Global_Solution, BMR)) Global_Solution = NextSolution;
            return NextSolution;
        }
        //znajduje najlepsze rozwiazanie z otoczenia
        //--------------sprawdzenie czy rozwiazanie z listy taboo globalnie polepsza rozwiazanie
        static public void CheckTabooSolution(int n,List<Food> Solution,double BMR)
        {
            Random r = new Random();
            for (int j = 0; j < 10; j++)
                {
                    int r2 = r.Next(0, Produkty.Count);
                    Solution[n] = Produkty[r2];
                    if (Function(Solution, BMR) < Function(Global_Solution, BMR))
                    {
                        Global_Solution = Solution;
                    }
                }
        }
        static public List<Food> FindBestSolution(List<Food> Solution, double BMR)
        {
            for (int i = 0; i < 10; i++)
            {
                FindNextSolution(Solution, BMR);
                CheckTaboo();
            }
            return Solution;
        }
        //CheckTaboo zmniejsza kadencję oraz usuwa z listy Taboo rozwiazania z zerowa kadencja
        public static void CheckTaboo()
        {
            for (int i = 0; i < TabooList.Count; i++)
            {
                LifeTime[i]--;
                if (LifeTime[i] == 0)
                {
                    TabooList.Remove(TabooList[i]);
                    LifeTime.Remove(LifeTime[i]);
                }
            }
        }
        static void Main(string[] args)
        {
            Produkty = FromFileToList("produkty.txt");
            Used.AddRange(Produkty);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //List<Food> Solution = RandSolution(5);
            //foreach (Food food in Solution)
            //Console.WriteLine(food.ToString());
            Console.WriteLine("LOL");
            Console.ReadKey();
        }
    }
}


