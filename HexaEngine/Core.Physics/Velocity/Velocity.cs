using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;

namespace HexaEngine.Core.Physics
{
    public static class Velocity
    {
        public static void ProcessObject(IPhysicsObject physicsObject, TimeSpan time)
        {
            // Velocity.
            Vector3 velocity = physicsObject.Velocity;
            velocity.X += physicsObject.Acceleration.X * (float)time.TotalMilliseconds / 1000;
            velocity.Y += physicsObject.Acceleration.Y * (float)time.TotalMilliseconds / 1000;
            velocity.Z += physicsObject.Acceleration.Z * (float)time.TotalMilliseconds / 1000;
            physicsObject.Velocity = velocity;

            // Rotation velocity.
            Vector3 rotationVelocity = physicsObject.RotationVelocity;
            rotationVelocity.X += physicsObject.RotationAcceleration.X * (float)time.TotalMilliseconds / 1000;
            rotationVelocity.Y += physicsObject.RotationAcceleration.Y * (float)time.TotalMilliseconds / 1000;
            rotationVelocity.Z += physicsObject.RotationAcceleration.Z * (float)time.TotalMilliseconds / 1000;
            physicsObject.RotationVelocity = rotationVelocity;

            // Update position.
            Vector3 position = physicsObject.BoundingBox.Minimum;
            position.X += velocity.X * (float)time.TotalMilliseconds / 1000;
            position.Y += velocity.Y * (float)time.TotalMilliseconds / 1000;
            position.Z += velocity.Z * (float)time.TotalMilliseconds / 1000;
            physicsObject.SetPosition(position);

            // Update rotation.
            Vector3 rotation = physicsObject.Rotation;
            rotation.X = (rotation.X + rotationVelocity.X * (float)time.TotalMilliseconds / 1000) % 359;
            rotation.Y = (rotation.Y + rotationVelocity.Y * (float)time.TotalMilliseconds / 1000) % 359;
            rotation.Z = (rotation.Z + rotationVelocity.Z * (float)time.TotalMilliseconds / 1000) % 359;
            physicsObject.SetRotation(rotation);
        }
    }
}