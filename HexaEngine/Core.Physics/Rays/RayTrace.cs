using HexaEngine.Core.Extensions;
using HexaEngine.Core.Objects.Components;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;

namespace HexaEngine.Core.Physics.Rays
{
    public static class RayTrace
    {
        public static void Process(IRayCasting casting, IPhysicsObject a, List<IPhysicsObject> physicsObjects)
        {
            casting.RayCastingModule.ResetPointer();
            var descCast = casting.RayCastDiscription;
            Vector3 center = a.BoundingBox.GetPositionCenter();
            for (float i = descCast.StartAngle; i <= descCast.EndAngle;)
            {
                float x = (float)Math.Cos(Math.PI * i / 180) * descCast.RayRange;
                float y = (float)Math.Sin(Math.PI * i / 180) * descCast.RayRange;

                Vector3 pos = center;
                Vector3 dir = new Vector3(center.X + x, center.Y + y, a.Position.Z);
                TraceRay(ref pos, ref dir, a, casting.RayCastingModule, ref physicsObjects);
                casting.RayCastingModule.AddRay(pos, dir);

                i += 1 / descCast.RayDensity;
            }
        }

        public static void RayCollision(ref Vector3 pos, ref Vector3 dir, BoundingBox boundingBox, IRayMirror rayMirror, ref RayCastingModule module, ref List<IPhysicsObject> physicsObjects)
        {
            if (pos.Z == boundingBox.Minimum.Z)
            {
                bool collision = false;
                Direction direction = DirectionMethods.TraceDirection(pos, boundingBox.Center);
                foreach (Direction4 direction4 in direction.SplitDirection())
                {
                    switch (direction4)
                    {
                        case Direction4.Up:
                            (bool interTC, Vector3 vectorTC) = RayMath.LineSegementsIntersect(pos, dir, boundingBox.Maximum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interTC)
                            {
                                dir.X = vectorTC.X;
                                dir.Y = vectorTC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Right:
                            (bool interRC, Vector3 vectorRC) = RayMath.LineSegementsIntersect(pos, dir, boundingBox.Maximum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interRC)
                            {
                                dir.X = vectorRC.X;
                                dir.Y = vectorRC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Down:
                            (bool interBC, Vector3 vectorBC) = RayMath.LineSegementsIntersect(pos, dir, boundingBox.Minimum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interBC)
                            {
                                dir.X = vectorBC.X;
                                dir.Y = vectorBC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Left:
                            (bool interLC, Vector3 vectorLC) = RayMath.LineSegementsIntersect(pos, dir, boundingBox.Minimum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interLC)
                            {
                                dir.X = vectorLC.X;
                                dir.Y = vectorLC.Y;
                                collision = true;
                            }
                            break;
                    }

                    if (collision)
                    {
                        MirrorRay(ref pos, ref dir, ref module, rayMirror, direction4, ref physicsObjects);
                    }
                }
            }
        }

        public static void MirrorRay(ref Vector3 pos, ref Vector3 dir, ref RayCastingModule module, IRayMirror rayMirror, Direction4 direction4, ref List<IPhysicsObject> physicsObjects)
        {
            if (rayMirror is null)
            {
                return;
            }

            switch (direction4)
            {
                case Direction4.Up:
                    if (rayMirror.Top)
                    {
                        Vector3 direction = new Vector3(dir.X, dir.Y, dir.Z);
                        Vector3 position = new Vector3(pos.X, pos.Y, pos.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        TraceRay(ref direction, ref reflectedDirection, rayMirror, module, ref physicsObjects);
                        module.AddRay(direction, reflectedDirection);
                    }

                    break;

                case Direction4.Right:
                    if (rayMirror.Right)
                    {
                        Vector3 direction = new Vector3(dir.X, dir.Y, dir.Z);
                        Vector3 position = new Vector3(pos.X, pos.Y, pos.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength, transformedPosition.Y / factor * -1, transformedPosition.Z), Matrix.Translation(direction));
                        TraceRay(ref direction, ref reflectedDirection, rayMirror, module, ref physicsObjects);
                        module.AddRay(direction, reflectedDirection);
                    }

                    break;

                case Direction4.Down:
                    if (rayMirror.Bottom)
                    {
                        Vector3 direction = new Vector3(dir.X, dir.Y, dir.Z);
                        Vector3 position = new Vector3(pos.X, pos.Y, pos.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.Y / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(transformedPosition.X / factor * -1, rayMirror.ReflectionStrength, transformedPosition.Z), Matrix.Translation(direction));
                        TraceRay(ref direction, ref reflectedDirection, rayMirror, module, ref physicsObjects);
                        module.AddRay(direction, reflectedDirection);
                    }
                    break;

                case Direction4.Left:
                    if (rayMirror.Left)
                    {
                        Vector3 direction = new Vector3(dir.X, dir.Y, dir.Z);
                        Vector3 position = new Vector3(pos.X, pos.Y, pos.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        TraceRay(ref direction, ref reflectedDirection, rayMirror, module, ref physicsObjects);
                        module.AddRay(direction, reflectedDirection);
                    }

                    break;
            }
        }

        public static Vector3 GetPositionCenter(this BoundingBox bounding) => new Vector3(bounding.Minimum.X + Math.Abs(bounding.Width).Half(), bounding.Minimum.Y + Math.Abs(bounding.Height).Half(), bounding.Minimum.Z);

        public static void TraceRay(ref Vector3 pos, ref Vector3 dir, object caster, RayCastingModule module, ref List<IPhysicsObject> physicsObjects)
        {
            foreach (IPhysicsObject physicsObject in physicsObjects)
            {
                if (physicsObject == caster)
                {
                    continue;
                }
                else if (physicsObject is IRayLens lens)
                {
                    //Lens.Process(ref pos, ref dir, lens.BoundingBox, ref rayBuffer, lens);
                }
                else
                {
                    RayCollision(ref pos, ref dir, physicsObject.BoundingBox, physicsObject as IRayMirror, ref module, ref physicsObjects);
                }
            }
        }
    }
}