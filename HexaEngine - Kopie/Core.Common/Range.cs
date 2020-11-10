namespace HexaEngine.Core.Common
{
    public struct Range
    {
        public int From;

        public int To;

        public Range(int from, int to)
        {
            From = from;
            To = to;
        }

        public bool InRange(int value)
        {
            return From <= value && value <= To;
        }

        public int FitToRange(int value)
        {
            if (InRange(value))
            {
                return value;
            }
            else
            {
                if (From > value)
                {
                    return From;
                }
                else if (value > To)
                {
                    return To;
                }
            }

            return 0;
        }
    }
}