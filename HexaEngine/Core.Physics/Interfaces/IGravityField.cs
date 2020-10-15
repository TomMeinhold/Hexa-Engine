using HexaEngine.Core.Physics.Rays;
using SharpDX;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IGravityField
    {
        public Vector3 Mass { get; }
    }
}