using HexaEngine.Core.Mathmatics.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexaEngine.Core.Mathmatics
{
    public class SinBuffer
    {
        private readonly MathBuffer<float, float> Buffer;

        public SinBuffer()
        {
            Buffer = new MathBuffer<float, float>(Sin);
        }

        private static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        public float GetValue(float t1) => Buffer.GetValue(t1);
    }
}