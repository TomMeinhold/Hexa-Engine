using HexaEngine.Core.Physics.Rays;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IGravityField
    {
        public Direction4 Direction { get; }

        public float GravitationalForce { get; }
    }
}