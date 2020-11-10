using HexaEngine.Core.Mathmatics.Buffer;
using System;

namespace HexaEngine.Core.Mathmatics
{
    public class CosBuffer
    {
        private readonly MathBuffer<float, float> Buffer;

        public CosBuffer()
        {
            Buffer = new MathBuffer<float, float>(Cos);
        }

        private static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }

        public float GetValue(float t1) => Buffer.GetValue(t1);
    }
}