using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace JPEG
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class DCT
    {
        public static void IDCT2D(double[,] input, double[,] output)
        {
            var height = input.GetLength(0);
            var width = input.GetLength(1);

            for (var i = 0; i < height; i++)
            {
                var row = new double[width];
                for (var j = 0; j < width; j++)
                    row[j] = input[i, j];
                row = IDCT1D(row);
                for (var j = 0; j < width; j++)
                    output[i, j] = row[j];
            }

            for (var j = 0; j < width; j++)
            {
                var column = new double[height];
                for (var i = 0; i < height; i++)
                    column[i] = output[i, j];
                column = IDCT1D(column);
                for (var i = 0; i < height; i++)
                    output[i, j] = column[i];
            }
        }

        private static readonly double sqrt = 2 * Math.Sqrt(2);

        private static double[] IDCT1D(IReadOnlyList<double> input)
        {
            var count = input.Count;
            var output = new double[count];
            var fact = Math.PI / count;
            var d = input[0];

            for (var k = 0; k < count; k++)
            {
                output[k] = d;
                for (var n = 1; n < count; n++)
                {
                    output[k] += input[n] * Math.Cos(fact * n * (k + 0.5d));
                }

                output[k] /= sqrt;
            }

            return output;
        }

        public static double[,] DCT2D8x8(double[,] input)
        {
            if (input.GetLength(0) != 8 || input.GetLength(1) != 8)
            {
                throw new ArgumentException($"Expected array 8x8, got {input.GetLength(0)}x{input.GetLength(1)}");
            }

            var output = new double[8, 8];
            var tmp = new int[8, 8];

            const int
                c1 = 1004, // cos(pi/16) << 10 
                s1 = 200, // sin(pi/16) 
                c3 = 851, // cos(3pi/16) << 10 
                s3 = 569, // sin(3pi/16) << 10 
                r2c6 = 554, // sqrt(2)*cos(6pi/16) << 10 
                r2s6 = 1337, // sqrt(2)*sin(6pi/16) << 10 
                r2 = 181; // sqrt(2) << 7

            for (var i = 0; i < 8; i++)
            {
                var x0 = (int) input[i, 0];
                var x1 = (int) input[i, 1];
                var x2 = (int) input[i, 2];
                var x3 = (int) input[i, 3];
                var x4 = (int) input[i, 4];
                var x5 = (int) input[i, 5];
                var x6 = (int) input[i, 6];
                var x7 = (int) input[i, 7];

                /* Stage 1 */
                var x8 = x7 + x0;
                x0 -= x7;
                x7 = x1 + x6;
                x1 -= x6;
                x6 = x2 + x5;
                x2 -= x5;
                x5 = x3 + x4;
                x3 -= x4;

                /* Stage 2 */
                x4 = x8 + x5;
                x8 -= x5;
                x5 = x7 + x6;
                x7 -= x6;
                x6 = c1 * (x1 + x2);
                x2 = (-s1 - c1) * x2 + x6;
                x1 = (s1 - c1) * x1 + x6;
                x6 = c3 * (x0 + x3);
                x3 = (-s3 - c3) * x3 + x6;
                x0 = (s3 - c3) * x0 + x6;

                /* Stage 3 */
                x6 = x4 + x5;
                x4 -= x5;
                x5 = r2c6 * (x7 + x8);
                x7 = (-r2s6 - r2c6) * x7 + x5;
                x8 = (r2s6 - r2c6) * x8 + x5;
                x5 = x0 + x2;
                x0 -= x2;
                x2 = x3 + x1;
                x3 -= x1;

                /* Stage 4 and output */
                tmp[i, 0] = x6;
                tmp[i, 4] = x4;
                tmp[i, 2] = x8 >> 10;
                tmp[i, 6] = x7 >> 10;
                tmp[i, 7] = (x2 - x5) >> 10;
                tmp[i, 1] = (x2 + x5) >> 10;
                tmp[i, 3] = (x3 * r2) >> 17;
                tmp[i, 5] = (x0 * r2) >> 17;
            }

            for (var i = 0; i < 8; i++)
            {
                var x0 = tmp[0, i];
                var x1 = tmp[1, i];
                var x2 = tmp[2, i];
                var x3 = tmp[3, i];
                var x4 = tmp[4, i];
                var x5 = tmp[5, i];
                var x6 = tmp[6, i];
                var x7 = tmp[7, i];

                /* Stage 1 */
                var x8 = x7 + x0;
                x0 -= x7;
                x7 = x1 + x6;
                x1 -= x6;
                x6 = x2 + x5;
                x2 -= x5;
                x5 = x3 + x4;
                x3 -= x4;

                /* Stage 2 */
                x4 = x8 + x5;
                x8 -= x5;
                x5 = x7 + x6;
                x7 -= x6;
                x6 = c1 * (x1 + x2);
                x2 = (-s1 - c1) * x2 + x6;
                x1 = (s1 - c1) * x1 + x6;
                x6 = c3 * (x0 + x3);
                x3 = (-s3 - c3) * x3 + x6;
                x0 = (s3 - c3) * x0 + x6;

                /* Stage 3 */
                x6 = x4 + x5;
                x4 -= x5;
                x5 = r2c6 * (x7 + x8);
                x7 = (-r2s6 - r2c6) * x7 + x5;
                x8 = (r2s6 - r2c6) * x8 + x5;
                x5 = x0 + x2;
                x0 -= x2;
                x2 = x3 + x1;
                x3 -= x1;

                /* Stage 4 and output */
                output[0, i] = (x6 + 16) >> 3;
                output[4, i] = (x4 + 16) >> 3;
                output[2, i] = (x8 + 16384) >> 13;
                output[6, i] = (x7 + 16384) >> 13;
                output[7, i] = (x2 - x5 + 16384) >> 13;
                output[1, i] = (x2 + x5 + 16384) >> 13;
                output[3, i] = ((x3 >> 8) * r2 + 8192) >> 12;
                output[5, i] = ((x0 >> 8) * r2 + 8192) >> 12;
            }

            return output;
        }
    }
}
