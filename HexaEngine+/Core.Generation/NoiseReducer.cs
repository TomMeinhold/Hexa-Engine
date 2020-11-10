using System.Collections.Generic;
using System.Drawing;

namespace HexaEngine.Core.Generation
{
    public static class NoiseReducer
    {
        public static void ReduceNoise(Bitmap bitmap, float strenght)
        {
            for (int y = 0; y < bitmap.Height;)
            {
                for (int x = 0; x < bitmap.Width;)
                {
                    List<Color> pixels = new List<Color>();
                    if (x - 1 > 0)
                    {
                        pixels.Add(bitmap.GetPixel(x - 1, y));
                    }

                    if (x + 1 < bitmap.Width)
                    {
                        pixels.Add(bitmap.GetPixel(x + 1, y));
                    }

                    if (y - 1 > 0)
                    {
                        pixels.Add(bitmap.GetPixel(x, y - 1));
                    }

                    if (y + 1 < bitmap.Height)
                    {
                        pixels.Add(bitmap.GetPixel(x, y + 1));
                    }

                    bitmap.SetPixel(x, y, Blend(bitmap.GetPixel(x, y), pixels.ToArray()));
                    x++;
                }
                y++;
            }
        }

        private static Color Blend(Color baseColor, params Color[] colors)
        {
            int a = baseColor.A;
            int r = baseColor.R;
            int g = baseColor.G;
            int b = baseColor.B;
            foreach (Color color in colors)
            {
                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;
            }

            a /= colors.Length + 1;
            r /= colors.Length + 1;
            g /= colors.Length + 1;
            b /= colors.Length + 1;
            return Color.FromArgb(a, r, g, b);
        }
    }
}