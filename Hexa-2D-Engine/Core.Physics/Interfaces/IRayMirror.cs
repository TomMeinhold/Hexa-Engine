using SharpDX;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IRayMirror : IPhysicsObject
    {
        public float ReflectionStrength { get; set; }

        public Color ReflectionColor { get; set; }
    }
}