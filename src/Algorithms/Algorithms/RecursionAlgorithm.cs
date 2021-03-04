using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public static class RecursionAlgorithm
    {
        #region Sum

        public static int SumByRecursion(List<int> datum)
        {
            if (datum.Count == 0)
            {
                return 0;
            }

            var currentData = datum.First();
            return currentData + SumByRecursion(datum.Skip(1).ToList());
        }

        #endregion

        #region Hanoi

        public static void HanoiTower(int n, char a, char b, char c)
        {
            if (n == 1)
            {
                MoveHanoiPanel(a, b);
                return;
            }

            HanoiTower(n-1, a, c, b);
            MoveHanoiPanel(a, b);
            HanoiTower(n-1, c, b, a);
        }

        private static void MoveHanoiPanel(char x, char y)
        {
            Console.WriteLine($"Move {x} to {y}");
        }

        #endregion

        #region Climb the stair

        public static int GetMethodCount_ClimbStair(int stepCount)
        {
            if (stepCount <= 0) return 0;
            if (stepCount == 1) return 1;
            if (stepCount == 2) return 2;

            return GetMethodCount_ClimbStair(stepCount - 1) + GetMethodCount_ClimbStair(stepCount - 2);
        }

        #endregion

        

    }
}
