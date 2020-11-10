using System;
using System.Threading;
using System.Threading.Tasks;

namespace HexaEngine.Core.Timers
{
    public class Timer : IDisposable
    {
        private TimeSpan delay;

        private readonly Task timerWorker;

        private bool disposedValue;

        private readonly int cylces;

        public bool stopping;

        private readonly Random random = new Random();

        public Timer(TimeSpan delay, int cylces = 0)
        {
            this.delay = delay;
            timerWorker = new Task(TimerVoid);
            this.cylces = cylces;
        }

        ~Timer()
        {
            Dispose(disposing: false);
        }

        public bool RandomTimeSpan { get; set; }

        public int RandomTimeMax { get; set; } = 1000;

        public int RandomTimeMin { get; set; } = 0;

        public int Cycle { get; private set; }

        public event EventHandler<TimerTickEventArgs> TimerTick;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            timerWorker.Start();
        }

        private void TimerVoid()
        {
            while (!stopping && (cylces > Cycle | cylces == 0))
            {
                if (RandomTimeSpan)
                {
                    Thread.Sleep(random.Next(RandomTimeMin, RandomTimeMax));
                }
                else
                {
                    Thread.Sleep(delay);
                }

                Cycle++;
                TimerTick?.Invoke(this, new TimerTickEventArgs(Cycle));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timerWorker.Dispose();
                }
                stopping = true;
                disposedValue = true;
            }
        }
    }
}