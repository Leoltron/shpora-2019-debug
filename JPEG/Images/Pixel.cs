using System;

namespace JPEG.Images
{
    public struct Pixel
    {
        public Pixel(double a, double b, double c, PixelFormat format)
            : this(ToByte(a), ToByte(b), ToByte(c), format)
        {
        }

        public Pixel(byte firstComponent, byte secondComponent, byte thirdComponent, PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Rgb:
                    R = firstComponent;
                    G = secondComponent;
                    B = thirdComponent;
                    break;
                case PixelFormat.YCbCr:
                    var y = firstComponent;
                    var cb = secondComponent;
                    var cr = thirdComponent;

                    R = ToByte((298.082 * y + 408.583 * cr) / 256.0 - 222.921);
                    G = ToByte((298.082 * y - 100.291 * cb - 208.120 * cr) / 256.0 + 135.576);
                    B = ToByte((298.082 * y + 516.412 * cb) / 256.0 - 276.836);
                    break;
                default:
                    throw new FormatException("Unknown pixel format: " + pixelFormat);
            }
        }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public byte Y => ToByte(16.0 + (65.738 * R + 129.057 * G + 24.064 * B) / 256.0);
        public byte Cb => ToByte(128.0 + (-37.945 * R - 74.494 * G + 112.439 * B) / 256.0);
        public byte Cr => ToByte(128.0 + (112.439 * R - 94.154 * G - 18.285 * B) / 256.0);


        private static byte ToByte(double d)
        {
            var val = (int) d;
            if (val > byte.MaxValue)
                return byte.MaxValue;
            if (val < byte.MinValue)
                return byte.MinValue;
            return (byte) val;
        }
    }
}
