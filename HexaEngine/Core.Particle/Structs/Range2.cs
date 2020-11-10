using SharpDX;
using System;

namespace HexaEngine.Core.Particle.Structs
{
    public struct Range2
    {
        public float XMin;
        public float XMax;
        public float YMin;
        public float YMax;

        public Range2(float xMin, float xMax, float yMin, float yMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }

        public Vector2 GetVector(Random random)
        {
            return new Vector2(random.NextFloat(XMin, XMax), random.NextFloat(YMin, YMax));
        }
    }
}