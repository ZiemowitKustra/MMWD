using System;
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
        ///---------------------------------------Test--------------------------------------------
        public static void Test(double TEST, string destination)
        {
            if (File.Exists(destination))
            {
                string all_from_file = File.ReadAllText(destination) + Convert.ToString(TEST) + " ";
                File.WriteAllText(destination, all_from_file);
            }
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
            double MaxCalories = BMR + 300, MaxCarbo = 0.065 * MaxCalories, MaxFats = 0.25 * MaxCalories, MaxProteins = 0.3 * MaxCalories;
            double MinCalories = BMR - 300, MinCarbo = 0.05 * MinCalories, MinFats = 0.15 * MinCalories, MinProteins = 0.2 * MinCalories;
            double  wsp2 = 1, wsp_edi = 0.1;
            for (int i = 0; i < n; i++)
            {

                SumOfCalories = SumOfCalories + Solution[i].K_Calories;
                SumOfCosts = SumOfCosts + Solution[i].Cost;
                SumOfCarbo = SumOfCarbo + Solution[i].Carbonhydrates*Solution[i].K_Calories;
                SumOfFats = SumOfFats + Solution[i].Fats*Solution[i].K_Calories;
                SumOfProteins = SumOfProteins + Solution[i].Proteins*Solution[i].K_Calories;
            }
            SumOfCalories = 2 * SumOfCalories;
            if (SumOfCalories > MaxCalories) Penalty = Penalty + 0.1*(SumOfCalories - MaxCalories);
            if (SumOfCalories < MinCalories) Penalty = Penalty + 0.1*(MinCalories - SumOfCalories);
            if (SumOfCarbo > MaxCarbo) Penalty = Penalty + wsp_edi*(SumOfCarbo-MaxCarbo);
            if (SumOfCarbo < MinCarbo) Penalty = Penalty + wsp_edi * (MinCarbo-SumOfCarbo);
            if (SumOfFats > MaxFats) Penalty = Penalty + wsp_edi * (SumOfFats - MaxFats);
            if (SumOfFats < MinFats) Penalty = Penalty + wsp_edi * (MinFats-SumOfFats);
            if (SumOfProteins > MaxProteins) Penalty = Penalty + wsp_edi * (SumOfProteins-MaxProteins);
            if (SumOfProteins < MinProteins) Penalty = Penalty + wsp_edi * (MinProteins-SumOfProteins);
            SumOfCosts = 2 * SumOfCosts;
            double Function =  SumOfCosts + wsp2 * Penalty;
            return Function;
        }
        ///-------------------------------Znajdywanie randomowego rozwiązania-------------------------------------------------

        public static List<Food> RandSolution(int n)
        {
            List<Food> Solution = new List<Food>();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
            Rand3:
                int rInt = r.Next(0, 10);
                for (int j = 0; j < Solution.Count(); j++)
                {
                    if (Solution[j] == Produkty[rInt]) goto Rand3;
                }
                Solution.Add(Produkty[rInt]);
            }
            Global_Solution.AddRange(Solution);
            //przekopiowac 
            return Solution;
        }
        public static List<Food> SetSolution(int n)
        {
            List<Food> Solution = new List<Food>(n);
            Solution.Add(Produkty[8]);//Cielecina, Marakuja, Masło,pieczen,Mozarella,Wino, Czekolada biala,Zurek
            Solution.Add(Produkty[12]);
            Solution.Add(Produkty[15]);
            Solution.Add(Produkty[20]);
            Solution.Add(Produkty[45]);
            Solution.Add(Produkty[48]);
            Solution.Add(Produkty[34]);
            Solution.Add(Produkty[39]);
            Global_Solution.AddRange(Solution);
          
            return Solution;
        }
        public static List<Food> FindNextSolution(List<Food> Solution, double BMR)
        {
            List<Food> NextSolution = new List<Food>();
            List<Food> PreviousSolution = new List<Food>();
            //List<Food> CopyofSolution = new List<Food>();
            int counter = Produkty.Count;//ilosc wszystkich produktow
            NextSolution.AddRange(Solution);
            PreviousSolution.AddRange(Solution);
            //przepisuje wszystkie produkty od 1 do n - ilosc wybranych produktow

            //sprawdza czy rozwiazanie z listy taboo polepsza rozwiazanie globalnie
            for (int i = 0; i < TabooList.Count; i++)
            {
                CheckTabooSolution(TabooList[i], Solution, BMR);
            }
            CheckTaboo();
            Solution.Clear();
            Solution.AddRange(NextSolution);
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
            Rand2:
                int r2 = r.Next(0, counter);
                for (int i = 0; i < Solution.Count(); i++)
                {
                    if (Solution[i] == Produkty[r2]) goto Rand2;
                }
                Solution[r1] = Produkty[r2];
                if (Function(Solution, BMR) < Function(NextSolution, BMR))
                {
                    NextSolution.Clear();
                    NextSolution.AddRange(Solution);

                }
            }
            for (int j = 0; j < NextSolution.Count; j++)
            {
                if (NextSolution[j] != PreviousSolution[j])
                {
                    TabooList.Add(j);
                    LifeTime.Add(cadence);
                }
            }
            if (Function(NextSolution, BMR) < Function(Global_Solution, BMR))
            {
                Global_Solution.Clear();
                Global_Solution.AddRange(NextSolution);
            }
            return NextSolution;

        }
        //znajduje najlepsze rozwiazanie z otoczenia
        //--------------sprawdzenie czy rozwiazanie z listy taboo globalnie polepsza rozwiazanie
        static public void CheckTabooSolution(int n, List<Food> Solution, double BMR)
        {
            Random r = new Random();
            for (int j = 0; j < 10; j++)
            {
                int r2 = r.Next(0, Produkty.Count());
                Solution[n] = Produkty[r2];
                if (Function(Solution, BMR) < Function(Global_Solution, BMR))
                {
                    Global_Solution.Clear();
                    Global_Solution.AddRange(Solution);
                }
            }
        }

        static public List<Food> FindBestSolution(List<Food> Solution, double BMR)
        {
            for (int i = 0; i < 10; i++)
            {
                Solution = FindNextSolution(Solution, BMR);
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
        }
    }
}



