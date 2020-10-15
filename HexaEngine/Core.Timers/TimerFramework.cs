using System.Collections.Generic;

namespace HexaEngine.Core.Timers
{
    public static class TimerFramework
    {
        public static List<Timer> Timers { get; } = new List<Timer>();

        public static void Dispose()
        {
            Timers.ForEach(x => x.Dispose());
        }
    }
}