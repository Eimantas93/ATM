using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace ATM
{
    class MainClass
    {

        static Dictionary<string, string> language = new Dictionary<string, string>();
        static int existingPIN = 1993;
        static decimal bankBalance = 1000;
        static bool loggedIn = false;

        // path to the Languages csv file
        static string pathLanguagesLib = "/Users/eimantas/Projects/First/First/Misc/Languages.csv";
        // path to the History csv file
        static string pathHistoryLib = "/Users/eimantas/Projects/First/First/Misc/History.csv";


        public static void Main(string[] args)
        {
            while (true)
            {

                LanguageSelector();

                Console.Clear();

                ShowMenu();                

            }
        }


        static void LanguageSelector()
        {
            Console.Clear();

            // Temp Dictionary witch will be assigned to global language dictionary.
            Dictionary<string, string> tempDict = new Dictionary<string, string>();

            Console.WriteLine("Pasirinkite kalba / Choose language / Jazik: \n" +
                              "1 - Lietuviu kalba \n" +
                              "2 - English language \n" +
                              "3 - Paruski jazik");


            // Checking if entered correct value - 1, 2 or 3
            if (int.TryParse(Console.ReadLine(), out int choise))
            {
                if (choise == 1 || choise == 2 || choise == 3)
                {
                    // continue if entered 1 or 2 or 3
                }
                else
                {
                    Console.WriteLine("Iveskite skaiciu 1 arba 2 arba 3 / Please enter number 1 or 2 or 3");
                    System.Threading.Thread.Sleep(1000);
                    LanguageSelector();
                }
            }
            else
            {
                Console.WriteLine("Iveskite skaiciu / Enter a number");
                System.Threading.Thread.Sleep(1000);
                LanguageSelector();
            }


            string[] lines = System.IO.File.ReadAllLines(pathLanguagesLib);


            // Reading CSV file and assigning values to temporary dictionary
            for (int i = 0; i < lines.Length; i++)
            {

                string[] rowData = lines[i].Split(',');

                // rowData[0] - bendrinis raktazodis, rowData[1] - LT kalba, rowData[2] - ENG kalba, rowData[3] - RU kalba
                tempDict.Add(rowData[0], rowData[choise]);

            }

            // Assigning temporary dictionary to global language dictionary
            language = tempDict;

            Console.Clear();
            Console.WriteLine(language["Pasisveikinimas"]);

            // Checking if user is logged in
            if (loggedIn)
            {
                ShowMenu();
            }
            else
            {
                CheckingPin();
            }

        }

        static void CheckingPin()
        {

            System.Threading.Thread.Sleep(1000);

            Console.Clear();

            int triesForPin = 3;

            while (triesForPin != 0)
            {
                Console.WriteLine(language["PinVedimas"]);

                // Checking if entered number
                if (int.TryParse(Console.ReadLine(), out int enteredPIN))
                {

                }
                else
                {
                    Console.WriteLine("Iveskite skaiciu / Enter a number");
                    System.Threading.Thread.Sleep(1000);
                    LanguageSelector();
                }

                // Checking if PIN is correct
                if (enteredPIN == existingPIN)
                {
                    loggedIn = true;
                    break;
                }
                else
                {
                    triesForPin--;
                    Console.WriteLine(language["BlogasSlaptazodis"] + " " + language["Liko"] + " "
                        + $"{ triesForPin }" + " " + language["Bandymai"]);
                }

                if (triesForPin == 0)
                {
                    Environment.Exit(0);
                }
            }
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("1 - " + language["KeistiKalba"] + "\n" +
                              "2 - " + language["KeistiSlaptazodi"] + "\n" +
                              "3 - " + language["SaskaitosLikutis"] + "\n" +
                              "4 - " + language["SaskaitosIsrasas"] + "\n" +
                              "5 - " + language["InestiPinigu"] + "\n" +
                              "6 - " + language["PasiimtiPinigu"] + "\n" +
                              "7 - " + language["BaigtiDarba"] + "\n");

            SelectOperation();
        }

        static void SelectOperation()
        {
            int operation = Convert.ToInt32(Console.ReadLine());


            switch (operation)
            {
                // Select language
                case 1:
                    LanguageSelector();
                    break;
                // Change password
                case 2:
                    ChangePassword();
                    break;
                // Saskaitos likutis
                case 3:
                    ViewBalance();
                    break;
                // Saskaitos israsas
                case 4:
                    ShowEventsHistory();
                    break;
                // Pinigu inesimas
                case 5:
                    InsertingCash();
                    break;
                // Pinigu paemimas
                case 6:
                    WithdrawCash();
                    break;
                // Baigti darba
                case 7:
                    EndProgram();
                    break;
                default:
                    break;
            }
        }

        static void ChangePassword()
        {
            while (true)
            {
                Console.WriteLine(language["DabartinioSlaptazodzioIvedimas"]);
                int oldPassword = Convert.ToInt32(Console.ReadLine());

                if (oldPassword == existingPIN)
                {
                    Console.WriteLine(language["IveskiteNaujaSlaptazodi"]);
                    existingPIN = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine(language["SlaptazodisPakeistas"] + " " + existingPIN);
                    ShowMenu();
                }
                else if (oldPassword == 0000)
                {
                    ShowMenu();
                }
                else
                {
                    Console.WriteLine(language["NeteisingasDabartinisSlaptazodis"]);
                }
            }
        }

        static void ViewBalance()
        {
            Console.Clear();

            Console.WriteLine(language["PerziuretiBalansa"] + bankBalance + "EUR");

            Console.WriteLine(language["PaspaudimasIseiti"]);

            Console.ReadKey();
            ShowMenu();
        }

        static void InsertingCash()
        {
            Console.Clear();

            Console.WriteLine(language["PiniguInesimas"]);
            decimal cash = Convert.ToDecimal(Console.ReadLine());

            bankBalance += cash;

            Console.WriteLine(language["PinigaiInesti"] + cash);
            // Cia tikrinti kupiuras
            System.Threading.Thread.Sleep(1000);

            AddEventToHistory(language["Inesimas"], cash);

            ShowMenu();
        }

        static void WithdrawCash()
        {
            Console.Clear();

            Console.WriteLine(language["PiniguNusiemimas"]);

            decimal cash = Convert.ToDecimal(Console.ReadLine());


            if (bankBalance >= cash)
            {
                bankBalance -= cash;

                Console.WriteLine(language["PinigaiNuimti"] + cash + "EUR");
                System.Threading.Thread.Sleep(3000);
                AddEventToHistory(language["Isemimas"], cash);
                ShowMenu();
            }
            else
            {
                Console.WriteLine(language["NepakankamasLikutis"]);
                System.Threading.Thread.Sleep(1000);
                ShowMenu();
            }
        }

        static void EndProgram()
        {
            Environment.Exit(0);
        }

        static void AddEventToHistory(string operation, decimal money)
        {
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");

            string row = currentDate + "," + operation + "," + money + ",";

            using (StreamWriter sw = File.AppendText(pathHistoryLib))
            {
                sw.WriteLine(row);
            }

        }

        static void ShowEventsHistory()
        {
            Console.Clear();

            string[] lines = System.IO.File.ReadAllLines(pathHistoryLib);


            for (int i = 0; i < lines.Length; i++)
            {

                string[] rowData = lines[i].Split(',');

                Console.WriteLine(rowData[0] + " " + rowData[1] + " " + rowData[2]);

            }

            Console.ReadKey();

            ShowMenu();
        }
    }
}
