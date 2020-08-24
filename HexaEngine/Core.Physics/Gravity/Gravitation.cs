using HexaEngine.Core.Extensions;
using HexaEngine.Core.Physics.Collision;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays;
using SharpDX;
using System;

namespace HexaEngine.Core.Physics.Gravity
{
    public static class Gravitation
    {
        public static void Process(IPhysicsObject a, IPhysicsObject b, PhysicsEngine engine)
        {
            var mulitplier = engine.ScalingMode switch
            {
                ScalingMode.Meters => 1,
                ScalingMode.Kilometers => 1000,
                ScalingMode.Gigameters => 1000000000,
                _ => 1,
            };

            if (b is IGravityField gravityField)
            {
                foreach (Direction4 direction4 in DirectionMethods.TraceDirection(a.BoundingBox.GetPositionCenter(), b.BoundingBox.GetPositionCenter()).SplitDirection())
                {
                    if (direction4 == gravityField.Direction)
                    {
                        Vector3 forceBefore1 = a.Force;
                        switch (direction4)
                        {
                            case Direction4.Up:

                                forceBefore1 += new Vector3(0, gravityField.GravitationalForce * -1, 0);
                                a.Force = forceBefore1;
                                break;

                            case Direction4.Right:

                                forceBefore1 += new Vector3(gravityField.GravitationalForce, 0, 0);
                                a.Force = forceBefore1;
                                break;

                            case Direction4.Down:

                                forceBefore1 += new Vector3(0, gravityField.GravitationalForce, 0);
                                a.Force = forceBefore1;
                                break;

                            case Direction4.Left:

                                forceBefore1 += new Vector3(gravityField.GravitationalForce * -1, 0, 0);
                                a.Force = forceBefore1;
                                break;
                        }
                    }
                }
            }

            if (!a.Static)
            {
                Vector3 aCenterOfMass = a.MassCenter;
                Vector3 bCenterOfMass = b.MassCenter;
                Vector3 center1 = a.Position + aCenterOfMass;
                Vector3 center2 = b.Position + bCenterOfMass;
                Vector3 pointer = center2 - center1;
                float distance = Vector3.Distance(center1, center2);
                Vector3 force = CalculateGravityForce(a.Mass / mulitplier, b.Mass / mulitplier, distance, pointer);

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

        public static Vector3 CalculateGravityForce(float mass1, float mass2, float distance, Vector3 pointer)
        {
            return Constants.Gravity * mass1 * mass2 / (float)Math.Pow(distance, 2) * pointer;
        }
    }
}