using System.Numerics;
using System.Runtime.InteropServices;

namespace HexaFramework.Resources.Buffers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferLightType
    {
        public Vector4 AmbientColor;
        public Vector4 Color;
        public Vector3 Direction;
        public float SpecularPower;
        public Vector4 SpecularColor;
    }
}