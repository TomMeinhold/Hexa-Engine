using System;
using System.Threading;

namespace HexaEngine.Core.Common
{
    public sealed class Timer
    {
        private bool Active;

        private readonly Thread TimerThread;

        public int Interval;

        public event EventHandler<TimerEventArgs> TimerTick;

        public Timer(int interval, bool active = false)
        {
            Interval = interval;
            TimerThread = new Thread(TimerWorker);
            TimerThread.Start();
            Active = active;
        }

        public void Stop()
        {
            Active = false;
        }

        public void Start()
        {
            Active = true;
        }

        public void Abort()
        {
            TimerThread.Abort();
        }

        public void Dispose()
        {
            TimerThread.Abort();
        }

        private void TimerWorker()
        {
            while (true)
            {
                while (Active)
                {
                    EventHandler<TimerEventArgs> handler = TimerTick;
                    TimerEventArgs eventArgs = new TimerEventArgs() { Alive = false };
                    handler?.Invoke(this, eventArgs);
                    Thread.Sleep(Interval);
                }
                Thread.Sleep(1);
            }
        }

        public class TimerEventArgs : EventArgs
        {
            public bool Alive { get; set; }
        }
    }
}
