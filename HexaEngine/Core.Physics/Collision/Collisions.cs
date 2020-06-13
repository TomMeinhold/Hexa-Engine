using HexaEngine.Core.Extentions;
using HexaEngine.Core.Physics.Interfaces;
using SharpDX;

namespace HexaEngine.Core.Physics.Collision
{
    public static class Collisions
    {
        public static void Process(IPhysicsObject a, IPhysicsObject b)
        {
            BoundingBox rectangle1 = a.BoundingBox;
            BoundingBox rectangle2 = b.BoundingBox;

            BlockedDirection sides = Collision(rectangle1, rectangle2);
            a.Sides = sides;

            if (sides.Bottom)
            {
                if (!a.Static)
                {
                    Vector3 pos = a.BoundingBox.Minimum;
                    pos.Y += sides.TopDepth;
                    a.SetPosition(pos);
                    (a.Velocity, b.Velocity) = Collide(a, b);
                    if (!b.Static)
                    {
                        Vector3 vector = b.Force;
                        vector.Y += a.Force.Y;
                        b.Force = vector;
                    }
                }
            }

            if (sides.Top)
            {
                if (!a.Static)
                {
                    Vector3 pos = a.BoundingBox.Minimum;
                    pos.Y -= sides.BottomDepth;
                    a.SetPosition(pos);
                    (a.Velocity, b.Velocity) = Collide(a, b);
                    if (!b.Static)
                    {
                        Vector3 vector = b.Force;
                        vector.Y += a.Force.Y;
                        b.Force = vector;
                    }
                }
            }

            if (sides.Right)
            {
                if (!a.Static)
                {
                    Vector3 pos = a.BoundingBox.Minimum;
                    pos.X += sides.RightDepth;
                    a.SetPosition(pos);
                    (a.Velocity, b.Velocity) = Collide(a, b);
                    if (!b.Static)
                    {
                        Vector3 vector = b.Force;
                        vector.X += a.Force.X;
                        b.Force = vector;
                    }
                }
            }

            if (sides.Left)
            {
                if (!a.Static)
                {
                    Vector3 pos = a.BoundingBox.Minimum;
                    pos.X -= sides.LeftDepth;
                    a.SetPosition(pos);
                    (a.Velocity, b.Velocity) = Collide(a, b);
                    if (!b.Static)
                    {
                        Vector3 vector = b.Force;
                        vector.X += a.Force.X;
                        b.Force = vector;
                    }
                }
            }
        }

        public static BlockedDirection Collision(BoundingBox ab, BoundingBox bb)
        {
            RectangleF a = ab.BoundingBoxToRect();
            RectangleF b = bb.BoundingBoxToRect();
            var output = new BlockedDirection();
            var intersection = RectangleF.Intersect(a, b);
            if (intersection.IsEmpty)
            {
                if ((int)a.X + a.Width + 1 == (int)b.X && (a.Y + a.Height > b.Y && a.Y < b.Y))
                {
                    output.Right = true;
                }

                if ((int)a.X - 1 == (int)b.X + b.Width && (a.Y + a.Height > b.Y && a.Y < b.Y))
                {
                    output.Left = true;
                }

                if ((int)a.Y + a.Height + 1 == (int)b.Y && (a.X + a.Width > b.X && a.X < b.X))
                {
                    output.Bottom = true;
                }

                if ((int)a.Y - 1 == (int)b.Y + b.Height && (a.X + a.Width > b.X && a.X < b.X))
                {
                    output.Top = true;
                }

                return output;
            }

            output.Bottom = a.Top == intersection.Top;
            output.BottomDepth = intersection.Height;

            output.Top = a.Bottom == intersection.Bottom;
            output.TopDepth = intersection.Height;

            output.Left = a.Right == intersection.Right;
            output.LeftDepth = intersection.Width;

            output.Right = a.Left == intersection.Left;
            output.RightDepth = intersection.Width;

            if (intersection.Height < 3)
            {
                output.Right = false;
                output.Left = false;
            }

            if (intersection.Height < intersection.Width)
            {
                output.Right = false;
                output.Left = false;
            }
            else
            {
                output.Top = false;
                output.Bottom = false;
            }

            return output;
        }

        public static (Vector3, Vector3) Collide(IPhysicsObject a, IPhysicsObject b)
        {
            Vector3 v1 = (a.Mass - b.Mass) / (a.Mass + b.Mass) * a.Velocity + 2 * b.Mass / (a.Mass + b.Mass) * b.Velocity;
            Vector3 v2 = 2 * a.Mass / (a.Mass + b.Mass) * a.Velocity + (b.Mass - a.Mass) / (a.Mass + b.Mass) * b.Velocity;
            return (v1, v2);
        }
    }
}