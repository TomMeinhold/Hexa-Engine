using SharpDX;
using System;

namespace HexaEngine.Core.Physics.Rays
{
    public static class RayMath
    {
        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < 1e-10;
        }

        public static float Cross(this Vector3 v1, Vector3 v2) => v1.X * v2.Y - v1.Y * v2.X;

        public static (bool, Vector3) LineSegementsIntersect(Ray ray, Vector3 vector1, Vector3 vector2)
        {
            return (LineSegementsIntersect(ray.Position, ray.Direction, vector1, vector2, out Vector3 vector), vector);
        }

        public static bool LineSegementsIntersect(Vector3 p, Vector3 p2, Vector3 q, Vector3 q2, out Vector3 intersection)
        {
            intersection = new Vector3();

            var r = p2 - p;
            var s = q2 - q;

            float rxs = r.Cross(s);
            float qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
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

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1 the two line segments meet at the point
            // p + t r = q + u s.
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
    }
}