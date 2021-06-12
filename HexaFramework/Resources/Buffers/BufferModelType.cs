using System.Numerics;
using System.Runtime.InteropServices;

namespace HexaFramework.Resources.Buffers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferModelType
    {
        public Matrix4x4 ModelTransform;
    }
}