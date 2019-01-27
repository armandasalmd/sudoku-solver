using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Checker
    {

        public static bool Main()
        {
            string line = string.Empty;
            int[][] map = new int[SudokuMap.WIDTH][];
            string[] rows = new string[SudokuMap.WIDTH];
            bool back = false;
            int i;

            for (i = 0; i < SudokuMap.WIDTH; i++)
            { // collecting each row
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter 9 numbers of {0} row: (b - back)", i + 1);
                    outputMap(rows, i);
                
                    line = Console.ReadLine();

                    if (line == "b") // isejimas
                    {
                        back = true;
                        break;
                    }

                    int a = 0;
                    if (line.Length == 9 && int.TryParse(line, out a)) // tikrina ar geras
                    {
                        rows[i] = line;
                        map[i] = stringToRow(line);
                        break;
                    }
                    else
                        informFailure();

                }

                if (back)
                {
                    back = false;
                    //map[0] = new int[] { 2, 9, 5, 7, 4, 3, 8, 6, 1 };
                    //map[1] = new int[] { 4, 3, 1, 8, 6, 5, 9, 2, 7 };
                    //map[2] = new int[] { 8, 7, 6, 1, 9, 2, 5, 4, 3 };
                    //map[3] = new int[] { 3, 8, 7, 4, 5, 9, 2, 1, 6 };
                    //map[4] = new int[] { 6, 1, 2, 3, 8, 7, 4, 9, 5 };
                    //map[5] = new int[] { 5, 4, 9, 2, 1, 6, 7, 3, 8 };
                    //map[6] = new int[] { 7, 6, 3, 5, 2, 4, 1, 8, 9 };
                    //map[7] = new int[] { 9, 2, 8, 6, 7, 1, 3, 5, 4 };
                    //map[8] = new int[] { 1, 5, 4, 9, 3, 8, 6, 7, 2 };
                    //i = 9;
                    break;
                }                
            }
            
            if (i == SudokuMap.WIDTH)
            {
                if (checkIfTrue(map))
                    Console.WriteLine("Sudoku is right!");
                else
                    Console.WriteLine("Sudoku is NOT right!");
                Console.WriteLine("Press any key to continue");
                char c = Console.ReadKey().KeyChar;
            }

            return true;
        }
        

        public static bool checkIfTrue(int[][] array)
        {
            int sum = 0;
            for (int i = 0; i < SudokuMap.WIDTH; i++) // Tikrina eiles
            {
                for (int j = 0; j < SudokuMap.WIDTH; j++)
                    sum += array[i][j];
                if (sum != 45)
                    return false;
                sum = 0;
            }

            int[] cSum = new int[SudokuMap.WIDTH];
            for (int i = 0; i < SudokuMap.WIDTH; i++) // Tikrina eiles
            {
                for (int j = 0; j < SudokuMap.WIDTH; j++)
                    cSum[j] += array[i][j];
            }
            for (int i = 0; i < SudokuMap.WIDTH; i++)
                if (cSum[i] != 45)
                    return false;                
            return true;
        }

        private static int[] stringToRow(string str)
        {
            int[] row = new int[SudokuMap.WIDTH];
            for (int i = 0; i < SudokuMap.WIDTH; i++)
                row[i] = (int)Char.GetNumericValue(str[i]);
            return row;
        }

        public static void informFailure()
        {
            Console.Clear();
            Console.WriteLine("Wrong input. Please press any key to try again!");
            char a = Console.ReadKey().KeyChar;
        }

        public static void outputMap(string[] array, int rows)
        {
            for (int i = 0; i < rows; i++)
                Console.WriteLine(array[i]);
        }

    }
}
