using SharpDX;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayMirror : IPhysicsObject
    {
        public float ReflectionStrength { get; set; }

        public Color ReflectionColor { get; set; }

        public bool Top { get; set; }

        public bool Right { get; set; }

        public bool Left { get; set; }

        public bool Bottom { get; set; }
    }
}