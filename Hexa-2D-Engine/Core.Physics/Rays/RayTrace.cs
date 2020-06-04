using HexaEngine.Core.Extentions;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexaEngine.Core.Physics.Rays
{
    public static class RayTrace
    {
        public const int Depth = 10;

        public static void Process(IRayCasting casting, IPhysicsObject a, List<IPhysicsObject> physicsObjects)
        {
            Vector3 center = a.BoundingBox.GetPositionCenter();
            List<Ray> rays = new List<Ray>();
            for (float i = casting.StartAngle; i <= casting.EndAngle;)
            {
                float x = (float)Math.Cos(Math.PI * i / 180) * casting.RayRange;
                float y = (float)Math.Sin(Math.PI * i / 180) * casting.RayRange;

                Ray ray = new Ray(center, new Vector3(center.X + x, center.Y + y, a.Position.Z));
                TraceRay(ref ray, a, ref rays, ref physicsObjects);

                ray.Direction.Y -= Math.Abs(a.BoundingBox.Height);
                ray.Position.Y -= Math.Abs(a.BoundingBox.Height);
                ray.Direction.Y *= -1;
                ray.Position.Y *= -1;

                rays.Add(ray);

                i += 1 / casting.RayDensity;
            }

            casting.Rays = rays;
        }

        public static void RayCollision(ref Ray ray, BoundingBox boundingBox, IRayMirror rayMirror, ref List<Ray> rays, ref List<IPhysicsObject> physicsObjects)
        {
            // W = Top X = Right, Y = Bottom, Z = Left.
            if (ray.Position.Z == boundingBox.Minimum.Z)
            {
                bool collision = false;
                Direction direction = DirectionMethods.TraceDirection(ray.Position, boundingBox.Center);
                foreach (Direction4 direction4 in direction.SplitDirection())
                {
                    switch (direction4)
                    {
                        case Direction4.Top:
                            (bool interTC, Vector vectorTC) = LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
                            if (interTC)
                            {
                                ray.Direction.X = vectorTC.X;
                                ray.Direction.Y = vectorTC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Right:
                            (bool interRC, Vector vectorRC) = LineSegementsIntersect(ray, boundingBox.Maximum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interRC)
                            {
                                ray.Direction.X = vectorRC.X;
                                ray.Direction.Y = vectorRC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Bottom:
                            (bool interBC, Vector vectorBC) = LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z));
                            if (interBC)
                            {
                                ray.Direction.X = vectorBC.X;
                                ray.Direction.Y = vectorBC.Y;
                                collision = true;
                            }
                            break;

                        case Direction4.Left:
                            (bool interLC, Vector vectorLC) = LineSegementsIntersect(ray, boundingBox.Minimum, new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z));
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
                        MirrorRay(ref ray, ref rays, boundingBox, rayMirror, direction4, ref physicsObjects);
                    }
                }
            }
        }

        public static void MirrorRay(ref Ray ray, ref List<Ray> rays, BoundingBox boundingBox, IRayMirror rayMirror, Direction4 direction4, ref List<IPhysicsObject> physicsObjects)
        {
            if (rayMirror is null)
            {
                return;
            }

            switch (direction4)
            {
                case Direction4.Top:
                    if (true)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, (ray.Direction.Y - Math.Abs(boundingBox.Height)) * -1, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, (ray.Position.Y - Math.Abs(boundingBox.Height)) * -1, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay2(ref ray1, rayMirror, ref rays, ref physicsObjects);
                        rays.Add(ray1);
                    }

                    break;

                case Direction4.Right:
                    if (true)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, (ray.Direction.Y - Math.Abs(boundingBox.Height)) * -1, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, (ray.Position.Y - Math.Abs(boundingBox.Height)) * -1, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength, transformedPosition.Y / factor * -1, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay2(ref ray1, rayMirror, ref rays, ref physicsObjects);
                        rays.Add(ray1);
                    }

                    break;

                case Direction4.Bottom:
                    if (true)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, (ray.Direction.Y - Math.Abs(boundingBox.Height)) * -1, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, (ray.Position.Y - Math.Abs(boundingBox.Height)) * -1, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.Y / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(transformedPosition.X / factor * -1, rayMirror.ReflectionStrength, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay2(ref ray1, rayMirror, ref rays, ref physicsObjects);
                        rays.Add(ray1);
                    }
                    break;

                case Direction4.Left:
                    if (true)
                    {
                        Vector3 direction = new Vector3(ray.Direction.X, (ray.Direction.Y - Math.Abs(boundingBox.Height)) * -1, ray.Direction.Z);
                        Vector3 position = new Vector3(ray.Position.X, (ray.Position.Y - Math.Abs(boundingBox.Height)) * -1, ray.Position.Z);
                        Vector3 transformedPosition = Vector3.TransformCoordinate(position, Matrix.Translation(direction * -1));
                        float factor = transformedPosition.X / rayMirror.ReflectionStrength;
                        Vector3 reflectedDirection = Vector3.TransformCoordinate(new Vector3(rayMirror.ReflectionStrength * -1, transformedPosition.Y / factor, transformedPosition.Z), Matrix.Translation(direction));
                        Ray ray1 = new Ray(direction, reflectedDirection);
                        TraceRay2(ref ray1, rayMirror, ref rays, ref physicsObjects);
                        rays.Add(ray1);
                    }

                    break;
            }
        }

        private static (bool, Vector) LineSegementsIntersect(Ray ray, Vector3 vector1, Vector3 vector2)
        {
            return (LineSegementsIntersect(new Vector(ray.Position.X, ray.Position.Y), new Vector(ray.Direction.X, ray.Direction.Y), new Vector(vector1.X, vector1.Y), new Vector(vector2.X, vector2.Y), out Vector vector), vector);
        }

        private const double Epsilon = 1e-10;

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static bool LineSegementsIntersect(Vector p, Vector p2, Vector q, Vector q2, out Vector intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Vector();

            var r = p2 - p;
            var s = q2 - q;
            float rxs = r.Cross(s);
            float qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either 0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s then the two
                // lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                {
                    if ((0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s))
                    {
                        return true;
                    }
                }

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s then the two
                // lines are collinear but disjoint. No need to implement this expression, as it
                // follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
            {
                return false;
            }

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)
            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1 the two line segments meet at the
            // point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }

        public static Vector3 GetPositionCenter(this BoundingBox bounding) => new Vector3(bounding.Minimum.X + Math.Abs(bounding.Width).Half(), bounding.Minimum.Y + Math.Abs(bounding.Height).Half(), bounding.Minimum.Z);

        public static void TraceRay(ref Ray ray, object caster, ref List<Ray> rays, ref List<IPhysicsObject> physicsObjects)
        {
            foreach (IPhysicsObject physicsObject in physicsObjects)
            {
                if (physicsObject == caster)
                {
                    continue;
                }
                else
                {
                    RayCollision(ref ray, physicsObject.BoundingBox, physicsObject as IRayMirror, ref rays, ref physicsObjects);
                }
            }
        }

        public static void TraceRay2(ref Ray ray, IPhysicsObject caster, ref List<Ray> rays, ref List<IPhysicsObject> physicsObjects)
        {
            physicsObjects = physicsObjects.OrderBy(x => Vector3.Distance(caster.Position, x.Position)).ToList();
            foreach (IPhysicsObject physicsObject in physicsObjects)
            {
                if (physicsObject == caster)
                {
                    continue;
                }
                else
                {
                    var min = physicsObject.BoundingBox.Minimum;
                    min.Y *= -1;
                    min.Y += Math.Abs(physicsObject.BoundingBox.Height);
                    var max = physicsObject.BoundingBox.Maximum;
                    max.Y *= -1;
                    max.Y += Math.Abs(physicsObject.BoundingBox.Height);
                    var bound = new BoundingBox(min, max);
                    RayCollision(ref ray, bound, physicsObject as IRayMirror, ref rays, ref physicsObjects);
                }
            }
        }
    }
}