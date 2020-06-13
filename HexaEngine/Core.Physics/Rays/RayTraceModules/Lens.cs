using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Rays.RayTraceModules
{
    public static class Lens
    {
        public static void Process(ref Ray ray, BoundingBox boundingBox, ref RayBuffer rayBuffer, IRayLens lens)
        {
            // W = Top X = Right, Y = Bottom, Z = Left.
            if (ray.Position.Z == boundingBox.Minimum.Z)
            {
                Direction direction = DirectionMethods.TraceDirection(ray.Position, boundingBox.Center);
                foreach (Direction4 direction4 in direction.SplitDirection())
                {
                    switch (direction4)
                    {
                        case Direction4.Top:
                            (bool interTC, Vector3 vectorTC) = RayMath.LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interTC)
                            {
                                Ray ray1 = new Ray(ray.Position, new Vector3(ray.Direction.X * lens.Multiplier, ray.Direction.Y * lens.Multiplier, ray.Direction.Z));
                                ray.Direction.X = vectorTC.X;
                                ray.Direction.Y = vectorTC.Y;
                                ray1.Position = ray.Direction;
                                rayBuffer.Add(ray1);
                            }
                            break;

                        case Direction4.Right:
                            (bool interRC, Vector3 vectorRC) = RayMath.LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interRC)
                            {
                                Ray ray1 = new Ray(ray.Position, new Vector3(ray.Direction.X * lens.Multiplier, ray.Direction.Y * lens.Multiplier, ray.Direction.Z));
                                ray.Direction.X = vectorRC.X;
                                ray.Direction.Y = vectorRC.Y;
                                ray1.Position = ray.Direction;
                                ray1.Position.Y *= -1;
                                ray1.Direction.Y *= -1;
                                ray1.Position.Y += Math.Abs(lens.BoundingBox.Height);
                                ray1.Direction.Y += Math.Abs(lens.BoundingBox.Height);
                                rayBuffer.Add(ray1);
                            }
                            break;

                        case Direction4.Bottom:
                            (bool interBC, Vector3 vectorBC) = RayMath.LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interBC)
                            {
                                Ray ray1 = new Ray(ray.Position, ray.Direction * lens.Multiplier);
                                ray.Direction.X = vectorBC.X;
                                ray.Direction.Y = vectorBC.Y;
                                ray1.Position = ray.Direction;
                                rayBuffer.Add(ray1);
                            }
                            break;

                        case Direction4.Left:
                            (bool interLC, Vector3 vectorLC) = RayMath.LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interLC)
                            {
                                Ray ray1 = new Ray(ray.Position, ray.Direction * lens.Multiplier);
                                ray.Direction.X = vectorLC.X;
                                ray.Direction.Y = vectorLC.Y;
                                ray1.Position = ray.Direction;
                                rayBuffer.Add(ray1);
                            }
                            break;
                    }
                }
            }
        }
    }
}