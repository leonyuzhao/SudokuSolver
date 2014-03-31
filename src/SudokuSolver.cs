using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DancingLinks;

namespace SudoKu
{
    public class SudokuSolver
    {
        // At least 17 number to solve a standard sudoku
        const int cLimit = 17;

        private int[,] i_SudokuMatrix;
        private List<int[]> i_SolidDLXRows;
        private List<int[]> i_GuessDLXRows;

        public SudokuSolver(string origin)
        {
            if (origin == null || origin.Length != 81) { throw new Exception("Invalid Sudoku string."); }

            i_SolidDLXRows = new List<int[]>();
            i_GuessDLXRows = new List<int[]>();
            i_SudokuMatrix = new int[9, 9];

            // Left to Right, Up to Down, . = null
            int numCount = 0;
            for (int i = 0; i < origin.Length; i++)
            {
                // Cal each item's location
                int x, y;
                if ((i + 1) % 9 == 0)
                {
                    x = 9;
                    y = (i + 1) / 9;
                }
                else
                {
                    x = (i + 1) % 9;
                    y = (i + 1) / 9 + 1;
                }

                char tempChar = origin[i];
                if (tempChar == '.')
                {
                    i_GuessDLXRows.AddRange(ConvertToDLXRow(x, y).ToArray());
                    numCount++;
                }
                else
                {
                    if (tempChar < '0' || tempChar > '9') { throw new ArgumentException("Input value should be a number."); }
                    i_SolidDLXRows.Add(ConvertToDLXRow(x, y, tempChar - '0'));
                    i_SudokuMatrix[x - 1, y - 1] = tempChar - '0';
                }
            }
            if (numCount < cLimit) { throw new Exception("Sudoku needs at least 17 numbers."); }
        }

        public bool Solve()
        {
            if (i_GuessDLXRows.Count == 0 || i_SolidDLXRows.Count == 0)
            {
                throw new Exception("Invalid Sudoku.");
            }
            DLX dlxEngine = new DLX(324);
            int index = 1;
            Dictionary<int, int[]> map = new Dictionary<int, int[]>();    
            // TODO
            // Push solid rows into DLX engine's solution to reduce the matrix size for speeding up
            foreach (int[] item in i_SolidDLXRows)
            {
                dlxEngine.AppendRow(item);
                map.Add(index, item);
                index++;
            }
            foreach (int[] item in i_GuessDLXRows)
            {
                dlxEngine.AppendRow(item);
                map.Add(index, item);
                index++;
            }
            dlxEngine.Solve(0);
            Stack<int> result = dlxEngine.Solution;
            if (result.Count == 0)
            {
                return false;
            }
            foreach (int item in result)
            {
                int[] set = ConvertBack(map[item]);
                i_SudokuMatrix[set[0] - 1, set[1] - 1] = set[2];
            }
            return true;
        }

        /* 
         * <Restrictions>
         * 
         *  RES #1
            Column N [1,81]: (X(r),Y(c)) has a value
            X = FLOOR((N-1) / 9) + 1;
            Y = ((N-1) MOD 9) + 1;
            N = (X - 1) * 9 + Y;

            RES #2
            Column N [82,162]: Row X has value Y
            X = FLOOR((N - 81 - 1) / 9) + 1;
            Y = ((N - 81 - 1) MOD 9) + 1;
            N = (X - 1) * 9 + Y + 81;

            RES #3
            Column N [163,243]: Column X has value Y
            X = FLOOR((N - 162 - 1) / 9) + 1;
            Y = ((N - 162 - 1) MOD 9) + 1;
            N = (X - 1) * 9 + Y + 162;

            RES #4
            Column N [244,324]: Set X has value Y
            X = FLOOR((N - 243 - 1) / 9) + 1;
            Y = ((N - 243 - 1) MOD 9) + 1;
            N = (X - 1) * 9 + Y + 243;
         *
         */

        // Convert (X, Y) with value into DLX row
        private int[] ConvertToDLXRow(int row, int column, int value)
        {
            int[] columnNum = new int[4];
            // RES #1
            columnNum[0] = (row - 1) * 9 + column;

            // RES #2
            columnNum[1] = (row - 1) * 9 + value + 81;

            // RES #3
            columnNum[2] = (column - 1) * 9 + value + 162;

            // RES #4
            int set = 0;
            if (row >= 1 && row <= 3 && column >= 1 && column <= 3) { set = 1; }
            if (row >= 1 && row <= 3 && column >= 4 && column <= 6) { set = 2; }
            if (row >= 1 && row <= 3 && column >= 7 && column <= 9) { set = 3; }
            if (row >= 4 && row <= 6 && column >= 1 && column <= 3) { set = 4; }
            if (row >= 4 && row <= 6 && column >= 4 && column <= 6) { set = 5; }
            if (row >= 4 && row <= 6 && column >= 7 && column <= 9) { set = 6; }
            if (row >= 7 && row <= 9 && column >= 1 && column <= 3) { set = 7; }
            if (row >= 7 && row <= 9 && column >= 4 && column <= 6) { set = 8; }
            if (row >= 7 && row <= 9 && column >= 7 && column <= 9) { set = 9; }
            if (set == 0) { throw new Exception("Can not find set."); }
            columnNum[3] = (set - 1) * 9 + value + 243;

            return columnNum;
        }

        // Convert (X, Y) without value into DLX rows
        private List<int[]> ConvertToDLXRow(int row, int column)
        {
            List<int[]> columnNums = new List<int[]>();
            for (int i = 1; i < 10; i++)
            {
                columnNums.Add(ConvertToDLXRow(row, column, i));
            }
            return columnNums;
        }

        private int[] ConvertBack(int[] columnNum)
        {
            int[] set = new int[3];

            set[0] = (int)Math.Floor((double)(columnNum[0] - 1) / 9) + 1; // X
            set[1] = ((columnNum[0] - 1) % 9) + 1; // Y
            set[2] = ((columnNum[1] - 81 - 1) % 9) + 1; // Value

            return set;
        }

        public int[,] SudokuMatrix
        {
            get { return i_SudokuMatrix; }
        }
    }
}
