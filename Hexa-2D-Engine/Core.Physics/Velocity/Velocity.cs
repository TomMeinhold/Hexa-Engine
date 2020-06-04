using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;

namespace HexaEngine.Core.Physics
{
    public static class Velocity
    {
        public static void ProcessObject(IPhysicsObject physicsObject, TimeSpan time)
        {
            Vector3 velocity = physicsObject.Velocity;
            velocity.X += physicsObject.Acceleration.X * (float)time.TotalMilliseconds / 1000;
            velocity.Y += physicsObject.Acceleration.Y * (float)time.TotalMilliseconds / 1000;
            velocity.Z += physicsObject.Acceleration.Z * (float)time.TotalMilliseconds / 1000;
            physicsObject.Velocity = velocity;
            Vector3 position = physicsObject.BoundingBox.Minimum;
            position.X += velocity.X;
            position.Y += velocity.Y;
            position.Z += velocity.Z;
            physicsObject.SetPosition(position);
        }
    }
}