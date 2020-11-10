using SharpDX;
using System;

namespace HexaEngine.Core.Mathmatics
{
    public static class MatrixCalculation
    {
        public static Matrix3x2 RotateAt(float angle, Vector3 center)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            float dx = (center.X * (1.0F - cos)) + (center.Y * sin);
            float dy = (center.Y * (1.0F - cos)) - (center.X * sin);

            return new Matrix3x2
            {
                M11 = cos,
                M12 = sin,
                M21 = -sin,
                M22 = cos,
                M31 = dx,
                M32 = dy
            }; ;
        }
    }
}