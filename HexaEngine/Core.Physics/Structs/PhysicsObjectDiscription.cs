using HexaEngine.Core.Physics.Interfaces;
using SharpDX;

namespace HexaEngine.Core.Physics.Structs
{
    public class PhysicsObjectDiscription
    {
        public bool Colliding { get; set; }

        public float Mass { get; set; }

        public bool Static { get; set; }

        public Vector3 Velocity { get; set; }

        public Vector3 RotationVelocity { get; set; }

        public float ForceAbsorbtion { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public void SetValues(IPhysicsObject physicsObject)
        {
            physicsObject.Colliding = Colliding;
            physicsObject.Mass = Mass;
            physicsObject.Static = Static;
            physicsObject.Velocity = Velocity;
            physicsObject.RotationVelocity = RotationVelocity;
            physicsObject.ForceAbsorbtion = ForceAbsorbtion;
            physicsObject.SetPosition(Position);
            physicsObject.SetRotation(Rotation);
            physicsObject.SetScale(Scale);
        }
    }
}