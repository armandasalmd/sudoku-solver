using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class SudokuMap
    {
        public class KV
        {
            public bool org;
            private int VAL;
            public int val
            {
                set
                {
                    possible = new List<int>();
                    VAL = value;
                }
                get
                {
                    return VAL;
                }
            }
            public List<int> possible = new List<int>();
            
        }

        public const int WIDTH = 9;
        public const char SEPHOR = '-';
        public const char SEPVER = '|';
        public const char EMPTY = '.';

        private int filledCount = 0;

        public enum TYPE { ROW, COLUMN, CELL};

        public KV[][] map = new KV[WIDTH][]; // [eile][stulpelis]

        //int[][] map = new int[WIDTH][]; // [eile][stulpelis]

        public void InitMap(string[] rows)
        {
            map = init2D(WIDTH, map);

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                    if (rows[i][j] != EMPTY)
                    {
                        map[i][j].org = true;
                        map[i][j].val = (int)Char.GetNumericValue(rows[i][j]);
                        filledCount++;
                    }
            }
            InitCellPossibilities();
        }

        private void InitCellPossibilities()
        {
            List<int> missingNums = new List<int>(), copy;
            int[] cell;
            int x, y;

            for (int i = 0; i < WIDTH; i++)
            { // throw each cell
                cell = getCell(i);
                for (int j = 1; j <= WIDTH; j++)
                    if (!cell.Contains(j))
                        missingNums.Add(j);

                for (int j = 0; j < WIDTH; j++)
                {
                    int[] copy1 = missingNums.ToArray();
                    copy = copy1.ToList();

                    getCellCord(out x, out y, i, j);
                    if (map[x][y].val == 0) {
                        for (int k = missingNums.Count - 1; k >= 0; k--)
                        { // check if possibility is posible
                            if (getRow(x).Contains(copy[k]) || getColumn(y).Contains(copy[k]))
                                copy.Remove(copy[k]);
                        }
                        map[x][y].possible = copy;
                    }
                    
                }
                missingNums = new List<int>();
            }
            
        }

        public int get0Count(TYPE type, int id, out int result)
        {
            int[] data;
            result = -1;
            switch (type)
            {
                case TYPE.ROW: data = getRow(id);
                    break;
                case TYPE.COLUMN: data = getColumn(id);
                    break;
                case TYPE.CELL: data = getCell(id);
                    break;
                default: return 0;
            }
            int count = 0;
            for (int i = 0; i < WIDTH; i++)
                if (data[i] == 0)
                {
                    count++;
                    result = i;
                }
                    
            return count;
        }

        public int[] getRow(int row)
        {
            int[] temp = new int[WIDTH];
            for (int i = 0; i < WIDTH; i++)
                temp[i] = map[row][i].val;
            return temp;
        }

        public int[] getColumn(int column)
        {
            int[] temp = new int[WIDTH];
            for (int i = 0; i < WIDTH; i++)
                temp[i] = map[i][column].val;
            return temp;
        }

        public int[] getCell(int cell)
        {
            int[] array = new int[WIDTH];
            int startC, startR;
            cellStartCord(cell, out startR, out startC);

            int a = 0;
            for (int i = startR; i <= startR + 2; i++)
                for (int j = startC; j <= startC + 2; j++)
                {
                    array[a] = map[i][j].val;
                    a++;
                }

            return array;
        }

        public KV[] getCellAsKV(int cell)
        {
            KV[] array = new KV[WIDTH];
            int startC, startR;
            cellStartCord(cell, out startR, out startC);

            int a = 0;
            for (int i = startR; i <= startR + 2; i++)
                for (int j = startC; j <= startC + 2; j++)
                {
                    array[a] = map[i][j];
                    a++;
                }

            return array;
        }
        
        public void getCellCord(out int row, out int column, int cell, int cellID)
        {
            //cell++; // 0 = 1
            int startC = (cell % 3) * 3; // 0 1 2 0 1 2 0 1 2
            int startR = (cell / 3) * 3; // 0 0 0 1 1 1 2 2 2
            column = startC + (cellID % 3);
            row = startR + (cellID / 3);
        } 

        public int getNum(int row, int column)
        {
            return map[row][column].val;
        }

        public void PrintMap()
        {
            /*| 1 2 3 | 1 2 3 | 1 2 3 |
              | 4 5 6 | 4 5 6 | 4 5 6 |
              | 7 8 9 | 7 8 9 | 7 8 9 |
              -------------------------*/
            Console.WriteLine(topStr());

            string line = String.Empty;
            for (int i = 1; i < WIDTH + 1; i++) // print segmentais kaip komentare
            {
                line = "| ";
                int[] row = getRow(i - 1);
                for (int k = 1; k < WIDTH + 1; k++)
                {
                    if (row[k - 1] == 0 || row[k - 1] == -1)
                        line += EMPTY + " ";
                    else
                        line += row[k - 1].ToString() + " ";
                    if (k % 3 == 0)
                        line += "| ";
                }
                Console.WriteLine(line);
                if (i % 3 == 0)
                    Console.WriteLine(topStr());
            }
        }

        private string topStr()
        {
            string ans = "";
            for (int i = 0; i < 25; i++)
                ans += SEPHOR;
            return ans;
        }

        private KV[][] init2D(int rowCount, KV[][] array)
        {
            for (int i = 0; i < rowCount; i++)
                array[i] = new KV[WIDTH];
            for (int i = 0; i < WIDTH; i++) // col
                for (int j = 0; j < rowCount; j++) // row
                    array[j][i] = new KV();
            return array;
        }

        public void set(int row, int column, int value)
        {
            if (value == 0)
            {
                if (map[row][column].val != 0)
                {
                    map[row][column].val = value;
                    filledCount--;
                }
            } else
            {
                map[row][column].val = value;
                filledCount++;
            }
            RemovePossibilities(row, column, value);
        }

        public int get(int row, int column)
        {
            return map[row][column].val;
        }

        public bool completed()
        {
            if (filledCount == 81)
                return true;
            else
                return false;
        }

        private int cellInt(int row, int column)
        { // 2 6
            int c = column / 3 + 1;
            int r = row / 3;
            return r * 3 + c - 1;
        }

        private void cellStartCord(int cell, out int x, out int y)
        {
            y = (cell % 3) * 3; // 0 1 2 0 1 2 0 1 2
            x = (cell / 3) * 3; // 0 0 0 1 1 1 2 2 2
        }

        public void RemovePossibilities(int x, int y, int value)
        {
            int startR, startC;
            cellStartCord(cellInt(x, y), out startR, out startC);
            for (int i = 0; i < WIDTH; i++)
            {
                if (map[i][y].possible.Contains(value)) // row
                    map[i][y].possible.Remove(value);
                if (map[x][i].possible.Contains(value)) // column
                    map[x][i].possible.Remove(value);
            }
            for (int i = startR; i < startR + 3; i++)
                for (int j = startC; j < startC + 3; j++)
                    if (map[i][j].possible.Contains(value)) // cell
                        map[i][j].possible.Remove(value);
        }

        public static int[][] KVMapToInt(SudokuMap map)
        {
            int[][] result = new int[WIDTH][];
            int[] row = new int[WIDTH];
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    row[j] = map.map[i][j].val;
                }
                result[i] = row;
                row = new int[WIDTH];
            }
            return result;
        }
    }
}
