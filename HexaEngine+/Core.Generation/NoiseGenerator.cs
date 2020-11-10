using HexaEngine.Core.Render.Components;
using HexaEngine.Core.Ressources;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace HexaEngine.Core.Generation
{
    public static class NoiseGenerator
    {
        public static Sprite CreateUnmanaged(Engine engine, TimeSpan time, string name, int width, int height)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            for (int y = 0; y < height;)
            {
                for (int x = 0; x < width;)
                {
                    byte v = (byte)((Perlin.Generate(x, y) + 1) / 2 * 255);
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(v, v, v, v));
                    x++;
                }
                y++;
            }

            return Sprite.LoadUnmanaged(engine, time, Texture.LoadUnmanaged(engine, bitmap, name));
        }

        public static void Create(Engine engine, TimeSpan time, string name, int width, int height, params NoiseRule[] rules)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            for (int y = 0; y < height;)
            {
                for (int x = 0; x < width;)
                {
                    int v = (int)((Perlin.Generate(x, y) + 1) / 2 * 255);
                    rules.ToList().ForEach(rule => rule.ApplyRule(x, y, ref v));
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(v, v, v, v));
                    x++;
                }
                y++;
            }

            Sprite sprite = Sprite.LoadUnmanaged(engine, time, Texture.LoadUnmanaged(engine, bitmap, name));
            RessourceManager.Sprites.Add(sprite);
        }

        public static int[][] Create(int width, int height, int variation = 255, int seed = 0, params NoiseRule[] rules)
        {
            int[][] memory = new int[height][];
            for (int y = 0; y < height;)
            {
                memory[y] = new int[width];
                for (int x = 0; x < width;)
                {
                    int v = (int)((Perlin.Generate(x + seed, y + seed) + 1) / 2 * variation);
                    rules.ToList().ForEach(rule => rule.ApplyRule(x, y, ref v));
                    memory[y][x] = v;
                    x++;
                }
                y++;
            }

            return memory;
        }
    }
}