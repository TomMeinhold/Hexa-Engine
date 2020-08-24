using System;

namespace HexaEngine.Core.Common
{
    public struct Color
    {
        public static SharpDX.Color FromArgb(int a = 0, int r = 0, int g = 0, int b = 0)
        {
            return SharpDX.Color.FromRgba(BitConverter.ToInt32(new byte[] { (byte)r, (byte)b, (byte)g, (byte)a }, 1));
        }

        public static SharpDX.Color FromRGBA(int r = 0, int g = 0, int b = 0, int a = 0)
        {
            return SharpDX.Color.FromRgba(BitConverter.ToInt32(new byte[] { (byte)r, (byte)b, (byte)g, (byte)a }, 1));
        }
    }
}