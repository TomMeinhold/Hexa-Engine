using System.Numerics;
using System.Runtime.InteropServices;

namespace HexaFramework.Resources.Buffers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferMatrixType
    {
        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Projection;
        public Vector4 CameraPosition;
    }
}