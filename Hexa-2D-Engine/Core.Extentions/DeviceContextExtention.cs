﻿// <copyright file="DeviceContextExtention.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extentions
{
    using System.Collections.Generic;
    using SharpDX.Direct2D1;
    using SharpDX.Mathematics.Interop;

    public static class DeviceContextExtention
    {
        public static bool IsVisible(this DeviceContext target, RawRectangleF vector2)
        {
            if (target == null)
            {
                return false;
            }

            if ((int)vector2.Bottom < 0)
            {
                return false;
            }

            if ((int)vector2.Left < 0)
            {
                return false;
            }

            if ((int)vector2.Right < 0)
            {
                return false;
            }

            if ((int)vector2.Top < 0)
            {
                return false;
            }

            if ((int)vector2.Right > target.Size.Width)
            {
                return false;
            }

            if ((int)vector2.Top > target.Size.Height)
            {
                return false;
            }

            if ((int)vector2.Left > target.Size.Width)
            {
                return false;
            }

            if ((int)vector2.Bottom > target.Size.Height)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check a Vector with matrix if it is visible on context.
        /// </summary>
        /// <param name="target">D2D1 Render Context.</param>
        /// <param name="vector2">Target Vector.</param>
        /// <param name="matrix">Transformation matrix.</param>
        /// <returns>Is Visible.</returns>
        public static bool IsVisible(this DeviceContext target, RawVector2 vector2, RawMatrix3x2 matrix)
        {
            if (target == null)
            {
                return false;
            }

            if (vector2.X + matrix.M11 < 0)
            {
                if (vector2.X < 0)
                {
                    return false;
                }
            }

            if (vector2.Y + matrix.M21 < 0)
            {
                if (vector2.Y < 0)
                {
                    return false;
                }
            }

            if (vector2.X + matrix.M11 > target.Size.Width)
            {
                if (vector2.X > target.Size.Width)
                {
                    return false;
                }
            }

            if (vector2.Y + matrix.M22 > target.Size.Height)
            {
                if (vector2.Y > target.Size.Height)
                {
                    return false;
                }
            }

            return true;
        }
    }
}