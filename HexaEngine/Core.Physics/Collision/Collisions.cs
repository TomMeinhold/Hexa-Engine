using HexaEngine.Core.Physics.Interfaces;
using SharpDX;

namespace HexaEngine.Core.Physics.Collision
{
    public static class Collisions
    {
        public static void Process(IPhysicsObject a, IPhysicsObject b)
        {
            BlockedDirection sides = new BlockedDirection(a, b);
            Vector3 pos = a.Position;
            a.Sides = sides;

            if (sides.Bottom)
            {
                if (!a.Static)
                {
                    pos.Y += sides.TopDepth;
                }
            }

            if (sides.Top)
            {
                if (!a.Static)
                {
                    pos.Y -= sides.BottomDepth;
                }
            }

            if (sides.Right)
            {
                if (!a.Static)
                {
                    pos.X += sides.RightDepth;
                }
            }

            if (sides.Left)
            {
                if (!a.Static)
                {
                    pos.X -= sides.LeftDepth;
                }
            }

            if (sides.CollisionDetected)
            {
                a.SetPosition(pos);

                if (b.Static)
                {
                    (a.Velocity, _) = Collide(a, b);
                }
                else
                {
                    (a.Velocity, b.Velocity) = Collide(a, b);
                }

                a.CallOnCollision(new OnCollisionEventArgs(b));
                b.CallOnCollision(new OnCollisionEventArgs(a));
            }
        }

        public static (Vector3, Vector3) Collide(IPhysicsObject a, IPhysicsObject b)
        {
            Vector3 v1 = ((a.Mass * a.Velocity) + (b.Mass * ((2 * b.Velocity) - a.Velocity))) / (a.Mass + b.Mass);
            Vector3 v2 = ((b.Mass * b.Velocity) + (a.Mass * ((2 * a.Velocity) - b.Velocity))) / (a.Mass + b.Mass);

            return (v1, v2);
        }

        public static bool Intersect(BoundingBox a, BoundingBox b)
        {
            return a.Minimum.X <= b.Maximum.X && a.Maximum.X >= b.Minimum.X && a.Minimum.Y <= b.Maximum.Y && a.Maximum.Y >= b.Minimum.Y && a.Minimum.Z <= b.Maximum.Z && a.Maximum.Z >= b.Minimum.Z;
        }

        public static bool TunnelIntersect(IPhysicsObject a, IPhysicsObject b)
        {
            return false;
        }
    }
}