namespace HexaEngine.Core.UI.Structs
{
    public struct Thickness
    {
        public float Top;
        public float Right;
        public float Left;
        public float Bottom;

        public Thickness(float top, float right, float left, float bottom)
        {
            Top = top;
            Right = right;
            Left = left;
            Bottom = bottom;
        }

        public static Thickness Zero = default;

        public static Thickness NaN = new Thickness(float.NaN, float.NaN, float.NaN, float.NaN);

        public Thickness Add(Thickness thickness)
        {
            return new Thickness(Top + thickness.Top, Right + thickness.Right, Left + thickness.Left, Bottom + thickness.Bottom);
        }

        public static Thickness operator +(Thickness first, Thickness second)
        {
            return first.Add(second);
        }
    }
}