using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DancingLinks;
using SudoKu;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Exact Match");
            Console.WriteLine("{3,5,6}");
            Console.WriteLine("{1,4,7}");
            Console.WriteLine("{2,3,6}");
            Console.WriteLine("{1,4}");
            Console.WriteLine("{2,7}");
            Console.WriteLine("{4,5,7}");
            
            DLX x = new DLX(7);

            x.AppendRow(3, 5, 6);
            x.AppendRow(1, 4, 7);
            x.AppendRow(2, 3, 6);
            x.AppendRow(1, 4);
            x.AppendRow(2, 7);
            x.AppendRow(4, 5, 7);

            DateTime t1 = DateTime.Now;
            x.Solve(0);
            DateTime t2 = DateTime.Now;
            Console.WriteLine(string.Format("Used {0} milliseconds.", (t2 - t1).TotalMilliseconds));
            Console.WriteLine(string.Format("Exact Match Result: {{{0}}}", string.Join(",", x.Solution.ToArray())));
            Console.WriteLine();
            
            Console.WriteLine("Test Sudoku");
            string board = "8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..";

            SudokuSolver solver = new SudokuSolver(board);
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(solver.SudokuMatrix[j, i] + " ");
                }
                Console.WriteLine();
            }

            t1 = DateTime.Now;
            bool solved = solver.Solve();
            t2 = DateTime.Now;
            Console.WriteLine(string.Format("{0} Used {1} milliseconds.", solved ? "Solved." : "Not solved.", (t2 - t1).TotalMilliseconds));

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(solver.SudokuMatrix[j, i] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}

