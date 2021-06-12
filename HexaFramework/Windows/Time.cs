namespace HexaFramework.Windows
{
    using System.Diagnostics;

    public class Time
    {
        // Variables
        private Stopwatch _StopWatch;

        private float m_ticksPerMs;
        private long m_LastFrameTime = 0;

        // Properties
        public float Delta { get; private set; }

        public float CumulativeFrameTime { get; private set; }

        // Public Methods
        internal bool Initialize()
        {
            // Check to see if this system supports high performance timers.
            if (!Stopwatch.IsHighResolution)
                return false;
            if (Stopwatch.Frequency == 0)
                return false;

            // Find out how many times the frequency counter ticks every millisecond.
            m_ticksPerMs = Stopwatch.Frequency / 1000.0f;

            _StopWatch = Stopwatch.StartNew();
            return true;
        }

        internal void FrameUpdate()
        {
            // Query the current time.
            long currentTime = _StopWatch.ElapsedTicks;

            // Calculate the difference in time since the last time we queried for the current time.
            float timeDifference = currentTime - m_LastFrameTime;

            // Calculate the frame time by the time difference over the timer speed resolution.
            Delta = timeDifference / m_ticksPerMs / 1000;
            CumulativeFrameTime += Delta;

            // record this Frames durations to the LastFrame for next frame processing.
            m_LastFrameTime = currentTime;
        }
    }
}