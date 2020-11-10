using HexaEngine.Core.Common;

namespace HexaEngine.Core.Generation
{
    public struct NoiseRule
    {
        public NoiseRule(Range rangeX, Range rangeY, Range valueRange)
        {
            RangeX = rangeX;
            RangeY = rangeY;
            ValueRange = valueRange;
        }

        public Range RangeX { get; }

        public Range RangeY { get; }

        public Range ValueRange { get; }

        public void ApplyRule(int x, int y, ref int value)
        {
            if (RangeX.InRange(x) && RangeY.InRange(y))
            {
                value = ValueRange.FitToRange(value);
            }
        }
    }
}