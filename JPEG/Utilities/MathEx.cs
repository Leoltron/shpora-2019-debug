using System;
using System.Threading.Tasks;

namespace JPEG.Utilities
{
    public static class MathEx
    {
        public static double SumByTwoVariables(int from1, int to1, int from2, int to2, Func<int, int, double> function)
        {
            var sum = 0d;
            for (var i1 = from1; i1 < to1; i1++)
            for (var i2 = from2; i2 < to2; i2++)
                sum += function(i1, i2);

            return sum;
        }

        public static void LoopByTwoVariables(int from1, int to1, int from2, int to2, Action<int, int> function)
        {
            Parallel.For(from1, to1, i1 =>
            {
                Parallel.For(from2, to2, i2 => function(i1, i2));
            });
        }
    }
}
