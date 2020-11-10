using HexaEngine.Core.Objects.Interfaces;
using HexaEngine.Core.Physics.Collision;
using SharpDX;
using System;

namespace HexaEngine.Core.Physics.Interfaces
{
    public interface IPhysicsObject : IBaseObject
    {
        public bool Colliding { get; set; }

        public BoundingBox BoundingBox { get; set; }

        public BoundingBox BoundingBoxBefore { get; set; }

        public float Mass { get; set; }

        public Vector3 MassCenter { get; set; }

        public bool Static { get; set; }

        public Vector3 PositionBefore { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public Vector3 Acceleration { get; set; }

        public Vector3 RotationAcceleration { get; set; }

        public Vector3 Force { get; set; }

        public Matrix3x2 ObjectViewMatrix { get; set; }

        public float ForceAbsorbtion { get; set; }

        public BlockedDirection Sides { get; set; }

        public event EventHandler<OnCollisionEventArgs> OnCollision;

        public void CallOnCollision(OnCollisionEventArgs onCollisionEventArgs);

        public void SetPosition(Vector3 vector3);

        public void SetRotation(Vector3 vector3);

        public void SetScale(Vector3 vector3);
    }
}