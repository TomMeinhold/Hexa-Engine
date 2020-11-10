using System;

namespace HexaEngine.Core.Generation
{
    public class ObjectGenerator
    {
        public ObjectGenerator(int width, int height, int variation = 255, int seed = 0, params NoiseRule[] rules)
        {
            noise = NoiseGenerator.Create(width, height, variation, seed, rules);
        }

        private int[][] noise { get; }

        public void Generate(Action<int, int, int> action)
        {
            int y = 0, x = 0;
            foreach (int[] row in noise)
            {
                foreach (int v in row)
                {
                    action.Invoke(v, x, y);
                    x++;
                }
                y++;
                x = 0;
            }
        }
    }
}