// <copyright file="ColorExtentions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extentions
{
    using System;

    public static class ColorExtentions
    {
        public static SharpDX.Color Convert(this System.Drawing.Color c) => SharpDX.Color.FromRgba(BitConverter.ToInt32(new byte[] { c.R, c.G, c.B, c.A }, 0));

        public static System.Drawing.Color Convert(this SharpDX.Color c) => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);

        public static System.Drawing.Color Convert(this SharpDX.Mathematics.Interop.RawColor4 c) => System.Drawing.Color.FromArgb((int)(c.A * 255), (int)(c.R * 255), (int)(c.G * 255), (int)(c.B * 255));

        public static SharpDX.Color Blend(this SharpDX.Color color, SharpDX.Color backColor)
        {
            int r = (color.R + backColor.R) / 2;
            int g = (color.G + backColor.G) / 2;
            int b = (color.B + backColor.B) / 2;
            int a = (color.A + backColor.A) / 2;
            return SharpDX.Color.FromRgba(BitConverter.ToInt32(new byte[] { (byte)r, (byte)b, (byte)g, (byte)a }, 1));
        }

        public static SharpDX.Mathematics.Interop.RawColor4 Blend(this SharpDX.Mathematics.Interop.RawColor4 color, SharpDX.Mathematics.Interop.RawColor4 backColor)
        {
            float r = (color.R + backColor.R) / 2;
            float g = (color.G + backColor.G) / 2;
            float b = (color.B + backColor.B) / 2;
            float a = (color.A + backColor.A) / 2;
            return new SharpDX.Mathematics.Interop.RawColor4(r, b, g, a);
        }

        public static System.Drawing.Color Blend(this System.Drawing.Color color, System.Drawing.Color backColor)
        {
            int r = (color.R + backColor.R) / 2;
            int g = (color.G + backColor.G) / 2;
            int b = (color.B + backColor.B) / 2;
            int a = (color.A + backColor.A) / 2;
            return System.Drawing.Color.FromArgb(a, r, b, g);
        }
    }
}