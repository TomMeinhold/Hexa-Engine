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

        public float X { get; set; }

        public float Y { get; set; }
    }
}