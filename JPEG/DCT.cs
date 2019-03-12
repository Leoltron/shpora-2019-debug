using System;
using JPEG.Utilities;

namespace JPEG
{
    public class DCT
    {
        public static double[,] DCT2D(double[,] input)
        {
            var height = input.GetLength(0);
            var width = input.GetLength(1);
            var coeffs = new double[width, height];

            MathEx.LoopByTwoVariables(
                0, width,
                0, height,
                (u, v) =>
                {
                    var sum = MathEx
                       .SumByTwoVariables(
                            0, width,
                            0, height,
                            (x, y) => BasisFunction(input[x, y], u, v, x, y, height, width));

                    coeffs[u, v] = sum * Beta(height, width) * Alpha(u) * Alpha(v);
                });

            return coeffs;
        }

        public static void IDCT2D(double[,] coeffs, double[,] output)
        {
            for (var x = 0; x < coeffs.GetLength(1); x++)
            {
                for (var y = 0; y < coeffs.GetLength(0); y++)
                {
                    var sum = MathEx
                       .SumByTwoVariables(
                            0, coeffs.GetLength(1),
                            0, coeffs.GetLength(0),
                            (u, v) =>
                                BasisFunction(coeffs[u, v], u, v, x, y, coeffs.GetLength(0), coeffs.GetLength(1)) *
                                Alpha(u) * Alpha(v));

                    output[x, y] = sum * Beta(coeffs.GetLength(0), coeffs.GetLength(1));
                }
            }
        }

        public static double BasisFunction(double a, double u, double v, double x, double y, int height, int width)
        {
            var b = Math.Cos(((2d * x + 1d) * u * Math.PI) / (2 * width));
            var c = Math.Cos(((2d * y + 1d) * v * Math.PI) / (2 * height));

            return a * b * c;
        }

        private static readonly double a1 = 1 / Math.Sqrt(2);

        private static double Alpha(int u)
        {
            return u == 0 ? a1 : 1;
        }

        private static double Beta(int height, int width)
        {
            return 1d / width + 1d / height;
        }

        public static double[] DCT1D(double[] input)
        {
            var count = input.Length;
            var output = new double[count];
            for (var u = 0; u < count; u++)
            {
                for (var x = 0; x < count; x++)
                {
                    output[u] += input[x] * Math.Cos(((2d * x + 1) * u * Math.PI) / (2 * count));
                }

                output[u] *= Alpha(u) / 2;
            }

            return output;
        }

        public static double[,] DCT2DOpt1(double[,] input)
        {
            var height = input.GetLength(0);
            var width = input.GetLength(1);
            var coeffs = new double[height, width];

            var row = new double[width];
            var column = new double[height];

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                    row[j] = input[i, j];
                row = DCT1D(row);
                for (var j = 0; j < width; j++)
                    coeffs[i, j] = row[j];
            }

            for (var j = 0; j < width; j++)
            {
                for (var i = 0; i < height; i++)
                    column[i] = coeffs[i, j];
                column = DCT1D(column);
                for (var i = 0; i < height; i++)
                    coeffs[i, j] = column[i];
            }

            return coeffs;
        }
    }
}
