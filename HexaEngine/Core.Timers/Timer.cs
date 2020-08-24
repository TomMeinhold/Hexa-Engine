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

        public Timer(TimeSpan delay, int cylces = 0)
        {
            this.delay = delay;
            this.timerWorker = new Task(TimerVoid);
            this.cylces = cylces;
        }

        ~Timer()
        {
            Dispose(disposing: false);
        }

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
                Thread.Sleep(delay);
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