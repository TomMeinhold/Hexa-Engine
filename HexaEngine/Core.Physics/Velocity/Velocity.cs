using HexaEngine.Core.Physics.Interfaces;
using SharpDX;

namespace HexaEngine.Core.Physics
{
    public static class Velocity
    {
        public static void ProcessObject(IPhysicsObject physicsObject, PhysicsEngine engine)
        {
            var time = engine.ThreadTiming;

            // Velocity.
            Vector3 velocity = physicsObject.Velocity;
            velocity.X += physicsObject.Acceleration.X * (float)time.TotalSeconds;
            velocity.Y += physicsObject.Acceleration.Y * (float)time.TotalSeconds;
            velocity.Z += physicsObject.Acceleration.Z * (float)time.TotalSeconds;
            physicsObject.Velocity = velocity;

            // Rotation velocity.
            Vector3 rotationVelocity = physicsObject.RotationVelocity;
            rotationVelocity.X += physicsObject.RotationAcceleration.X * (float)time.TotalSeconds;
            rotationVelocity.Y += physicsObject.RotationAcceleration.Y * (float)time.TotalSeconds;
            rotationVelocity.Z += physicsObject.RotationAcceleration.Z * (float)time.TotalSeconds;
            physicsObject.RotationVelocity = rotationVelocity;

            // Update position.
            Vector3 position = physicsObject.Position;
            position.X += velocity.X * (float)time.TotalSeconds;
            position.Y += velocity.Y * (float)time.TotalSeconds;
            position.Z += velocity.Z * (float)time.TotalSeconds;
            physicsObject.SetPosition(position);

            // Update rotation.
            Vector3 rotation = physicsObject.Rotation;
            rotation.X = (rotation.X + (rotationVelocity.X * (float)time.TotalSeconds)) % 359;
            rotation.Y = (rotation.Y + (rotationVelocity.Y * (float)time.TotalSeconds)) % 359;
            rotation.Z = (rotation.Z + (rotationVelocity.Z * (float)time.TotalSeconds)) % 359;
            physicsObject.SetRotation(rotation);
        }
    }
}