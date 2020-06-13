using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace HexaEngine.Core.Physics
{
    public static class Acceleration
    {
        public static void ProcessObject(IPhysicsObject physicsObject)
        {
            Vector3 Acceleration = new Vector3();
            if (physicsObject.Static)
            {
                // Acceleration is always zero.
                physicsObject.Acceleration = default;
            }
            else
            {
                // Calculates the acceleration. F = a * m
                Acceleration.X += physicsObject.Force.X / physicsObject.Mass;
                Acceleration.Y += physicsObject.Force.Y / physicsObject.Mass;

                physicsObject.Acceleration = Acceleration;

                physicsObject.Force = default;
            }
        }
    }
}