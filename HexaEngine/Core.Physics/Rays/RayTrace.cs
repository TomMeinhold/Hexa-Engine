using HexaEngine.Core.Extentions;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays.RayTraceModules;
using SharpDX;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Rays
{
    public static class RayTrace
    {
        public static void Process(IRayCasting casting, IPhysicsObject a, List<IPhysicsObject> physicsObjects)
        {
            Vector3 center = a.BoundingBox.GetPositionCenter();
            RayBuffer rayBuffer = new RayBuffer(casting.Rays);
            for (float i = casting.StartAngle; i <= casting.EndAngle;)
            {
                float x = (float)Math.Cos(Math.PI * i / 180) * casting.RayRange;
                float y = (float)Math.Sin(Math.PI * i / 180) * casting.RayRange;

                Ray ray = new Ray(center, new Vector3(center.X + x, center.Y + y, a.Position.Z));
                TraceRay(ref ray, a, ref rayBuffer, ref physicsObjects);

                rayBuffer.Add(ray);

                i += 1 / casting.RayDensity;
            }

            if (casting.Rays is null)
            {
                casting.Rays = rayBuffer.GetRays();
            }
            else
            {
                lock (casting.Rays)
                {
                    casting.Rays = rayBuffer.GetRays();
                }
            }
        }

        public static void RayCollision(ref Ray ray, BoundingBox boundingBox, IRayMirror rayMirror, ref RayBuffer rayBuffer, ref List<IPhysicsObject> physicsObjects)
        {
            if (ray.Position.Z == boundingBox.Minimum.Z)
            {
                bool collision = false;
                Direction direction = DirectionMethods.TraceDirection(ray.Position, boundingBox.Center);
                foreach (Direction4 direction4 in direction.SplitDirection())
                {
                    switch (direction4)
                    {
                        case Direction4.Top:
                            (bool interTC, Vector3 vectorTC) = RayMath.LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interTC)
                            {
                                ray.Direction.X = vectorTC.X;
                                ray.Direction.Y = vectorTC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Right:
                            (bool interRC, Vector3 vectorRC) = RayMath.LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interRC)
                            {
                                ray.Direction.X = vectorRC.X;
                                ray.Direction.Y = vectorRC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Bottom:
                            (bool interBC, Vector3 vectorBC) = RayMath.LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interBC)
                            {
                                ray.Direction.X = vectorBC.X;
                                ray.Direction.Y = vectorBC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Left:
                            (bool interLC, Vector3 vectorLC) = RayMath.LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interLC)
                            {
                                ray.Direction.X = vectorLC.X;
                                ray.Direction.Y = vectorLC.Y;
                                collision = true;
                            }
                            break;
                    }

                    if (collision)
                    {
                        MirrorRay(ref ray, ref rayBuffer, rayMirror, direction4, ref physicsObjects);
                    }
                }
            }
        }

        public static void MirrorRay(ref Ray ray, ref RayBuffer rayBuffer, IRayMirror rayMirror, Direction4 direction4, ref List<IPhysicsObject> physicsObjects)
        {
            if (rayMirror is null)
            {
                return;
            }

            switch (direction4)
            {
                case Direction4.Top:
                    if (rayMirror.Top)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay(ref ray1, rayMirror, ref rayBuffer, ref physicsObjects);
                        rayBuffer.Add(ray1);
                    }

                    break;

                case Direction4.Right:
                    if (rayMirror.Right)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength, transformedPosition.Y / factor * -1, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay(ref ray1, rayMirror, ref rayBuffer, ref physicsObjects);
                        rayBuffer.Add(ray1);
                    }

                    break;

                case Direction4.Bottom:
                    if (rayMirror.Bottom)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.Y / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(transformedPosition.X / factor * -1, rayMirror.ReflectionStrength, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay(ref ray1, rayMirror, ref rayBuffer, ref physicsObjects);
                        rayBuffer.Add(ray1);
                    }
                    break;

                case Direction4.Left:
                    if (rayMirror.Left)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, ray.Direction.Y, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay(ref ray1, rayMirror, ref rayBuffer, ref physicsObjects);
                        rayBuffer.Add(ray1);
                    }

                    break;
            }
        }

        public static Vector3 GetPositionCenter(this BoundingBox bounding) => new Vector3(bounding.Minimum.X + Math.Abs(bounding.Width).Half(), bounding.Minimum.Y + Math.Abs(bounding.Height).Half(), bounding.Minimum.Z);

        public static void TraceRay(ref Ray ray, object caster, ref RayBuffer rayBuffer, ref List<IPhysicsObject> physicsObjects)
        {
            foreach (IPhysicsObject physicsObject in physicsObjects)
            {
                if (physicsObject == caster)
                {
                    continue;
                }
                else if (physicsObject is IRayLens lens)
                {
                    Lens.Process(ref ray, lens.BoundingBox, ref rayBuffer, lens);
                }
                else
                {
                    RayCollision(ref ray, physicsObject.BoundingBox, physicsObject as IRayMirror, ref rayBuffer, ref physicsObjects);
                }
            }
        }
    }
}