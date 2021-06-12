using System.Numerics;
using System.Runtime.InteropServices;

namespace HexaFramework.Resources
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector4 Position;
        public Vector2 Texture;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector3 Binormal;

        public Vertex(Vector4 position, Vector2 texture, Vector3 normal)
        {
            Position = position;
            Texture = texture;
            Normal = normal;
            Tangent = Vector3.Zero;
            Binormal = Vector3.Zero;
        }
    }
}