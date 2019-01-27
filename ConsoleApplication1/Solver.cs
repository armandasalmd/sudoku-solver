using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Solver
    {

        public class Solution
        {
            public enum SOLT { LASTDIGIT, NAKEDCELL, HIDDENSINGLE };

            public SOLT solution;
            public int row = -1, column = -1;
            public int value = 0; // 1 - 9

            public Solution(int row, int column, int val, SOLT solt)
            {
                solution = solt;
                value = val;
                this.row = row;
                this.column = column;
            }
        }

        private const int SOLVELIM = 150;

        private const int WIDTH = 9;
        private static SudokuMap map;
        private static List<Solution> solutions;

        public static bool Main()
        {
            map = new SudokuMap();
            solutions = new List<Solution>();
            string[] rows;
            //if (AskForInput(out rows))
            //{
            /*rows = new string[] {
                "8...146..",
                ".23..54..",
                "6...8..21",
                "..9.68..7",
                "74.....35",
                "...4.79..",
                ".51...2.6",
                ".8..96.5.",
                "4..85...."
            };*/
            rows = new string[]
            {
                ".8..1.72.",
                "34.9.8...",
                "..1....89",
                ".....94.2",
                ".7645....",
                "4..2..17.",
                "..963..4.",
                "65...72..",
                "..7..259."
            };
            /*rows = new string[]
            {
                "..5..1...",
                "97.65.8..",
                ".42..7...",
                "28..7...3",
                "..7...5..",
                "5...3..26",
                "...7..26.",
                "..3.29.58",
                "...5..3.."
            };*/
                map.InitMap(rows);
                if (StartSolving())
                    Console.WriteLine("Sudoku is successfully solved");
                else
                    Console.WriteLine("Sudoku is imposible");
                char a = Console.ReadKey().KeyChar;
            //}
            

            return true;
        }

        private static bool AskForInput(out string[] lines)
        {
            string line = string.Empty;
            string[] rows = new string[SudokuMap.WIDTH];
            int i;

            for (i = 0; i < SudokuMap.WIDTH; i++)
            { // collecting each row
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter 9 numbers of {0} row: (b - back)", i + 1);
                    Console.WriteLine("Use dot(.) for empty", i + 1);
                    Checker.outputMap(rows, i);

                    line = Console.ReadLine();

                    if (line == "b") // isejimas
                    {
                        lines = rows;
                        return false;
                    }
                    if (line.Length == 9)
                    {
                        rows[i] = line;
                        break;
                    }
                        
                }
                
            }
            lines = rows;
            return true;
        }

        private static bool StartSolving()
        {
            int i = 0;
            int suc = 0;
            map.PrintMap();
            while (!map.completed() && i < SOLVELIM)
            {
                switch (i % 3)
                {
                    case 0:
                        if (TryRemoveCandidates())
                            suc++;
                        break;
                    case 1:
                        if (TryFillNaked())
                            suc++;
                        break;
                    case 2:
                        if (TryFill1Missing())
                            suc++;
                        break;
                }
                i++;
            }
            map.PrintMap();
            Console.WriteLine("Success rate:{0} of {1}", suc, SOLVELIM);
            return Checker.checkIfTrue(SudokuMap.KVMapToInt(map));
        }

        private static void FillSolved(Solution sol)
        {
            solutions.Add(sol);
            map.set(sol.row, sol.column, sol.value);
        }

        private static bool TryFill1Missing()
        {
            bool success = false;
            int id0 = -1, count0 = 0;
            for (int i = 0; i < WIDTH; i++) // check for rows
            {
                count0 = map.get0Count(SudokuMap.TYPE.ROW, i, out id0);
                if (count0 == 1)
                {
                    FillSolved(new Solution(i, id0, FindMissing(map.getRow(i)), Solution.SOLT.LASTDIGIT));
                    success = true;
                }
                    
            }
            for (int i = 0; i < WIDTH; i++) // check for columns
            {
                count0 = map.get0Count(SudokuMap.TYPE.COLUMN, i, out id0);
                if (count0 == 1)
                {
                    FillSolved(new Solution(id0, i, FindMissing(map.getColumn(i)), Solution.SOLT.LASTDIGIT));
                    success = true;
                }
            }
            for (int i = 0; i < WIDTH; i++)
            {
                count0 = map.get0Count(SudokuMap.TYPE.CELL, i, out id0);
                if (count0 == 1)
                {
                    int x = 0, y = 0;
                    map.getCellCord(out x, out y, i, id0);
                    FillSolved(new Solution(x, y, FindMissing(map.getCell(i)), Solution.SOLT.LASTDIGIT));
                    success = true;
                }
                    
            }

            return success;
        }

        private static int FindMissing(int[] array)
        {
            bool[] nums = new bool[WIDTH];
            foreach(int i in array)
            {
                if (i > 0)
                    nums[i - 1] = true;
            }
            for (int i = 0; i < WIDTH; i++)
                if (!nums[i])
                    return i + 1;
            return 0;
        }

        private static bool TryFillNaked()
        {
            bool success = false;
            int count = 0, KVID = 0;
            int x = 0, y = 0;
            
            for (int i = 0; i < WIDTH; i++)
            { // check all cells
                SudokuMap.KV[] cells = map.getCellAsKV(i);
                for (int j = 1; j <= WIDTH; j++)
                { // tikrinu skaicius
                    for (int k = 0; k < WIDTH; k++)
                    { // kiek cell yra tokiu galimu skaiciu
                        if (cells[k].possible.Contains(j))
                        {
                            KVID = k;
                            count++;
                        }
                    }
                    if (count == 1)
                    { // radom viena naked
                        // turim info KVID, cell(i)
                        map.getCellCord(out x, out y, i, KVID);
                        FillSolved(new Solution(x, y, j, Solution.SOLT.NAKEDCELL));
                        success = true;
                    }
                    count = 0;
                }
            }

            for (int i = 0; i < WIDTH; i++)
                for (int j = 0; j < WIDTH; j++)
                    if (map.map[i][j].possible.Count == 1)
                    {
                        FillSolved(new Solution(i, j, map.map[i][j].possible[0], Solution.SOLT.NAKEDCELL));
                        success = true;
                    }

            return success;
        }
        
        private static bool TryRemoveCandidates()
        {
            // langely: surenki visas kandidatu poras po du, nelygiagrecius ismeti, lygiagretumo eileje 
            // kitiem ismeti kandidatus

            // cell id, value, KV id 1, KV id 2
            List<int[]> pairs = GetPairs();
            // value kuria removint, orientacija, eile/stul, ign1, ign2
            pairs = SelectWithParallel(pairs);
            return RemoveCandidates1(pairs);
        }

        private static List<int[]> GetPairs()
        {
            List<int[]> pairs = new List<int[]>(); 
            SudokuMap.KV[] cell;

            List<int> temp = new List<int>();
            int count;

            for (int i = 0; i < WIDTH; i++)
            { // per cell
                cell = map.getCellAsKV(i);
                for (int j = 1; j <= WIDTH; j++) // eini per visus skaicius
                {
                    count = 0;
                    temp = new List<int>();
                    for (int k = 0; k < WIDTH; k++)
                        if (cell[k].possible.Contains(j))
                        {
                            temp.Add(k);
                            count++;
                        }

                    if (count == 2)
                        pairs.Add(new int[] { i, j, temp[0], temp[1] });
                }
            }
            return pairs;
        }

        private static List<int[]> SelectWithParallel(List<int[]> list)
        {
            List<int[]> ans = new List<int[]>();
            int[] first = new int[2];
            int[] second = new int[2];
            for (int i = 0; i < list.Count; i++)
            {
                map.getCellCord(out first[0], out first[1], list[i][0], list[i][2]);
                map.getCellCord(out second[0], out second[1], list[i][0], list[i][3]);
                if (first[0] == second[0])
                {
                    int[] temp = new int[] { list[i][1], 0, first[0], first[1], second[1] };
                    ans.Add(temp);
                } else if (first[1] == second[1])
                {
                    int[] temp = new int[] { list[i][1], 1, first[1], first[0], second[0] };
                    ans.Add(temp);
                }
                    
            }
            return ans;
        }

        private static bool RemoveCandidates1(List<int[]> pairs)
        {
            bool success = false;
            for (int i = 0; i < pairs.Count; i++)
            { // visas poras padaro 4
                for (int j = 0; j < WIDTH; j++)
                { // 1 - 9 langeliai
                    if (j != pairs[i][3] && j != pairs[i][4])
                    { // naikinsim
                        if (pairs[i][1] == 0 && map.map[pairs[i][2]][j].possible.Contains(pairs[i][0])) // row
                        {
                            map.map[pairs[i][2]][j].possible.Remove(pairs[i][0]);
                            success = true;
                        }
                        else if (pairs[i][1] == 1 && map.map[j][pairs[i][2]].possible.Contains(pairs[i][0])) // column
                        {
                            map.map[j][pairs[i][2]].possible.Remove(pairs[i][0]);
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        private static bool TryNakedSubset()
        {
            bool success = false;
            // https://www.kristanix.com/sudokuepic/sudoku-solving-techniques.php

            return success;
        }
    }
}
