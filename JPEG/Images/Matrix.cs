using System.Drawing;
using System.Drawing.Imaging;

namespace JPEG.Images
{
    class Matrix
    {
        public readonly Pixel[,] Pixels;
        public readonly int Height;
        public readonly int Width;

        public Matrix(int height, int width)
        {
            Height = height;
            Width = width;

            Pixels = new Pixel[height, width];
            for (var i = 0; i < height; ++i)
            for (var j = 0; j < width; ++j)
                Pixels[i, j] = new Pixel(0, 0, 0, PixelFormat.RGB);
        }

        public static explicit operator Matrix(Bitmap bmp)
        {
            var height = bmp.Height - bmp.Height % 8;
            var width = bmp.Width - bmp.Width % 8;
            var matrix = new Matrix(height, width);

            var bmd = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, bmp.PixelFormat);
            unsafe
            {
                for (var y = 0; y < height; y++)
                {
                    var row = (byte*) bmd.Scan0 + y * bmd.Stride;
                    for (var x = 0; x < width; x++)
                    {
                        matrix.Pixels[y, x] = new Pixel(row[3 * x + 2], row[3 * x + 1], row[3 * x], PixelFormat.RGB);
                    }
                }
            }

            bmp.UnlockBits(bmd);


            return matrix;
        }

        public static explicit operator Bitmap(Matrix matrix)
        {
            var bmp = new Bitmap(matrix.Width, matrix.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            var bmd = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, bmp.PixelFormat);
            unsafe
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    var row = (byte*) bmd.Scan0 + y * bmd.Stride;
                    for (var x = 0; x < bmp.Width; x++)
                    {
                        var pixel = matrix.Pixels[y, x];
                        row[3 * x + 2] = pixel.R;
                        row[3 * x + 1] = pixel.G;
                        row[3 * x] = pixel.B;
                    }
                }
            }

            return bmp;
        }
    }
}
