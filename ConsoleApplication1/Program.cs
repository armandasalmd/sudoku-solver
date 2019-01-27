using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            int choice = loopMenu();
            bool stay = false;
            Console.Clear();
            switch (choice)
            {
                case 1: stay = Checker.Main(); // true = continue, false = exit
                    break;
                case 2: stay = action2();
                    break;
                case 3: stay = Solver.Main();
                    break;
            }
            if (stay)
                Main(args);

            //myMap = new SudokuMap();
            //myMap.PrintMap();
        }

        static int loopMenu()
        {
            int choice = 0;
            while (choice > 4 || choice < 1)
                choice = showMenu();
            return choice;
        }

        static int showMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Check if Sudoku is right");
            Console.WriteLine("2. Generate Sudoku");
            Console.WriteLine("3. Solve Sudoku");
            Console.WriteLine("4. Exit");
            char choice = Console.ReadKey().KeyChar;
            int iChoice = (int)char.GetNumericValue(choice);

            if (iChoice > 4 || iChoice < 1)
            {
                Console.Clear();
                Console.WriteLine("Bad choice!\nClick any key to continue");
                char a = Console.ReadKey().KeyChar;
                return -1;
            }
            return iChoice;
        }



        static bool action2()
        {
            return true;
        }

    }
}
