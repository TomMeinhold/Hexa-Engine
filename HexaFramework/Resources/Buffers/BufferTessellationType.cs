using System.Numerics;
using System.Runtime.InteropServices;

namespace HexaFramework.Resources.Buffers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferTessellationType
    {
        public float TessellationAmount;
        public Vector3 Padding;
    }
}