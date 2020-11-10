using System;

namespace HexaEngine.Core.Timers
{
    public class TimerTickEventArgs : EventArgs
    {
        public TimerTickEventArgs(int cycle)
        {
            Cycle = cycle;
        }

        public int Cycle { get; }
    }
}