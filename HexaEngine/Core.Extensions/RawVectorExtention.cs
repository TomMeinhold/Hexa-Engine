// <copyright file="RawVectorExtention.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extensions
{
    using System;
    using HexaEngine.Core;
    using SharpDX;
    using SharpDX.Mathematics.Interop;

    public static class RawVectorExtention
    {
        public static bool IsSameVector(this RawVector2 vector1, RawVector2 vector2)
        {
            return (vector1.Y == vector2.Y) && (vector1.X == vector2.X);
        }

        public static RawVector2 Normalize(this RawVector2 vector1)
        {
            RawVector2 outp = vector1;
            if (float.IsNaN(outp.X) | float.IsInfinity(outp.X))
            {
                if (outp.X > 0)
                {
                    outp.X = Engine.Current.Settings.Width;
                }
            }

            return outp;
        }

        public static RawVector2 Downgrade(this RawVector3 vec) => new RawVector2(vec.X, vec.Y);

        public static Vector2 Downgrade(this Vector3 vec) => new Vector2(vec.X, vec.Y);

        public static Vector3 Upgrade(this Vector2 vec) => new Vector3(vec.X, vec.Y, 0);

        public static Vector2 SizeToVector(this Size2F size) => new Vector2(size.Width, size.Height);

        public static RawVector2 Invert(this RawVector2 vec) => new RawVector2(vec.X * -1, vec.Y * -1);

        public static bool IsDefault(this RawVector2 vec) => vec.X == 0 && vec.Y == 0;

        public static RawVector2 UpgradeToVector2(this float x, float y = 0) => new RawVector2(x, y);

        public static RawVector3 UpgradeToVector3(this RawVector2 vec, float z = 0) => new RawVector3(vec.X, vec.Y, z);

        public static bool InRadius(this RawVector2 vec, RawVector2 otherVec, float radius)
        {
            return vec.X <= otherVec.X + radius && vec.Y <= otherVec.Y + radius && vec.Y >= otherVec.Y - radius && vec.Y >= otherVec.Y - radius;
        }

        public static Vector3 Invert(this Vector3 vec) => new Vector3(vec.X * -1, vec.Y * -1, vec.Z * -1);

        public static RawVector3 Invert(this RawVector3 vec) => new RawVector3(vec.X * -1, vec.Y * -1, vec.Z * -1);

        public static Vector3 InvertX(this Vector3 vec) => new Vector3(vec.X * -1, vec.Y, vec.Z);

        public static Vector3 InvertY(this Vector3 vec) => new Vector3(vec.X, vec.Y * -1, vec.Z);

        public static Vector3 InvertZ(this Vector3 vec) => new Vector3(vec.X, vec.Y, vec.Z * -1);

        public static System.Drawing.PointF ToPointF(this Vector3 vec) => new System.Drawing.PointF(vec.X, vec.Y);
    }
}