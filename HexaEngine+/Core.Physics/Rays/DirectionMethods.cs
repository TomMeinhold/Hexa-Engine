using SharpDX;

namespace HexaEngine.Core.Physics.Rays
{
    public static class DirectionMethods
    {
        public static Direction TraceDirection(Vector3 from, Vector3 after)
        {
            if (from.X < after.X)
            {
                // Left.
                if (from.Y > after.Y)
                {
                    // Top.
                    return Direction.TopLeft;
                }
                else if (from.Y < after.Y)
                {
                    // Bottom
                    return Direction.BottomLeft;
                }
                else
                {
                    // Middle Hori
                    return Direction.LeftCenter;
                }
            }
            else if (from.X > after.X)
            {
                // Right.
                if (from.Y > after.Y)
                {
                    // Top.
                    return Direction.TopRight;
                }
                else if (from.Y < after.Y)
                {
                    // Bottom
                    return Direction.BottomRight;
                }
                else
                {
                    // Middle Hori
                    return Direction.RightCenter;
                }
            }
            else
            {
                // Middle Vert
                if (from.Y > after.Y)
                {
                    // Top.
                    return Direction.TopCenter;
                }
                else if (from.Y < after.Y)
                {
                    // Bottom
                    return Direction.BottomCenter;
                }
                else
                {
                    // Middle Hori
                    return Direction.Center;
                }
            }
        }

        public static Direction4[] SplitDirection(this Direction direction)
        {
            Direction4[] direction4s = direction switch
            {
                Direction.RightCenter => new Direction4[] { Direction4.Right },
                Direction.LeftCenter => new Direction4[] { Direction4.Left },
                Direction.TopRight => new Direction4[] { Direction4.Up, Direction4.Right },
                Direction.TopLeft => new Direction4[] { Direction4.Up, Direction4.Left },
                Direction.TopCenter => new Direction4[] { Direction4.Up },
                Direction.BottomRight => new Direction4[] { Direction4.Down, Direction4.Right },
                Direction.BottomLeft => new Direction4[] { Direction4.Down, Direction4.Left },
                Direction.BottomCenter => new Direction4[] { Direction4.Down },
                Direction.Center => new Direction4[0],
                _ => new Direction4[0],
            };
            return direction4s;
        }
    }
}