using HexaEngine.Core.Extensions;
using HexaEngine.Core.Physics.Interfaces;
using HexaEngine.Core.Physics.Rays;
using SharpDX;
using System.Threading;

namespace HexaEngine.Core.Physics.Collision
{
    public class BlockedDirection
    {
        public bool Top { get; set; }

        public float TopDepth { get; set; }

        public bool Bottom { get; set; }

        public float BottomDepth { get; set; }

        public bool Right { get; set; }

        public float RightDepth { get; set; }

        public bool Left { get; set; }

        public float LeftDepth { get; set; }

        public bool CollisionDetected { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public Vector3 Penetration { get; set; }

        public BlockedDirection()
        {
        }

        public BlockedDirection(IPhysicsObject aObj, IPhysicsObject bObj)
        {
            Vector3 Adimentions = new Vector3(aObj.BoundingBox.Width.Half(), aObj.BoundingBox.Height.Half(), aObj.BoundingBox.Depth.Half());
            Vector3 Bdimentions = new Vector3(bObj.BoundingBox.Width.Half(), bObj.BoundingBox.Height.Half(), bObj.BoundingBox.Depth.Half());
            Vector3 distance = bObj.Position - aObj.Position;
            Penetration = distance + (Adimentions + Bdimentions);
            RectangleF a = aObj.BoundingBox.BoundingBoxToRect();
            RectangleF b = bObj.BoundingBox.BoundingBoxToRect();
            var intersection = RectangleF.Intersect(a, b);
            if (intersection.IsEmpty)
            {
                if (a.X + a.Width + 1 == b.X && a.Y + a.Height > b.Y && a.Y < b.Y)
                {
                    Right = true;
                }

                if (a.X - 1 == b.X + b.Width && a.Y + a.Height > b.Y && a.Y < b.Y)
                {
                    Left = true;
                }

                if (a.Y + a.Height + 1 == b.Y && a.X + a.Width > b.X && a.X < b.X)
                {
                    Bottom = true;
                }

                if (a.Y - 1 == b.Y + b.Height && a.X + a.Width > b.X && a.X < b.X)
                {
                    Top = true;
                }

                if (Top && Bottom)
                {
                    Top = false;
                    Bottom = false;
                }

                if (Right && Left)
                {
                    Right = false;
                    Left = false;
                }

                CollisionDetected = Right || Left || Top || Bottom;
                return;
            }

            Bottom = a.Top == intersection.Top;
            BottomDepth = intersection.Height;

            Top = a.Bottom == intersection.Bottom;
            TopDepth = intersection.Height;

            Left = a.Right == intersection.Right;
            LeftDepth = intersection.Width;

            Right = a.Left == intersection.Left;
            RightDepth = intersection.Width;

            if (intersection.Height < intersection.Width)
            {
                Right = false;
                Left = false;
            }
            else
            {
                Top = false;
                Bottom = false;
            }

            CollisionDetected = Right || Left || Top || Bottom;
            return;
        }
    }
}