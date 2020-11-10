using SharpDX;
using System;

namespace HexaEngine.Core.Particle.Structs
{
    public struct Range
    {
        public float Min;
        public float Max;

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetFloat(Random random) => random.NextFloat(Min, Max);
    }
}