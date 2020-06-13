using HexaEngine.Core.Extentions;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using System.Threading;

namespace HexaEngine.Core.Physics.Gravity
{
    public static class Gravitation
    {
        public static void ProcessObject(IPhysicsObject a, IPhysicsObject b)
        {
            if (!a.Static)
            {
                Vector3 aCenterOfMass = a.MassCenter;
                Vector3 bCenterOfMass = b.MassCenter;
                Vector3 center1 = a.Position + aCenterOfMass;
                Vector3 center2 = b.Position + bCenterOfMass;
                Vector3 pointer = center2 - center1;
                float distance = Vector3.Distance(center1, center2);
                Vector3 force = CalculateGravityForce(a.Mass, b.Mass, distance, pointer);

                if (a.Sides is null)
                {
                    a.Sides = new BlockedDirection();
                }

                Vector3 forceBefore1 = a.Force;
                forceBefore1.Y += force.Y;
                forceBefore1.X += force.X;
                a.Force = forceBefore1;
            }
        }

        public static Vector3 ProcessGravitation(IPhysicsObject a, IPhysicsObject b)
        {
            if (!a.Static)
            {
                BoundingBox aabb1 = a.BoundingBox;
                BoundingBox aabb2 = b.BoundingBox;

                Vector3 center1 = aabb1.Center;
                Vector3 center2 = aabb2.Center;
                Vector3 pointer = center2 - center1;
                float distance = Vector3.Distance(center1, center2);
                Vector3 force = CalculateGravityForce(a.Mass, b.Mass, distance, pointer);

                if (a.Sides is null)
                {
                    a.Sides = new BlockedDirection();
                }

                Vector3 newForce = a.Force;
                newForce.Y += force.Y;
                newForce.X += force.X;
                newForce.Z += force.Z;
                return newForce;
            }

            return a.Force;
        }

        public static Vector3 CalculateGravityForce(float mass1, float mass2, float distance, Vector3 pointer)
        {
            return Constants.Gravity * mass1 * mass2 / (float)Math.Pow(distance, 2) * pointer;
        }
    }
}