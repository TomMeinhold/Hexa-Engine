using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using SharpDX;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IPhysicsObject : IBaseObject
    {
        public BoundingBox BoundingBox { get; set; }

        public float Mass { get; set; }

        public bool Static { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 Force { get; set; }

        public float ForceAbsorbtion { get; set; }

        public BlockedDirection Sides { get; set; }

        public void SetPosition(Vector3 vector3);
    }
}