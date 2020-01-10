using SharpDX;

namespace HexaEngine.Core.Physics
{
    public partial class PhysicsEngine
    {
        public bool[] Collisions(RectangleF hero, RectangleF rect)
        {
            var intersection = RectangleF.Intersect(hero, rect);
            if (intersection.IsEmpty)
            {
                return new[] { false, false, false, false };
            }

            bool hitSomethingAbove = hero.Top == intersection.Top;
            bool hitSomethingBelow = hero.Bottom == intersection.Bottom;


            bool hitSomethingOnTheRight = hero.Right == intersection.Right;
            bool hitSomethingOnTheLeft = hero.Left == intersection.Left;

            if (intersection.Height < 3)
            {
                hitSomethingOnTheRight = false;
                hitSomethingOnTheLeft = false;
            }

            return new bool[]
            {
                hitSomethingAbove,
                hitSomethingBelow,
                hitSomethingOnTheRight,
                hitSomethingOnTheLeft,
            };
        }
    }
}
