using SharpDX;
using System;

namespace HexaEngine.Core.Particle.Structs
{
    public struct Range3
    {
        public float XMin;
        public float XMax;
        public float YMin;
        public float YMax;
        public float ZMin;
        public float ZMax;

        public Range3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            ZMin = zMin;
            ZMax = zMax;
        }

        public Vector3 GetVector(Random random)
        {
            return new Vector3(random.NextFloat(XMin, XMax), random.NextFloat(YMin, YMax), random.NextFloat(ZMin, ZMax));
        }
    }
}